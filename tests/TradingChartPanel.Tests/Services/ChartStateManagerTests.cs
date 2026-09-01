using Xunit;
using TradingChartPanel.Core.Services;
using TradingChartPanel.Core.Models;

namespace TradingChartPanel.Tests.Services
{
    public class ChartStateManagerTests
    {
        [Fact]
        public void ChartStateManager_ZoomIn_IncreasesZoomLevel()
        {
            // Arrange
            var manager = new ChartStateManager();
            var initialZoom = manager.GetState().ZoomLevel;

            // Act
            manager.ZoomIn(1.5);
            var newZoom = manager.GetState().ZoomLevel;

            // Assert
            Assert.True(newZoom > initialZoom);
        }

        [Fact]
        public void ChartStateManager_ZoomOut_DecreasesZoomLevel()
        {
            // Arrange
            var manager = new ChartStateManager();
            manager.SetZoomLevel(20.0);

            // Act
            manager.ZoomOut(2.0);
            var newZoom = manager.GetState().ZoomLevel;

            // Assert
            Assert.True(newZoom < 20.0);
        }

        [Fact]
        public void ChartStateManager_SetZoomLevel_ClampsToMinMax()
        {
            // Arrange
            var manager = new ChartStateManager();

            // Act
            manager.SetZoomLevel(1000.0);  // Too high

            // Assert
            Assert.Equal(ChartConstants.MaxZoomLevel, manager.GetState().ZoomLevel);
        }

        [Fact]
        public void ChartStateManager_PanLeft_IncreasesPanOffset()
        {
            // Arrange
            var manager = new ChartStateManager();

            // Act
            manager.PanLeft(10);

            // Assert
            Assert.Equal(10, manager.GetState().PanOffset);
        }

        [Fact]
        public void ChartStateManager_PanRight_DecreasesPanOffset()
        {
            // Arrange
            var manager = new ChartStateManager();
            manager.PanLeft(20);

            // Act
            manager.PanRight(10);

            // Assert
            Assert.Equal(10, manager.GetState().PanOffset);
        }

        [Fact]
        public void ChartStateManager_Reset_ResetsAllState()
        {
            // Arrange
            var manager = new ChartStateManager();
            manager.ZoomIn(2.0);
            manager.PanLeft(20);

            // Act
            manager.Reset();

            // Assert
            Assert.Equal(ChartConstants.DefaultZoomLevel, manager.GetState().ZoomLevel);
            Assert.Equal(0, manager.GetState().PanOffset);
        }
    }
}
