namespace MySingingMonstersOverlay
{
    public class Tile
    {
        private Rectangle Rect { get; set; }

        public bool ShouldCompare { get { return IsFlipped && !FoundMatch; } }
        public bool IsFlipped { get; set; } = false;
        public bool FoundMatch { get; set; } = false;

        public int X { get { return Rect.X; } }
        public int Y { get { return Rect.Y; } }
        public int Width { get { return Rect.Width; } }
        public int Height { get { return Rect.Height; } }

        public Tile(int x, int y, int width, int height, bool isFlipped)
        {
            Rect = new Rectangle(x, y, width, height);
            IsFlipped = isFlipped;
        }

    }
}
