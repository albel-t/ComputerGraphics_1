using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ComputerGraphics_1
{
    public partial class FormSpace : Form
    {
        public FormSpace()
        {
            InitializeComponent();
            textBox1.Text = "0";
            textBox2.Text = "0";
            textBox3.Text = "1";
            textBox4.Text = "1";
            textBox5.Text = "0";
        }
        public Form1.MRR_info GetResult()
        {
            Form1.MRR_info result = new Form1.MRR_info();

            result.move.x = int.Parse(textBox1.Text);
            result.move.y = int.Parse(textBox2.Text);

            result.scale_x = double.Parse(textBox3.Text);
            result.scale_y = double.Parse(textBox4.Text);

            result.rotate = int.Parse(textBox5.Text);
            
            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
