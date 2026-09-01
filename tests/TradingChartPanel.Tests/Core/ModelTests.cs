using Xunit;
using TradingChartPanel.Core.Models;
using TradingChartPanel.Core.Utilities;

namespace TradingChartPanel.Tests.Core
{
    public class OHLCModelTests
    {
        [Fact]
        public void OHLC_IsValid_WithValidData_ReturnsTrue()
        {
            // Arrange
            var ohlc = new OHLC
            {
                Open = 1.2000m,
                High = 1.2050m,
                Low = 1.1950m,
                Close = 1.2010m,
                Volume = 1000,
                Timestamp = System.DateTime.UtcNow,
                IsComplete = true
            };

            // Act
            var result = ohlc.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void OHLC_IsValid_WithHighLessThanLow_ReturnsFalse()
        {
            // Arrange
            var ohlc = new OHLC
            {
                Open = 1.2000m,
                High = 1.1950m,  // Less than Low
                Low = 1.2050m,
                Close = 1.2010m,
                Volume = 1000
            };

            // Act
            var result = ohlc.IsValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void OHLC_GetBodySize_ReturnsAbsoluteDifference()
        {
            // Arrange
            var ohlc = new OHLC
            {
                Open = 1.2000m,
                Close = 1.2100m,
                High = 1.2150m,
                Low = 1.1950m
            };

            // Act
            var bodySize = ohlc.GetBodySize();

            // Assert
            Assert.Equal(0.0100m, bodySize);
        }

        [Fact]
        public void OHLC_GetMidpoint_ReturnsAverageOfHighAndLow()
        {
            // Arrange
            var ohlc = new OHLC
            {
                High = 1.2100m,
                Low = 1.1900m
            };

            // Act
            var midpoint = ohlc.GetMidpoint();

            // Assert
            Assert.Equal(1.2000m, midpoint);
        }
    }

    public class TickModelTests
    {
        [Fact]
        public void Tick_IsValid_WithValidData_ReturnsTrue()
        {
            // Arrange
            var tick = new Tick
            {
                Bid = 1.2000m,
                Ask = 1.2010m,
                Volume = 100,
                Timestamp = System.DateTime.UtcNow
            };

            // Act
            var result = tick.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Tick_IsValid_WithAskLessThanBid_ReturnsFalse()
        {
            // Arrange
            var tick = new Tick
            {
                Bid = 1.2010m,
                Ask = 1.2000m,  // Less than Bid
                Volume = 100
            };

            // Act
            var result = tick.IsValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Tick_GetSpread_ReturnsAskMinusBid()
        {
            // Arrange
            var tick = new Tick
            {
                Bid = 1.2000m,
                Ask = 1.2010m
            };

            // Act
            var spread = tick.GetSpread();

            // Assert
            Assert.Equal(0.0010m, spread);
        }
    }

    public class TimeFrameTests
    {
        [Fact]
        public void TimeFrame_Parse_ValidString_ReturnsTimeFrame()
        {
            // Act
            var tf = TimeFrame.Parse("M5");

            // Assert
            Assert.NotNull(tf);
            Assert.Equal("M5", tf.Name);
            Assert.Equal(5, tf.Minutes);
        }

        [Fact]
        public void TimeFrame_Parse_InvalidString_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<System.ArgumentException>(() => TimeFrame.Parse("INVALID"));
        }

        [Fact]
        public void TimeFrame_TryParse_ValidString_ReturnsTrue()
        {
            // Act
            var result = TimeFrame.TryParse("H1", out var tf);

            // Assert
            Assert.True(result);
            Assert.NotNull(tf);
            Assert.Equal("H1", tf.Name);
        }

        [Fact]
        public void TimeFrame_GetStandardTimeFrames_ReturnsNineTimeframes()
        {
            // Act
            var timeframes = TimeFrame.GetStandardTimeFrames();

            // Assert
            Assert.Equal(9, System.Linq.Enumerable.Count(timeframes));
        }
    }

    public class SymbolTests
    {
        [Fact]
        public void Symbol_Constructor_WithValidCode_CreatesSymbol()
        {
            // Act
            var symbol = new Symbol("EURUSD", 5, "Euro vs USD");

            // Assert
            Assert.Equal("EURUSD", symbol.Code);
            Assert.Equal(5, symbol.Digits);
            Assert.Equal("Euro vs USD", symbol.Description);
        }

        [Fact]
        public void Symbol_Equality_SameCode_ReturnsTrue()
        {
            // Arrange
            var symbol1 = new Symbol("EURUSD");
            var symbol2 = new Symbol("EURUSD");

            // Act & Assert
            Assert.Equal(symbol1, symbol2);
        }

        [Fact]
        public void Symbol_Equality_DifferentCode_ReturnsFalse()
        {
            // Arrange
            var symbol1 = new Symbol("EURUSD");
            var symbol2 = new Symbol("GBPUSD");

            // Act & Assert
            Assert.NotEqual(symbol1, symbol2);
        }
    }
}
