using System.Drawing;

namespace AirSoft.Service.Common;

public static class ImageHelper
{
    // Create a thumbnail in byte array format from the image encoded in the passed byte array.  
    // (RESIZE an image in a byte[] variable.)  
    public static byte[] CreateThumbnail(byte[] passedImage, int largestSide)
    {
        byte[] returnedThumbnail;

        using (MemoryStream startMemoryStream = new MemoryStream(),
                            newMemoryStream = new MemoryStream())
        {
            // write the string to the stream  
            startMemoryStream.Write(passedImage, 0, passedImage.Length);

            // create the start Bitmap from the MemoryStream that contains the image  
            Bitmap startBitmap = new Bitmap(startMemoryStream);

            // set thumbnail height and width proportional to the original image.  
            int newHeight;
            int newWidth;
            double HW_ratio;
            if (startBitmap.Height > startBitmap.Width)
            {
                newHeight = largestSide;
                HW_ratio = (double)((double)largestSide / (double)startBitmap.Height);
                newWidth = (int)(HW_ratio * (double)startBitmap.Width);
            }
            else
            {
                newWidth = largestSide;
                HW_ratio = (double)((double)largestSide / (double)startBitmap.Width);
                newHeight = (int)(HW_ratio * (double)startBitmap.Height);
            }

            // create a new Bitmap with dimensions for the thumbnail.  
            Bitmap newBitmap = new Bitmap(newWidth, newHeight);

            // Copy the image from the START Bitmap into the NEW Bitmap.  
            // This will create a thumnail size of the same image.  
            newBitmap = ResizeImage(startBitmap, newWidth, newHeight);

            // Save this image to the specified stream in the specified format.  
            newBitmap.Save(newMemoryStream, System.Drawing.Imaging.ImageFormat.Png);

            // Fill the byte[] for the thumbnail from the new MemoryStream.  
            returnedThumbnail = newMemoryStream.ToArray();
        }

        // return the resized image as a string of bytes.  
        return returnedThumbnail;
    }

    // Resize a Bitmap  
    private static Bitmap ResizeImage(Bitmap image, int width, int height)
    {
        Bitmap resizedImage = new Bitmap(width, height);
        using (Graphics gfx = Graphics.FromImage(resizedImage))
        {
            gfx.DrawImage(image, new Rectangle(0, 0, width, height),
                new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
        }
        return resizedImage;
    }
}