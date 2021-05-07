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
        private double Kc;

        public ReversibleReactionTool()
        {
            InitializeComponent();
        }

        public void AddReactant(object sender, RoutedEventArgs e)
        {
            Reactants.Children.Add(App.GenericChemical());
        }

        public void RemoveReactant(object sender, RoutedEventArgs e)
        {
            Reactants.Children.RemoveAt(Reactants.Children.Count - 1);
        }

        public void AddProduct(object sender, RoutedEventArgs e)
        {
            Products.Children.Add(App.GenericChemical());
        }

        public void RemoveProduct(object sender, RoutedEventArgs e)
        {
            Products.Children.RemoveAt(Products.Children.Count - 1);
        }

        public void Equilibrium(object sender, RoutedEventArgs e)
        {
            double KcReactants = 1;
            double KcProducts = 1;

            if(Products.Children.Count * Reactants.Children.Count == 0 || Products.Children.Count * Reactants.Children.Count == 1)
            {
                App.Alert("A total of at least 3 Products and Reactants must be specified");
                return;
            }

            foreach(Substance reactant in Reactants.Children.OfType<StackPanel>().Select(t => new Substance(t)))
            {
                KcReactants *= Math.Pow(reactant.Concentration, reactant.Moles);
            }

            foreach (Substance product in Products.Children.OfType<StackPanel>().Select(t => new Substance(t)))
            {
                KcProducts *= Math.Pow(product.Concentration, product.Moles);
            }

            Kc = KcProducts / KcReactants;
            EqConst.Text = $"Kc: {Kc}";

            EqData.Visibility = Visibility.Visible;
        }
    }
}
