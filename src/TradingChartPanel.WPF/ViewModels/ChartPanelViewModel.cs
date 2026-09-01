using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Series;
using TradingChartPanel.Core.Interfaces;
using TradingChartPanel.Core.Models;
using TradingChartPanel.Core.Services;

namespace TradingChartPanel.WPF.ViewModels
{
    /// <summary>
    /// Main view model for chart panel.
    /// Implements MVVM pattern for WPF chart control.
    /// </summary>
    public class ChartPanelViewModel : ViewModelBase
    {
        private IDataSource _dataSource;
        private DataManager _dataManager;
        private ChartStateManager _stateManager;
        private ViewportCalculator _viewportCalculator;
        private RenderingEngine _renderingEngine;
        private PlotModel _plotModel;
        private string _statusText = "Ready";
        private OxyPlotRenderer _renderer;

        public ChartPanelViewModel()
        {
            _stateManager = new ChartStateManager();
            _viewportCalculator = new ViewportCalculator();
        }

        /// <summary>
        /// Initialize chart with data source.
        /// </summary>
        public void Initialize(IDataSource dataSource)
        {
            _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
            _dataManager = new DataManager(_dataSource);
            _renderer = new OxyPlotRenderer();
            _renderingEngine = new RenderingEngine(_renderer);
            StatusText = "Initialized";
        }

        /// <summary>
        /// Load symbol and timeframe.
        /// </summary>
        public async Task LoadSymbolAsync(Symbol symbol, TimeFrame timeframe)
        {
            try
            {
                StatusText = "Loading data...";
                var success = await _dataManager.LoadLatestBarsAsync(symbol, timeframe, 500);
                
                if (!success)
                {
                    StatusText = "Failed to load data";
                    return;
                }

                _stateManager.SetSymbolAndTimeFrame(symbol, timeframe, _dataManager.GetBarCount());
                RenderChart();
                StatusText = $"Loaded: {symbol.Code} {timeframe.Name} ({_dataManager.GetBarCount()} bars)";
            }
            catch (Exception ex)
            {
                StatusText = $"Error: {ex.Message}";
            }
        }

        /// <summary>
        /// Render the chart.
        /// </summary>
        public void RenderChart()
        {
            try
            {
                var bars = _dataManager.GetAllBars().ToArray();
                if (bars.Length == 0)
                    return;

                _renderingEngine.Render(bars, _stateManager.GetState(), 750, 530);
                PlotModel = _renderer.GetPlotModel();
                StatusText = _stateManager.GetState().ToString();
            }
            catch (Exception ex)
            {
                StatusText = $"Render error: {ex.Message}";
            }
        }

        /// <summary>
        /// Handle mouse wheel zoom.
        /// </summary>
        public void HandleMouseWheelZoom(int delta)
        {
            var coordinator = new InteractionCoordinator(_stateManager, _viewportCalculator);
            coordinator.HandleMouseWheelZoom(delta);
            RenderChart();
        }

        /// <summary>
        /// Handle mouse drag pan.
        /// </summary>
        public void HandleMouseDragPan(double deltaX)
        {
            var coordinator = new InteractionCoordinator(_stateManager, _viewportCalculator);
            coordinator.HandleMouseDragPan(deltaX);
            RenderChart();
        }

        /// <summary>
        /// Handle double-click reset.
        /// </summary>
        public void HandleDoubleClick()
        {
            var coordinator = new InteractionCoordinator(_stateManager, _viewportCalculator);
            coordinator.HandleDoubleClick();
            RenderChart();
        }

        #region Properties

        public PlotModel PlotModel
        {
            get => _plotModel;
            set => SetProperty(ref _plotModel, value);
        }

        public string StatusText
        {
            get => _statusText;
            set => SetProperty(ref _statusText, value);
        }

        public AxisViewModel PriceAxisViewModel { get; set; } = new();
        public AxisViewModel TimeAxisViewModel { get; set; } = new();

        #endregion
    }

    /// <summary>
    /// View model for axis display.
    /// </summary>
    public class AxisViewModel : ViewModelBase
    {
        private string _title = "Axis";

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}
