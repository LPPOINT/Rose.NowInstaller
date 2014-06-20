namespace Rose.NowInstaller.Core.UIInteraction
{
    public interface ILaunchButtonController
    {
        string ButtonText { get; set; }
        bool IsActive { get; set; }
    }
}
