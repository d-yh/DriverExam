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
    public partial class Login : CCSkinMain
    {
        public static string USERID;
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtName.Text.ToString();
            string password = txtpassword.Text.ToString();
            DataTable dt = new Tool().ExecuteSqlQuery("select * from ExamUser where name='"+username+"' and password='"+password+"'");
            if (dt != null && dt.Rows.Count == 1)
            {                
                new Main(dt.Rows[0]["name"].ToString()).Show();
                this.Visible = false;
                USERID = dt.Rows[0]["id"].ToString();
            }
            else if(dt.Rows.Count<1)
            {
                MessageBoxEx.Show("密码或用户名错误,请仔细检查您的用户名和密码，或联系管理员");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
