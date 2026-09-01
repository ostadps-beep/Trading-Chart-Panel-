using System;
using System.Threading.Tasks;
using Xunit;
using TradingChartPanel.Core.Models;
using TradingChartPanel.Core.Services;
using TradingChartPanel.Tests.Mocks;
using TradingChartPanel.WPF.ViewModels;
using TradingChartPanel.WPF.Input;

namespace TradingChartPanel.Tests.Integration
{
    /// <summary>
    /// End-to-end integration tests for the complete chart workflow.
    /// Tests data loading, state management, viewport calculation, and rendering.
    /// </summary>
    public class ChartIntegrationTests
    {
        private MockDataSource _dataSource;
        private DataManager _dataManager;
        private ChartStateManager _stateManager;
        private ViewportCalculator _viewportCalculator;
        private MockRenderer _renderer;
        private RenderingEngine _renderingEngine;

        public ChartIntegrationTests()
        {
            _dataSource = new MockDataSource();
            _dataManager = new DataManager(_dataSource);
            _stateManager = new ChartStateManager();
            _viewportCalculator = new ViewportCalculator();
            _renderer = new MockRenderer();
            _renderingEngine = new RenderingEngine(_renderer);
        }

        [Fact]
        public async Task EndToEnd_LoadChartData_RenderChart_Success()
        {
            // Arrange
            var symbol = new Symbol("EURUSD", 5, "Euro vs USD");
            var timeframe = TimeFrame.M5;

            // Act - Load data
            var loadSuccess = await _dataManager.LoadLatestBarsAsync(symbol, timeframe, 100);
            Assert.True(loadSuccess);
            Assert.Equal(100, _dataManager.GetBarCount());

            // Act - Set state
            _stateManager.SetSymbolAndTimeFrame(symbol, timeframe, _dataManager.GetBarCount());
            var state = _stateManager.GetState();
            Assert.Equal(symbol, state.CurrentSymbol);
            Assert.Equal(timeframe, state.CurrentTimeFrame);

            // Act - Calculate viewport
            var bars = _dataManager.GetAllBars().ToArray();
            var viewport = _viewportCalculator.CalculateViewport(state, bars, 800, 600);
            Assert.NotNull(viewport);
            Assert.True(viewport.IsValid());

            // Act - Render
            var renderSuccess = _renderingEngine.Render(bars, state, 800, 600);
            Assert.True(renderSuccess);
            Assert.NotNull(_renderingEngine.GetCurrentViewport());
        }

        [Fact]
        public async Task EndToEnd_ZoomInOut_UpdatesViewport()
        {
            // Arrange
            var symbol = new Symbol("GBPUSD");
            var timeframe = TimeFrame.H1;
            await _dataManager.LoadLatestBarsAsync(symbol, timeframe, 200);
            _stateManager.SetSymbolAndTimeFrame(symbol, timeframe, _dataManager.GetBarCount());

            var initialZoom = _stateManager.GetState().ZoomLevel;

            // Act - Zoom in
            _stateManager.ZoomIn(1.5);
            var zoomedInZoom = _stateManager.GetState().ZoomLevel;
            Assert.True(zoomedInZoom > initialZoom);

            // Act - Zoom out
            _stateManager.ZoomOut(1.5);
            var zoomedOutZoom = _stateManager.GetState().ZoomLevel;
            Assert.True(zoomedOutZoom < zoomedInZoom);
        }

        [Fact]
        public async Task EndToEnd_PanLeftRight_UpdatesState()
        {
            // Arrange
            var symbol = new Symbol("USDJPY");
            var timeframe = TimeFrame.D1;
            await _dataManager.LoadLatestBarsAsync(symbol, timeframe, 250);
            _stateManager.SetSymbolAndTimeFrame(symbol, timeframe, _dataManager.GetBarCount());

            var initialPan = _stateManager.GetState().PanOffset;

            // Act - Pan left (backward in time)
            _stateManager.PanLeft(20);
            Assert.Equal(initialPan + 20, _stateManager.GetState().PanOffset);

            // Act - Pan right (forward in time)
            _stateManager.PanRight(10);
            Assert.Equal(initialPan + 10, _stateManager.GetState().PanOffset);
        }

