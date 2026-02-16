using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        //private ;
        private List<Coord2D> dots;
        private MouseClickState mouse;
        string selectedTool;
        string methodSolution;

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
            textBox1.Text = $"{10}";
            scale = int.Parse(textBox1.Text);
            selectedTool = comboBox1.SelectedItem?.ToString() ?? "dot";
            methodSolution = comboBox2.SelectedItem?.ToString() ?? "Bresenham's line algorithm";
            // Создание Bitmap с размерами PictureBox
            bitmapPrint = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bitmapDebug = new Bitmap(pictureBox1.Width, pictureBox1.Height);

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
                g.FillRectangle(brush, (x/ scale)* scale - scale / 2, (y/ scale)* scale - scale / 2, scale, scale);
            }
            
        }
        private void SetPixel(Graphics g, int x, int y, double b = 1)
        {
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(255-(int)(b*255), 255-(int)(b*255), 255-(int)(b*255))))

            {
                g.FillRectangle(brush, x  * scale - scale / 2, y * scale - scale / 2, scale, scale);
            }

        }
        public void DrawLineBresenham(Graphics g, int x1, int y1, int x2, int y2)
        {
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = x1 < x2 ? 1 : -1;
            int sy = y1 < y2 ? 1 : -1;
            int err = dx - dy;
    
            while (true)
            {
                // Рисуем текущий пиксель
                SetPixel(g, x1, y1);
        
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
        public void DrawLineWu(Graphics g, int x1, int y1, int x2, int y2)
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
            DrawPointWu(g, steep, x1, y1, 1.0);
            DrawPointWu(g, steep, x2, y2, 1.0);

            int dx = x2 - x1;
            int dy = y2 - y1;
            double gradient = dx == 0 ? 1.0 : (double)dy / dx;

            // Первый y-пересечение
            double y = y1 + gradient;

            for (int x = x1 + 1; x < x2; x++)
            {
                // Рисуем два пикселя с разной яркостью
                DrawPointWu(g, steep, x, (int)y, 1 - (y - Math.Floor(y)));
                DrawPointWu(g, steep, x, (int)y + 1, y - Math.Floor(y));

                y += gradient;
            }
        }

        private void DrawPointWu(Graphics g, bool steep, int x, int y, double brightness)
        {
            if (brightness <= 0 || brightness > 1) return;

            if (steep)
            {
                SetPixel(g, y, x, brightness);
            }
            else
            {
                SetPixel(g, x, y, brightness);
            }
        }

        private void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
        // Добавьте этот обработчик для рисования точек
        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                

                    mouse.click(e, scale, selectedTool);
                    switch(mouse.currentSelect())
                    {
                        case 0:
                            break;
                        case 1:
                        using (Graphics g = Graphics.FromImage(bitmapPrint))
                        {
                            textBoxX1.Text = $"{mouse.firstClick.Get().x}";
                            textBoxY1.Text = $"{mouse.firstClick.Get().y}";
                            switch (selectedTool)
                            {
                                case "dot":
                                case "line":

                                    // Рисуем точку
                                    SetDot(g, mouse.firstClick.x, mouse.firstClick.y);
                                    break;

                                case "eraser":
                                    // Ластик (рисуем белым)
                                    using (SolidBrush brush = new SolidBrush(Color.White))
                                    {
                                        g.FillRectangle(brush, e.X - pointSize, e.Y - pointSize, pointSize * 2, pointSize * 2);
                                    }
                                    break;

                                    // Здесь потом добавите другие инструменты
                            }
                        }
                            break;
                        case 2:
                        using (Graphics g = Graphics.FromImage(bitmapPrint))
                        {
                            textBoxX2.Text = $"{mouse.secondClick.Get().x}";
                            textBoxY2.Text = $"{mouse.secondClick.Get().y}";
                            switch (selectedTool)
                            {
                                case "line":
                                    switch (methodSolution)
                                    {
                                        case "Bresenham's line algorithm":
                                            DrawLineBresenham(g,
                                                mouse.firstClick.Get().x, mouse.firstClick.Get().y,
                                                mouse.secondClick.Get().x, mouse.secondClick.Get().y);
                                            break;

                                        case "Xiaolin Wu's line algorithm":
                                            DrawLineWu(g,
                                                mouse.firstClick.Get().x, mouse.firstClick.Get().y,
                                                mouse.secondClick.Get().x, mouse.secondClick.Get().y);
                                            break;

                                    }

                                    break;
                            }
                        }
                        break; 
                    }

                  

                

                pictureBox1.Refresh();
            }
        }
        private void OneTimerTick(object sender, EventArgs e)
        {

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
            public MouseClickState()
            {
                reset();
            }
            public void click(MouseEventArgs e, int scale_, string brush = "dot")
            {
                switch (brush)
                {
                    case "eraser":
                    case "dot":

                        clickState = (clickState + 1) % 2;
                        break;
                    case "line":
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
                        firstClick = new Coord2D(0, 0, 0);
                        secondClick = new Coord2D(0, 0, 0);
                        break;
                }
            }
            public void reset()
            {
                clickState = 0;
                firstClick = new Coord2D(0, 0, 0);
                secondClick = new Coord2D(0, 0, 0);
            }
            public int currentSelect()
            {
                return clickState;
            }
            public int nextSelect(string brush = "dot")
            {
                switch (brush)
                {
                    case "eraser":
                    case "dot":
                        return ((clickState + 1) % 2);
                    case "line":
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
                return scale == 0;
            }
        }

        private void buttonRebuildWindow_Click(object sender, EventArgs e)
        {

        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            scale = int.Parse(textBox1.Text);
            selectedTool = comboBox1.SelectedItem?.ToString() ?? "dot";
            methodSolution = comboBox2.SelectedItem?.ToString() ?? "Bresenham's line algorithm";
        }
    }
}