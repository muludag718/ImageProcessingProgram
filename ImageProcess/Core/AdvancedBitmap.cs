using ImageProcess.Core.Interfaces;
using ImageProcess.Core.Models;
using System.Buffers;
using System.Drawing.Imaging;

namespace ImageProcess.Core;

public class AdvancedBitmap<TPixel> : ICloneable, IDisposable where TPixel : struct, IPixel<TPixel>
{
    private IMemoryOwner<TPixel> pixelMemoryOwner;

    private Memory<TPixel> pixelMemory;

    private readonly Stack<IMemoryOwner<TPixel>> undoStack = new();

    private readonly Stack<IMemoryOwner<TPixel>> redoStack = new();

    private readonly List<IOperation<TPixel>> operationQueue = [];

    public int Width { get; private set; }

    public int Height { get; private set; }
    // Property that indicates whether there is a process waiting in the queue.
    public bool HasPendingChanges => operationQueue.Count > 0;


    /// <summary>
    /// Creates a new AdvancedBitmap object by loading an image from the specified file path.
    /// </summary>
    /// <param name="filePath">Path to the image file.</param>
    public AdvancedBitmap(string filePath)
    {
        using var bmp = new Bitmap(filePath);
        LoadFromBitmap(bmp);
    }

    /// <summary>
    /// Creates a new AdvancedBitmap object by taking data from an existing Bitmap object.
    /// </summary>
    /// <param name="sourceBitmap">Source Bitmap object.</param>
    public AdvancedBitmap(Bitmap sourceBitmap)
    {
        LoadFromBitmap(sourceBitmap);
    }

    /// <summary>
    /// A special constructor method that creates an empty AdvancedBitmap canvas of the specified dimensions.
    /// This method is used for operations within the class itself.
    /// </summary>
    private AdvancedBitmap(int width, int height)
    {
        this.Width = width;
        this.Height = height;
        // Rent enough memory from the MemoryPool to hold the pixels.
        pixelMemoryOwner = MemoryPool<TPixel>.Shared.Rent(width * height);
        pixelMemory = pixelMemoryOwner.Memory;
    }

    /// <summary>
    /// A special constructor method that creates a deep copy of another AdvancedBitmap object.
    /// Used when implementing the ICloneable interface.
    /// </summary>
    private AdvancedBitmap(AdvancedBitmap<TPixel> source)
    {
        this.Width = source.Width;
        this.Height = source.Height;
        pixelMemoryOwner = MemoryPool<TPixel>.Shared.Rent(Width * Height);
        pixelMemory = pixelMemoryOwner.Memory;
        // Copy the pixel data of the source image into the memory of this newly created object.
        source.pixelMemory.CopyTo(pixelMemory);
    }



    /// <summary>
    /// Loads the pixel data of a standard Bitmap object into our own fast memory space.
    /// </summary>
    /// <param name="source">Source Bitmap object.</param>
    private unsafe void LoadFromBitmap(Bitmap source)
    {
        this.Width = source.Width;
        this.Height = source.Height;

        pixelMemoryOwner = MemoryPool<TPixel>.Shared.Rent(Width * Height);
        pixelMemory = pixelMemoryOwner.Memory;

        var rect = new Rectangle(0, 0, Width, Height);

        BitmapData bmpData = source.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        try
        {
            var sourceSpan = new Span<Rgba32>(bmpData.Scan0.ToPointer(), Width * Height);

            var destSpan = pixelMemory.Span;

            int pixelCount = Width * Height;
            for (int i = 0; i < pixelCount; i++)
            {
                destSpan[i].FromRgba32(sourceSpan[i]);
            }
        }
        finally
        {
            source.UnlockBits(bmpData);
        }
    }


    /// <summary>
    /// Converts the current pixel data into a standard System.Drawing.Bitmap object that can be displayed on the screen.
    /// </summary>
    /// <returns>A new Bitmap object containing the pixel data.</returns>
    public unsafe Bitmap ToBitmap()
    {
        if (HasPendingChanges) Execute();

        var bmp = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

        var rect = new Rectangle(0, 0, Width, Height);

        var bmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

        try
        {
            var sourceSpan = pixelMemory.Span;

            var destSpan = new Span<Rgba32>(bmpData.Scan0.ToPointer(), Width * Height);

            int pixelCount = this.Width * this.Height;
            for (int i = 0; i < pixelCount; i++)
            {
                destSpan[i] = sourceSpan[i].ToRgba32();
            }

        }
        finally
        {
            bmp.UnlockBits(bmpData);
        }

        return bmp;
    }

    #region Dönüştürme Operatörleri (Conversion Operators)

    /// <summary>
    /// AdvancedBitmap nesnesini standart System.Drawing.Bitmap nesnesine açıkça dönüştürür.
    /// Kullanım: Bitmap bmp = (Bitmap)myAdvancedBitmap;
    /// </summary>
    public static explicit operator Bitmap(AdvancedBitmap<TPixel> advancedBitmap)
    {
        return advancedBitmap.ToBitmap();
    }

