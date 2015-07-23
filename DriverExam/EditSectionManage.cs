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
    public partial class EditSectionManage : CCSkinMain
    {
        private string id = "";
        public EditSectionManage()
        {
            InitializeComponent();
            this.Text = "章节添加";
        }

        public EditSectionManage(string id)
        {
            InitializeComponent();
            this.id = id;
            this.Text = "章节修改";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string sqlString = "";
            if (id == "")
            {
                id = Guid.NewGuid().ToString();
                sqlString = "insert into ExamSection(id,section) values('" + id + "','" + txtName.Text.ToString() + "')";
            }
            else
            {
                sqlString = "update ExamSection set section='" + txtName.Text + "' where id = '" + id + "'";
            }

            try
            {
                new Tool().ExecNonSQLQuery(sqlString);
                MessageBoxEx.Show("保存成功");
                Close();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message.ToString() + "|可能原因：章节名称重复，章节名称应该是唯一的");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = txtName.Text.Length > 0;
        }

        private void EditSectionManage_Load(object sender, EventArgs e)
        {
            if (id != "")
            {
                string sqlString = "select * from ExamSection where id='" + id + "'";
                DataTable dt = new Tool().ExecuteSqlQuery(sqlString);
                txtName.Text = dt.Rows[0]["section"].ToString();
            }
        }
    }
}
