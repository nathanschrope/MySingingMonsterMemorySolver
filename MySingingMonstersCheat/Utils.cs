using System.Drawing;

namespace MySingingMonstersCheat
{
    public static class Utils
    {
        public static bool AreColorsSimilar(Color c1, Color c2, int tolerance)
        {
            return Math.Abs(c1.R - c2.R) < tolerance &&
                   Math.Abs(c1.G - c2.G) < tolerance &&
                   Math.Abs(c1.B - c2.B) < tolerance;
        }


        public static bool AreOnEdge(int x, int y, int width, int height)
        {
            return x < width / 9 || x > width - (width / 9) ||
                y < height / 5 || y > height - (height / 10);
        }
    }
}
