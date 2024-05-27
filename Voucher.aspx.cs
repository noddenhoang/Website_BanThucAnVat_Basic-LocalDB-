using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TiemAnVat
{
    public partial class Voucher : System.Web.UI.Page
    {
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
    }
}