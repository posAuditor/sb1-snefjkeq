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

public partial class Payments_PaymentsGroupList : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Session

    private DataTable dtPaymentsList
    {
        get
        {
            return (DataTable)Session["dtPaymentsList" + this.WinID];
        }

        set
        {
            Session["dtPaymentsList" + this.WinID] = value;
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
                if (!MyContext.PageData.IsViewList) Response.Redirect(PageLinks.Authorization, true);
                gvPaymentsList.Columns[9].Visible = this.MyContext.PageData.IsViewDoc;
                gvPaymentsList.Columns[10].Visible = this.MyContext.PageData.IsPrint;
                lnkadd.Visible = this.MyContext.PageData.IsAdd;
                this.LoadControls();
                this.FillPaymentsList();
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
            this.FillPaymentsList();
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
            txtBillNo.Clear();
            if (acBranch.Enabled) acBranch.Clear();
            acDebitAccount.Clear();
            acCreditAccount.Clear();
            this.FilterAccounts(null, null);
            this.FillPaymentsList();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void FilterAccounts(object sender, EventArgs e)
    {
        try
        {
            string DebitParent_ID = string.Empty;
            string CreditParent_ID = string.Empty;

            switch (Request.PathInfo)
            {
                case "/CashIn":
                    DebitParent_ID = COA.CashOnHand.ToInt().ToExpressString();
                    txtBillNo.Visible = false;
                    break;

                case "/CashInCustomer":
                    CreditParent_ID = COA.Customers.ToInt().ToExpressString();
                    DebitParent_ID = COA.CashOnHand.ToInt().ToExpressString();
                    break;

                case "/CashOut":
                    CreditParent_ID = COA.CashOnHand.ToInt().ToExpressString();
                    txtBillNo.Visible = false;
                    break;

                case "/CashOutVendor":
                    CreditParent_ID = COA.CashOnHand.ToInt().ToExpressString();
                    DebitParent_ID = COA.Vendors.ToInt().ToExpressString();
                    break;

                case "/BankDeposit":
                    DebitParent_ID = COA.Banks.ToInt().ToExpressString();
                    txtBillNo.Visible = false;
                    break;
                case "/BankDepositCustomer":
                    CreditParent_ID = COA.Customers.ToInt().ToExpressString();
                    DebitParent_ID = COA.Banks.ToInt().ToExpressString();
                    break;
                case "/BankWithdraw":
                    CreditParent_ID = COA.Banks.ToInt().ToExpressString();
                    txtBillNo.Visible = false;
                    break;

                case "/BankWithdrawVendor":
                    CreditParent_ID = COA.Banks.ToInt().ToExpressString();
                    DebitParent_ID = COA.Vendors.ToInt().ToExpressString();
                    break;
            }
            string Currency_ID = ddlCurrency.SelectedIndex == 0 ? string.Empty : ddlCurrency.SelectedValue;
            acCreditAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + acBranch.Value + "," + Currency_ID + "," + CreditParent_ID + ",true";
            acDebitAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + acBranch.Value + "," + Currency_ID + "," + DebitParent_ID + ",true";
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvPaymentsList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvPaymentsList.PageIndex = e.NewPageIndex;
            gvPaymentsList.DataSource = this.dtPaymentsList;
            gvPaymentsList.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvPaymentsList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            int result = dc.usp_Customers_Delete(gvPaymentsList.DataKeys[e.NewSelectedIndex]["ID"].ToInt());

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
            int Payment_ID = gvPaymentsList.DataKeys[Index]["ID"].ToInt();
            int? Branch_ID = gvPaymentsList.DataKeys[Index]["Branch_ID"].ToNullableInt();
            ReportDocument doc = new ReportDocument();
            doc.Load(Server.MapPath("~\\Reports\\Payment_Print.rpt"));
            doc.SetParameterValue("@Payments_ID", Payment_ID);
            Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(Branch_ID, "Payment"), false);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private void FillPaymentsList()
    {

        byte EntryType = 0;

        EntryType = PaymentsTypes.CashInCustomerGroup.ToByte();



        lnkadd.NavigateUrl = PageLinks.PaymentsGroup + Request.PathInfo;
        int? Currency_ID = ddlCurrency.SelectedIndex == 0 ? (int?)null : ddlCurrency.SelectedValue.ToInt();
        byte? DocStatus_ID = ddlStatus.SelectedIndex == 0 ? (byte?)null : ddlStatus.SelectedValue.ToByte();

       

        this.dtPaymentsList = dc.usp_Payments_Select(acBranch.Value.ToNullableInt(), Currency_ID, txtSerialsrch.TrimmedText, acCreditAccount.Value.ToNullableInt(), acDebitAccount.Value.ToNullableInt(), txtDateFromSrch.Text.ToDate(), txtDateToSrch.Text.ToDate(), txtUserRefNo.Text, DocStatus_ID, txtBillNo.TrimmedText, MyContext.CurrentCulture.ToByte(), EntryType, (MyContext.UserProfile.HasPermissionShow == false ? (int?)null : MyContext.UserProfile.Contact_ID)).CopyToDataTable();
        gvPaymentsList.DataSource = this.dtPaymentsList;
        gvPaymentsList.DataBind();
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
        this.FilterAccounts(null, null);
    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;


        foreach (DataControlField col in gvPaymentsList.Columns)
        {
            if (col.ItemStyle.CssClass == "BranchCol") col.Visible = MyContext.Features.BranchesEnabled;
        }
    }

    #endregion

}