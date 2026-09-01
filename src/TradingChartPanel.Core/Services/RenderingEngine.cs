using System;
using System.Collections.Generic;
using TradingChartPanel.Core.Models;
using TradingChartPanel.Core.Interfaces;

namespace TradingChartPanel.Core.Services
{
    /// <summary>
    /// Coordinates all chart rendering operations.
    /// Acts as intermediary between chart state/data and the rendering engine.
    /// </summary>
    public class RenderingEngine
    {
        private readonly IRenderer _renderer;
        private readonly ViewportCalculator _viewportCalculator;
        private Viewport _currentViewport;
        private DateTime _lastRenderTime;

        public RenderingEngine(IRenderer renderer)
        {
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            _viewportCalculator = new ViewportCalculator();
        }

        /// <summary>
        /// Render the complete chart.
        /// </summary>
        public bool Render(
            OHLC[] allBars,
            ChartState state,
            double viewportWidth,
            double viewportHeight,
            List<IIndicator> indicators = null)
        {
            try
            {
                // Calculate viewport
                _currentViewport = _viewportCalculator.CalculateViewport(state, allBars, viewportWidth, viewportHeight);
                if (_currentViewport == null)
                    return false;

                // Clear render surface
                _renderer.Clear();

                // Render candlesticks
                _renderer.RenderCandlesticks(allBars, _currentViewport);

                // Render indicators if provided
                if (indicators != null)
                {
                    foreach (var indicator in indicators)
                    {
                        // TODO: Calculate indicator values and render
                        // _renderer.RenderIndicator(indicator, indicatorValues);
                    }
                }

                // Render axes
                var priceAxis = new AxisInfo
                {
                    Name = "Price",
                    Min = _currentViewport.MinPrice,
                    Max = _currentViewport.MaxPrice,
                    MajorInterval = CalculateMajorInterval(_currentViewport.PriceRange),
                    LabelFormat = "F5"
                };

                var timeAxis = new AxisInfo
                {
                    Name = "Time",
                    Min = 0,
                    Max = _currentViewport.VisibleBarCount - 1,
                    MajorInterval = CalculateTimeInterval(_currentViewport.VisibleBarCount),
                    LabelFormat = "N0"
                };

                _renderer.RenderAxes(priceAxis, timeAxis);

                // Refresh output
                _renderer.Refresh();

                _lastRenderTime = DateTime.UtcNow;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Rendering error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Render crosshair at price/bar location.
        /// </summary>
        public void RenderCrosshair(decimal price, int barIndex)
        {
            if (_currentViewport != null)
            {
                _renderer.RenderCrosshair(price, barIndex);
            }
        }

        /// <summary>
        /// Get current viewport.
        /// </summary>
        public Viewport GetCurrentViewport() => _currentViewport;

        private decimal CalculateMajorInterval(decimal range)
        {
            // Simple interval calculation - can be improved
            if (range <= 0) return 0.0001m;
            if (range < 0.01m) return 0.001m;
            if (range < 0.1m) return 0.01m;
            if (range < 1m) return 0.1m;
            if (range < 10m) return 1m;
            return 10m;
        }

        private decimal CalculateTimeInterval(int visibleBars)
        {
            // Aim for ~5-10 major ticks
            if (visibleBars < 50) return 5;
            if (visibleBars < 100) return 10;
            if (visibleBars < 500) return 50;
            return 100;
        }
    }
}
