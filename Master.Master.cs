using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TiemAnVat
{
    public partial class Master : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string user = (string)Session["TenDN"];
            if (user == "admin")
            {
                QuanlyLink.Visible = true;
                QuanlyVCLink.Visible = true;
                QuanlyDHLink.Visible = true;
            }    
            else
            {
                QuanlyLink.Visible = false;
                QuanlyVCLink.Visible = false;
                QuanlyDHLink.Visible = false;
            }
            if (string.IsNullOrEmpty(user) == false)
            {
                Login_BTN.Visible = false;
                Logout_BTN.Visible = true;
                LSDHLink.Visible = true;
                CurrentUser.Text = user;
            }
            else
            {
                Login_BTN.Visible = true;
                Logout_BTN.Visible = false;
                LSDHLink.Visible = false;
                CurrentUser.Text = "";
            }
        }

        protected void Login_BTN_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
        protected void Logout_BTN_Click(object sender,EventArgs e)
        {
            Session["TenDN"] = "";
            Response.Redirect("default.aspx");
        }
    }
}