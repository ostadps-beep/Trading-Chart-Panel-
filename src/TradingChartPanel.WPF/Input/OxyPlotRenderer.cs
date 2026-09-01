using System;
using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using OxyPlot.Series;
using TradingChartPanel.Core.Interfaces;
using TradingChartPanel.Core.Models;

namespace TradingChartPanel.WPF.Input
{
    /// <summary>
    /// OxyPlot renderer implementation.
    /// Converts chart data to OxyPlot rendering commands.
    /// </summary>
    public class OxyPlotRenderer : IRenderer
    {
        private PlotModel _plotModel;

        public OxyPlotRenderer()
        {
            CreatePlotModel();
        }

        /// <summary>
        /// Clear the plot model.
        /// </summary>
        public void Clear()
        {
            _plotModel?.Series.Clear();
            _plotModel?.Annotations.Clear();
        }

        /// <summary>
        /// Refresh the plot model.
        /// </summary>
        public void Refresh()
        {
            _plotModel?.InvalidatePlot(true);
        }

        /// <summary>
        /// Render candlesticks.
        /// </summary>
        public void RenderCandlesticks(OHLC[] data, Viewport viewport)
        {
            if (data == null || data.Length == 0 || viewport == null)
                return;

            Clear();

            var candleSeries = new CandleStickSeries
            {
                IncreasingColor = OxyColor.FromRgb(0, 200, 0),    // Green (bullish)
                DecreasingColor = OxyColor.FromRgb(255, 0, 0),    // Red (bearish)
                StrokeThickness = 1
            };

            // Add visible bars to series
            for (int i = viewport.FirstVisibleBarIndex; i <= viewport.LastVisibleBarIndex; i++)
            {
                if (i >= 0 && i < data.Length)
                {
                    var bar = data[i];
                    var item = new HighLowItem
                    {
                        X = i,
                        High = (double)bar.High,
                        Low = (double)bar.Low,
                        Open = (double)bar.Open,
                        Close = (double)bar.Close
                    };
                    candleSeries.Items.Add(item);
                }
            }

            _plotModel.Series.Add(candleSeries);

            // Update axes
            _plotModel.DefaultXAxis.Minimum = viewport.FirstVisibleBarIndex - 0.5;
            _plotModel.DefaultXAxis.Maximum = viewport.LastVisibleBarIndex + 0.5;
            _plotModel.DefaultYAxis.Minimum = (double)viewport.MinPrice;
            _plotModel.DefaultYAxis.Maximum = (double)viewport.MaxPrice;

            Refresh();
        }

        /// <summary>
        /// Render indicator.
        /// </summary>
        public void RenderIndicator(IIndicator indicator, IndicatorValue[] values)
        {
            if (indicator == null || values == null || values.Length == 0)
                return;

            var lineSeries = new LineSeries
            {
                Title = indicator.Name,
                Color = OxyColor.FromArgb(255, 255, 0, 0),
                StrokeThickness = 2
            };

            for (int i = 0; i < values.Length; i++)
            {
                lineSeries.Points.Add(new DataPoint(i, (double)values[i].Value));
            }

            _plotModel.Series.Add(lineSeries);
            Refresh();
        }

        /// <summary>
        /// Render axes.
        /// </summary>
        public void RenderAxes(AxisInfo priceAxis, AxisInfo timeAxis)
        {
            if (priceAxis != null)
            {
                _plotModel.DefaultYAxis.Title = priceAxis.Name;
                _plotModel.DefaultYAxis.MajorStep = (double)priceAxis.MajorInterval;
            }

            if (timeAxis != null)
            {
                _plotModel.DefaultXAxis.Title = timeAxis.Name;
                _plotModel.DefaultXAxis.MajorStep = (double)timeAxis.MajorInterval;
            }

            Refresh();
        }

        /// <summary>
        /// Render crosshair.
        /// </summary>
        public void RenderCrosshair(decimal price, double barIndex)
        {
            // TODO: Add line annotations for crosshair
            // var verticalLine = new LineAnnotation { Type = LineAnnotationType.Vertical, X = barIndex };
            // var horizontalLine = new LineAnnotation { Type = LineAnnotationType.Horizontal, Value = (double)price };
            // _plotModel.Annotations.Add(verticalLine);
            // _plotModel.Annotations.Add(horizontalLine);
            // Refresh();
        }

        /// <summary>
        /// Render price line.
        /// </summary>
        public void RenderPriceLine(decimal price, string color = "#808080")
        {
            // TODO: Implement price line rendering
        }

        /// <summary>
        /// Get the plot model for display.
        /// </summary>
        public PlotModel GetPlotModel() => _plotModel;

        private void CreatePlotModel()
        {
            _plotModel = new PlotModel
            {
                Title = "Financial Chart",
                Background = OxyColor.FromRgb(255, 255, 255),
                PlotAreaBackground = OxyColor.FromRgb(245, 245, 245),
                PlotAreaBorderColor = OxyColor.FromRgb(200, 200, 200),
                PlotAreaBorderThickness = new OxyThickness(1)
            };

            // Configure X-axis (Time)
            var xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Bar Index",
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColor.FromArgb(40, 0, 0, 0),
                MinorGridlineStyle = LineStyle.Dot,
                MinorGridlineColor = OxyColor.FromArgb(20, 0, 0, 0)
            };

            // Configure Y-axis (Price)
            var yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Price",
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColor.FromArgb(40, 0, 0, 0),
                MinorGridlineStyle = LineStyle.Dot,
                MinorGridlineColor = OxyColor.FromArgb(20, 0, 0, 0)
            };

            _plotModel.Axes.Add(xAxis);
            _plotModel.Axes.Add(yAxis);
        }
    }
}
