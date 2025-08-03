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

public partial class Checks_ChecksList : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Session

    private DataTable dtChecksList
    {
        get
        {
            return (DataTable)Session["dtChecksList" + this.WinID];
        }

        set
        {
            Session["dtChecksList" + this.WinID] = value;
        }
    }

    #endregion

    #region ViewState

    private int Check_ID
    {
        get
        {
            if (ViewState["Check_ID"] == null) return 0;
            return (int)ViewState["Check_ID"];
        }

        set
        {
            ViewState["Check_ID"] = value;
        }
    }

    private string IssueDate
    {
        get
        {
            if (ViewState["IssueDate"] == null) return string.Empty;
            return (string)ViewState["IssueDate"];
        }

        set
        {
            ViewState["IssueDate"] = value;
        }
    }

    private int CollectOrRefuse
    {
        get
        {
            if (ViewState["CollectOrRefuse"] == null) return 0;
            return (int)ViewState["CollectOrRefuse"];
        }

        set
        {
            ViewState["CollectOrRefuse"] = value;
        }
    }

    private string ConfirmationMessage
    {
        get
        {
            if (ViewState["ConfirmationMessage"] == null)
            {
                ViewState["ConfirmationMessage"] = string.Empty;
            }
            return (string)ViewState["ConfirmationMessage"];
        }

        set
        {
            ViewState["ConfirmationMessage"] = value;
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
                gvChecksList.Columns[12].Visible = this.MyContext.PageData.IsViewDoc;
                gvChecksList.Columns[13].Visible = this.MyContext.PageData.IsApprove;
                gvChecksList.Columns[14].Visible = this.MyContext.PageData.IsApprove;
                gvChecksList.Columns[15].Visible = this.MyContext.PageData.IsPrint;
                lnkadd.Visible = this.MyContext.PageData.IsAdd;
                this.LoadControls();
                this.FillChecksList();
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

    protected void txtCollectingExpenses_TextChanged(object sender, EventArgs e)
    {
        try
        {
            acExpensesAccount.IsRequired = (txtCollectingExpenses.Text.ToDecimalOrDefault() > 0);
            if (sender != null) this.FocusNextControl(sender);
            mpeCollectRefuse.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSearch_click(object sender, EventArgs e)
    {
        try
        {
            this.FillChecksList();
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
            txtMatDateFrom.Clear();
            txtMatDateTo.Clear();
            ddlCheckStatus.SelectedIndex = 0;
            ddlCurrency.SelectedIndex = 0;
            txtSerialsrch.Clear();
            txtUserRefNo.Clear();
            txtBillNo.Clear();
            ddlStatus.SelectedIndex = 0;
            if (acBranch.Enabled) acBranch.Clear();
            acIssueBank.Clear();
            acDepositAccountSrch.Clear();
            this.FilterAccounts(null, null);
            this.FillChecksList();
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

            string Currency_ID = ddlCurrency.SelectedIndex == 0 ? string.Empty : ddlCurrency.SelectedValue;
            acDepositAccountSrch.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + acBranch.Value + "," + Currency_ID;
            acIssueBank.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + acBranch.Value + "," + Currency_ID + "," + COA.Banks.ToInt().ToExpressString() + ",false";
            acAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + acBranch.Value + "," + Currency_ID + ",,true";
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvChecksList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvChecksList.PageIndex = e.NewPageIndex;
            gvChecksList.DataSource = this.dtChecksList;
            gvChecksList.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvChecksList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            int result = dc.usp_Customers_Delete(gvChecksList.DataKeys[e.NewSelectedIndex]["ID"].ToInt());

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void lnkRefuse_Click(object sender, EventArgs e)
    {
        try
        {
            this.CollectOrRefuse = 1;
            acDepositAccount.Visible = (Request.PathInfo == "/CheckIn");
            int Index = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
            this.Check_ID = gvChecksList.DataKeys[Index]["ID"].ToInt();
            int Currency_ID = gvChecksList.DataKeys[Index]["Currency_ID"].ToInt();
            string Branch_ID = gvChecksList.DataKeys[Index]["Branch_ID"].ToStringOrEmpty();
            this.IssueDate = gvChecksList.DataKeys[Index]["IssueDate"].ToDate().Value.ToString("d/M/yyyy");
            lblCheckSerial.Text = gvChecksList.DataKeys[Index]["Serial"].ToExpressString();
            acExpensesAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + Branch_ID + "," + Currency_ID + ",,true";
            acDepositAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + Branch_ID + "," + Currency_ID;
            acExpensesAccount.Value = COA.BankExpensesAndBenefits.ToInt().ToExpressString();
            btnOkCollect.Text = Resources.Labels.Refuse;
            mpeCollectRefuse.Show();

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
            this.CollectOrRefuse = 0;
            acDepositAccount.Visible = (Request.PathInfo == "/CheckIn");
            int Index = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
            this.Check_ID = gvChecksList.DataKeys[Index]["ID"].ToInt();
            int Currency_ID = gvChecksList.DataKeys[Index]["Currency_ID"].ToInt();
            string Branch_ID = gvChecksList.DataKeys[Index]["Branch_ID"].ToStringOrEmpty();
            this.IssueDate = gvChecksList.DataKeys[Index]["IssueDate"].ToDate().Value.ToString("d/M/yyyy");
            lblCheckSerial.Text = gvChecksList.DataKeys[Index]["Serial"].ToExpressString();
            acExpensesAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + Branch_ID + "," + Currency_ID + ",,true";
            acDepositAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + Branch_ID + "," + Currency_ID;
            acExpensesAccount.Value = COA.BankExpensesAndBenefits.ToInt().ToExpressString();
            btnOkCollect.Text = Resources.Labels.CollectPay;
            mpeCollectRefuse.Show();
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
            acDepositAccount.Clear();
            txtCollectingExpenses.Clear();
            txtCollectingOrRefuseDate.Clear();
            acExpensesAccount.Clear();
            mpeCollectRefuse.Hide();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnOkCollect_click(object sender, EventArgs e)
    {

        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {
            this.ConfirmationMessage = string.Empty;
            if (txtCollectingOrRefuseDate.Text.ToDate() < this.IssueDate.ToDate())
            {
                string Message = this.CollectOrRefuse == 0 ? Resources.UserInfoMessages.CollectDateLessIssus : Resources.UserInfoMessages.RefuseDateLessIssus;
                UserMessages.Message(null, Message, string.Empty);
                trans.Rollback();
                mpeCollectRefuse.Show();
                return;
            }
            if (txtCollectingOrRefuseDate.Text.ToDate() > DateTime.Now.Date)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
                trans.Rollback();
                mpeCollectRefuse.Show();
                return;
            }

            if (this.CollectOrRefuse == 0)
            {
                int result = dc.usp_PayCheck(this.Check_ID, txtCollectingOrRefuseDate.Text.ToDate(), acDepositAccount.Value.ToNullableInt(), txtCollectingExpenses.Text.ToDecimalOrDefault(), acExpensesAccount.Value.ToNullableInt());

                if (sender != null && result == -5)
                {
                    this.ConfirmationMessage += "<br> \u2022 " + Resources.UserInfoMessages.BankBalanceNotEnough;
                }
            }
            else
            {
                dc.usp_RefuseCheck(this.Check_ID, txtCollectingOrRefuseDate.Text.ToDate(), acDepositAccount.Value.ToNullableInt(), txtCollectingExpenses.Text.ToDecimalOrDefault(), acExpensesAccount.Value.ToNullableInt());
            }
            if (this.ConfirmationMessage != string.Empty)
            {
                ltConfirmationMessage.Text = this.ConfirmationMessage;
                mpeConfirm.Show();
                trans.Rollback();
                return;
            }
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
            trans.Commit();
            this.ClosePopup_Click(null, null);
            this.FillChecksList();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            int Index = ((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex;
            int Payment_ID = gvChecksList.DataKeys[Index]["ID"].ToInt();
            int? Branch_ID = gvChecksList.DataKeys[Index]["Branch_ID"].ToNullableInt();
            ReportDocument doc = new ReportDocument();
            doc.Load(Server.MapPath("~\\Reports\\Check_Print.rpt"));
            doc.SetParameterValue("@Check_ID", Payment_ID);
            Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(Branch_ID, "Check"), false);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnYes_click(object sender, EventArgs e)
    {
        try
        {
            this.btnOkCollect_click(null, null);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private void FillChecksList()
    {

        byte EntryType = 0;
        switch (Request.PathInfo)
        {
            case "/CheckIn":
                EntryType = ChecksTypes.ReceivedCheck.ToByte();
                acAccount.LabelText = Resources.Labels.Drawer;
                gvChecksList.Columns[5].HeaderText = Resources.Labels.Drawer;
                break;

            case "/CheckOut":
                EntryType = ChecksTypes.IssuedCheck.ToByte();
                acAccount.LabelText = Resources.Labels.Beneficiary;
                gvChecksList.Columns[5].HeaderText = Resources.Labels.Beneficiary;
                gvChecksList.Columns[6].Visible = false;
                break;

        }

        lnkadd.NavigateUrl = PageLinks.Checks + Request.PathInfo;
        int? Currency_ID = ddlCurrency.SelectedIndex == 0 ? (int?)null : ddlCurrency.SelectedValue.ToInt();
        byte? DocStatus_ID = ddlStatus.SelectedIndex == 0 ? (byte?)null : ddlStatus.SelectedValue.ToByte();
        byte? CheckStatus = ddlCheckStatus.SelectedIndex == 0 ? (byte?)null : ddlCheckStatus.SelectedValue.ToByte();
        this.dtChecksList = dc.usp_Checks_Select(acBranch.Value.ToNullableInt(), Currency_ID, txtSerialsrch.TrimmedText, acIssueBank.Value.ToNullableInt(), acDepositAccountSrch.Value.ToNullableInt(), acAccount.Value.ToNullableInt(), txtDateFromSrch.Text.ToDate(), txtDateToSrch.Text.ToDate(), txtMatDateFrom.Text.ToDate(), txtMatDateTo.Text.ToDate(), txtUserRefNo.Text, DocStatus_ID, txtBillNo.TrimmedText, CheckStatus, MyContext.CurrentCulture.ToByte(), EntryType).CopyToDataTable();
        gvChecksList.DataSource = this.dtChecksList;
        gvChecksList.DataBind();
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
        acDepositAccountSrch.Visible = acDepositAccount.Visible = (Request.PathInfo == "/CheckIn");
    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;


        foreach (DataControlField col in gvChecksList.Columns)
        {
            if (col.ItemStyle.CssClass == "BranchCol") col.Visible = MyContext.Features.BranchesEnabled;
        }
    }

    #endregion

}