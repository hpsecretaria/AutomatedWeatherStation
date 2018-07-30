using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AutomatedWeatherStation.Helpers.ReportViewer
{
    public class ReportViewBuilder : IDisposable
    {
        public ReportViewBuilder(string source, IReadOnlyCollection<DataSetValuePair> initialDataSource)
        {
            ReportContent = new ReportView(source, initialDataSource);
            ReportContent.Viewer.ReportRefresh += RptViewerOnReportRefresh;
        }

        public ReportView ReportContent { get; }
        public Func<IReadOnlyCollection<DataSetValuePair>> RefreshDataSOurceCallback { private get; set; }

        public void Dispose()
        {
            ReportContent.Viewer.ReportRefresh -= RptViewerOnReportRefresh;
        }

        private void RptViewerOnReportRefresh(object sender, CancelEventArgs e)
        {
            if (RefreshDataSOurceCallback == null)

                throw new InvalidOperationException(
                    "Unable to locate the methot that updates this report's data source.\n" +
                    "Use RefreshDataSourceCallback to point to your refresh data source method");
            ReportContent.UpdateDataSource(RefreshDataSOurceCallback());
        }
    }
}