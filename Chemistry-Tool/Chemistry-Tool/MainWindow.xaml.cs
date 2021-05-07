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
#pragma warning disable IDE0059
            //            Chemical chem1 = new Chemical("C");
            //            Chemical chem2 = new Chemical("O2");
            //            Chemical chem3 = new Chemical("CO2");
            //            Chemical chem4 = new Chemical("Al2Cl3");
            //            Chemical chem5 = new Chemical("OS");
            //            Chemical chem6 = new Chemical("Os");
            //            Chemical chem7 = new Chemical("HCOOH");
            //            Chemical chem8 = new Chemical("Ca(NO3)2");

            //Simple esterification test case
            Chemical r1 = new Chemical("CH3COOH");
            Chemical r2 = new Chemical("CH3CH2OH");
            Chemical p1 = new Chemical("CH3COOCH2CH3");
            Chemical p2 = new Chemical("H2O");
#pragma warning restore IDE0059
        }

        public void AppLoader(object sender, RoutedEventArgs e)
        {
            AppData.Visibility = Visibility.Visible;
            AppName.Text = ((Button)sender).Content.ToString();
        }

        public void LaunchApp(object sender, RoutedEventArgs e)
        {
            Window opener;
            switch(AppName.Text)
            {
                case "Reversible Reaction Tool":
                    opener = new ReversibleReactionTool();
                    break;

                case "Equation Balancer":
                    opener = new EquationBalancer();
                    break;

                default:
                    throw new NotImplementedException($"Specified tool of name \"{AppName.Text}\" has not yet been implemented.");
            }

            opener.Show();
        }

        public void EH_About(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }

        public void EH_OpenSettings(object sender, RoutedEventArgs e)
        {

        }

        public void EH_GoToWebsite(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.proleindustries.com");
        }
    }
}
