using static MySingingMonstersOverlay.Utils;

namespace MySingingMonstersOverlay
{
    public class Map
    {
        private List<Tile> tiles;
        private Bitmap origSourceImage;


        public Map(Bitmap file)
        {
            Bitmap sourceImage = file;
            origSourceImage = new Bitmap(file);

            int xmin = sourceImage.Width;
            int xmax = 0;
            int ymin = sourceImage.Height;
            int ymax = 0;

            for (int x = 0; x < sourceImage.Width; x++)
            {
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    var pix = sourceImage.GetPixel(x, y);
                    if (Utils.AreOnEdge(x, y, sourceImage.Width, sourceImage.Height) || !Utils.AreColorsSimilar(pix, Color.Black, 50))
                    {
                        sourceImage.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        if (x < xmin) xmin = x;
                        if (x > xmax) xmax = x;
                        if (y < ymin) ymin = y;
                        if (y > ymax) ymax = y;
                    }
                }

            }

            int checkLineWidth = 30;

            List<int> ys;
            List<int> xs;

            sourceImage.DoWorkX(xmin, ymin, ymax, checkLineWidth, out ys);

            sourceImage.DoWorkY(xmin, ymin, xmax, checkLineWidth, out xs);

            tiles = new List<Tile>();

            for (int x = 0; x < xs.Count; x++)
            {
                for (int y = 0; y < ys.Count; y++)
                {
                    var endX = x + 1 < xs.Count ? xs[x + 1] : xmax;
                    var endY = y + 1 < ys.Count ? ys[y + 1] : ymax;
                    tiles.Add(new Tile(xs[x], ys[y], endX - xs[x], endY - ys[y], origSourceImage.CheckFlipped2(xs[x], ys[y], endX, endY)));
                }
            }

        }

        public List<int> GetMatch()
        {

            origSourceImage.Save("pleasework.jpg");
            var list = new List<int>();

            for (int x = 0; x < tiles.Count; x++)
            {
                for (int y = x + 1; y < tiles.Count; y++)
                {
                    if (tiles[x].ShouldCompare && tiles[y].ShouldCompare)
                    {
                        if (CompareTile(x, y))
                        {
                            list.Add(x);
                            list.Add(y);
                            tiles[x].FoundMatch = true;
                            tiles[y].FoundMatch = true;
                            return list;
                        }
                    }
                }
            }

            return list;
        }

        public bool CompareTile(int rect1, int rect2)
        {
            var rectangle1 = tiles[rect1];
            var rectangle2 = tiles[rect2];

            if (!rectangle1.IsFlipped || !rectangle2.IsFlipped)
            {
                return false;
            }
            for (int x = 0; x < rectangle1.Width; x++)
            {
                for (int y = 0; y < rectangle2.Height; y++)
                {
                    if (rectangle1.X + x < origSourceImage.Width && rectangle1.Y < origSourceImage.Height)
                    {
                        if (!Utils.AreColorsSimilar(origSourceImage.GetPixel(rectangle1.X + x, rectangle1.Y + y), origSourceImage.GetPixel(rectangle2.X + x, rectangle2.Y + y), 150))
                            return false;

                    }
                }
            }

            return true;
        }

        public Point GetLeftCorner(int i)
        {
            return new Point(tiles[i].X, tiles[i].Y);
        }

        public void AddTiles(Bitmap bitmap)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                if (!tiles[i].IsFlipped && !tiles[i].FoundMatch)
                {
                    if (bitmap.CheckFlipped2(tiles[i].X, tiles[i].Y, tiles[i].X + tiles[i].Width, tiles[i].Y + tiles[i].Height))
                    {
                        tiles[i].IsFlipped = true;
                        for (int x = tiles[i].X; x < tiles[i].X + tiles[i].Width; x++)
                        {
                            for (int y = tiles[i].Y; y < tiles[i].Y + tiles[i].Height; y++)
                            {
                                origSourceImage.SetPixel(x, y, bitmap.GetPixel(x,y));
                            }
                        }
                    }
                }
            }
        }
    }
}
