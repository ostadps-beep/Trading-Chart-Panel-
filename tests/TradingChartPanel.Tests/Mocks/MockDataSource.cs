using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingChartPanel.Core.Models;
using TradingChartPanel.Core.Interfaces;

namespace TradingChartPanel.Tests.Mocks
{
    /// <summary>
    /// Mock data source for testing purposes.
    /// Generates synthetic OHLC data.
    /// </summary>
    public class MockDataSource : IDataSource
    {
        private Random _random = new(42);
        private decimal _basePrice = 1.2000m;

        public async Task<IEnumerable<OHLC>> GetHistoricalDataAsync(
            Symbol symbol,
            TimeFrame timeframe,
            DateTime from,
            DateTime to)
        {
            // Simulate network delay
            await Task.Delay(100);

            var bars = new List<OHLC>();
            var currentTime = from;

            while (currentTime <= to)
            {
                bars.Add(GenerateBar(currentTime));
                currentTime = currentTime.Add(timeframe.Duration);
            }

            return bars;
        }

        public async Task<IEnumerable<OHLC>> GetLatestBarsAsync(
            Symbol symbol,
            TimeFrame timeframe,
            int barCount)
        {
            await Task.Delay(50);

            var bars = new List<OHLC>();
            var currentTime = DateTime.UtcNow;

            for (int i = 0; i < barCount; i++)
            {
                bars.Insert(0, GenerateBar(currentTime));
                currentTime = currentTime.Subtract(timeframe.Duration);
            }

            return bars;
        }

        public IObservable<Tick> SubscribeToTicks(Symbol symbol)
        {
            return System.Reactive.Linq.Observable.Interval(TimeSpan.FromMilliseconds(100))
                .Select(_ => new Tick
                {
                    Bid = _basePrice - 0.0001m,
                    Ask = _basePrice + 0.0001m,
                    Volume = 100,
                    Timestamp = DateTime.UtcNow
                });
        }

        public IObservable<OHLC> SubscribeToCandleCompletion(Symbol symbol, TimeFrame timeframe)
        {
            return System.Reactive.Linq.Observable.Interval(timeframe.Duration)
                .Select(_ => GenerateBar(DateTime.UtcNow));
        }

        public bool ValidateData(OHLC[] data)
        {
            if (data == null || data.Length == 0)
                return false;

            foreach (var bar in data)
            {
                if (!bar.IsValid())
                    return false;
            }

            return true;
        }

        public async Task<IEnumerable<Symbol>> GetAvailableSymbolsAsync()
        {
            await Task.Delay(10);
            return new[]
            {
                new Symbol("EURUSD", 5, "Euro vs US Dollar"),
                new Symbol("GBPUSD", 5, "British Pound vs US Dollar"),
                new Symbol("USDJPY", 3, "US Dollar vs Japanese Yen")
            };
        }

        public async Task<IEnumerable<TimeFrame>> GetAvailableTimeFramesAsync()
        {
            await Task.Delay(10);
            return TimeFrame.GetStandardTimeFrames();
        }

        private OHLC GenerateBar(DateTime timestamp)
        {
            decimal open = _basePrice + (decimal)(_random.NextDouble() - 0.5) * 0.0010m;
            decimal close = open + (decimal)(_random.NextDouble() - 0.5) * 0.0010m;
            decimal high = Math.Max(open, close) + (decimal)_random.NextDouble() * 0.0005m;
            decimal low = Math.Min(open, close) - (decimal)_random.NextDouble() * 0.0005m;
            long volume = (long)(_random.Next(1000, 10000));

            _basePrice = close;

            return new OHLC
            {
                Open = open,
                High = high,
                Low = low,
                Close = close,
                Volume = volume,
                Timestamp = timestamp,
                IsComplete = true
            };
        }
    }
}
