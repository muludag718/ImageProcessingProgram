using ImageProcess.Core.Interfaces;
using ImageProcess.Core.Models;
using ImageProcess.Filters.Settings;

namespace ImageProcess.Filters;

public struct GaussianBlurFilter : IFilter<Rgba32>
{
    public readonly string Name => "Gaussian Blur Filter";

    public readonly string Category => "Blur";

    public GaussianBlurSettings Settings { get; private set; }

    public GaussianBlurFilter()
    {
        Settings = new();
    }

    public readonly void Execute(ProcessContext<Rgba32> context)
    {
        var kernel = CreateGaussianKernel(Settings.Radius);
        context.SourceImage.Convolve(kernel, context.ROI);
    }
    private static float[,] CreateGaussianKernel(int radius, float sigma)
    {
        int size = radius * 2 + 1;
        float[,] kernel = new float[size, size];
        double sum = 0.0;

        double twoSigmaSquare = 2.0 * sigma * sigma;
        double sigmaRoot = 1.0 / (Math.Sqrt(2.0 * Math.PI) * sigma);

        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                double exponent = -(x * x + y * y) / twoSigmaSquare;
                double kernelValue = sigmaRoot * Math.Exp(exponent);

                kernel[y + radius, x + radius] = (float)kernelValue;
                sum += kernelValue;
            }
        }

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                kernel[y, x] = (float)(kernel[y, x] / sum);
            }
        }

        return kernel;
    }

    private static float[,] CreateGaussianKernel(int radius)
    {
        float sigma = radius / 2.0f;
        return CreateGaussianKernel(radius, sigma);
    }
}
