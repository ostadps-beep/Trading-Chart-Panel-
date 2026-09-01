using System;
using TradingChartPanel.Core.Models;

namespace TradingChartPanel.Core.Services
{
    /// <summary>
    /// Calculates which bars and prices are visible in the current viewport.
    /// Handles coordinate transformations between data and screen space.
    /// </summary>
    public class ViewportCalculator
    {
        /// <summary>
        /// Calculate viewport based on chart state and available bars.
        /// </summary>
        public Viewport CalculateViewport(
            ChartState state,
            OHLC[] allBars,
            double viewportWidth,
            double viewportHeight)
        {
            if (allBars == null || allBars.Length == 0)
                return null;

            var viewport = new Viewport
            {
                ViewportWidth = viewportWidth,
                ViewportHeight = viewportHeight,
                BarWidth = state.ZoomLevel
            };

            // Calculate visible bar range
            int visibleBarsCount = (int)(viewportWidth / state.ZoomLevel);
            visibleBarsCount = Math.Max(ChartConstants.MinimumVisibleBars, visibleBarsCount);

            int lastBarIndex = allBars.Length - 1 - state.PanOffset;
            int firstBarIndex = Math.Max(0, lastBarIndex - visibleBarsCount + 1);

            if (firstBarIndex >= allBars.Length)
                firstBarIndex = Math.Max(0, allBars.Length - visibleBarsCount);
            if (lastBarIndex >= allBars.Length)
                lastBarIndex = allBars.Length - 1;

            viewport.FirstVisibleBarIndex = firstBarIndex;
            viewport.LastVisibleBarIndex = lastBarIndex;

            // Calculate price range from visible bars
            decimal minPrice = decimal.MaxValue;
            decimal maxPrice = decimal.MinValue;

            for (int i = firstBarIndex; i <= lastBarIndex; i++)
            {
                if (allBars[i].Low < minPrice)
                    minPrice = allBars[i].Low;
                if (allBars[i].High > maxPrice)
                    maxPrice = allBars[i].High;
            }

            // Add margin to price range (5%)
            decimal margin = (maxPrice - minPrice) * 0.05m;
            minPrice -= margin;
            maxPrice += margin;

            viewport.MinPrice = minPrice;
            viewport.MaxPrice = maxPrice;

            // Calculate pixels per price
            if (viewport.PriceRange > 0)
            {
                viewport.PixelsPerPrice = viewportHeight / (double)viewport.PriceRange;
            }

            return viewport.IsValid() ? viewport : null;
        }

        /// <summary>
        /// Convert price value to Y-coordinate (screen space).
        /// </summary>
        public double PriceToYCoordinate(decimal price, Viewport viewport)
        {
            if (viewport?.PriceRange <= 0)
                return 0;

            double ratio = (double)(price - viewport.MinPrice) / (double)viewport.PriceRange;
            return viewport.ViewportHeight - (ratio * viewport.ViewportHeight);
        }

        /// <summary>
        /// Convert Y-coordinate to price value (data space).
        /// </summary>
        public decimal YCoordinateToPriceValue(double yCoord, Viewport viewport)
        {
            if (viewport?.PriceRange <= 0)
                return viewport?.MinPrice ?? 0;

            double ratio = 1.0 - (yCoord / viewport.ViewportHeight);
            return viewport.MinPrice + ((decimal)ratio * viewport.PriceRange);
        }

        /// <summary>
        /// Convert bar index to X-coordinate (screen space).
        /// </summary>
        public double BarIndexToXCoordinate(int barIndex, Viewport viewport, ChartState state)
        {
            if (viewport == null)
                return 0;

            int relativeIndex = barIndex - viewport.FirstVisibleBarIndex;
            return relativeIndex * viewport.BarWidth;
        }

        /// <summary>
        /// Convert X-coordinate to bar index (data space).
        /// </summary>
        public int XCoordinateToBarIndex(double xCoord, Viewport viewport)
        {
            if (viewport == null)
                return 0;

            int relativeIndex = (int)(xCoord / viewport.BarWidth);
            return viewport.FirstVisibleBarIndex + relativeIndex;
        }
    }
}
