using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class user_Audit_myfiles : System.Web.UI.Page
{
    dbconect db = new dbconect();
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            ds.Clear();
            ds = db.discont1("select * from tb_upload where duplicated_st='" + "0" + "' and upload_by='" + Session["user"] + "'");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataList2.DataSource = ds;
                DataList2.DataBind();
            }
            for (int i = 0; i < DataList2.Items.Count; i++)
            {
                string fid = ((Label)DataList2.Items[i].FindControl("Label10")).Text;
                string fname = ((Label)DataList2.Items[i].FindControl("Label7")).Text;


                ds.Clear();
                ds = db.discont("select splited_hashes from tb_upload  where fid='" + fid + "'");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    List<string> partial_dup = new List<string>();
                    List<string> full_dup = new List<string>();
                    string splited_hash = ds.Tables[0].Rows[0]["splited_hashes"].ToString();
                    string[] ar = splited_hash.Split(',');
                    string first_hash = ar[0];
                    string second_hash = ar[1];

                    DataSet ds1 = new DataSet();
                    ds1 = db.discont("select duplicated_parts,upload_by from tb_upload where fid!='" + fid + "' and duplicated_parts!='" + "0" + "' ");
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                        {
                            string finded = ds1.Tables[0].Rows[j]["duplicated_parts"].ToString();
                            string[] fetch = finded.Split(',');
                            if (fetch.Contains(first_hash))
                            {
                                if (fetch.Contains(second_hash))
                                {
                                    full_dup.Add(ds1.Tables[0].Rows[j]["upload_by"].ToString());

                                }
                                else
                                {
                                    partial_dup.Add(ds1.Tables[0].Rows[j]["upload_by"].ToString());
                                }
                            }
                            else if (fetch.Contains(second_hash))
                            {
                                partial_dup.Add(ds1.Tables[0].Rows[j]["upload_by"].ToString());
                            }
                        }
                    }

                    int f_dup = full_dup.Count();
                    int p_dup = partial_dup.Count();
                    ((Label)DataList2.Items[i].FindControl("Label9")).Text = f_dup.ToString();
                    ((Label)DataList2.Items[i].FindControl("Label11")).Text = p_dup.ToString();


                }





              
            }
        }

    }
    protected void DataList2_UpdateCommand(object source, DataListCommandEventArgs e)
    {
        Session["fid"] = ((Label)e.Item.FindControl("Label10")).Text;
        Response.Redirect("fullduplication.aspx");
    }
    protected void DataList2_DeleteCommand(object source, DataListCommandEventArgs e)
    {
        Session["fid"] = ((Label)e.Item.FindControl("Label10")).Text;
        Response.Redirect("partialDuplication.aspx");
    }
}