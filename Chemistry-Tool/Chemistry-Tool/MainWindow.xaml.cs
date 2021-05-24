using System;
using System.Windows;
using System.Windows.Controls;

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
        /// Instance of app to be loaded
        /// </summary>
        private static Window opener;

        /// <summary>
        /// Called when an app button is clicked
        /// </summary>
        public void AppLoader(object sender, RoutedEventArgs e)
        {
            AppData.Visibility = Visibility.Visible;                //Sets AppData control to visible
            AppName.Text = ((Button)sender).Content.ToString();     //Sets the title of the AppData control to be the app name

            switch (AppName.Text)    //Load a certain app window into opener, determined by casing the appname
            {
                case "Reversible Reaction Tool":
                    Description.Text =
"Tool used for finding equilibrium constants in a given reaction.";
                    break;

                case "Equation Balancer":
                    Description.Text =
"Tool used for balancing a given chemical equation.";
                    break;

                case "Chemical Lookup":
                    Description.Text =
@"Searches the PubChem database for the specified chemical.
Warning: This tool is really unstable from developmental reasons, tread carefully.";
                    break;

                default:    //Shouldn't run, but here if you need it
                    throw new NotImplementedException($"Specified tool of name \"{AppName.Text}\" has not yet been implemented.");
            }
        }

        /// <summary>
        /// Called when launch is clicked
        /// </summary>
        public void LaunchApp(object sender, RoutedEventArgs e)
        {
            switch (AppName.Text)    //Load a certain app window into opener, determined by casing the appname
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
        /// Opens website
        /// </summary>
        public void EH_GoToWebsite(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.proleindustries.com");
        }
    }
}