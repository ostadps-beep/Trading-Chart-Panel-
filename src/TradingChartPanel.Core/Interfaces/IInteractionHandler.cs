using System.Windows.Input;

namespace TradingChartPanel.Core.Interfaces
{
    /// <summary>
    /// Contract for user interaction handlers (mouse, keyboard, touch).
    /// </summary>
    public interface IInteractionHandler
    {
        void OnMouseMove(double x, double y);
        void OnMouseDown(MouseButton button, double x, double y);
        void OnMouseUp(MouseButton button, double x, double y);
        void OnMouseWheel(double x, double y, int delta);
        void OnKeyDown(Key key);
        void OnKeyUp(Key key);
    }
}
