using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_add_auditor : System.Web.UI.Page
{
    DataClassesDataContext db = new DataClassesDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var qry = from item in db.tb_auditors select item;
            if (qry.Count() > 0)
            {
                DataList1.DataSource = qry;
                DataList1.DataBind();
            }
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string path = "";
        if (FileUpload1.HasFiles)
        {
            path = "~/Index/login/Register/auditor/"+FileUpload1.FileName;
            FileUpload1.SaveAs(MapPath(path));
        }
        dbconect connect = new dbconect();
        string password = connect.numpassword(4);
        var query = from o in db.tb_logins
                    where o.username == "" + TextBox2.Text || o.password == "" + password
                    select o;
        if (query.Count() > 0)
        {
            Response.Write("<script>alert('Existed Details')</script>");
        }
        else
        {
            tb_login tb = new tb_login
            {
                username = TextBox2.Text,
                password = password,
                utype = "auditor",
                st = 1,
            };
            
            
                tb_auditor reg = new tb_auditor
                {
                   email=TextBox2.Text,
                   aname=TextBox1.Text,
                   phne=TextBox3.Text,
                    rdate = DateTime.Now.ToString("dd/MM/yyyy"),
                    photo = path,

                };
                db.tb_logins.InsertOnSubmit(tb);
                db.tb_auditors.InsertOnSubmit(reg);
                db.SubmitChanges();

            }
        Response.Redirect("~/Admin/add_auditor.aspx");
        }
    }
