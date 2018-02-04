using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fiddler;

namespace FiddlerExtensionDemo
{
    public class ExtensionInterface : UserControl
    {
        private Panel settingPanel;             //设置区域Panel
        private CheckBox enableCheckBox;        //是否启用插件Checkbox
        private Label logNumLabel;              //日志条数标签
        private NumericUpDown logNumber;        //日志条数数值输入框
        private Button applyButton;             //应用设置按钮
        private Label statusLabel;              //当前状态信息显示
        private Label authorLabel;              //作者信息显示

        private Panel logPanel;                 //日志Panel
        private TextBox logTextBox;             //日志显示区域
        
        private decimal logNumMax = 100;                    //插件日志最大显示条数
        public bool isRunning = false;                     //当前是否在运行
        private Queue<String> logs = new Queue<string>();   //当前日志

        public delegate void InvokeResult(string str);       //委托

        public ExtensionInterface()
        {
            this.InitializeComponents();

            TabPage oPage = new TabPage("ofo 跳一跳")
            {
                ImageIndex = (int)Fiddler.SessionIcons.Composer
            };
            oPage.Controls.Add(this);
            FiddlerApplication.UI.tabsViews.TabPages.Add(oPage);

            this.SetStatusLabel();


            this.applyButton.Click += new EventHandler(this.applyButton_Click);
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            this.isRunning = this.enableCheckBox.Checked;
            logNumMax = this.logNumber.Value;

            this.SetStatusLabel();
        }

        public void InitializeComponents()
        {
            this.Dock = DockStyle.Fill;
            this.AutoSize = true;
            this.BackColor = Color.White;

            this.settingPanel = new Panel
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.LightGoldenrodYellow,
                Height = 24
            };

            this.enableCheckBox = new CheckBox
            {
                Text = "是否启用",
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom,
                Checked = this.isRunning,
                TextAlign = ContentAlignment.MiddleLeft,
                Width = 80,
            };

            this.logNumLabel = new Label
            {
                Text = "日志条数:",
                Left = this.enableCheckBox.Right + 16,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left,
                TextAlign = ContentAlignment.MiddleRight,
            };

            this.logNumber = new NumericUpDown
            {
                Left = this.logNumLabel.Right,
                Value = this.logNumMax,
                Maximum = 65535,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left,
                AutoSize = true
            };

            this.applyButton = new Button
            {
                Text = "√ 应用",
                BackColor = Color.White,
                Left = this.logNumber.Right + 64,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true
            };

            this.statusLabel = new Label
            {
                Left = this.applyButton.Right + 64,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left,
                TextAlign = ContentAlignment.MiddleCenter,
            };

            this.authorLabel = new Label
            {
                Text = "By: poklau",
                TextAlign = ContentAlignment.MiddleRight,
                Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom
            };

            this.logPanel = new Panel
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom,
                Top = 30,
            };

            this.logTextBox = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom,
                Multiline = true,
                Enabled = true
            };

            this.settingPanel.Controls.Add(this.enableCheckBox);
            this.settingPanel.Controls.Add(this.logNumLabel);
            this.settingPanel.Controls.Add(this.logNumber);
            this.settingPanel.Controls.Add(this.applyButton);
            this.settingPanel.Controls.Add(this.statusLabel);
            this.settingPanel.Controls.Add(this.authorLabel);

            this.logPanel.Controls.Add(logTextBox);
            
            this.Controls.Add(settingPanel);
            this.Controls.Add(logPanel);
        }

        /// <summary>
        /// 设置运行状态Label的显示
        /// </summary>
        private void SetStatusLabel()
        {
            if (this.isRunning)
            {
                this.statusLabel.ForeColor = Color.Green;
                this.statusLabel.Text = "运行中...";
            }
            else
            {
                this.statusLabel.ForeColor = Color.Red;
                this.statusLabel.Text = "未运行";
            }
        }


        public void Log(String log)
        {
            if(this.logs.Count > this.logNumMax)
            {
                logs.Dequeue();
            }
            if(!this.logTextBox.InvokeRequired)
            {
                this.logTextBox.AppendText("[ " + DateTime.Now.ToShortTimeString() + " ] " + log + "\r\n");
            }
            else
            {
                InvokeResult invokeResult = new InvokeResult(this.Log);
                this.logTextBox.BeginInvoke(invokeResult, log);
            }
            logs.Enqueue(log);
        }
    }
}