    /// <summary>
    /// Standart System.Drawing.Bitmap nesnesini AdvancedBitmap nesnesine açıkça dönüştürür.
    /// Kullanım: AdvancedBitmap<Rgba32> adv = (AdvancedBitmap<Rgba32>)myBitmap;
    /// </summary>
    public static explicit operator AdvancedBitmap<TPixel>(Bitmap sourceBitmap)
    {
        // Bu operatör, Adım 2.2'de yazdığımız kurucu metodu çağırır.
        return new AdvancedBitmap<TPixel>(sourceBitmap);
    }

    #endregion

    /// <summary>
    /// Returns a Span that provides direct access to the specified row of the image without copying.
    /// This is the fundamental building block of all pixel manipulation operations.
    /// </summary>
    /// <param name="rowIndex">The index (Y coordinate) of the row to be accessed.</param>
    /// <returns>A Span<TPixel> representing the specified row.</returns>
    public Span<TPixel> GetRowSpan(int rowIndex)
    {
        return pixelMemory.Span.Slice(rowIndex * Width, Width);
    }
    //public Span<TPixel> GetRowSpan(int rowIndex, Rectangle? ROI)
    //{
    //    var startIndex = ROI != null ? ROI.Value.Top : 0;
    //    var finishIndex = ROI != null ? ROI.Value.Bottom : Width;
    //    return pixelMemory.Span.Slice((startIndex + rowIndex) * finishIndex, finishIndex);
    //}
    /// <summary>
    /// Runs a given 'processor' object in parallel on each row of the image.
    /// This is the highest-performance processing method.
    /// </summary>
    /// <typeparam name="TProcessor">A struct-type handler that implements the IRowProcessor interface.</typeparam>
    /// <param name="processor">The handler object whose ProcessRow method will be called for each row.</param>
    /// <param name="roi">The Region of Interest to which the operation will be applied. If null, it will be applied to the entire image.</param>
    public void ProcessRows<TProcessor>(TProcessor processor, Rectangle? roi = null) where TProcessor : struct, IRowProcessor<TPixel>
    {
        Rectangle processArea;
        if (roi == null || roi?.Width == 0 || roi?.Height == 0)
            processArea = new Rectangle(0, 0, this.Width, this.Height);
        else
            processArea = roi ?? new Rectangle(0, 0, this.Width, this.Height);

        Parallel.For(processArea.Top, processArea.Bottom, y =>
        {
            var row = GetRowSpan(y);

            var roiRow = row.Slice(processArea.Left, processArea.Width);

            processor.ProcessRow(roiRow);
        });
    }

    private bool RoiCheck(Rectangle? roi)
    {
        if (roi == null) return false;
        if (roi?.Width == 0 || roi?.Height == 0) return false;
        return true;
    }




    #region Will Change

    /// <summary>
    /// Applies convolution to the image using a specified kernel (matrix).
    /// This method forms the basis of many filters such as blurring, sharpening, and edge detection.
    /// </summary>
    /// <param name="kernel">The 2D convolution matrix (kernel) to apply.</param>
    /// <param name="roi">The Region of Interest to which the operation will be applied. If null, it will be applied to the entire image.</param>
    public void Convolve(float[,] kernel, Rectangle? roi = null)
    {

        int kernelHeigth = kernel.GetLength(0);
        int kernelWidth = kernel.GetLength(1);

        int radiusY = kernelHeigth / 2;
        int radiusX = kernelWidth / 2;

        var sourceBufferOwner = MemoryPool<TPixel>.Shared.Rent(Width * Height);
        var sourceMemory = sourceBufferOwner.Memory;
        pixelMemory.CopyTo(sourceMemory);
        var sourceSpan = sourceMemory.Span;
        var destSpan = pixelMemory.Span;

        Rectangle processArea = roi ?? new Rectangle(0, 0, this.Width, this.Height);

        try
        {
            Parallel.For(processArea.Top, processArea.Bottom, y =>
            {
                var sourceSpan = sourceMemory.Span;

                var destSpan = pixelMemory.Span;

                for (int x = processArea.Left; x < processArea.Right; x++)
                {
                    float sumR = 0, sumG = 0, sumB = 0;

                    for (int ky = 0; ky < kernelHeigth; ky++)
                    {

                        for (int kx = 0; kx < kernelWidth; kx++)
                        {

                            // Calculate the coordinate of the neighboring pixel.
                            int pixelX = x + (kx - radiusX);
                            int pixelY = y + (ky - radiusX);

                            // Edge control: If the kernel extends beyond the image, use the nearest edge pixel.
                            pixelX = Math.Max(0, Math.Min(Width - 1, pixelX));
                            pixelY = Math.Max(0, Math.Min(Height - 1, pixelY));

                            //Get the value of neighbor pixel and kernel value from original (source) image.
                            var sourcePixel = sourceSpan[pixelY * Width + pixelX].ToRgba32();
                            float kernelValue = kernel[kx, ky];

                            sumR += sourcePixel.R * kernelValue;
                            sumG += sourcePixel.G * kernelValue;
                            sumB += sourcePixel.B * kernelValue;
                        }
                    }
                    // Keep original alpha value.
                    var originalAlpha = sourceSpan[y * Width + x].ToRgba32().A;
                    // Create the new pixel by compressing the calculated total value between 0 - 255.
                    var finalPixel = new Rgba32(
                        (byte)Math.Max(0, Math.Min(255, sumR)),
                        (byte)Math.Max(0, Math.Min(255, sumG)),
                        (byte)Math.Max(0, Math.Min(255, sumB)),
                        originalAlpha);
                    destSpan[y * Width + x].FromRgba32(finalPixel);

                }
            });
        }
        finally
        {
            // Our work is done, we return the rented temporary memory to the pool.
            sourceBufferOwner.Dispose();
        }


    }

