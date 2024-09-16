using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsModelGeneratorLib.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class TypeScriptIgnoreAttribute : Attribute
    { 
        public TypeScriptIgnoreAttribute()
        {
        }
    }
}
