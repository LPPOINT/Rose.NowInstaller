using MonoTorrent.Client;
using Rose.NowInstaller.Core.EnvironmentInteraction;
using Rose.NowInstaller.Core.TorrentIntegration;

namespace Rose.NowInstaller.Core.Installation
{
    public class InstallManagerSettings
    {

        public InstallManagerSettings()
        {
            
        }

        public InstallManagerSettings(InstallScript installScript, InstallTorrent installTorrent, ClientEngine torrentEngine, InstallUI installUi)
        {
            InstallScript = installScript;
            InstallTorrent = installTorrent;
            TorrentEngine = torrentEngine;
            InstallUI = installUi;
        }

        public InstallScript InstallScript { get; set; }
        public InstallTorrent InstallTorrent { get; set; }
        public ClientEngine TorrentEngine { get; set; }
        public InstallUI InstallUI { get; set; }
    }
}
