using System;
using System.Collections.Generic;
using System.Text;

namespace TsModelGeneratorLib.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class GenerateTypeScriptAttribute : Attribute
    {
        public GenerateTypeScriptAttribute()
        {
        }
    }
}
