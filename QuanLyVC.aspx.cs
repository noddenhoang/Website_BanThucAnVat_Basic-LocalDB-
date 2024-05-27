using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TiemAnVat
{
    public partial class QuanLyVC : System.Web.UI.Page
    {
        int IdVC;
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
                using (SqlCommand myCom = new SqlCommand("SELECT * FROM dbo.MaGiamGia", myCon))
                {
                    SqlDataReader myDr = myCom.ExecuteReader();
                    gvMaGG.DataSource = myDr;
                    gvMaGG.DataBind();
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

        protected void lbNewVC_Click(object sender, EventArgs e)
        {
            try
            {
                txtMaVC.Text = "";
                txtPhanTramGiam.Text = "";

                lblVCNew.Visible = true;
                lblVCUpd.Visible = false;
                btnAddVC.Visible = true;
                btnUpdVC.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openVCDetail();", true);
            }
            catch (Exception) { throw; }
        }

        protected void gvMaGG_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdVC")
            {
                IdVC = Convert.ToInt32(e.CommandArgument);


                txtMaVC.Text = "";
                txtPhanTramGiam.Text = "";

                lblVCNew.Visible = false;
                lblVCUpd.Visible = true;
                btnAddVC.Visible = false;
                btnUpdVC.Visible = true;
                GetVC(IdVC);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { openVCDetail(); });", true);
            }
        }

        protected void gvMaGG_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            IdVC = Convert.ToInt32(gvMaGG.DataKeys[e.RowIndex].Value.ToString());

            try
            {
                //myCon.Open();
                myCon = DBClass.OpenConn();
                using (SqlCommand cmd = new SqlCommand("delete from dbo.MaGiamGia where IdVC = @ID", myCon))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = IdVC;
                    cmd.ExecuteScalar();
                }
            }
            catch (Exception ex) { lblMessage.Text = "" + ex.Message; }
            finally { myCon.Close(); }
            DoGridView();
        }
        protected void btnAddVC_Click(object sender, EventArgs e)
        {
            try
            {
                myCon = DBClass.OpenConn();
                using (SqlCommand myCom = new SqlCommand("INSERT INTO dbo.MaGiamGia (MaVoucher, PhanTramGiam) VALUES (@MaVC, @PhanTram)", myCon))
                {
                    myCom.CommandType = CommandType.Text;
                    myCom.Parameters.Add("@MaVC", SqlDbType.NVarChar).Value = txtMaVC.Text;
                    myCom.Parameters.Add("@PhanTram", SqlDbType.Float).Value = txtPhanTramGiam.Text;

                    myCom.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { lblMessage.Text = "" + ex.Message; }
            finally { myCon.Close(); }
            DoGridView();
        }

        private void GetVC(int Comp_ID)
        {
            try
            {
                myCon = DBClass.OpenConn();
                using (SqlCommand myCmd = new SqlCommand("SELECT IdVC, MaVoucher, PhanTramGiam FROM dbo.MaGiamGia WHERE dbo.MaGiamGia.IdVC = @ID", myCon))
                {
                    myCmd.Connection = myCon;
                    myCmd.CommandType = CommandType.Text;
                    myCmd.Parameters.Add("@ID", SqlDbType.Int).Value = Comp_ID;
                    SqlDataReader myDr = myCmd.ExecuteReader();

                    if (myDr.HasRows)
                    {
                        while (myDr.Read())
                        {
                            txtMaVC.Text = myDr.GetValue(1).ToString();
                            txtPhanTramGiam.Text = myDr.GetValue(2).ToString();
                            lblVCID.Text = Comp_ID.ToString();
                        }
                    }
                }
            }
            catch (Exception ex) { lblMessage.Text = "" + ex.Message; }
            finally { myCon.Close(); }
        }
        protected void btnUpdVC_Click(object sender, EventArgs e)
        {
            UpdVC();
            DoGridView();
        }
        private void UpdVC()
        {
            try
            {
                myCon = DBClass.OpenConn();
                using (SqlCommand cmd = new SqlCommand("UPDATE [dbo].[MaGiamGia] SET MaVoucher = ISNULL(@MaVC, MaVoucher), PhanTramGiam = ISNULL(@PhanTramGiam, PhanTramGiam) WHERE IdVC = @ID", myCon))
                {
                    cmd.Connection = myCon;
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = int.Parse(lblVCID.Text);
                    cmd.Parameters.Add("@MaVC", SqlDbType.NVarChar).Value = txtMaVC.Text;
                    cmd.Parameters.Add("@PhanTramGiam", SqlDbType.Float).Value = txtPhanTramGiam.Text;

                    int rows = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { lblMessage.Text = "" + ex.Message; }
            finally { myCon.Close(); }
        }
    }
}