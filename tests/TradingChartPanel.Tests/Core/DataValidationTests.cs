using Xunit;
using TradingChartPanel.Core.Models;
using TradingChartPanel.Core.Utilities;

namespace TradingChartPanel.Tests.Core
{
    public class DataValidationTests
    {
        [Fact]
        public void ValidateOHLC_WithValidBar_ReturnsValid()
        {
            // Arrange
            var bar = new OHLC
            {
                Open = 1.2000m,
                High = 1.2050m,
                Low = 1.1950m,
                Close = 1.2010m,
                Volume = 1000,
                Timestamp = System.DateTime.UtcNow
            };

            // Act
            var result = DataValidation.ValidateOHLC(bar);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void ValidateOHLC_WithNullBar_ReturnsInvalid()
        {
            // Act
            var result = DataValidation.ValidateOHLC(null);

            // Assert
            Assert.False(result.IsValid);
            Assert.NotNull(result.ErrorMessage);
        }

        [Fact]
        public void ValidateOHLCArray_WithValidBars_ReturnsValid()
        {
            // Arrange
            var baseTime = System.DateTime.UtcNow;
            var bars = new[]
            {
                new OHLC { Open = 1.2000m, High = 1.2050m, Low = 1.1950m, Close = 1.2010m, Volume = 1000, Timestamp = baseTime },
                new OHLC { Open = 1.2010m, High = 1.2060m, Low = 1.1960m, Close = 1.2020m, Volume = 1100, Timestamp = baseTime.AddMinutes(1) },
            };

            // Act
            var result = DataValidation.ValidateOHLCArray(bars);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void ValidateOHLCArray_WithEmptyArray_ReturnsInvalid()
        {
            // Act
            var result = DataValidation.ValidateOHLCArray(new OHLC[] { });

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ValidateOHLCArray_WithTimingIssues_ReturnsInvalid()
        {
            // Arrange
            var baseTime = System.DateTime.UtcNow;
            var bars = new[]
            {
                new OHLC { Open = 1.2000m, High = 1.2050m, Low = 1.1950m, Close = 1.2010m, Volume = 1000, Timestamp = baseTime },
                new OHLC { Open = 1.2010m, High = 1.2060m, Low = 1.1960m, Close = 1.2020m, Volume = 1100, Timestamp = baseTime }, // Same time
            };

            // Act
            var result = DataValidation.ValidateOHLCArray(bars);

            // Assert
            Assert.False(result.IsValid);
        }
    }
}
