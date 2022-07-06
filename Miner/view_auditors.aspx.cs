using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_view_auditors : System.Web.UI.Page
{
    DataClassesDataContext db = new DataClassesDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var qry = from item in db.tb_auditors select item;
            DataList1.DataSource = qry;
            DataList1.DataBind();
            for (int i = 0; i < DataList1.Items.Count; i++)
            {
                string email = ((Button)DataList1.Items[i].FindControl("button1")).CommandArgument;
                var st = (from item in db.tb_logins where item.username == email select item.st).FirstOrDefault();
                if (st == 1)
                {
                    ((Button)DataList1.Items[i].FindControl("button1")).Text = "Click To Reject";
                    ((Button)DataList1.Items[i].FindControl("button1")).BackColor = System.Drawing.Color.PaleVioletRed;


                }
                else
                {
                    ((Button)DataList1.Items[i].FindControl("button1")).Text = "Click To Accept";
                    ((Button)DataList1.Items[i].FindControl("button1")).BackColor = System.Drawing.Color.LightGreen;

                }
            }
        }
    }

    protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
    {
        string email = e.CommandArgument.ToString();
        var st = (from item in db.tb_logins where item.username == email select item.st).FirstOrDefault();
        if (st == 1)
        {
            var qry = from item in db.tb_logins where item.username == email select item;
            if (qry.Count() > 0)
            {
             
                foreach(var q in qry)
                {
                    q.st = 0;
                    break;
                }
            }
        }
        else
        {
            var qry = from item in db.tb_logins where item.username == email select item;
            if (qry.Count() > 0)
            {

                foreach (var q in qry)
                {
                    q.st = 1;
                    break;
                }
            }
        }
        db.SubmitChanges();
        Response.Redirect("~/Admin/view_auditors.aspx");
    }
}