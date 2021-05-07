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
    /// Interaction logic for EquationBalancer.xaml
    /// </summary>
    public partial class EquationBalancer : Window
    {
        public EquationBalancer()
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

        public void Balance(object sender, RoutedEventArgs e)
        {
            Dictionary<Atom, int> ReactantAtoms = new Dictionary<Atom, int>();
            Dictionary<Atom, int> ProductAtoms = new Dictionary<Atom, int>();

            if (Products.Children.Count * Reactants.Children.Count == 0 || Products.Children.Count * Reactants.Children.Count == 1)
            {
                App.Alert("A total of at least 3 Products and Reactants must be specified");
                return;
            }

            foreach (Substance reactant in Reactants.Children.OfType<StackPanel>().Select(t => new Substance(t)))
            {
                foreach(KeyValuePair<Atom, int> atoms in reactant.Chemical.Elements)
                {
                    if (!ReactantAtoms.ContainsKey(atoms.Key)) ReactantAtoms.Add(atoms.Key, atoms.Value);
                    else ReactantAtoms[atoms.Key] += atoms.Value;
                }
            }

            foreach (Substance product in Products.Children.OfType<StackPanel>().Select(t => new Substance(t)))
            {
                foreach (KeyValuePair<Atom, int> atoms in product.Chemical.Elements)
                {
                    if (!ProductAtoms.ContainsKey(atoms.Key)) ProductAtoms.Add(atoms.Key, atoms.Value);
                    else ProductAtoms[atoms.Key] += atoms.Value;
                }
            }

            double[] _reactants = ReactantAtoms.OrderBy(t => t.Key.AtomicNumber).Select(t => (double)t.Value).ToArray();
            double[] _products  = ProductAtoms.OrderBy(t => t.Key.AtomicNumber).Select(t => (double)t.Value).ToArray();
        }
    }
}
