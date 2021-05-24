using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chemistry_Tool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Shorthand method for creating an error dialog box
        /// </summary>
        /// <param name="message">Message to be shown to the user</param>
        public static void Alert(string message)
        {
            MessageBox.Show(message, "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Generates a group of controls which allows a user to enter a chemical formula, moles, and concentration
        /// </summary>
        /// <returns>A grid control group for chemical formula, moles, and concentration</returns>
        public static UIElement GenericChemical()
        {
            //Build a parent stackpanel to contain the controls
            StackPanel g = new StackPanel
            {
                Background = new SolidColorBrush(Color.FromArgb(0x44, 0x99, 0x99, 0x99)),
                Margin = new Thickness(0, 5, 0, 5)
            };

            g.Children.Add(new TextBlock() { Style = (Style)Current.FindResource("GenericTextBox"), Text = "Chemical Formula:" });                          //Label for chemical formula
            g.Children.Add(new TextBox() { Style = (Style)Current.FindResource("GenericTextEntry"), ToolTip = "Formula of Chemical" });                     //Entry for chemical formula
            g.Children.Add(new TextBlock() { Style = (Style)Current.FindResource("GenericTextBox"), Text = "Concentration:" });                             //Label for concentration
            g.Children.Add(new TextBox() { Style = (Style)Current.FindResource("GenericTextEntry"), ToolTip = "Concentration of Chemical (mol dm⁻³)" });    //Entry for concentration
            g.Children.Add(new TextBlock() { Style = (Style)Current.FindResource("GenericTextBox"), Text = "Moles:" });                                     //Label for moles
            g.Children.Add(new TextBox() { Style = (Style)Current.FindResource("GenericTextEntry"), ToolTip = "Moles of Chemical" });                       //Entry for moles
            return g;
        }

        /// <summary>
        /// Checks if all boxes of a generic chemical control are filled in and valid
        /// </summary>
        /// <param name="target">The generic chemical control (should be a stack panel, made from GenericChemical();)</param>
        /// <returns></returns>
        public static bool IsChemicalValid(StackPanel target)
        {
            if (((TextBox)target.Children[1]).Text == "") { App.Alert("One or more chemical names are missing."); return false; }
            if (!IsNumValid((TextBox)target.Children[3])) { App.Alert("One or more chemical concentrations are invalid."); return false; }
            if (!IsNumValid((TextBox)target.Children[5])) { App.Alert("One or more molar values are invalid."); return false; }
            return true;
        }

        /// <summary>
        /// Matches a regex against a numerical value (accepts floating points)
        /// </summary>
        /// <param name="target">Textbox containing text to be checked</param>
        /// <returns></returns>
        public static bool IsNumValid(TextBox target)
        {
            Regex regex = new Regex(@"^(\d+)((.)?(\d+))?$", RegexOptions.Multiline);
            return regex.IsMatch(target.Text);
        }
    }

    /// <summary>
    /// Describes a singular Atom/Ion
    /// </summary>
    public class Atom
    {
        /// <summary>
        /// Atomic Symbol of the atom (e.g. H, Na, etc.)
        /// </summary>
        public string Symbol { get; private set; }

        /// <summary>
        /// Name of the atom (e.g. Hydrogen, Sodium, etc.)
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Atomic number of the atom (proton count)
        /// </summary>
        public int AtomicNumber { get; private set; }

        /// <summary>
        /// Mass number of the atom (nucleon count)
        /// </summary>
        public double MassNumber { get; private set; }

        /// <summary>
        /// Period of the atom
        /// </summary>
        public int Period { get; private set; }

        /// <summary>
        /// Group of the atom
        /// </summary>
        public int Group { get; private set; }

        /// <summary>
        /// Electron configuration of the atom
        /// </summary>
        /// <remarks>Not yet implemented</remarks>
        public List<int> ElectronConfig { get; set; }

        /// <summary>
        /// The category of the atom (e.g. Alkali metal, Halogen, etc.)
        /// </summary>
        public Categories Category { get; private set; }

        /// <summary>
        /// Only to be used if the data entered is invalid
        /// </summary>
        public Atom() { }

        /// <summary>
        /// Constructor for an atom
        /// </summary>
        /// <param name="_symbol">Atomic Symbol of the atom</param>
        /// <param name="_name">Name of the atom</param>
        /// <param name="_anum">Atomic Number of the atom</param>
        /// <param name="_mnum">Mass Number of the atom</param>
        /// <param name="_period">Period of the atom</param>
        /// <param name="_group">Group of the atom</param>
        /// <param name="_category">Category of the atom</param>
        public Atom(string _symbol, string _name, int _anum, double _mnum, int _period, int _group, Categories _category)
        {
            Symbol = _symbol;
            Name = _name;
            AtomicNumber = _anum;
            MassNumber = _mnum;
            Period = _period;
            Group = _group;
            Category = _category;
        }

        /// <summary>
        /// Catgeories of atoms (e.g. Alkali Metals, Halogens)
        /// </summary>
        public enum Categories
        {
            AlkaliMetal,
            AlkaliEarthMetal,
            Metalloid,
            NonMetal,
            Halogen,
            NobleGas,
            TransitionMetal,
            PostTransitionMetal,
            Lanthanide,
            Actinide,
        }

        /// <summary>
        /// Returns an atom by entering its atomic symbol
        /// </summary>
        /// <param name="symbol">Symbol to be searched for</param>
        /// <returns>An atom with the symbol passed</returns>
        public static Atom GetBySymbol(string symbol)
        {
            try
            {
                return PeriodicTable[PeriodicTable.FindIndex(t => t.Symbol == symbol)]; //Get atom by searching the periodic table for the symbol
            }
            catch (ArgumentOutOfRangeException)
            {
                App.Alert($"Symbol '{symbol}' could not be resolved.");                  //If symbol does not exist, then throw an error
            }
            return new Atom();
        }

        //I wrote the periodic table out for you can I please have a 9 thanks
        public static List<Atom> PeriodicTable = new List<Atom>()
        {
            //       Symbol Name            Atomic  Nucleon Period  Group   Category
            new Atom("H",   "Hydrogen",     1,      1,      1,      1,      Categories.NonMetal),
            new Atom("He",  "Helium",       2,      4,      1,      8,      Categories.NobleGas),

            new Atom("Li",  "Lithium",      3,      7,      2,      1,      Categories.AlkaliMetal),
            new Atom("Be",  "Beryllium",    4,      9,      2,      2,      Categories.AlkaliEarthMetal),
            new Atom("B",   "Boron",        5,      11,     2,      3,      Categories.Metalloid),
            new Atom("C",   "Carbon",       6,      12,     2,      4,      Categories.NonMetal),
            new Atom("N",   "Nitrogen",     7,      14,     2,      5,      Categories.NonMetal),
            new Atom("O",   "Oxygen",       8,      16,     2,      6,      Categories.NonMetal),
            new Atom("F",   "Fluorine",     9,      19,     2,      7,      Categories.Halogen),
            new Atom("Ne",  "Neon",         10,     20,     2,      8,      Categories.NobleGas),

            new Atom("Na",  "Sodium",       11,     23,     3,      1,      Categories.AlkaliMetal),
            new Atom("Mg",  "Magnesium",    12,     24,     3,      2,      Categories.AlkaliEarthMetal),
            new Atom("Al",  "Aluminium",    13,     27,     3,      3,      Categories.PostTransitionMetal),
            new Atom("Si",  "Silicon",      14,     28,     3,      4,      Categories.Metalloid),
            new Atom("P",   "Phosphorus",   15,     31,     3,      5,      Categories.NonMetal),
            new Atom("S",   "Sulphur",      16,     32,     3,      6,      Categories.NonMetal),
            new Atom("Cl",  "Chlorine",     17,     35.5,   3,      7,      Categories.Halogen),
            new Atom("Ar",  "Argon",        18,     40,     3,      8,      Categories.NobleGas),

            new Atom("K",   "Potassium",    19,     39,     4,      1,      Categories.AlkaliMetal),
            new Atom("Ca",  "Calcium",      20,     40,     4,      2,      Categories.AlkaliEarthMetal),
            new Atom("Sc",  "Scandium",     21,     45,     4,      0,      Categories.TransitionMetal),
            new Atom("Ti",  "Titanium",     22,     48,     4,      0,      Categories.TransitionMetal),
            new Atom("V",   "Vanadium",     23,     51,     4,      0,      Categories.TransitionMetal),
            new Atom("Cr",  "Chromium",     24,     52,     4,      0,      Categories.TransitionMetal),
            new Atom("Mn",  "Manganese",    25,     55,     4,      0,      Categories.TransitionMetal),
            new Atom("Fe",  "Iron",         26,     56,     4,      0,      Categories.TransitionMetal),
            new Atom("Co",  "Cobalt",       27,     59,     4,      0,      Categories.TransitionMetal),
            new Atom("Ni",  "Nickel",       28,     59,     4,      0,      Categories.TransitionMetal),
            new Atom("Cu",  "Copper",       29,     63.5,   4,      0,      Categories.TransitionMetal),
            new Atom("Zn",  "Zinc",         30,     65,     4,      0,      Categories.TransitionMetal),
            new Atom("Ga",  "Gallium",      31,     70,     4,      3,      Categories.PostTransitionMetal),
            new Atom("Ge",  "Germanium",    32,     73,     4,      4,      Categories.Metalloid),
            new Atom("As",  "Arsenic",      33,     75,     4,      5,      Categories.Metalloid),
            new Atom("Se",  "Selenium",     34,     79,     4,      6,      Categories.NonMetal),
            new Atom("Br",  "Bromine",      35,     80,     4,      7,      Categories.Halogen),
            new Atom("Kr",  "Krypton",      36,     84,     4,      8,      Categories.NobleGas),

            new Atom("Rb",  "Rubidium",     37,     85,     5,      1,      Categories.AlkaliMetal),
            new Atom("Sr",  "Strontium",    38,     88,     5,      2,      Categories.AlkaliEarthMetal),
            new Atom("Y",   "Yttrium",      39,     89,     5,      0,      Categories.TransitionMetal),
            new Atom("Zr",  "Zirconium",    40,     91,     5,      0,      Categories.TransitionMetal),
            new Atom("Nb",  "Niobium",      41,     93,     5,      0,      Categories.TransitionMetal),
            new Atom("Mo",  "Molybdenum",   42,     96,     5,      0,      Categories.TransitionMetal),
            new Atom("Tc",  "Technetium",   43,     98,     5,      0,      Categories.TransitionMetal),
            new Atom("Ru",  "Ruthenium",    44,     101,    5,      0,      Categories.TransitionMetal),
            new Atom("Rh",  "Rhodium",      45,     103,    5,      0,      Categories.TransitionMetal),
            new Atom("Pd",  "Palladium",    46,     106,    5,      0,      Categories.TransitionMetal),
            new Atom("Ag",  "Silver",       47,     108,    5,      0,      Categories.TransitionMetal),
            new Atom("Cd",  "Cadmium",      48,     112,    5,      0,      Categories.TransitionMetal),
            new Atom("In",  "Indium",       49,     115,    5,      5,      Categories.PostTransitionMetal),
            new Atom("Sn",  "Tin",          50,     119,    5,      4,      Categories.PostTransitionMetal),
            new Atom("Sb",  "Antimony",     51,     122,    5,      5,      Categories.Metalloid),
            new Atom("Te",  "Tellerium",    52,     128,    5,      6,      Categories.Metalloid),
            new Atom("I",   "Iodine",       53,     127,    5,      7,      Categories.Halogen),
            new Atom("Xe",  "Xenon",        54,     131,    5,      8,      Categories.NobleGas),

            new Atom("Cs",  "Caesium",      55,     133,    6,      1,      Categories.AlkaliMetal),
            new Atom("Ba",  "Barium",       56,     137,    6,      2,      Categories.AlkaliEarthMetal),

            new Atom("La",  "Lanthanum",    57,     139,    6,      0,      Categories.Lanthanide),
            new Atom("Ce",  "Cerium",       58,     140,    6,      0,      Categories.Lanthanide),
            new Atom("Pr",  "Praeseodymium",59,     141,    6,      0,      Categories.Lanthanide),
            new Atom("Nd",  "Neodymium",    60,     144,    6,      0,      Categories.Lanthanide),
            new Atom("Pm",  "Promethium",   61,     145,    6,      0,      Categories.Lanthanide),
            new Atom("Sm",  "Samarium",     62,     150,    6,      0,      Categories.Lanthanide),
            new Atom("Eu",  "Europium",     63,     152,    6,      0,      Categories.Lanthanide),
            new Atom("Gd",  "Gadolinium",   64,     157,    6,      0,      Categories.Lanthanide),
            new Atom("Tb",  "Terbium",      65,     159,    6,      0,      Categories.Lanthanide),
            new Atom("Dy",  "Dysprosium",   66,     162,    6,      0,      Categories.Lanthanide),
            new Atom("Ho",  "Holmium",      67,     165,    6,      0,      Categories.Lanthanide),
            new Atom("Er",  "Erbium",       68,     167,    6,      0,      Categories.Lanthanide),
            new Atom("Tm",  "Thulium",      69,     169,    6,      0,      Categories.Lanthanide),
            new Atom("Yb",  "Ytterbium",    70,     173,    6,      0,      Categories.Lanthanide),
            new Atom("Lu",  "Lutetium",     71,     175,    6,      0,      Categories.Lanthanide),

            new Atom("Hf",  "Hafnium",      72,     178,    6,      0,      Categories.TransitionMetal),
            new Atom("Ta",  "Tantalum",     73,     181,    6,      0,      Categories.TransitionMetal),
            new Atom("W",   "Tungsten",     74,     184,    6,      0,      Categories.TransitionMetal),
            new Atom("Re",  "Rhenium",      75,     186,    6,      0,      Categories.TransitionMetal),
            new Atom("Os",  "Osmium",       76,     190,    6,      0,      Categories.TransitionMetal),
            new Atom("Ir",  "Iridium",      77,     192,    6,      0,      Categories.TransitionMetal),
            new Atom("Pt",  "Platinum",     78,     195,    6,      0,      Categories.TransitionMetal),
            new Atom("Au",  "Gold",         79,     198,    6,      0,      Categories.TransitionMetal),
            new Atom("Hg",  "Mercury",      80,     201,    6,      0,      Categories.TransitionMetal),
            new Atom("Tl",  "Thallium",     81,     204,    6,      3,      Categories.PostTransitionMetal),
            new Atom("Pb",  "Lead",         82,     207,    6,      4,      Categories.PostTransitionMetal),
            new Atom("Bi",  "Bismuth",      83,     209,    6,      5,      Categories.PostTransitionMetal),
            new Atom("Po",  "Polonium",     84,     209,    6,      6,      Categories.Metalloid),
            new Atom("At",  "Astatine",     85,     210,    6,      7,      Categories.Halogen),
            new Atom("Rn",  "Radon",        86,     222,    6,      8,      Categories.NobleGas),

            new Atom("Fr",  "Francium",     87,     223,    7,      1,      Categories.AlkaliMetal),
            new Atom("Ra",  "Radium",       88,     226,    7,      2,      Categories.AlkaliEarthMetal),

            new Atom("Ac",  "Actinium",     89,     227,    7,      0,      Categories.Actinide),
            new Atom("Th",  "Thorium",      90,     232,    7,      0,      Categories.Actinide),
            new Atom("Pa",  "Protactinium", 91,     231,    7,      0,      Categories.Actinide),
            new Atom("U",   "Uranium",      92,     238,    7,      0,      Categories.Actinide),
            new Atom("Np",  "Neptunium",    93,     237,    7,      0,      Categories.Actinide),
            new Atom("Pu",  "Plutonium",    94,     244,    7,      0,      Categories.Actinide),
            new Atom("Am",  "Americium",    95,     243,    7,      0,      Categories.Actinide),
            new Atom("Cm",  "Curium",       96,     247,    7,      0,      Categories.Actinide),
            new Atom("Bk",  "Berkelium",    97,     247,    7,      0,      Categories.Actinide),
            new Atom("Cf",  "Californium",  98,     251,    7,      0,      Categories.Actinide),
            new Atom("Es",  "Einsteinium",  99,     252,    7,      0,      Categories.Actinide),
            new Atom("Fm",  "Fermium",      100,    257,    7,      0,      Categories.Actinide),
            new Atom("Md",  "Mendelevium",  101,    258,    7,      0,      Categories.Actinide),
            new Atom("No",  "Nobelium",     102,    259,    7,      0,      Categories.Actinide),
            new Atom("Lr",  "Lawrencium",   103,    262,    7,      0,      Categories.Actinide),

            new Atom("Rf",  "Rutherfordium",104,    261,    7,      0,      Categories.TransitionMetal),
            new Atom("Db",  "Dubnium",      105,    262,    7,      0,      Categories.TransitionMetal),
            new Atom("Sg",  "Seaborgium",   106,    263,    7,      0,      Categories.TransitionMetal),
            new Atom("Bh",  "Bohrium",      107,    264,    7,      0,      Categories.TransitionMetal),
            new Atom("Hs",  "Hassium",      108,    277,    7,      0,      Categories.TransitionMetal),
            new Atom("Mt",  "Meitnerium",   109,    268,    7,      0,      Categories.TransitionMetal),
            new Atom("Ds",  "Darmstadtium", 110,    271,    7,      0,      Categories.TransitionMetal),
            new Atom("Rg",  "Roentgenium",  111,    272,    7,      0,      Categories.TransitionMetal),
            new Atom("Cn",  "Copernicium",  112,    285,    7,      0,      Categories.TransitionMetal),
            new Atom("Nh",  "Nihonium",     113,    286,    7,      3,      Categories.PostTransitionMetal),    //Executive decision
            new Atom("Fl",  "Flerovium",    114,    289,    7,      4,      Categories.PostTransitionMetal),    //Executive decision
            new Atom("Mc",  "Moscovium",    115,    289,    7,      5,      Categories.PostTransitionMetal),    //Executive decision
            new Atom("Lv",  "Livermorium",  116,    293,    7,      6,      Categories.PostTransitionMetal),    //Executive decision
            new Atom("Ts",  "Tennessine",   117,    294,    7,      7,      Categories.Halogen),
            new Atom("Og",  "Oganesson",    118,    294,    7,      8,      Categories.NobleGas)
        };
    }

    /// <summary>
    /// Describes a chemical
    /// </summary>
    public class Chemical
    {
        /// <summary>
        /// Dictionary of all elements in the atom and the amount of atoms
        /// </summary>
        public Dictionary<Atom, int> Elements { get; private set; } = new Dictionary<Atom, int>();

        /// <summary>
        /// The simple, parse-safe version of the formula
        /// </summary>
        public string MolecularFormula { get; private set; }

        /// <summary>
        /// The formula containing subscript
        /// </summary>
        public string MolecularFormulaPretty { get; private set; }

        /// <summary>
        /// The molar weight of the chemical
        /// </summary>
        public double MolarWeight { get; private set; } = 0;

        public bool IsValid { get; private set; } = true;

        /// <summary>
        /// Constructor for a chemical object
        /// </summary>
        /// <param name="formula">The parse-safe formula of the chemical (no subscript)</param>
        public Chemical(string formula)
        {
            GetElements(formula, 1);    //Find the elements of the chemical (parses formula), number is 1 because it needs to satisfy the recursive parameter

            MolecularFormula = formula; //Molecular formula (parse-safe) is set to the constructor parameter

            for (int x = 48; x < 58; x++)    //Converts unicode 0-9 to subscript 0-9 by shifting their unicode values
            {
                formula = formula.Replace(Convert.ToChar(x), Convert.ToChar(x + 8272));   //Iterative, shifts the standard numerals to subscript
            }
            MolecularFormulaPretty = formula;   //Set this replaced formula to be the pretty formula

            foreach (KeyValuePair<Atom, int> kvp in Elements)
            {
                MolarWeight += kvp.Key.MassNumber * kvp.Value;  //Find molar weight by computing a total of the atomic weights of each of the atoms
            }
        }

        /// <summary>
        /// Parses the standard formula to develop the Elements dictionary
        /// </summary>
        /// <param name="formula">Formula to be parsed</param>
        /// <param name="number">Number of groups (if multiple ionic groups exist)</param>
        private void GetElements(string formula, int number)
        {
            string elementRegex = @"[A-Z][a-z]?\d*|(?<!\([^)]*)\(.*\)\d+(?![^(]*\))";   //Used to isolate atoms or groups of atoms, and their counts if applicable
            string validateRegex = $"^({elementRegex})+$";                              //Used to check if the formula is legal

            if (!Regex.IsMatch(formula, validateRegex, RegexOptions.Multiline))
            {
                App.Alert("Formula could not be successfully parsed.");                  //Throws if the formula passed is illegal
                IsValid = false;
                return;
            }

            foreach (Match match in Regex.Matches(formula, elementRegex))               //Foreach atom or atom group found in the formula
            {
                string mstring = match.ToString();  //String representation of the match, prevents having to run the same function call continuously
                if (mstring.Contains("("))          //All atom group matches contain an open bracket, so if it exists...
                {
                    //...run this function recursively on the atom group, by isolating the atom group and its count.
                    GetElements(mstring.Substring(1, mstring.Length - mstring.LastIndexOf(")") + 1), Convert.ToInt32(mstring.Substring(mstring.LastIndexOf(")") + 1)));
                }
                else    //Enters if the match isn't an atom group
                {
                    string symbol = Regex.Replace(mstring, @"\d*", string.Empty);       //Atom symbol is found by removing all numerics
                    int count =                                                         //Ternary logic operator; set the count to the number after the symbol if it exists, else make it 1
                        Regex.Replace(mstring, @"[A-Za-z]", string.Empty) != "" ?
                        int.Parse(Regex.Replace(mstring, @"[A-Za-z]", string.Empty)) :
                        1;
                    Atom genericAtom = Atom.GetBySymbol(symbol);                        //Build an atom by the given symbol

                    if (genericAtom == new Atom())
                    {
                        IsValid = false;
                        return;
                    }
                    if (Elements.ContainsKey(genericAtom)) Elements[genericAtom] += (count * number);   //If the atom is already present in the Elements dictonary, increase the associate int by count * number of atom groups
                    else Elements.Add(genericAtom, count * number);                                     //If the atom doesn't exist in Elements, add the Atom to it and set its inital associate int to count * number of atom groups
                }
            }
        }

        /// <summary>
        /// Prints out a list of atoms present as well as the number of atoms present
        /// </summary>
        /// <remarks>Debug function</remarks>
        public void ResolveElements()
        {
            foreach (KeyValuePair<Atom, int> element in Elements)
            {
                Console.WriteLine($"{element.Key.Name} ({element.Key.Symbol}) >>> {element.Value}");
            }
        }

        /// <summary>
        /// Holds information about the chemical in a human-readable way.
        /// </summary>
        public class Metadata
        {
            public string Name { get; private set; }
            public string Structure { get; private set; }
            public string InChI { get; private set; }
            public string MeltingPoint { get; private set; }
            public string BoilingPoint { get; private set; }

            /// <summary>
            /// Constructor for Metadata class
            /// </summary>
            /// <param name="_name">Name of chemical</param>
            /// <param name="_structure">Structure of chemical (equivalent to MolecularFormula property of Chemical)</param>
            /// <param name="_inchi">IUPAC International Chemical Identifier</param>
            /// <param name="_meltingpoint">Melting point of the chemical (°C)</param>
            /// <param name="_boilingpoint">Boiling point of the chemical (°C)</param>
            public Metadata(string _name, string _structure, string _inchi, string _meltingpoint, string _boilingpoint)
            {
                Name = _name;
                Structure = _structure;
                InChI = _inchi;
                MeltingPoint = _meltingpoint;
                BoilingPoint = _boilingpoint;
            }
        }
    }

    /// <summary>
    /// Describes quantities of a chemical
    /// </summary>
    public class Substance
    {
        /// <summary>
        /// Subject chemical
        /// </summary>
        public Chemical Chemical { get; private set; }

        /// <summary>
        /// Concentration of the chemical in mol dm^-3
        /// </summary>
        public double Concentration { get; private set; }

        /// <summary>
        /// Moles of the chemical present
        /// </summary>
        public double Moles { get; private set; }

        /// <summary>
        /// Constructor for Substance
        /// </summary>
        /// <param name="sp">Stackpanel to be parsed to build the substance</param>
        public Substance(StackPanel sp)
        {
            try
            {
                Chemical = new Chemical(((TextBox)sp.Children[1]).Text);        //References first text entry control
                Concentration = double.Parse(((TextBox)sp.Children[3]).Text);   //References second text entry control
                Moles = double.Parse(((TextBox)sp.Children[5]).Text);           //References third text entry control
            }
            catch
            {
                App.Alert("Substance cannot be resolved.");                     //If parsing unsuccessful, alert user
            }
        }
    }
}