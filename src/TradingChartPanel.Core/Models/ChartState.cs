using System;

namespace TradingChartPanel.Core.Models
{
    /// <summary>
    /// Represents the current state of the chart (zoom, pan, scale).
    /// </summary>
    public class ChartState
    {
        public double ZoomLevel { get; set; } = 5.0;
        public int PanOffset { get; set; } = 0;
        public double PriceScale { get; set; } = 10.0;
        public double PriceOffset { get; set; } = 0.0;
        public bool AutoScroll { get; set; } = true;
        public Symbol CurrentSymbol { get; set; }
        public TimeFrame CurrentTimeFrame { get; set; }
        public int TotalBars { get; set; }
        public int FirstVisibleBarIndex { get; set; }
        public int LastVisibleBarIndex { get; set; }
        public decimal MaxVisiblePrice { get; set; }
        public decimal MinVisiblePrice { get; set; }

        public override string ToString()
        {
            return $"Symbol:{CurrentSymbol?.Code}, TF:{CurrentTimeFrame?.Name}, Zoom:{ZoomLevel:F1}, Pan:{PanOffset}, Bars:{TotalBars}";
        }
    }
}
