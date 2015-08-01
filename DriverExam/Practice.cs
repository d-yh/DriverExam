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
    public partial class Practice : CCSkinMain
    {
        private DataTable dt;
        private string random = "";
        private string sqlString = "";
        private int index = 0;
        private bool isError = true;
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

        private void setData()
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
            clearColor();
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

                this.checkBoxA.Text = "A." + dt.Rows[this.index]["option_a"].ToString().Replace("A","");
                this.checkBoxB.Text = "B." + dt.Rows[this.index]["option_b"].ToString().Replace("B","");
                this.checkBoxC.Text = "C." + dt.Rows[this.index]["option_c"].ToString().Replace("C","");
                this.checkBoxD.Text = "D." + dt.Rows[this.index]["option_d"].ToString().Replace("D","");
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
                this.radioA.Text = "对";
                this.radioB.Text = "错";

            }

            string section = new Tool().ExecuteSqlQuery("select section from ExamSection where id = '" + dt.Rows[index]["section"].ToString() + "'").Rows[0]["section"].ToString();
            this.lblTopicContent.Text ="("+section+")"+ dt.Rows[index]["question"].ToString();
        }

        //取出数据
        private void LoadDefault()
        {
            pic.MouseHover += new System.EventHandler(this.pictureBoxMouseOn);
            pic.MouseLeave += new System.EventHandler(this.pictureBoxMouseOut);
            this.TopMost = true;
            dt = new Tool().ExecuteSqlQuery(this.sqlString);
        }

        private void pictureBoxMouseOut(object sender, EventArgs e)
        {
            if (pic.Image != null)
            {
                picMax.Visible = false;
            }

        }

        private void pictureBoxMouseOn(object sender, EventArgs e)
        {
            if (pic.Image != null)
            {
                picMax.Image = pic.Image;
                picMax.Visible = true;
                
            }
        }

        private void Practice_Load(object sender, EventArgs e)
        {
            LoadDefault();
            setData();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            isError = true;
            index--;
            setData();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (!checkAnswer()&&isError)//检查答案是否正确
            {
                isError = false;
                setAnswerStyle();
                lblError.Text = "正确答案：" + dt.Rows[this.index]["answer"].ToString();
                if (checkBoxIsShowTrue.Checked)//检查是否查看详解
                {
                    lblErrorDetail.Text = dt.Rows[this.index]["error_detail"].ToString();                    
                }
                if (sqlString == "select a.* from ExamSubject a left join ExamErrorSubject b on a.id= b.subject_id where b.user_id = '" + Login.USERID + "'")
                {
                    return;
                }
                new Tool().ExecNonSQLQuery("insert into ExamErrorSubject (user_id,subject_id)values('" + Login.USERID + "','" + dt.Rows[this.index]["id"].ToString() + "')");              
                return;
            }
            else if (checkAnswer())
            {
                index++;
                setData();
            }

            if (!isError)
            {
                index++;
                isError = true;
                setData();
            }            
        }

        private void clearSelected()
        {
            lblError.Text = "";
            lblErrorDetail.Text = "";
            this.checkBoxA.Checked = false;
            this.checkBoxB.Checked = false;
            this.checkBoxC.Checked = false;
            this.checkBoxD.Checked = false;
            this.radioA.Checked = false;
            this.radioB.Checked = false;
            this.radioC.Checked = false;
            this.radioD.Checked = false;            
        }

        /// <summary>
        /// 还原颜色为黑色
        /// </summary>
        private void clearColor()
        {
            checkBoxA.ForeColor = Color.Black;
            checkBoxB.ForeColor = Color.Black;
            checkBoxC.ForeColor = Color.Black;
            checkBoxD.ForeColor = Color.Black;
            radioA.ForeColor = Color.Black;
            radioB.ForeColor = Color.Black;
            radioC.ForeColor = Color.Black;
            radioD.ForeColor = Color.Black;
        }

        private bool checkAnswer()
        {
            //首先判断是何种题目
            if (dt.Rows[this.index]["type"].ToString() == "判断题")
            {
                if ((this.radioA.Checked && dt.Rows[this.index]["answer"].ToString().Trim() == "对") || (this.radioB.Checked && dt.Rows[this.index]["answer"].ToString().Trim() == "错"))
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
            if (index == dt.Rows.Count)
            {
                index--;
            }
            if (new Tool().ExecNonSQLQuery("insert into ExamCollect(user_id,subject_id)values('" + Login.USERID + "','" + dt.Rows[this.index]["id"].ToString() + "')")>0)
            {
                MessageBoxEx.Show("添加成功");
            }
            else
            {
                MessageBoxEx.Show("该题已经是您的收藏题目了，不允许重复添加");
            }
        }

        private void setAnswerStyle()
        {
            //首先判断是何种题目
            if (dt.Rows[this.index]["type"].ToString() == "判断题")
            {
                if (radioA.Checked)
                {
                    radioA.ForeColor = Color.Red;
                    radioB.ForeColor = Color.Green;
                }
                else
                {
                    radioB.ForeColor = Color.Red;
                    radioA.ForeColor = Color.Green;
                }

            }

            if (dt.Rows[this.index]["type"].ToString().Trim() == "多项选择题" && dt.Rows[this.index]["answer"].ToString().Length > 1)
            {
                //先将正确答案设置为绿色
                string answer = dt.Rows[this.index]["answer"].ToString().Trim();
                if (answer.IndexOf("A") != -1)
                {
                    checkBoxA.ForeColor = Color.Green;
                }

                if (answer.IndexOf("B") != -1)
                {
                    checkBoxB.ForeColor = Color.Green;
                }

                if (answer.IndexOf("C") != -1)
                {
                    checkBoxC.ForeColor = Color.Green;
                }

                if (answer.IndexOf("D") != -1)
                {
                    checkBoxD.ForeColor = Color.Green;
                }
               
                //在判断用户所选的答案
                if (checkBoxA.Checked && answer.IndexOf("A") == -1)
                {
                    checkBoxA.ForeColor = Color.Red;
                }

                if (checkBoxB.Checked && answer.IndexOf("B") == -1)
                {
                    checkBoxB.ForeColor = Color.Red;
                }

                if (checkBoxC.Checked && answer.IndexOf("C") == -1)
                {
                    checkBoxC.ForeColor = Color.Red;
                }

                if (checkBoxD.Checked && answer.IndexOf("D") == -1)
                {
                    checkBoxD.ForeColor = Color.Red;
                }
            }

            if (dt.Rows[this.index]["type"].ToString().Trim() == "单项选择题" && dt.Rows[this.index]["answer"].ToString().Length == 1)
            {
                string answer = dt.Rows[this.index]["answer"].ToString().Trim();
                if (this.radioA.Checked)
                {
                    radioA.ForeColor = Color.Red;
                }

                if (this.radioB.Checked)
                {
                    radioB.ForeColor = Color.Red;
                }

                if (this.radioC.Checked)
                {
                    radioC.ForeColor = Color.Red;
                }

                if (this.radioD.Checked)
                {
                    radioD.ForeColor = Color.Red;
                }

                if (answer == "A") radioA.ForeColor = Color.Green;
                if (answer == "B") radioB.ForeColor = Color.Green;
                if (answer == "C") radioC.ForeColor = Color.Green;
                if (answer == "D") radioD.ForeColor = Color.Green;
                    
            }
        }
    }
}
