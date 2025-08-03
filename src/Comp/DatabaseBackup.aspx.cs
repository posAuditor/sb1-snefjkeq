using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

public partial class Comp_DatabaseBackup : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();


    private string OldName
    {
        get
        {
            if (ViewState["OldName"] == null) return "";
            return (string)ViewState["OldName"];
        }

        set
        {
            ViewState["OldName"] = value;
        }
    }
    private string SelectRowsName
    {
        get
        {
            if (ViewState["SelectRowsName"] == null) return "";
            return (string)ViewState["SelectRowsName"];
        }

        set
        {
            ViewState["SelectRowsName"] = value;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                this.FillBackups();
                if (!this.MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
                gvBackups.Columns[2].Visible = this.MyContext.PageData.IsDelete;
                gvBackups.Columns[3].Visible = this.MyContext.PageData.IsApprove;
                lnkBackUp.Visible = this.MyContext.PageData.IsAdd;
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void lnkBackUp_Click(object sender, EventArgs e)
    {
        try
        {
            int index = 0;
            string fileName = string.Empty;
            do
            {
                fileName = dc.Connection.Database + DateTime.Now.ToString("_d-M-yyyy-H-m") + "-" + index.ToExpressString() + ".bak";
                index++;
            }
            while (File.Exists(Server.MapPath("~\\DatabaseBackups\\" + fileName)));
            dc.usp_Backup_Create(Server.MapPath("~\\DatabaseBackups\\" + fileName), dc.Connection.Database);
            this.Encrypt(Server.MapPath("~\\DatabaseBackups\\" + fileName));
            this.FillBackups();
            LogAction(Actions.Add, string.Empty, dc);
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvBackups_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvBackups.PageIndex = e.NewPageIndex;
            this.FillBackups();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvBackups_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            txtPassword1.Text = string.Empty;
            mpeValidation.Show();
            SelectRowsName = gvBackups.DataKeys[e.NewSelectedIndex]["FullName"].ToExpressString();


            //string backupPath = gvBackups.DataKeys[e.NewSelectedIndex]["FullName"].ToExpressString();
            //this.Decrypt(backupPath);
            //backupPath = Path.ChangeExtension(backupPath, ".bak");
            //string stp4 = @"USE MASTER; ALTER DATABASE [" + dc.Connection.Database + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE ;RESTORE DATABASE [" + dc.Connection.Database + "] FROM DISK = '" + backupPath + "' WITH  FILE = 1, NOUNLOAD, REPLACE, STATS = 10; ALTER DATABASE [" + dc.Connection.Database + "] SET MULTI_USER ";
            //dc.ExecuteCommand(stp4);
            //try { File.Delete(backupPath); }
            //catch { }
            //LogAction(Actions.Approve, "استرجاع نسخه احتياطية", dc);
            //UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);




        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvBackups_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            File.Delete(gvBackups.DataKeys[e.RowIndex]["FullName"].ToExpressString());
            this.FillBackups();
            LogAction(Actions.Delete, string.Empty, dc);
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private void FillBackups()
    {
        DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~\\DatabaseBackups"));

        var files = from file in dir.EnumerateFiles("*.zip") orderby file.CreationTime descending select new { FullName = file.FullName, Name = Path.GetFileName(file.FullName), Date = file.CreationTime };
        gvBackups.DataSource = files.CopyToDataTable();
        gvBackups.DataBind();
    }

    private void Encrypt(string FilePath)
    {
        string password = ">X2]5N;33&#627x-678c^|9)'sS367vV";

        FileStream openStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
        FileStream saveStream = new FileStream(Path.ChangeExtension(FilePath, ".zip"), FileMode.OpenOrCreate, FileAccess.Write);

        AesManaged cryptic = new AesManaged();

        cryptic.Key = ASCIIEncoding.ASCII.GetBytes(password);
        cryptic.IV = ASCIIEncoding.ASCII.GetBytes(password.ToCharArray(), 15, 16);

        CryptoStream crStream = new CryptoStream(saveStream,
           cryptic.CreateEncryptor(), CryptoStreamMode.Write);

        byte[] decryptedData = new byte[10485760];

        int byteRead = 0;
        do
        {
            byteRead = openStream.Read(decryptedData, 0, decryptedData.Length);
            crStream.Write(decryptedData, 0, byteRead);
        }
        while (byteRead > 0);


        crStream.Close();
        saveStream.Close();
        openStream.Close();
        try { File.Delete(FilePath); }
        catch { }

    }

    private void Decrypt(string FilePath)
    {

        string password = ">X2]5N;33&#627x-678c^|9)'sS367vV";

        FileStream openStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
        FileStream saveStream = new FileStream(Path.ChangeExtension(FilePath, ".bak"), FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write);

        AesManaged cryptic = new AesManaged();

        cryptic.Key = ASCIIEncoding.ASCII.GetBytes(password);
        cryptic.IV = ASCIIEncoding.ASCII.GetBytes(password.ToCharArray(), 15, 16);

        CryptoStream crStream = new CryptoStream(openStream,
           cryptic.CreateDecryptor(), CryptoStreamMode.Read);

        byte[] decryptedData = new byte[10485760];

        int byteRead = 0;
        do
        {
            byteRead = crStream.Read(decryptedData, 0, decryptedData.Length);
            saveStream.Write(decryptedData, 0, byteRead);
        }
        while (byteRead > 0);

        crStream.Close();
        saveStream.Close();
        openStream.Close();

    }
    protected void btnSaveNew_Click(object sender, EventArgs e)
    {
        try
        {

            string sourceFile = Server.MapPath("~\\DatabaseBackups\\" + OldName);
            System.IO.FileInfo fi = new System.IO.FileInfo(sourceFile);
            if (fi.Exists)
            {

                fi.MoveTo(Server.MapPath("~\\DatabaseBackups\\" + txtDateFrom.Text + ".zip"));

            }
            FillBackups();
        }
        catch (Exception ex)
        {

            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }

    }
    protected void BtnClearNew_Click(object sender, EventArgs e)
    {

    }
    protected void Unnamed_Click(object sender, EventArgs e)
    {
        mpeCreateNew.Show();
        OldName = System.IO.Path.GetFileName((sender as LinkButton).CommandArgument.ToExpressString()); ;
    }
    protected void close_popup_Click(object sender, EventArgs e)
    {

    }
    protected void btnValidation_Click(object sender, EventArgs e)
    {


        if (Membership.ValidateUser("Auditor", txtPassword1.Text))
        {
            try
            {
 
                string backupPath = SelectRowsName;
                this.Decrypt(backupPath);
                backupPath = Path.ChangeExtension(backupPath, ".bak");
                string stp4 = @"USE MASTER; ALTER DATABASE [" + dc.Connection.Database + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE ;RESTORE DATABASE [" + dc.Connection.Database + "] FROM DISK = '" + backupPath + "' WITH  FILE = 1, NOUNLOAD, REPLACE, STATS = 10; ALTER DATABASE [" + dc.Connection.Database + "] SET MULTI_USER ";
                dc.ExecuteCommand(stp4);
                try { File.Delete(backupPath); }
                catch { }
                LogAction(Actions.Approve, "استرجاع نسخه احتياطية", dc);
                UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
            }
            catch (Exception ex)
            {
                Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
            }

        }
        else
        {
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.Validations.invCred, string.Empty);
            mpeValidation.Show();
        }



    }
    protected void btnClear_Click(object sender, EventArgs e)
    {

    }
}