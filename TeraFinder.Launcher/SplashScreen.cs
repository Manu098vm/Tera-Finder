
namespace TeraFinder.Launcher
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        public void ShowRefresh()
        {
            Show();
            Refresh();
        }
    }
}
