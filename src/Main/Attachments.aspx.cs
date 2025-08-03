using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using XPRESS.Common;
using System.Data;
using System.IO;
using System.Threading;

public partial class Main_Attachments : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();
    bool render = true;

    private DataTable dtAttachments
    {
        get
        {
            return (DataTable)Session["dtAttachments" + this.WinID];
        }

        set
        {
            Session["dtAttachments" + this.WinID] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ifDownload.Attributes.Remove("src");
            fpFile.Attributes.Add("multiple", "multiple");
            ScriptManager.GetCurrent(this).RegisterPostBackControl(btnUpload);
            if (!Page.IsPostBack)
            {




                var context = dc.usp_MyContext_select(Request["DocumentPath"].ToExpressString(), Request["DocumentPathInfo"].ToExpressString(), new Guid(Membership.GetUser().ProviderUserKey.ToExpressString())).FirstOrDefault();
                if (!context.AllowAttach) Response.Redirect(PageLinks.Authorization, true);



                this.Fill();

                if (!context.AllowViewAttch) gvAttachments.Columns[2].Visible = context.AllowViewAttch;
                if (!context.AllowViewAttch) gvAttachments.Columns[3].Visible = context.AllowViewAttch;
                if (!context.AllowDeleteAttach) gvAttachments.Columns[4].Visible = context.AllowDeleteAttach;


                if (Request["guid"] != null && Request["filename"] != null) this.Download();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            UserMessages.Message(Resources.UserInfoMessages.OperationFailed);
        }
    }

    protected override void Render(HtmlTextWriter writer)
    {
        if (render) base.Render(writer);
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {


        try
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                string Guid = string.Empty;
                for (int index = 0; index < Request.Files.Count; index++)
                {
                    Guid = this.GenerateGuid();
                    HttpPostedFile f = Request.Files[index];
                    if (f.ContentLength <= 0) continue;
                    f.SaveAs(Server.MapPath("~/Uploads/Attachments/" + Guid));
                    // f.SaveAs(Server.MapPath("~/Uploads/Attachments/temp/" + Guid + "." + f.FileName.Split('.')[f.FileName.Split('.').Length - 1]));
                    dc.usp_Attachments_Insert(Request["DocumentURI"].ToExpressString(), f.FileName, Guid, (decimal?)f.ContentLength / (decimal?)1024.00);
                    LogAction(Actions.Add, f.FileName, dc);
                }
                this.Fill();
                if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                    UserMessages.Message(Resources.UserInfoMessages.OperationSuccess);

            }).Start();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            UserMessages.Message(Resources.UserInfoMessages.OperationFailed);
        }

        //try
        //{

        //    string Guid = string.Empty;
        //    for (int index = 0; index < Request.Files.Count; index++)
        //    {
        //        Guid = this.GenerateGuid();
        //        HttpPostedFile f = Request.Files[index];
        //        if (f.ContentLength <= 0) continue;
        //        f.SaveAs(Server.MapPath("~/Uploads/Attachments/" + Guid));
        //        dc.usp_Attachments_Insert(Request["DocumentURI"].ToExpressString(), f.FileName, Guid, (decimal?)f.ContentLength / (decimal?)1024.00);
        //        LogAction(Actions.Add, f.FileName, dc);
        //    }
        //    this.Fill();
        //    if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0) UserMessages.Message(Resources.UserInfoMessages.OperationSuccess);
        //}
        //catch (Exception ex)
        //{
        //    Logger.LogError(ex);
        //    UserMessages.Message(Resources.UserInfoMessages.OperationFailed);
        //}
    }

    protected void gvAttachments_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvAttachments.PageIndex = e.NewPageIndex;
            gvAttachments.DataSource = this.dtAttachments;
            gvAttachments.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            UserMessages.Message(Resources.UserInfoMessages.OperationFailed);
        }
    }

    protected void gvAttachments_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            ifDownload.Attributes.Add("src", Request.Url.ToExpressString() + "&guid=" + gvAttachments.DataKeys[e.NewSelectedIndex]["Guid"].ToExpressString() + "&filename=" + gvAttachments.DataKeys[e.NewSelectedIndex]["FileName"].ToExpressString());
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            UserMessages.Message(Resources.UserInfoMessages.OperationFailed);
        }
    }

    protected void gvAttachments_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            File.Delete(Server.MapPath("~/Uploads/Attachments/" + gvAttachments.DataKeys[e.RowIndex]["Guid"]));
            dc.usp_Attachments_Delete(gvAttachments.DataKeys[e.RowIndex]["ID"].ToInt());
            UserMessages.Message(Resources.UserInfoMessages.OperationSuccess);
            LogAction(Actions.Delete, gvAttachments.DataKeys[e.RowIndex]["FileName"].ToExpressString(), dc);
            this.Fill();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            UserMessages.Message(Resources.UserInfoMessages.OperationFailed);
        }
    }

    private void Fill()
    {
        this.dtAttachments = dc.usp_Attachments_Select(Request["DocumentURI"].ToExpressString()).CopyToDataTable();
        gvAttachments.DataSource = this.dtAttachments;
        gvAttachments.DataBind();




    }

    private string GenerateGuid()
    {
        try
        {
            string guid = string.Empty;
            do
            {
                guid = Guid.NewGuid().ToExpressString();
            } while (File.Exists(Server.MapPath("~/Uploads/Attachments/" + guid)));
            return guid;
        }
        catch
        {
            throw;
        }
    }

    private void Download()
    {
        render = false;
        Response.Clear();
        Response.Buffer = true;
        Response.HeaderEncoding = System.Text.Encoding.GetEncoding("windows-1256");
        Response.AddHeader("Content-Disposition", "attachment; filename=\"" + Request["FileName"] + "\"");
        Response.ContentType = "application/octet-stream";
        Response.TransmitFile(Server.MapPath("~/Uploads/Attachments/" + Request["Guid"]));
    }


    public string GetUrl(string fileName, string guid)
    {
        System.IO.File.Copy(Server.MapPath("~/Uploads/Attachments/" + guid), Server.MapPath("~/Uploads/Attachments/temp/" + guid + "." + fileName.Split('.')[fileName.Split('.').Length - 1]));
        return "javascript:window.open('" + "/Uploads/Attachments/temp/" + guid + "." + fileName.Split('.')[fileName.Split('.').Length - 1] + "');";

    }
}