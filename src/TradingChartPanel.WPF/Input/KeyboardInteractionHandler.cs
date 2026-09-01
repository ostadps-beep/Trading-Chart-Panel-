using System.Windows.Input;
using TradingChartPanel.WPF.ViewModels;

namespace TradingChartPanel.WPF.Input
{
    /// <summary>
    /// Handles keyboard interactions on the chart.
    /// </summary>
    public class KeyboardInteractionHandler
    {
        private readonly ChartPanelViewModel _viewModel;

        public KeyboardInteractionHandler(ChartPanelViewModel viewModel)
        {
            _viewModel = viewModel ?? throw new System.ArgumentNullException(nameof(viewModel));
        }

        /// <summary>
        /// Handle keyboard key down event.
        /// </summary>
        public void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Home:
                    // Reset to initial view
                    _viewModel?.HandleDoubleClick();
                    e.Handled = true;
                    break;

                case Key.Left:
                    // Pan left
                    _viewModel?.HandleMouseDragPan(50);
                    e.Handled = true;
                    break;

                case Key.Right:
                    // Pan right
                    _viewModel?.HandleMouseDragPan(-50);
                    e.Handled = true;
                    break;

                case Key.Add:
                case Key.OemPlus:
                    // Zoom in
                    _viewModel?.HandleMouseWheelZoom(120);
                    e.Handled = true;
                    break;

                case Key.Subtract:
                case Key.OemMinus:
                    // Zoom out
                    _viewModel?.HandleMouseWheelZoom(-120);
                    e.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// Handle keyboard key up event.
        /// </summary>
        public void OnKeyUp(KeyEventArgs e)
        {
            // Can be used for tracking key release if needed
        }
    }
}
