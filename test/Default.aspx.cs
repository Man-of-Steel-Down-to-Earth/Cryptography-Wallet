using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class test_Default : System.Web.UI.Page
{
    DataClassesDataContext db = new DataClassesDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            List<string> alreadhhash = new List<string>();

            var qry1 = (from item in db.tb_sectors select item.shash).ToList();
            foreach (var q in qry1)
            {
                alreadhhash.Add(q);

            }
        }
    }
}