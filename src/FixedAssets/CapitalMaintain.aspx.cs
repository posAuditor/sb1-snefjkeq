using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class FixedAssets_CapitalMaintain : UICulturePage
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
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.CapitalMaintainence + "?ID=" + this.Asset_ID.ToExpressString(), PageLinks.CapitalMaintainenceList, PageLinks.CapitalMaintainence);
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
            acParentAsset.ContextKey = acBranch.Value + ",";
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
            this.Asset_ID = Request["ID"].ToInt();
        }
    }

    private bool Save(bool IsApproving, System.Data.Common.DbTransaction trans)
    {

        if (txtPurchaseDate.Text.ToDate() > DateTime.Now.Date)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
            trans.Rollback();
            return false;
        }

        var ParentAsset = dc.usp_Assets_Select(acParentAsset.Value.ToInt(), string.Empty, null, null, null, null, null, null, null, null, null, 0, string.Empty, string.Empty).First();


        if (txtPurchaseDate.Text.ToDate() > txtStartWorkDate.Text.ToDate())
        {
            UserMessages.Message(null, Resources.UserInfoMessages.PurchaseDateBiggerThanWorkDate, string.Empty);
            trans.Rollback();
            return false;
        }


        string Serial = string.Empty;
        int Serial_ID = DocSerials.CapitalMaintainence.ToInt();
        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        DateTime? ApproveDate = IsApproving ? DateTime.Now : (DateTime?)null;
        int? approvedBY_ID = IsApproving ? MyContext.UserProfile.Contact_ID : (int?)null;
        if (!this.EditMode)
        {

            this.Asset_ID = dc.usp_Assets_Insert(txtName.TrimmedText, ParentAsset.Category_ID, txtPurchaseDate.Text.ToDate(), txtStartWorkDate.Text.ToDate(), txtCost.Text.ToDecimal(), txtProductionAge.Text.ToInt(), 0, 0, ParentAsset.DepreciationRate, acOppositeAccount.Value.ToInt(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate, approvedBY_ID, acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimal(), this.DocRandomString, acParentAsset.Value.ToInt(), txtNotes.Text, txtUserRefNo.TrimmedText);
            if (this.Asset_ID == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                trans.Rollback();
                return false;
            }

            if (this.Asset_ID > 0)
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
            int Result = dc.usp_Assets_Update(this.Asset_ID, txtName.TrimmedText, ParentAsset.Category_ID, txtPurchaseDate.Text.ToDate(), txtStartWorkDate.Text.ToDate(), txtCost.Text.ToDecimal(), txtProductionAge.Text.ToInt(), 0, 0, ParentAsset.DepreciationRate, acOppositeAccount.Value.ToInt(), Serial, Serial_ID, DocStatus_ID, ApproveDate, approvedBY_ID, acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimal(), txtNotes.Text, txtUserRefNo.TrimmedText);
            if (Result == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                trans.Rollback();
                return false;
            }
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
        UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess + Serial, PageLinks.CapitalMaintainence + "?ID=" + this.Asset_ID.ToExpressString(), PageLinks.CapitalMaintainenceList, PageLinks.CapitalMaintainence);
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
        txtName.Text = Asset.Name;
        txtCost.Text = Asset.Price.ToExpressString();
        txtPurchaseDate.Text = Asset.PurchaseDate.Value.ToString("d/M/yyyy");
        txtStartWorkDate.Text = Asset.StartWorkDate.Value.ToString("d/M/yyyy");
        txtProductionAge.Text = Asset.ProductionAge.ToExpressString();
        acOppositeAccount.Value = Asset.OppositeAccount_ID.ToExpressString();
        if (Asset.DocStatus_ID == 2) acParentAsset.ContextKey += ",true";
        acParentAsset.Value = Asset.ParentAsset_ID.ToExpressString();
        txtUserRefNo.Text = Asset.UserRefNo;
        txtNotes.Text = Asset.Notes;

        this.ImgStatus = ((DocStatus)Asset.DocStatus_ID).ToExpressString();
        btnCancel.Visible = (Asset.DocStatus_ID == 1) && MyContext.PageData.IsDelete;
        btnApprove.Visible = (Asset.DocStatus_ID == 1) && MyContext.PageData.IsApprove;
        btnSave.Visible = (Asset.DocStatus_ID == 1) && ((MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode));


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