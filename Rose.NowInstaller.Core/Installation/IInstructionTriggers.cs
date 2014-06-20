using JetBrains.Annotations;
using Rose.NowInstaller.Core.Instructions;

namespace Rose.NowInstaller.Core.Installation
{
    public interface  IInstructionTriggers
    {
        InstructionTrigger InstallStatusChanged { get; }
        InstructionTrigger StartDownload { get; }
        InstructionTrigger PauseDownload { get; }

        InstructionTrigger DownloadProcessChanged(int percentDownloaded);

    }

    public class InstructionTriggers : IInstructionTriggers
    {
        public InstructionTrigger InstallStatusChanged { get; private set; }
        public InstructionTrigger StartDownload { get; private set; }
        public InstructionTrigger PauseDownload { get; private set; }
        public InstructionTrigger DownloadProcessChanged(int percentDownloaded)
        {
            throw new System.NotImplementedException();
        }
    }
}
