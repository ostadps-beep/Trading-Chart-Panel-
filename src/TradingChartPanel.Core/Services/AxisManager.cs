using System;
using System.Collections.Generic;
using TradingChartPanel.Core.Models;

namespace TradingChartPanel.Core.Services
{
    /// <summary>
    /// Manages price and time axes.
    /// Calculates axis ranges, tick positions, and label formatting.
    /// </summary>
    public class AxisManager
    {
        /// <summary>
        /// Calculate price axis info from viewport.
        /// </summary>
        public AxisInfo CalculatePriceAxis(Viewport viewport)
        {
            if (viewport == null)
                return null;

            var majorInterval = CalculatePriceInterval(viewport.PriceRange);
            
            return new AxisInfo
            {
                Name = "Price",
                Min = viewport.MinPrice,
                Max = viewport.MaxPrice,
                MajorInterval = majorInterval,
                MinorInterval = majorInterval / 5,
                LabelFormat = "F5"
            };
        }

        /// <summary>
        /// Calculate time axis info from viewport.
        /// </summary>
        public AxisInfo CalculateTimeAxis(Viewport viewport, OHLC[] bars)
        {
            if (viewport == null || bars == null || bars.Length == 0)
                return null;

            var firstBarTime = bars[viewport.FirstVisibleBarIndex].Timestamp;
            var lastBarTime = bars[viewport.LastVisibleBarIndex].Timestamp;
            var timeRange = lastBarTime - firstBarTime;

            return new AxisInfo
            {
                Name = "Time",
                Min = 0,
                Max = viewport.VisibleBarCount - 1,
                MajorInterval = CalculateTimeTickInterval(viewport.VisibleBarCount),
                LabelFormat = "N0"
            };
        }

        /// <summary>
        /// Get price tick values for axis.
        /// </summary>
        public List<decimal> GetPriceTickValues(AxisInfo axis, int maxTicks = 10)
        {
            var ticks = new List<decimal>();
            if (axis == null || axis.MajorInterval <= 0)
                return ticks;

            decimal currentValue = Math.Ceiling(axis.Min / axis.MajorInterval) * axis.MajorInterval;
            while (currentValue <= axis.Max && ticks.Count < maxTicks)
            {
                ticks.Add(currentValue);
                currentValue += axis.MajorInterval;
            }

            return ticks;
        }

        private decimal CalculatePriceInterval(decimal range)
        {
            if (range <= 0) return 0.0001m;
            
            // Find appropriate interval (0.0001, 0.001, 0.01, 0.1, 1, 10, etc.)
            decimal interval = 0.0001m;
            while (range / interval > 20)
                interval *= 10;
            
            return interval;
        }

        private decimal CalculateTimeTickInterval(int visibleBars)
        {
            // Aim for 5-15 major ticks
            if (visibleBars < 10) return 1;
            if (visibleBars < 50) return 5;
            if (visibleBars < 100) return 10;
            if (visibleBars < 500) return 50;
            return 100;
        }
    }
}
