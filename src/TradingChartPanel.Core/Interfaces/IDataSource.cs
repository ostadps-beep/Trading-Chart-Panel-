using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingChartPanel.Core.Models;

namespace TradingChartPanel.Core.Interfaces
{
    /// <summary>
    /// Contract for market data sources.
    /// Any data provider (MT4, MT5, API, CSV, etc.) must implement this interface.
    /// </summary>
    public interface IDataSource
    {
        /// <summary>
        /// Get historical OHLC data for a symbol and timeframe.
        /// </summary>
        Task<IEnumerable<OHLC>> GetHistoricalDataAsync(
            Symbol symbol,
            TimeFrame timeframe,
            DateTime from,
            DateTime to);

        /// <summary>
        /// Get latest N bars for a symbol and timeframe.
        /// </summary>
        Task<IEnumerable<OHLC>> GetLatestBarsAsync(
            Symbol symbol,
            TimeFrame timeframe,
            int barCount);

        /// <summary>
        /// Subscribe to real-time tick updates for a symbol.
        /// </summary>
        IObservable<Tick> SubscribeToTicks(Symbol symbol);

        /// <summary>
        /// Subscribe to candle completion events.
        /// </summary>
        IObservable<OHLC> SubscribeToCandleCompletion(Symbol symbol, TimeFrame timeframe);

        /// <summary>
        /// Validate OHLC data integrity.
        /// </summary>
        bool ValidateData(OHLC[] data);

        /// <summary>
        /// Get list of available symbols.
        /// </summary>
        Task<IEnumerable<Symbol>> GetAvailableSymbolsAsync();

        /// <summary>
        /// Get list of available timeframes.
        /// </summary>
        Task<IEnumerable<TimeFrame>> GetAvailableTimeFramesAsync();
    }
}
