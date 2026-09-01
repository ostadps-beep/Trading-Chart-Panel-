using System.Windows;
using System.Windows.Controls;
using TradingChartPanel.Core.Models;
using TradingChartPanel.Core.Services;
using TradingChartPanel.WPF.ViewModels;

namespace TradingChartPanel.WPF.Views
{
    /// <summary>
    /// Main chart panel control.
    /// Displays candlestick chart with price axis, time axis, and interactive elements.
    /// </summary>
    public partial class ChartPanel : UserControl
    {
        private ChartPanelViewModel _viewModel;

        public ChartPanel()
        {
            InitializeComponent();
            _viewModel = new ChartPanelViewModel();
            DataContext = _viewModel;
        }

        /// <summary>
        /// Initialize chart with data source.
        /// </summary>
        public void Initialize(TradingChartPanel.Core.Interfaces.IDataSource dataSource)
        {
            _viewModel?.Initialize(dataSource);
        }

        /// <summary>
        /// Load symbol and timeframe.
        /// </summary>
        public async void LoadSymbol(Symbol symbol, TimeFrame timeframe)
        {
            await _viewModel?.LoadSymbolAsync(symbol, timeframe);
        }

        /// <summary>
        /// Get view model for testing/external access.
        /// </summary>
        public ChartPanelViewModel ViewModel => _viewModel;
    }
}
