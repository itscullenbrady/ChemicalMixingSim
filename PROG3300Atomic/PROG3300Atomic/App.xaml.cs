using System.Configuration;
using System.Data;
using System.Windows;

namespace PROG3300Atomic
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainMenu mainMenu = new MainMenu();
            mainMenu.Show();
        }


    }

}
