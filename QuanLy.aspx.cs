using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TiemAnVat
{
    public partial class QuanLy : System.Web.UI.Page
    {
        int IdSP;
        SqlConnection myCon;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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

                        gvSanphams.DataSource = myDr;
                        gvSanphams.DataBind();

                        myDr.Close();
                    }
                }
                else
                {
                    // Nếu không có mục nào được chọn trong DropDownList, hiển thị toàn bộ sản phẩm
                    using (SqlCommand myCom = new SqlCommand("SELECT dbo.SanPham.IdSP, dbo.NhomSP.TenNhom, dbo.SanPham.MaSP, dbo.SanPham.TenSP, dbo.SanPham.DVT, dbo.SanPham.GiaBan, dbo.SanPham.IMG FROM dbo.SanPham INNER JOIN dbo.NhomSP ON dbo.SanPham.IdNhom = dbo.NhomSP.IdNhom", myCon))
                    {
                        SqlDataReader myDr = myCom.ExecuteReader();

                        gvSanphams.DataSource = myDr;
                        gvSanphams.DataBind();

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
        protected void gvSanphams_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdSanpham")
            {
                IdSP = Convert.ToInt32(e.CommandArgument);


                txtSanphamName.Text = "";
                txtSanphamMa.Text = "";
                txtSanphamDVT.Text = "";
                txtDongia.Text = "";

                lblSanphamNew.Visible = false;
                lblSanphamUpd.Visible = true;
                btnAddSanpham.Visible = false;
                btnUpdSanpham.Visible = true;
                GetNhomForDLL();
                GetSanpham(IdSP);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openSPDetail();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { openSPDetail(); });", true);
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
                            ddlNhom.SelectedValue = myDr.GetValue(6).ToString();
                            lblSPID.Text = Comp_ID.ToString();
                        }
                    }
                }
            }
            catch (Exception ex) { lblMessage.Text = "Error in GetSanpham: " + ex.Message; }
            finally { myCon.Close(); }
        }
        protected void lbNewSanpham_Click(object sender, EventArgs e)
        {
            try
            {
                txtSanphamName.Text = "";
                txtSanphamMa.Text = "";
                txtSanphamDVT.Text = "";
                txtDongia.Text = "";

                lblSanphamNew.Visible = true;
                lblSanphamUpd.Visible = false;
                btnAddSanpham.Visible = true;
                btnUpdSanpham.Visible = false;

                GetNhomForDLL();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openSPDetail();", true);
            }
            catch (Exception) { throw; }
        }
        private void GetNhomForDLL()
        {
            try
            {
                myCon = DBClass.OpenConn();
                using (SqlCommand cmd = new SqlCommand("select IdNhom, TenNhom from dbo.NhomSP", myCon))
                {
                    SqlDataReader myDr = cmd.ExecuteReader();

                    ddlNhom.DataSource = myDr;
                    ddlNhom.DataTextField = "TenNhom";
                    ddlNhom.DataValueField = "IdNhom";
                    ddlNhom.DataBind();
                    ddlNhom.Items.Insert(0, new ListItem("-- Chọn nhóm --", "0"));

                    myDr.Close();
                }
            }
            catch (Exception ex) { lblMessage.Text = "" + ex.Message; }
            finally { myCon.Close(); }
        }
        protected void ShowNhom(object sender, EventArgs e)
        {
            DoGridView();
        }

        protected void gvSanphams_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            IdSP = Convert.ToInt32(gvSanphams.DataKeys[e.RowIndex].Value.ToString());

            try
            {
                //myCon.Open();
                myCon = DBClass.OpenConn();
                using (SqlCommand cmd = new SqlCommand("delete from dbo.SanPham where IdSP = @ID", myCon))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = IdSP;
                    cmd.ExecuteScalar();
                }
            }
            catch (Exception ex) { lblMessage.Text = "Error in gvSanphams_RowDeleting: " + ex.Message; }
            finally { myCon.Close(); }
            DoGridView();
        }

        protected void btnAddSanpham_Click(object sender, EventArgs e)
        {
            try
            {
                // Lưu hình ảnh vào thư mục "Images"
                string imageName = SaveImageToFolder(fileUploadHinhAnh);

                // Thêm thông tin sản phẩm vào cơ sở dữ liệu
                myCon = DBClass.OpenConn();
                using (SqlCommand myCom = new SqlCommand("INSERT INTO [dbo].[SanPham] ([TenSP], [MaSP], [DVT], [GiaBan], [IDNhom], [IMG]) VALUES (@TenSP, @MaSP, @Dvt, @Gia, @NhomID, @ImageName)", myCon))
                {
                    myCom.CommandType = CommandType.Text;
                    myCom.Parameters.Add("@TenSP", SqlDbType.NVarChar).Value = txtSanphamName.Text;
                    myCom.Parameters.Add("@MaSP", SqlDbType.NVarChar).Value = txtSanphamMa.Text;
                    myCom.Parameters.Add("@Dvt", SqlDbType.NVarChar).Value = txtSanphamDVT.Text;
                    myCom.Parameters.Add("@Gia", SqlDbType.Decimal).Value = decimal.Parse(txtDongia.Text);
                    myCom.Parameters.Add("@NhomID", SqlDbType.Int).Value = int.Parse(ddlNhom.SelectedValue);
                    myCom.Parameters.Add("@ImageName", SqlDbType.NVarChar).Value = imageName;

                    myCom.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { lblMessage.Text = "Error in btnAddSanpham_Click: " + ex.Message; }
            finally { myCon.Close(); }
            DoGridView();
        }

        protected void btnUpdSanpham_Click(object sender, EventArgs e)
        {
            UpdSanpham();
            DoGridView();
        }
        private void UpdSanpham()
        {
            try
            {
                string imageName = ""; // Biến để lưu tên ảnh mới hoặc cũ

                // Kiểm tra xem người dùng đã chọn ảnh mới hay không
                if (fileUploadHinhAnh.HasFile)
                {
                    // Nếu có chọn ảnh mới, lưu ảnh mới và cập nhật tên ảnh
                    imageName = SaveImageToFolder(fileUploadHinhAnh);
                }
                else
                {
                    // Nếu không chọn ảnh mới, lấy tên ảnh cũ từ cơ sở dữ liệu
                    myCon = DBClass.OpenConn();
                    using (SqlCommand cmd = new SqlCommand("SELECT IMG FROM [dbo].[SanPham] WHERE IdSP = @ID", myCon))
                    {
                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = int.Parse(lblSPID.Text);
                        object imgNameObj = cmd.ExecuteScalar();
                        if (imgNameObj != DBNull.Value)
                        {
                            imageName = imgNameObj.ToString();
                        }
                    }
                }

                // Tiếp tục cập nhật thông tin sản phẩm trong cơ sở dữ liệu
                myCon = DBClass.OpenConn();
                using (SqlCommand cmd = new SqlCommand("UPDATE [dbo].[SanPham] SET TenSP = ISNULL(@TenSP, TenSP), MaSP = ISNULL(@MaSP, MaSP), DVT = ISNULL(@Dvt, DVT), GiaBan = ISNULL(@Gia, GiaBan), IDNhom = ISNULL(@NhomID, IDNhom), IMG = ISNULL(@ImageName, IMG) WHERE IdSP = @ID", myCon))
                {
                    cmd.Connection = myCon;
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = int.Parse(lblSPID.Text);
                    cmd.Parameters.Add("@TenSP", SqlDbType.NVarChar).Value = txtSanphamName.Text;
                    cmd.Parameters.Add("@MaSP", SqlDbType.NVarChar).Value = txtSanphamMa.Text;
                    cmd.Parameters.Add("@Dvt", SqlDbType.NVarChar).Value = txtSanphamDVT.Text;
                    cmd.Parameters.Add("@Gia", SqlDbType.Decimal).Value = decimal.Parse(txtDongia.Text);
                    cmd.Parameters.Add("@NhomID", SqlDbType.Int).Value = ddlNhom.SelectedValue;

                    // Sử dụng tên ảnh mới nếu có, ngược lại sử dụng tên ảnh cũ từ cơ sở dữ liệu
                    cmd.Parameters.Add("@ImageName", SqlDbType.NVarChar).Value = imageName;

                    int rows = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { lblMessage.Text = "Error in UpdSanpham: " + ex.Message; }
            finally { myCon.Close(); }
        }

        private string SaveImageToFolder(FileUpload fileUploadControl)
        {
            string imageName = "";
            if (fileUploadControl.HasFile)
            {
                try
                {
                    // Tạo tên ngẫu nhiên cho tập tin hình ảnh
                    string fileName = Path.GetFileName(fileUploadControl.FileName);
                    imageName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);

                    // Lưu hình ảnh vào thư mục "Images"
                    fileUploadControl.SaveAs(Server.MapPath("~/Images/") + imageName);
                }
                catch (Exception ex)
                {
                    lblModalMessage.Text = "Lỗi khi tải lên hình ảnh: " + ex.Message;
                }
            }
            return imageName;
        }
    }
}