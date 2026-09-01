using System;
using System.Threading.Tasks;
using Xunit;
using TradingChartPanel.Core.Models;
using TradingChartPanel.Core.Services;
using TradingChartPanel.Tests.Mocks;

namespace TradingChartPanel.Tests.Integration
{
    /// <summary>
    /// Performance and stress tests for large datasets.
    /// </summary>
    public class PerformanceTests
    {
        [Fact]
        public async Task Performance_LoadLargeDataset_CompletsQuickly()
        {
            // Arrange
            var dataSource = new MockDataSource();
            var dataManager = new DataManager(dataSource);
            var symbol = new Symbol("EURUSD");
            var timeframe = TimeFrame.M1;
            const int largeSize = 10000; // 10,000 bars

            var startTime = DateTime.Now;

            // Act
            var success = await dataManager.LoadLatestBarsAsync(symbol, timeframe, largeSize);

            var elapsed = DateTime.Now - startTime;

            // Assert
            Assert.True(success);
            Assert.Equal(largeSize, dataManager.GetBarCount());
            Assert.True(elapsed.TotalSeconds < 5, $"Loading {largeSize} bars took {elapsed.TotalSeconds} seconds");
        }

        [Fact]
        public async Task Performance_ViewportCalculation_LargeDataset_Fast()
        {
            // Arrange
            var dataSource = new MockDataSource();
            var dataManager = new DataManager(dataSource);
            var symbol = new Symbol("EURUSD");
            var timeframe = TimeFrame.M1;
            const int largeSize = 5000;

            await dataManager.LoadLatestBarsAsync(symbol, timeframe, largeSize);
            var bars = dataManager.GetAllBars().ToArray();
            var state = new ChartState { ZoomLevel = 5.0, TotalBars = bars.Length };
            var calculator = new ViewportCalculator();

            var startTime = DateTime.Now;

            // Act - Calculate viewport multiple times (simulate rapid interactions)
            for (int i = 0; i < 100; i++)
            {
                var viewport = calculator.CalculateViewport(state, bars, 800, 600);
                Assert.NotNull(viewport);
            }

            var elapsed = DateTime.Now - startTime;

            // Assert - Should complete 100 calculations in < 1 second
            Assert.True(elapsed.TotalMilliseconds < 1000, $"100 viewport calculations took {elapsed.TotalMilliseconds}ms");
        }

        [Fact]
        public async Task Performance_Rendering_LargeDataset_Completes()
        {
            // Arrange
            var dataSource = new MockDataSource();
            var dataManager = new DataManager(dataSource);
            var symbol = new Symbol("EURUSD");
            var timeframe = TimeFrame.M1;
            const int largeSize = 3000;

            await dataManager.LoadLatestBarsAsync(symbol, timeframe, largeSize);
            var bars = dataManager.GetAllBars().ToArray();
            var state = new ChartState { ZoomLevel = 5.0, TotalBars = bars.Length };
            var renderer = new MockRenderer();
            var engine = new RenderingEngine(renderer);

            var startTime = DateTime.Now;

            // Act - Render large dataset
            var success = engine.Render(bars, state, 800, 600);

            var elapsed = DateTime.Now - startTime;

            // Assert
            Assert.True(success);
            Assert.True(elapsed.TotalSeconds < 2, $"Rendering {largeSize} bars took {elapsed.TotalSeconds} seconds");
        }

        [Fact]
        public async Task Performance_CacheHitRate_DataManager()
        {
            // Arrange
            var dataSource = new MockDataSource();
            var dataManager = new DataManager(dataSource);
            var symbol = new Symbol("EURUSD");
            var timeframe = TimeFrame.M5;

            // Act - Load data
            var success = await dataManager.LoadLatestBarsAsync(symbol, timeframe, 500);
            Assert.True(success);

            var startTime = DateTime.Now;

            // Act - Access cached data multiple times (should be very fast)
            for (int i = 0; i < 1000; i++)
            {
                var bar = dataManager.GetBarAt(i % 500);
                Assert.NotNull(bar);
            }

            var elapsed = DateTime.Now - startTime;

            // Assert - Accessing from cache should be very fast
            Assert.True(elapsed.TotalMilliseconds < 100, $"1000 cache accesses took {elapsed.TotalMilliseconds}ms");
        }
    }
}
