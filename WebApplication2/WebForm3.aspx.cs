﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Session["username"] = "Donald Duck";
            Session["pass"] = "test";
            //Response.Write("Welcome " + Session["username"]);


        }
    }
}