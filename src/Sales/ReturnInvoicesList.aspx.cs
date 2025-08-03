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

public partial class Sales_ReturnInvoicesList : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Session

    private DataTable dtReturnInvoicesList
    {
        get
        {
            return (DataTable)Session["dtReturnInvoicesList" + this.WinID];
        }

        set
        {
            Session["dtReturnInvoicesList" + this.WinID] = value;
        }
    }

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.gvReturnInvoicesList.FormatNumber = MyContext.FormatNumber;
            if (!Page.IsPostBack)
            {
                if (!MyContext.PageData.IsViewList) Response.Redirect(PageLinks.Authorization, true);
                gvReturnInvoicesList.Columns[8].Visible = this.MyContext.PageData.IsViewDoc;
                gvReturnInvoicesList.Columns[9].Visible = this.MyContext.PageData.IsPrint;
                lnkadd.Visible = this.MyContext.PageData.IsAdd;
                this.LoadControls();
                this.FillReturnInvoicesList();
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
            this.FillReturnInvoicesList();
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
            acCustomerName.Clear();
            this.FilterCustomers(null, null);
            this.FillReturnInvoicesList();
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

    protected void gvReturnInvoicesList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvReturnInvoicesList.PageIndex = e.NewPageIndex;
            gvReturnInvoicesList.DataSource = this.dtReturnInvoicesList;
            gvReturnInvoicesList.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvReturnInvoicesList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            int result = dc.usp_Customers_Delete(gvReturnInvoicesList.DataKeys[e.NewSelectedIndex]["ID"].ToInt());

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
            int ID = gvReturnInvoicesList.DataKeys[Index]["ID"].ToInt();
            Response.Redirect("~/Report_Dev/PrintReturninvoice.aspx?Invoice_ID=" + ID + "&IsMaterla=1", false);
            //int? Branch_ID = gvReturnInvoicesList.DataKeys[Index]["Branch_ID"].ToNullableInt();
            //ReportDocument doc = new ReportDocument();

            //var returnInvoice = dc.usp_ReturnInvoice_SelectByID(ID);
            //var totalAmount = returnInvoice.Select(x => x.GrossTotalAmount).FirstOrDefault();


            //doc.Load(Server.MapPath("~\\Reports\\ReturnInvoice_Print ssb.rpt"));
            //doc.SetParameterValue("@ReturnInvoice_ID", ID);
            //doc.SetParameterValue("BaseDir", HttpContext.Current.Server.MapPath("~\\Uploads\\"));
            //doc.SetParameterValue("ByEmp", ((UICulturePage)HttpContext.Current.Handler).MyContext.UserProfile.EmployeeName);
            //doc.SetParameterValue("@Branch_ID", acBranch.Value.ToNullableInt());
            //doc.SetParameterValue("@TheNo1", totalAmount.ToDecimalOrDefault().ToString(), "Tafkit.rpt");
            //doc.SetParameterValue("@TheNo2", "0", "Tafkit.rpt");


            //Response.Redirect(PageLinks.Print + Request.PathInfo + "?File=" + doc.ExportToPDF(Branch_ID, "ReturnInvoice"), false);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private void FillReturnInvoicesList()
    {
        lnkadd.NavigateUrl = PageLinks.ReturnInvoice;
        int? Currency_ID = ddlCurrency.SelectedIndex == 0 ? (int?)null : ddlCurrency.SelectedValue.ToInt();
        byte? DocStatus_ID = ddlStatus.SelectedIndex == 0 ? (byte?)null : ddlStatus.SelectedValue.ToByte();


        this.dtReturnInvoicesList = dc.usp_ReturnInvoice_Select(acBranch.Value.ToNullableInt(), Currency_ID, txtSerialsrch.TrimmedText, acCustomerName.Value.ToNullableInt(), txtDateFromSrch.Text.ToDate(), txtDateToSrch.Text.ToDate(), txtUserRefNo.Text, DocStatus_ID, MyContext.CurrentCulture.ToByte(), (MyContext.UserProfile.HasPermissionShow == false ? (int?)null : MyContext.UserProfile.Contact_ID)).CopyToDataTable();
        gvReturnInvoicesList.DataSource = this.dtReturnInvoicesList;
        gvReturnInvoicesList.DataBind();
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

        foreach (DataControlField col in gvReturnInvoicesList.Columns)
        {
            if (col.ItemStyle.CssClass == "BranchCol") col.Visible = MyContext.Features.BranchesEnabled;
        }
    }

    #endregion

}