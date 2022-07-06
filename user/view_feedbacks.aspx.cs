using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class user_view_feedbacks : System.Web.UI.Page
{
    dbconect db = new dbconect();
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
if(!IsPostBack)
{
    ds = db.discont("select * from [tb_feedback] where [for_feedback]='" + Session["user"] + "' ");
    if (ds.Tables[0].Rows.Count > 0)
    {
        GridView1.DataSource = ds;
        GridView1.DataBind();
    }
}
    }
}