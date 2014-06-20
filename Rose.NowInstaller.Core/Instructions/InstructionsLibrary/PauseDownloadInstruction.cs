namespace Rose.NowInstaller.Core.Instructions.InstructionsLibrary
{
    [Instruction(InstructionsMapping.PauseDownload)]
    public class PauseDownloadInstruction : Instruction
    {
        protected PauseDownloadInstruction(int id, InstructionData data)
            : base(id, data)
        {
        }

        public PauseDownloadInstruction(InstructionData data)
            : base(InstructionsMapping.PauseDownload, data)
        {
        }
    }
}
