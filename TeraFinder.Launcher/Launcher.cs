namespace TeraFinder.Launcher;

internal static class Launcher
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var splash = new SplashScreen();
        splash.ShowRefresh();
        var mainForm = new TeraFinderForm();

        mainForm.Shown += (sender, e) => {
            splash.Invoke(splash.Close);
            mainForm.Activate();
            mainForm.BringToFront();
        };

        Application.Run(mainForm);
    }
}