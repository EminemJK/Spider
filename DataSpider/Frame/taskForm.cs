using DataSpider.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataSpider.Models;

namespace DataSpider.Frame
{
    public partial class taskForm : Form
    {
        public taskForm(mainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;

            zxtype.Items.Clear();
            zxtype.Items.Add(EnumDescription.GetText(ERunCycle.OnlyOnce));
            zxtype.Items.Add(EnumDescription.GetText(ERunCycle.EveryDay));
            zxtype.Items.Add(EnumDescription.GetText(ERunCycle.EveryWeek));
            zxtype.Items.Add(EnumDescription.GetText(ERunCycle.EveryMonth));
            zxtype.SelectedIndex = 0;
            confgir = new string[] { DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), "0:00:00" };
        }

        protected mainForm mainForm = null;
        public TaskInfo taskInfo = null;
        /// <summary>
        /// index:0 开始日期，2结束日期，3时分秒
        /// </summary>
        public string[] confgir;
        public int IsEndDate = 0;

        /// <summary>
        /// 缓存当前运行任务的状态
        /// </summary>
        private ERunState runState = ERunState.HangUp;

        public void Set(TaskInfo taskInfo)
        {
            if (taskInfo == null)
                taskInfo = new TaskInfo();
            this.taskInfo = taskInfo; 
            if (!string.IsNullOrEmpty(taskInfo.StartDate))
                confgir[0] = taskInfo.StartDate;
            if (!string.IsNullOrEmpty(taskInfo.EndDate))
                confgir[1] = taskInfo.EndDate;
            if (!string.IsNullOrEmpty(taskInfo.RunTime))
                confgir[2] = taskInfo.RunTime;

            #region 填充任务计划选项 
            txtBoxWebSiteName.Text = taskInfo.Title;
            zxtype.SelectedIndex = zxtype.Items.IndexOf(EnumDescription.GetText(taskInfo.Type));
            dateTimePkStart.Value = Convert.ToDateTime(confgir[2]);
            numBoxEveryDay.Value = taskInfo.RunDay;
            dateTimePkOne.Value = Convert.ToDateTime(confgir[0]);
            numBoxEveryWeek.Value = taskInfo.RunWeek;
            string[] weeks = taskInfo.RunWeekDay.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (weeks.Length == 7)
            {
                chkWeek_7.Checked = weeks[0] == "1";
                chkWeek_1.Checked = weeks[1] == "1";
                chkWeek_2.Checked = weeks[2] == "1";
                chkWeek_3.Checked = weeks[3] == "1";
                chkWeek_4.Checked = weeks[4] == "1";
                chkWeek_5.Checked = weeks[5] == "1";
                chkWeek_6.Checked = weeks[6] == "1";
            }
            string[] months = taskInfo.RunMonth.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (months.Length == 12)
            {
                chkMonth_1.Checked = months[0] == "1";
                chkMonth_2.Checked = months[1] == "1";
                chkMonth_3.Checked = months[2] == "1";
                chkMonth_4.Checked = months[3] == "1";
                chkMonth_5.Checked = months[4] == "1";
                chkMonth_6.Checked = months[5] == "1";
                chkMonth_7.Checked = months[6] == "1";
                chkMonth_8.Checked = months[7] == "1";
                chkMonth_9.Checked = months[8] == "1";
                chkMonth_10.Checked = months[9] == "1";
                chkMonth_11.Checked = months[10] == "1";
                chkMonth_12.Checked = months[11] == "1";
            }
            rdBoxMonthDate.Checked = taskInfo.RunMonthWeek == -1;
            rdBoxMonthWeek.Checked = !rdBoxMonthDate.Checked;
            if (rdBoxMonthWeek.Checked)
            {
                cbBoxMonthEveryWeek.SelectedIndex = taskInfo.RunMonthWeek;
            }
            txtBoxRemark.Text = taskInfo.Remark;
            IsEndDate = taskInfo.IsEndDate;
            #endregion

            #region 填充爬虫设置选项
            var spider = taskInfo.SpiderConfig;

            txtBoxWebURL.Text = spider.ListUrl;
            txtBoxKeyWords.Text = spider.KeyWords;
            numBoxPageCount.Value = spider.PageCount;
            numBoxLastDay.Value = spider.ScanLastDay;
            txtBoxListTag_xpath.Text = spider.ListTag;
            txtBoxListLink_xpath.Text = spider.ListTitleTag;
            txtBoxListDate_xpath.Text = spider.ListDateTag;
            txtBoxListLinkURL.Text = spider.ListTitleSpliceUrl;

            txtBoxFirstPageFileName.Text = spider.FirstPageFile;
            txtBoxNextPageFileName.Text = spider.NextPageFile;
            numBoxStart.Value = spider.PageStartInx;

            txtBoxContextTag_xpath.Text = spider.ContextTag;
            txtBoxContextTitle_xpath.Text = spider.ContextTitleTag;
            txtBoxContextDetail_xpath.Text = spider.ContextDetailTag;
            txtBoxCharset.Text = spider.Charset;
            #endregion

            runState = taskInfo.RunState;
            //配置过程中，暂停运行
            taskInfo.RunState = ERunState.Stop;
        }

