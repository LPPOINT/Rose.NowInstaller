namespace Rose.NowInstaller.Core.Instructions
{
    public interface ITriggerable
    {
        InstructionTrigger Trigger { get; set; }
    }
}
