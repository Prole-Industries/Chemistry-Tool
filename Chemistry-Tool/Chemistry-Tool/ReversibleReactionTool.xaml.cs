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
        /// <summary>
        /// Equilibrium constant
        /// </summary>
        private double Kc;

        public ReversibleReactionTool()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Adds generic chemical control to reactants
        /// </summary>
        public void AddReactant(object sender, RoutedEventArgs e)
        {
            Reactants.Children.Add(App.GenericChemical());
        }

        /// <summary>
        /// Removes generic chemical control from reactants
        /// </summary>
        public void RemoveReactant(object sender, RoutedEventArgs e)
        {
            Reactants.Children.RemoveAt(Reactants.Children.Count - 1);
        }

        /// <summary>
        /// Adds generic chemical control to products
        /// </summary>
        public void AddProduct(object sender, RoutedEventArgs e)
        {
            Products.Children.Add(App.GenericChemical());
        }

        /// <summary>
        /// Removes generic chemical control from reactants
        /// </summary>
        public void RemoveProduct(object sender, RoutedEventArgs e)
        {
            Products.Children.RemoveAt(Products.Children.Count - 1);
        }

        /// <summary>
        /// Finds equilibrium constant
        /// </summary>
        public void Equilibrium(object sender, RoutedEventArgs e)
        {
            //We set these to 1 because we need to multiply all these values
            double KcReactants = 1; //Local for total reactants value
            double KcProducts = 1;  //Local for total products value

            //Checks if at least 2 products and 1 reactant or vice versa are present
            if(Products.Children.Count * Reactants.Children.Count == 0 || Products.Children.Count * Reactants.Children.Count == 1)
            {
                //If not present, alert user and exit function
                App.Alert("A total of at least 3 Products and Reactants must be specified");
                return;
            }

            //Multitply reactants with the concentration of each reactant raised to the corresponding moles
            foreach(Substance reactant in Reactants.Children.OfType<StackPanel>().Select(t => new Substance(t)))
            {
                KcReactants *= Math.Pow(reactant.Concentration, reactant.Moles);
            }

            //Multitply products with the concentration of each product raised to the corresponding moles
            foreach (Substance product in Products.Children.OfType<StackPanel>().Select(t => new Substance(t)))
            {
                KcProducts *= Math.Pow(product.Concentration, product.Moles);
            }

            Kc = KcProducts / KcReactants;  //Set Kc to Products divided by Reactants
            EqConst.Text = $"Kc: {Kc}";     //Display Kc

            EqData.Visibility = Visibility.Visible; //Make data visible
        }
    }
}
