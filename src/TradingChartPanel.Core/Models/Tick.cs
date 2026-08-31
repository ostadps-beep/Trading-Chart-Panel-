using System;

namespace TradingChartPanel.Core.Models
{
    /// <summary>
    /// Represents a single tick (price update) for real-time data.
    /// </summary>
    public class Tick
    {
        /// <summary>
        /// Bid price (buy price)
        /// </summary>
        public decimal Bid { get; set; }

        /// <summary>
        /// Ask price (sell price)
        /// </summary>
        public decimal Ask { get; set; }

        /// <summary>
        /// Tick volume
        /// </summary>
        public long Volume { get; set; }

        /// <summary>
        /// Tick timestamp (UTC)
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Get the midpoint (Bid + Ask) / 2
        /// </summary>
        public decimal GetMidpoint() => (Bid + Ask) / 2;

        /// <summary>
        /// Get the spread (Ask - Bid)
        /// </summary>
        public decimal GetSpread() => Ask - Bid;

        /// <summary>
        /// Validate tick integrity
        /// </summary>
        public bool IsValid()
        {
            if (Ask < Bid) return false;
            if (Bid <= 0 || Ask <= 0) return false;
            if (Volume < 0) return false;
            return true;
        }

        public override string ToString()
        {
            return $"[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] Bid:{Bid:F5} Ask:{Ask:F5} V:{Volume}";
        }
    }
}
