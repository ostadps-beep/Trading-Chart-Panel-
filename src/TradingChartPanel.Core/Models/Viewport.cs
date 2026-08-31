using System;

namespace TradingChartPanel.Core.Models
{
    /// <summary>
    /// Represents calculations for the visible viewport of the chart.
    /// </summary>
    public class Viewport
    {
        public int FirstVisibleBarIndex { get; set; }
        public int LastVisibleBarIndex { get; set; }
        public int VisibleBarCount => LastVisibleBarIndex - FirstVisibleBarIndex + 1;
        public double BarWidth { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
        public decimal PriceRange => MaxPrice - MinPrice;
        public double PixelsPerPrice { get; set; }
        public double ViewportWidth { get; set; }
        public double ViewportHeight { get; set; }

        public bool IsValid()
        {
            return FirstVisibleBarIndex >= 0 &&
                   LastVisibleBarIndex >= FirstVisibleBarIndex &&
                   MaxPrice > MinPrice &&
                   BarWidth > 0 &&
                   PixelsPerPrice > 0 &&
                   ViewportWidth > 0 &&
                   ViewportHeight > 0;
        }

        public override string ToString()
        {
            return $"Bars:{FirstVisibleBarIndex}-{LastVisibleBarIndex} ({VisibleBarCount}), Price:{MinPrice:F5}-{MaxPrice:F5}, BarWidth:{BarWidth:F1}px";
        }
    }
}
