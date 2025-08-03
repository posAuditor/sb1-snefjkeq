using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

public partial class DocCredit_Expenses : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtDocCreditExpenses
    {
        get
        {
            return (DataTable)Session["dtDocCreditExpenses" + this.WinID];
        }

        set
        {
            Session["dtDocCreditExpenses" + this.WinID] = value;
        }
    }

    private int Receipt_ID
    {
        get
        {
            if (ViewState["Receipt_ID"] == null) return 0;
            return (int)ViewState["Receipt_ID"];
        }

        set
        {
            ViewState["Receipt_ID"] = value;
        }
    }

    private int EditID
    {
        get
        {
            if (ViewState["EditID"] == null) return 0;
            return (int)ViewState["EditID"];
        }

        set
        {
            ViewState["EditID"] = value;
        }
    }

    private int? Branch_ID
    {
        get
        {
            return (int?)ViewState["Branch_ID"];
        }

        set
        {
            ViewState["Branch_ID"] = value;
        }
    }

    private int? BankChartOfAccount_ID
    {
        get
        {
            return (int?)ViewState["BankChartOfAccount_ID"];
        }

        set
        {
            ViewState["BankChartOfAccount_ID"] = value;
        }
    }

    private int? Currency_ID
    {
        get
        {
            return (int?)ViewState["Currency_ID"];
        }

        set
        {
            ViewState["Currency_ID"] = value;
        }
    }

    private bool PopupOpen
    {
        get
        {
            if (ViewState["PopupOpen"] == null) return false;
            return (bool)ViewState["PopupOpen"];
        }

        set
        {
            ViewState["PopupOpen"] = value;
        }
    }

    #endregion

    #region Page events
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                this.CheckSecurity();
                this.LoadControls();
                this.Fill();
            }
            if (this.PopupOpen) mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Control Events

    protected void lnkAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            this.PopupOpen = true;
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            var ratio = dc.fun_GetCurrentRatio(ddlCurrency.SelectedValue.ToInt(), txtDate.Text.ToDate());
            txtRatio.Text = ratio == null ? string.Empty : ratio.ToExpressString();
            if (sender != null) this.FocusNextControl(sender);
            this.FilterByBranchAndCurrency();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void ucNewExpenseName_NewAttributeCreated(string AttName, int AttID)
    {
        try
        {
            acExpenseName.Refresh();
            acExpenseName.Value = AttID.ToExpressString();
            txtAmount.Focus();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvExpenses_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvExpenses.PageIndex = e.NewPageIndex;
            gvExpenses.DataSource = this.dtDocCreditExpenses;
            gvExpenses.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvExpenses_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {

            DataRow dr = this.dtDocCreditExpenses.Select("ID=" + gvExpenses.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            ddlExpenseType.SelectedValue = dr["ExpenseType"].ToExpressString();
            this.ddlExpenseType_SelectedIndexChanged(null, null);
            acExpenseName.Value = dr["ExpenseName_ID"].ToExpressString();
            txtAmount.Text = dr["Amount"].ToExpressString();
            ddlCurrency.SelectedValue = dr["Currency_ID"].ToExpressString();
            txtRatio.Text = dr["Ratio"].ToExpressString();
            txtDate.Text = dr["Operationdate"].ToDate().Value.ToString("d/M/yyyy");
            this.FilterByBranchAndCurrency();
            acOppsiteAccount.Value = dr["OppositeAccount_ID"].ToExpressString();
            txtNotes.Text = dr["Notes"].ToExpressString();
            this.EditID = gvExpenses.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            this.PopupOpen = true;
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvExpenses_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_DocumentryCreditExpenses_Delete(gvExpenses.DataKeys[e.RowIndex]["ID"].ToInt());
            LogAction(Actions.Delete, gvExpenses.DataKeys[e.RowIndex]["ExpenseName"].ToExpressString(), dc);
            this.Fill();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
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
            this.ClearForm();
            this.EditID = 0;
            if (sender == null && MyContext.FastEntryEnabled)
            {
                mpeCreateNew.Show();
                this.PopupOpen = true;
            }
            else
            {
                mpeCreateNew.Hide();
                this.PopupOpen = false;
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSave_click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {
            if (this.Save(false, trans))
                trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnApprove_click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {
            if (this.Save(true, trans))
                trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnClearNew_click(object sender, EventArgs e)
    {
        try
        {
            this.ClearForm();
            this.PopupOpen = true;
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void ddlExpenseType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlCurrency.Enabled = acOppsiteAccount.Enabled = (ddlExpenseType.SelectedIndex == 1);
            if (ddlExpenseType.SelectedIndex == 0)
            {
                ddlCurrency.SelectedValue = this.Currency_ID.ToExpressString();
                this.ddlCurrency_SelectedIndexChanged(null, null);
                acOppsiteAccount.Value = this.BankChartOfAccount_ID.ToExpressString();
            }
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private bool Save(bool IsApproving, System.Data.Common.DbTransaction trans)
    {
        int result = 0;

        if (txtDate.Text.ToDate() > DateTime.Now.Date)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
            trans.Rollback();
            return false;
        }
        if (ddlCurrency.SelectedValue.ToInt() == dc.DefaultCurrancy().Value && txtRatio.Text.ToDecimalOrDefault() != 1)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.DefCurrRatio, string.Empty);
            trans.Rollback();
            return false;
        }
        if (IsApproving && ddlExpenseType.SelectedIndex == 0 && (txtAmount.Text.ToDecimalOrDefault() > dc.fun_GetAccountBalanceInForeign(this.BankChartOfAccount_ID, null, this.Branch_ID).Value))
        {
            UserMessages.Message(null, Resources.UserInfoMessages.BankBalanceNotEnough, string.Empty);
            trans.Rollback();
            return false;
        }
        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        if (this.EditID == 0) //insert
        {
            result = dc.usp_DocumentryCreditExpenses_Insert(this.Receipt_ID, ddlExpenseType.SelectedValue.ToByte(), acExpenseName.Value.ToInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimal(), txtAmount.Text.ToDecimal(), acOppsiteAccount.Value.ToInt(), txtDate.Text.ToDate(), txtNotes.Text, DocStatus_ID,0,null,null,null);
            LogAction(IsApproving ? Actions.Approve : Actions.Add, acExpenseName.Text, dc);
        }
        else
        {
            result = dc.usp_DocumentryCreditExpenses_Update(this.EditID, ddlExpenseType.SelectedValue.ToByte(), acExpenseName.Value.ToInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimal(), txtAmount.Text.ToDecimal(), acOppsiteAccount.Value.ToInt(), txtDate.Text.ToDate(), txtNotes.Text, DocStatus_ID,0,null,null,null);
            LogAction(IsApproving ? Actions.Approve : Actions.Edit, acExpenseName.Text, dc);
        }

        this.Fill();
        this.ClosePopup_Click(null, null);
        UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        return true;
    }

    private void ClearForm()
    {
        acExpenseName.Clear();
        acOppsiteAccount.Clear();
        txtDate.Clear();
        txtAmount.Clear();
        txtNotes.Clear();
        ddlExpenseType.SelectedIndex = 0;
        this.ddlExpenseType_SelectedIndexChanged(null, null);
    }

    private void LoadControls()
    {
        this.Receipt_ID = Request["Receipt_ID"].ToInt();
        var Receipt = dc.usp_Receipt_SelectByID(this.Receipt_ID).FirstOrDefault();
        this.Branch_ID = Receipt.Branch_ID;
        this.Currency_ID = Receipt.Currency_ID;
        var DocCredit = dc.usp_DocumentryCredit_SelectByReceipt_ID(this.Receipt_ID).FirstOrDefault();
        this.BankChartOfAccount_ID = DocCredit.BankChartOfAccount_ID;

        lnkviewdocCredit.NavigateUrl = PageLinks.DocumentryCredit + "?ID=" + this.Receipt_ID.ToExpressString() + "&ViewInPopUpMode=1";
        ddlCurrency.DataSource = dc.usp_Currency_Select(false);
        ddlCurrency.DataTextField = "Name";
        ddlCurrency.DataValueField = "ID";
        ddlCurrency.DataBind();

        acExpenseName.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.DocumentryCreditExpenses.ToInt().ToExpressString(); 
        this.ddlExpenseType_SelectedIndexChanged(null, null);
        this.FilterByBranchAndCurrency();

        if (DocCredit.IsClosed.Value)
        {
            gvExpenses.Columns[8].Visible = false;
            gvExpenses.Columns[9].Visible = false;
            lnkAddNew.Visible = false;
        }
    }

    private void Fill()
    {
        this.dtDocCreditExpenses = dc.usp_DocumentryCreditExpenses_Select(this.Receipt_ID).CopyToDataTable();
        gvExpenses.DataSource = this.dtDocCreditExpenses;
        gvExpenses.DataBind();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        gvExpenses.Columns[8].Visible = MyContext.PageData.IsEdit;
        gvExpenses.Columns[9].Visible = MyContext.PageData.IsDelete;
        lnkAddNew.Visible = MyContext.PageData.IsAdd;
        btnApprove.Visible = MyContext.PageData.IsApprove;
    }

    private void FilterByBranchAndCurrency()
    {
        try
        {
            acOppsiteAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + this.Branch_ID.ToStringOrEmpty() + "," + ddlCurrency.SelectedValue + ",,true";
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion
}