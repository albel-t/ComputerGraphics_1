using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ComputerGraphics_1
{

    public partial class Form1 : Form
    {
        private Bitmap bitmapPrint;
        private Bitmap bitmapDebug;
        private Bitmap bitmapSelect;
        private Pen pen;
        private Color debugColor = Color.Green;
        private Color printColor = Color.Black;
        private int pointSize = 5;
        private Timer timer;
        private int scale;
        private int roundCircle;
        private List<Coord2D> dots;
        private MouseState mouse;
        string selectedTool;
        string methodSolution;
        MRR_info mrr_Info;
        bool rebildGraphic;


        public Form1()
        {
            InitializeComponent();
            InitializeDrawing();
            InitializeTimer();
            mrr_Info = new MRR_info();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void InitializeTimer()
        {
            timer = new Timer();
            timer.Interval = 33;
            timer.Tick += OneTimerTick;
            timer.Start();
        }
        private void InitializeDrawing()
        {
            rebildGraphic = false;
            textBox1.Text = $"{10}";
            scale = int.Parse(textBox1.Text);
            textBox2.Text = $"{10}";
            roundCircle = int.Parse(textBox2.Text);
            selectedTool = comboBox1.SelectedItem?.ToString() ?? "dot";
            methodSolution = comboBox2.SelectedItem?.ToString() ?? "Bresenham's line algorithm";
            bitmapPrint = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bitmapDebug = new Bitmap(bitmapPrint);

            using (Graphics g = Graphics.FromImage(bitmapPrint))
            {
                g.Clear(Color.White);
            }
            pictureBox1.Image = bitmapPrint;

            // Настройка пера
            pen = new Pen(printColor, pointSize);


            pictureBox1.MouseClick += PictureBox1_MouseClick;
            pictureBox1.MouseMove += PictureBox1_MouseMove;
            mouse = new MouseState();


            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }
        private void SetDot(Graphics g, int x, int y)
        {

            using (SolidBrush brush = new SolidBrush(printColor))
            {
                g.FillRectangle(brush, (x / scale) * scale - scale / 2, (y / scale) * scale - scale / 2, scale, scale);
            }

        }
        private void SetPixel(Graphics g, bool debug, int x, int y, double b = 1)
        {
            if (debug)
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255 - (int)(b * 255), 255 - (int)(b * 255))))
                {
                    g.FillRectangle(brush, x * scale - scale / 2, y * scale - scale / 2, scale, scale);
                }
            }
            else
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(255 - (int)(b * 255), 255 - (int)(b * 255), 255 - (int)(b * 255))))
                {
                    g.FillRectangle(brush, x * scale - scale / 2, y * scale - scale / 2, scale, scale);
                }
            }

        }
        public void DrawLineBresenham(Graphics g, bool debug, int x1, int y1, int x2, int y2)
        {
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = x1 < x2 ? 1 : -1;
            int sy = y1 < y2 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                SetPixel(g, debug, x1, y1);

                if (x1 == x2 && y1 == y2) break;

                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x1 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y1 += sy;
                }

            }
        }
        public void DrawLineWu(Graphics g, bool debug, int x1, int y1, int x2, int y2)
        {
            bool steep = Math.Abs(y2 - y1) > Math.Abs(x2 - x1);
            if (steep)
            {
                Swap(ref x1, ref y1);
                Swap(ref x2, ref y2);
            }
            if (x1 > x2)
            {
                Swap(ref x1, ref x2);
                Swap(ref y1, ref y2);
            }
            DrawPointWu(g, debug, steep, x1, y1, 1.0);
            DrawPointWu(g, debug, steep, x2, y2, 1.0);

            int dx = x2 - x1;
            int dy = y2 - y1;
            double gradient = dx == 0 ? 1.0 : (double)dy / dx;
            double y = y1 + gradient;

            for (int x = x1 + 1; x < x2; x++)
            {
                DrawPointWu(g, debug, steep, x, (int)y, 1 - (y - Math.Floor(y)));
                DrawPointWu(g, debug, steep, x, (int)y + 1, y - Math.Floor(y));

                y += gradient;
            }
        }
        private void DrawPointWu(Graphics g, bool debug, bool steep, int x, int y, double brightness)
        {
            if (brightness <= 0 || brightness > 1) return;

            if (steep)
            {
                SetPixel(g, debug, y, x, brightness);
            }
            else
            {
                SetPixel(g, debug, x, y, brightness);
            }
        }
        private void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
        void DrawLine(Graphics g, bool debug, Coord2D dot1, Coord2D dot2)
        {
            switch (methodSolution)
            {
                case "Bresenham's line algorithm":
                    DrawLineBresenham(g, debug,
                        dot1.Get().x, dot1.Get().y,
                        dot2.Get().x, dot2.Get().y);
                    break;

                case "Xiaolin Wu's line algorithm":
                    DrawLineWu(g, debug,
                        dot1.Get().x, dot1.Get().y,
                        dot2.Get().x, dot2.Get().y);
                    break;
            }
        }
        void DrawRect(Graphics g, bool debug, Coord2D dot1, Coord2D dot2)
        {
            DrawLineBresenham(g, debug,
            dot1.Get().x, dot1.Get().y,
            dot2.Get().x, dot1.Get().y);
            DrawLineBresenham(g, debug,
            dot1.Get().x, dot1.Get().y,
            dot1.Get().x, dot2.Get().y);
            DrawLineBresenham(g, debug,
            dot1.Get().x, dot2.Get().y,
            dot2.Get().x, dot2.Get().y);
            DrawLineBresenham(g, debug,
            dot2.Get().x, dot1.Get().y,
            dot2.Get().x, dot2.Get().y);

        }
        public void DrawCircle(Graphics g, bool debug, Coord2D dot1, Coord2D dot2, int segments)
        {
            DrawCircleSimple(g, debug, dot1.Get().x, dot1.Get().y, dot2.Get().x, dot2.Get().y, segments);
        }
        public void DrawCircleSimple(Graphics g, bool debug, int centerX, int centerY, int pointX, int pointY, int segments)
        {
            double radius = Math.Sqrt(Math.Pow(pointX - centerX, 2) + Math.Pow(pointY - centerY, 2));

            if (radius < 1) return;

            double angleStep = 2 * Math.PI / segments;
            double firstAngle = 0;

            int firstX = (int)(centerX + radius * Math.Cos(firstAngle));
            int firstY = (int)(centerY + radius * Math.Sin(firstAngle));

            int prevX = firstX;
            int prevY = firstY;
            for (int i = 1; i <= segments; i++)
            {
                double angle = i * angleStep;
                int x = (int)(centerX + radius * Math.Cos(angle));
                int y = (int)(centerY + radius * Math.Sin(angle));


                switch (methodSolution)
                {
                    case "Xiaolin Wu's line algorithm":
                        DrawLineWu(g, debug, prevX, prevY, x, y);
                        break;

                    case "Bresenham's line algorithm":
                        DrawLineBresenham(g, debug, prevX, prevY, x, y);
                        break;
                }
                prevX = x;
                prevY = y;
            }
        }

        public Bitmap ScaleBitmap(Bitmap sourceBitmap, double scaleX, double scaleY)
        {
            if (sourceBitmap == null)
                throw new ArgumentNullException(nameof(sourceBitmap));

            int newWidth = (int)(sourceBitmap.Width * scaleX);
            int newHeight = (int)(sourceBitmap.Height * scaleY);

            Bitmap scaledBitmap = new Bitmap(newWidth, newHeight);

            using (Graphics g = Graphics.FromImage(scaledBitmap))
            {
                g.Clear(Color.Transparent);
                using (var matrix = new System.Drawing.Drawing2D.Matrix())
                {
                    matrix.Scale((float)scaleX, (float)scaleY);
                    g.Transform = matrix;
                    g.DrawImage(sourceBitmap, 0, 0);
                }
            }

            return scaledBitmap;
        }
        public Bitmap RotateBitmap(Bitmap sourceBitmap, float angleDegrees)
        {
            if (sourceBitmap == null)
                throw new ArgumentNullException(nameof(sourceBitmap));

            float angleRadians = angleDegrees * (float)Math.PI / 180f;

            float cos = (float)Math.Abs(Math.Cos(angleRadians));
            float sin = (float)Math.Abs(Math.Sin(angleRadians));

            int newWidth = (int)(sourceBitmap.Width * cos + sourceBitmap.Height * sin);
            int newHeight = (int)(sourceBitmap.Width * sin + sourceBitmap.Height * cos);

            Bitmap rotatedBitmap = new Bitmap(newWidth, newHeight);

            using (Graphics g = Graphics.FromImage(rotatedBitmap))
            {
                g.Clear(Color.Transparent);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                using (var matrix = new System.Drawing.Drawing2D.Matrix())
                {
                    matrix.Rotate(angleDegrees);
                    g.Transform = matrix;

                    g.DrawImage(sourceBitmap, 0, 0);
                }
            }

            return rotatedBitmap;
        }
        public Bitmap InsertBitmap(Bitmap fromBitmap, Bitmap toBitmap, int tabX, int tabY)
        {
            for (int x = 0; x < fromBitmap.Width; x++)
            {
                for (int y = 0; y < fromBitmap.Height; y++)
                {
                    if (tabX + x >= 0 && tabX + x < toBitmap.Width &&
                         tabY + y >= 0 && tabY + y < toBitmap.Height)
                    {
                        Color Color = fromBitmap.GetPixel(x, y);

                        if (Color.R != 255 || Color.G != 255 || Color.B != 255)
                        {
                            toBitmap.SetPixel(tabX*scale + x, tabY*scale + y, Color);
                        }
                    }
                }
            }
            return toBitmap;
        }
        public void PopupClosed(object sender, FormClosedEventArgs e)
        {
            FormSpace popup = sender as FormSpace;
            if (popup != null && popup.DialogResult == DialogResult.OK)
            {
                mrr_Info = popup.GetResult();
            }
        }
        void BildFigure()
        {
            if (rebildGraphic)
            {
                rebildGraphic = false;
                bool debug = true;
                switch (mouse.currentSelect())
                {
                    case 0:
                        if (mouse.prevSelect() != 1)
                            debug = false;
                        using (Graphics g = Graphics.FromImage(bitmapPrint))
                        {
                            g.DrawImage(bitmapDebug, 0, 0);
                        }

                        goto case 2;
                    case 1:

                        using (Graphics g = Graphics.FromImage(bitmapPrint))
                        {
                            textBoxX1.Text = $"{mouse.firstClick.Get().x}";
                            textBoxY1.Text = $"{mouse.firstClick.Get().y}";
                            switch (selectedTool)
                            {
                                case "circle":
                                case "line":
                                case "dot":
                                    SetDot(g, mouse.firstClick.x, mouse.firstClick.y);
                                    break;
                                case "eraser":
                                    using (SolidBrush brush = new SolidBrush(Color.White))
                                    {
                                        g.FillEllipse(brush, mouse.curentCoord.x, mouse.curentCoord.y, pointSize * 4, pointSize * 4);
                                        rebildGraphic = true;
                                    }
                                    break;
                                case "selection":
                                    DrawRect(g, debug, mouse.firstClick.Get(), mouse.curentCoord.Get());
                                    rebildGraphic = true;
                                    break;
                            }
                        }
                        break;
                    case 2:
                        using (Graphics g = Graphics.FromImage(bitmapPrint))
                            if (mouse.secondClick.exists() || !debug)
                            {

                                textBoxX2.Text = $"{mouse.secondClick.Get().x}";
                                textBoxY2.Text = $"{mouse.secondClick.Get().y}";
                                switch (selectedTool)
                                {
                                    case "line":
                                        DrawLine(g, debug, mouse.firstClick.Get(), mouse.secondClick.Get());
                                        break;
                                    case "circle":
                                        DrawCircle(g, debug, mouse.firstClick.Get(), mouse.secondClick.Get(), roundCircle);
                                        break;
                                    case "selection":

                                        if (debug)
                                        {
                                            DrawRect(g, debug, mouse.firstClick.Get(), mouse.secondClick.Get());
                                        }
                                        bitmapSelect = new Bitmap(Math.Abs(mouse.secondClick.x - mouse.firstClick.x) + 1,
                                                                    Math.Abs(mouse.secondClick.y - mouse.firstClick.y) + 1);
                                        using (Graphics space = Graphics.FromImage(bitmapSelect))
                                        {
                                            if (debug)
                                            {
                                                space.DrawImage(bitmapDebug, 0, 0,
                                                new Rectangle(mouse.firstClick.x, mouse.firstClick.y,
                                                mouse.secondClick.x - mouse.firstClick.x,
                                                mouse.secondClick.y - mouse.firstClick.y),
                                                            GraphicsUnit.Pixel);

                                                FormSpace popup = new FormSpace();
                                                popup.FormClosed += PopupClosed;
                                                popup.ShowDialog(this);
                                            }
                                            //g.DrawImage(bitmapSelect, 3, 3);
                                            g.DrawImage(InsertBitmap(bitmapSelect, bitmapDebug, mrr_Info.move.x, mrr_Info.move.y), 0, 0);

                                            //InsertBitmap(bitmapSelect, bitmapPrint, mrr_Info.move.x, mrr_Info.move.y);
                                            //using (Graphics g_ = Graphics.FromImage(bitmapPrint))
                                            {
                                                // g_.DrawImage(bitmapPrint, 0, 0);
                                                //g_.DrawImage(InsertBitmap(bitmapSelect, bitmapPrint, 3, 3), 0, 0);
                                            }
                                            //bitmapPrint = InsertBitmap(bitmapSelect, bitmapPrint, 3, 3);
                                            //bitmapDebug = InsertBitmap(bitmapSelect, bitmapDebug, 3, 3);
                                            //InsertBitmap(bitmapSelect, bitmapDebug, 10, 10);

                                        }
                                        
                                        break;

                                }
                            }
                        if (!debug)
                            using (Graphics g = Graphics.FromImage(bitmapDebug))
                            {
                                g.DrawImage(bitmapPrint, 0, 0);
                            }
                        break;
                }
                pictureBox1.Refresh();
            }
        }
        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouse.click(e, scale, selectedTool);
                rebildGraphic = true;
            }
        }
        private void OneTimerTick(object sender, EventArgs e)
        {
            BildFigure();
        }
        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Coord2D tmpDot = new Coord2D(e, scale);
            mouse.move(e, scale);
            switch (mouse.nextSelect(selectedTool))
            {
                case 1:
                    textBoxX1.Text = $"{tmpDot.Get().x}";
                    textBoxY1.Text = $"{tmpDot.Get().y}";

                    break;
                case 2:

                    textBoxX2.Text = $"{tmpDot.Get().x}";
                    textBoxY2.Text = $"{tmpDot.Get().y}";
                    break;
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public class MRR_info
        {
            public MRR_info()
            {
                move = new Coord2D(0, 0, 1);
                scale_x = 1;
                scale_y = 1;
                rotate = 0;
            }
            public Coord2D move;
            public double scale_x;
            public double scale_y;
            public int rotate;
        }
        public partial class MouseState
        {
            public Coord2D firstClick;
            public Coord2D secondClick;
            public Coord2D curentCoord;
            private int clickState;
            private int pClickState;
            public MouseState()
            {
                reset();
            }
            public void move(MouseEventArgs e, int scale_)
            {
                curentCoord.Withdraw(e, scale_);
            }
            public void click(MouseEventArgs e, int scale_, string brush = "dot")
            {
                pClickState = clickState;
                switch (brush)
                {
                    case "eraser":
                    case "dot":

                        clickState = (clickState + 1) % 2;
                        break;
                    case "line":
                    case "circle":
                    case "selection":
                        clickState = (clickState + 1) % 3;
                        break;
                }
                switch (clickState)
                {
                    case 1:
                        firstClick.Withdraw(e, scale_);
                        break;
                    case 2:
                        secondClick.Withdraw(e, scale_);
                        break;
                    case 0:
                        //firstClick = new Coord2D(0, 0, 0);
                        //secondClick = new Coord2D(0, 0, 0);
                        break;
                }
            }
            public void reset()
            {
                pClickState = 0;
                clickState = 0;
                firstClick = new Coord2D(0, 0, 0);
                secondClick = new Coord2D(0, 0, 0);
                curentCoord = new Coord2D(0, 0, 0);
            }
            public int currentSelect()
            {
                return clickState;
            }
            public int prevSelect()
            {
                return pClickState;
            }
            public int nextSelect(string brush = "dot")
            {
                switch (brush)
                {
                    case "eraser":
                    case "dot":
                        return ((clickState + 1) % 2);
                    case "line":
                    case "circle":
                    case "selection":
                        return ((clickState + 1) % 3);
                }
                return 0;
            }
        }
        public partial class Coord2D
        {
            public int x;
            public int y;
            private int scale;
            public Coord2D(int x_, int y_, int scale_)
            {
                x = x_;
                y = y_;
                scale = scale_;
            }
            public Coord2D(MouseEventArgs e, int scale_)
            {
                Withdraw(e, scale_);
            }
            public void Withdraw(MouseEventArgs e, int scale_)
            {
                x = e.X;
                y = e.Y;
                scale = scale_;
            }
            public Coord2D Get()
            {
                if (scale == 0)
                    return new Coord2D(0, 0, 1);
                else
                    return new Coord2D(x / scale, y / scale, 1);
            }
            public bool exists()
            {
                return scale != 0;
            }
        }

        private void buttonRebuildWindow_Click(object sender, EventArgs e)
        {

        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            scale = int.Parse(textBox1.Text);
            roundCircle = int.Parse(textBox2.Text);
            selectedTool = comboBox1.SelectedItem?.ToString() ?? "dot";
            methodSolution = comboBox2.SelectedItem?.ToString() ?? "Bresenham's line algorithm";
            if (textBoxX1.Text != "")
                mouse.firstClick.x = int.Parse(textBoxX1.Text) * scale;
            if (textBoxX2.Text != "")
                mouse.firstClick.y = int.Parse(textBoxY1.Text) * scale;
            if (textBoxX2.Text != "")
                mouse.secondClick.x = int.Parse(textBoxX2.Text) * scale;
            if (textBoxY2.Text != "")
                mouse.secondClick.y = int.Parse(textBoxY2.Text) * scale;
            rebildGraphic = true;
        }
    }
}