using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DataSpider.Frame
{
    public partial class dateSetForm : Form
    {
        public dateSetForm()
        {
            InitializeComponent();
        }
        private string[] confgir;
        public string[] Confgir { get => confgir; set => confgir = value; }

        public bool isEndDate = false;

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker2.Enabled = checkBox1.Checked;
        }

        private void task_set_Load(object sender, EventArgs e)
        {
            dateTimePicker2.Enabled = checkBox1.Checked = isEndDate;
            if (confgir[0] != "") {dateTimePicker1.Text= confgir[0]; }

            if (confgir[1] != "") { dateTimePicker2.Text = confgir[1]; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            confgir[0] = dateTimePicker1.Text;
            isEndDate = checkBox1.Checked;
            if (isEndDate)
                confgir[1] = dateTimePicker2.Text;

            this.DialogResult = DialogResult.OK;
        }
    }
}