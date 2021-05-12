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
using System.Windows.Shapes;

namespace Chemistry_Tool
{
    /// <summary>
    /// Interaction logic for Lookup.xaml
    /// </summary>
    public partial class Lookup : Window
    {
        public Lookup()
        {
            InitializeComponent();
        }

        public void SearchForChemical(object sender, RoutedEventArgs e)
        {
            Information.Visibility = Visibility.Hidden;
            SearchButton.Content = "Working...";

            Chemical.Metadata metadata = Chemical.GetData(SearchTerm.Text);

            Name.Text = $"{metadata.Name}";
            Structure.Text = $"{metadata.Structure}";
            InChI_Identifier.Text = $"InChI Identifier: {metadata.InChI}";

            for(int x = 0; x < 3; x++)
            {
                try
                {
                    string desc = metadata.Descriptions[x];
                    Descriptions.Children.Add(new TextBlock() { Style = (Style)Application.Current.FindResource("GenericTextBox"), Text = desc });
                    Descriptions.Children.Add(new Separator());
                }
                catch(ArgumentOutOfRangeException)
                {
                    break;
                }
            }
            Descriptions.Children.RemoveAt(Descriptions.Children.Count - 1);

            MeltingPoint.Text = $"Melting Point: {metadata.MeltingPoint}";
            BoilingPoint.Text = $"Boiling Point: {metadata.BoilingPoint}";

            SearchButton.Content = "Search";
            Information.Visibility = Visibility.Visible;
        }
    }
}
