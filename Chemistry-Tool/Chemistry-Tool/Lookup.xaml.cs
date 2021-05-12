using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
            ProgressBar.Visibility = Visibility.Visible;
            SearchButton.IsEnabled = false;

            Worker = new BackgroundWorker();
            //Worker.ProgressChanged += ProgressChanged;
            Worker.DoWork += DoWork;
            //Worker.WorkerReportsProgress = true;
            Worker.RunWorkerCompleted += WorkDone;
            Worker.RunWorkerAsync();
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                Chemical.Metadata metadata = Chemical.GetData(SearchTerm.Text);

                Name.Text = $"{metadata.Name}";
                Structure.Text = $"{metadata.Structure}";
                InChI_Identifier.Text = $"InChI Identifier: {metadata.InChI}";

                MeltingPoint.Text = $"Melting Point: {metadata.MeltingPoint}";
                BoilingPoint.Text = $"Boiling Point: {metadata.BoilingPoint}";
            });
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
        }

        private void WorkDone(object sender, RunWorkerCompletedEventArgs e)
        {
            Information.Visibility = Visibility.Visible;
            ProgressBar.Visibility = Visibility.Hidden;
            SearchButton.IsEnabled = true;
        }
    }
}
