using ImageProcess.Core;
using ImageProcess.Core.Interfaces;
using ImageProcess.Core.Models;
using ImageProcess.Filters;
using ImageProcess.Utils;
using System.Reflection;
namespace ImageProcess.Forms;

public partial class MainForm : Form
{
    private AdvancedBitmap<Rgba32>? originalImage;
    private AdvancedBitmap<Rgba32>? processedImage;

    private List<IFilter<Rgba32>> availableFilters = [];

    private const int FormExpandedSize = 1000;
    private const int FormCollapsedSize = 800;
    private const int PanelExpandedSize = 200;
    private const int PanelCollapsedSize = 0;

    private Rectangle _currentSelection;

    Rectangle imageRoi;

    public MainForm()
    {
        InitializeComponent();
        InitForm();
        this.Width = 800;
        PnlChild.Width = 0;
    }

    public void InitForm()
    {
        TSMExit.Click += (o, s) =>
        {
            Application.Exit();
        };

        TSMSaveAs.Click += (o, s) =>
        {
            if (pictureBoxProcessed.Image == null)
                MessageBox.Show("There is no processed image to save.", "Warning");

            else if (SaveFileDialog.ShowDialog() == DialogResult.OK)
                pictureBoxProcessed.Image.Save(SaveFileDialog.FileName);
        };

        TSMOpenFile.Click += (o, s) =>
        {
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                originalImage?.Dispose();
                processedImage?.Dispose();

                originalImage = (AdvancedBitmap<Rgba32>)new Bitmap(OpenFileDialog.FileName);
                processedImage = (AdvancedBitmap<Rgba32>)originalImage.Clone();

                pictureBoxOriginal.Image = (Bitmap)originalImage;
                pictureBoxProcessed.Image = (Bitmap)processedImage;
            }
        };

    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        DiscoverFilters();
        BuildFilterMenu();
        var rectangleSelector = new RectangleSelector(pictureBoxOriginal);
        rectangleSelector.SelectionCompleted += (s, rect) =>
        {
            _currentSelection = rect;
        };
    }

    private Task<DialogResult> OpenChildForm(object? settingsObject = null)
    {
        var tcs = new TaskCompletionSource<DialogResult>();

        this.Width = FormExpandedSize;
        PnlChild.Width = PanelExpandedSize;
        var childForm = new SettingsForm(settingsObject)
        {
            TopLevel = false,
            FormBorderStyle = FormBorderStyle.None,
            Dock = DockStyle.Fill
        };

        childForm.FormClosed += (sender, e) =>
        {
            this.Width = FormCollapsedSize;
            PnlChild.Width = PanelCollapsedSize;
            tcs.SetResult(childForm.DialogResult);
        };

        this.PnlChild.Controls.Clear();
        this.PnlChild.Controls.Add(childForm);
        this.PnlChild.Tag = childForm;
        childForm.BringToFront();
        childForm.Show();

        return tcs.Task;
    }

    private void BuildFilterMenu()
    {
        var groupedFilters = availableFilters.GroupBy(f => f.Category);

        foreach (var group in groupedFilters)
        {
            var categoryMenuItem = new ToolStripMenuItem(group.Key);

            foreach (var filter in group)
            {
                var filterMenuItem = new ToolStripMenuItem(filter.Name)
                {
                    Tag = filter
                };

                filterMenuItem.Click += FilterMenuItem_Click;

                categoryMenuItem.DropDownItems.Add(filterMenuItem);
            }

            TSMFilter.DropDownItems.Add(categoryMenuItem);
        }
    }

    private async void FilterMenuItem_Click(object? sender, EventArgs e)
    {
        if (originalImage == null)
        {
            MessageBox.Show("Lütfen önce bir resim açın.", "Uyarı");
            return;
        }

        var menuItem = sender as ToolStripMenuItem;
        if (menuItem?.Tag is not IFilter<Rgba32> filter) return;

        processedImage?.Dispose();
        processedImage = (AdvancedBitmap<Rgba32>)originalImage.Clone();

        Rectangle? controlRoi = _currentSelection; // Assuming you have a reference '_selectionManager'

        if (controlRoi.HasValue && controlRoi.Value.Width > 0)
        {
            imageRoi = (Rectangle)MapControlRectToImageRect(pictureBoxOriginal, controlRoi.Value);
        }

        var context = new ProcessContext<Rgba32>
        {
            SourceImage = processedImage,
            ROI = imageRoi,
        };


        var settingsProperty = filter.GetType().GetProperty("Settings");

        if (settingsProperty != null)
        {
            object? settingsObject = settingsProperty.GetValue(filter);
            var result = await OpenChildForm(settingsObject);
            if (result != DialogResult.OK) return;
        }

        if (filter is IMultiImageFilter<Rgba32>)
        {
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                using var bmp = new Bitmap(OpenFileDialog.FileName);

                context.SecondaryImage = (AdvancedBitmap<Rgba32>)bmp;
            }
            else
            {
                return;
            }
        }


        this.Cursor = Cursors.WaitCursor;


        await Task.Run(() => filter.Execute(context));

        pictureBoxProcessed.Image = (Bitmap)processedImage;

        this.Cursor = Cursors.Default;
    }

    private void DiscoverFilters()
    {
        availableFilters =[..Assembly.GetExecutingAssembly().GetTypes()
        .Where(t => typeof(IFilter<Rgba32>).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
        .Select(t => (IFilter<Rgba32>?)Activator.CreateInstance(t))];
    }

    /// <summary>
    /// Converts a rectangle from the PictureBox's coordinate space to the actual image's coordinate space.
    /// This accounts for scaling and padding caused by the PictureBox's SizeMode.
    /// </summary>
    /// <param name="pictureBox">The PictureBox control containing the image.</param>
    /// <param name="controlRect">The selection rectangle in the PictureBox's coordinates.</param>
    /// <returns>The corresponding rectangle in the image's pixel coordinates, or null if there is no image.</returns>
    private static Rectangle? MapControlRectToImageRect(PictureBox pictureBox, Rectangle controlRect)
    {
        // Ensure there is an image to work with.
        if (pictureBox.Image == null)
        {
            return null;
        }

        // 1. Calculate the scale ratio (how much the image is zoomed in or out).
        // The smaller ratio determines the overall scale factor due to SizeMode.Zoom.
        float widthRatio = (float)pictureBox.ClientSize.Width / pictureBox.Image.Width;
        float heightRatio = (float)pictureBox.ClientSize.Height / pictureBox.Image.Height;
        float ratio = Math.Min(widthRatio, heightRatio);

        // 2. Calculate the size of the displayed image and the empty padding around it.
        int displayWidth = (int)(pictureBox.Image.Width * ratio);
        int displayHeight = (int)(pictureBox.Image.Height * ratio);
        int paddingX = (pictureBox.ClientSize.Width - displayWidth) / 2;
        int paddingY = (pictureBox.ClientSize.Height - displayHeight) / 2;

        // 3. Translate the corners of the control rectangle to image coordinates.
        // We remove the padding and then divide by the scale ratio.
        int imageX1 = (int)((controlRect.Left - paddingX) / ratio);
        int imageY1 = (int)((controlRect.Top - paddingY) / ratio);
        int imageX2 = (int)((controlRect.Right - paddingX) / ratio);
        int imageY2 = (int)((controlRect.Bottom - paddingY) / ratio);

        // Create the final rectangle from the translated points.
        var imageRect = new Rectangle(imageX1, imageY1, imageX2 - imageX1, imageY2 - imageY1);

        // 4. Ensure the calculated rectangle is within the actual image's bounds to prevent errors.
        // Rectangle.Intersect finds the overlapping area between the two rectangles.
        var imageBounds = new Rectangle(0, 0, pictureBox.Image.Width, pictureBox.Image.Height);
        imageRect.Intersect(imageBounds);

        return imageRect;
    }







}