        private void zxtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (zxtype.Text.Trim())
            {
                case "每天":
                    rw_groupBoxOne.Hide();
                    rw_groupBoxWeek.Hide();
                    rw_groupBoxMonth.Hide();

                    rw_groupBoxDay.Location = new Point(20, 42);
                    rw_groupBoxDay.Show();
                    rw_groupBoxDay.Width = 635;
                    btnRwAdvanced.Enabled = true;
                    break;
                case "每周":
                    rw_groupBoxOne.Hide();
                    rw_groupBoxDay.Hide();
                    rw_groupBoxMonth.Hide(); 

                    rw_groupBoxWeek.Location = new Point(20, 42);
                    rw_groupBoxWeek.Show();
                    rw_groupBoxWeek.Width = 635;
                    btnRwAdvanced.Enabled = true;
                    break;
                case "每月":
                    rw_groupBoxOne.Hide();
                    rw_groupBoxDay.Hide();
                    rw_groupBoxWeek.Hide();

                    rw_groupBoxMonth.Location = new Point(20, 42);
                    rw_groupBoxMonth.Show();
                    rw_groupBoxMonth.Width = 635;
                    btnRwAdvanced.Enabled = true;
                    break;
                case "一次":
                    rw_groupBoxDay.Hide();
                    rw_groupBoxWeek.Hide();
                    rw_groupBoxMonth.Hide();

                    rw_groupBoxOne.Location = new Point(20, 42);
                    rw_groupBoxOne.Show();
                    rw_groupBoxOne.Width = 635;
                    btnRwAdvanced.Enabled = false;
                    break;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            cbBoxMonthEveryWeek.Enabled = false;
            numBoxStart.Enabled = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            numBoxStart.Enabled = false;
            cbBoxMonthEveryWeek.Enabled = true;
        }

        private void btnRwAdvanced_Click(object sender, EventArgs e)
        {
            dateSetForm dateSet = new dateSetForm(); 
            dateSet.Confgir = confgir;
            dateSet.isEndDate = IsEndDate == 1;
            if (dateSet.ShowDialog() == DialogResult.OK)
            {
                confgir = dateSet.Confgir;
                IsEndDate = dateSet.isEndDate == true ? 1 : 0;
            }
        }

        private void btnSave_ButtonClick(object sender, EventArgs e)
        {
            if (taskInfo == null)
                taskInfo = new TaskInfo();

            fillTaskInfo(taskInfo);
            fillSpiderConfig(taskInfo);

            //配置好了，还原运行状态
            taskInfo.RunState = runState;
            this.DialogResult = DialogResult.OK;
        }

        private void fillTaskInfo(TaskInfo taskInfo)
        {
            ///按 enum DayOfWeek的顺序 ,从周日开始计
            int w1 = chkWeek_7.Checked ? 1 : 0, 
                w2 = chkWeek_1.Checked ? 1 : 0, 
                w3 = chkWeek_2.Checked ? 1 : 0,
               w4 = chkWeek_3.Checked ? 1 : 0, 
               w5 = chkWeek_4.Checked ? 1 : 0, 
               w6 = chkWeek_5.Checked ? 1 : 0,
               w7 = chkWeek_6.Checked ? 1 : 0,
               
               m1 = chkMonth_1.Checked ? 1 : 0, m2 = chkMonth_2.Checked ? 1 : 0, m3 = chkMonth_3.Checked ? 1 : 0,
               m4 = chkMonth_4.Checked ? 1 : 0, m5 = chkMonth_5.Checked ? 1 : 0, m6 = chkMonth_6.Checked ? 1 : 0,
               m7 = chkMonth_7.Checked ? 1 : 0, m8 = chkMonth_8.Checked ? 1 : 0, m9 = chkMonth_9.Checked ? 1 : 0,
               m10 = chkMonth_10.Checked ? 1 : 0, m11 = chkMonth_11.Checked ? 1 : 0, m12 = chkMonth_12.Checked ? 1 : 0;

            int oneday = (int)numBoxEveryDay.Value, oneweek = (int)numBoxEveryWeek.Value, onemonth = (int)numBoxMonth.Value;

            string sDate = confgir[0], eDate = confgir[1], HHmmss = confgir[2] = dateTimePkStart.Value.ToString("HH:mm:00"); //精确到分钟，秒容易遗漏
            string zxweekg = w1 + "," + w2 + "," + w3 + "," + w4 + "," + w5 + "," + w6 + "," + w7,
              zxmonth = m1 + "," + m2 + "," + m3 + "," + m4 + "," + m5 + "," + m6 + "," + m7 + "," + m8 + "," + m9 + "," + m10 + "," + m11 + "," + m12;

            ERunCycle taskType = ERunCycle.EveryDay;
            switch (zxtype.Text.Trim())
            {
                case "每天":
                    taskType = ERunCycle.EveryDay;
                    break;
                case "每周":
                    taskType = ERunCycle.EveryWeek;
                    break;
                case "每月":
                    taskType = ERunCycle.EveryMonth;
                    break;
                case "一次":
                    taskType = ERunCycle.OnlyOnce;
                    confgir[0] = dateTimePkOne.Value.ToString("yyyy-MM-dd");
                    break;
            }

            taskInfo.Type = taskType;
            taskInfo.Title = txtBoxWebSiteName.Text;
            taskInfo.RunTime = HHmmss;
            taskInfo.RunDay = oneday;
            taskInfo.RunMonth = zxmonth;
            taskInfo.RunMonthDay = onemonth;
            taskInfo.RunMonthWeek = getWeekIndex();
            taskInfo.RunWeek = oneweek;
            taskInfo.RunWeekDay = zxweekg;
            taskInfo.StartDate = sDate;
            taskInfo.EndDate = eDate;
            taskInfo.Enabled = 1;
            taskInfo.Remark = txtBoxRemark.Text;
            taskInfo.IsEndDate = IsEndDate;
        }

