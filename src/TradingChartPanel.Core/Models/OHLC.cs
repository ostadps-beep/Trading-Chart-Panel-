using System;

namespace TradingChartPanel.Core.Models
{
    /// <summary>
    /// Represents a single candlestick (OHLC) bar with volume and timestamp.
    /// </summary>
    public class OHLC
    {
        /// <summary>
        /// Opening price
        /// </summary>
        public decimal Open { get; set; }

        /// <summary>
        /// Highest price during the period
        /// </summary>
        public decimal High { get; set; }

        /// <summary>
        /// Lowest price during the period
        /// </summary>
        public decimal Low { get; set; }

        /// <summary>
        /// Closing price
        /// </summary>
        public decimal Close { get; set; }

        /// <summary>
        /// Trading volume
        /// </summary>
        public long Volume { get; set; }

        /// <summary>
        /// Bar timestamp (UTC)
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// True if this bar represents a complete candle
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Calculate body size (Close - Open or Open - Close)
        /// </summary>
        public decimal GetBodySize() => Math.Abs(Close - Open);

        /// <summary>
        /// Get the midpoint price
        /// </summary>
        public decimal GetMidpoint() => (High + Low) / 2;

        /// <summary>
        /// Validate OHLC integrity
        /// </summary>
        public bool IsValid()
        {
            if (High < Low) return false;
            if (High < Open || High < Close) return false;
            if (Low > Open || Low > Close) return false;
            if (Volume < 0) return false;
            if (Open <= 0 || High <= 0 || Low <= 0 || Close <= 0) return false;
            return true;
        }

        public override string ToString()
        {
            return $"[{Timestamp:yyyy-MM-dd HH:mm}] O:{Open:F5} H:{High:F5} L:{Low:F5} C:{Close:F5} V:{Volume}";
        }
    }
}
