using CCWin;
using CCWin.SkinControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DriverExam
{
    public partial class PracticeTest : Skin_DevExpress
    {
        private DateTime startTime = DateTime.Now;
        private int index = 0;
        private Label[] totalLbl = new Label[100];
        private string[] answers = new string[100];//保存用户所选的答案
        private List<int> listNumber;
        private bool isEnd = false;
        private DataTable dt;
        public PracticeTest()
        {
            InitializeComponent();
        }

        private void PracticeTest_Load(object sender, EventArgs e)
        {
            init();
            LoadDefault();
        }

        private void LoadDefault()
        {
            dt = new Tool().ExecuteSqlQuery("select distinct * from ExamSubject");
            setRandomIndex();
            setQuestion();
        }

        private void setQuestion()
        {
            
            btnCommit.Enabled = index > 0;
            btnBack.Enabled = index > 0;
            if (index >= 100)
            {
                btnNext.Enabled = false;
                return;
            }
            btnNext.Enabled = index < dt.Rows.Count;
            clearSelected();
            topicNumber.Text = "" + (index+1);
            if (isEnd)
            {
                btnBack.Enabled = false;
                btnNext.Enabled = false;
                btnCommit.Enabled = false;
            }
            try
            {
                //首先判断该题是否有图片
                pic.Image = dt.Rows[listNumber[index]]["picture_name"].ToString() == "" ? null : new Bitmap(Application.StartupPath + "\\pic\\" + dt.Rows[listNumber[index]]["picture_name"].ToString() + ".png");
            }
            catch
            {
            }

            //判断该题的类型是单选，判断，多选
            if (dt.Rows[listNumber[index]]["type"].ToString() == "多项选择题") //必是多选
            {

                this.checkBoxA.Visible = true;
                this.checkBoxB.Visible = true;
                this.checkBoxC.Visible = true;
                this.checkBoxD.Visible = true;
                this.radioA.Visible = false;
                this.radioB.Visible = false;
                this.radioC.Visible = false;
                this.radioD.Visible = false;

                this.checkBoxA.Text = "A." + dt.Rows[listNumber[index]]["option_a"].ToString();
                this.checkBoxB.Text = "B." + dt.Rows[listNumber[index]]["option_b"].ToString();
                this.checkBoxC.Text = "C." + dt.Rows[listNumber[index]]["option_c"].ToString();
                this.checkBoxD.Text = "D." + dt.Rows[listNumber[index]]["option_d"].ToString();
            }

            if (dt.Rows[listNumber[index]]["type"].ToString() == "单项选择题")//必是单选
            {
                this.checkBoxA.Visible = false;
                this.checkBoxB.Visible = false;
                this.checkBoxC.Visible = false;
                this.checkBoxD.Visible = false;
                this.radioA.Visible = true;
                this.radioB.Visible = true;
                this.radioC.Visible = true;
                this.radioD.Visible = true;

                this.radioA.Text = "A." + dt.Rows[listNumber[index]]["option_a"].ToString();
                this.radioB.Text = "B." + dt.Rows[listNumber[index]]["option_b"].ToString();
                this.radioC.Text = "C." + dt.Rows[listNumber[index]]["option_c"].ToString();
                this.radioD.Text = "D." + dt.Rows[listNumber[index]]["option_d"].ToString();
            }

            if (dt.Rows[listNumber[index]]["type"].ToString() == "判断题")
            {
                this.checkBoxA.Visible = false;
                this.checkBoxB.Visible = false;
                this.checkBoxC.Visible = false;
                this.checkBoxD.Visible = false;
                this.radioA.Visible = true;
                this.radioB.Visible = true;
                this.radioC.Visible = false;
                this.radioD.Visible = false;
                this.radioA.Text = "正确";
                this.radioB.Text = "错误";

            }
            lblquestion.Text = dt.Rows[listNumber[index]]["question"].ToString();
        }
        private void setRandomIndex()
        {
            int random = 0;
            List<int> list = new List<int>();
            bool IsAdd = true;
            int [] testArray = new int [100];
            for (int i = 0; i < 100; i++)
            {
                IsAdd = true;//重置状态
                random = new Random().Next(0,dt.Rows.Count);
                for (int j = 0; j <list.Count; j++)
                {
                    if (list[j] == random)
                    {
                        i--;
                        IsAdd = false;
                        break;                        
                    }
                }
                if (IsAdd)
                {
                    list.Add(random);
                }
            }
            listNumber = list;
        }
        private void init()
        {
            string sqlString = "select * from ExamUser where id= '"+Login.USERID+"'";
            DataTable dtable = new Tool().ExecuteSqlQuery(sqlString);
            lblname.Text = dtable.Rows[0]["real_name"].ToString();
            lbluser.Text = dtable.Rows[0]["name"].ToString();
            for (int i = 0; i < 100; i++)
            {
                Label lbl = new Label();
                lbl.Text = "" + (i + 1);
                lbl.Name = "lbl" + (i + 1);
                flowLayoutPanel1.Controls.Add(lbl);
                totalLbl[i] = lbl;
                lbl.BorderStyle = BorderStyle.FixedSingle;
                lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                lbl.Click += new System.EventHandler(this.lbl_Click);

            }
            timerExam.Start();
        }

        private void timerExam_Tick(object sender, EventArgs e)
        {
            DateTime endTime = DateTime.Now;
            lbltime.Text = endTime.Subtract(this.startTime).Hours.ToString() + "时" + endTime.Subtract(this.startTime).Minutes.ToString() + "分" + endTime.Subtract(this.startTime).Seconds.ToString() + "秒";
        }

        private void PracticeTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            timerExam.Stop();
            timerExam.Enabled = false;           
        }

        private void clearSelected()
        {
            this.checkBoxA.Checked = false;
            this.checkBoxB.Checked = false;
            this.checkBoxC.Checked = false;
            this.checkBoxD.Checked = false;
            this.radioA.Checked = false;
            this.radioB.Checked = false;
            this.radioC.Checked = false;
            this.radioD.Checked = false;
        }

        private void saveSelected()
        {
            string answer = "";
            //首先判断是何种题目
            if (dt.Rows[listNumber[index]]["type"].ToString() == "判断题")
            {
                if (radioA.Checked)
                {
                    answer= "Y";
                }
                else if (radioB.Checked)
                {
                    answer = "N";
                }
            }
            else if(dt.Rows[listNumber[index]]["type"].ToString().Trim() == "多项选择题" && dt.Rows[listNumber[index]]["answer"].ToString().Length > 1)
            {               
                answer += checkBoxA.Checked ? "A" : "";
                answer += checkBoxB.Checked ? "B" : "";
                answer += checkBoxC.Checked ? "C" : "";
                answer += checkBoxD.Checked ? "D" : "";
                answers[this.index] = answer.Trim();
            }
            else if(dt.Rows[listNumber[index]]["type"].ToString().Trim() == "单项选择题" && dt.Rows[listNumber[index]]["answer"].ToString().Length == 1)
            {
                if (radioA.Checked) answer = "A";
                if (radioB.Checked) answer = "B";
                if (radioC.Checked) answer = "C";
                if (radioD.Checked) answer = "D";
            }

            answers[this.index] = answer;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (index >= 100)
            {
                index = 99;
            }
             totalLbl[index].BackColor = Color.Green;
             saveSelected();
             index++;
             setQuestion();           
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (index >= 100)
            {
                index = 99;
            }
            index--;
            setQuestion();
        }

        private void lbl_Click(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            
            if(lbl.Name.ToString().Length==4)
            {
                index =Convert.ToInt16(lbl.Name.ToString().Substring(3,1))-1;
            }
            else if (lbl.Name.ToString().Length == 5)
            {
                index = Convert.ToInt16(lbl.Name.ToString().Substring(3, 2))-1;
            }
            else
            {
                index = Convert.ToInt16(lbl.Name.ToString().Substring(3, 3))-1;
            }
            if (isEnd)
            {
                lblTrue.Text = "正确答案：" + dt.Rows[listNumber[index]]["answer"].ToString();
                lblCustomer.Text = "您的答案：" + answers[index];
            }
            setQuestion();
        }

        private void btnCommit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确认交卷吗", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                btnBack.Enabled = false;
                btnNext.Enabled = false;
                btnCommit.Enabled = false;
                timerExam.Enabled = false;
                checkBoxIsNext.Enabled = false;
                lblresult.Text = "" + getResult() + "分";
                skinPanel4.Enabled = false;
                isEnd = true;
                lblTrue.Text = "正确答案："+dt.Rows[listNumber[index-1]]["answer"].ToString();
                lblCustomer.Text = "您的答案："+answers[index-1];
            }
        }

        private void btnCollect_Click(object sender, EventArgs e)
        {
            if (new Tool().ExecNonSQLQuery("insert into ExamCollect(user_id,subject_id)values('" + Login.USERID + "','" + dt.Rows[this.index]["id"].ToString() + "')") > 0)
            {
                MessageBoxEx.Show("添加成功");
            }
            else
            {
                MessageBoxEx.Show("该题已经是您的收藏题目了，不允许重复添加");
            }
        }

        private void radioA_CheckedChanged(object sender, EventArgs e)
        {

            SkinRadioButton radio = (SkinRadioButton)sender;
            if (!radio.Checked)
            {
                checkBoxIsNext.Enabled = true;
            }
            else
            {
                checkBoxIsNext.Enabled = false;
            }
            if (checkBoxIsNext.Checked && radio.Checked &&!checkBoxA.Visible)
            {
                btnNext_Click(this, new EventArgs());
            }
        }

        private int getResult()
        {
            int result = 0;
            for (int i = 0; i < 100; i++)
            {
                if (dt.Rows[listNumber[i]]["answer"].ToString() == answers[i])
                {
                    result++;

                }
                else 
                {
                    totalLbl[i].BackColor = Color.Red;
                }
            }
            return result;
        }

        private void checkBoxIsNext_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
