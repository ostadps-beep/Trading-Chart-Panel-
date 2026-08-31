using System;

namespace TradingChartPanel.Core.Models
{
    /// <summary>
    /// Information about a chart axis (price or time).
    /// </summary>
    public class AxisInfo
    {
        public string Name { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public decimal MajorInterval { get; set; }
        public decimal MinorInterval { get; set; }
        public string LabelFormat { get; set; }
        public decimal Range => Max - Min;

        public override string ToString()
        {
            return $"{Name}: {Min:F5} - {Max:F5} (Major:{MajorInterval:F5})";
        }
    }
}
