using CCWin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DriverExam
{
    public partial class EditSubject : CCSkinMain
    {
        string id = "";
        public EditSubject()
        {
            InitializeComponent();
        }

        public EditSubject(string id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!checkValue())
            {
                return;
            }
            string sqlString = "";
            if (id != "")
            {
                sqlString = "update ExamSubject set topic = '" + cbTopic.Text + "',type='" + cbSubjectType.Text + "',question = '" + txtQuestion.Text + "',picture_name='" + txtpicName.Text + "',option_a ='" + txtA.Text + "',option_b = '" + txtB.Text + "',option_c='" + txtC.Text + "',option_d='" + txtD.Text + "',answer='" + txtAnswer.Text + "',section = '" + cbSectionType.SelectedValue.ToString() + "' where id = '"+id+"'";
                new Tool().ExecNonSQLQuery(sqlString);
            }
            else 
            {               
                sqlString = "insert into ExamSubject (topic,type,question,picture_name,option_a,option_b,option_c,option_d,answer,section)values('" + cbTopic.Text + "','" + cbSubjectType.Text + "','" + txtQuestion.Text + "','" + txtpicName.Text + "','" + txtA.Text + "','" + txtB.Text + "','" + txtC.Text + "','" + txtD.Text + "','" + txtAnswer.Text + "','" + cbSectionType.SelectedValue.ToString() + "')";
                new Tool().ExecNonSQLQuery(sqlString);
            }

            MessageBoxEx.Show("保存成功");
            Close();
        }

        private void EditSubject_Load(object sender, EventArgs e)
        {
            LoadDefault();            
        }

        private void LoadDefault()
        {
            string sqlString = "select * from ExamSection";
            cbSectionType.DataSource = new Tool().ExecuteSqlQuery(sqlString);
            cbSectionType.ValueMember = "id";
            cbSectionType.DisplayMember = "section";
            cbSubjectType.SelectedIndex = 0;
            cbTopic.SelectedIndex = 0;
            if (id != "")
            {
                sqlString = sqlString = "select a.id,b.section as 章节类别,topic as 题目主题,type as 题目类型,question as 题目问题,picture_name as 图片名称,option_a as 选项A,option_b as 选项B,option_c as 选项C,option_d as 选项D,answer as 答案 from ExamSubject a left join ExamSection b on a.section = b.id where a.id='"+id+"'";
                DataTable dt = new Tool().ExecuteSqlQuery(sqlString);
                txtA.Text = dt.Rows[0]["选项A"].ToString();
                txtB.Text = dt.Rows[0]["选项B"].ToString();
                txtC.Text = dt.Rows[0]["选项C"].ToString();
                txtD.Text = dt.Rows[0]["选项D"].ToString();
                cbSectionType.Text = dt.Rows[0]["章节类别"].ToString();
                cbTopic.Text = dt.Rows[0]["题目主题"].ToString();
                cbSubjectType.Text = dt.Rows[0]["题目类型"].ToString();
                txtAnswer.Text = dt.Rows[0]["答案"].ToString();
                txtQuestion.Text = dt.Rows[0]["题目问题"].ToString();
                txtpicName.Text = dt.Rows[0]["图片名称"].ToString();
            }
        }

        private void cbSubjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtA.Enabled = cbSubjectType.Text != "判断题";
            txtB.Enabled = cbSubjectType.Text != "判断题";
            txtC.Enabled = cbSubjectType.Text != "判断题";
            txtD.Enabled = cbSubjectType.Text != "判断题";
            if (cbSubjectType.Text == "判断题")
            {
                txtA.Text = "";
                txtB.Text = "";
                txtC.Text = "";
                txtD.Text = "";
            }
        }

        private bool checkValue()
        {
            if (cbSubjectType.Text == "判断题")
            {
                if (txtQuestion.Text.Trim() == "")
                {
                    MessageBoxEx.Show("问题不能为空");
                    return false;
                }

                if (txtAnswer.Text.Trim() == "" || (txtAnswer.Text.Trim() != "Y" && txtAnswer.Text.Trim() != "N"))
                {
                    MessageBoxEx.Show("判断题答案不能为空且答案只能为Y或者N");
                    return false;
                }

            }
            else 
            {
                if (txtQuestion.Text.Trim() == "")
                {
                    MessageBoxEx.Show("问题不能为空");
                    return false;
                }

                if (txtAnswer.Text.Trim() == "")
                {
                    MessageBoxEx.Show("答案不能为空");
                    return false;
                }

                if (txtA.Text.Trim() == "" || txtB.Text.Trim() == "" || txtC.Text.Trim() == "" || txtD.Text.Trim() == "")
                {
                    MessageBoxEx.Show("任一选项不能为空");
                    return false;
                }
            }
            return true;
        }
    }
}
