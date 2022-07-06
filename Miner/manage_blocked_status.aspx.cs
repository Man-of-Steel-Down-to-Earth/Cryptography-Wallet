using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Admin_manage_blocked_status : System.Web.UI.Page
{
    DataClassesDataContext db = new DataClassesDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var qry = from item in db.tb_user_registrations select item;
            DataList1.DataSource = qry;
            DataList1.DataBind();
            for (int i = 0; i < DataList1.Items.Count; i++)
            {
                string email = ((TextBox)DataList1.Items[i].FindControl("TextBox2")).Text;

                var duplcate_count = (from item in db.tb_uploads where item.duplicated_st == "1" && item.upload_by == email select item).Count();
                int cnt = duplcate_count;

                var tot_files = (from item in db.tb_uploads select item).Count();
                int cnt_files = tot_files;

                int percent = (100 * cnt) / cnt_files;
                if (percent >= 50)
                {
                    ((Button)DataList1.Items[i].FindControl("label2")).Text=percent.ToString()+" % ";
                    ((Button)DataList1.Items[i].FindControl("label2")).BackColor = System.Drawing.Color.OrangeRed;

                }
                else
                {
                    ((Button)DataList1.Items[i].FindControl("label2")).Text = percent.ToString()+" % ";
                    ((Button)DataList1.Items[i].FindControl("label2")).BackColor = System.Drawing.Color.Green;

                }
            }
            }
    }

    protected void DataList1_UpdateCommand(object source, DataListCommandEventArgs e)
    {
        string username= ((TextBox)e.Item.FindControl("TextBox2")).Text;
        var qry = from item in db.tb_logins where item.username == username select item;
        foreach (var item in qry)
        {
            item.st = 1;
        }
        db.SubmitChanges();
    }
}