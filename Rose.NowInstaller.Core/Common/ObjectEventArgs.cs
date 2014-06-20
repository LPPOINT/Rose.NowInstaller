using System;

namespace Rose.NowInstaller.Core.Common
{
    public class ObjectEventArgs<T> : EventArgs
    {
        public ObjectEventArgs(T o)
        {
            Object = o;
        }

        public T Object { get; private set; }
    }
}