        [Fact]
        public async Task EndToEnd_ViewportCalculation_PriceRangeCorrect()
        {
            // Arrange
            var symbol = new Symbol("EURUSD");
            var timeframe = TimeFrame.M15;
            await _dataManager.LoadLatestBarsAsync(symbol, timeframe, 150);
            _stateManager.SetSymbolAndTimeFrame(symbol, timeframe, _dataManager.GetBarCount());

            var bars = _dataManager.GetAllBars().ToArray();
            var state = _stateManager.GetState();

            // Act
            var viewport = _viewportCalculator.CalculateViewport(state, bars, 800, 600);

            // Assert
            Assert.NotNull(viewport);
            Assert.True(viewport.MinPrice > 0);
            Assert.True(viewport.MaxPrice > viewport.MinPrice);
            Assert.True(viewport.PixelsPerPrice > 0);
        }

        [Fact]
        public async Task EndToEnd_CoordinateTransformations_Accurate()
        {
            // Arrange
            var symbol = new Symbol("EURUSD");
            var timeframe = TimeFrame.M5;
            await _dataManager.LoadLatestBarsAsync(symbol, timeframe, 100);
            _stateManager.SetSymbolAndTimeFrame(symbol, timeframe, _dataManager.GetBarCount());

            var bars = _dataManager.GetAllBars().ToArray();
            var state = _stateManager.GetState();
            var viewport = _viewportCalculator.CalculateViewport(state, bars, 800, 600);

            // Act - Convert price to Y coordinate
            double yCoord = _viewportCalculator.PriceToYCoordinate(viewport.MinPrice, viewport);
            Assert.True(yCoord >= 0);
            Assert.True(yCoord <= viewport.ViewportHeight);

            // Act - Convert Y coordinate back to price
            decimal price = _viewportCalculator.YCoordinateToPriceValue(yCoord, viewport);
            Assert.InRange(price, viewport.MinPrice - 0.0001m, viewport.MinPrice + 0.0001m);

            // Act - Convert bar index to X coordinate
            double xCoord = _viewportCalculator.BarIndexToXCoordinate(viewport.FirstVisibleBarIndex, viewport, state);
            Assert.True(xCoord >= 0);
        }

        [Fact]
        public async Task EndToEnd_LargeDataset_HandlesCorrectly()
        {
            // Arrange - Load many bars
            var symbol = new Symbol("EURUSD");
            var timeframe = TimeFrame.M1;
            const int largeDatasetSize = 5000; // 5000 candles

            // Act
            var success = await _dataManager.LoadLatestBarsAsync(symbol, timeframe, largeDatasetSize);
            Assert.True(success);
            Assert.Equal(largeDatasetSize, _dataManager.GetBarCount());

            // Act - Should still be able to render
            _stateManager.SetSymbolAndTimeFrame(symbol, timeframe, largeDatasetSize);
            var bars = _dataManager.GetAllBars().ToArray();
            var renderSuccess = _renderingEngine.Render(bars, _stateManager.GetState(), 800, 600);
            Assert.True(renderSuccess);
        }

        [Fact]
        public async Task EndToEnd_DataValidation_RejectsInvalidData()
        {
            // Arrange
            var symbol = new Symbol("EURUSD");
            var timeframe = TimeFrame.M5;

            // Create invalid bars (High < Low)
            var invalidBars = new OHLC[]
            {
                new OHLC
                {
                    Open = 1.2000m,
                    High = 1.1900m,  // Invalid: High < Low
                    Low = 1.2100m,
                    Close = 1.2010m,
                    Volume = 1000,
                    Timestamp = DateTime.UtcNow
                }
            };

            // Act
            var isValid = _dataSource.ValidateData(invalidBars);

            // Assert
            Assert.False(isValid);
        }
    }
}
