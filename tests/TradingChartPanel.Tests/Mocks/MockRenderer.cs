using TradingChartPanel.Core.Models;
using TradingChartPanel.Core.Interfaces;
using System.Collections.Generic;

namespace TradingChartPanel.Tests.Mocks
{
    /// <summary>
    /// Mock renderer for testing rendering logic without WPF dependencies.
    /// </summary>
    public class MockRenderer : IRenderer
    {
        public List<string> RenderCalls { get; } = new();

        public void Clear()
        {
            RenderCalls.Add("Clear");
        }

        public void Refresh()
        {
            RenderCalls.Add("Refresh");
        }

        public void RenderAxes(AxisInfo priceAxis, AxisInfo timeAxis)
        {
            RenderCalls.Add($"RenderAxes: Price({priceAxis.Min}-{priceAxis.Max}), Time({timeAxis.Min}-{timeAxis.Max})");
        }

        public void RenderCandlesticks(OHLC[] data, Viewport viewport)
        {
            RenderCalls.Add($"RenderCandlesticks: {data.Length} bars, Viewport({viewport.FirstVisibleBarIndex}-{viewport.LastVisibleBarIndex})");
        }

        public void RenderCrosshair(decimal price, double barIndex)
        {
            RenderCalls.Add($"RenderCrosshair: Price={price}, BarIndex={barIndex}");
        }

        public void RenderIndicator(IIndicator indicator, IndicatorValue[] values)
        {
            RenderCalls.Add($"RenderIndicator: {indicator.Name}, {values.Length} values");
        }

        public void RenderPriceLine(decimal price, string color = "#808080")
        {
            RenderCalls.Add($"RenderPriceLine: Price={price}, Color={color}");
        }
    }
}
