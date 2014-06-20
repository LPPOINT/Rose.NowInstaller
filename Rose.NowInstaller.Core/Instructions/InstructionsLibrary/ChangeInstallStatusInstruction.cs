namespace Rose.NowInstaller.Core.Instructions.InstructionsLibrary
{
    [Instruction(InstructionsMapping.ChangeInstallStatus)]
    public class ChangeInstallStatusInstruction : Instruction
    {

        [InstructionProperty]
        public string NewStatus { get; set; }

        protected ChangeInstallStatusInstruction(int id, InstructionData data) : base(id, data)
        {
        }

        public ChangeInstallStatusInstruction(InstructionData data)
            : base(InstructionsMapping.ChangeInstallStatus, data)
        {
        }
    }
}
