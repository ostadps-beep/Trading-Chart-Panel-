namespace TradingChartPanel.Core
{
    /// <summary>
    /// Global constants for the chart panel.
    /// </summary>
    public static class ChartConstants
    {
        // Zoom constraints
        public const double MinZoomLevel = 0.5;
        public const double MaxZoomLevel = 50.0;
        public const double DefaultZoomLevel = 5.0;

        // Price scale constraints
        public const double MinPriceScale = 0.1;
        public const double MaxPriceScale = 1000.0;
        public const double DefaultPriceScale = 10.0;

        // Performance
        public const int MaxCachedBars = 10000;
        public const int RenderBatchSize = 1000;
        public const int MaxFrameRate = 60;

        // Data
        public const int MinimumVisibleBars = 5;
        public const int RecommendedVisibleBars = 100;

        // Margins
        public const double AxisMarginLeft = 60.0;
        public const double AxisMarginRight = 20.0;
        public const double AxisMarginTop = 10.0;
        public const double AxisMarginBottom = 30.0;

        // Colors
        public const string ColorBullish = "#00FF00";
        public const string ColorBearish = "#FF0000";
        public const string ColorNeutral = "#808080";
        public const string ColorGrid = "#CCCCCC";
        public const string ColorCrosshair = "#FFFF00";
    }
}
