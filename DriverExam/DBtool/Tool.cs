using CCWin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DriverExam
{
    
    class Tool
    {
        private string connectionString;

        public Tool()
        {
            this.connectionString = ConfigurationSettings.AppSettings["SqlConString"].ToString();
        }


        /// <summary>
        /// 对数据库执行查询操作
        /// </summary>
        /// <param name="queryString">查询SQL语句</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteSqlQuery(string queryString)
        {
            DataSet da = new DataSet();
            DataTable dt = new DataTable();

            SqlConnection sqlConnection = null;
            SqlDataAdapter sqlDataAdapter = null;
            try
            {
                //MessageBox.Show("Exect SQL query:" + queryString);
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                sqlDataAdapter = new SqlDataAdapter(queryString, sqlConnection);
                sqlDataAdapter.SelectCommand.CommandTimeout = 900;
                sqlDataAdapter.Fill(da);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
            finally
            {
                if (sqlConnection != null) { sqlConnection.Close(); }
                if (sqlDataAdapter != null) { sqlDataAdapter.Dispose(); }
            }
            if (da.Tables.Count > 0)
                dt = da.Tables[0];
            return dt;
        }

        public int ExecNonSQLQuery(string Query)
        {
            string connectionString = ConfigurationSettings.AppSettings["SqlConString"].ToString();

            return ExecNonSQLQuery(connectionString, Query);
        }

        public int ExecNonSQLQuery(string connectionStr, string queryString)
        {
            int count = -1;
            SqlCommand cmd = null;

            try
            {
                cmd = new SqlCommand(queryString);

                //MessageBox.Show("MyTest:" + queryString);
                cmd.Connection = new SqlConnection(connectionStr);
                cmd.Connection.Open();
                cmd.CommandTimeout = 900;

                count = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error " + ex.Message);
            }
            finally
            {
                if (cmd != null && cmd.Connection != null) { cmd.Connection.Close(); }
                if (cmd != null) { cmd.Dispose(); }
            }
            return count;
        }

        public Bitmap DBgetimage(string sqlString)
        {
            //string sqlString = "select "+filedName+" from "+tableName+" where "+tableIDName+" = '"+tableIdValue+"'";
            byte[] imagebytes = null;
            Bitmap bmpt = null;
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            try
            {
                SqlCommand com = new SqlCommand(sqlString, con);
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.GetValue(0) == DBNull.Value)
                    {
                        return null;
                    }
                    imagebytes = (byte[])dr.GetValue(0);
                }
                dr.Close();
                com.Clone();

                MemoryStream ms = new MemoryStream(imagebytes);
                bmpt = new Bitmap(ms);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return bmpt;
        }

        public void SaveImage(string sqlString, Bitmap image)
        {
            try
            {
                //打开数据库
                string connectionString = ConfigurationSettings.AppSettings["SqlConString"].ToString();
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand com = new SqlCommand(sqlString, con);
                com.Parameters.Add("ImageList1", SqlDbType.Image);
                System.IO.MemoryStream Ms = new MemoryStream();
                image.Save(Ms, System.Drawing.Imaging.ImageFormat.Bmp);
                byte[] imagebytes = new byte[Ms.Length];
                Ms.Position = 0;
                Ms.Read(imagebytes, 0, Convert.ToInt32(Ms.Length));
                Ms.Close();
                com.Parameters["ImageList1"].Value = imagebytes;
                com.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("图片出错未保存，可能您未拍摄或选择图片,其他数据将正常保存。原因：" + e.Message.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void importExcel()
        {
            OleDbConnection conn = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel files(*.xls,*.xlsx)|*.xls;*.xlsx";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Loading load = new Loading();
                load.Show();
                DataSet ds = new DataSet();
                SqlConnection cnn = new SqlConnection(connectionString);
                SqlCommand cm = new SqlCommand();
                cm.Connection = cnn;
                cnn.Open();
                SqlTransaction trans = cnn.BeginTransaction();
                cm.Transaction = trans;
                
                try
                {
                    //获取全部数据  
                    string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + openFileDialog.FileName + ";" + "Extended Properties=Excel 8.0;";
                    conn = new OleDbConnection(strConn);
                    conn.Open();
                    //OleDbDataAdapter da = new OleDbDataAdapter("SELECT *  FROM [" + Path.GetFileNameWithoutExtension(openFileDialog.FileName) + "$]", strConn);
                    OleDbDataAdapter da = new OleDbDataAdapter("SELECT *  FROM [ExamSubject$]", strConn); 
                    DataTable dt = new DataTable();
                    da.Fill(ds);
                    dt = ds.Tables[0];

                    dt = initData(dt);
                    cm.Parameters.Add("@topic", SqlDbType.NVarChar);
                    cm.Parameters.Add("@type", SqlDbType.NVarChar);
                    cm.Parameters.Add("@question", SqlDbType.NVarChar);
                    cm.Parameters.Add("@picture_name", SqlDbType.NVarChar);
                    cm.Parameters.Add("@option_a", SqlDbType.NVarChar);
                    cm.Parameters.Add("@option_b", SqlDbType.NVarChar);
                    cm.Parameters.Add("@option_c", SqlDbType.NVarChar);
                    cm.Parameters.Add("@option_d", SqlDbType.NVarChar);
                    cm.Parameters.Add("@answer", SqlDbType.NVarChar);
                    cm.Parameters.Add("@section", SqlDbType.NVarChar);
                    cm.Parameters.Add("@problem", SqlDbType.NVarChar);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cm.CommandText = "insert into ExamSubject(topic,type,question,picture_name,option_a,option_b,option_c,option_d,answer,section,problem) values (@topic,@type,@question,@picture_name,@option_a,@option_b,@option_c,@option_d,@answer,@section,@problem)";
                       
                        cm.Parameters["@topic"].Value = dt.Rows[i]["topic"];

                        
                        cm.Parameters["@type"].Value = dt.Rows[i]["type"];

                       
                        cm.Parameters["@question"].Value = dt.Rows[i]["question"];

                        //cm.Parameters.Add("@picture", SqlDbType.Image);
                        //cm.Parameters["@picture"].Value = null;

                       
                        cm.Parameters["@picture_name"].Value = dt.Rows[i]["picture_name"];

                       
                        cm.Parameters["@option_a"].Value = dt.Rows[i]["option_a"];

                        
                        cm.Parameters["@option_b"].Value = dt.Rows[i]["option_b"];

                        
                        cm.Parameters["@option_c"].Value = dt.Rows[i]["option_c"];

                       
                        cm.Parameters["@option_d"].Value = dt.Rows[i]["option_d"];

                       
                        cm.Parameters["@answer"].Value = dt.Rows[i]["answer"];
                  
                        cm.Parameters["@section"].Value = dt.Rows[i]["section"];

                        cm.Parameters["@problem"].Value = dt.Rows[i]["problem"];

                        cm.ExecuteNonQuery();
                    }
                    
                    trans.Commit();
                    ExecNonSQLQuery("update ExamSubject set answer = '对' where answer = 'Y'");
                    ExecNonSQLQuery("update ExamSubject set answer = '错' where answer = 'N'");                    
                    MessageBoxEx.Show("已成功导入");
                }
                catch(Exception e)
                {
                    trans.Rollback(); 
                    MessageBox.Show(e.Message.ToString());
                }
                finally
                {
                    load.Close();
                    conn.Close();
                    cnn.Close();
                    trans.Dispose();
                    cnn.Close();
                }
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        public DataTable initData(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //检查是否属于新章节
                string sqlString = "select id from ExamSection where section = '" + dt.Rows[i]["section"] + "'";
                string id = "";
                DataTable dtable = ExecuteSqlQuery(sqlString);
                if (dtable.Rows.Count == 0)//说明没有
                {
                    id = Guid.NewGuid().ToString();
                    sqlString = "insert into ExamSection(id,section)values('" + id + "','" + dt.Rows[i]["section"].ToString() + "')";
                    ExecuteSqlQuery(sqlString);
                    dt.Rows[i]["section"] = id;
                }
                else if (dtable.Rows.Count == 1)//说明有
                {
                    dt.Rows[i]["section"] = dtable.Rows[0]["id"].ToString();
                }
            }
           
            return dt;
           
        }
    }
}
