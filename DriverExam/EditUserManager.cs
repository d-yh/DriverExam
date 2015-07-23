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
    public partial class EditUserManager : CCSkinMain
    {
        private string id="";
        public EditUserManager()
        {
            InitializeComponent();
            this.Text = "添加用户";
        }

        public EditUserManager(string id)
        {
            InitializeComponent();
            this.id = id;
            this.Text = "修改用户";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (checkFiled())
            {
                string sqlString = "";
                if (id == "")
                {
                    id = Guid.NewGuid().ToString();
                    sqlString = "insert into ExamUser(id,name,real_name,password) values('"+id+"','"+txtName.Text.ToString()+"','"+txtRealName.Text+"','"+txtPassword.Text+"')";
                }
                else
                {
                    sqlString = "update ExamUser set name='" + txtName.Text + "',password='"+txtPassword.Text+"',real_name='"+txtRealName.Text+"' where id = '"+id+"'";
                }

                try
                {
                    new Tool().ExecNonSQLQuery(sqlString);
                    MessageBoxEx.Show("保存成功");
                    Close();
                }
                catch(Exception ex)
                {
                    MessageBoxEx.Show(ex.Message.ToString()+"|可能原因：用户名重复，用户名应该是唯一的");
                }
            }
        }

        private bool checkFiled()
        {
            if (txtName.Text.ToString() == "")
            {
                MessageBoxEx.Show("用户名不能为空");
                return false;
            }

            if (txtPassword.Text.ToString() == "")
            {
                MessageBoxEx.Show("密码不能为空");
                return false;
            }

            if (txtName.Text.ToString().Length < 4 || txtName.Text.ToString().Length > 10)
            {
                MessageBoxEx.Show("用户名不符合规定长度");
                return false;
            }

            if (txtPassword.Text.ToString().Length < 6 || txtPassword.Text.ToString().Length > 10)
            {
                MessageBoxEx.Show("密码不符合规定长度");
                return false;
            }

            if (txtRealName.Text.ToString() == "")
            {
                MessageBoxEx.Show("用户姓名不能为空");
                return false;
            }

            return true;
        }

        private void EditUserManager_Load(object sender, EventArgs e)
        {
            if(id!="")
            {
                string sqlString= "select * from ExamUser where id='"+id+"'";
                DataTable dt = new Tool().ExecuteSqlQuery(sqlString);
                txtName.Text = dt.Rows[0]["name"].ToString();
                if (txtName.Text == "admin")
                {
                    txtName.Enabled = false;
                }
                txtRealName.Text = dt.Rows[0]["real_name"].ToString();
                txtPassword.Text = dt.Rows[0]["password"].ToString();
            }
            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
