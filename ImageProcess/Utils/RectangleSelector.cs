using Microsoft.VisualBasic.Devices;

namespace ImageProcess.Utils;

public class RectangleSelector
{
    public event EventHandler<Rectangle>? SelectionCompleted;

    private Point selectionStartPoint;

    private Rectangle selectionRectangle = new();

    private bool isSelecting = false;

    public RectangleSelector(PictureBox box)
    {
        box.MouseDown += Box_MouseDown;
        box.MouseMove += Box_MouseMove;
        box.MouseUp += Box_MouseUp;
        box.Paint += Box_Paint;
    }

    private void Box_Paint(object? sender, PaintEventArgs e)
    {
        if (selectionRectangle.Width > 0 && selectionRectangle.Height > 0)
        {
            using var selectionPen = new Pen(Color.Red);

            e.Graphics.DrawRectangle(selectionPen, selectionRectangle);
        }
    }

    private void Box_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;
        isSelecting = true;
        selectionStartPoint = e.Location;
        selectionRectangle = new Rectangle();
    }

    private void Box_MouseMove(object? sender, MouseEventArgs e)
    {
        if (!isSelecting) return;
        selectionRectangle.X = Math.Min(selectionStartPoint.X, e.Location.X);
        selectionRectangle.Y = Math.Min(selectionStartPoint.Y, e.Location.Y);
        selectionRectangle.Width = Math.Abs(selectionStartPoint.X - e.Location.X);
        selectionRectangle.Height = Math.Abs(selectionStartPoint.Y - e.Location.Y);
        if (sender != null) ((PictureBox)sender).Invalidate();
    }

    private void Box_MouseUp(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;
        isSelecting = false;
        SelectionCompleted?.Invoke(this, selectionRectangle);
    }

}
