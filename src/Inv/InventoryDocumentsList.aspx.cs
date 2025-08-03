using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class Inv_InventoryDocumentsList : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Session

    private DataTable dtInventoryDocumentsList
    {
        get
        {
            return (DataTable)Session["dtInventoryDocumentsList" + this.WinID];
        }

        set
        {
            Session["dtInventoryDocumentsList" + this.WinID] = value;
        }
    }

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.gvInventoryDocumentsList.FormatNumber = MyContext.FormatNumber;
            if (!Page.IsPostBack)
            {
                if (!MyContext.PageData.IsViewList) Response.Redirect(PageLinks.Authorization, true);
                gvInventoryDocumentsList.Columns[5].Visible = this.MyContext.PageData.IsViewDoc;
                gvInventoryDocumentsList.Columns[6].Visible = this.MyContext.PageData.IsPrint;
                lnkadd.Visible = this.MyContext.PageData.IsAdd;
                this.LoadControls();
                this.FillInventoryDocumentsList();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        this.CustomPage();
    }

    #endregion

    #region Control Events

    protected void btnSearch_click(object sender, EventArgs e)
    {
        try
        {
            this.FillInventoryDocumentsList();
            ddlStatus.Focus();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnClearSrch_Click(object sender, EventArgs e)
    {
        try
        {
            txtDateFromSrch.Clear();
            txtDateToSrch.Clear();
            txtSerialsrch.Clear();
            txtUserRefNo.Clear();
            ddlStatus.SelectedIndex = 0;
            if (acBranch.Enabled) acBranch.Clear();
            this.FillInventoryDocumentsList();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvInventoryDocumentsList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvInventoryDocumentsList.PageIndex = e.NewPageIndex;
            gvInventoryDocumentsList.DataSource = this.dtInventoryDocumentsList;
            gvInventoryDocumentsList.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvInventoryDocumentsList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            int result = dc.usp_Customers_Delete(gvInventoryDocumentsList.DataKeys[e.NewSelectedIndex]["ID"].ToInt());

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            string SaveName = Request.PathInfo == "/InvCorr" ? "InventoryCorrection" : "InventoryTransfer";
            int Index = ((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex;
            int InvDoc_ID = gvInventoryDocumentsList.DataKeys[Index]["ID"].ToInt();
            int? Branch_ID = gvInventoryDocumentsList.DataKeys[Index]["Branch_ID"].ToNullableInt();
            ReportDocument doc = new ReportDocument();
            doc.Load(Server.MapPath("~\\Reports\\" + SaveName + "_Print.rpt"));
            doc.SetParameterValue("@Doc_ID", InvDoc_ID);
            Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(Branch_ID, SaveName), false);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private void FillInventoryDocumentsList()
    {
        byte EntryType = Request.PathInfo == "/InvCorr" ? (byte)0 : (byte)1;
        lnkadd.NavigateUrl = Request.PathInfo == "/InvCorr" ? PageLinks.InventoryCorrection : PageLinks.InventoryTransfer;
        gvInventoryDocumentsList.Columns[2].Visible = Request.PathInfo == "/InvCorr";
        byte? DocStatus_ID = ddlStatus.SelectedIndex == 0 ? (byte?)null : ddlStatus.SelectedValue.ToByte();
        this.dtInventoryDocumentsList = dc.usp_InventoryDocument_Select(acBranch.Value.ToNullableInt(), txtSerialsrch.TrimmedText, txtDateFromSrch.Text.ToDate(), txtDateToSrch.Text.ToDate(), txtUserRefNo.Text, DocStatus_ID, MyContext.CurrentCulture.ToByte(), EntryType).CopyToDataTable();
        gvInventoryDocumentsList.DataSource = this.dtInventoryDocumentsList;
        gvInventoryDocumentsList.DataBind();
    }

    private void LoadControls()
    {
        acBranch.ContextKey = string.Empty;
        if (this.MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = this.MyContext.UserProfile.Branch_ID.ToExpressString();
            acBranch.Enabled = false;
        }

    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;
        foreach (DataControlField col in gvInventoryDocumentsList.Columns)
        {
            if (col.ItemStyle.CssClass == "BranchCol") col.Visible = MyContext.Features.BranchesEnabled;
        }
    }

    #endregion

}