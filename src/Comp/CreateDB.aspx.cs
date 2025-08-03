using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
using System.IO;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Configuration;
public partial class Comp_CreateDB : UICulturePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckSecurity();
    }
    private void CheckSecurity()
    {
        if (MyContext.UserProfile.Contact_ID != 1) Response.Redirect(PageLinks.Authorization, true);

    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        XpressDataContext dc = new XpressDataContext();
        SqlConnectionStringBuilder OldConStringBulider = new SqlConnectionStringBuilder(dc.Connection.ConnectionString);
        int index = 0;
        string NewDatabaseName = string.Empty;
        string fileName = string.Empty;
        do
        {
            fileName = txtName.Text.ToExpressString() + ".bak";  //DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + "-" + index.ToExpressString() + ".bak";
            index++;
        }
        while (File.Exists(Server.MapPath("~\\DatabaseBackups\\" + fileName)));
        dc.usp_Backup_Create(Server.MapPath("~\\DatabaseBackups\\" + fileName), OldConStringBulider.InitialCatalog);
        dc.usp_FiscalYearRestoreDB(dc.Connection.Database.Split("-".ToCharArray())[0] + "_" + fileName.Replace(".bak",""), Server.MapPath("~\\DatabaseBackups\\" + fileName), ref NewDatabaseName);
        UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        txtName.Text = string.Empty;
    }
}