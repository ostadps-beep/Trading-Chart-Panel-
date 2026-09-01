namespace TradingChartPanel.Core.Services
{
    /// <summary>
    /// Coordinates user interactions (mouse, keyboard, touch).
    /// Routes input events to appropriate handlers.
    /// </summary>
    public class InteractionCoordinator
    {
        private ChartStateManager _stateManager;
        private ViewportCalculator _viewportCalculator;

        public InteractionCoordinator(ChartStateManager stateManager, ViewportCalculator viewportCalculator)
        {
            _stateManager = stateManager ?? throw new System.ArgumentNullException(nameof(stateManager));
            _viewportCalculator = viewportCalculator ?? throw new System.ArgumentNullException(nameof(viewportCalculator));
        }

        /// <summary>
        /// Handle mouse wheel zoom.
        /// </summary>
        public void HandleMouseWheelZoom(int delta, bool shiftPressed = false)
        {
            if (delta > 0)
            {
                // Zoom in
                if (shiftPressed)
                    _stateManager.ZoomIn(1.05);  // Fine control
                else
                    _stateManager.ZoomIn(1.2);   // Normal zoom
            }
            else
            {
                // Zoom out
                if (shiftPressed)
                    _stateManager.ZoomOut(1.05);
                else
                    _stateManager.ZoomOut(1.2);
            }
        }

        /// <summary>
        /// Handle mouse drag for panning.
        /// </summary>
        public void HandleMouseDragPan(double deltaX)
        {
            var state = _stateManager.GetState();
            int barsMoved = (int)(deltaX / state.ZoomLevel);
            
            if (deltaX > 0)
                _stateManager.PanLeft(barsMoved);
            else
                _stateManager.PanRight(-barsMoved);
        }

        /// <summary>
        /// Handle double-click to reset zoom/pan.
        /// </summary>
        public void HandleDoubleClick()
        {
            _stateManager.Reset();
        }
    }
}
