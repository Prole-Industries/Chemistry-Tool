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

        public void RemoveReactant(object sender, RoutedEventArgs e)
        {
            Reactants.Children.RemoveAt(Reactants.Children.Count - 1);
        }

        public void AddProduct(object sender, RoutedEventArgs e)
        {
            Products.Children.Add(GenericChemical());
        }

        public void RemoveProduct(object sender, RoutedEventArgs e)
        {
            Products.Children.RemoveAt(Products.Children.Count - 1);
        }

        private UIElement GenericChemical()
        {
            StackPanel g = new StackPanel
            {
                Background = new SolidColorBrush(Color.FromArgb(0x44, 0x99, 0x99, 0x99)),
                Margin = new Thickness(0, 5, 0, 5)
            };

            g.Children.Add(new TextBlock() { Style = (Style)FindResource("GenericTextBox"), Text = "Chemical Name:"});
            g.Children.Add(new TextBox() { ToolTip = "Name of Chemical" });
            g.Children.Add(new TextBlock() { Style = (Style)FindResource("GenericTextBox"), Text = "Concentration:" });
            g.Children.Add(new TextBox() { ToolTip = "Concentration of Chemical (mol dm⁻³)" });
            g.Children.Add(new TextBlock() { Style = (Style)FindResource("GenericTextBox"), Text = "Moles:" });
            g.Children.Add(new TextBox() { ToolTip = "Moles of Chemical" });
            return g;
        }

        public void Equilibrium(object sender, RoutedEventArgs e)
        {
            double KcReactants = 1;
            double KcProducts = 1;
            double Kc;

            foreach(StackPanel productchild in Products.Children.OfType<StackPanel>())
            {
                IsChemicalValid(productchild);
                KcProducts *= Math.Pow(float.Parse(((TextBox)productchild.Children[3]).Text), float.Parse(((TextBox)productchild.Children[5]).Text));
            }

            foreach (StackPanel reactantchild in Reactants.Children.OfType<StackPanel>())
            {
                IsChemicalValid(reactantchild);
                KcReactants *= Math.Pow(float.Parse(((TextBox)reactantchild.Children[3]).Text), float.Parse(((TextBox)reactantchild.Children[5]).Text));
            }

            Kc = KcReactants / KcProducts;

            Console.WriteLine(Kc);
        }

        private void IsChemicalValid(StackPanel target)
        {
            if (((TextBox)target.Children[1]).Text == "") MessageBox.Show("One or more chemical names are missing", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            if (!IsNumValid((TextBox)target.Children[3])) MessageBox.Show("One or more chemical concentrations are invalid", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            if (!IsNumValid((TextBox)target.Children[5])) MessageBox.Show("One or more molar values are invalid", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool IsNumValid(TextBox target)
        {
            Regex regex = new Regex(@"^(\d+)((.)?(\d+))?$", RegexOptions.Multiline);
            return regex.IsMatch(target.Text);
        }
    }
}
