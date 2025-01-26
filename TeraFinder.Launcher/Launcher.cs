namespace TeraFinder.Launcher;

internal static class Launcher
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        var splash = new SplashScreen();
        new Task(() => splash.ShowDialog()).Start();
        var TF = new TeraFinderForm();
        splash.Invoke(splash.Close);
        TF.Load += (sender, e) => TF.Activate();
        Application.Run(TF);
    }
}