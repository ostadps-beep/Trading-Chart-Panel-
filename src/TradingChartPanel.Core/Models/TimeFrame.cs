using System;
using System.Collections.Generic;
using System.Linq;

namespace TradingChartPanel.Core.Models
{
    /// <summary>
    /// Represents chart timeframes (M1, M5, H1, D1, etc.)
    /// </summary>
    public class TimeFrame
    {
        /// <summary>
        /// Display name (M1, M5, H1, etc.)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Duration in minutes
        /// </summary>
        public int Minutes { get; set; }

        /// <summary>
        /// Duration as TimeSpan
        /// </summary>
        public TimeSpan Duration => TimeSpan.FromMinutes(Minutes);

        private TimeFrame(string name, int minutes)
        {
            Name = name;
            Minutes = minutes;
        }

        // Standard timeframes
        public static readonly TimeFrame M1 = new("M1", 1);
        public static readonly TimeFrame M5 = new("M5", 5);
        public static readonly TimeFrame M15 = new("M15", 15);
        public static readonly TimeFrame M30 = new("M30", 30);
        public static readonly TimeFrame H1 = new("H1", 60);
        public static readonly TimeFrame H4 = new("H4", 240);
        public static readonly TimeFrame D1 = new("D1", 1440);
        public static readonly TimeFrame W1 = new("W1", 10080);
        public static readonly TimeFrame MN1 = new("MN1", 43200);

        public static IEnumerable<TimeFrame> GetStandardTimeFrames()
        {
            return new[] { M1, M5, M15, M30, H1, H4, D1, W1, MN1 };
        }

        public static TimeFrame Parse(string timeframeString)
        {
            var tf = GetStandardTimeFrames().FirstOrDefault(t => t.Name == timeframeString);
            if (tf == null)
                throw new ArgumentException($"Unknown timeframe: {timeframeString}");
            return tf;
        }

        public static bool TryParse(string timeframeString, out TimeFrame timeFrame)
        {
            timeFrame = GetStandardTimeFrames().FirstOrDefault(t => t.Name == timeframeString);
            return timeFrame != null;
        }

        public override string ToString() => Name;
        public override bool Equals(object obj) => obj is TimeFrame tf && tf.Minutes == Minutes;
        public override int GetHashCode() => Minutes.GetHashCode();
    }
}
