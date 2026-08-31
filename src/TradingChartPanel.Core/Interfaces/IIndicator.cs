using System.Collections.Generic;
using TradingChartPanel.Core.Models;

namespace TradingChartPanel.Core.Interfaces
{
    /// <summary>
    /// Represents the visual properties of an indicator.
    /// </summary>
    public class IndicatorVisuals
    {
        public string LineColor { get; set; } = "#FF0000";
        public int LineWidth { get; set; } = 1;
        public string FillColor { get; set; }
        public bool IsHistogram { get; set; } = false;
    }

    /// <summary>
    /// Represents a calculated indicator value.
    /// </summary>
    public class IndicatorValue
    {
        public decimal Value { get; set; }
        public decimal? Value2 { get; set; }
        public decimal? Value3 { get; set; }
    }

    /// <summary>
    /// Indicator type classification.
    /// </summary>
    public enum IndicatorType
    {
        Overlay,
        Separate,
        Histogram
    }

    /// <summary>
    /// Contract for technical indicators.
    /// </summary>
    public interface IIndicator
    {
        string Name { get; }
        IndicatorType Type { get; }
        IEnumerable<IndicatorValue> Calculate(OHLC[] data);
        IndicatorValue CalculateIncremental(OHLC candle, IndicatorValue previous);
        IndicatorVisuals GetVisuals();
    }
}
