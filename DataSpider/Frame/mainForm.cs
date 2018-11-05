using DataSpider.BLL;
using DataSpider.Common;
using DataSpider.Frame;
using DataSpider.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataSpider
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
            this.notifyIcon.Text = this.Text;
            ReportInfo(this.Text +"  已启动...");

            taskTreeView.ExpandAll();
            Init();
        }

        #region 窗口设置
        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                this.Visible = false; 
                notifyIcon.ShowBalloonTip(3000, "提示", "Hi，我缩小在这里^_^", ToolTipIcon.Info);
            }
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void NotifyExit_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            if (DialogResult.Yes == MessageBox.Show(this, "确认要退出?", "退出", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                notifyIcon.Visible = false;
                Application.Exit();
            } 
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            taskDataView.MultiSelect = false;
            taskDataView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            taskDataView.Columns.Clear();
            taskDataView.Columns.Add("任务ID", "任务ID");
            taskDataView.Columns.Add("网站", "网站");
            taskDataView.Columns.Add("运行状态", "运行状态");
            taskDataView.Columns.Add("频率", "频率");
            taskDataView.Columns.Add("最近启动时间", "最近启动时间");
            taskDataView.Columns.Add("运行次数", "运行次数");
            taskDataView.Columns.Add("失败次数", "失败次数");
            taskDataView.Columns.Add("备注", "备注");

            RefreshDataGridView(TaskMngBLL.GetInstance().TaskInfos);

            tbSysLog.AppendText("运行中：任务正在运行中，请勿修改配置；\r\n");
            tbSysLog.AppendText("\r\n");
            tbSysLog.AppendText("挂起中：任务就绪状态，允许修改配置；\r\n");
            tbSysLog.AppendText("\r\n");
            tbSysLog.AppendText("已停止：任务已被禁用，允许修改配置、启动。\r\n");

            //运行频率
            int interval = 30;
            if (Properties.Settings.Default.runInterval_s > 0)
                interval = Properties.Settings.Default.runInterval_s;
            timerAuto.Interval = interval * 1000;
            timerAuto.Start();
        }

        /// <summary>
        /// 刷新任务列表
        /// </summary>
        public void RefreshDataGridView(List<TaskInfo> taskList)
        {
            taskDataView.Rows.Clear();
            foreach (var data in taskList)
            {
                FillTaksInfo(data);
            }
        }

        private void FillTaksInfo(TaskInfo data, int index = -1)
        {
            if (index == -1)
            {
                index = this.taskDataView.Rows.Add();
            }
            int idx = 0;
            this.taskDataView.Rows[index].Cells[idx++].Value = data.Id;
            this.taskDataView.Rows[index].Cells[idx++].Value = data.Title;
            this.taskDataView.Rows[index].Cells[idx++].Value = EnumDescription.GetText(data.RunState);
            this.taskDataView.Rows[index].Cells[idx++].Value = EnumDescription.GetText(data.Type);
            this.taskDataView.Rows[index].Cells[idx++].Value = data.RunMessage.IRunTime != 0 ? data.RunMessage.LastRunningTimeStr : "";
            this.taskDataView.Rows[index].Cells[idx++].Value = data.RunMessage.RunCount;
            this.taskDataView.Rows[index].Cells[idx++].Value = data.RunMessage.ErrorCount;
            this.taskDataView.Rows[index].Cells[idx++].Value = data.Remark;
            this.taskDataView.Rows[index].Tag = data.Id;
        }

        public void RefreshDataGridView(TaskInfo data)
        {
            if (taskTreeView.SelectedNode.Text == EnumDescription.GetText(data.RunState) || taskTreeView.SelectedNode.Text.Contains("任务"))
            {
                int rowIndex = -1;
                for (int i = 0; i < this.taskDataView.Rows.Count; i++)
                {
                    if ((int)taskDataView.Rows[i].Cells[0].Value == data.Id)
                    {
                        rowIndex = i;
                        break;
                    }
                }
                FillTaksInfo(data, rowIndex);
            }
        }
        #endregion

        #region 消息通知
        /// <summary>
        /// 输出消息
        /// </summary> 
        public void ReportInfo(string info)
        {
            string msg = DateTime.Now.ToString() + "   " + info + "\r\n"; 
            if (tbLog.Text.Length > 102400)
            {
                tbLog.Text = "";
            }
            tbLog.AppendText(msg);
            tbLog.ScrollToCaret();
        } 

        #endregion
        
        /// <summary>
        /// 定时运行
        /// </summary>
        private void timerAuto_Tick(object sender, EventArgs e)
        {
            if (TaskMngBLL.GetInstance().Run(this))
            {
                RefreshDataGridView(TaskMngBLL.GetInstance().TaskInfos);
            }
        }

        #region DataGridView 新增、编辑等

        private void tbNewTask_Click(object sender, EventArgs e)
        {
            taskForm taskForm = new taskForm(this);
            if (taskForm.ShowDialog() == DialogResult.OK)
            {
                var newTask = taskForm.taskInfo;
                int taskId = TaskMngBLL.GetInstance().AddNewTask(newTask);
                if (taskId == 0)
                {
                    MessageBox.Show("保存任务失败了");
                    return;
                }
                FillTaksInfo(newTask);
            }
        }

        private void taskDataView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (taskDataView.SelectedCells.Count != 0)
            {
                int taskid = Convert.ToInt16(taskDataView.CurrentRow.Cells[0].Value);
                var taskInfo = TaskMngBLL.GetInstance().GetTask(taskid);
                if (taskInfo.RunState == ERunState.Running)
                {
                    MessageBox.Show("任务运行中，不能进行编辑");
                    return;
                }
                taskForm taskForm = new taskForm(this);
                taskForm.Set(taskInfo);
                if (taskForm.ShowDialog() == DialogResult.OK)
                {
                    if (TaskMngBLL.GetInstance().UpdateTask(taskForm.taskInfo) == 0)
                    {
                        MessageBox.Show("保存任务失败了");
                        return;
                    }
                }
                RefreshDataGridView(taskInfo);
            }
        }

        private void taskTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var node = e.Node;
            if (node != null)
            {
                var taskList = TaskMngBLL.GetInstance().TaskInfos;
                if (node.Text.Equals("运行中"))
                {
                    RefreshDataGridView(taskList.FindAll(t => t.RunState == ERunState.Running));
                }
                else if (node.Text.Equals("挂起中"))
                {
                    RefreshDataGridView(taskList.FindAll(t => t.RunState == ERunState.HangUp));
                }
                else if (node.Text.Equals("已停止"))
                {
                    RefreshDataGridView(taskList.FindAll(t => t.RunState == ERunState.Stop));
                }
                else
                {
                    RefreshDataGridView(taskList);
                }
            }
        } 
        #endregion

        #region 任务启用、禁用、删除、立即运行

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (taskDataView.SelectedCells.Count != 0)
            {
                int taskid = Convert.ToInt16(taskDataView.CurrentRow.Cells[0].Value);
                var taskInfo = TaskMngBLL.GetInstance().GetTask(taskid);
                taskInfo.Run(this, () =>
                {
                    RefreshDataGridView(taskInfo);
                },
                MessageBox.Show("爬取结果是否保存到数据库？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK);
                RefreshDataGridView(taskInfo);
            }
        }

        private void btnTaskTurnOn_Click(object sender, EventArgs e)
        {
            if (taskDataView.SelectedCells.Count != 0)
            {
                int taskid = Convert.ToInt16(taskDataView.CurrentRow.Cells[0].Value);
                string msg = TaskMngBLL.GetInstance().ChangeState(taskid, 1);
                if (!string.IsNullOrEmpty(msg))
                {
                    MessageBox.Show(msg);
                    return;
                }
                RefreshDataGridView(TaskMngBLL.GetInstance().GetTask(taskid));
            }
        }

        private void btnTaskOff_Click(object sender, EventArgs e)
        {
            if (taskDataView.SelectedCells.Count != 0)
            {
                int taskid = Convert.ToInt16(taskDataView.CurrentRow.Cells[0].Value);
                string msg = TaskMngBLL.GetInstance().ChangeState(taskid, 0);
                if (!string.IsNullOrEmpty(msg))
                {
                    MessageBox.Show(msg);
                    return;
                }
                RefreshDataGridView(TaskMngBLL.GetInstance().GetTask(taskid));
            }
        }

        private void btnTaskDelete_Click(object sender, EventArgs e)
        {
            if (taskDataView.SelectedCells.Count != 0)
            {
                if (MessageBox.Show("确认删除？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    int taskid = Convert.ToInt16(taskDataView.CurrentRow.Cells[0].Value);
                    string msg = TaskMngBLL.GetInstance().ChangeState(taskid, -1);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        MessageBox.Show(msg);
                        return;
                    }
                    RefreshDataGridView(TaskMngBLL.GetInstance().TaskInfos);
                }
            }
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            taskDataView_CellContentDoubleClick(null, null);
        }
        #endregion

        private void btnRemoveError_Click(object sender, EventArgs e)
        {
            if (taskDataView.SelectedCells.Count != 0)
            {
                int taskid = Convert.ToInt16(taskDataView.CurrentRow.Cells[0].Value);
                var task = TaskMngBLL.GetInstance().GetTask(taskid);
                task.RunMessage.ErrorCount = 0;
                RefreshDataGridView(TaskMngBLL.GetInstance().GetTask(taskid));
            }
        }
    }
}
