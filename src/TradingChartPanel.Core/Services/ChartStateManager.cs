using System;
using TradingChartPanel.Core.Models;

namespace TradingChartPanel.Core.Services
{
    /// <summary>
    /// Manages chart state (zoom, pan, scale, visible range).
    /// Tracks all user interactions that affect the chart view.
    /// </summary>
    public class ChartStateManager
    {
        private ChartState _state = new();

        /// <summary>
        /// Get current chart state.
        /// </summary>
        public ChartState GetState() => _state;

        /// <summary>
        /// Set current symbol and timeframe.
        /// </summary>
        public void SetSymbolAndTimeFrame(Symbol symbol, TimeFrame timeframe, int totalBars)
        {
            _state.CurrentSymbol = symbol;
            _state.CurrentTimeFrame = timeframe;
            _state.TotalBars = totalBars;
        }

        /// <summary>
        /// Zoom in (increase bar width).
        /// </summary>
        public void ZoomIn(double factor = 1.2)
        {
            _state.ZoomLevel *= factor;
            if (_state.ZoomLevel > ChartConstants.MaxZoomLevel)
                _state.ZoomLevel = ChartConstants.MaxZoomLevel;
        }

        /// <summary>
        /// Zoom out (decrease bar width).
        /// </summary>
        public void ZoomOut(double factor = 1.2)
        {
            _state.ZoomLevel /= factor;
            if (_state.ZoomLevel < ChartConstants.MinZoomLevel)
                _state.ZoomLevel = ChartConstants.MinZoomLevel;
        }

        /// <summary>
        /// Set zoom level directly.
        /// </summary>
        public void SetZoomLevel(double zoomLevel)
        {
            _state.ZoomLevel = Math.Max(ChartConstants.MinZoomLevel, 
                                        Math.Min(zoomLevel, ChartConstants.MaxZoomLevel));
        }

        /// <summary>
        /// Pan left (move backward through time).
        /// </summary>
        public void PanLeft(int barCount = 10)
        {
            _state.PanOffset += barCount;
        }

        /// <summary>
        /// Pan right (move forward through time).
        /// </summary>
        public void PanRight(int barCount = 10)
        {
            _state.PanOffset = Math.Max(0, _state.PanOffset - barCount);
        }

        /// <summary>
        /// Set pan offset directly.
        /// </summary>
        public void SetPanOffset(int offset)
        {
            _state.PanOffset = Math.Max(0, offset);
        }

        /// <summary>
        /// Scale price axis.
        /// </summary>
        public void ScalePrice(double factor)
        {
            _state.PriceScale *= factor;
            if (_state.PriceScale < ChartConstants.MinPriceScale)
                _state.PriceScale = ChartConstants.MinPriceScale;
            if (_state.PriceScale > ChartConstants.MaxPriceScale)
                _state.PriceScale = ChartConstants.MaxPriceScale;
        }

        /// <summary>
        /// Set visible bar range.
        /// </summary>
        public void SetVisibleBarRange(int firstIndex, int lastIndex)
        {
            _state.FirstVisibleBarIndex = Math.Max(0, firstIndex);
            _state.LastVisibleBarIndex = Math.Min(_state.TotalBars - 1, lastIndex);
        }

        /// <summary>
        /// Set visible price range.
        /// </summary>
        public void SetVisiblePriceRange(decimal minPrice, decimal maxPrice)
        {
            _state.MinVisiblePrice = minPrice;
            _state.MaxVisiblePrice = maxPrice;
        }

        /// <summary>
        /// Toggle auto-scroll.
        /// </summary>
        public void SetAutoScroll(bool enabled)
        {
            _state.AutoScroll = enabled;
        }

        /// <summary>
        /// Reset to default state.
        /// </summary>
        public void Reset()
        {
            _state = new ChartState();
        }
    }
}
