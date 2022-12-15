using System.Diagnostics;
using System.Drawing.Imaging;

namespace MySingingMonstersOverlay
{
    public partial class Form1 : Form
    {

        private int top { get; set; }
        private int bottom { get; set; }
        private int left { get; set; }
        private int right { get; set; }
        private int width { get { return right - left; } }
        private int height { get { return bottom - top; } }
        private IntPtr ID { get; set; }

        private Map map { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var mainRect = new Utils.RECT();
            try
            {
                var procesInfo = Process.GetProcessesByName("MySingingMonsters")[0];
                Console.WriteLine("process {0} {1:x}", procesInfo.ProcessName, procesInfo.Id);
                foreach (ProcessThread threadInfo in procesInfo.Threads)
                {
                    // uncomment to dump thread handles
                    //Console.WriteLine("\tthread {0:x}", threadInfo.Id);
                    IntPtr[] windows = Utils.GetWindowHandlesForThread(threadInfo.Id);
                    if (windows != null && windows.Length > 0)
                        foreach (IntPtr hWnd in windows)
                        {
                            Console.WriteLine("\twindow {0:x} text:{1} caption:{2}", hWnd.ToInt32(), Utils.GetText(hWnd), Utils.GetEditText(hWnd));
                            var rect = new Utils.RECT();
                            Utils.GetWindowRect(hWnd, ref rect);
                            Console.WriteLine($"top: {rect.Top} ");
                            if (rect.Top > 0)
                            {
                                ID = hWnd;
                                mainRect.Top = rect.Top;
                                mainRect.Bottom = rect.Bottom;
                                mainRect.Left = rect.Left;
                                mainRect.Right = rect.Right;
                                break;
                            }

                        }
                }
                this.Size = new Size(mainRect.Right - mainRect.Left, mainRect.Bottom - mainRect.Top);
                this.Location = new Point(mainRect.Left, mainRect.Top);
            }
            catch { }
            this.BackColor = Color.LimeGreen;
            this.TransparencyKey = Color.LimeGreen;
            this.button1.Visible = false;
            this.button2.Visible = true;
            this.TopMost = true;

            top = mainRect.Top;
            bottom = mainRect.Bottom;
            right = mainRect.Right;
            left = mainRect.Left;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap bmpScreenshot;
            try
            {
                bmpScreenshot = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                // Create a graphics object from the bitmap.
                var g = Graphics.FromImage(bmpScreenshot);

                // Take the screenshot from the upper left corner to the right bottom corner.
                g.CopyFromScreen(left, top, 0, 0,
                                 new Size(width, height),
                                 CopyPixelOperation.SourcePaint);

                bmpScreenshot.Save("temp.jpg");

                if (map == null)
                {
                    map = new Map(bmpScreenshot);
                }
                else
                {
                    map.AddTiles(bmpScreenshot);
                }
            }
            catch { }

            if(map == null) map = new Map(new Bitmap("test8x4.jpg"));
            this.label1.Text = "Getting Matches";
            var matches = map.GetMatch();

            if (matches != null && matches.Count == 2)
            {
                var point = map.GetLeftCorner(matches[0]);
                this.textBox1.Left = point.X;
                this.textBox1.Top = point.Y;
                this.textBox1.Text = "" + matches[0];
                point = map.GetLeftCorner(matches[1]);
                this.textBox2.Left = point.X;
                this.textBox2.Top = point.Y;
                this.textBox2.Text = "" + matches[1];
                this.textBox1.Visible = true;
                this.textBox2.Visible = true;
                this.label1.Text = "Got Match";
            }
            else
            {
                this.label1.Text = "No Matches";
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}