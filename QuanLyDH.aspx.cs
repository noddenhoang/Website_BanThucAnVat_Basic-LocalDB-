using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Printing;
using System.Xml.Linq;
using System.Text;

namespace TiemAnVat
{
    public partial class QuanLyDH : System.Web.UI.Page
    {
        int IdDH;
        SqlConnection myCon;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DoGridView();
            }
        }
        protected void DoGridView() // Assuming voucherID is passed as a parameter
        {
            try
            {
                myCon = DBClass.OpenConn();
                using (SqlCommand myCom = new SqlCommand("SELECT DonHang.*, TenDN FROM DonHang INNER JOIN TAIKHOAN ON DonHang.IdKH = TAIKHOAN.Id", myCon))
                {
                    SqlDataReader myDr = myCom.ExecuteReader();
                    gvDH.DataSource = myDr;
                    gvDH.DataBind();
                    myDr.Close();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
            finally
            {
                myCon.Close();
            }
        }
        protected void gvDH_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            IdDH = Convert.ToInt32(gvDH.DataKeys[e.RowIndex].Value.ToString());

            try
            {
                myCon = DBClass.OpenConn();
                using (SqlCommand cmd = new SqlCommand("delete from dbo.DonHang where Id = @ID", myCon))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = IdDH;
                    cmd.ExecuteScalar();
                }
            }
            catch (Exception ex) { lblMessage.Text = "" + ex.Message; }
            finally { myCon.Close(); }
            DoGridView();
        }
        protected void listDH_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "XemDH")
            {
                IdDH = Convert.ToInt32(e.CommandArgument);
                GetHD(IdDH);
            }
        }
        private void GetHD(int Comp_ID)
        {
            try
            {
                myCon = DBClass.OpenConn();
                using (SqlCommand myCmd = new SqlCommand("SELECT DonHang.*, TenDN FROM DonHang INNER JOIN TAIKHOAN ON DonHang.IdKH = TAIKHOAN.Id WHERE DonHang.Id = @ID", myCon))
                {
                    myCmd.CommandType = CommandType.Text;
                    myCmd.Parameters.Add("@ID", SqlDbType.Int).Value = Comp_ID;
                    SqlDataReader myDr = myCmd.ExecuteReader();

                    if (myDr.HasRows)
                    {
                        if (myDr.Read())
                        {
                            // Retrieve data from the selected row
                            string maDonHang = myDr["Id"].ToString();
                            string tenKhachHang = myDr["TenKhachHang"].ToString();
                            string soDienThoai = myDr["SoDienThoai"].ToString();
                            string diaChi = myDr["DiaChi"].ToString();
                            string sanPham = myDr["SanPham"].ToString();
                            string maGiamGia = myDr["MaGiamGia"].ToString();
                            string tongTien = myDr["TongTien"].ToString();
                            string tongTienSauGiamGia = myDr["TongTienSauGiamGia"].ToString();

                            // Build the order details string
                            StringBuilder hoaDon = new StringBuilder();
                            hoaDon.Append("---------- Thông tin đặt hàng ----------");
                            hoaDon.Append("<br />Mã đơn hàng: " + maDonHang);
                            hoaDon.Append("<br />Họ và tên: " + tenKhachHang);
                            hoaDon.Append("<br />Số Điện Thoại: " + soDienThoai);
                            hoaDon.Append("<br />Địa Chỉ: " + diaChi);
                            hoaDon.Append("<br />Sản phẩm đặt mua: " + sanPham);
                            hoaDon.Append("<br />Mã giảm giá đã áp dụng: " + maGiamGia);
                            hoaDon.Append("<br />Tổng tiền: " + tongTien);
                            hoaDon.Append("<br />Tổng tiền cần thanh toán: " + tongTienSauGiamGia);
                            hoaDon.Append("<br />----------------------------------------------");
                            lblHoaDon.Text = hoaDon.ToString();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#hoaDonModal').modal('show');", true);
                        }
                    }
                    else
                    {
                        // Handle case where no rows found
                        lblMessage.Text = "No order found with the specified ID.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
            finally
            {
                myCon.Close();
            }
        }

    }
}