using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ConsoleApp20
{
    //То, ради чего всё затевалось. Он реализует необходимый интерфейс
    public class AutoEquality<T> : IEqualityComparer<T>
    {
        //Перепреоделяемые делегаты, которые требуются в IEquality
        private readonly Func<T, T, bool> equals;
        private readonly Func<T, int> getHashCode;

        //Можно ввести энум, но пусть пока так
        private const int LeftExpr = 0;
        private const int RightExpr = 1;

        //Закрытый конструктор
        private AutoEquality(Func<T, T, bool> equals, Func<T, int> getHashCode)
        {
            this.equals = equals;
            this.getHashCode = getHashCode;
        }

        //Статичный конструктор
        public static IEqualityComparer<T> Create()
        {
            //Получаем список свойств, которые помечены EqualityProperty
            var equalityProperties =
                typeof(T)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(x => x.GetCustomAttribute<EqualityPropertyAttribute>() != null);

            //Лист таплов
            var expressions = new List<(Expression EqualsExpr, Expression GetHashCodeExpr)>();
            var leftExpression = Expression.Parameter(typeof(T), "left");
            var rightExpression = Expression.Parameter(typeof(T), "right");
            //Проходим по всему списку свойств
            //TODO: слишком большой блок, надо как-то его уменьшить
            foreach (var equalityProperty in equalityProperties)
            {
                //Вытаскиваем значение конкретного свойства
                var equalityPropertyAttribute = equalityProperty.GetCustomAttribute<EqualityPropertyAttribute>();
                //Вытаскиваем имя типа этого свойства
                var propertyType = equalityProperty.PropertyType;
                //Получаем левое и правое значение выражений
                var leftPropertyExpression = (Expression)Expression.Property(leftExpression, equalityProperty);
                var rightPropertyExpression = (Expression)Expression.Property(rightExpression, equalityProperty);
                //Параметры типов для equals
                var equalsMethodParameterTypes = new List<Type> { propertyType };
                //Вытаскиваем из класса свойства метод сравнения (Equals), по которому будем вести сравнение
                var equalsMethod = propertyType.GetMethod(nameof(object.Equals), equalsMethodParameterTypes.ToArray());
                //Получаем лист с параметрами сравнения
                var equalsMethodParameters = new List<Expression> { rightPropertyExpression };
                //Получаем expression для метода equals текущего свойства
                var equalsExpression = Expression.Call(leftPropertyExpression, equalsMethod, equalsMethodParameters);
                //Вытаскиваем метод получения хеш-кода
                var getHashCodeMethod = propertyType.GetMethod(nameof(object.GetHashCode));
                //Получаем expression для метода getHashCode текущего свойства
                var getHashCodeExpression = Expression.Call(leftPropertyExpression, getHashCodeMethod);
                //Добавляем полученные expressions к нашему листу 
                expressions.Add((equalsExpression, getHashCodeExpression));
                if (composeExpressions(expressions, out var t))
                {
                    expressions.Clear();
                    expressions.Add(t);
                }

            }
            //Получаем элемент последовательности
            var result = expressions.Single();
            //Задаём лямбда-выражение для equals
            var equalsFunc = Expression.Lambda<Func<T, T, bool>>(result.EqualsExpr, leftExpression, rightExpression).Compile();
            //Задаём лямбда-выражение для getHashCode
            var getHashCodeFunc = Expression.Lambda<Func<T, int>>(result.GetHashCodeExpr, leftExpression).Compile();
            //Возвращаем сгенерированные функции
            return new AutoEquality<T>(equalsFunc, getHashCodeFunc);
        }

        //Здесь мы "склеиваем" equals и hashCode всех свойств
        private static bool composeExpressions(IList<(Expression EqualsExpr, Expression GetHashCodeExpr)> exprs, out (Expression EqualsExpr, Expression GetHashCodeExpr) result)
        {
            //Составляем выражение, если их два
            if (exprs.Count == 2)
            {
                //Получаем бинарный оператор
                var andAlsoExpr = Expression.AndAlso(exprs[LeftExpr].EqualsExpr, exprs[RightExpr].EqualsExpr);
                // (left.GetHashCode() * 31) + right.GetHashCode()
                var addExpr =
                    Expression.Add(
                        Expression.Multiply(
                            exprs[LeftExpr].GetHashCodeExpr,
                            Expression.Constant(31)
                        ),
                        exprs[RightExpr].GetHashCodeExpr
                    );
                result = (andAlsoExpr, addExpr);
                return true;
            }
            //Если по дороге что-то сломалось
            result = (default(Expression), default(Expression));
            return false;
        }

        //Считаем значение метода Equals
        public bool Equals(T left, T right)
        {
            //Если явно обозначен метод equals - считаем просто по ссылке
            if (ReferenceEquals(left, right)) return true;
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null)) return true;
            if (ReferenceEquals(left, null)) return false;
            if (ReferenceEquals(right, null)) return false;
            //Иначе возвращаем то, что получилось в результате просмотра всех свойств
            return equals(left, right);
        }

        //Вохвращаем hashCode
        public int GetHashCode(T obj)
        {
            return getHashCode(obj);
        }
    }
}
