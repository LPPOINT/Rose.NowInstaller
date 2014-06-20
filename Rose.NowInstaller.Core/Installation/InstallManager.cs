using System.Runtime.InteropServices;
using MonoTorrent.Client;
using Rose.NowInstaller.Core.Instructions;
using Rose.NowInstaller.Core.TorrentIntegration;

namespace Rose.NowInstaller.Core.Installation
{

    [ComVisible(true)]
    public class InstallManager
    {
        public InstallManager(InstallManagerSettings settings)
        {
            InstallScript = settings.InstallScript;
            InstallTorrent = settings.InstallTorrent;
            instructionExecutionContext = new InstructionExecutionContext(settings.InstallTorrent);
            instructionsExecutor = new InstructionsExecutor(settings.InstallScript.CreateInstructions(new InstructionTriggers()));
            torrentClientEngine = settings.TorrentEngine;
            torrentManager = new TorrentManager(settings.InstallTorrent.Torrent, "", new TorrentSettings());
        }

        public InstallScript InstallScript { get; private set; }
        public InstallTorrent InstallTorrent { get; private set; }

        private InstructionsExecutor instructionsExecutor;
        private IInstructionExecutionContext instructionExecutionContext;

        private ClientEngine torrentClientEngine;
        private TorrentManager torrentManager;


        public void Start()
        {
            
        }

        public void Pause()
        {
            
        }

    }
}
