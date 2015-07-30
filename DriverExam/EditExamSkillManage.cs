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
    public partial class EditExamSkillManage : CCSkinMain
    {
        private string id = "";
        public EditExamSkillManage()
        {
            InitializeComponent();
        }

        public EditExamSkillManage(string id)
        {
            InitializeComponent();
            this.id = id;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBoxEx.Show("秘籍名不能为空");
                return;
            }

            string sqlString = "";
            if (id != "")
            {
                sqlString = "update ExamSkill set name = '" + txtName.Text.Trim() + "',details = '" + txtContent.Text.Trim() + "' where id = '" + id + "'";               
            }
            else
            {
                sqlString = "insert into ExamSkill (name,details)values('" + txtName.Text.Trim() + "','" + txtContent.Text.Trim() + "')";
            }
            new Tool().ExecuteSqlQuery(sqlString);
            MessageBoxEx.Show("保存成功");
            Close();
        }

        private void EditExamSkillManage_Load(object sender, EventArgs e)
        {
            if (id != "")
            {
                string sqlString = "select name,details from ExamSkill where id = '"+id+"' ";
                DataTable dt = new Tool().ExecuteSqlQuery(sqlString);
                txtName.Text = dt.Rows[0]["name"].ToString();
                txtContent.Text = dt.Rows[0]["details"].ToString();
            }
        }
    }
}
