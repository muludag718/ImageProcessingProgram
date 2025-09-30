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

    private const int ExpandedSize = 1000;
    private const int CollapsedSize = 1200;

    private Rectangle _currentSelection;


    public MainForm()
    {
        InitializeComponent();
        this.Width = 800;
        PnlChild.Width = 0;
    }


    private void MainForm_Load(object sender, EventArgs e)
    {
        DiscoverFilters();
        BuildFilterMenu();
        var rectangleSelector = new RectangleSelector(pictureBoxOriginal);
        rectangleSelector.SelectionCompleted += (s, rect) =>
        {
            _currentSelection = rect;
            yardımToolStripMenuItem.Text = $"x:{rect.X} y: {rect.Y}  width={rect.Width}, h:{rect.Height}";
        };
    }

    private Task<DialogResult> OpenChildForm(object? settingsObject = null)
    {
        var tcs = new TaskCompletionSource<DialogResult>();

        this.Width = 1000;
        PnlChild.Width = 200;
        var childForm = new SettingsForm(settingsObject)
        {
            TopLevel = false,
            FormBorderStyle = FormBorderStyle.None,
            Dock = DockStyle.Fill
        };

        childForm.FormClosed += (sender, e) =>
        {
            this.Width = 800;
            PnlChild.Width = 0;
            tcs.SetResult(childForm.DialogResult);
        };

        this.PnlChild.Controls.Clear();
        this.PnlChild.Controls.Add(childForm);
        this.PnlChild.Tag = childForm;
        childForm.BringToFront();
        childForm.Show();

        return tcs.Task;
    }

    private void ChildForm_FormClosed(object? sender, FormClosedEventArgs e)
    {
        this.Width = 800;
        PnlChild.Width = 0;
    }

    private void BuildFilterMenu()
    {
        var groupedFilters = availableFilters.GroupBy(f => f.Category);

        foreach (var group in groupedFilters)
        {
            var categoryMenuItem = new ToolStripMenuItem(group.Key);

            foreach (var filter in group)
            {
                var filterMenuItem = new ToolStripMenuItem(filter.Name);

                // ÖNEMLİ: Menü öğesinin 'Tag' özelliğine, filtrenin nesnesini atıyoruz.
                // Bu sayede hangi menüye tıklandığında hangi filtrenin çalışacağını bileceğiz.
                filterMenuItem.Tag = filter;

                // Tüm filtre menü öğelerini aynı 'Click' olayına bağlıyoruz.
                filterMenuItem.Click += FilterMenuItem_Click;

                categoryMenuItem.DropDownItems.Add(filterMenuItem);
            }

            // Oluşturulan bu kategoriyi ana "Filtreler" menüsüne ekle.
            filtrelerToolStripMenuItem.DropDownItems.Add(categoryMenuItem);
        }
    }

    Rectangle imageRoi;

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
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using var bmp = new Bitmap(openFileDialog.FileName);

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
        availableFilters = [.. Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => typeof(IFilter<Rgba32>).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => (IFilter<Rgba32>?) Activator.CreateInstance(t))];
    }

    private void AçToolStripMenuItem1_Click(object sender, EventArgs e)
    {
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            originalImage?.Dispose(); // Varsa bir önceki orijinali temizle.
            processedImage?.Dispose(); // Varsa bir önceki işlenmişi temizle.

            // Orijinal resmi yükle.
            originalImage = (AdvancedBitmap<Rgba32>)new Bitmap(openFileDialog.FileName);

            // Orijinalin bir kopyasını işlenecek resim olarak ata.
            // Adım 6.1'de yazdığımız Clone() metodu burada işe yarıyor!
            processedImage = (AdvancedBitmap<Rgba32>)originalImage.Clone();

            pictureBoxOriginal.Image = (Bitmap)originalImage;
            // Başlangıçta işlenmiş resim de orijinalin aynısıdır.
            pictureBoxProcessed.Image = (Bitmap)processedImage;
        }
    }
    public static float[,] Laplacian3x3
    {
        get
        {
            return new float[,]
            { { -1, -1, -1,  },
                  { -1,  8, -1,  },
                  { -1, -1, -1,  }, };
        }
    }
    private void FarklıKaydetToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (pictureBoxProcessed.Image == null)
        {
            MessageBox.Show("Kaydedilecek işlenmiş bir görüntü yok.", "Uyarı");
            return;
        }

        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
            pictureBoxProcessed.Image.Save(saveFileDialog.FileName);
        }
    }

    private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    private async void laplacian3x3ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (originalImage == null)
        {
            MessageBox.Show("Lütfen önce bir resim açın.", "Uyarı");
            return;
        }





        //// 3. Sonucu _processedImage'den alıp göster.
        //pictureBoxProcessed.Image = await Task.Run(() => Exam.AddNoise(pictureBoxOriginal.Image));

        //this.Cursor = Cursors.Default;
    }


    /// <summary>
    /// Converts a rectangle from the PictureBox's coordinate space to the actual image's coordinate space.
    /// This accounts for scaling and padding caused by the PictureBox's SizeMode.
    /// </summary>
    /// <param name="pictureBox">The PictureBox control containing the image.</param>
    /// <param name="controlRect">The selection rectangle in the PictureBox's coordinates.</param>
    /// <returns>The corresponding rectangle in the image's pixel coordinates, or null if there is no image.</returns>
    private Rectangle? MapControlRectToImageRect(PictureBox pictureBox, Rectangle controlRect)
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
        Rectangle imageBounds = new Rectangle(0, 0, pictureBox.Image.Width, pictureBox.Image.Height);
        imageRect.Intersect(imageBounds);

        return imageRect;
    }
}
