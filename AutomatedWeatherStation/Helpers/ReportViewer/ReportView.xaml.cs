using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using UserControl = System.Windows.Controls.UserControl;

namespace AutomatedWeatherStation.Helpers.ReportViewer
{
    /// <summary>
    ///     Interaction logic for ReportView.xaml
    /// </summary>
    public partial class ReportView : UserControl
    {
        private readonly IReadOnlyCollection<DataSetValuePair> _initialDataSource;

        /// <summary>
        ///     Initializes an instance of a REportView
        /// </summary>
        /// <param name="reportSource"> The source for the embedded resource of the ReportViewer</param>
        /// <param name="initialDataSource"> The data source represented by a collection of a DatasetValuePair</param>
        public ReportView(string reportSource, IReadOnlyCollection<DataSetValuePair> initialDataSource)
        {
            InitializeComponent();
            ReportSource = reportSource;
            _initialDataSource = initialDataSource;
            Viewer.LocalReport.ReportEmbeddedResource = reportSource;
        }

        public string ReportSource { get; private set; }

        private void ReportView_OnLoaded(object sender, RoutedEventArgs e)
        {
            Viewer.SetDisplayMode(DisplayMode.PrintLayout);
            Viewer.ZoomPercent = 100;
            Viewer.ZoomMode = ZoomMode.Percent;

            UpdateDataSource(_initialDataSource);
        }

        public void UpdateDataSource(IReadOnlyCollection<DataSetValuePair> sources)
        {
            Viewer.LocalReport.DataSources.Clear();
            foreach (var datasetSource in sources)
            {
                Viewer.LocalReport.DataSources.Add(CreateDataSource(datasetSource.DatasetName, datasetSource.DataSource));
            }
            Viewer.RefreshReport();
        }

        private ReportDataSource CreateDataSource(string sourceName, object datasetValue)
        {
            var dataSource = new ReportDataSource();
            var dataset = new BindingSource();
            dataset.DataSource = datasetValue;

            dataSource.Name = sourceName;
            dataSource.Value = dataset;
            return dataSource;
        }
    }
}