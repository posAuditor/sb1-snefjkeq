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

public partial class Purchases_ReturnReceiptsList : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Session

    private DataTable dtReturnReceiptsList
    {
        get
        {
            return (DataTable)Session["dtReturnReceiptsList" + this.WinID];
        }

        set
        {
            Session["dtReturnReceiptsList" + this.WinID] = value;
        }
    }

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.gvReceiptsList.FormatNumber = MyContext.FormatNumber;
            if (!Page.IsPostBack)
            {
                if (!MyContext.PageData.IsViewList) Response.Redirect(PageLinks.Authorization, true);
                gvReceiptsList.Columns[8].Visible = this.MyContext.PageData.IsViewDoc;
                gvReceiptsList.Columns[9].Visible = this.MyContext.PageData.IsPrint;
                lnkadd.Visible = this.MyContext.PageData.IsAdd;
                this.LoadControls();
                this.FillReturnReceiptsList();
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
            this.FillReturnReceiptsList();
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
            ddlCurrency.SelectedIndex = 0;
            txtSerialsrch.Clear();
            txtUserRefNo.Clear();
            ddlStatus.SelectedIndex = 0;
            if (acBranch.Enabled) acBranch.Clear();
            acVendorName.Clear();
            this.FilterVendors(null, null);
            this.FillReturnReceiptsList();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void FilterVendors(object sender, EventArgs e)
    {
        try
        {
            acVendorName.ContextKey = "V," + acBranch.Value + "," + (ddlCurrency.SelectedIndex == 0 ? string.Empty : ddlCurrency.SelectedValue) + ",";
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvReceiptsList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvReceiptsList.PageIndex = e.NewPageIndex;
            gvReceiptsList.DataSource = this.dtReturnReceiptsList;
            gvReceiptsList.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvReceiptsList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            int result = dc.usp_Customers_Delete(gvReceiptsList.DataKeys[e.NewSelectedIndex]["ID"].ToInt());

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
            int Index = ((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex;
            int ID = gvReceiptsList.DataKeys[Index]["ID"].ToInt();
            //int? Branch_ID = gvReceiptsList.DataKeys[Index]["Branch_ID"].ToNullableInt();
            //ReportDocument doc = new ReportDocument();
           
            //doc.Load(Server.MapPath("~\\Reports\\ReturnReceipt_Print ssb.rpt"));
            //doc.SetParameterValue("@ReturnReceipt_ID", ID);
            ////doc.SetParameterValue("@DocTableType_ID", DocumentsTableTypes.ReturnReceipt.ToInt(), "DocumentTaxes.rpt");
            ////doc.SetParameterValue("@Doc_ID", ID, "DocumentTaxes.rpt");
            ////doc.SetParameterValue("@IsReturn", true, "DocumentTaxes.rpt");
            //var receipt = dc.usp_ReturnReceipt_SelectByID(ID);
            //var totalAmount = receipt.Select(x => x.GrossTotalAmount).FirstOrDefault();



            ////doc.SetParameterValue("@TheNo1", totalAmount.ToDecimalOrDefault(), "Tafkit.rpt");
            //doc.SetParameterValue("@TheNo2", "0", "Tafkit.rpt");
            //Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(Branch_ID, "ReturnReceipt"), false);


            Response.Redirect("~/Report_Dev/PrintReturnReceiptDev.aspx?Invoice_ID=" + ID + "&IsMaterla=1", false);

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private void FillReturnReceiptsList()
    {
        lnkadd.NavigateUrl = PageLinks.ReturnReceipt;
        int? Currency_ID = ddlCurrency.SelectedIndex == 0 ? (int?)null : ddlCurrency.SelectedValue.ToInt();
        byte? DocStatus_ID = ddlStatus.SelectedIndex == 0 ? (byte?)null : ddlStatus.SelectedValue.ToByte();


        this.dtReturnReceiptsList = dc.usp_ReturnReceipt_Select(acBranch.Value.ToNullableInt(), Currency_ID, txtSerialsrch.TrimmedText, acVendorName.Value.ToNullableInt(), txtDateFromSrch.Text.ToDate(), txtDateToSrch.Text.ToDate(), txtUserRefNo.Text, DocStatus_ID, MyContext.CurrentCulture.ToByte(), (MyContext.UserProfile.HasPermissionShow == false ? (int?)null : MyContext.UserProfile.Contact_ID)).CopyToDataTable();
        gvReceiptsList.DataSource = this.dtReturnReceiptsList;
        gvReceiptsList.DataBind();
    }

    private void LoadControls()
    {
        acBranch.ContextKey = string.Empty;
        if (this.MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = this.MyContext.UserProfile.Branch_ID.ToExpressString();
            acBranch.Enabled = false;
        }
        ddlCurrency.DataSource = dc.usp_Currency_Select(false);
        ddlCurrency.DataTextField = "Name";
        ddlCurrency.DataValueField = "ID";
        ddlCurrency.DataBind();
        ddlCurrency.Items.Insert(0, new ListItem(Resources.Labels.Select, "-1"));
        this.FilterVendors(null, null);
    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;
        

        foreach (DataControlField col in gvReceiptsList.Columns)
        {
            if (col.ItemStyle.CssClass == "BranchCol") col.Visible = MyContext.Features.BranchesEnabled;
        }
    }

    #endregion

}