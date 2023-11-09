using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace WebApplication2
{
    public partial class WebForm8 : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["connection_string"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

            string paramValue = Request.QueryString["link"];
            
            DateTime dateTime = DateTime.Now;

            SqlConnection con = new SqlConnection(strcon);
            string query = $"SELECT linkactivare FROM users WHERE linkactivare = @paramValue AND time > @dateTime ";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@paramValue", paramValue);
            cmd.Parameters.AddWithValue("@dateTime", dateTime);









        }
    }
}