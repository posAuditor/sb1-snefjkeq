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

public partial class Comp_FiscalYearClosing : UICulturePage
{


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (new XpressDataContext().usp_Company_Select().FirstOrDefault().IsYearClosed.Value)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.YearAlreadyClosed, PageLinks.StartScreen);
                return;
            }
            this.FixUnbalancedOperations();
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {

        System.Data.Common.DbTransaction trans = null;

        try
        {

            XpressDataContext oldDatabase_dc = new XpressDataContext();
            XpressDataContext NewDatabase_dc = new XpressDataContext();
            var re = oldDatabase_dc.usp_FiscalYear_ICJ_Close(MyContext.UserProfile.Contact_ID, true);
            int result = Convert.ToInt32(re);
            if (result == -3)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.NegativeQty, string.Empty);
                return;
            }

            NewDatabase_dc.Connection.ConnectionString = this.CreateNewDatabase();

            NewDatabase_dc.Connection.Open();
            trans = NewDatabase_dc.Connection.BeginTransaction();
            NewDatabase_dc.Transaction = trans;

            NewDatabase_dc.usp_FiscalYear_ICJ_Close(MyContext.UserProfile.Contact_ID, false);

            foreach (var branch in NewDatabase_dc.usp_Branchs_Select(string.Empty, null))
            {
                NewDatabase_dc.usp_FiscalYearOperations_Close(MyContext.UserProfile.Contact_ID, branch.ID);
            }
            NewDatabase_dc.usp_FiscalYearOperations_Close(MyContext.UserProfile.Contact_ID, 0);

            NewDatabase_dc.usp_FiscalYear_Close();

            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, PageLinks.CompanySettings + "?StartNewYear=1");
            trans.Commit();

            oldDatabase_dc.usp_Company_Select().FirstOrDefault().IsYearClosed = true;
            oldDatabase_dc.SubmitChanges();
        }
        catch (Exception ex)
        {
            if (trans != null) trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private string CreateNewDatabase()
    {
        XpressDataContext dc = new XpressDataContext();
        SqlConnectionStringBuilder OldConStringBulider = new SqlConnectionStringBuilder(dc.Connection.ConnectionString);
        int index = 0;
        string NewDatabaseName = string.Empty;
        string fileName = string.Empty;
        do
        {
            fileName = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + "-" + index.ToExpressString() + ".bak";
            index++;
        }
        while (File.Exists(Server.MapPath("~\\DatabaseBackups\\" + fileName)));
        dc.usp_Backup_Create(Server.MapPath("~\\DatabaseBackups\\" + fileName), OldConStringBulider.InitialCatalog);
        dc.usp_FiscalYearRestoreDB(string.Format("{0}-{1:dd_MM_yyyy}", dc.Connection.Database.Split("-".ToCharArray())[0], MyContext.FiscalYearEndDate.AddDays(1)), Server.MapPath("~\\DatabaseBackups\\" + fileName), ref NewDatabaseName);

        try { File.Delete(Server.MapPath("~\\DatabaseBackups\\" + fileName)); }
        catch { }
        SqlConnectionStringBuilder NewConStringBulider = new SqlConnectionStringBuilder(OldConStringBulider.ToExpressString());
        NewConStringBulider.InitialCatalog = NewDatabaseName;
        if (!NewConStringBulider.IntegratedSecurity) NewConStringBulider.Password = GetDBUserPass(OldConStringBulider.ToExpressString());

        Configuration WebConfig = WebConfigurationManager.OpenWebConfiguration("~");
        ConnectionStringsSection ConectionStringsSection = (ConnectionStringsSection)WebConfig.GetSection("connectionStrings");

        foreach (ConnectionStringSettings Connection in ConectionStringsSection.ConnectionStrings)
        {
            Connection.ConnectionString = NewConStringBulider.ToExpressString();
            ConectionStringsSection.ConnectionStrings.Remove(Connection.Name);
            ConectionStringsSection.ConnectionStrings.Add(Connection);
        }
        if (File.Exists(Server.MapPath("~/Web.config"))) File.SetAttributes(Server.MapPath("~/Web.config"), FileAttributes.Normal);
        WebConfig.Save(ConfigurationSaveMode.Modified);

        System.Web.HttpCookie c = new System.Web.HttpCookie("ConnectionStringV2");
        c.Value = Server.UrlEncode(NewConStringBulider.ConnectionString);
        c.Expires = DateTime.Now.AddYears(1);
        Response.Cookies.Add(c);

        return NewConStringBulider.ToExpressString();
    }

    private string GetDBUserPass(string ConnectionString)
    {
        string userPass = string.Empty;
        string[] ConInfo = ConnectionString.Split(";".ToCharArray());
        userPass = Array.Find<string>(ConInfo, str => str.Replace(" ", "").ToLower().Contains("password="));
        if (userPass != null) userPass = userPass.Split("=".ToCharArray())[1].Trim();
        return userPass;
    }
}