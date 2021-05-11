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
            Chemical.Metadata metadata = Chemical.GetData(SearchTerm.Text);

            Name.Text = $"{metadata.Name}";
            Structure.Text = $"{metadata.Structure}";
            InChI_Identifier.Text = $"InChI Identifier: {metadata.InChI}";

            Description.Text = $"{/*string.Join("\n", metadata.Descriptions.ToArray())*/metadata.Descriptions[0]}";

            MeltingPoint.Text = $"Melting Point: {metadata.MeltingPoint}";
            BoilingPoint.Text = $"Boiling Point: {metadata.BoilingPoint}";
        }
    }
}
