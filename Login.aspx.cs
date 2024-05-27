using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TiemAnVat
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBox1.Text) || string.IsNullOrWhiteSpace(TextBox2.Text))
            {
                Label1.Text = "Vui lòng không để trống thông tin!";
                return;
            }

            SqlConnection myCon = new SqlConnection(ConfigurationManager.ConnectionStrings["TiemAnVatConnection"].ConnectionString);
            myCon.Open();
            string qry = "SELECT * FROM TaiKhoan WHERE TenDN='" + TextBox1.Text + "' AND MatKhau='" + TextBox2.Text + "'";
            SqlCommand cmd = new SqlCommand(qry, myCon);
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                Session["TenDN"] = TextBox1.Text;
                Response.Redirect("Default.aspx");
            }
            else
            {
                Label1.Text = "Tên đăng nhập và mật khẩu không chính xác!";
            }
        }


        protected void Button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBox1.Text) || string.IsNullOrWhiteSpace(TextBox2.Text))
            {
                Label1.Text = "Vui lòng không để trống thông tin!";
                return;
            }

            using (SqlConnection myCon = new SqlConnection(ConfigurationManager.ConnectionStrings["TiemAnVatConnection"].ConnectionString))
            {
                myCon.Open();

                string checkQuery = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDN=@username";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, myCon))
                {
                    checkCmd.Parameters.AddWithValue("@username", TextBox1.Text);

                    int existingUserCount = (int)checkCmd.ExecuteScalar();

                    if (existingUserCount > 0)
                    {
                        Label1.Text = "Tên đăng nhập đã được sử dụng";
                    }
                    else
                    {
                        string insertQuery = "INSERT INTO TaiKhoan (TenDN, MatKhau) VALUES (@username, @password)";
                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, myCon))
                        {
                            insertCmd.Parameters.AddWithValue("@username", TextBox1.Text);
                            insertCmd.Parameters.AddWithValue("@password", TextBox2.Text);

                            int rowsAffected = insertCmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                Label1.Text = "Tài khoản đã được tạo thành công";
                            }
                            else
                            {
                                Label1.Text = "Đã xảy ra lỗi khi tạo tài khoản";
                            }
                        }
                    }
                }
            }
        }
    }
}