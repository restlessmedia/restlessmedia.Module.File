using restlessmedia.Module.File;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace System.Drawing
{
  public static class ImageExtensions
  {
    /// <summary>
    /// Saves an image as a jpeg image, with the given quality
    /// </summary>
    /// <param name="path"></param>
    /// <param name="image"></param>
    /// <param name="quality"></param>
    public static void SaveJpeg(this Image image, string path, int quality)
    {
      if (quality < 0 || quality > 100)
      {
        throw new ArgumentOutOfRangeException($"Jpeg image quality must be between 0 and 100, with 100 being the highest quality.  A value of {quality} was specified.");
      }

      using (EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, quality))
      {
        using (EncoderParameters encoderParams = new EncoderParameters(1))
        {
          ImageCodecInfo jpegCodec = GetEncoder(ContentType.Jpeg);
          encoderParams.Param[0] = qualityParam;
          image.Save(path, jpegCodec, encoderParams);
        }
      }
    }

    public static Image Crop(this Image image, int x, int y, int width, int height)
    {
      Rectangle area = new Rectangle(x, y, width, height);
      return Crop(image, area);
    }

    public static Image Crop(this Image image, Rectangle cropArea)
    {
      using (Bitmap bmp = new Bitmap(image))
      {
        Bitmap croppedBmp = bmp.Clone(cropArea, bmp.PixelFormat);
        return croppedBmp as Image;
      }
    }

    /// <summary>
    /// Resizes an image. The caller is responsible for properly closing the image object.
    /// </summary>
    /// <param name="image"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="maintainAspectRatio"></param>
    /// <returns></returns>
    public static Image Resize(this Image image, int width, int height, bool maintainAspectRatio)
    {
      Resize(image.Width, image.Height, ref width, ref height, maintainAspectRatio);
      
      Bitmap bmp = new Bitmap(width, height);

      using (Graphics graphics = Graphics.FromImage((Image)bmp))
      {
        graphics.DrawImage(image, 0, 0, width, height);
        return (Image)bmp;
      }
    }

    /// <summary>
    /// Resizes dimensions
    /// </summary>
    /// <param name="originalWidth"></param>
    /// <param name="originalHeight"></param>
    /// <param name="newWidth"></param>
    /// <param name="newHeight"></param>
    /// <param name="maintainAspectRatio"></param>
    public static void Resize(int originalWidth, int originalHeight, ref int newWidth, ref int newHeight, bool maintainAspectRatio = true)
    {
      if (maintainAspectRatio)
      {
        float percent = 0;
        float percentWidth = 0;
        float percentHeight = 0;

        percentWidth = ((float)newWidth / (float)originalWidth);
        percentHeight = ((float)newHeight / (float)originalHeight);

        if (percentHeight < percentWidth)
        {
          percent = percentHeight;
        }
        else
        {
          percent = percentWidth;
        }

        newWidth = (int)(originalWidth * percent);
        newHeight = (int)(originalHeight * percent);
      }
    }

    public static Image FromBytes(byte[] bytes)
    {
      using (MemoryStream ms = new MemoryStream(bytes))
      {
        return Image.FromStream(ms);
      }
    }

    public static ImageCodecInfo GetEncoder(this string mimeType)
    {
      return ImageCodecInfo.GetImageEncoders().FirstOrDefault(x=> string.Compare(x.MimeType, mimeType, true) == 0);
    }
  }
}