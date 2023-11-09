using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace WebApplication2
{
    public partial class WebForm6 : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["connection_string"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            string paramValue = Request.QueryString["link"];

            DateTime dateTime = DateTime.Now;

            SqlConnection con = new SqlConnection(strcon);
                string query = $"SELECT linkactivare FROM users WHERE linkactivare = @paramValue AND time > @dateTime";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@paramValue", paramValue);
                cmd.Parameters.AddWithValue("@dateTime", dateTime);

            con.Open();
                string matchingValue = (string)cmd.ExecuteScalar();
                con.Close();

              
            

            
            DataTable dt = new DataTable();
            SqlConnection con2 = new SqlConnection(strcon);


            string query2 = "UPDATE users SET activ = '1', linkactivare = ''  WHERE linkactivare LIKE @match";
            


            SqlCommand cmd2 = new SqlCommand(query2, con2);
            cmd2.Parameters.AddWithValue("@match", matchingValue);


            con2.Open();
            using (SqlDataReader reader = cmd2.ExecuteReader())
            {
                dt.Load(reader);
                con2.Close();
            }


        }
    }
}