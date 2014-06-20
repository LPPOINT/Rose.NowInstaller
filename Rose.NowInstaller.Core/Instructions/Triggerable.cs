namespace Rose.NowInstaller.Core.Instructions
{


    public class Triggerable : ITriggerable
    {

        public Triggerable()
        {
            
        }

        public Triggerable(InstructionTrigger trigger, TriggerableDelegate executor)
        {
            Trigger = trigger;
            Executor = executor;
        }

        public InstructionTrigger Trigger { get; set; }

        public delegate void TriggerableDelegate(IInstructionExecutionContext context);

        public TriggerableDelegate Executor { get; set; }


    }
}