    #endregion


    /// <summary>
    /// Adds a given operation to the operation queue instead of immediately executing it.
    /// Returns the object itself for chaining.
    /// </summary>
    /// <param name="operation">The operation to be performed.</param>
    /// <returns>The same AdvancedBitmap object with the operation added.</returns>
    public AdvancedBitmap<TPixel> Apply(IOperation<TPixel> operation)
    {
        operationQueue.Add(operation);
        return this;
    }


    /// <summary>
    /// Executes all operations pending in the processing queue in order.
    /// This method is usually called implicitly by other methods.
    /// </summary>
    private void Execute()
    {
        // If there is no operation waiting in the queue, do nothing.
        if (!HasPendingChanges) return;

        // To save the entire chain of operations as a single 'Undo' step,
        // push the current state onto the stack before the operation.
        PushToUndoStack();
        var context = new ProcessContext<TPixel>
        {
            SourceImage = this
        };

        foreach (var oq in operationQueue)
        {
            oq.Execute(context);
        }
        operationQueue.Clear();


    }


    /// <summary>
    /// Creates a copy of the current state of the image and adds it to the Undo stack.
    /// The Redo history is cleared when a new operation is performed.
    /// </summary>
    public void PushToUndoStack()
    {
        // Rent a new memory space from the memory pool to copy the existing pixel data into.
        var historyOwner = MemoryPool<TPixel>.Shared.Rent(Width * Height);
        pixelMemory.CopyTo(historyOwner.Memory);

        // Push the owner (slip) of the copy to the undo stack.
        undoStack.Push(historyOwner);

        // Since a new chain of operations has begun, there is no more state to roll forward.
        // Clear the stack by returning all memory in the roll forward stack to the pool.
        while (redoStack.Count > 0)
        {
            redoStack.Pop().Dispose();
        }
    }



    /// <summary>
    /// Indicates whether there is an operation to be undone.
    /// </summary>
    public bool CanUndo => undoStack.Count > 0;

    /// <summary>
    /// Undoes the last action.
    /// </summary>
    public void Undo()
    {
        if (!CanUndo) return;

        // Push the current state to _redoStack to roll it forward.
        redoStack.Push(pixelMemoryOwner);

        // Fetch the previous state from _undoStack and make it the active memory.
        pixelMemoryOwner = undoStack.Pop();
        pixelMemory = pixelMemoryOwner.Memory;
    }


    /// <summary>
    /// Indicates whether a transaction needs to be rolled back.
    /// </summary>
    public bool CanRedo => redoStack.Count > 0;

    /// <summary>
    /// Rolls back an undone transaction.
    /// </summary>
    public void Redo()
    {
        if (!CanRedo) return;

        // Push the current state to _undoStack so you can undo it again.
        undoStack.Push(pixelMemoryOwner);

        // Fetch the next state from _redoStack and make it the active memory.
        pixelMemoryOwner = redoStack.Pop();
        pixelMemory = pixelMemoryOwner.Memory;
    }

    /// <summary>
    /// Creates a deep copy of the current AdvancedBitmap object.
    /// Pixel data is copied to a new memory area, making the clone and the original independent of each other.
    /// </summary>
    /// <returns>A new object that is a copy of this object.</returns>
    public object Clone()
    {
        return new AdvancedBitmap<TPixel>(this);
    }


    private bool disposed = false;


    /// <summary>
    /// Returns all memory resources leased by the class to the MemoryPool.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        // Since this object has been cleaned up by us, we're letting the Garbage Collector know that
        // it doesn't need to try to clean it up separately. This is an optimization.
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
        {
            return;
        }
        if (disposing)
        {
            // Return all leased memory to the pool.
            pixelMemoryOwner?.Dispose();

            while (undoStack.Count > 0)
            {
                undoStack.Pop().Dispose();
            }
            while (redoStack.Count > 0)
            {
                redoStack.Pop().Dispose();
            }

        }

        disposed = true;
    }
}
