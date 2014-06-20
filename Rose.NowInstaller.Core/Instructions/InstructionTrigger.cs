using System;

namespace Rose.NowInstaller.Core.Instructions
{

    public class InstructionTriggerInvokedEventArgs : EventArgs
    {
        
    }

    public class InstructionTrigger
    {

        public InstructionTrigger()
        {
            Guid = Guid.NewGuid();
        }

        public event EventHandler<InstructionTriggerInvokedEventArgs> Invoked;

        private Guid Guid { get;  set; }

        public virtual void Invoke(InstructionTriggerInvokedEventArgs e)
        {
            var handler = Invoked;
            if (handler != null) handler(this, e);
        }

        public void Invoke()
        {
            Invoke(new InstructionTriggerInvokedEventArgs());
        }

        protected bool Equals(InstructionTrigger other)
        {
            return Guid.Equals(other.Guid);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((InstructionTrigger) obj);
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }
    }
}
