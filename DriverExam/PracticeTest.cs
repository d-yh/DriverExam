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
       
        private int totalTime = 0;
        private List<int> selectedNumber = new List<int>();
        private DateTime startTime = DateTime.Now;
        private int index = 0;
        private Label[] totalLbl = new Label[100];
        private string[] answers = new string[100];//保存用户所选的答案
        private List<int> listNumber = new List<int>();//保存题目的随机数
        private bool isEnd = false;
         private string SubjectString = "";
        //private DataTable dt;
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
            getExamData();
            setQuestion();
        }

        private void setQuestion()
        {
            
            DataTable dt = getCurrentSubject();
            IsShowSelect();
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
                pic.Image = dt.Rows[0]["picture_name"].ToString() == "" ? null : new Bitmap(Application.StartupPath + "\\pic\\" + dt.Rows[0]["picture_name"].ToString() + ".png");
            }
            catch
            {
                pic.Image = null;
            }

            //判断该题的类型是单选，判断，多选
            if (dt.Rows[0]["type"].ToString() == "多项选择题") //必是多选
            {
                lblA.Visible = true;
                lblB.Visible = true;
                lblC.Visible = true;
                lblD.Visible = true;
                this.checkBoxA.Visible = true;
                this.checkBoxB.Visible = true;
                this.checkBoxC.Visible = true;
                this.checkBoxD.Visible = true;
                this.radioA.Visible = false;
                this.radioB.Visible = false;
                this.radioC.Visible = false;
                this.radioD.Visible = false;
            }

            if (dt.Rows[0]["type"].ToString() == "单项选择题")//必是单选
            {
                lblA.Visible = true;
                lblB.Visible = true;
                lblC.Visible = true;
                lblD.Visible = true;
                this.checkBoxA.Visible = false;
                this.checkBoxB.Visible = false;
                this.checkBoxC.Visible = false;
                this.checkBoxD.Visible = false;
                this.radioA.Visible = true;
                this.radioB.Visible = true;
                this.radioC.Visible = true;
                this.radioD.Visible = true;
                this.radioA.Text = "A";
                this.radioB.Text = "B";
            }

            if (dt.Rows[0]["type"].ToString() == "判断题")
            {
                this.checkBoxA.Visible = false;
                this.checkBoxB.Visible = false;
                this.checkBoxC.Visible = false;
                this.checkBoxD.Visible = false;
                this.radioA.Visible = true;
                this.radioB.Visible = true;
                this.radioC.Visible = false;
                this.radioD.Visible = false;
                lblA.Visible = false;
                lblB.Visible = false;
                lblC.Visible = false;
                lblD.Visible = false;
                this.radioA.Text = "对";
                this.radioB.Text = "错";

            }
            this.lblInfo.Visible = checkBoxA.Visible;
            this.btnTrue.Visible = checkBoxA.Visible;
            this.lblA.Text = "A." + dt.Rows[0]["option_a"].ToString().Replace("A", "");
            this.lblB.Text = "B." + dt.Rows[0]["option_b"].ToString().Replace("B", "");
            this.lblC.Text = "C." + dt.Rows[0]["option_c"].ToString().Replace("C", "");
            this.lblD.Text = "D." + dt.Rows[0]["option_d"].ToString().Replace("D", "");
            lblquestion.Text = "(" + dt.Rows[0]["type"].ToString()+")" + dt.Rows[0]["question"].ToString();
        }

        private void init()
        {
            string sqlString = "select * from ExamUser where id= '"+Login.USERID+"'";
            DataTable dtable = new Tool().ExecuteSqlQuery(sqlString);
            lblname.Text = dtable.Rows[0]["real_name"].ToString();
            lbluser.Text = dtable.Rows[0]["name"].ToString();
            this.checkBoxIsNext.Checked = true;
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
            totalTime++;
            if (totalTime == 3600)
            {
                DataTable dt = getCurrentSubject();
                this.checkBoxA.Enabled = false;
                this.checkBoxB.Enabled = false;
                this.checkBoxC.Enabled = false;
                this.checkBoxD.Enabled = false;
                this.radioA.Enabled = false;
                this.radioB.Enabled = false;
                this.radioC.Enabled = false;
                this.radioD.Enabled = false;
                btnTrue.Enabled = false;
                lblTrue.Visible = true;
                btnBack.Enabled = false;
                btnNext.Enabled = false;
                btnCommit.Enabled = false;
                timerExam.Enabled = false;
                checkBoxIsNext.Enabled = false;
                lblresult.Text = "" + getResult() + "分";
                isEnd = true;
                string selectAnswer = dt.Rows[0]["answer"].ToString();
                if (index == 0)
                {
                    lblTrue.Text = "正确答案：" + selectAnswer;
                    MessageBoxEx.Show("您的答题时间已到,系统已经强制交卷");
                    return;
                }
                lblTrue.Text = "正确答案：" + selectAnswer;
                MessageBoxEx.Show("您的答题时间已到,系统已经强制交卷");
            }
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
            DataTable dt = getCurrentSubject();
            string answer = "";
            //首先判断是何种题目
            if (dt.Rows[0]["type"].ToString() == "判断题")
            {
                if (radioA.Checked)
                {
                    answer= "对";
                }
                else if (radioB.Checked)
                {
                    answer = "错";
                }
            }
            else if(dt.Rows[0]["type"].ToString().Trim() == "多项选择题")
            {               
                answer += checkBoxA.Checked ? "A" : "";
                answer += checkBoxB.Checked ? "B" : "";
                answer += checkBoxC.Checked ? "C" : "";
                answer += checkBoxD.Checked ? "D" : "";
                answers[this.index] = answer.Trim();
            }
            else if(dt.Rows[0]["type"].ToString().Trim() == "单项选择题" && dt.Rows[0]["answer"].ToString().Length == 1)
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
             //totalLbl[index].BackColor = Color.Gray;
            
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
            string selectAnswer = "";
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
            DataTable dt = getCurrentSubject();
            if (isEnd)
            {
                selectAnswer = dt.Rows[0]["answer"].ToString();
                lblTrue.Text = "正确答案：" + selectAnswer;
            }

            selectAnswer = answers[index];
           
            this.lblCurrentAnswer.Text = "您的选择：" + selectAnswer;
            setQuestion();
        }

        private void btnCommit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确认交卷吗", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DataTable dt = getCurrentSubject();
                this.checkBoxA.Enabled = false;
                this.checkBoxB.Enabled = false;
                this.checkBoxC.Enabled = false;
                this.checkBoxD.Enabled = false;
                this.radioA.Enabled = false;
                this.radioB.Enabled = false;
                this.radioC.Enabled = false;
                this.radioD.Enabled = false;
                btnTrue.Enabled = false;
                lblTrue.Visible = true;
                btnBack.Enabled = false;
                btnNext.Enabled = false;
                btnCommit.Enabled = false;
                timerExam.Enabled = false;
                checkBoxIsNext.Enabled = false;
                lblresult.Text = "" + getResult() + "分";
                isEnd = true;
                string selectAnswer = dt.Rows[0]["answer"].ToString();
                if (index == 0)
                {
                    lblTrue.Text = "正确答案：" + selectAnswer;
                    return;
                }
                lblTrue.Text = "正确答案：" + selectAnswer;
            }
        }

        private void btnCollect_Click(object sender, EventArgs e)
        {
            DataTable dt = getCurrentSubject();
            if (new Tool().ExecNonSQLQuery("insert into ExamCollect(user_id,subject_id)values('" + Login.USERID + "','" + dt.Rows[0]["id"].ToString() + "')") > 0)
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
                lblCurrentAnswer.Text ="您的选择："+ radio.Text;//向用户展示他的选择
                checkBoxIsNext.Enabled = false;
            }

            
            if (checkBoxIsNext.Checked && radio.Checked &&!checkBoxA.Visible)
            {
                btnTrue_Click(this, new EventArgs());
            }
        }

        private int getResult()
        {
            int result = 0;
            int currentIndex = index;
            index = 0;
            for (int i = 0; i < 100; i++)
            {
                DataTable dt = getCurrentSubject();
                if (dt.Rows[0]["answer"].ToString().Trim() == answers[i])
                {
                    totalLbl[i].BackColor = Color.Green;
                    result++;
                }
                else 
                {
                    totalLbl[i].BackColor = Color.Red;
                }
                index++;
            }
            index = currentIndex;
            return result;
        }

        /// <summary>
        /// 检查该题用户是否做过
        /// </summary>
        private void IsShowSelect()
        {
            for (int i = 0; i < selectedNumber.Count; i++)
            {
                if (index == selectedNumber[i])//说明有
                {
                    string selectAnswer = answers[index];
                    lblCurrentAnswer.Visible = true;
                    lblCurrentAnswer.Text = "您的选择：" + selectAnswer;
                    return;
                }
            }
            lblCurrentAnswer.Text = "您的选择:";
        }

        private void btnTrue_Click(object sender, EventArgs e)
        {          
            saveSelected();
            string selectAnswer = "" + (this.index + 1) + ":" + answers[index];
            totalLbl[index].Text =selectAnswer;
            selectedNumber.Add(index);//保存所做的索引
            //进入下一题
            btnNext_Click(this, new EventArgs());
        }

        /// <summary>
        /// 根据比例考试数据共100条
        /// </summary>
        private void getExamData()
        {
            try
            {
                DataTable data;
                string newSectionId = new Tool().ExecuteSqlQuery("select id from ExamSection where section = '新规新题'").Rows[0]["id"].ToString();
                string[] sixSectionId = new string[6];
                string[] sevenSectionId = new string[7];
                string[] eightSectionId = new string[8];
                //赋值
                data = new Tool().ExecuteSqlQuery("select id from ExamSection where section = '道路交通安全法律法规' or section = '道路运输法律法规' or section = '教育学.教育心理学及其应用' or section='教学方法及规范化教学' or section='车辆结构与安全性能' or section='环保与节能驾驶'");

                for (int i = 0; i < data.Rows.Count; i++)
                {
                    sixSectionId[i] = data.Rows[i]["id"].ToString();
                }

                data = new Tool().ExecuteSqlQuery("select id from ExamSection where section = '道路交通安全法律法规' or section = '道路运输法律法规' or section = '教育学.教育心理学及其应用' or section='教学方法及规范化教学' or section='车辆结构与安全性能' or section='环保与节能驾驶' or section = '教学手段'");
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    sevenSectionId[i] = data.Rows[i]["id"].ToString();
                }

                data = new Tool().ExecuteSqlQuery("select id from ExamSection where section = '道路交通安全法律法规' or section = '道路运输法律法规' or section = '教育学.教育心理学及其应用' or section='教学方法及规范化教学' or section='车辆结构与安全性能' or section='应急驾驶' or section = '驾驶员培训教学大纲' or section = '教案的编写'");
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    eightSectionId[i] = data.Rows[i]["id"].ToString();
                }

                #region 装载判断
                //先获取难题中的两道图片题
                string sqlString = "select * from ExamSubject where problem = '难题强化' and not picture_name is null and picture_name <>'' and type='判断题'";
                data = new Tool().ExecuteSqlQuery(sqlString);
                List<int> list = new List<int>();
                list = getRandom(data, 2);
                for (int i = 0; i < list.Count; i++)
                {
                    listNumber.Add(Convert.ToInt16(data.Rows[list[i]]["random_number"].ToString()));
                }
                //获取剩下的14道判断难题
                sqlString = "select * from ExamSubject where problem = '难题强化' and (picture_name is null or picture_name ='') and type='判断题'";
                data = new Tool().ExecuteSqlQuery(sqlString);

                list = getRandom(data, 14);

                for (int i = 0; i < list.Count; i++)
                {
                    listNumber.Add(Convert.ToInt16(data.Rows[list[i]]["random_number"].ToString()));
                }
                //判断难题装载完毕

                //装载判断 各一题
                for (int i = 0; i < sixSectionId.Length; i++)
                {
                    sqlString = "select a.* from ExamSubject a left join ExamSection b on a.section = b.id  where a.type = '判断题' and b.id = '"+sixSectionId[i]+"' and(a.problem is null or a.problem ='')";
                    data = new Tool().ExecuteSqlQuery(sqlString);
                    list = getRandom(data, 1);
                    listNumber.Add(Convert.ToInt16(data.Rows[list[0]]["random_number"].ToString()));
                }

                //装载8道新题新规
                sqlString = "select a.* from ExamSubject a left join ExamSection b on a.section = b.id  where a.type = '判断题' and b.id = '" + newSectionId + "'";
                data = new Tool().ExecuteSqlQuery(sqlString);
                list = getRandom(data, 8);

                for (int i = 0; i < list.Count; i++)
                {
                    listNumber.Add(Convert.ToInt16(data.Rows[list[i]]["random_number"].ToString()));
                }

                //判断题装载完毕
                #endregion

                #region 装载单选
                //装载5道图片题
                sqlString = "select * from ExamSubject where problem = '难题强化' and not picture_name is null and picture_name <>'' and type='单项选择题'";
                data = new Tool().ExecuteSqlQuery(sqlString);
                list = getRandom(data, 5);
                for (int i = 0; i < list.Count; i++)
                {
                    listNumber.Add(Convert.ToInt16(data.Rows[list[i]]["random_number"].ToString()));
                }

                //装载剩下的11道难题
                sqlString = "select * from ExamSubject where problem = '难题强化' and (picture_name is null or picture_name ='') and type='单项选择题'";
                data = new Tool().ExecuteSqlQuery(sqlString);
                list = getRandom(data, 11);
                for (int i = 0; i < list.Count; i++)
                {
                    listNumber.Add(Convert.ToInt16(data.Rows[list[i]]["random_number"].ToString()));
                }

                //装载各章节各两题

                for (int i = 0; i < sevenSectionId.Length; i++)
                {
                    sqlString = "select a.* from ExamSubject a left join ExamSection b on a.section = b.id  where a.type = '单项选择题' and b.id = '" + sevenSectionId[i] + "' and (a.problem is null or a.problem = '')";
                    data = new Tool().ExecuteSqlQuery(sqlString);
                    list = getRandom(data, 2);

                    for (int j = 0; j < list.Count; j++)
                    {

                        listNumber.Add(Convert.ToInt16(data.Rows[list[j]]["random_number"].ToString()));
                    }
                }
                #endregion

                #region 装载多选
                //先获取难题中的3道图片题
                sqlString = "select * from examsubject where problem = '难题强化' and not picture_name is null and picture_name <>'' and type='多项选择题'";
                data = new Tool().ExecuteSqlQuery(sqlString);
                list = getRandom(data, 3);
                for (int i = 0; i < list.Count; i++)
                {
                    listNumber.Add(Convert.ToInt16(data.Rows[list[i]]["random_number"].ToString()));
                }

                //装载剩下的13道难题
                sqlString = "select * from ExamSubject where problem = '难题强化' and (picture_name is null or picture_name ='') and type='多项选择题'";
                data = new Tool().ExecuteSqlQuery(sqlString);
                list = getRandom(data, 13);
                for (int i = 0; i < list.Count; i++)
                {
                    listNumber.Add(Convert.ToInt16(data.Rows[list[i]]["random_number"].ToString()));
                }

                //装载各章节各3题

                for (int i = 0; i < eightSectionId.Length; i++)
                {
                    sqlString = "select a.* from ExamSubject a left join ExamSection b on a.section = b.id  where a.type = '多项选择题' and b.id = '" + eightSectionId[i] + "' and (a.problem is null or a.problem = '') ";
                    data = new Tool().ExecuteSqlQuery(sqlString);
                    list = getRandom(data, 3);

                    for (int j = 0; j < list.Count; j++)
                    {
                        listNumber.Add(Convert.ToInt16(data.Rows[list[j]]["random_number"].ToString()));                        
                    }

                }

                
                for (int i = 0; i < listNumber.Count; i++)
                {
                    if (listNumber.Contains(i))
                    {
                        MessageBox.Show("发现重复值");
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
            }           
        }

        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="needNumber">需要几个随机数</param>
        /// <returns></returns>
        private List<int> getRandom(DataTable data,int needNumber)
        {
            int random = 0;
            List<int> currentList = new List<int>();
            List<int> saveList = new List<int>();
            int count = data.Rows.Count;
            for (int i = 0; i < count; i++)//初始化集合
            {
                currentList.Add(i);
            }

            for (int i = 0; i < needNumber; i++)//
            {
                random = new Random().Next(0,currentList.Count);
                saveList.Add(currentList[random]);
                currentList.RemoveAt(random);
            }
            return saveList;
        }

        private DataTable getCurrentSubject()
        {
            SubjectString = "select * from ExamSubject where random_number = '" + listNumber[index] + "'";
            DataTable dt = new Tool().ExecuteSqlQuery(SubjectString);
            return dt;
        }
    }
}
