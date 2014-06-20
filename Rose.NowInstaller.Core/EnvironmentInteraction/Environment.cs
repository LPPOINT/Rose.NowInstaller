namespace Rose.NowInstaller.Core.EnvironmentInteraction
{
    public interface IEnvironment
    {
        string RootFolder { get; }

        string GetFolderForProgram(string programName);

        void CreatePopup(string title, string content);

    }
}
