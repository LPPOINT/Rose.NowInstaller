using Rose.NowInstaller.Core.UIInteraction;

namespace Rose.NowInstaller.Core.EnvironmentInteraction
{
    public class InstallUI
    {
        public InstallUI(IStatusBarController statusBar, IStatusController status, ILaunchButtonController launchButton)
        {
            StatusBar = statusBar;
            Status = status;
            LaunchButton = launchButton;
        }

        public IStatusBarController StatusBar { get; private set; }
        public IStatusController Status { get; private set; }
        public ILaunchButtonController LaunchButton { get; private set; }
    }
}
