using System;
using System.Collections.Generic;
using System.Linq;
using TradingChartPanel.Core.Models;
using TradingChartPanel.Core.Interfaces;

namespace TradingChartPanel.Core.Services
{
    /// <summary>
    /// Manages technical indicators.
    /// Loads, computes, and caches indicator values.
    /// </summary>
    public class IndicatorManager
    {
        private Dictionary<string, IIndicator> _indicators = new();
        private Dictionary<string, List<IndicatorValue>> _cachedValues = new();

        /// <summary>
        /// Register an indicator.
        /// </summary>
        public void RegisterIndicator(IIndicator indicator)
        {
            if (indicator == null)
                throw new ArgumentNullException(nameof(indicator));
            _indicators[indicator.Name] = indicator;
        }

        /// <summary>
        /// Unregister an indicator.
        /// </summary>
        public void UnregisterIndicator(string indicatorName)
        {
            _indicators.Remove(indicatorName);
            _cachedValues.Remove(indicatorName);
        }

        /// <summary>
        /// Compute indicator values for OHLC data.
        /// </summary>
        public List<IndicatorValue> ComputeIndicator(string indicatorName, OHLC[] data)
        {
            if (!_indicators.TryGetValue(indicatorName, out var indicator))
                return new List<IndicatorValue>();

            try
            {
                var values = indicator.Calculate(data).ToList();
                _cachedValues[indicatorName] = values;
                return values;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error computing indicator {indicatorName}: {ex.Message}");
                return new List<IndicatorValue>();
            }
        }

        /// <summary>
        /// Get cached indicator values.
        /// </summary>
        public List<IndicatorValue> GetCachedValues(string indicatorName)
        {
            return _cachedValues.TryGetValue(indicatorName, out var values) ? values : new List<IndicatorValue>();
        }

        /// <summary>
        /// Get all registered indicators.
        /// </summary>
        public IEnumerable<IIndicator> GetAllIndicators() => _indicators.Values;

        /// <summary>
        /// Get indicator by name.
        /// </summary>
        public IIndicator GetIndicator(string indicatorName)
        {
            return _indicators.TryGetValue(indicatorName, out var indicator) ? indicator : null;
        }
    }
}
