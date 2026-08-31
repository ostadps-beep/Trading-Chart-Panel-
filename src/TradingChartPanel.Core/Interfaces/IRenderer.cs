using TradingChartPanel.Core.Models;

namespace TradingChartPanel.Core.Interfaces
{
    /// <summary>
    /// Contract for chart rendering.
    /// </summary>
    public interface IRenderer
    {
        void RenderCandlesticks(OHLC[] data, Viewport viewport);
        void RenderIndicator(IIndicator indicator, IndicatorValue[] values);
        void RenderAxes(AxisInfo priceAxis, AxisInfo timeAxis);
        void RenderCrosshair(decimal price, double barIndex);
        void RenderPriceLine(decimal price, string color = "#808080");
        void Clear();
        void Refresh();
    }
}
