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

        /// <summary>
        /// Generates a group of controls which allows a user to enter a chemical formula
        /// </summary>
        /// <returns>A grid control group for chemical formula, moles, and concentration</returns>
        private static UIElement Unit()
        {
            //Build a parent stackpanel to contain the controls
            StackPanel g = new StackPanel
            {
                Background = new SolidColorBrush(Color.FromArgb(0x44, 0x99, 0x99, 0x99)),
                Margin = new Thickness(0, 5, 0, 5)
            };

            g.Children.Add(new TextBlock() { Style = (Style)Application.Current.FindResource("GenericTextBox"), Text = "Chemical Formula:" });  //Label for chemical formula
            g.Children.Add(new TextBox() { ToolTip = "Formula of Chemical" });                                                                  //Entry for chemical formula                                                      //Entry for moles
            return g;
        }

        public void AddReactant(object sender, RoutedEventArgs e)
        {
            Reactants.Children.Add(Unit());
        }

        public void RemoveReactant(object sender, RoutedEventArgs e)
        {
            Reactants.Children.RemoveAt(Reactants.Children.Count - 1);
        }

        public void AddProduct(object sender, RoutedEventArgs e)
        {
            Products.Children.Add(Unit());
        }

        public void RemoveProduct(object sender, RoutedEventArgs e)
        {
            Products.Children.RemoveAt(Products.Children.Count - 1);
        }

        public void Balance(object sender, RoutedEventArgs e)
        {
            if (Products.Children.Count * Reactants.Children.Count == 0 || Products.Children.Count * Reactants.Children.Count == 1)
            {
                App.Alert("A total of at least 3 Products and Reactants must be specified");
                return;
            }

            //Implement a Gauss-Jordan Elimination to solve the chemical equation
            Dictionary<Atom, int[]> GaussJordanMatrix = new Dictionary<Atom, int[]>();
            List<Chemical> Chemicals = new List<Chemical>();
            foreach (Chemical chemical in Reactants.Children.OfType<StackPanel>().Select(t => new Chemical(((TextBox)t.Children[1]).Text)))
            {
                Chemicals.Add(chemical);
            }
            int FirstProductIndex = Chemicals.Count;
            foreach (Chemical chemical in Products.Children.OfType<StackPanel>().Select(t => new Chemical(((TextBox)t.Children[1]).Text)))
            {
                Chemicals.Add(chemical);
            }

            for (int x = 0; x < Chemicals.Count; x++)
            {
                bool IsProduct = FirstProductIndex <= x;
                foreach(KeyValuePair<Atom, int> kvp in Chemicals[x].Elements)
                {
                    Atom atom = kvp.Key; int AtomSigma = kvp.Value;
                    if(!GaussJordanMatrix.ContainsKey(atom))
                    {
                        GaussJordanMatrix.Add(atom, new int[Chemicals.Count]);
                    }

                    AtomSigma -= Convert.ToInt16(IsProduct) * 2 * AtomSigma;
                    GaussJordanMatrix[atom][x] = AtomSigma;
                }
            }

            //ShowMatrix(GaussJordanMatrix);
        }

        private void ShowMatrix(Dictionary<Atom, int[]> matrix)
        {
            foreach(KeyValuePair<Atom, int[]> kvp in matrix)
            {
                Console.Write($"{kvp.Key.Symbol} >>> ");
                foreach(int x in kvp.Value)
                {
                    Console.Write($"{x} ");
                }
                Console.Write("\n");
            }
        }
    }
}
