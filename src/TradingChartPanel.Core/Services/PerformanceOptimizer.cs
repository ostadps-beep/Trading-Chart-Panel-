namespace TradingChartPanel.Core.Services
{
    /// <summary>
    /// Implements multi-level performance optimization strategies.
    /// Handles caching, level-of-detail, viewport culling.
    /// </summary>
    public class PerformanceOptimizer
    {
        /// <summary>
        /// Determine if LOD (Level of Detail) should be applied.
        /// </summary>
        public static bool ShouldApplyLOD(int visibleBarCount)
        {
            // Apply LOD when too many bars are visible (performance impact)
            return visibleBarCount > 5000;
        }

        /// <summary>
        /// Calculate LOD aggregation factor.
        /// </summary>
        public static int CalculateLODAggregationFactor(int visibleBars, int maxDesiredBars = 1000)
        {
            if (visibleBars <= maxDesiredBars)
                return 1;

            return (visibleBars + maxDesiredBars - 1) / maxDesiredBars;
        }

        /// <summary>
        /// Determine render batch size based on bar count.
        /// </summary>
        public static int CalculateRenderBatchSize(int totalBars)
        {
            if (totalBars < 1000)
                return totalBars;
            if (totalBars < 10000)
                return 1000;
            return 500;  // Process smaller batches for large datasets
        }

        /// <summary>
        /// Calculate cache size needed.
        /// </summary>
        public static int CalculateCacheSize(int barCount)
        {
            // Keep recent bars in memory, but not more than ChartConstants.MaxCachedBars
            return System.Math.Min(barCount, ChartConstants.MaxCachedBars);
        }
    }
}
