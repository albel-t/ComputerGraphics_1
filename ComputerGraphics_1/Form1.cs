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
        // Добавьте эти поля в начало класса
        private Bitmap bitmapPrint;
        private Bitmap bitmapDebug;
        private Pen pen;
        private Color debugColor = Color.Green;
        private Color printColor = Color.Black;
        private int pointSize = 5;
        private Timer timer;
        private int scale;
        private int roundCircle;
        //private ;
        private List<Coord2D> dots;
        private MouseClickState mouse;
        string selectedTool;
        string methodSolution;

        bool rebildGraphic;


        public Form1()
        {
            InitializeComponent();
            InitializeDrawing();
            InitializeTimer();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // Этот метод можно оставить пустым, если он не нужен
            // Или перенести сюда код инициализации, если хотите
        }
        // Добавьте этот новый метод для инициализации рисования
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
            // Создание Bitmap с размерами PictureBox
            bitmapPrint = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bitmapDebug = new Bitmap(bitmapPrint);

            using (Graphics g = Graphics.FromImage(bitmapPrint))
            {
                g.Clear(Color.White);
            }
            pictureBox1.Image = bitmapPrint;

            // Настройка пера
            pen = new Pen(printColor, pointSize);

            // Подписка на событие мыши (вместо Click используем MouseClick для координат)
            pictureBox1.MouseClick += PictureBox1_MouseClick;
            pictureBox1.MouseMove += PictureBox1_MouseMove;
            mouse = new MouseClickState();

            // Настройка ComboBox, если нужно
            comboBox1.SelectedIndex = 0; // Выбираем "dot" по умолчанию
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
                // Рисуем текущий пиксель
                SetPixel(g, debug, x1, y1);

                // Проверяем, достигли ли конца линии
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

            // Транспонируем, если линия крутая
            if (steep)
            {
                Swap(ref x1, ref y1);
                Swap(ref x2, ref y2);
            }

            // Убеждаемся, что x1 < x2
            if (x1 > x2)
            {
                Swap(ref x1, ref x2);
                Swap(ref y1, ref y2);
            }

            // Рисуем первую точку
            DrawPointWu(g, debug, steep, x1, y1, 1.0);
            DrawPointWu(g, debug, steep, x2, y2, 1.0);

            int dx = x2 - x1;
            int dy = y2 - y1;
            double gradient = dx == 0 ? 1.0 : (double)dy / dx;

            // Первый y-пересечение
            double y = y1 + gradient;

            for (int x = x1 + 1; x < x2; x++)
            {
                // Рисуем два пикселя с разной яркостью
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


        public void DrawCircleSimple(Graphics g, bool debug, int centerX, int centerY, int pointX, int pointY, int segments, bool useWu)
        {
            // Считаем радиус
            double radius = Math.Sqrt(Math.Pow(pointX - centerX, 2) + Math.Pow(pointY - centerY, 2));

            if (radius < 1) return;

            // Шаг угла в радианах
            double angleStep = 2 * Math.PI / segments;

            // Запоминаем первую точку
            double firstAngle = 0;
            int firstX = (int)(centerX + radius * Math.Cos(firstAngle));
            int firstY = (int)(centerY + radius * Math.Sin(firstAngle));

            int prevX = firstX;
            int prevY = firstY;

            // Рисуем линии между точками
            for (int i = 1; i <= segments; i++)
            {
                double angle = i * angleStep;
                int x = (int)(centerX + radius * Math.Cos(angle));
                int y = (int)(centerY + radius * Math.Sin(angle));

                if (useWu)
                    DrawLineWu(g, debug, prevX, prevY, x, y);
                else
                    DrawLineBresenham(g, debug, prevX, prevY, x, y);

                prevX = x;
                prevY = y;
            }


        }
        void BildFigure()
        {
            if (rebildGraphic)
            {
                bool debug = true;
                //bitmapPrint
                //bitmapDebug
                
                switch (mouse.currentSelect())
                {
                    case 0:
                        if(mouse.prevSelect() != 1)
                            debug = false;
                        using (Graphics g = Graphics.FromImage(bitmapPrint))
                        {
                            g.DrawImage(bitmapDebug, 0, 0);
                        }
                        //using (Graphics g = Graphics.FromImage(bitmapDebug))
                        //{
                        //    g.DrawImage(bitmapPrint, 0, 0);
                        //}
                        goto case 2;
                    case 1:
                        
                        using (Graphics g = Graphics.FromImage(bitmapPrint))
                        {
                            textBoxX1.Text = $"{mouse.firstClick.Get().x}";
                            textBoxY1.Text = $"{mouse.firstClick.Get().y}";
                            switch (selectedTool)
                            {
                                case "circle":
                                case "dot":
                                case "line":
                                    SetDot(g, mouse.firstClick.x, mouse.firstClick.y);
                                    break;

                                case "eraser":
                                    using (SolidBrush brush = new SolidBrush(Color.White))
                                    {
                                        g.FillRectangle(brush, mouse.firstClick.Get().x, mouse.firstClick.Get().y, pointSize * 2, pointSize * 2);
                                    }
                                    break;
                            }
                        }
                        bitmapDebug = new Bitmap(bitmapPrint);
                        break;
                    case 2:
                        using (Graphics g = Graphics.FromImage(bitmapPrint))
                            if(mouse.secondClick.exists() || !debug)
                        {

                            textBoxX2.Text = $"{mouse.secondClick.Get().x}";
                            textBoxY2.Text = $"{mouse.secondClick.Get().y}";
                            switch (selectedTool)
                            {
                                case "line":
                                    switch (methodSolution)
                                    {
                                        case "Bresenham's line algorithm":
                                            DrawLineBresenham(g, debug,
                                                mouse.firstClick.Get().x, mouse.firstClick.Get().y,
                                                mouse.secondClick.Get().x, mouse.secondClick.Get().y);
                                            break;

                                        case "Xiaolin Wu's line algorithm":
                                            DrawLineWu(g, debug,
                                                mouse.firstClick.Get().x, mouse.firstClick.Get().y,
                                                mouse.secondClick.Get().x, mouse.secondClick.Get().y);
                                            break;
                                    }
                                    break;
                                case "circle":
                                    switch (methodSolution)
                                    {
                                        case "Bresenham's line algorithm":
                                            DrawCircleSimple(g, debug,
                                                mouse.firstClick.Get().x, mouse.firstClick.Get().y,
                                                mouse.secondClick.Get().x, mouse.secondClick.Get().y,
                                                roundCircle, false);
                                            break;

                                        case "Xiaolin Wu's line algorithm":
                                            DrawCircleSimple(g, debug,
                                                mouse.firstClick.Get().x, mouse.firstClick.Get().y,
                                                mouse.secondClick.Get().x, mouse.secondClick.Get().y,
                                                roundCircle, true);
                                            break;
                                    }
                                    break;
                            }
                        }
                        if(!debug)
                            //using (Graphics g = Graphics.FromImage(bitmapPrint))
                            //{
                            //    g.DrawImage(bitmapDebug, 0, 0);
                            //}
                        using (Graphics g = Graphics.FromImage(bitmapDebug)) 
                        {
                            g.DrawImage(bitmapPrint, 0, 0);
                        }
                        break;
                }
                rebildGraphic = false;
                pictureBox1.Refresh();
            }
        }
        // Добавьте этот обработчик для рисования точек
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
        //private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Coord2D tmpDot = new Coord2D(e, scale);
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
            // Здесь можно менять настройки в зависимости от выбранного инструмента
            

            switch (selectedTool)
            {
                case "eraser":
                    // Для ластика можно увеличить размер
                    pointSize = 10;
                    break;
                default:
                    pointSize = 5;
                    break;
            }
        }

        public partial class MouseClickState
        {
            public Coord2D firstClick;
            public Coord2D secondClick;
            private int clickState;
            private int pClickState;
            public MouseClickState()
            {
                reset();
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
            if(textBoxX1.Text != "")
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