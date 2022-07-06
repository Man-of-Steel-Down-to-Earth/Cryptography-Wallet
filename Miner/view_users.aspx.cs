using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Admin_view_users : System.Web.UI.Page
{
    DataClassesDataContext db = new DataClassesDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var qry = from item in db.tb_user_registrations
                      select
                      item;
            if (qry.Count() > 0)
            {
                GridView1.DataSource = qry;
                GridView1.DataBind();
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    string id = ((Label)GridView1.Rows[i].Cells[0].FindControl("label1")).Text;
                    var qry1 = from item in db.tb_user_registrations
                               where item.uid == int.Parse(id)
                               select
                               item;
                    if (qry.Count() > 0)
                    {
                        foreach(var q in qry1)
                        {
                            string email = q.username;
                            var qry2= from item in db.tb_logins
                                        where item.username == email
                                        select
                                        item;
                            foreach (var q1 in qry2)
                            {
                                int status =int.Parse( q1.st.ToString());
                                if (status == 1)
                                {
                                    ((Label)GridView1.Rows[i].Cells[4].FindControl("label11")).Text = "Accepted";
                                    ((Label)GridView1.Rows[i].Cells[4].FindControl("label11")).ForeColor = System.Drawing.Color.Green;

                                }
                                else
                                {
                                    ((Label)GridView1.Rows[i].Cells[4].FindControl("label11")).Text = "Rejected";
                                    ((Label)GridView1.Rows[i].Cells[4].FindControl("label11")).ForeColor = System.Drawing.Color.Red;

                                }
                            }
                            }
                    }

                }  
            }
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        List<string> collect_users = new List<string>();

        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            bool b = ((CheckBox)GridView1.Rows[i].Cells[0].FindControl("checkbox1")).Checked;
            if (b == true)
            {
                collect_users.Add(GridView1.Rows[i].Cells[2].Text);

            }
        }
       
            foreach(string element in collect_users)
            {
                string userid = element;
                var status = (from item in db.tb_logins where item.username == userid select item.st).FirstOrDefault();
                string u_st = status.ToString();
                
                if (u_st== "1"){
                  var upd_st=  (from p in db.tb_logins
                     where p.username == userid
                     select p);
                    int flag = 0;
                    foreach (var item in upd_st)
                    {
                        item.st = 0;
                        flag = 1;
                        if (flag == 1)
                        {
                            break;
                        }
                    }
                    db.SubmitChanges();
                }
                else{
                    var upd_st = (from p in db.tb_logins
                                  where p.username == userid
                                  select p);
                    int flag = 0;
                    foreach (var item in upd_st)
                    {
                        item.st = 1;
                        flag = 1;
                        if (flag == 1)
                        {
                            break;
                        }
                    }
                    db.SubmitChanges();
                }
               
            }
        

        Response.Redirect("~/Admin/view_users.aspx");

        }
}