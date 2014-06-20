using System;

namespace Rose.NowInstaller.Core.Instructions
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class InstructionPropertyAttribute : Attribute
    {
        public InstructionPropertyAttribute(string key)
        {
            Key = key;
        }

        public InstructionPropertyAttribute()
        {
            
        }

        public bool IsUnnamed
        {
            get { return string.IsNullOrWhiteSpace(Key); }
        }

        public string Key { get; private set; }
    }
}