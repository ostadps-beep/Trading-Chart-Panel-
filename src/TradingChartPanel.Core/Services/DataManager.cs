using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingChartPanel.Core.Models;
using TradingChartPanel.Core.Interfaces;
using TradingChartPanel.Core.Utilities;

namespace TradingChartPanel.Core.Services
{
    /// <summary>
    /// Manages loading, validation, caching, and retrieval of OHLC market data.
    /// Acts as the central data hub between IDataSource and chart rendering.
    /// </summary>
    public class DataManager
    {
        private readonly IDataSource _dataSource;
        private List<OHLC> _cachedBars = new();
        private Symbol _currentSymbol;
        private TimeFrame _currentTimeFrame;
        private DateTime _cacheLoadedAt;

        public DataManager(IDataSource dataSource)
        {
            _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        }

        /// <summary>
        /// Load historical data from data source.
        /// </summary>
        public async Task<bool> LoadHistoricalDataAsync(Symbol symbol, TimeFrame timeframe, DateTime from, DateTime to)
        {
            try
            {
                _currentSymbol = symbol;
                _currentTimeFrame = timeframe;

                var data = await _dataSource.GetHistoricalDataAsync(symbol, timeframe, from, to);
                var ohlcArray = data.ToArray();

                // Validate data
                var validationResult = DataValidation.ValidateOHLCArray(ohlcArray);
                if (!validationResult.IsValid)
                {
                    Console.WriteLine($"Data validation failed: {validationResult}");
                    return false;
                }

                _cachedBars = ohlcArray.ToList();
                _cacheLoadedAt = DateTime.UtcNow;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading historical data: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Load latest N bars (typical for chart initialization).
        /// </summary>
        public async Task<bool> LoadLatestBarsAsync(Symbol symbol, TimeFrame timeframe, int barCount)
        {
            try
            {
                _currentSymbol = symbol;
                _currentTimeFrame = timeframe;

                var data = await _dataSource.GetLatestBarsAsync(symbol, timeframe, barCount);
                var ohlcArray = data.ToArray();

                var validationResult = DataValidation.ValidateOHLCArray(ohlcArray);
                if (!validationResult.IsValid)
                {
                    Console.WriteLine($"Data validation failed: {validationResult}");
                    return false;
                }

                _cachedBars = ohlcArray.ToList();
                _cacheLoadedAt = DateTime.UtcNow;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading latest bars: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Get bars within a specific range.
        /// </summary>
        public IEnumerable<OHLC> GetBarsInRange(int startIndex, int endIndex)
        {
            if (startIndex < 0 || endIndex >= _cachedBars.Count)
                return Enumerable.Empty<OHLC>();

            return _cachedBars.Skip(startIndex).Take(endIndex - startIndex + 1);
        }

        /// <summary>
        /// Get all cached bars.
        /// </summary>
        public IEnumerable<OHLC> GetAllBars() => _cachedBars.AsReadOnly();

        /// <summary>
        /// Get bar by index.
        /// </summary>
        public OHLC GetBarAt(int index)
        {
            if (index < 0 || index >= _cachedBars.Count)
                return null;
            return _cachedBars[index];
        }

        /// <summary>
        /// Get the latest bar.
        /// </summary>
        public OHLC GetLatestBar() => _cachedBars.Count > 0 ? _cachedBars[_cachedBars.Count - 1] : null;

        /// <summary>
        /// Get total number of cached bars.
        /// </summary>
        public int GetBarCount() => _cachedBars.Count;

        /// <summary>
        /// Clear cache.
        /// </summary>
        public void ClearCache()
        {
            _cachedBars.Clear();
            _currentSymbol = null;
            _currentTimeFrame = null;
        }

        /// <summary>
        /// Check if data is cached.
        /// </summary>
        public bool HasCachedData => _cachedBars.Count > 0;

        /// <summary>
        /// Get cache info.
        /// </summary>
        public string GetCacheInfo()
        {
            return $"Symbol: {_currentSymbol?.Code}, TF: {_currentTimeFrame?.Name}, Bars: {_cachedBars.Count}, Loaded: {_cacheLoadedAt:yyyy-MM-dd HH:mm:ss}";
        }
    }
}
