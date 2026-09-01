using System;
using System.Windows.Input;
using TradingChartPanel.Core.Models;
using TradingChartPanel.Core.Services;
using TradingChartPanel.WPF.ViewModels;

namespace TradingChartPanel.WPF.Input
{
    /// <summary>
    /// Handles mouse interactions on the chart (zoom, pan, selection).
    /// Implements MT4-style interaction patterns.
    /// </summary>
    public class MouseInteractionHandler
    {
        private readonly ChartPanelViewModel _viewModel;
        private Point _lastMousePosition;
        private bool _isDragging = false;
        private const double DragThreshold = 5.0;

        public MouseInteractionHandler(ChartPanelViewModel viewModel)
        {
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }

        /// <summary>
        /// Handle mouse down event.
        /// </summary>
        public void OnMouseDown(MouseButtonEventArgs e, Point currentPosition)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _lastMousePosition = currentPosition;
                _isDragging = false;
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                // Right click for context menu (future)
            }
        }

        /// <summary>
        /// Handle mouse move event.
        /// </summary>
        public void OnMouseMove(MouseEventArgs e, Point currentPosition)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                double deltaX = currentPosition.X - _lastMousePosition.X;

                // Only start drag after threshold
                if (Math.Abs(deltaX) > DragThreshold)
                {
                    _isDragging = true;
                    _viewModel?.HandleMouseDragPan(deltaX);
                    _lastMousePosition = currentPosition;
                }
            }
        }

        /// <summary>
        /// Handle mouse up event.
        /// </summary>
        public void OnMouseUp(MouseButtonEventArgs e)
        {
            _isDragging = false;
        }

        /// <summary>
        /// Handle mouse wheel event (zoom).
        /// </summary>
        public void OnMouseWheel(MouseWheelEventArgs e)
        {
            _viewModel?.HandleMouseWheelZoom(e.Delta);
        }

        /// <summary>
        /// Handle double-click event (reset).
        /// </summary>
        public void OnDoubleClick()
        {
            _viewModel?.HandleDoubleClick();
        }
    }
}
