using GrammarAnalyzer.Info_Collection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GrammarAnalyzer
{
    public partial class Form1 : Form
    {
        private static List<CheckBox> checkboxes = new List<CheckBox>();
        public static List<int> checkd_positions = new List<int>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkboxes.Count; i++)
            {
                if (checkboxes[i].Checked)
                {
                    checkd_positions.Add(i);
                }
            }
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Dictionary<string, List<string>> NT_CT = ASTGenerationArrayCollector.getN_TC_T;

            CheckBox box;
            int counter = 0;
            foreach (var var in NT_CT)
            {
                //Console.WriteLine(var.Key);
                box = new CheckBox();
                box.Tag = counter.ToString();
                box.Text = var.Key;
                box.AutoSize = true;
                box.Location = new Point(10, counter * 15);
                panel1.Controls.Add(box);
                counter++;
                checkboxes.Add(box);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
