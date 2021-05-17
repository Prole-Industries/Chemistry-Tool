using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chemistry_Tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Put any test logic here
        }

        /// <summary>
        /// Called when an app button is clicked
        /// </summary>
        public void AppLoader(object sender, RoutedEventArgs e)
        {
            AppData.Visibility = Visibility.Visible;                //Sets AppData control to visible
            AppName.Text = ((Button)sender).Content.ToString();     //Sets the title of the AppData control to be the app name
        }

        /// <summary>
        /// Called when launch is clicked
        /// </summary>
        public void LaunchApp(object sender, RoutedEventArgs e)
        {
            Window opener;          //Create a generic window to be loaded with the specific app
            switch(AppName.Text)    //Load a certain app window into opener, determined by casing the appname
            {
                case "Reversible Reaction Tool":
                    opener = new ReversibleReactionTool();
                    break;

                case "Equation Balancer":
                    opener = new EquationBalancer();
                    break;

                case "Chemical Lookup":
                    opener = new Lookup();
                    break;

                default:    //Shouldn't run, but here if you need it
                    throw new NotImplementedException($"Specified tool of name \"{AppName.Text}\" has not yet been implemented.");
            }

            opener.Show();  //Reveal window loaded with specified app
        }

        /// <summary>
        /// Reveals about window
        /// </summary>
        public void EH_About(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }

        /// <summary>
        /// Reveals settings window
        /// </summary>
        public void EH_OpenSettings(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Opens website
        /// </summary>
        public void EH_GoToWebsite(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.proleindustries.com");
        }
    }
}
