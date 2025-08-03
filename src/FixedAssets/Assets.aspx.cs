using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class FixedAssets_Assets : UICulturePage
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

    private int Asset_ID
    {
        get
        {
            if (ViewState["Asset_ID"] == null) return 0;
            return (int)ViewState["Asset_ID"];
        }

        set
        {
            ViewState["Asset_ID"] = value;
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
                if (EditMode) this.FillAsset();
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
            var ratio = dc.fun_GetCurrentRatio(ddlCurrency.SelectedValue.ToInt(), txtPurchaseDate.Text.ToDate());
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

    protected void acCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            var category = dc.usp_AssetCategories_Select(string.Empty).Where(x => x.ID == acAssetsCategories.Value.ToNullableInt()).FirstOrDefault();
            if (category != null)
            {
                if (category.DepMethod == 0)
                {
                    txtScrapValue.Clear();
                    txtProductionAge.Clear();
                    txtDepRate.Clear();
                    txtDepValue.Clear();
                }
                txtScrapValue.IsRequired = txtScrapValue.Enabled = (category.DepMethod != 0);
                txtProductionAge.IsRequired = txtProductionAge.Enabled = (category.DepMethod != 0);
                txtDepRate.IsRequired = (category.DepMethod != 0);
                txtDepValue.IsRequired = (category.DepMethod != 0);
            }
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
            dc.usp_Assets_Cancel(this.Asset_ID, MyContext.UserProfile.Contact_ID);
            LogAction(Actions.Delete, txtSerial.Text, dc);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.Assets + "?ID=" + this.Asset_ID.ToExpressString(), PageLinks.AssetsList, PageLinks.Assets);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void CalculateDepValue(object sender, EventArgs e)
    {
        try
        {
            txtDepRate.Clear();
            txtDepValue.Clear();
            if (sender != null) this.FocusNextControl(sender);

            if (txtProductionAge.Text.ToIntOrDefault() > 0)
            {
                txtDepRate.Text = (100.0000 / (txtProductionAge.Text.ToIntOrDefault() / 12.000)).ToString("0.####");
            }
            else
            {
                return;
            }
            if (txtStartWorkDate.Text.ToDate() == null) return;

            if (txtStartWorkDate.Text.ToDate() >= MyContext.FiscalYearStartDate && txtStartWorkDate.Text.ToDate() <= MyContext.FiscalYearEndDate)
            {
                txtDepValue.Text = "0";
            }
            else
            {
                int MonthDiff = this.MonthDifference(txtStartWorkDate.Text.ToDate().Value, txtDepDate.Text.ToDate().Value);
                if (MonthDiff > 0)
                {
                    txtDepValue.Text = ((100.0000m / txtProductionAge.Text.ToIntOrDefault()) * (txtCost.Text.ToDecimalOrDefault() - txtScrapValue.Text.ToDecimalOrDefault()) * 0.01m * MonthDiff).ToString("0.####");
                }
            }
            if (txtDepValue.Text.ToDecimalOrDefault() > txtCost.Text.ToDecimalOrDefault()) txtDepValue.Text = (txtCost.Text.ToDecimalOrDefault() - txtScrapValue.Text.ToDecimalOrDefault()).ToString("0.####");
            if (txtDepRate.Text.ToDecimalOrDefault() > 100) txtDepRate.Text = "100";


        }
        catch (Exception ex)
        {
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
        acAssetsCategories.ContextKey = string.Empty;
        txtDepDate.Text = MyContext.FiscalYearStartDate.AddDays(-1).ToString("d/M/yyyy");
    }

    private void SetEditMode()
    {
        if (Request["ID"] != null)
        {
            this.EditMode = true;
            this.Asset_ID = Request["ID"].ToInt();
        }
    }

    private bool Save(bool IsApproving, System.Data.Common.DbTransaction trans)
    {
        if (ddlCurrency.SelectedValue.ToInt() == dc.DefaultCurrancy().Value && txtRatio.Text.ToDecimalOrDefault() != 1)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.DefCurrRatio, string.Empty);
            trans.Rollback();
            return false;
        }

        if (txtPurchaseDate.Text.ToDate() > DateTime.Now.Date)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
            trans.Rollback();
            return false;
        }

        if (txtPurchaseDate.Text.ToDate() > txtStartWorkDate.Text.ToDate())
        {
            UserMessages.Message(null, Resources.UserInfoMessages.PurchaseDateBiggerThanWorkDate, string.Empty);
            trans.Rollback();
            return false;
        }

        if (txtScrapValue.Text.ToDecimalOrDefault() > txtCost.Text.ToDecimalOrDefault())
        {
            UserMessages.Message(null, Resources.UserInfoMessages.ScrapValueBiggerThanValue, string.Empty);
            trans.Rollback();
            return false;
        }

        string Serial = string.Empty;
        int Serial_ID = DocSerials.Asset.ToInt();
        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        DateTime? ApproveDate = IsApproving ? DateTime.Now : (DateTime?)null;
        int? approvedBY_ID = IsApproving ? MyContext.UserProfile.Contact_ID : (int?)null;
        if (!this.EditMode)
        {

            this.Asset_ID = dc.usp_Assets_Insert(txtName.TrimmedText, acAssetsCategories.Value.ToInt(), txtPurchaseDate.Text.ToDate(), txtStartWorkDate.Text.ToDate(), txtCost.Text.ToDecimal(), txtProductionAge.Text.ToIntOrDefault(), txtScrapValue.Text.ToDecimalOrDefault(), txtDepValue.Text.ToDecimalOrDefault(), txtDepRate.Text.ToDecimalOrDefault(), acOppositeAccount.Value.ToInt(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate, approvedBY_ID, acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimal(), this.DocRandomString, null, txtNotes.Text, txtUserRefNo.TrimmedText);
            if (this.Asset_ID == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                trans.Rollback();
                return false;
            }

            if (this.Asset_ID > 0)
            {

                var asst = dc.Assets.Where(x => x.ID == this.Asset_ID).FirstOrDefault();
                asst.DepAutomatic = chkNotDepAuto.Checked;
                dc.SubmitChanges();


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
            int Result = dc.usp_Assets_Update(this.Asset_ID, txtName.TrimmedText, acAssetsCategories.Value.ToInt(), txtPurchaseDate.Text.ToDate(), txtStartWorkDate.Text.ToDate(), txtCost.Text.ToDecimal(), txtProductionAge.Text.ToIntOrDefault(), txtScrapValue.Text.ToDecimalOrDefault(), txtDepValue.Text.ToDecimalOrDefault(), txtDepRate.Text.ToDecimalOrDefault(), acOppositeAccount.Value.ToInt(), Serial, Serial_ID, DocStatus_ID, ApproveDate, approvedBY_ID, acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimal(), txtNotes.Text, txtUserRefNo.TrimmedText);
            if (Result == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                trans.Rollback();
                return false;
            }
            if (Result > 0)
            {

                var asst = dc.Assets.Where(x => x.ID == this.Asset_ID).FirstOrDefault();
                asst.DepAutomatic = chkNotDepAuto.Checked;
                dc.SubmitChanges();


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
        UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess + Serial, PageLinks.Assets + "?ID=" + this.Asset_ID.ToExpressString(), PageLinks.AssetsList, PageLinks.Assets);
        return true;
    }

    private bool InsertOperations()
    {
        int result = dc.usp_Assets_Approve(this.Asset_ID);
        if (result == -2)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
            return false;
        }
        int ss = dc.usp_AssetDep_Calc(this.Asset_ID, txtDepDate.Text.ToDate());
        return true;
    }

    private void FillAsset()
    {
        var Asset = dc.usp_Assets_Select(this.Asset_ID, string.Empty, null, null, null, null, null, null, null, null, null, 0, string.Empty, string.Empty).FirstOrDefault();
        ddlCurrency.SelectedValue = Asset.Currency_ID.ToExpressString();
        txtRatio.Text = Asset.Ratio.ToExpressString();
        acBranch.Value = Asset.Branch_ID.ToStringOrEmpty();
        this.FilterByBranchAndCurrency();
        txtSerial.Text = Asset.Serial;
        this.DocRandomString = Asset.DocRandomString;
        lblCreatedBy.Text = Asset.CreatedByName;
        lblApprovedBy.Text = Asset.ApprovedBYName;
        acAssetsCategories.Value = Asset.Category_ID.ToExpressString();
        txtName.Text = Asset.Name;
        txtCost.Text = Asset.Price.ToExpressString();
        txtPurchaseDate.Text = Asset.PurchaseDate.Value.ToString("d/M/yyyy");
        acOppositeAccount.Value = Asset.OppositeAccount_ID.ToExpressString();
        txtScrapValue.Text = Asset.ScrapValue.ToExpressString();
        txtProductionAge.Text = Asset.ProductionAge.ToExpressString();
        txtStartWorkDate.Text = Asset.StartWorkDate.Value.ToString("d/M/yyyy");
        txtDepRate.Text = Asset.DepreciationRate.ToExpressString();
        txtDepValue.Text = Asset.DepValue.ToExpressString();
        txtUserRefNo.Text = Asset.UserRefNo;
        txtNotes.Text = Asset.Notes;
        this.acCategory_SelectedIndexChanged(null, null);

        this.ImgStatus = ((DocStatus)Asset.DocStatus_ID).ToExpressString();
        btnCancel.Visible = (Asset.DocStatus_ID == 1) && MyContext.PageData.IsDelete;
        btnApprove.Visible = (Asset.DocStatus_ID == 1) && MyContext.PageData.IsApprove;
        btnCancelApprove.Visible = (Asset.DocStatus_ID == 2) && MyContext.PageData.IsNotApprove;
        btnSave.Visible = (Asset.DocStatus_ID == 1) && ((MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode));

        var asst = dc.Assets.Where(x => x.ID == this.Asset_ID).FirstOrDefault();
        chkNotDepAuto.Checked = asst.DepAutomatic.ToBooleanOrDefault();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        btnSave.Visible = (MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode);
        btnApprove.Visible = MyContext.PageData.IsApprove; 
        btnCancelApprove.Visible =  !btnApprove.Visible && MyContext.PageData.IsNotApprove;
        btnCancel.Visible = MyContext.PageData.IsDelete && EditMode;
    }

    private int MonthDifference(DateTime StartDate, DateTime EndDate)
    {
        return ((EndDate.Month - StartDate.Month) + 12 * (EndDate.Year - StartDate.Year)) + 1;
    }

    private void CustomPage()
    {
        acBranch.IsRequired = acBranch.Visible = MyContext.Features.BranchesEnabled;
        
    }


    #endregion
    protected void btnCancelApprove_Click(object sender, EventArgs e)
    {
        try
        {
            dc.ups_CancelApprove_Assets(this.Asset_ID);

            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.Assets + "?ID=" + this.Asset_ID.ToExpressString(), PageLinks.AssetsList, PageLinks.Assets);
            LogAction(Actions.NotApprove, txtSerial.Text, dc);
        }
        catch (Exception ex)
        {
            // trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
}