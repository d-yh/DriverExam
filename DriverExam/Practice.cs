using CCWin;
using CCWin.SkinControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DriverExam
{
    public partial class Practice : Skin_DevExpress
    {
        private DataTable dt;
        private string random = "";
        private string sqlString = "";
        private int index = 0;
        public Practice(string name,string sqlString)
        {
            InitializeComponent();
            this.sqlString = sqlString;
            this.Text = name;
        }

        public Practice(string name, string sqlString, bool isCollect)
        {
            InitializeComponent();
            this.sqlString = sqlString;
            this.btnCollect.Enabled = isCollect;
            this.Text = name;
        }

        public Practice(string name, string sqlString, string random)
        {
            InitializeComponent();
            this.sqlString = sqlString;
            this.random = random;
            this.Text = name;
        }

        // public Practice(string type, string section, string topic, string name)
        //{
        //    InitializeComponent();
        //    this.type = type;
        //    this.section = section;
        //    this.topic = topic;
        //    this.sqlString = "select distinct * from ExamSubject where topic = '" + topic + "' or type='" + type + "' or section = '" + section + "'";
        //    this.Text = name;
        //}
        private void setLayout()
        {
                      
            btnBack.Enabled = index > 0;
            if (index == dt.Rows.Count)
            {
                btnNext.Enabled = false;
                index--;
                if (checkAnswer())
                {
                    lblError.Text = "正确,该题已经是最后一题";
                }
                index++;
                return;
            }
            btnNext.Enabled = index<dt.Rows.Count;
            clearSelected();
            if (this.random != "")
            {
                this.index = new Random().Next(0, dt.Rows.Count);
                this.btnBack.Enabled = false;
            }
            try
            {
                //首先判断该题是否有图片
                pic.Image = dt.Rows[this.index]["picture_name"].ToString() == "" ? null : new Bitmap(Application.StartupPath + "\\pic\\" + dt.Rows[index]["picture_name"].ToString()+".png");
            }catch
            {
                lblError.Text = "未找到图片";
            }
            
            //判断该题的类型是单选，判断，多选
            if (dt.Rows[this.index]["type"].ToString() == "多项选择题") //必是多选
            {
                
                this.checkBoxA.Visible = true;
                this.checkBoxB.Visible = true;
                this.checkBoxC.Visible = true;
                this.checkBoxD.Visible = true;
                this.radioA.Visible = false;
                this.radioB.Visible = false;
                this.radioC.Visible = false;
                this.radioD.Visible = false;

                this.checkBoxA.Text = "A."+dt.Rows[this.index]["option_a"].ToString();
                this.checkBoxB.Text = "B."+dt.Rows[this.index]["option_b"].ToString();
                this.checkBoxC.Text = "C."+dt.Rows[this.index]["option_c"].ToString();
                this.checkBoxD.Text = "D."+dt.Rows[this.index]["option_d"].ToString();
            }

            if (dt.Rows[this.index]["type"].ToString() == "单项选择题")//必是单选
            {
                this.checkBoxA.Visible = false;
                this.checkBoxB.Visible = false;
                this.checkBoxC.Visible = false;
                this.checkBoxD.Visible = false;
                this.radioA.Visible = true;
                this.radioB.Visible = true;
                this.radioC.Visible = true;
                this.radioD.Visible = true;

                this.radioA.Text = "A."+dt.Rows[this.index]["option_a"].ToString();
                this.radioB.Text = "B."+dt.Rows[this.index]["option_b"].ToString();
                this.radioC.Text = "C."+dt.Rows[this.index]["option_c"].ToString();
                this.radioD.Text = "D."+dt.Rows[this.index]["option_d"].ToString();
            }

            if (dt.Rows[this.index]["type"].ToString() == "判断题")
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

            this.lblTopicContent.Text = dt.Rows[index]["question"].ToString();
        }

        //取出数据
        private void LoadDefault()
        {
            dt = new Tool().ExecuteSqlQuery(this.sqlString);
        }

        private void Practice_Load(object sender, EventArgs e)
        {
            LoadDefault();
            setLayout();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            index--;
            //LoadDefault();
            setLayout();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            //if (!this.checkBoxA.Checked && !this.checkBoxB.Checked && !this.checkBoxC.Checked && !this.checkBoxD.Checked && !this.radioA.Checked && !this.radioB.Checked && !this.radioC.Checked && !this.radioD.Checked)
            //{
            //    MessageBoxEx.Show("请选择至少一个答案");
            //    return;
            //}
            if (!checkAnswer())
            {
                if (sqlString == "select a.* from ExamSubject a left join ExamErrorSubject b on a.id= b.subject_id where b.user_id = '" + Login.USERID + "'")
                {
                   MessageBoxEx.Show("正确答案为：" + dt.Rows[this.index]["answer"].ToString());
                   index++;
                   LoadDefault();
                   setLayout();
                   return;
                }
                if (MessageBox.Show("正确答案为：" + dt.Rows[this.index]["answer"].ToString() + "。是否加入我的错题集？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    new Tool().ExecNonSQLQuery("insert into ExamErrorSubject (user_id,subject_id)values('" + Login.USERID + "','" + dt.Rows[this.index]["id"].ToString() + "')");
                }
            }
            index++;
            //LoadDefault();
            setLayout();
        }

        private void clearSelected()
        {
            lblError.Text = "";
            this.checkBoxA.Checked = false;
            this.checkBoxB.Checked = false;
            this.checkBoxC.Checked = false;
            this.checkBoxD.Checked = false;
            this.radioA.Checked = false;
            this.radioB.Checked = false;
            this.radioC.Checked = false;
            this.radioD.Checked = false;
        }

        private bool checkAnswer()
        {
            //首先判断是何种题目
            if (dt.Rows[this.index]["type"].ToString() == "判断题")
            {
                if ((this.radioA.Checked && dt.Rows[this.index]["answer"].ToString().Trim() == "Y") || (this.radioB.Checked && dt.Rows[this.index]["answer"].ToString().Trim() == "N"))
                {
                    
                    return true;
                }
            }

            if (dt.Rows[this.index]["type"].ToString().Trim() == "多项选择题" && dt.Rows[this.index]["answer"].ToString().Length > 1)
            {
                string answer = dt.Rows[this.index]["answer"].ToString().Trim();
                string customerAnswer = "";
                if (checkBoxA.Checked)
                {
                    customerAnswer += "A";
                }

                if (checkBoxB.Checked)
                {
                    customerAnswer += "B";
                }

                if (checkBoxC.Checked)
                {
                    customerAnswer += "C";
                }

                if (checkBoxD.Checked)
                {
                    customerAnswer += "D";
                }
                return answer==customerAnswer.Trim();
            }

            if (dt.Rows[this.index]["type"].ToString().Trim() == "单项选择题" && dt.Rows[this.index]["answer"].ToString().Length == 1)
            {
                string answer = dt.Rows[this.index]["answer"].ToString().Trim();
                if (this.radioA.Checked)
                {
                    if ("A" == answer)
                    {
                        return true;
                    }
                }

                if (this.radioB.Checked)
                {
                    if ("B" == answer)
                    {
                        return true;
                    }
                }

                if (this.radioC.Checked)
                {
                    if ("C" == answer)
                    {
                        return true;
                    }
                }

                if (this.radioD.Checked)
                {
                    if ("D" == answer)
                    {
                        return true;
                    }
                }
            }

            return false;
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
            if (checkBoxIsNext.Checked &&radio.Checked&&!checkBoxA.Visible)
            {
                btnNext_Click(this, new EventArgs());
            }
        }

        private void btnCollect_Click(object sender, EventArgs e)
        {

            if (new Tool().ExecNonSQLQuery("insert into ExamCollect(user_id,subject_id)values('" + Login.USERID + "','" + dt.Rows[this.index]["id"].ToString() + "')")>0)
            {
                MessageBoxEx.Show("添加成功");
            }
            else
            {
                MessageBoxEx.Show("该题已经是您的收藏题目了，不允许重复添加");
            }
        }
    }
}
