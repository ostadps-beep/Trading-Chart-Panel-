using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TradingChartPanel.Core.Services;
using TradingChartPanel.Core.Models;
using TradingChartPanel.Tests.Mocks;

namespace TradingChartPanel.Tests.Services
{
    public class DataManagerTests
    {
        private MockDataSource _dataSource;
        private DataManager _dataManager;

        public DataManagerTests()
        {
            _dataSource = new MockDataSource();
            _dataManager = new DataManager(_dataSource);
        }

        [Fact]
        public async Task DataManager_LoadLatestBarsAsync_LoadsData()
        {
            // Arrange
            var symbol = new Symbol("EURUSD");
            var timeframe = TimeFrame.M5;

            // Act
            var result = await _dataManager.LoadLatestBarsAsync(symbol, timeframe, 100);

            // Assert
            Assert.True(result);
            Assert.Equal(100, _dataManager.GetBarCount());
        }

        [Fact]
        public async Task DataManager_GetBarAt_ReturnsCorrectBar()
        {
            // Arrange
            var symbol = new Symbol("EURUSD");
            var timeframe = TimeFrame.M5;
            await _dataManager.LoadLatestBarsAsync(symbol, timeframe, 50);

            // Act
            var bar = _dataManager.GetBarAt(25);

            // Assert
            Assert.NotNull(bar);
            Assert.True(bar.IsValid());
        }

        [Fact]
        public async Task DataManager_GetLatestBar_ReturnsLastBar()
        {
            // Arrange
            var symbol = new Symbol("EURUSD");
            var timeframe = TimeFrame.M5;
            await _dataManager.LoadLatestBarsAsync(symbol, timeframe, 50);

            // Act
            var bar = _dataManager.GetLatestBar();

            // Assert
            Assert.NotNull(bar);
            Assert.True(bar.IsValid());
        }

        [Fact]
        public async Task DataManager_ClearCache_RemovesAllData()
        {
            // Arrange
            var symbol = new Symbol("EURUSD");
            var timeframe = TimeFrame.M5;
            await _dataManager.LoadLatestBarsAsync(symbol, timeframe, 50);

            // Act
            _dataManager.ClearCache();

            // Assert
            Assert.Equal(0, _dataManager.GetBarCount());
        }
    }
}
