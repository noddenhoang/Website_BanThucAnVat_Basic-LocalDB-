using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TiemAnVat
{
    public class DBClass
    {
        public static SqlConnection OpenConn()
        {
            SqlConnection myCon = new SqlConnection(ConfigurationManager.ConnectionStrings["TiemAnVatConnection"].ConnectionString);
            myCon.Open();
            return myCon;
        }
    }
}