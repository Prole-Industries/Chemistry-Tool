using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for ReversibleReactionTool.xaml
    /// </summary>
    public partial class ReversibleReactionTool : Window
    {
        public ReversibleReactionTool()
        {
            InitializeComponent();
        }

        public void AddReactant(object sender, RoutedEventArgs e)
        {
            Reactants.Children.Add(GenericChemical());
        }

        public void AddProduct(object sender, RoutedEventArgs e)
        {
            Products.Children.Add(GenericChemical());
        }

        private UIElement GenericChemical()
        {
            StackPanel g = new StackPanel
            {
                Background = new SolidColorBrush(Color.FromArgb(0x44, 0x99, 0x99, 0x99)),
                Margin = new Thickness(0, 5, 0, 5)
            };

            TextBox name = new TextBox
            {
                ToolTip = "Name of Chemical"
            };

            TextBox conc = new TextBox
            {
                ToolTip = "Concentration of Chemical (mol dm⁻³)",
            };
            conc.PreviewTextInput += new TextCompositionEventHandler(IsConcValid);

            g.Children.Add(name);
            g.Children.Add(conc);
            return g;
        }

        private void IsConcValid(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^(\d+)((.)?(\d+))?$", RegexOptions.Multiline);
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
