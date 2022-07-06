using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class user_complaints : System.Web.UI.Page
{
    dbconect db = new dbconect();
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ds = db.discont("select [c_from],[c_to],[date],[complaint] from [tb_duplication_complaints] where c_to='"+Session["user"]+"'");
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView1.DataSource = ds;
                GridView1.DataBind();
            }
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                string frommail = ((Label)GridView1.Rows[i].Cells[0].FindControl("Label2")).Text;
              

                string fromname = db.extscalr("select fname+lname from tb_user_registration where username='" + frommail + "'");
                ((Label)GridView1.Rows[i].Cells[0].FindControl("Label5")).Text = fromname;
               


            }
        }
    }
}