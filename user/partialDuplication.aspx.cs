using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class user_partialDuplication : System.Web.UI.Page
{
    dbconect db = new dbconect();
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string filename = db.extscalr("select filename from tb_upload where fid='" + Session["fid"] + "'");
            Label6.Text = "Partial Duplicated Uploading of " + filename;
            string check = db.extscalr("select splited_hashes from tb_upload where fid='" + Session["fid"] + "'");
            string[] ar = check.Split(',');
            string first = ar[0].ToString() + ",";
            string second = ar[1].ToString() + ",";
            ds = db.discont("select fname+lname as name,upload_date,upload_by from [tb_upload],[tb_user_registration] where [tb_user_registration].username=tb_upload.upload_by and [duplicated_st]='" + "1" + "' and duplicated_parts in('" + first + "','" + second + "')");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataList1.DataSource = ds;
                DataList1.DataBind();
            }
        }
    }
    protected void DataList1_UpdateCommand(object source, DataListCommandEventArgs e)
    {
        ((Panel)e.Item.FindControl("Panel1")).Visible = true;
    }
    protected void DataList1_DeleteCommand(object source, DataListCommandEventArgs e)
    {
        string to = ((Label)e.Item.FindControl("Label7")).Text;
        string complaints = ((TextBox)e.Item.FindControl("TextBox1")).Text;
        db.extnon("insert into tb_duplication_complaints values('" + Session["user"] + "','" + to + "','" + DateTime.Now.ToString() + "','" + complaints + "','" + "0" + "')");
        Response.Redirect("Audit_myfiles.aspx");

    }
}