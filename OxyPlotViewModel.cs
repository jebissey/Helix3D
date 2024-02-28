using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;

namespace WpfAppHelix3D;

internal class OxyPlotViewModel : ObservableObject
{
    public OxyPlotViewModel()
    {
        SetGraph(TestPlotModel, "GraphTitle", "GraphXAxisTitle", "GraphYAxisTitle", "GraphLegendTitle", new List<GraphSerie>() { });

        WeakReferenceMessenger.Default.Register<List<DataPoint>>(this, (_, lines) =>
        {
            SetGraph(TestPlotModel, "GraphTitle", "GraphXAxisTitle", "GraphYAxisTitle", "GraphLegendTitle", new List<GraphSerie>()
            {
                new() { Series = lines, Title = "GraphSerieNominalTitle", Color = OxyColors.Green, SeriesType = SeriesType.lineSeries, Thickness = 6 },
                new() { Series = lines, Title = "GraphSerieMaximalTitle", Color = OxyColors.Red, SeriesType = SeriesType.scatterSeries, Thickness = 8 },
            });
        });
    }

    #region Properties
    private PlotModel testPlotModel = new();
    public PlotModel TestPlotModel
    {
        get => testPlotModel;
        private set => SetProperty(ref testPlotModel, value);
    }
    #endregion

    private static void SetGraph(PlotModel plotModel, string title, string xAxisTitle, string yAxisTitle, string legendTitle, List<GraphSerie> graphSeries)
    {
        plotModel.Title = title;

        while (plotModel.Axes.Any()) plotModel.Axes.RemoveAt(0);
        plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = xAxisTitle });
        plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = yAxisTitle });

        while (plotModel.Legends.Any()) plotModel.Legends.RemoveAt(0);
        plotModel.Legends.Add(new Legend() { LegendPosition = LegendPosition.TopRight, LegendTitle = legendTitle });


        while (plotModel.Series.Any()) plotModel.Series.RemoveAt(0);
        LineSeries lineSeries;
        ScatterSeries scatterSeries;
        foreach (GraphSerie graphSerie in graphSeries)
        {
            if (graphSerie.SeriesType == SeriesType.lineSeries)
            {
                plotModel.Series.Add(lineSeries = new LineSeries { Color = graphSerie.Color, StrokeThickness = graphSerie.Thickness, LineJoin = LineJoin.Round, Title = graphSerie.Title });
                lineSeries.Points.AddRange(graphSerie.Series);
            }
            else if (graphSerie.SeriesType == SeriesType.scatterSeries)
            {
                plotModel.Series.Add(scatterSeries = new ScatterSeries { MarkerFill = graphSerie.Color, MarkerStroke = graphSerie.Color, MarkerType = MarkerType.Circle, MarkerSize = graphSerie.Thickness, Title = graphSerie.Title });
                scatterSeries.Points.AddRange(DataPointsToScatterPoints(graphSerie.Series));
            }
            else throw new ApplicationException($"Unknow SeriesType ({graphSerie.SeriesType})");
        }

        plotModel.InvalidatePlot(true); // to fix null reference exception in oxyplot

        #region Local methods
        List<ScatterPoint> DataPointsToScatterPoints(List<DataPoint> dataPoints)
        {
            List<ScatterPoint> scatterPoints = new();
            dataPoints.ForEach(dp => scatterPoints.Add(new ScatterPoint(Math.Round(dp.X, 0), Math.Round(dp.Y, 2))));
            return scatterPoints;
        }
        #endregion
    }

    public class GraphSerie
    {
        public List<DataPoint>? Series { get; set; }
        public string? Title { get; set; }
        public OxyColor Color { get; set; }
        public SeriesType SeriesType { get; set; }
        public double Thickness { get; set; }
    }

    public enum SeriesType
    {
        lineSeries,
        scatterSeries,
    }
}
