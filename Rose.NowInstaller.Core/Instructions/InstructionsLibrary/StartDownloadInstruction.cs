namespace Rose.NowInstaller.Core.Instructions.InstructionsLibrary
{
    [Instruction(InstructionsMapping.StartDownload)]
    public class StartDownloadInstruction : Instruction
    {
        protected StartDownloadInstruction(int id, InstructionData data) : base(id, data)
        {
        }

        public StartDownloadInstruction(InstructionData data)
            : base(InstructionsMapping.StartDownload, data)
        {
        }
    }
}
