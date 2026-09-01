using System;
using System.Windows;
using TradingChartPanel.WPF.Input;
using TradingChartPanel.WPF.ViewModels;

namespace TradingChartPanel.WPF.Views
{
    /// <summary>
    /// Code-behind for ChartPanel with interaction handlers.
    /// </summary>
    public partial class ChartPanel : FrameworkElement
    {
        private MouseInteractionHandler _mouseHandler;
        private KeyboardInteractionHandler _keyboardHandler;

        public void InitializeInteractionHandlers()
        {
            if (_viewModel != null)
            {
                _mouseHandler = new MouseInteractionHandler(_viewModel);
                _keyboardHandler = new KeyboardInteractionHandler(_viewModel);
            }
        }

        private void ChartArea_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _mouseHandler?.OnMouseDown(e, e.GetPosition((UIElement)sender));
        }

        private void ChartArea_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _mouseHandler?.OnMouseUp(e);
        }

        private void ChartArea_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _mouseHandler?.OnMouseMove(e, e.GetPosition((UIElement)sender));
        }

        private void ChartArea_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            _mouseHandler?.OnMouseWheel(e);
            e.Handled = true;
        }
    }
}
