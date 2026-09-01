using Xunit;
using TradingChartPanel.Core.Services;
using TradingChartPanel.Core.Models;
using TradingChartPanel.Tests.Mocks;

namespace TradingChartPanel.Tests.Services
{
    public class RenderingEngineTests
    {
        [Fact]
        public void RenderingEngine_Render_WithValidData_Succeeds()
        {
            // Arrange
            var renderer = new MockRenderer();
            var engine = new RenderingEngine(renderer);
            
            var bars = new[]
            {
                new OHLC { Open = 1.2000m, High = 1.2050m, Low = 1.1950m, Close = 1.2010m, Volume = 1000, Timestamp = System.DateTime.UtcNow, IsComplete = true },
                new OHLC { Open = 1.2010m, High = 1.2060m, Low = 1.1960m, Close = 1.2020m, Volume = 1100, Timestamp = System.DateTime.UtcNow.AddMinutes(1), IsComplete = true },
            };
            
            var state = new ChartState
            {
                CurrentSymbol = new Symbol("EURUSD"),
                CurrentTimeFrame = TimeFrame.M5,
                TotalBars = bars.Length,
                ZoomLevel = 5.0
            };

            // Act
            var result = engine.Render(bars, state, 800, 600);

            // Assert
            Assert.True(result);
            Assert.NotEmpty(renderer.RenderCalls);
        }

        [Fact]
        public void RenderingEngine_GetCurrentViewport_ReturnsCalculatedViewport()
        {
            // Arrange
            var renderer = new MockRenderer();
            var engine = new RenderingEngine(renderer);
            
            var bars = new[]
            {
                new OHLC { Open = 1.2000m, High = 1.2050m, Low = 1.1950m, Close = 1.2010m, Volume = 1000, Timestamp = System.DateTime.UtcNow, IsComplete = true },
                new OHLC { Open = 1.2010m, High = 1.2060m, Low = 1.1960m, Close = 1.2020m, Volume = 1100, Timestamp = System.DateTime.UtcNow.AddMinutes(1), IsComplete = true },
            };
            
            var state = new ChartState { ZoomLevel = 5.0, TotalBars = bars.Length };

            // Act
            engine.Render(bars, state, 800, 600);
            var viewport = engine.GetCurrentViewport();

            // Assert
            Assert.NotNull(viewport);
            Assert.True(viewport.IsValid());
        }
    }
}
