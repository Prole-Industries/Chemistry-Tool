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

            //Chemical[] chems = new Chemical[]
            //{
            //    new Chemical("C"),
            //    new Chemical("CO"),
            //    new Chemical("O2"),
            //    new Chemical("CO2"),
            //    new Chemical("Al2Cl3")
            //};

            //foreach (Chemical chem in chems)
            //{
            //    chem.ResolveElements();
            //    Console.WriteLine();
            //}

            Console.WriteLine(Atom.GetBySymbol("C").Name);
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
