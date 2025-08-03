using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class FixedAssets_AssetsSelling : UICulturePage
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

    private int AssetSell_ID
    {
        get
        {
            if (ViewState["AssetSell_ID"] == null) return 0;
            return (int)ViewState["AssetSell_ID"];
        }

        set
        {
            ViewState["AssetSell_ID"] = value;
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
                if (EditMode) this.FillAssetSell();
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
            var ratio = dc.fun_GetCurrentRatio(ddlCurrency.SelectedValue.ToInt(), txtSellDate.Text.ToDate());
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
            dc.usp_AssetsSelling_Cancel(this.AssetSell_ID, MyContext.UserProfile.Contact_ID);
            LogAction(Actions.Delete, txtSerial.Text, dc);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.AssetsSelling + "?ID=" + this.AssetSell_ID.ToExpressString(), PageLinks.AssetsSellingList, PageLinks.AssetsSelling);
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
            this.AssetSell_ID = Request["ID"].ToInt();
        }
    }

    private bool Save(bool IsApproving, System.Data.Common.DbTransaction trans)
    {

        if (txtSellDate.Text.ToDate() > DateTime.Now.Date)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
            trans.Rollback();
            return false;
        }

        if (txtSellDate.Text.ToDate() < txtEndUseDate.Text.ToDate())
        {
            UserMessages.Message(null, Resources.UserInfoMessages.SelldateLessEnduseDate, string.Empty);
            trans.Rollback();
            return false;
        }

        string Serial = string.Empty;
        int Serial_ID = DocSerials.AssetSell.ToInt();
        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        DateTime? ApproveDate = IsApproving ? DateTime.Now : (DateTime?)null;
        int? approvedBY_ID = IsApproving ? MyContext.UserProfile.Contact_ID : (int?)null;
        if (!this.EditMode)
        {
            this.AssetSell_ID = dc.usp_AssetsSelling_Insert(acParentAsset.Value.ToInt(), acOppositeAccount.Value.ToInt(), txtSellDate.Text.ToDate(), txtEndUseDate.Text.ToDate(), txtCost.Text.ToDecimal(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate, approvedBY_ID, acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimal(), this.DocRandomString, txtNotes.Text, txtUserRefNo.TrimmedText);
            if (this.AssetSell_ID > 0)
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
            int Result = dc.usp_AssetsSelling_Update(this.AssetSell_ID, acParentAsset.Value.ToInt(), acOppositeAccount.Value.ToInt(), txtSellDate.Text.ToDate(), txtEndUseDate.Text.ToDate(), txtCost.Text.ToDecimal(), Serial, Serial_ID, DocStatus_ID, ApproveDate, approvedBY_ID, acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimal(), txtNotes.Text, txtUserRefNo.TrimmedText);
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
        UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess + Serial, PageLinks.AssetsSelling + "?ID=" + this.AssetSell_ID.ToExpressString(), PageLinks.AssetsSellingList, PageLinks.AssetsSelling);
        return true;
    }

    private bool InsertOperations()
    {
        int result = 0;
        var AssetWithMaintains = from data in dc.Assets where data.DocStatus_ID == DocStatus.Approved.ToByte() && (data.ID == acParentAsset.Value.ToInt() || data.ParentAsset_ID == acParentAsset.Value.ToInt()) select data;
        foreach (var asset in AssetWithMaintains)
        {
            if (txtEndUseDate.Text.ToDate() < asset.StartWorkDate)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.SelldateLessEnduseDate, string.Empty);
                return false;
            }
            result = dc.usp_AssetDep_Calc(asset.ID, txtEndUseDate.Text.ToDate());
            if (result == -10)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.EndUseDateAfterLastDepDAte, string.Empty);
                return false;
            }
        }

        dc.usp_AssetSelling_Approve(this.AssetSell_ID);
        return true;
    }

    private void FillAssetSell()
    {
        var AssetSell = dc.usp_AssetsSelling_Select(this.AssetSell_ID, null, null, null, null, string.Empty, null, null, null, 0, string.Empty).FirstOrDefault();
        ddlCurrency.SelectedValue = AssetSell.Currency_ID.ToExpressString();
        txtRatio.Text = AssetSell.Ratio.ToExpressString();
        acBranch.Value = AssetSell.Branch_ID.ToStringOrEmpty();
        this.FilterByBranchAndCurrency();
        txtSerial.Text = AssetSell.Serial;
        this.DocRandomString = AssetSell.DocRandomString;
        lblCreatedBy.Text = AssetSell.CreatedByName;
        lblApprovedBy.Text = AssetSell.ApprovedBYName;
        if (AssetSell.DocStatus_ID == 2) acParentAsset.ContextKey += ",true";
        acParentAsset.Value = AssetSell.Asset_ID.ToExpressString();
        txtEndUseDate.Text = AssetSell.EndUseDate.Value.ToString("d/M/yyyy");
        txtCost.Text = AssetSell.Amount.ToExpressString();
        txtSellDate.Text = AssetSell.SellDate.Value.ToString("d/M/yyyy");
        acOppositeAccount.Value = AssetSell.OppositeAccount_ID.ToExpressString();
        txtUserRefNo.Text = AssetSell.UserRefNo;
        txtNotes.Text = AssetSell.Notes;

        this.ImgStatus = ((DocStatus)AssetSell.DocStatus_ID).ToExpressString();
        btnCancel.Visible = (AssetSell.DocStatus_ID == 1) && MyContext.PageData.IsDelete;
        btnApprove.Visible = (AssetSell.DocStatus_ID == 1) && MyContext.PageData.IsApprove;
        btnSave.Visible = (AssetSell.DocStatus_ID == 1) && ((MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode));


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