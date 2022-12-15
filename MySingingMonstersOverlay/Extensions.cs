using System.Drawing;

namespace MySingingMonstersOverlay
{
    public static class Extensions
    {
        public static bool DoXPixelsNotHaveColor(this Bitmap bitmap, int xStart, int yStart, int xWidth, Color color)
        {
            for (int x = 0; x < xWidth; x++)
            {
                if (!Utils.AreColorsSimilar(bitmap.GetPixel(xStart + x, yStart), color, 25))
                {
                    return true;
                }
            }
            return false;
        }


        public static bool DoXPixelsHaveSameColor(this Bitmap bitmap, int xStart, int yStart, int xWidth, Color color)
        {
            for (int x = 0; x < xWidth; x++)
            {
                if (!Utils.AreColorsSimilar(bitmap.GetPixel(xStart + x, yStart), color, 25))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool DoYPixelsNotHaveColor(this Bitmap bitmap, int xStart, int yStart, int yWidth, Color color)
        {
            for (int y = 0; y < yWidth; y++)
            {
                if (!Utils.AreColorsSimilar(bitmap.GetPixel(xStart, yStart + y), color, 25))
                {
                    return true;
                }
            }
            return false;
        }


        public static bool DoYPixelsHaveSameColor(this Bitmap bitmap, int xStart, int yStart, int yWidth, Color color)
        {
            for (int y = 0; y < yWidth; y++)
            {
                if (!Utils.AreColorsSimilar(bitmap.GetPixel(xStart, yStart + y), color, 25))
                {
                    return false;
                }
            }
            return true;
        }

        public static void SetPixels(this Bitmap bitmap, int xStart, int yStart, int xEnd, int yEnd, Color color)
        {
            for (int x = xStart; x <= xEnd; x++)
            {
                for (int y = yStart; y <= yEnd; y++)
                {
                    if (x < bitmap.Width && y < bitmap.Height) bitmap.SetPixel(x, y, color);

                }
            }
        }

        public static void DoWorkX(this Bitmap sourceImage, int inBoxX, int inBoxY, int ymax, int checkLineWidth, out List<int> ys)
        {
            ys = new List<int>();
            var spaceHeight = 0;
            var boxHeight = 0;
            var countSpaceX = 0;
            var countBoxX = 0;
            while (sourceImage.DoXPixelsHaveSameColor(inBoxX, inBoxY, checkLineWidth, Color.White))
            {
                if (inBoxY >= ymax) break;
                inBoxY++;
            }

            while (inBoxY + checkLineWidth < ymax)
            {
                var temp1 = inBoxY;
                ys.Add(temp1);
                while (sourceImage.DoXPixelsNotHaveColor(inBoxX, inBoxY, checkLineWidth, Color.White))
                {
                    if (inBoxY + checkLineWidth >= ymax) break;
                    //sourceImage.SetPixels(inBoxX, inBoxY, inBoxX + checkLineWidth, inBoxY, Color.Red);
                    inBoxY++;
                }
                countBoxX++;
                boxHeight += inBoxY - temp1;

                var temp = inBoxY;
                while (sourceImage.DoXPixelsHaveSameColor(inBoxX, inBoxY, checkLineWidth, Color.White))
                {
                    if (inBoxY + checkLineWidth >= ymax) break;
                    //sourceImage.SetPixels(inBoxX, inBoxY, 2000, inBoxY, Color.Blue);
                    inBoxY++;
                }
                spaceHeight += inBoxY - temp;
                countSpaceX++;
            }

            spaceHeight = spaceHeight / countSpaceX;
            boxHeight = boxHeight / countBoxX;
        }

        public static void DoWorkY(this Bitmap sourceImage, int inBoxX, int inBoxY, int xmax, int checkLineWidth, out List<int> xs)
        {
            xs = new List<int>();
            var boxWidth = 0;
            var spaceWidth = 0;
            var countSpaceY = 0;
            var countBoxY = 0;

            while (sourceImage.DoYPixelsHaveSameColor(inBoxX, inBoxY, checkLineWidth, Color.White))
            {
                if (inBoxX + checkLineWidth >= xmax) break;
                inBoxX++;
            }

            while (inBoxX + checkLineWidth < xmax)
            {
                var temp1 = inBoxX;
                xs.Add(temp1);
                while (sourceImage.DoYPixelsNotHaveColor(inBoxX, inBoxY, checkLineWidth, Color.White))
                {
                    if (inBoxX + checkLineWidth >= xmax) break;
                    //sourceImage.SetPixels(inBoxX, inBoxY, inBoxX, inBoxY + checkLineWidth, Color.Red);
                    inBoxX++;
                }
                countBoxY++;
                boxWidth += inBoxX - temp1;

                var temp = inBoxX;
                while (sourceImage.DoYPixelsHaveSameColor(inBoxX, inBoxY, checkLineWidth, Color.White))
                {
                    if (inBoxX + checkLineWidth >= xmax) break;
                    //sourceImage.SetPixels(inBoxX, inBoxY, inBoxX, 1000, Color.Blue);
                    inBoxX++;
                }
                countSpaceY++;
                spaceWidth += inBoxX - temp;
            }

            spaceWidth = spaceWidth / countBoxY;
            boxWidth = boxWidth / countBoxY;
        }

        public static bool CheckFlipped(this Bitmap bitmap, int xStart, int yStart, int xEnd, int yEnd)
        {
            int trimmedxStart = xStart + 5;
            int trimmedxEnd = xEnd - 5;
            int trimmedyStart = yStart + 5;
            int trimmedyEnd = yEnd - 5;
            int grays = 0;
            int graysToSuccess = (xEnd - xStart) * (yEnd - yStart);
            graysToSuccess = (int)(graysToSuccess * (.9));
            for (int x = trimmedxStart; x < trimmedxEnd; x++)
            {
                for (int y = trimmedyStart; y < trimmedyEnd; y++)
                {
                    if (Utils.IsPurple(bitmap.GetPixel(x, y)))
                    {
                        grays++;
                    }
                }
            }


            return grays < 1000;
        }

        public static bool CheckFlipped2(this Bitmap bitmap, int xStart, int yStart, int xEnd, int yEnd)
        {
            Bitmap tile = new Bitmap(xEnd - xStart, yEnd - yStart);

            for (int x = xStart; x < xEnd; x++)
            {
                for (int y = yStart; y < yEnd; y++)
                {
                    tile.SetPixel(x - xStart, y-yStart, bitmap.GetPixel(x,y));
                }
            }

            Bitmap sixTeen = new Bitmap(tile,16, 16);

            sixTeen.Save("thumbnail.jpg");

            Bitmap saved = new Bitmap("thumb.jpg");
            int count = 0;
            for (int x = 0; x < sixTeen.Width; x++)
            {
                for (int y = 0; y  < sixTeen.Height; y++)
                {
                    if (Utils.AreColorsSimilar(sixTeen.GetPixel(x, y), saved.GetPixel(x, y), 75))
                        count++;
                }
            }

            return count < saved.Width * saved.Height * .9;
        }
    }
}
