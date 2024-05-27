using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Text;
using System.Collections;

namespace TiemAnVat
{
    public partial class Default : System.Web.UI.Page
    {
        public static DataTable tbGioHang = new DataTable();
        int IdSP;
        SqlConnection myCon;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["GioHang"] != null)
                {
                    tbGioHang = Session["GioHang"] as DataTable;
                }
                else
                {
                    tbGioHang.Rows.Clear();
                    tbGioHang.Columns.Clear();
                    tbGioHang.Columns.Add("IdSP", typeof(int));
                    tbGioHang.Columns.Add("TenSP", typeof(string));
                    tbGioHang.Columns.Add("Gia", typeof(decimal));
                    tbGioHang.Columns.Add("SoLuong", typeof(int));
                    tbGioHang.Columns.Add("TongTien", typeof(decimal), "SoLuong * Gia");
                }
                lbGiohang.Text = "Giỏ hàng (" + tbGioHang.Rows.Count + ")";
                DoGridView();
            }
        }
        private void DoGridView()
        {
            try
            {
                myCon = DBClass.OpenConn();
                int nhomID = 0;

                // Kiểm tra nếu có mục được chọn trong DropDownList
                if (ListNhom.SelectedIndex > 0)
                {
                    // Lấy giá trị IDNhom từ mục được chọn trong DropDownList
                    nhomID = Convert.ToInt32(ListNhom.SelectedItem.Value);

                    // Sử dụng câu truy vấn để lấy sản phẩm có IDNhom tương ứng
                    using (SqlCommand myCom = new SqlCommand("SELECT dbo.SanPham.IdSP, dbo.NhomSP.TenNhom, dbo.SanPham.MaSP, dbo.SanPham.TenSP, dbo.SanPham.DVT, dbo.SanPham.GiaBan, dbo.SanPham.IMG FROM dbo.SanPham INNER JOIN dbo.NhomSP ON dbo.SanPham.IdNhom = dbo.NhomSP.IdNhom WHERE dbo.SanPham.IDNhom = @NhomID", myCon))
                    {
                        myCom.Parameters.Add("@NhomID", SqlDbType.Int).Value = nhomID;
                        myCom.CommandType = CommandType.Text;

                        SqlDataReader myDr = myCom.ExecuteReader();

                        listSanphams.DataSource = myDr;
                        listSanphams.DataBind();

                        myDr.Close();
                    }
                }
                else
                {
                    using (SqlCommand myCom = new SqlCommand("SELECT dbo.SanPham.IdSP, dbo.NhomSP.TenNhom, dbo.SanPham.MaSP, dbo.SanPham.TenSP, dbo.SanPham.DVT, dbo.SanPham.GiaBan, dbo.SanPham.IMG FROM dbo.SanPham INNER JOIN dbo.NhomSP ON dbo.SanPham.IdNhom = dbo.NhomSP.IdNhom", myCon))
                    {
                        SqlDataReader myDr = myCom.ExecuteReader();

                        listSanphams.DataSource = myDr;
                        listSanphams.DataBind();

                        myDr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "" + ex.Message;
            }
            finally
            {
                myCon.Close();
            }
        }
        private void GetSanpham(int Comp_ID)
        {
            try
            {
                myCon = DBClass.OpenConn();
                using (SqlCommand myCmd = new SqlCommand("SELECT IdSP, TenSP, MaSP, DVT, GiaBan, IMG, dbo.SanPham.IDNhom FROM dbo.SanPham INNER JOIN NhomSP ON dbo.SanPham.IDNhom = dbo.NhomSP.IDNhom WHERE IdSP = @ID", myCon))
                {
                    myCmd.Connection = myCon;
                    myCmd.CommandType = CommandType.Text;
                    myCmd.Parameters.Add("@ID", SqlDbType.Int).Value = Comp_ID;
                    SqlDataReader myDr = myCmd.ExecuteReader();

                    if (myDr.HasRows)
                    {
                        while (myDr.Read())
                        {
                            txtSanphamName.Text = myDr.GetValue(1).ToString();
                            txtSanphamMa.Text = myDr.GetValue(2).ToString();
                            txtSanphamDVT.Text = myDr.GetValue(3).ToString();
                            txtDongia.Text = myDr.GetValue(4).ToString();
                            lblSPID.Text = Comp_ID.ToString();
                            Image1.ImageUrl = "~/Images/" + myDr.GetValue(5).ToString();
                        }
                    }
                }
            }
            catch (Exception ex) { lblMessage.Text = "" + ex.Message; }
            finally { myCon.Close(); }
        }
        protected void lbGiohang_Click(object sender, EventArgs e)
        {
            GridView1.DataSource = tbGioHang;
            GridView1.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { openGiohang(); });", true);
        }

        protected void ShowNhom(object sender, EventArgs e)
        {
            DoGridView();
        }

        protected void listSanphams_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "XemSanpham")
            {
                IdSP = Convert.ToInt32(e.CommandArgument);


                txtSanphamName.Text = "";
                txtSanphamMa.Text = "";
                txtSanphamDVT.Text = "";
                txtDongia.Text = "";
                txtSoLuong.Text = "1";

                GetSanpham(IdSP);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openSPDetail();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { openSPDetail(); });", true);
            }
        }

        protected void btnChonSanpham_Click(object sender, EventArgs e)
        {
            int idSP = int.Parse(lblSPID.Text);
            string strTenSP = txtSanphamName.Text;
            decimal Gia = decimal.Parse(txtDongia.Text);
            int SoLuong = 1;
            if (!string.IsNullOrEmpty(txtSoLuong.Text) && int.TryParse(txtSoLuong.Text, out int parsedQuantity))
            {
                if (parsedQuantity > 0)
                {
                    SoLuong = parsedQuantity;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "MessageBox", "alert('Vui lòng nhập số lượng sản phẩm muốn đặt.');", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "MessageBox", "alert('Vui lòng nhập số lượng sản phẩm muốn đặt.');", true);
                return;
            }

            foreach (DataRow row in tbGioHang.Rows)
            {
                if ((int)row["IdSP"] == idSP)
                {
                    row["SoLuong"] = (int)row["SoLuong"] + SoLuong;
                    goto GioHang;
                }
            }
            tbGioHang.Rows.Add(idSP, strTenSP, Gia, SoLuong);
        GioHang:
            lbGiohang.Text = "Giỏ hàng (" + tbGioHang.Rows.Count + ")";
            Session["GioHang"] = tbGioHang;
        }


        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //IdSP = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
            tbGioHang.Rows[e.RowIndex].Delete();
            lbGiohang.Text = "Giỏ hàng (" + tbGioHang.Rows.Count + ")";

            GridView1.DataSource = tbGioHang;
            GridView1.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$(function() { openGiohang(); });", true);
        }

        protected void btnDathang_Click(object sender, EventArgs e)
        {
            string user = (string)Session["TenDN"];
            if (string.IsNullOrEmpty(user) == true)
            {
                Response.Redirect("Login.aspx");
            }
            if (tbGioHang.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "alert('Giỏ hàng trống. Không thể đặt hàng.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#myModal').modal('show');", true);
            }
        }
        decimal tongTienCanThanhToan = 0;
        protected void btnDatHangModal_Click(object sender, EventArgs e)
        {
            myCon = DBClass.OpenConn();
            string tenKhachHang = txtTenKhachHang.Text;
            string soDienThoai = txtSoDienThoai.Text;
            string diaChi = txtDiaChi.Text;
            string MaGG = txtVC.Text;
            string user = (string)Session["TenDN"];
            string sqlGetUserId = "SELECT Id FROM TaiKhoan WHERE TenDN = @TenDN";
            int userId = 0; // Khởi tạo biến userId ở đây để sử dụng sau
            using (SqlCommand cmdGetUserId = new SqlCommand(sqlGetUserId, myCon))
            {
                cmdGetUserId.Parameters.AddWithValue("@TenDN", user);
                object userIdResult = cmdGetUserId.ExecuteScalar();
                if (userIdResult != null)
                {
                    userId = Convert.ToInt32(userIdResult);
                }
                else
                {
                }
            }

            if (string.IsNullOrEmpty(tenKhachHang) || string.IsNullOrEmpty(soDienThoai) || string.IsNullOrEmpty(diaChi))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "alert('Vui lòng nhập đầy đủ thông tin.');", true);
            }
            else
            {
                decimal phanTramGiamGia = 0;
                bool validVoucher = false;

                string sqlGG = "SELECT PhanTramGiam FROM MaGiamGia WHERE MaVoucher = @MaVoucher";
                using (SqlCommand cmd = new SqlCommand(sqlGG, myCon))
                {
                    cmd.Parameters.AddWithValue("@MaVoucher", MaGG);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        phanTramGiamGia = Convert.ToDecimal(result);
                        validVoucher = true;
                    }
                    myCon.Close();
                }

                if (!validVoucher && !string.IsNullOrEmpty(MaGG))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "alert('Mã giảm giá không đúng.');", true);
                }
                else
                {
                    StringBuilder hoaDon = new StringBuilder();
                    hoaDon.Append("---------- Thông tin đặt hàng ----------<br />");
                    hoaDon.Append("Tài khoản đặt hàng: " + user + "<br />");
                    hoaDon.Append("Họ và tên: " + tenKhachHang + "<br />");
                    hoaDon.Append("Số Điện Thoại: " + soDienThoai + "<br />");
                    hoaDon.Append("Địa Chỉ: " + diaChi + "<br />");
                    hoaDon.Append("Mã giảm giá đã áp dụng: " + MaGG + "<br />");
                    hoaDon.Append("----------------------------------------------<br />");
                    List<string> danhSachSanPham = new List<string>();
                    foreach (DataRow row in tbGioHang.Rows)
                    {
                        string tenSanPham = row["TenSP"].ToString();
                        decimal gia = Convert.ToDecimal(row["Gia"]);
                        int soLuong = Convert.ToInt32(row["SoLuong"]);
                        decimal tongTien = Convert.ToDecimal(row["TongTien"]);
                        string jsonSanPham = tenSanPham + "\n" +
                              "x" + soLuong + "\n" +
                              tongTien.ToString("#,##0") + " VNĐ\n";
                        danhSachSanPham.Add(jsonSanPham);
                        hoaDon.Append("Tên Sản Phẩm: " + tenSanPham + "<br />");
                        hoaDon.Append("Giá: " + gia.ToString("#,##0") + " VNĐ<br />");
                        hoaDon.Append("Số Lượng: " + soLuong + "<br />");
                        hoaDon.Append("Tổng Tiền: " + tongTien.ToString("#,##0") + " VNĐ<br />");
                        hoaDon.Append("<br />");
                        hoaDon.Append("----------------------------------------------<br />");
                    }
                    string chuoiSanPham = string.Join("\n",danhSachSanPham);
                    foreach (DataRow row in tbGioHang.Rows)
                    {
                        decimal tongTien = Convert.ToDecimal(row["TongTien"]);
                        tongTienCanThanhToan += tongTien;
                    }
                    decimal tongTienCanThanhToanSauGiamGia = tongTienCanThanhToan * (1 - phanTramGiamGia / 100);
                    hoaDon.Append("Tổng tiền tất cả sản phẩm : " + tongTienCanThanhToan.ToString("#,##0") + " VNĐ<br />");
                    hoaDon.Append("Số tiền giảm giá : " + ((tongTienCanThanhToan * phanTramGiamGia / 100) * -1).ToString("#,##0") + " VNĐ<br />");
                    hoaDon.Append("Tổng tiền cần thanh toán : " + tongTienCanThanhToanSauGiamGia.ToString("#,##0") + " VNĐ<br />");

                    lblHoaDon.Text = hoaDon.ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#hoaDonModal').modal('show');", true);
                    tbGioHang.Clear();
                    lbGiohang.Text = "Giỏ hàng (" + tbGioHang.Rows.Count + ")";

                    string sqlInsertDonHang = "INSERT INTO DonHang (IdKH, TenKhachHang, SoDienThoai, DiaChi, SanPham, MaGiamGia, TongTien, TongTienSauGiamGia) " +
                           "VALUES (@Id, @TenKhachHang, @SoDienThoai, @DiaChi, @SanPham, @MaGiamGia, @TongTien, @TongTienSauGiamGia)";
                    using (SqlCommand cmdInsertDonHang = new SqlCommand(sqlInsertDonHang, myCon))
                    {
                        cmdInsertDonHang.Parameters.AddWithValue("@Id", userId);
                        cmdInsertDonHang.Parameters.AddWithValue("@TenKhachHang", tenKhachHang);
                        cmdInsertDonHang.Parameters.AddWithValue("@SoDienThoai", soDienThoai);
                        cmdInsertDonHang.Parameters.AddWithValue("@DiaChi", diaChi);
                        cmdInsertDonHang.Parameters.AddWithValue("@SanPham", chuoiSanPham);
                        cmdInsertDonHang.Parameters.AddWithValue("@MaGiamGia", MaGG);
                        cmdInsertDonHang.Parameters.AddWithValue("@TongTien", tongTienCanThanhToan);
                        cmdInsertDonHang.Parameters.AddWithValue("@TongTienSauGiamGia", tongTienCanThanhToanSauGiamGia);

                        // Thực thi câu lệnh SQL
                        myCon.Open();
                        cmdInsertDonHang.ExecuteNonQuery();
                        myCon.Close();
                    }
                }
            }
        }
    }
}