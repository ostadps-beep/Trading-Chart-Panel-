using System;

namespace TradingChartPanel.Core.Models
{
    /// <summary>
    /// Represents a trading instrument (EURUSD, GBPUSD, etc.)
    /// </summary>
    public class Symbol : IEquatable<Symbol>
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int Digits { get; set; }

        public Symbol(string code, int digits = 5, string description = null)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Digits = digits;
            Description = description ?? code;
        }

        public bool Equals(Symbol other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Code == other.Code;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Symbol)obj);
        }

        public override int GetHashCode() => Code?.GetHashCode() ?? 0;
        public override string ToString() => Code;
    }
}
