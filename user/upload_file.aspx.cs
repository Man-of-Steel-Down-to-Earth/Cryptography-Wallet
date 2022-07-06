using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class user_assets_upload_file : System.Web.UI.Page
{
    List<string> Packets = new List<string>();
    List<string> hash = new List<string>();
    public static StringBuilder strb = new StringBuilder();
    pdfreader pdf = new pdfreader();
    read_doc doc = new read_doc();
    read_txt txt = new read_txt();
    cloud cs = new cloud();
    DataClassesDataContext db = new DataClassesDataContext();
    dbconect connect = new dbconect();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           // Session["user"] = "neethu@gmail.com";
            var qry = (from item in db.tb_user_registrations where item.username == Session["user"].ToString() select item.fname).FirstOrDefault();
            TextBox1.Text = "UPLOAD BY  :  "+qry;
            TextBox4.Text = DateTime.Now.ToString();
            var count_value = (from cnt in db.tb_uploads where cnt.upload_by ==Session["user"].ToString() select cnt).Count();
            TextBox5.Text = count_value.ToString();
            var count_duplicate = (from cnt in db.tb_uploads where cnt.upload_by == Session["user"].ToString() && cnt.duplicated_st!="0" select cnt).Count();
            TextBox6.Text = count_duplicate.ToString();

        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {




        List<string> not_existed = new List<string>();
        string orginalfilename = TextBox2.Text + FileUpload1.FileName;
        int qry_cnt = 0;
        try
        {
            var qry = (from item in db.tb_uploads where item.filename == TextBox2.Text select item.filename).FirstOrDefault();
             qry_cnt = qry.Count();
        }
        catch(Exception ex)
        {
            qry_cnt = 0;
        }
            if (qry_cnt == 0)
        {
            var query = from r in db.tb_temps select r;

            db.tb_temps.DeleteAllOnSubmit(query);

            db.SubmitChanges();

           

            string path = "~/user/Files/sfiles/" + TextBox2.Text + FileUpload1.FileName;
            FileUpload1.SaveAs(MapPath(path));

                //extracting each word in a file//
            List<string> allword = new List<string>();
            char[] arr = { ' ', ',', ';', ':', '(', ')', '$', '&', '_', '"', '"', '*' };

            string[] all_text = File.ReadAllLines(Server.MapPath(path));
            foreach (string oneword in all_text)
            {
                string[] slitword = oneword.Split(arr);
                foreach (string s in slitword)
                {
                    allword.Add(s);
                }
            }

                //removing stopwords..................//

            string[] words = allword.ToArray();
            List<string> final_words = new List<string>(words);
            string st_path = Server.MapPath("~/user/Files/words_stopwords.txt");

            for (int i = 0; i < final_words.Count; i++)
            {
                if (final_words[i] == "")
                {

                    final_words.Remove(final_words[i].ToString());
                    i = 0;
                }

            }
            string[] stop = File.ReadAllLines(st_path);
            List<string> orginal_word = new List<string>();
            for (int i = 0; i < final_words.Count(); i++)
            {
                if (!stop.Contains(final_words[i]))
                {
                    if (!orginal_word.Contains(final_words[i]))
                    {

                        orginal_word.Add(final_words[i].ToString());
                    }
                }

            }

                //creating mtching file................//
                string name=connect.extscalr("select fname+lname from tb_user_registration where username='"+Session["user"]+"'");

                string shavaluefile = "~/Dataset/" + name + "/" + TextBox2.Text + ".txt";
                FileStream fs = null;
                if(!File.Exists(Server.MapPath(shavaluefile)))
                {
                    using (fs = File.Create(Server.MapPath(shavaluefile)))
                    {
                    }
                   
                    
                }
                fs.Close();


                List<string> listsha=new List<string>();
                List<string> listmd5 = new List<string>(); 
                List<string> sha_list=new List<string>();

                string p_sha256 = "~/Dataset/" + name + "/Dataset1sha256.txt";
                string p_md5 = "~/Dataset/" + name + "/Dataset2md5.txt";
            for (int i = 0; i < orginal_word.Count; i++)
            {
                string sha256 = cs.createhash_sha256(orginal_word[i]);
                string md5 = cs.createhash_md5(orginal_word[i]);
                
                string[] sha_arr = File.ReadAllLines(Server.MapPath(p_sha256));
                foreach (var item in sha_arr)
                {
                    listsha.Add(item); 
                }
                
                if(!listsha.Contains(sha256))
                {
                    listsha.Add(sha256); 
                   
                }


                string[] md5_arr = File.ReadAllLines(Server.MapPath(p_md5));
                foreach (var item in md5_arr)
                {
                    listmd5.Add(item);
                }
                
                if (!listmd5.Contains(md5))
                {
                    listmd5.Add(md5);
                }

                string SHA123 = "~/Dataset/" + name + "/" + TextBox2.Text +".txt";
                
               
                string[] sha_file = File.ReadAllLines(Server.MapPath(SHA123));
             
               foreach (var item in sha_file)
               {
                   sha_list.Add(item);
               }
                
               sha_list.Add(sha256);
               // string[] arr12=sha_list.ToArray();
               // File.WriteAllLines(Server.MapPath(shavaluefile), arr12); ;


            }
                 string SHA1234 = "~/Dataset/" + name + "/" + TextBox2.Text + ".txt";
            string[] file_sha = sha_list.ToArray();
            File.WriteAllLines(Server.MapPath(SHA1234), file_sha);

            //string[] arr_splsha = sha_list.ToArray();

                //writing to dataset1

            string[] arr_sha256 = listsha.ToArray();
            File.WriteAllLines(Server.MapPath(p_sha256),arr_sha256);


                //writing to datalist2

                string [] arr_md5=listmd5.ToArray();
                File.WriteAllLines(Server.MapPath(p_md5),arr_md5);




            //...............hashing  and splitting file...............................//
            string fname = TextBox2.Text + FileUpload1.FileName;
            string ext = System.IO.Path.GetExtension(FileUpload1.FileName);

            string source = Server.MapPath("~/user/Files/sfiles/" + TextBox2.Text + FileUpload1.FileName);
            string contents = "";
            if (ext == ".txt")
            {
                contents = txt.readtxt(source);
            }
            else if ((ext == ".doc") || (ext == ".docx"))
            {
                contents = doc.read_from_doc(source);
            }
            else if (ext == ".pdf")
            {
                contents = pdf.pdfText(source);
            }

            string extractpath = Server.MapPath("~/user/Files/sfiles/" + TextBox2.Text + ".txt");
            File.WriteAllText(extractpath, contents);
            string full_hash = createhash(contents);

            SplitFile(extractpath, 2);
            Dictionary<string, string> non_duplicate = new Dictionary<string, string>();
            for (int i = 0; i < 2; i++)
            {
                string hashp = Server.MapPath("~/user/Files/sfiles/" + Packets[i].ToString());
                string content = File.ReadAllText(hashp);
                string hashing = createhash(content);
                non_duplicate.Add(hashing, Packets[i].ToString());
                hash.Add(hashing);
            }

            //.............checking if any duplicating file occur............

            List<string> alreadhhash = new List<string>();

            var qry1 = (from item in db.tb_sectors select item.shash).ToList();
           foreach(var q in qry1)
            {
                alreadhhash.Add(q);

            }
            

            List<string> any = new List<string>();
           
            foreach (string chash in hash)
            {
                if (alreadhhash.Contains(chash))
                {
                    any.Add(chash);
                }
                else
                {
                    not_existed.Add(chash);
                    string non_path =Server.MapPath("~/user/Files/sfiles/"+ non_duplicate[chash]);
                    string econt = File.ReadAllText(non_path);
                    Random rm = new Random();
                    int num = rm.Next(4, 6);
                   
                    string pass = connect.MakePwd(num);
                    cloud cs = new cloud();
                    string encpt = cs.encrypt(econt, pass);
                    string newpaths = Server.MapPath("~/user/Files/efiles/" + non_duplicate[chash]);
                    //upload into cloud
                    File.WriteAllText(newpaths, encpt);
                    int maxNumber = 0;
                    try
                    {
                         maxNumber = (from x in db.tb_uploads select x.fid).Max();
                    }
                    catch(Exception ex)
                    {

                    }
                    

                    //cs.cloud1(newpaths);
                    tb_sector sector = new tb_sector
                    {
                        fname = TextBox2.Text,
                        sname = non_duplicate[chash],
                        shash = chash,
                        ekey = pass,
                        status = "0",
                        uid = maxNumber.ToString(),

                    };
                    db.tb_sectors.InsertOnSubmit(sector);

                    db.SubmitChanges();
                }

                //for (int i = 0; i < Packets.Count(); i++)
                //{
                //    string sfile = Server.MapPath("~/user/Files/sfiles/" + non_duplicate[chash]);
                //    File.Delete(sfile);
                //}
            }
            

            //..........................if any duplicate file is occur............

            if (any.Count() != 0)
            {
               


                string parts = "";
                foreach (string item in any)
                {
                    parts = parts + item + ",";
                }
                string non = "";
                if (not_existed.Count() > 0)
                {
                    foreach (string s in not_existed)
                    {
                        non = non + s;
                    }
                }
                string h = "";
                foreach (var item in hash)
                {
                    h = h + item + ",";
                }
                //.............status=1 means file is already exit and passes to cloud server for proof of ownership......
                tb_upload upload = new tb_upload
                {
                    filename = TextBox2.Text,
                    upload_by = Session["user"].ToString(),
                    hash = full_hash,
                    description = TextBox3.Text,
                    upload_date = TextBox4.Text,
                    date = DateTime.Now.ToString(),
                    duplicated_st="1",
                    duplicated_parts=parts,
                    non_duplicated_parts=non,
                    splited_hashes=h,
                    ext=ext,

                };
                db.tb_uploads.InsertOnSubmit(upload);

                db.SubmitChanges();


                var duplcate_count = (from item in db.tb_uploads where item.duplicated_st == "1" && item.upload_by == Session["user"].ToString() select item).Count();
                int cnt = duplcate_count;

                var tot_files = (from item in db.tb_uploads select item).Count();
                int cnt_files = tot_files;

                int percent = (100 * cnt) / cnt_files;
                if (percent >= 50)
                {
                    var qry = from item in db.tb_logins where item.username == Session["user"].ToString() select item;
                    foreach (var item in qry)
                    {
                        item.st = 0;
                    }
                    db.SubmitChanges();

                    tb_blocked_status bl = new tb_blocked_status
                    {
                        username = Session["user"].ToString(),
                        bst = "0",
                        date = DateTime.Now.ToString(),
                    };
                    db.tb_blocked_status.InsertOnSubmit(bl);
                    db.SubmitChanges();
                    Response.Redirect("~/Index/login/login.aspx");
                }

                RegisterStartupScript("", "<script Language=JavaScript>alert('Duplicate file ,file pass to cloud server for proof of ownership')</Script>");

              

            }
            //..............................if no duplicate file....................

            else
            {
                string h = "";
                foreach (var item in hash)
                {
                    h = h + item+",";
                }
                //.............status=0 means file is not exit already......

                tb_upload upload = new tb_upload
                {
                    filename = TextBox2.Text,
                    upload_by = Session["user"].ToString(),
                    hash = full_hash,
                    description = TextBox3.Text,
                    upload_date = TextBox4.Text,
                    date = DateTime.Now.ToString(),
                    duplicated_st = "0",
                    duplicated_parts = "0",
                    non_duplicated_parts = "all hash",
                    splited_hashes = h,
                    ext = ext,

                };
                db.tb_uploads.InsertOnSubmit(upload);

                db.SubmitChanges();
                
                for (int i = 0; i < Packets.Count(); i++)
                {
                    string sfile = Server.MapPath("~/user/Files/sfiles/" + Packets[i].ToString());
                    File.Delete(sfile);
                }
                RegisterStartupScript("", "<script Language=JavaScript>alert('Uploaded')</Script>");
            }

        }
        else
        {
            tb_filename_checking tb = new tb_filename_checking
            {
                fname = TextBox2.Text,
                uploader = Session["user"].ToString(),
                count = "1",
                attepmt_date = DateTime.Now.ToString("dd/MM/yyyy"),

            };
        db.tb_filename_checkings.InsertOnSubmit(tb);
            db.SubmitChanges();

            RegisterStartupScript("", "<script Language=JavaScript>alert('File Name already occure')</Script>");
        }
    }



    public bool SplitFile(string SourceFile, int nNoofFiles)
    {
        bool Split = false;
        try
        {
            FileStream fs = new FileStream(SourceFile, FileMode.Open, FileAccess.Read);
            int SizeofEachFile = (int)Math.Ceiling((double)fs.Length / nNoofFiles);

            for (int i = 0; i < nNoofFiles; i++)
            {
                string baseFileName = TextBox2.Text;
                string Extension = Path.GetExtension(SourceFile);

                FileStream outputFile = new FileStream(Path.GetDirectoryName(SourceFile) + "\\" + TextBox2.Text + "." +
                    i.ToString().PadLeft(2, Convert.ToChar("0")) + Extension, FileMode.Create, FileAccess.Write);

                //    mergeFolder = Path.GetDirectoryName(Server.MapPath(SourceFile));
                //    Session["merg"] = mergeFolder;

                int bytesRead = 0;
                byte[] buffer = new byte[SizeofEachFile];

                if ((bytesRead = fs.Read(buffer, 0, SizeofEachFile)) > 0)
                {
                    outputFile.Write(buffer, 0, bytesRead);
                    //outp.Write(buffer, 0, BytesRead);

                    string packet = baseFileName + "." + i.ToString().PadLeft(2, Convert.ToChar("0")) + Extension.ToString();
                    Packets.Add(packet);
                }

                outputFile.Close();

            }
            fs.Close();

        }
        catch (Exception Ex)
        {
            throw new ArgumentException(Ex.Message);
        }

        return Split;
    }
    public static int dtkeysize = 1024;
    public string createhash(string data)
    {
        int keysize = dtkeysize / 8;
        byte[] bytes = Encoding.UTF32.GetBytes(data);
        byte[] byt = SHA1.Create().ComputeHash(bytes);
        string Hashvalue = Convert.ToBase64String(byt);
        return Hashvalue;
    }
}