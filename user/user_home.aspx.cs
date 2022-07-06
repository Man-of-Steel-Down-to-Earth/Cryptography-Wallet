using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class user_user_home : System.Web.UI.Page
{
    dbconect db1 = new dbconect();
    DataSet ds=new DataSet();
    DataClassesDataContext db = new DataClassesDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
           

            var duplcate_count = (from item in db.tb_uploads where item.duplicated_st == "1" && item.upload_by == Session["user"] select item).Count();
            int cnt = duplcate_count;

            var tot_files = (from item in db.tb_uploads select item).Count();
            int cnt_files = tot_files;

            int percent = (100 * cnt) / cnt_files;
            if (percent >= 50)
            {
                Label1.Visible = true;
                Label2.Visible = false;
               Label1.Text = "Duplicated Files :   "+percent.ToString() + " % ";
              Label1.BackColor = System.Drawing.Color.OrangeRed;
              Label1.ForeColor = System.Drawing.Color.White;
              Label1.Font.Bold = true;

            }
            else
            {
                Label1.Visible = false;
                Label2.Visible = true;
                Label2.Text = "Duplicated Files :   " + percent.ToString() + " % ";
              Label2.BackColor = System.Drawing.Color.Green;
              Label2.ForeColor = System.Drawing.Color.White;
              Label2.Font.Bold = true;

            }
        }

    }
}