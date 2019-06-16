using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp20
{
    public class EqualityPropertyAttribute : Attribute
    {
        //Делаем атрибут, который будет указывать на свойства, которые будут использоваться для автосравнения
        //Можно сделать не только свойства, но так же открытые поля, помечать методы и вообще всё,
        //что нам необходимо
        public EqualityPropertyAttribute() { }
        
    }
}
