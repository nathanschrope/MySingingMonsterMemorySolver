

using MySingingMonstersCheat;
using System.Drawing;
using System.Drawing.Drawing2D;

var file = "C:\\Users\\Natha\\source\\repos\\MySingingMonstersCheat\\MySingingMonstersOverlay\\bin\\Debug\\net6.0-windows\\test8x4.jpg";

Bitmap sourceImage = new Bitmap(file);
Bitmap origSourceImage = new Bitmap(file);

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

List<Rectangle> rects = new List<Rectangle>();

for (int x = 0; x < xs.Count; x++)
{
    for (int y = 0; y < ys.Count; y++)
    {
        var endX = x + 1 < xs.Count ? xs[x + 1] : xmax;
        var endY = y + 1 < ys.Count ? ys[y + 1] : ymax;
        rects.Add(new Rectangle(xs[x], ys[y], endX - xs[x], endY - ys[y]));
    }
}

List<Color> colors = new List<Color>();
colors.Add(Color.Black);
colors.Add(Color.Red);
colors.Add(Color.Blue);
colors.Add(Color.Green);
colors.Add(Color.Yellow);
colors.Add(Color.Purple);
colors.Add(Color.Orange);

int colorI = 0;
foreach (var rect in rects)
{
    if (colorI >= colors.Count) colorI = 0;
    sourceImage.SetPixels(rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height, colors[colorI++]);
}


//var inBoxX = xmin + (int)boxWidth + (int)spaceWidth;
//var inBoxY = ymin + (int)boxHeight + (int)spaceHeight;

//if (spaceHeight == 0) spaceHeight = 1;
//if (spaceWidth == 0) spaceWidth = 1;
//while (inBoxX + boxWidth < xmax)
//{
//    sourceImage.DoWorkX(inBoxX, ymin, ymax, (int)spaceWidth*2, out var temp1, out var temp2, out var temp3);
//    inBoxX += (int)boxWidth + (int)spaceWidth + 10;
//}
//while (inBoxY + boxHeight < ymax)
//{
//    sourceImage.DoWorkY(xmin, inBoxY, xmax, (int)spaceHeight*2, out var temp1, out var temp2, out var temp3);
//    inBoxY += (int)boxHeight + (int)spaceHeight;
//}

//find number of boxes
//var numXBox = (double)(xmax - xmin + spaceWidth) / (boxWidth + spaceWidth);
//var numYBox = (double)(ymax - ymin + spaceHeight) / (boxHeight + spaceHeight);


sourceImage.Save("C:\\Users\\Natha\\source\\repos\\MySingingMonstersCheat\\MySingingMonstersOverlay\\bin\\Debug\\net6.0-windows\\finish_white.jpeg");
sourceImage.Dispose();
Bitmap detectedimage = new Bitmap(xmax - xmin, ymax - ymin);

for (int xx = 0; xx < xmax - xmin; xx++)
{
    for (int yy = 0; yy < ymax - ymin; yy++)
    {
        detectedimage.SetPixel(xx, yy, origSourceImage.GetPixel(xx + xmin, yy + ymin));
    }
}


detectedimage.Save("C:\\Users\\Natha\\source\\repos\\MySingingMonstersCheat\\MySingingMonstersOverlay\\bin\\Debug\\net6.0-windows\\finish_clip.jpeg");


Console.WriteLine("Happy New Year");