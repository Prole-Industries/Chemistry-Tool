using System.Windows;
using System.Windows.Navigation;

namespace Chemistry_Tool
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    ///
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
        }

        public void EH_OpenSourceCode(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);    //Open GitHub repos
        }
    }
}