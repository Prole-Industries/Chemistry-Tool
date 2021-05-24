using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Chemistry_Tool
{
    /// <summary>
    /// Interaction logic for Lookup.xaml
    /// </summary>
    public partial class Lookup : Window
    {
        private BackgroundWorker Worker;    //Used to run PubChem webscraping on a separate thread

        public Lookup()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Entry point for PubChem webscraping process
        /// </summary>
        /// <remarks>Bound to SearchButton click event</remarks>
        public void SearchForChemical(object sender, RoutedEventArgs e)
        {
            Information.Visibility = Visibility.Hidden;     //Set information display (name, formula, etc.) to be invisible
            ProgressBox.Visibility = Visibility.Visible;    //Set progress box to be visible (shows that button has been pressed)
            SearchButton.IsEnabled = false;                 //Disables SearchButton so multiple parallel requests cannot be made

            Worker = new BackgroundWorker();                //Initialise background worker
            Worker.DoWork += DoWork;                        //Attach Process method (this gets run by RunWorkerAsync())
            Worker.RunWorkerCompleted += WorkDone;          //Attach Completion method (this gets run when RunWorkerAsync() is completed)
            Worker.RunWorkerAsync();                        //Run Process method
        }

        /// <summary>
        /// Flag used to check if webscraping and parsing was error free
        /// </summary>
        private static bool ExceptionPresent = false;

        /// <summary>
        /// Webscraping and parsing process
        /// </summary>
        private void DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(() => //Causes method to be run on a seperate thread so UI isn't blocked by thread traffic
            {
                ExceptionPresent = false;   //No exceptions run yet
                try
                {
                    WebClient client = new WebClient();                                                                                                 //Initialise web client

                    string cid = client.DownloadString($"https://pubchem.ncbi.nlm.nih.gov/rest/pug/compound/name/{SearchTerm.Text}/cids/TXT");          //Get a list(?) of all chemical ids used by pubchem for the given search term
                    cid = cid.Substring(0, cid.IndexOf("\n"));                                                                                          //Set used CID to be the first element
                    string chemdata = client.DownloadString($"https://pubchem.ncbi.nlm.nih.gov/rest/pug_view/data/compound/{cid}/json");                //Gets a .json of chemical data for the given CID
                    Regex regex = new Regex(@"-?\d+(\.\d+)? Â°C");  //Regex to filter for strings satisfying a float-legal string followed by °C (Â exists for some reason, no idea why)

                    JToken result = JObject.Parse(chemdata)["Record"];  //Use Newtonsoft.Json to parse the returned json; all data exists inside property "Record"

                    /* result is a really-hard-to-parse json, and its formatting is one of the worst, if not the worst, I've ever had the displeasure of seeing
                     * I mean really, why would any self-respecting individual choose to format a json like *that*? There's so many unnecessary arrays and repeated
                     * tags that just make this hard to read, let alone parse. I get that you wanna show all your citations and reference materials, and that's fine,
                     * but why not just add them as links as strings in a part of an object used to describe a property? And what's with the use of multiple values
                     * for a property? Sure, structural formulas aren't the same as empirical and all that jazz, but boiling points? There's an agreed standard, use
                     * 1 atm of pressure and USE DEGREES CELSIUS. This specky plan of using bloody fahrenheit because the yanks haven't yet got it through their thick
                     * skulls to use the global standard of units i.e. the metric system, and thus forcing like a third of the temperatures here to be totally useless
                     * really annoys me - I'm not using old, hard to understand numbers in this, and I really can't be asked making a converter for Fahrenheit to Celsius.
                     * Oh yeah, I went off on a tangent, THE FORMULA OF A CHEMICAL IS GIVEN LIKE 12 TIMES. The names being different, I can get and support, and isomers
                     * are fair enough, but having loads of copies is just naff. And christ almighty, the number of copies of experimental values is stupid. I get you
                     * might find different values for things in different labs, but wikipedia just gives me one number, not to mention that PUG-REST, a similar protocol
                     * made by PubChem themselves, only gives one value. Also, PUG-REST, which I imagine references the same database, doesn't give the same amount of
                     * data as PUG-VIEW, which is what I'm using. The fact that two parallel protocols, which use the same data, don't give the same abundancies of data
                     * really really annoys me to the point of pure anger. Also PUG-SOAP, which is meant to integrate really well into C# .NET doens't actually work
                     * due to obselete Web References.
                     *
                     * TL;DR: This is a mess, I'm mightily cheesed, Die well brave warrior. (Trust me bro)
                     */
                    string name = Regex.Replace(result["RecordTitle"].Value<string>(), @"(?<=[ \-,])[a-z]", new MatchEvaluator(CapitaliseSelective));
                    string structure = result
                        ["Section"]
                        .Where(t => t["TOCHeading"].Value<string>() == "Names and Identifiers").FirstOrDefault()
                        ["Section"]
                        .Where(t => t["TOCHeading"].Value<string>() == "Molecular Formula").FirstOrDefault()
                        ["Information"][0]["Value"]["StringWithMarkup"][0]["String"].Value<string>();
                    string inchi = result
                        ["Section"]
                        .Where(t => t["TOCHeading"].Value<string>() == "Names and Identifiers").FirstOrDefault()
                        ["Section"]
                        .Where(t => t["TOCHeading"].Value<string>() == "Computed Descriptors").FirstOrDefault()
                        ["Section"]
                        .Where(t => t["TOCHeading"].Value<string>() == "InChI").FirstOrDefault()
                        ["Information"][0]["Value"]["StringWithMarkup"][0]["String"].Value<string>();

                    string melting = result
                        ["Section"]
                        .Where(t => t["TOCHeading"].Value<string>() == "Chemical and Physical Properties").FirstOrDefault()
                        ["Section"]
                        .Where(t => t["TOCHeading"].Value<string>() == "Experimental Properties").FirstOrDefault()
                        ["Section"]
                        .Where(t => t["TOCHeading"].Value<string>() == "Melting Point").FirstOrDefault()
                        ["Information"]
                        .Where(t => t["Value"]["StringWithMarkup"] != null)
                        .Where(t => regex.IsMatch(t["Value"]["StringWithMarkup"][0]["String"].Value<string>())).FirstOrDefault()
                        ["Value"]["StringWithMarkup"][0]["String"].Value<string>().Replace("Â", "");

                    string boiling = result
                        ["Section"]
                        .Where(t => t["TOCHeading"].Value<string>() == "Chemical and Physical Properties").FirstOrDefault()
                        ["Section"]
                        .Where(t => t["TOCHeading"].Value<string>() == "Experimental Properties").FirstOrDefault()
                        ["Section"]
                        .Where(t => t["TOCHeading"].Value<string>() == "Boiling Point").FirstOrDefault()
                        ["Information"]
                        .Where(t => t["Value"]["StringWithMarkup"] != null)
                        .Where(t => regex.IsMatch(t["Value"]["StringWithMarkup"][0]["String"].Value<string>())).FirstOrDefault()
                        ["Value"]["StringWithMarkup"][0]["String"].Value<string>().Replace("Â", "");

                    //Create a chemical from the structural formula (guaranteed to be parse-safe) to get molar weight
                    Chemical homechem = new Chemical(structure);

                    //Set labels to have the given properties
                    Name.Text = $"{name}";
                    Structure.Text = $"{homechem.MolecularFormulaPretty}";
                    InChI_Identifier.Text = $"InChI Identifier: {inchi}";
                    MeltingPoint.Text = $"Melting Point: {melting.Substring(0, melting.IndexOf("°C") + 2)}";
                    BoilingPoint.Text = $"Boiling Point: {boiling.Substring(0, boiling.IndexOf("°C") + 2)}";
                    MolarWeight.Text = $"Molar Weight: {homechem.MolarWeight}";

                    //Get a bitmap image of the chemical structure
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri($"https://pubchem.ncbi.nlm.nih.gov/rest/pug/compound/cid/{cid}/png");
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    //Place the bitmap into the structure image control
                    StructureImage.Source = bitmap;
                }
                //Information gathering can throw exceptions, these get handled here
                catch (Exception _ex)
                {
                    ExceptionPresent = true;    //Indicates that exception has been run
                    switch (_ex)                 //Check type of exception
                    {
                        case NullReferenceException ex: //Usually thrown when JObject cannot be properly parsed (and considering it's Marietta's Cinco de Mayo I'm not going to fix these yet)
                            App.Alert("There was an error parsing the server return object and the resulting data could not be resolved.");
                            break;

                        case WebException ex:           //Usually thrown when unable to connect to the database or chemical not found
                            App.Alert($"{ex.Message}.");
                            break;
                    }
                }
            }, DispatcherPriority.ContextIdle); //ContextIdle is meant to do something but doesn't seem to, but I don't wanna mess about with it
        }

        /// <summary>
        /// Exit function of worker web-scraping
        /// </summary>
        private void WorkDone(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!ExceptionPresent) Information.Visibility = Visibility.Visible;  //Show the information ONLY IF NO EXCEPTIONS RAN (otherwise the separator would be visible)
            ProgressBox.Visibility = Visibility.Hidden;                         //Hide the working... label
            SearchButton.IsEnabled = true;                                      //Reenable the search button
        }

        /// <summary>
        /// Captialises the given match token
        /// </summary>
        /// <param name="match">Match token to convert to upper case</param>
        /// <returns>Upper case version of match</returns>
        private static string CapitaliseSelective(Match match)
        {
            return match.Value.ToUpper();
        }
    }
}