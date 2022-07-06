using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class Admin_view_feedback : System.Web.UI.Page
{
    dbconect db = new dbconect();
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            ds = db.discont("select date,des,fname+lname as name from [tb_feedback],[tb_user_registration] where [tb_user_registration].username=tb_feedback.[for_feedback] ");
            if(ds.Tables[0].Rows.Count>0)
            {
                GridView1.DataSource = ds;
                GridView1.DataBind();
            }
        }

    }
}