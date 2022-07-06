using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;



public partial class user_download_files : System.Web.UI.Page
{
    List<string> hash = new List<string>();
    public StringBuilder strb = new StringBuilder();
    cloud cs = new cloud();
    dbconect conect = new dbconect();
    DataSet ds = new DataSet();


    DataClassesDataContext db = new DataClassesDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Session["user"] = "remya@gmail.com";

            var qry = from item in db.tb_uploads where item.upload_by == Session["user"].ToString() select item;
            DataList1.DataSource = qry;
            DataList1.DataBind();
            for (int i = 0; i < DataList1.Items.Count; i++)
            {
                string fid = ((Label)DataList1.Items[i].FindControl("label2")).Text;
                var duplicated = (from item in db.tb_uploads where item.fid == int.Parse(fid) select item.duplicated_st).FirstOrDefault();
                if (duplicated != "0")
                {
                    ((ImageButton)DataList1.Items[i].FindControl("image1")).ImageUrl = "~/user/assets/images/red-file-folder.gif";
                }
                else
                {
                    ((ImageButton)DataList1.Items[i].FindControl("image1")).ImageUrl = "~/user/assets/images/folder1.gif";

                }
            }
        }
    }

    protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
    {
        string fid = e.CommandArgument.ToString();
        var fname = (from item in db.tb_uploads where item.fid == int.Parse(fid) select item.filename).FirstOrDefault();

        try
        {
            string flename = fname.ToString();
            var duplicated_status = (from item in db.tb_uploads where item.fid == int.Parse(fid) select item.duplicated_st).FirstOrDefault();
            if (duplicated_status == "0")
            {
                List<string> split_file_name = new List<string>();
                List<string> ekey = new List<string>();
                var fname_qry = from item in db.tb_sectors where item.fname == flename select item;
                foreach (var item in fname_qry)
                {
                    split_file_name.Add(item.sname);
                    ekey.Add(item.ekey);
                }
                cloud cs = new cloud();
                for (int i = 0; i < split_file_name.Count(); i++)
                {

                    string splitname = split_file_name[i].ToString();
                    string enckey = ekey[i].ToString();
                    string path = Server.MapPath("~/user/Files/efiles/" + splitname);

                    string content = File.ReadAllText(path);
                    string decrpt = cs.Decrypt(content, enckey);
                    strb.Append(decrpt);


                }

                string files = strb.ToString();
                var exten = (from item in db.tb_uploads where item.fid == int.Parse(fid) select item.ext).FirstOrDefault();
                if (exten == ".txt")
                {

                    string paths = Server.MapPath("~/user/Files/temp/" + fname + ".txt");
                    File.WriteAllText(paths, files);
                    Session["path"] = paths;
                }
                else
                {
                    string paths = Server.MapPath("~/user/Files/temp/" + fname + ".doc");
                    File.WriteAllText(paths, files);
                    Session["path"] = paths;
                }
            }
            else
            {
                List<string> split_file_name = new List<string>();
                List<string> ekey = new List<string>();
                var fname_qry = (from item in db.tb_uploads where item.filename == flename select item.splited_hashes).FirstOrDefault();
                string[] hashes = fname_qry.Split(',');
                //foreach (var item in fname_qry)
                //{
                //    split_file_name.Add(item.sname);
                //    ekey.Add(item.ekey);
                //}
                cloud cs = new cloud();
                for (int i = 0; i < 2; i++)
                {
                    var enckey = (from item in db.tb_sectors where item.shash == hashes[i].ToString() select item.ekey).FirstOrDefault();
                    var sname = (from item in db.tb_sectors where item.shash == hashes[i].ToString() select item.sname).FirstOrDefault();


                    string path = Server.MapPath("~/user/Files/efiles/" + sname);

                    string content = File.ReadAllText(path);
                    string decrpt = cs.Decrypt(content, enckey);
                    strb.Append(decrpt);


                }

                string files = strb.ToString();
                var exten = (from item in db.tb_uploads where item.fid == int.Parse(fid) select item.ext).FirstOrDefault();
                if (exten == ".txt")
                {

                    string paths = Server.MapPath("~/user/Files/temp/" + fname + ".txt");
                    File.WriteAllText(paths, files);
                    Session["path"] = paths;
                }
                else
                {
                    string paths = Server.MapPath("~/user/Files/temp/" + fname + ".doc");
                    File.WriteAllText(paths, files);
                    Session["path"] = paths;
                }

            }

            try
            {

                WebClient req = new WebClient();
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.Buffer = true;
                response.AddHeader("Content-Disposition", "attachment;filename=" + Path.GetFileName(Session["path"].ToString()));
                Response.WriteFile(Session["path"].ToString());

                download down = new download
                {
                    dname = Session["user"].ToString(),
                    fname = fname,
                    download1 = 1,
                    failed = 0,

                };
                db.downloads.InsertOnSubmit(down);
                db.SubmitChanges();
                Response.Flush();



                response.End();






            }
            catch (Exception ex)
            {
                download down = new download
                {
                    dname = Session["user"].ToString(),
                    fname = fname,
                    download1 = 0,
                    failed = 1,

                };
                db.downloads.InsertOnSubmit(down);
                db.SubmitChanges();

                RegisterStartupScript("", "<script Language=JavaScript>alert('Try again later')</Script>");
            }
        }
        catch (Exception ex)
        {
            download down = new download
            {
                dname = Session["user"].ToString(),
                fname = fname,
                download1 = 0,
                failed = 1,

            };
            db.downloads.InsertOnSubmit(down);
            db.SubmitChanges();

        }
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        string serach_word = TextBox1.Text;
        string sha256_hash = cs.createhash_sha256(TextBox1.Text);
        string md5_hash = cs.createhash_md5(TextBox1.Text);
        string name = conect.extscalr("select fname+lname from tb_user_registration where username='" + Session["user"] + "'");
        string p_sha256 = "~/Dataset/" + name + "/Dataset1sha256.txt";
        string p_md5 = "~/Dataset/" + name + "/Dataset2md5.txt";
        string[] arr_sha256 = File.ReadAllLines(Server.MapPath(p_sha256));
        string[] arr_md5 = File.ReadAllLines(Server.MapPath(p_md5));
        if (arr_sha256.Contains(sha256_hash))
        {
            if (arr_md5.Contains(md5_hash))
            {

                ds = conect.discont("select * from tb_upload where upload_by='" + Session["user"] + "'");
                List<string> filename = new List<string>();
                List<string> fid = new List<string>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    filename.Add(ds.Tables[0].Rows[i]["filename"].ToString());
                    fid.Add(ds.Tables[0].Rows[i]["fid"].ToString());
                }
                for (int i = 0; i < filename.Count; i++)
                {
                    string flpath = "~/Dataset/" + name + "/" + filename[i] + ".txt";
                    string[] filecontent = File.ReadAllLines(Server.MapPath(flpath));
                    conect.extnon("truncate table temp");
                    if (filecontent.Contains(sha256_hash))
                    {
                        conect.extnon("insert into temp values('" + filename[i] + "','"+fid[i]+"')");
                    }
                }
                ds.Clear();
                ds = conect.discont("select * from temp");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataList2.DataSource = ds;
                    DataList2.DataBind();
                    Panel1.Visible = true;
                }



            }
        }

    }
    protected void DataList2_UpdateCommand(object source, DataListCommandEventArgs e)
    {
        string fid = ((Label)e.Item.FindControl("Label5")).Text;
        var fname = (from item in db.tb_uploads where item.fid == int.Parse(fid) select item.filename).FirstOrDefault();

        try
        {
            string flename = fname.ToString();
            var duplicated_status = (from item in db.tb_uploads where item.fid == int.Parse(fid) select item.duplicated_st).FirstOrDefault();
            if (duplicated_status == "0")
            {
                List<string> split_file_name = new List<string>();
                List<string> ekey = new List<string>();
                var fname_qry = from item in db.tb_sectors where item.fname == flename select item;
                foreach (var item in fname_qry)
                {
                    split_file_name.Add(item.sname);
                    ekey.Add(item.ekey);
                }
                cloud cs = new cloud();
                for (int i = 0; i < split_file_name.Count(); i++)
                {

                    string splitname = split_file_name[i].ToString();
                    string enckey = ekey[i].ToString();
                    string path = Server.MapPath("~/user/Files/efiles/" + splitname);

                    string content = File.ReadAllText(path);
                    string decrpt = cs.Decrypt(content, enckey);
                    strb.Append(decrpt);


                }

                string files = strb.ToString();
                var exten = (from item in db.tb_uploads where item.fid == int.Parse(fid) select item.ext).FirstOrDefault();
                if (exten == ".txt")
                {

                    string paths = Server.MapPath("~/user/Files/temp/" + fname + ".txt");
                    File.WriteAllText(paths, files);
                    Session["path"] = paths;
                }
                else
                {
                    string paths = Server.MapPath("~/user/Files/temp/" + fname + ".doc");
                    File.WriteAllText(paths, files);
                    Session["path"] = paths;
                }
            }
            else
            {
                List<string> split_file_name = new List<string>();
                List<string> ekey = new List<string>();
                var fname_qry = (from item in db.tb_uploads where item.filename == flename select item.splited_hashes).FirstOrDefault();
                string[] hashes = fname_qry.Split(',');
                //foreach (var item in fname_qry)
                //{
                //    split_file_name.Add(item.sname);
                //    ekey.Add(item.ekey);
                //}
                cloud cs = new cloud();
                for (int i = 0; i < 2; i++)
                {
                    var enckey = (from item in db.tb_sectors where item.shash == hashes[i].ToString() select item.ekey).FirstOrDefault();
                    var sname = (from item in db.tb_sectors where item.shash == hashes[i].ToString() select item.sname).FirstOrDefault();


                    string path = Server.MapPath("~/user/Files/efiles/" + sname);

                    string content = File.ReadAllText(path);
                    string decrpt = cs.Decrypt(content, enckey);
                    strb.Append(decrpt);


                }

                string files = strb.ToString();
                var exten = (from item in db.tb_uploads where item.fid == int.Parse(fid) select item.ext).FirstOrDefault();
                if (exten == ".txt")
                {

                    string paths = Server.MapPath("~/user/Files/temp/" + fname + ".txt");
                    File.WriteAllText(paths, files);
                    Session["path"] = paths;
                }
                else
                {
                    string paths = Server.MapPath("~/user/Files/temp/" + fname + ".doc");
                    File.WriteAllText(paths, files);
                    Session["path"] = paths;
                }

            }

            try
            {

                WebClient req = new WebClient();
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.Buffer = true;
                response.AddHeader("Content-Disposition", "attachment;filename=" + Path.GetFileName(Session["path"].ToString()));
                Response.WriteFile(Session["path"].ToString());

                download down = new download
                {
                    dname = Session["user"].ToString(),
                    fname = fname,
                    download1 = 1,
                    failed = 0,

                };
                db.downloads.InsertOnSubmit(down);
                db.SubmitChanges();
                Response.Flush();



                response.End();






            }
            catch (Exception ex)
            {
                download down = new download
                {
                    dname = Session["user"].ToString(),
                    fname = fname,
                    download1 = 0,
                    failed = 1,

                };
                db.downloads.InsertOnSubmit(down);
                db.SubmitChanges();

                RegisterStartupScript("", "<script Language=JavaScript>alert('Try again later')</Script>");
            }
        }
        catch (Exception ex)
        {
            download down = new download
            {
                dname = Session["user"].ToString(),
                fname = fname,
                download1 = 0,
                failed = 1,

            };
            db.downloads.InsertOnSubmit(down);
            db.SubmitChanges();

        }


    }

}