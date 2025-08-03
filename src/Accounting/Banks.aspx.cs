using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class Accounting_Banks : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtBanks
    {
        get
        {
            return (DataTable)Session["dtBanks" + this.WinID];
        }

        set
        {
            Session["dtBanks" + this.WinID] = value;
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

    #endregion

    #region Page events
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.CheckSecurity();
            if (!Page.IsPostBack)
            {
                this.LoadControls();
                this.Fill();
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
            this.Fill();
            txtNameSrch.Focus();
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
            txtNameSrch.Clear();
            ddlCurrencySrch.SelectedIndex = 0;
            txtAccountNumberSrch.Clear();
            if (acBranchSrch.Enabled) acBranchSrch.Clear();
            this.Fill();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvBanks_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvBanks.PageIndex = e.NewPageIndex;
            gvBanks.DataSource = this.dtBanks;
            gvBanks.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvBanks_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {

            DataRow dr = this.dtBanks.Select("ID=" + gvBanks.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            txtName.Text = dr["Name"].ToExpressString();
            txtAddress.Text = dr["Address"].ToExpressString();
            txtTelephone.Text = dr["Telephone"].ToExpressString();
            txtAccountNumber.Text = dr["AccountNumber"].ToExpressString();
            acBranch.Value = dr["Branch_ID"].ToExpressString();
            ddlCurrency.SelectedValue = dr["Currency_ID"].ToExpressString();
            acParentAccount.Value = dr["Parent_ID"].ToExpressString();

            if (dr["OppositeAccountID"].ToExpressString() != string.Empty) acOppsiteAccount.Value = dr["OppositeAccountID"].ToExpressString();
            if (dr["StartFromDate"].ToExpressString() != string.Empty) txtStartFrom.Text = dr["StartFromDate"].ToDate().Value.ToString("d/M/yyyy");
            if (dr["OpenBalance"].ToExpressString() != string.Empty) txtOpenBalance.Text = dr["OpenBalance"].ToDecimal().ToExpressString();
            if (dr["Ratio"].ToExpressString() != string.Empty) txtRatio.Text = dr["Ratio"].ToDecimal().ToExpressString();
            this.EditID = gvBanks.DataKeys[e.NewSelectedIndex]["ID"].ToInt();

            if (!dr["IsDeletable"].ToBoolean())
            {
                acBranch.Enabled = false;
                txtOpenBalance.Enabled = false;
                ddlCurrency.Enabled = false;
                txtStartFrom.Enabled = false;
            }

            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvBanks_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_Bank_Delete(gvBanks.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvBanks.DataKeys[e.RowIndex]["Name"].ToExpressString(), dc);
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
            txtOpenBalance.Enabled = true;
            ddlCurrency.Enabled = true;
            txtStartFrom.Enabled = true;
            this.ClearForm();
            acBranch.Enabled = (this.MyContext.UserProfile.Branch_ID == null);
            acBranch.Value = MyContext.UserProfile.Branch_ID.ToStringOrEmpty();
            this.EditID = 0;
            if (sender == null && MyContext.FastEntryEnabled)
            {
                mpeCreateNew.Show();
            }
            else
            {
                mpeCreateNew.Hide();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSaveNew_click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {
            int result = 0;
            if (txtStartFrom.Text.ToDate() > DateTime.Now.Date && txtOpenBalance.Text.ToDecimalOrDefault() > 0)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
                trans.Rollback();
                mpeCreateNew.Show();
                return;
            }

            if (this.EditID == 0) //insert
            {
                result = dc.usp_Bank_Insert(txtName.TrimmedText, acParentAccount.Value.ToInt(), acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimalOrDefault(), txtOpenBalance.Text.ToDecimalOrDefault(), txtStartFrom.Text.ToDate(), acOppsiteAccount.Value.ToInt(), txtAddress.Text, txtTelephone.TrimmedText, txtAccountNumber.TrimmedText);
            }
            else
            {
                result = dc.usp_Bank_Update(this.EditID, txtName.TrimmedText, acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimalOrDefault(), txtOpenBalance.Text.ToDecimalOrDefault(), txtStartFrom.Text.ToDate(), acOppsiteAccount.Value.ToInt(), txtAddress.Text, txtTelephone.TrimmedText, txtAccountNumber.TrimmedText, acParentAccount.Value.ToInt());
            }
            if (result == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                trans.Rollback();
                mpeCreateNew.Show();
                return;
            }
            if (result == -35)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.ParentAccountNoChange, string.Empty);
                trans.Rollback();
                mpeCreateNew.Show();
                return;
            }
            if (result == -30)
            {
                UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.InvalidParentBranch, string.Empty);
                trans.Rollback();
                mpeCreateNew.Show();
                return;
            }
            LogAction(this.EditID == 0 ? Actions.Add : Actions.Edit, txtName.TrimmedText, dc);
            this.Fill();
            this.ClosePopup_Click(null, null);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
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
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtStartFrom_TextChanged(object sender, EventArgs e)
    {
        try
        {
            var ratio = dc.fun_GetCurrentRatio(ddlCurrency.SelectedValue.ToInt(), txtStartFrom.Text.ToDate());
            txtRatio.Text = ratio == null ? string.Empty : ratio.ToExpressString();
            if (sender != null) this.FocusNextControl(sender);
            if (sender != null) mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtOpenBalance_TextChanged(object sender, EventArgs e)
    {
        try
        {
            acBranch.IsRequired = txtRatio.IsRequired = txtStartFrom.IsRequired = (txtOpenBalance.Text.ToDecimalOrDefault() != 0);
            this.FocusNextControl(sender);
            mpeCreateNew.Show();
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
            acParentAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + acBranch.Value + ",," + COA.Banks.ToInt().ToExpressString() + ",true";
            if (sender != null) mpeCreateNew.Show();
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private void ClearForm()
    {
        txtName.Clear();
        txtAddress.Clear();
        if (acParentAccount.Enabled) acParentAccount.Clear();
        if (acBranch.Enabled) acBranch.Clear();
        txtAccountNumber.Clear();
        txtTelephone.Clear();
        txtAddress.Clear();
        if (ddlCurrency.Enabled) ddlCurrency.SelectedIndex = 0;
        if (txtStartFrom.Enabled) txtStartFrom.Text = this.MyContext.FiscalYearStartDate.ToString("d/M/yyyy");
        if (txtOpenBalance.Enabled) txtOpenBalance.Clear();
        if (txtStartFrom.Enabled) txtRatio.Clear();
        if (txtStartFrom.Enabled) txtRatio.IsRequired = txtStartFrom.IsRequired = false;
        this.acBranch_SelectedIndexChanged(null, null);
        if (txtStartFrom.Enabled) this.txtStartFrom_TextChanged(null, null);
    }

    private void LoadControls()
    {
        acBranchSrch.ContextKey = acBranch.ContextKey = string.Empty;
        if (MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = acBranchSrch.Value = MyContext.UserProfile.Branch_ID.ToExpressString();
            acBranch.Enabled = acBranchSrch.Enabled = false;
        }
        var currencies = dc.usp_Currency_Select(false).ToList();

        ddlCurrency.DataSource = currencies;
        ddlCurrency.DataTextField = "Name";
        ddlCurrency.DataValueField = "ID";
        ddlCurrency.DataBind();

        ddlCurrencySrch.DataSource = currencies;
        ddlCurrencySrch.DataTextField = "Name";
        ddlCurrencySrch.DataValueField = "ID";
        ddlCurrencySrch.DataBind();
        ddlCurrencySrch.Items.Insert(0, new ListItem(Resources.Labels.Select, "-1"));

        acOppsiteAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",,," + COA.Capital.ToInt().ToExpressString() + ",true";
        acOppsiteAccount.Value = COA.Capital.ToInt().ToExpressString();
        this.acBranch_SelectedIndexChanged(null, null);
        txtStartFrom.Text = this.MyContext.FiscalYearStartDate.ToString("d/M/yyyy");
        this.txtStartFrom_TextChanged(null, null);
    }

    private void Fill()
    {
        int? Currency_ID = ddlCurrencySrch.SelectedIndex == 0 ? (int?)null : ddlCurrencySrch.SelectedValue.ToInt();
        this.dtBanks = dc.usp_Banks_Select(txtNameSrch.TrimmedText, txtAccountNumberSrch.TrimmedText, acBranchSrch.Value.ToNullableInt(), Currency_ID).CopyToDataTable();
        gvBanks.DataSource = this.dtBanks;
        gvBanks.DataBind();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        gvBanks.Columns[5].Visible = MyContext.PageData.IsEdit;
        gvBanks.Columns[6].Visible = MyContext.PageData.IsDelete;
        lnkAddNew.Visible = MyContext.PageData.IsAdd;
        mpeCreateNew.TargetControlID = lnkAddNew.Visible ? lnkAddNew.UniqueID : hfmpeCreateNew.UniqueID;
    }

    private void CustomPage()
    {
        acBranchSrch.Visible = acBranch.Visible = MyContext.Features.BranchesEnabled;
        foreach (DataControlField col in gvBanks.Columns)
        {
            if (col.ItemStyle.CssClass == "BranchCol") col.Visible = MyContext.Features.BranchesEnabled;
        }
    }

    #endregion
}