        private void fillSpiderConfig(TaskInfo taskInfo)
        {
            var spider = taskInfo.SpiderConfig;
            spider.ListUrl = txtBoxWebURL.Text;
            spider.KeyWords = txtBoxKeyWords.Text;
            spider.PageCount = (int)numBoxPageCount.Value;
            spider.ScanLastDay = (int)numBoxLastDay.Value;
            spider.ListTag = txtBoxListTag_xpath.Text;
            spider.ListTitleTag = txtBoxListLink_xpath.Text;
            spider.ListDateTag = txtBoxListDate_xpath.Text;
            spider.ListTitleSpliceUrl = txtBoxListLinkURL.Text;

            spider.FirstPageFile = txtBoxFirstPageFileName.Text;
            spider.NextPageFile = txtBoxNextPageFileName.Text; 
            spider.PageStartInx = (int)numBoxStart.Value;

            spider.ContextTag = txtBoxContextTag_xpath.Text;
            spider.ContextTitleTag = txtBoxContextTitle_xpath.Text;
            spider.ContextDetailTag = txtBoxContextDetail_xpath.Text;
            spider.Charset = txtBoxCharset.Text;

        }

        private int getWeekIndex()
        {
            switch(cbBoxMonthEveryWeek.Text)
            {
                case "星期日": return 0;
                case "星期一": return 1;
                case "星期二": return 2;
                case "星期三": return 3;
                case "星期四": return 4;
                case "星期五": return 5;
                case "星期六": return 6; 
                default: return -1;
            }
        }

        private void btnCancel_ButtonClick(object sender, EventArgs e)
        {
            //配置好了，还原运行状态
            if (taskInfo != null)
                taskInfo.RunState = runState;
            this.DialogResult = DialogResult.Cancel;
        }

        private void taskForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (taskInfo != null)
                taskInfo.RunState = runState;
            if (this.DialogResult == DialogResult.OK)
                return;
            this.DialogResult = DialogResult.Cancel;
        }
         
      
        private void btnReserved_Click(object sender, EventArgs e)
        {
            if (taskInfo == null)
            {
                taskInfo = new TaskInfo();
            }
            if (taskInfo.RunState == ERunState.Running)
            {
                MessageBox.Show("正在运行中，勿扰");
                return;
            }
            fillSpiderConfig(taskInfo);
            taskInfo.Run(mainForm,()=>
            { 
                tbReserved.Text = "";
                var dataList = taskInfo.SpiderMan.ResultDataDic;
                if (dataList.Count == 0)
                {
                    tbReserved.Text = "爬取不到数据，尝试扩大扫描时间...";
                }
                else
                {
                    tbReserved.Text = $"共爬取到{ dataList.Values.Count} 条数据...\r\n";
                    StringBuilder sb = new StringBuilder();
                    foreach (var data in dataList.Values)
                    { 
                        sb.AppendLine("标题："+data.Title);
                        sb.AppendLine("日期：" + data.PublishDate);
                        sb.AppendLine("内容：" + (!string.IsNullOrEmpty(data.HtmlContext) ? "有数据，共" + data.HtmlContext.Length + "个字符" : "无数据"));
                        sb.AppendLine("----------------------------------------------");
                    }
                    tbReserved.Text += sb.ToString();
                }

                btnReserved.Enabled = true;
                btnReserved.Text = "点击预览";
                if (!string.IsNullOrEmpty(taskInfo.SpiderMan.ErrorMsg))
                {
                    MessageBox.Show(taskInfo.SpiderMan.ErrorMsg);
                }
            }, false);
            btnReserved.Enabled = false;
            btnReserved.Text = "运行中";
        }
    }
}
