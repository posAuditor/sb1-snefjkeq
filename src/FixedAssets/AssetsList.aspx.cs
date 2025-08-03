using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
using System.Data;

public partial class FixedAssets_AssetsList : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Session

    private DataTable dtAssetsList
    {
        get
        {
            return (DataTable)Session["dtAssetsList" + this.WinID];
        }

        set
        {
            Session["dtAssetsList" + this.WinID] = value;
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
                gvAssetsList.Columns[10].Visible = this.MyContext.PageData.IsViewDoc;
                lnkadd.Visible = this.MyContext.PageData.IsAdd;
                this.LoadControls();
                this.FillAssetsList();
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
            this.FillAssetsList();
            txtPurchaseDateFromSrch.Focus();
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
            txtPurchaseDateFromSrch.Clear();
            txtPurchaseDateToSrch.Clear();
            ddlCurrency.SelectedIndex = 0;
            txtSerialsrch.Clear();
            txtStartWorkDateFrom.Clear();
            txtStartWorkDateTo.Clear();
            txtName.Clear();
            acCategory.Clear();
            ddlStatus.SelectedIndex = 0;
            txtUserRefNo.Clear();
            if (acBranch.Enabled) acBranch.Clear();
            acOppositeAccount.Clear();
            this.FilterAccounts(null, null);
            this.FillAssetsList();
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
            acOppositeAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + acBranch.Value + "," + (ddlCurrency.SelectedIndex == 0 ? string.Empty : ddlCurrency.SelectedValue) + ",,true";
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvAssetsList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvAssetsList.PageIndex = e.NewPageIndex;
            gvAssetsList.DataSource = this.dtAssetsList;
            gvAssetsList.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }



    #endregion

    #region Private Methods

    private void FillAssetsList()
    {
        lnkadd.NavigateUrl = PageLinks.Assets;

        int? Currency_ID = ddlCurrency.SelectedIndex == 0 ? (int?)null : ddlCurrency.SelectedValue.ToInt();
        byte? DocStatus_ID = ddlStatus.SelectedIndex == 0 ? (byte?)null : ddlStatus.SelectedValue.ToByte();
        this.dtAssetsList = dc.usp_Assets_Select(null, txtName.TrimmedText, acCategory.Value.ToNullableInt(), txtPurchaseDateFromSrch.Text.ToDate(), txtPurchaseDateToSrch.Text.ToDate(), txtStartWorkDateFrom.Text.ToDate(), txtStartWorkDateTo.Text.ToDate(), acOppositeAccount.Value.ToNullableInt(), DocStatus_ID, Currency_ID, acBranch.Value.ToNullableInt(), MyContext.CurrentCulture.ToByte(), txtSerialsrch.TrimmedText, txtUserRefNo.TrimmedText).CopyToDataTable();
        gvAssetsList.DataSource = this.dtAssetsList;
        gvAssetsList.DataBind();
    }

    private void LoadControls()
    {
        acCategory.ContextKey = string.Empty;
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

        foreach (DataControlField col in gvAssetsList.Columns)
        {
            if (col.ItemStyle.CssClass == "BranchCol") col.Visible = MyContext.Features.BranchesEnabled;
        }
    }

    #endregion

    protected void lnkDepAsset_Click(object sender, EventArgs e)
    {
        var id = (sender as LinkButton).CommandArgument.ToIntOrDefault();
        gvDepAssetInFuture.DataSource = dc.getAllMonthinFiscatYear(id).ToList();
        gvDepAssetInFuture.DataBind();
        mpeCreateNew.Show();
    }
    protected void lnkDA_Click(object sender, EventArgs e)
    {
        int Index = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
        var id = gvDepAssetInFuture.DataKeys[Index]["Asset_ID"].ToInt();
        var DateDep = gvDepAssetInFuture.DataKeys[Index]["Date"].ToDate();
        var count = dc.getAllMonthinFiscatYear(id).Count(x => x.Date < DateDep);
        if (count > 0)
        {
            UserMessages.Message("لا يمكن الاهلاك بسبب ان هناك شهور من قبل  لم تهلك");
            mpeCreateNew.Show();
            return;
        }
        else
        {
            dc.usp_AssetDep_Calc(id, DateDep);
            gvDepAssetInFuture.DataSource = dc.getAllMonthinFiscatYear(id).ToList();
            gvDepAssetInFuture.DataBind();
            mpeCreateNew.Show();
            UserMessages.Message(Resources.UserInfoMessages.OperationSuccess);
        }
        // 
    }
}