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

public partial class Sales_InvoicesPosList : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Session

    private DataTable dtInvoicesList
    {
        get
        {
            return (DataTable)Session["dtInvoicesList" + this.WinID];
        }

        set
        {
            Session["dtInvoicesList" + this.WinID] = value;
        }
    }

    #endregion

    #region ViewState

    private int Invoice_ID
    {
        get
        {
            if (ViewState["Invoice_ID"] == null) return 0;
            return (int)ViewState["Invoice_ID"];
        }

        set
        {
            ViewState["Invoice_ID"] = value;
        }
    }

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
              //  if (!MyContext.PageData.IsViewList) Response.Redirect(PageLinks.Authorization, true);
                //gvInvoicesList.Columns[8].Visible = this.MyContext.PageData.IsViewDoc;
                //gvInvoicesList.Columns[9].Visible = this.MyContext.PageData.IsPrint;
                btnPrintList.Visible = this.MyContext.PageData.IsPrint;
                //lnkadd.Visible = this.MyContext.PageData.IsAdd;
                this.LoadControls();
                this.FillInvoicesList();
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
            this.FillInvoicesList();
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
            ddlIsHasBill.SelectedIndex = 0;
            if (acBranch.Enabled) acBranch.Clear();
            acCustomerName.Clear();
            this.FilterCustomers(null, null);
            this.FillInvoicesList();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void FilterCustomers(object sender, EventArgs e)
    {
        try
        {
            acCustomerName.ContextKey = "C," + acBranch.Value + "," + (ddlCurrency.SelectedIndex == 0 ? string.Empty : ddlCurrency.SelectedValue) + ",";
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvInvoicesList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvInvoicesList.PageIndex = e.NewPageIndex;
            gvInvoicesList.DataSource = this.dtInvoicesList;
            gvInvoicesList.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvInvoicesList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            int result = dc.usp_Customers_Delete(gvInvoicesList.DataKeys[e.NewSelectedIndex]["ID"].ToInt());

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void lnkCollect_Click(object sender, EventArgs e)
    {
        try
        {
            int Index = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
            this.Invoice_ID = gvInvoicesList.DataKeys[Index]["ID"].ToInt();
            lblInvoiceSerial.Text = gvInvoicesList.DataKeys[Index]["Serial"].ToExpressString();
            lblCollected.Text = gvInvoicesList.DataKeys[Index]["CollectedAmount"].ToExpressString();
            lblGrossTotal.Text = gvInvoicesList.DataKeys[Index]["GrossTotalAmount"].ToExpressString();
            mpeCollect.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void ClosePopup_Click(object sender, EventArgs e)
    {
        try
        {
            mpeCollect.Hide();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnOkCollect_click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(ddlCollectWith.SelectedValue + "?RelatedDocTableType_ID=1&RelatedDoc_ID=" + Invoice_ID.ToExpressString(), false);
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
            string SaveName = Request.PathInfo == "/Invoice" ? "Invoice" : "SalesOrder";
            int Index = ((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex;
            this.Invoice_ID = gvInvoicesList.DataKeys[Index]["ID"].ToInt();
            int? Branch_ID = gvInvoicesList.DataKeys[Index]["Branch_ID"].ToNullableInt();

            ReportDocument doc = new ReportDocument();
            doc.Load(Server.MapPath("~\\Reports\\Invoice_PrintListPos.rpt"));
            doc.SetParameterValue("@Invoice_ID", this.Invoice_ID);

            //doc.SetParameterValue("BaseDir", HttpContext.Current.Server.MapPath("~\\Uploads\\"));
            //doc.SetParameterValue("ByEmp", ((UICulturePage)HttpContext.Current.Handler).MyContext.UserProfile.EmployeeName);
            //doc.SetParameterValue("@Branch_ID", acBranch.Value.ToNullableInt());
            
            
            
            //ReportDocument doc = new ReportDocument();
           
            //doc.Load(Server.MapPath("~\\Reports\\InvoicePrint.rpt"));
            //doc.SetParameterValue("@Invoice_ID", this.Invoice_ID);
            //doc.SetParameterValue("@DocTableType_ID", DocumentsTableTypes.Invoice.ToInt(), "DocumentTaxes.rpt");
            //doc.SetParameterValue("@Doc_ID", this.Invoice_ID, "DocumentTaxes.rpt");
            //doc.SetParameterValue("@IsReturn", false, "DocumentTaxes.rpt");
            //doc.SetParameterValue("BaseDir", HttpContext.Current.Server.MapPath("~\\Uploads\\"));
            //doc.SetParameterValue("ByEmp", ((UICulturePage)HttpContext.Current.Handler).MyContext.UserProfile.EmployeeName);
            // doc.SetParameterValue("@Branch_ID", acBranch.Value.ToNullableInt());
            Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "Invoice"), false);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnPrintList_Click(object sender, EventArgs e)
    {
        try
        {
            int? Currency_ID = ddlCurrency.SelectedIndex == 0 ? (int?)null : ddlCurrency.SelectedValue.ToInt();
            byte? DocStatus_ID = ddlStatus.SelectedIndex == 0 ? (byte?)null : ddlStatus.SelectedValue.ToByte();
            bool? HasInvoice = ddlIsHasBill.SelectedIndex == 0 ? (bool?)null : ddlIsHasBill.SelectedValue.ToBoolean();

            ReportDocument doc = new ReportDocument();
            doc.Load(Server.MapPath("~\\Reports\\InvoiceList_Print.rpt"));
            doc.SetParameterValue("@Branch_ID", acBranch.Value.ToNullableInt());
            doc.SetParameterValue("@Currency_ID", Currency_ID);
            doc.SetParameterValue("@Serial", txtSerialsrch.Text);
            doc.SetParameterValue("@Contact_ID", acCustomerName.Value.ToNullableInt());
            doc.SetParameterValue("@FromDate", txtDateFromSrch.Text.ToDate());
            doc.SetParameterValue("@ToDate", txtDateToSrch.Text.ToDate());
            doc.SetParameterValue("@UserRefNo", txtUserRefNo.Text);
            doc.SetParameterValue("@DocStatus_ID", DocStatus_ID);
            doc.SetParameterValue("@Culture", MyContext.CurrentCulture.ToByte());
            doc.SetParameterValue("@HasInvoice", HasInvoice);
            doc.SetParameterValue("@EntryType", 2);
            doc.SetParameterValue("BaseDir", HttpContext.Current.Server.MapPath("~\\Uploads\\"));
            doc.SetParameterValue("ByEmp", MyContext.UserProfile.EmployeeName);


            doc.SetParameterValue("@DocTableType_ID", DocumentsTableTypes.Invoice.ToInt(), "DocumentTaxes.rpt");
            //doc.SetParameterValue("@Doc_ID", this.Invoice_ID, "DocumentTaxes.rpt");
            doc.SetParameterValue("@IsReturn", false, "DocumentTaxes.rpt");
            Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(null, "InvoiceList"), false);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private void FillInvoicesList()
    {
        byte EntryType = Request.PathInfo == "/Invoice" ? (byte)2 : (byte)0;
       // lnkadd.NavigateUrl = Request.PathInfo == "/Invoice" ? PageLinks.Invoice : PageLinks.SalesOrder;
        //gvInvoicesList.Columns[10].Visible = Request.PathInfo != "/Invoice";
        //gvInvoicesList.Columns[11].Visible = Request.PathInfo == "/Invoice";
        //gvInvoicesList.Columns[12].Visible = Request.PathInfo == "/Invoice";
        divHasBill.Visible = Request.PathInfo != "/Invoice";
        btnPrintList.Visible = Request.PathInfo == "/Invoice" && MyContext.PageData.IsPrint;
        int? Currency_ID = ddlCurrency.SelectedIndex == 0 ? (int?)null : ddlCurrency.SelectedValue.ToInt();
        byte? DocStatus_ID = ddlStatus.SelectedIndex == 0 ? (byte?)null : ddlStatus.SelectedValue.ToByte();
        bool? HasInvoice = ddlIsHasBill.SelectedIndex == 0 ? (bool?)null : ddlIsHasBill.SelectedValue.ToBoolean();
        var lstInvoices =
            dc.usp_InvPos_Select(acBranch.Value.ToNullableInt(), Currency_ID, txtSerialsrch.TrimmedText,
                acCustomerName.Value.ToNullableInt(), txtDateFromSrch.Text.ToDate(), txtDateToSrch.Text.ToDate(),
                txtUserRefNo.Text, DocStatus_ID, MyContext.CurrentCulture.ToByte(), HasInvoice, EntryType).ToList();
        foreach (var item in lstInvoices)
        {
            item.PageName = item.PageName.Replace("Invoice","InvoiceForm");
            item.ToInvoicePage = item.ToInvoicePage.Replace("Invoice", "InvoiceForm");
        }
        this.dtInvoicesList = lstInvoices.CopyToDataTable();
        
        gvInvoicesList.DataSource = this.dtInvoicesList;
        gvInvoicesList.DataBind();
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
        this.FilterCustomers(null, null);
    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;

        foreach (DataControlField col in gvInvoicesList.Columns)
        {
            if (col.ItemStyle.CssClass == "BranchCol") col.Visible = MyContext.Features.BranchesEnabled;
        }
    }

    #endregion

}