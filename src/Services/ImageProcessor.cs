using ImageDuplicateSearcher.Interfaces;
using ImageDuplicateSearcher.Settings;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImageDuplicateSearcher.Services;

/// <summary>
/// Handles processing of images, including scanning directories for image files.
/// </summary>
public class ImageProcessor : IImageProcessor
{
    private readonly ImageDuplicationOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageProcessor"/> class.
    /// </summary>
    /// <param name="options">The image duplication options.</param>
    public ImageProcessor(IOptions<ImageDuplicationOptions> options)
    {
        _options = options.Value;
    }

    // <inheritdoc/>
    public IEnumerable<string> GetImageList()
    {
        if (!Directory.Exists(_options.ImageDirectory))
        {
            throw new FileNotFoundException($"Folder {_options.ImageDirectory} is empty");
        }

        var images = Directory.GetFiles(_options.ImageDirectory).ToList();

        return images.Where(i => _options.SupportedFormats.Contains(Path.GetExtension(i).ToLowerInvariant()));
    }

    // <inheritdoc/>
    public MemoryStream GenerateThumbnail(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Image file not found.", filePath);
        }

        using var image = Image.Load(filePath);
        image.Mutate(x => x.Resize(32, 32));
        var stream = new MemoryStream();
        image.SaveAsJpeg(stream);
        stream.Position = 0;
        return stream;
    }

    // <inheritdoc/>
    public ulong ComputePerceptualHash(MemoryStream thumbnail)
    {
        using var image = Image.Load<Rgba32>(thumbnail);
        image.Mutate(x => x.Resize(32, 32).Grayscale());

        var pixels = new double[32, 32];
        for (int y = 0; y < 32; y++)
        {
            for (int x = 0; x < 32; x++)
            {
                pixels[y, x] = image[x, y].R / 255.0;
            }
        }

        var dct = ComputeDCT(pixels, 32);

        var coeffs = new List<double>();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                coeffs.Add(dct[i, j]);
            }
        }
        coeffs.Sort();
        double median = (coeffs[31] + coeffs[32]) / 2.0;

        ulong hash = 0;
        int bit = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (dct[i, j] > median)
                {
                    hash |= (1UL << bit);
                }
                bit++;
            }
        }
        return hash;
    }

    /// <summary>
    /// Computes the 2D Discrete Cosine Transform (DCT-II) for the given input matrix.
    /// </summary>
    /// <param name="input">The input matrix.</param>
    /// <param name="N">The size of the matrix (NxN).</param>
    /// <returns>The DCT matrix.</returns>
    private static double[,] ComputeDCT(double[,] input, int N)
    {
        var dct = new double[N, N];
        for (int u = 0; u < N; u++)
        {
            double cu = u == 0 ? 1 / Math.Sqrt(2) : 1;
            for (int v = 0; v < N; v++)
            {
                double cv = v == 0 ? 1 / Math.Sqrt(2) : 1;
                double sum = 0;
                for (int x = 0; x < N; x++)
                {
                    for (int y = 0; y < N; y++)
                    {
                        sum += input[y, x] * Math.Cos(Math.PI * u * (2 * x + 1) / (2 * N)) * Math.Cos(Math.PI * v * (2 * y + 1) / (2 * N));
                    }
                }
                dct[u, v] = sum * cu * cv * 2 / N;  // Normalization for DCT-II
            }
        }
        return dct;
    }
}
