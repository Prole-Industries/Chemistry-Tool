using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Chemistry_Tool
{
    /// <summary>
    /// Interaction logic for Lookup.xaml
    /// </summary>
    public partial class Lookup : Window
    {
        BackgroundWorker Worker;

        public Lookup()
        {
            InitializeComponent();
        }

        public void SearchForChemical(object sender, RoutedEventArgs e)
        {
            Information.Visibility = Visibility.Hidden;
            ProgressBox.Visibility = Visibility.Visible;
            SearchButton.IsEnabled = false;

            Worker = new BackgroundWorker();
            Worker.DoWork += DoWork;
            Worker.RunWorkerCompleted += WorkDone;
            Worker.RunWorkerAsync();
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                WebClient client = new WebClient();

                string cid = client.DownloadString($"https://pubchem.ncbi.nlm.nih.gov/rest/pug/compound/name/{SearchTerm.Text}/cids/TXT");
                cid = cid.Substring(0, cid.IndexOf("\n"));
                string chemdata = client.DownloadString($"https://pubchem.ncbi.nlm.nih.gov/rest/pug_view/data/compound/{cid}/json");
                Regex regex = new Regex(@"-?\d+(\.\d+)? Â°C");

                JToken result = JObject.Parse(chemdata)["Record"];

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

                Name.Text = $"{name}";
                Structure.Text = $"{structure}";
                InChI_Identifier.Text = $"InChI Identifier: {inchi}";

                MeltingPoint.Text = $"Melting Point: {melting}";
                BoilingPoint.Text = $"Boiling Point: {boiling}";
            }, DispatcherPriority.ContextIdle);
        }

        private void WorkDone(object sender, RunWorkerCompletedEventArgs e)
        {
            Information.Visibility = Visibility.Visible;
            ProgressBox.Visibility = Visibility.Hidden;
            SearchButton.IsEnabled = true;
        }

        private static string CapitaliseSelective(Match match)
        {
            return match.Value.ToUpper();
        }
    }
}
