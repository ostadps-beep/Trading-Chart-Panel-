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
    /// Tests for user interaction workflows (zoom, pan, selection).
    /// </summary>
    public class InteractionWorkflowTests
    {
        private MockDataSource _dataSource;
        private DataManager _dataManager;
        private ChartStateManager _stateManager;
        private ViewportCalculator _viewportCalculator;

        public InteractionWorkflowTests()
        {
            _dataSource = new MockDataSource();
            _dataManager = new DataManager(_dataSource);
            _stateManager = new ChartStateManager();
            _viewportCalculator = new ViewportCalculator();
        }

        [Fact]
        public async Task Workflow_MouseWheelZoom_IncreasesThenDecreases()
        {
            // Arrange
            var symbol = new Symbol("EURUSD");
            var timeframe = TimeFrame.M5;
            await _dataManager.LoadLatestBarsAsync(symbol, timeframe, 100);
            _stateManager.SetSymbolAndTimeFrame(symbol, timeframe, _dataManager.GetBarCount());
            var coordinator = new InteractionCoordinator(_stateManager, _viewportCalculator);

            var initialZoom = _stateManager.GetState().ZoomLevel;

            // Act - Simulate mouse wheel up (zoom in)
            coordinator.HandleMouseWheelZoom(120); // Positive delta
            var zoomedIn = _stateManager.GetState().ZoomLevel;
            Assert.True(zoomedIn > initialZoom);

            // Act - Simulate mouse wheel down (zoom out)
            coordinator.HandleMouseWheelZoom(-120); // Negative delta
            var zoomedOut = _stateManager.GetState().ZoomLevel;
            Assert.True(zoomedOut < zoomedIn);
        }

        [Fact]
        public async Task Workflow_MouseDragPan_MovesLeftAndRight()
        {
            // Arrange
            var symbol = new Symbol("EURUSD");
            var timeframe = TimeFrame.M5;
            await _dataManager.LoadLatestBarsAsync(symbol, timeframe, 100);
            _stateManager.SetSymbolAndTimeFrame(symbol, timeframe, _dataManager.GetBarCount());
            var coordinator = new InteractionCoordinator(_stateManager, _viewportCalculator);

            var initialPan = _stateManager.GetState().PanOffset;

            // Act - Drag left (move backward through time)
            coordinator.HandleMouseDragPan(100); // Positive delta = move left
            var panLeft = _stateManager.GetState().PanOffset;
            Assert.True(panLeft > initialPan);

            // Act - Drag right (move forward through time)
            coordinator.HandleMouseDragPan(-50); // Negative delta = move right
            var panRight = _stateManager.GetState().PanOffset;
            Assert.True(panRight < panLeft);
        }

        [Fact]
        public async Task Workflow_DoubleClick_ResetsState()
        {
            // Arrange
            var symbol = new Symbol("EURUSD");
            var timeframe = TimeFrame.M5;
            await _dataManager.LoadLatestBarsAsync(symbol, timeframe, 100);
            _stateManager.SetSymbolAndTimeFrame(symbol, timeframe, _dataManager.GetBarCount());
            var coordinator = new InteractionCoordinator(_stateManager, _viewportCalculator);

            // Modify state
            _stateManager.ZoomIn(2.0);
            _stateManager.PanLeft(20);
            var modifiedZoom = _stateManager.GetState().ZoomLevel;
            var modifiedPan = _stateManager.GetState().PanOffset;

            Assert.True(modifiedZoom > ChartConstants.DefaultZoomLevel);
            Assert.True(modifiedPan > 0);

            // Act - Double click to reset
            coordinator.HandleDoubleClick();

            // Assert - State should be reset to defaults
            Assert.Equal(ChartConstants.DefaultZoomLevel, _stateManager.GetState().ZoomLevel);
            Assert.Equal(0, _stateManager.GetState().PanOffset);
        }

        [Fact]
        public async Task Workflow_ComplexInteraction_ZoomPanZoomPan()
        {
            // Arrange
            var symbol = new Symbol("EURUSD");
            var timeframe = TimeFrame.M5;
            await _dataManager.LoadLatestBarsAsync(symbol, timeframe, 200);
            _stateManager.SetSymbolAndTimeFrame(symbol, timeframe, _dataManager.GetBarCount());
            var coordinator = new InteractionCoordinator(_stateManager, _viewportCalculator);

            var initialState = _stateManager.GetState();

            // Act - Complex interaction sequence
            coordinator.HandleMouseWheelZoom(120);  // Zoom in
            coordinator.HandleMouseDragPan(50);      // Pan left
            coordinator.HandleMouseWheelZoom(120);   // Zoom in more
            coordinator.HandleMouseDragPan(-30);     // Pan right

            var finalState = _stateManager.GetState();

            // Assert - State should be different from initial
            Assert.NotEqual(initialState.ZoomLevel, finalState.ZoomLevel);
            Assert.True(finalState.PanOffset > 0);
        }
    }
}
