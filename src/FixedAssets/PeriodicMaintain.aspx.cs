using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class FixedAssets_PeriodicMaintain : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    protected string ImgStatus
    {
        get
        {
            string result = Page.ResolveClientUrl("~/images/");
            if (ViewState["ImgStatus"] == null) result += "new"; else result += ViewState["ImgStatus"].ToExpressString();
            result += this.MyContext.CurrentCulture == XPRESS.Common.ABCulture.Arabic ? "-ar" : string.Empty;
            return result;
        }

        set
        {
            ViewState["ImgStatus"] = value;
        }
    }

    private int PeriodicMaintenance_ID
    {
        get
        {
            if (ViewState["PeriodicMaintenance_ID"] == null) return 0;
            return (int)ViewState["PeriodicMaintenance_ID"];
        }

        set
        {
            ViewState["PeriodicMaintenance_ID"] = value;
        }
    }

    private bool EditMode
    {
        get
        {
            if (ViewState["EditMode"] == null) return false;
            return (bool)ViewState["EditMode"];
        }

        set
        {
            ViewState["EditMode"] = value;
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

    private string DocRandomString
    {
        get
        {
            if (ViewState["DocRandomString"] == null)
            {
                ViewState["DocRandomString"] = DateTime.Now.Ticks.ToExpressString() + "_" + Guid.NewGuid().ToExpressString();
            }
            return (string)ViewState["DocRandomString"];
        }

        set
        {
            ViewState["DocRandomString"] = value;
        }
    }

    #endregion

    #region PageEvents
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            this.SetEditMode();
            if (!Page.IsPostBack)
            {
                this.DocRandomString.ToString();
                this.CheckSecurity();
                this.LoadControls();
                if (EditMode) this.FillPM();
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

    protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            var ratio = dc.fun_GetCurrentRatio(ddlCurrency.SelectedValue.ToInt(), txtDate.Text.ToDate());
            if (ratio != null) txtRatio.Text = ratio.ToExpressString();
            if (sender != null) this.FocusNextControl(sender);
            this.FilterByBranchAndCurrency();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void acBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            this.FilterByBranchAndCurrency();
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnSave_Click(object sender, EventArgs e)
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

    protected void BtnApprove_Click(object sender, EventArgs e)
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            dc.usp_PeriodicMaintenance_Cancel(this.PeriodicMaintenance_ID, MyContext.UserProfile.Contact_ID);
            LogAction(Actions.Delete, txtSerial.Text, dc);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.PeriodicMaintenance + "?ID=" + this.PeriodicMaintenance_ID.ToExpressString(), PageLinks.PeriodicMaintenanceList, PageLinks.PeriodicMaintenance);
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
            txtExpenseName.Clear();
            txtPeriodicFollowUp.Clear();
            mpeCreateExpense.Hide();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnClearEX_click(object sender, EventArgs e)
    {
        try
        {
            txtExpenseName.Clear();
            txtPeriodicFollowUp.Clear();
            mpeCreateExpense.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSaveEX_click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {
            if (!acParentAsset.HasValue)
            {
                trans.Rollback();
                UserMessages.Message(null, Resources.UserInfoMessages.SelectAssetFirst, string.Empty);
                return;
            }
            int result = dc.usp_PeriodicMaintenanceExpenses_Insert(txtExpenseName.TrimmedText, acParentAsset.Value.ToInt(), txtPeriodicFollowUp.Text.ToIntOrDefault(), ddlPeriodType.SelectedValue.ToByte());
            if (result == -2)
            {
                trans.Rollback();
                UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                mpeCreateExpense.Show();
                return;
            }
            acExpenseName.Refresh();
            acExpenseName.Value = result.ToExpressString();
            this.ClosePopup_Click(null, null);
            trans.Commit();
            this.FocusNextControl(lnkAddNewExpense);
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private void FilterByBranchAndCurrency()
    {
        try
        {
            acOppositeAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + acBranch.Value + "," + ddlCurrency.SelectedValue + ",,true";
            acParentAsset.ContextKey = acBranch.Value + "," + ddlCurrency.SelectedValue;
            acExpenseName.ContextKey = ddlCurrency.SelectedValue;

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
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
        this.FilterByBranchAndCurrency();
        lblCreatedBy.Text = MyContext.UserProfile.EmployeeName;

    }

    private void SetEditMode()
    {
        if (Request["ID"] != null)
        {
            this.EditMode = true;
            this.PeriodicMaintenance_ID = Request["ID"].ToInt();
        }
    }

    private bool Save(bool IsApproving, System.Data.Common.DbTransaction trans)
    {

        if (txtDate.Text.ToDate() > DateTime.Now.Date)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
            trans.Rollback();
            return false;
        }

        string Serial = string.Empty;
        int Serial_ID = DocSerials.PeriodicMaintenance.ToInt();
        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        DateTime? ApproveDate = IsApproving ? DateTime.Now : (DateTime?)null;
        int? approvedBY_ID = IsApproving ? MyContext.UserProfile.Contact_ID : (int?)null;
        if (!this.EditMode)
        {

            this.PeriodicMaintenance_ID = dc.usp_PeriodicMaintenance_Insert(acParentAsset.Value.ToInt(), acExpenseName.Value.ToInt(), acOppositeAccount.Value.ToInt(), txtDate.Text.ToDate(), txtCost.Text.ToDecimal(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate, approvedBY_ID, acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimal(), this.DocRandomString, txtNotes.Text, txtUserRefNo.TrimmedText);
            if (this.PeriodicMaintenance_ID > 0)
            {
                if (IsApproving)
                {
                    if (!this.InsertOperations())
                    {
                        trans.Rollback();
                        return false;
                    }
                }
                LogAction(IsApproving ? Actions.Approve : Actions.Add, Serial, dc);
            }
        }
        else
        {
            int Result = dc.usp_PeriodicMaintenance_Update(this.PeriodicMaintenance_ID, acParentAsset.Value.ToInt(), acExpenseName.Value.ToInt(), acOppositeAccount.Value.ToInt(), txtDate.Text.ToDate(), txtCost.Text.ToDecimal(), Serial, Serial_ID, DocStatus_ID, ApproveDate, approvedBY_ID, acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimal(), txtNotes.Text, txtUserRefNo.TrimmedText);
            if (Result > 0)
            {
                if (IsApproving)
                {
                    if (!this.InsertOperations())
                    {
                        trans.Rollback();
                        return false;
                    }
                }
                LogAction(IsApproving ? Actions.Approve : Actions.Edit, txtSerial.Text, dc);
            }
        }
        Serial = Serial == string.Empty ? string.Empty : " (" + Serial + ") ";
        UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess + Serial, PageLinks.PeriodicMaintenance + "?ID=" + this.PeriodicMaintenance_ID.ToExpressString(), PageLinks.PeriodicMaintenanceList, PageLinks.PeriodicMaintenance);
        return true;
    }

    private bool InsertOperations()
    {
        int result = dc.usp_PeriodicMaintenance_Approve(this.PeriodicMaintenance_ID);
        return true;
    }

    private void FillPM()
    {
        var PM = dc.usp_PeriodicMaintenance_Select(this.PeriodicMaintenance_ID, null, null, null, null, null, string.Empty, null, null, null, 0, string.Empty).FirstOrDefault();
        ddlCurrency.SelectedValue = PM.Currency_ID.ToExpressString();
        txtRatio.Text = PM.Ratio.ToExpressString();
        acBranch.Value = PM.Branch_ID.ToStringOrEmpty();
        this.FilterByBranchAndCurrency();
        txtSerial.Text = PM.Serial;
        this.DocRandomString = PM.DocRandomString;
        lblCreatedBy.Text = PM.CreatedByName;
        lblApprovedBy.Text = PM.ApprovedBYName;
        if (PM.DocStatus_ID == 2) acParentAsset.ContextKey += ",true";
        acParentAsset.Value = PM.Asset_ID.ToExpressString();
        acExpenseName.Value = PM.Expense_ID.ToExpressString();
        txtCost.Text = PM.Amount.ToExpressString();
        txtDate.Text = PM.OperationDate.Value.ToString("d/M/yyyy");
        acOppositeAccount.Value = PM.OppositeAccount_ID.ToExpressString();
        txtUserRefNo.Text = PM.UserRefNo;
        txtNotes.Text = PM.Notes;

        this.ImgStatus = ((DocStatus)PM.DocStatus_ID).ToExpressString();
        btnCancel.Visible = (PM.DocStatus_ID == 1) && MyContext.PageData.IsDelete;
        btnApprove.Visible = (PM.DocStatus_ID == 1) && MyContext.PageData.IsApprove;
        btnSave.Visible = (PM.DocStatus_ID == 1) && ((MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode));


    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        btnSave.Visible = (MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode);
        btnApprove.Visible = MyContext.PageData.IsApprove;
        btnCancel.Visible = MyContext.PageData.IsDelete && EditMode;
    }

    private void CustomPage()
    {
        acBranch.IsRequired = acBranch.Visible = MyContext.Features.BranchesEnabled;
        
    }


    #endregion
}