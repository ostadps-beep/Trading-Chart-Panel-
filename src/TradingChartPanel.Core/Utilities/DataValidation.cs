using System;
using System.Collections.Generic;
using System.Linq;
using TradingChartPanel.Core.Models;

namespace TradingChartPanel.Core.Utilities
{
    /// <summary>
    /// Validates OHLC and Tick data integrity.
    /// </summary>
    public static class DataValidation
    {
        public static ValidationResult ValidateOHLC(OHLC bar)
        {
            if (bar == null)
                return ValidationResult.Invalid("OHLC bar is null");
            if (!bar.IsValid())
                return ValidationResult.Invalid("OHLC values invalid (High < Low, etc.)");
            return ValidationResult.Valid();
        }

        public static ValidationResult ValidateOHLCArray(OHLC[] bars)
        {
            if (bars == null || bars.Length == 0)
                return ValidationResult.Invalid("OHLC array is null or empty");

            for (int i = 0; i < bars.Length; i++)
            {
                var result = ValidateOHLC(bars[i]);
                if (!result.IsValid)
                    return ValidationResult.Invalid($"Bar {i}: {result.ErrorMessage}");
            }

            for (int i = 1; i < bars.Length; i++)
            {
                if (bars[i].Timestamp <= bars[i - 1].Timestamp)
                    return ValidationResult.Invalid($"Time ordering violation at bar {i}");
            }

            var timestamps = new HashSet<DateTime>();
            foreach (var bar in bars)
            {
                if (!timestamps.Add(bar.Timestamp))
                    return ValidationResult.Invalid($"Duplicate timestamp: {bar.Timestamp}");
            }

            return ValidationResult.Valid();
        }

        public static ValidationResult ValidateTick(Tick tick)
        {
            if (tick == null)
                return ValidationResult.Invalid("Tick is null");
            if (!tick.IsValid())
                return ValidationResult.Invalid("Tick values invalid (Ask < Bid, etc.)");
            return ValidationResult.Valid();
        }

        public static ValidationResult ValidateDataContinuity(OHLC[] bars, TimeFrame expectedTimeframe)
        {
            if (bars == null || bars.Length < 2)
                return ValidationResult.Valid();

            var expectedInterval = expectedTimeframe.Duration;
            for (int i = 1; i < bars.Length; i++)
            {
                var actualInterval = bars[i].Timestamp - bars[i - 1].Timestamp;
                if (actualInterval != expectedInterval)
                {
                    return ValidationResult.Invalid(
                        $"Gap in data at bar {i}: expected interval {expectedInterval}, got {actualInterval}");
                }
            }

            return ValidationResult.Valid();
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; private set; }
        public string ErrorMessage { get; private set; }

        public static ValidationResult Valid() => new() { IsValid = true, ErrorMessage = null };
        public static ValidationResult Invalid(string errorMessage) => new() { IsValid = false, ErrorMessage = errorMessage };
        public override string ToString() => IsValid ? "Valid" : $"Invalid: {ErrorMessage}";
    }
}
