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
            try
            {
                Reactants.Children.RemoveAt(Reactants.Children.Count - 1);
            }
            catch { }
        }

        public void AddProduct(object sender, RoutedEventArgs e)
        {
            Products.Children.Add(Unit());
        }

        public void RemoveProduct(object sender, RoutedEventArgs e)
        {
            try
            {
                Products.Children.RemoveAt(Products.Children.Count - 1);
            }
            catch { }
        }

        public void Balance(object sender, RoutedEventArgs e)
        {
            if (Products.Children.Count * Reactants.Children.Count == 0 || Products.Children.Count * Reactants.Children.Count == 1)
            {
                App.Alert("A total of at least 3 Products and Reactants must be specified");
                return;
            }

            BalancedEq.Visibility = Visibility.Hidden;

            //Implement a Gauss-Jordan Elimination to solve the chemical equation
            //See more here: http://www.ijstr.org/final-print/jan2020/-Application-Of-Gauss-jordan-Elimination-Method-In-Balancing-Typical-Chemical-Equations.pdf

            Dictionary<Atom, double[]> GaussJordanMatrix = new Dictionary<Atom, double[]>();    //Will be used to find the ratios of all chemicals, atom key served to give ratio
            List<Chemical> Chemicals = new List<Chemical>();                                    //List of all chemicals (used as a proxy to build the Matrix)

            foreach (Chemical chemical in Reactants.Children.OfType<StackPanel>().Select(t => new Chemical(((TextBox)t.Children[1]).Text)))
            {
                Chemicals.Add(chemical);
            }
            int FirstProductIndex = Chemicals.Count;    //Keep track of first product index, as the products must be input as negative (helps code run branchless)
            foreach (Chemical chemical in Products.Children.OfType<StackPanel>().Select(t => new Chemical(((TextBox)t.Children[1]).Text)))
            {
                Chemicals.Add(chemical);
            }

            for (int x = 0; x < Chemicals.Count; x++)
            {
                bool IsProduct = FirstProductIndex <= x;    //Flag if the current chemical is a product or not
                foreach(KeyValuePair<Atom, int> kvp in Chemicals[x].Elements)
                {
                    Atom atom = kvp.Key; int AtomSigma = kvp.Value; //Current atom being checked, amount of that atom in the chemical
                    if(!GaussJordanMatrix.ContainsKey(atom))
                    {
                        //If the atom isn't in the matrix, add it in
                        //Add 1 extra column to act as a pre-emptive matrix augmentation (column has to be 0)
                        GaussJordanMatrix.Add(atom, new double[Chemicals.Count+1]);
                    }

                    AtomSigma -= Convert.ToInt16(IsProduct) * 2 * AtomSigma;    //IsProduct*2 allows to make AtomSigma negative without branching
                    GaussJordanMatrix[atom][x] = AtomSigma;                     //Sets the matrix value for that atom at that chemical index to be AtomSigma
                }
            }

            //At this point, GaussJordanMatrix is a standard augmented matrix, it must now be converted to its reduced row echelon form (rref)
            //This is how I'm doing it: https://www.geeksforgeeks.org/program-for-gauss-jordan-elimination-method/
            //Because this is copy-paste I'm not commenting, so trust the player

            //Convert to a 2d array because it makes things so much easier ygm
            double[,] Matrix = new double[GaussJordanMatrix.Count, GaussJordanMatrix.Values.ToArray()[0].Length];
            for(int x = 0; x < GaussJordanMatrix.Count; x++)
            {
                for(int y = 0; y < GaussJordanMatrix.Values.ToArray()[x].Length; y++)
                {
                    Matrix[x, y] = GaussJordanMatrix.Values.ToArray()[x][y];
                }
            }

            //int i, j, c;
            //int k = 0;
            //int n = Matrix.GetUpperBound(0)+1;

            //for(i = 0; i < n; i++)
            //{
            //    if(Matrix[i,i] == 0)
            //    {
            //        c = 1;
            //        while ((i + c) < n && Matrix[i + c, i] == 0) c++;
            //        if ((i + c) == n) break;
            //        for (j = i, k = 0; k <= n; k++)
            //        {
            //            double temp = Matrix[j, k];
            //            Matrix[j, k] = Matrix[j + c, k];
            //            Matrix[j + c, k] = temp;
            //        }
            //    }
            //    for (j = 0; j < n; j++)
            //    {
            //        //Excluding all i == j
            //        if (i != j)
            //        {
            //            //Converts Matrix to ref
            //            double p = Matrix[j, i] / Matrix[i, i];
            //            for (k = 0; k <= n; k++)
            //            {
            //                try
            //                {
            //                    Matrix[j, k] = Matrix[j, k] - (Matrix[i, k]) * p;
            //                }
            //                catch { }
            //            }
            //        }
            //    }
            //}
            //for (i = 0; i < n; i++)
            //{
            //    double div = Matrix[i, i];
            //    for (j = i; j < GaussJordanMatrix.Values.ToArray()[0].Length; j++) Matrix[i, j] /= div;
            //}

            //We're gonna try using this:
            //http://csharp.algorithmexamples.com/web/Algorithms/Numeric/GaussJordanElimination.html

            //Matrix is now in reduced row echelon form

            //Make coefficients integers
            double[] icoefficients = new double[Chemicals.Count];
            double[] dcoefficients = new double[Chemicals.Count];
            int[] coefficients = new int[Chemicals.Count];

            int coefIndex = 0;
            for(int x = 0; x < Matrix.GetLength(1); x++)
            {
                if(Matrix[0, x] != 0 && Matrix[0, x] != 1)
                {
                    coefIndex = x;
                    break;
                }
            }
            for(int x = 0; x < Matrix.GetLength(0); x++)
            {
                double q = Matrix[x, coefIndex];
                if (double.IsNaN(q) || q == 0) q = -1;
                icoefficients[x] = q;
            }
            icoefficients = icoefficients.Select(t => t == 0 ? -1 : t).ToArray();

            double smallest = icoefficients.Select(t => t).Max();    //Min in abs, but max in real (since all are negative)
            icoefficients.CopyTo(dcoefficients, 0);
            dcoefficients = dcoefficients.Select(t => t * 1000 / smallest).ToArray();   //Multiply by 1000 to be REALLY safe over purging decimals
            coefficients = dcoefficients.Select(t => (int)Math.Round(t)).ToArray();     //Haha floating points go brrr
            int gcd = ArrayGCD(coefficients);
            coefficients = coefficients.Select(t => t / gcd).ToArray();                 //Divide all the values by the greated common factor to give equation in simplest form
            //Coefficients are fully balanced (apply to each chemical sequentially)

            //Builds the equation in a nice string
            List<string> coefchems = new List<string>();
            for(int x = 0; x < Chemicals.Count; x++)
            {
                string cf = coefficients[x] == 1 ? "" : coefficients[x].ToString();
                string push = $"{cf}{Chemicals[x].MolecularFormulaPretty}";
                if (x == FirstProductIndex - 1) //To avoid spaghetti code, we add an arrow and wang in the next chemical all as one element (it's easier when joining)
                {
                    x++;
                    cf = coefficients[x] == 1 ? "" : coefficients[x].ToString();
                    push += $" → {cf}{Chemicals[x].MolecularFormulaPretty}";
                }
                coefchems.Add(push);
            }

            BalancedEq.Text = string.Join(" + ", coefchems);    //Add + signs between all elements
            BalancedEq.Visibility = Visibility.Visible;
        }

        private int ArrayGCD(int[] arr)
        {
            int GCD(int a, int b)
            {
                if (a == 0) return b;
                return GCD(b % a, a);
            }

            int result = arr[0];
            for(int i = 1; i < arr.Length; i++)
            {
                result = GCD(arr[i], result);
                if (result == 1) return 1;
            }
            return result;
        }
    }
}
