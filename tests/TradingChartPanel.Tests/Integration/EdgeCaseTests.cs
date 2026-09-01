using System;
using System.Threading.Tasks;
using Xunit;
using TradingChartPanel.Core.Models;
using TradingChartPanel.Core.Services;
using TradingChartPanel.Core.Utilities;
using TradingChartPanel.Tests.Mocks;

namespace TradingChartPanel.Tests.Integration
{
    /// <summary>
    /// Edge case and error handling tests.
    /// </summary>
    public class EdgeCaseTests
    {
        [Fact]
        public async Task EdgeCase_EmptyDataset_HandledGracefully()
        {
            // Arrange
            var dataSource = new MockDataSource();
            var dataManager = new DataManager(dataSource);

            // Act
            var count = dataManager.GetBarCount();
            var bar = dataManager.GetLatestBar();
            var allBars = dataManager.GetAllBars();

            // Assert - Should handle empty state gracefully
            Assert.Equal(0, count);
            Assert.Null(bar);
            Assert.Empty(allBars);
        }

        [Fact]
        public async Task EdgeCase_InvalidBarIndex_ReturnsNull()
        {
            // Arrange
            var dataSource = new MockDataSource();
            var dataManager = new DataManager(dataSource);
            var symbol = new Symbol("EURUSD");
            var timeframe = TimeFrame.M5;
            await dataManager.LoadLatestBarsAsync(symbol, timeframe, 50);

            // Act
            var barBefore = dataManager.GetBarAt(-1);      // Before start
            var barAfter = dataManager.GetBarAt(1000);      // After end

            // Assert
            Assert.Null(barBefore);
            Assert.Null(barAfter);
        }

        [Fact]
        public void EdgeCase_ZoomLevel_Clamped()
        {
            // Arrange
            var manager = new ChartStateManager();

            // Act - Try to set zoom beyond maximum
            manager.SetZoomLevel(1000);

            // Assert - Should be clamped to max
            Assert.Equal(ChartConstants.MaxZoomLevel, manager.GetState().ZoomLevel);

            // Act - Try to set zoom below minimum
            manager.SetZoomLevel(0);

            // Assert - Should be clamped to min
            Assert.Equal(ChartConstants.MinZoomLevel, manager.GetState().ZoomLevel);
        }

        [Fact]
        public void EdgeCase_PanOffset_NeverNegative()
        {
            // Arrange
            var manager = new ChartStateManager();

            // Act - Try to pan beyond beginning
            manager.PanRight(100);

            // Assert - Should not go below 0
            Assert.True(manager.GetState().PanOffset >= 0);
        }

        [Fact]
        public void EdgeCase_ViewportWithSingleBar()
        {
            // Arrange
            var bars = new OHLC[]
            {
                new OHLC
                {
                    Open = 1.2000m,
                    High = 1.2050m,
                    Low = 1.1950m,
                    Close = 1.2010m,
                    Volume = 1000,
                    Timestamp = DateTime.UtcNow,
                    IsComplete = true
                }
            };

            var state = new ChartState { ZoomLevel = 5.0, TotalBars = 1 };
            var calculator = new ViewportCalculator();

            // Act
            var viewport = calculator.CalculateViewport(state, bars, 800, 600);

            // Assert - Should handle single bar
            Assert.NotNull(viewport);
            Assert.True(viewport.IsValid());
            Assert.Equal(1, viewport.VisibleBarCount);
        }

        [Fact]
        public void EdgeCase_PriceRangeWithIdenticalValues()
        {
            // Arrange
            var bars = new OHLC[]
            {
                new OHLC { Open = 1.2000m, High = 1.2000m, Low = 1.2000m, Close = 1.2000m, Volume = 0, Timestamp = DateTime.UtcNow, IsComplete = true },
                new OHLC { Open = 1.2000m, High = 1.2000m, Low = 1.2000m, Close = 1.2000m, Volume = 0, Timestamp = DateTime.UtcNow.AddMinutes(1), IsComplete = true }
            };

            var state = new ChartState { ZoomLevel = 5.0, TotalBars = 2 };
            var calculator = new ViewportCalculator();

            // Act
            var viewport = calculator.CalculateViewport(state, bars, 800, 600);

            // Assert - Should still be valid even with zero price range
            Assert.NotNull(viewport);
            // The viewport should have added margin
            Assert.True(viewport.MaxPrice > viewport.MinPrice);
        }

        [Fact]
        public async Task EdgeCase_SymbolWithZeroDigits_Handled()
        {
            // Arrange
            var symbol = new Symbol("TEST", digits: 0, description: "Test Symbol");

            // Act & Assert
            Assert.Equal("TEST", symbol.Code);
            Assert.Equal(0, symbol.Digits);
        }

        [Fact]
        public void EdgeCase_DataValidation_NullArray_Rejected()
        {
            // Act
            var result = DataValidation.ValidateOHLCArray(null);

            // Assert
            Assert.False(result.IsValid);
            Assert.NotNull(result.ErrorMessage);
        }
    }
}
