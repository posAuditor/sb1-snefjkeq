using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class Accounting_Taxes : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtTaxes
    {
        get
        {
            return (DataTable)Session["dtTaxes" + this.WinID];
        }

        set
        {
            Session["dtTaxes" + this.WinID] = value;
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
            this.Fill();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvTaxes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvTaxes.PageIndex = e.NewPageIndex;
            gvTaxes.DataSource = this.dtTaxes;
            gvTaxes.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvTaxes_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            DataRow dr = this.dtTaxes.Select("ID=" + gvTaxes.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            txtName.Text = dr["Name"].ToExpressString();
            txtPercentageValue.Text = dr["PercentageValue"].ToExpressString();
            acPurchaseAccount.Value = dr["PurchaseAccountID"].ToExpressString();
            acSalesAccount.Value = dr["SalesAccountID"].ToExpressString();
            ddlOnSales.SelectedValue = dr["OnInvoiceType"].ToExpressString();
            ddlOnPurchases.SelectedValue = dr["OnReceiptType"].ToExpressString();
            ddlOnDocCredit.SelectedValue = dr["OnDocCreditType"].ToExpressString();
            this.EditID = gvTaxes.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvTaxes_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_Taxes_Delete(gvTaxes.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvTaxes.DataKeys[e.RowIndex]["Name"].ToExpressString(), dc);
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
        try
        {
            int result = 0;
            char? OnSalesType = ddlOnSales.SelectedIndex == 0 ? (char?)null : ddlOnSales.SelectedValue.ToCharArray()[0];
            char? OnPurchaseType = ddlOnPurchases.SelectedIndex == 0 ? (char?)null : ddlOnPurchases.SelectedValue.ToCharArray()[0];
            char? OnDocCreditType = ddlOnDocCredit.SelectedIndex == 0 ? (char?)null : ddlOnDocCredit.SelectedValue.ToCharArray()[0];

            if (this.EditID == 0) //insert
            {
                result = dc.usp_Taxes_Insert(txtName.TrimmedText, txtPercentageValue.Text.ToDecimal(), OnSalesType, OnPurchaseType, OnDocCreditType, acSalesAccount.Value.ToInt(), acPurchaseAccount.Value.ToInt());
            }
            else
            {
                result = dc.usp_Taxes_Update(this.EditID, txtName.TrimmedText, txtPercentageValue.Text.ToDecimal(), OnSalesType, OnPurchaseType, OnDocCreditType, acSalesAccount.Value.ToInt(), acPurchaseAccount.Value.ToInt());
            }
            if (result == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                mpeCreateNew.Show();
                return;
            }
            if (result == -31)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.BranchlessReq, string.Empty);
                mpeCreateNew.Show();
                return;
            }
            LogAction(this.EditID == 0 ? Actions.Add : Actions.Edit, txtName.TrimmedText, dc);
            this.Fill();
            this.ClosePopup_Click(null, null);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
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

    #endregion

    #region Private Methods

    private void ClearForm()
    {
        txtName.Clear();
        txtPercentageValue.Clear();
        ddlOnDocCredit.SelectedIndex = 0;
        ddlOnPurchases.SelectedIndex = 0;
        ddlOnSales.SelectedIndex = 0;
        //acPurchaseAccount.Clear();
        //acSalesAccount.Clear();
    }

    private void LoadControls()
    {
        acSalesAccount.ContextKey = acPurchaseAccount.ContextKey = MyContext.CurrentCulture.ToByte().ToExpressString();
        acSalesAccount.Value = acPurchaseAccount.Value = COA.OtherCreditAccounts.ToInt().ToExpressString();
        var taxAccount = this.dc.ChartOfAccounts.FirstOrDefault(r => r.CachedNumber == "2030303");
        if (taxAccount != null)
        {
            try
            {
                acSalesAccount.Value = taxAccount.ID.ToString();
                acPurchaseAccount.Value = taxAccount.ID.ToString();
                ddlOnPurchases.SelectedValue = "D";
                ddlOnSales.SelectedValue = "C";
            }
            catch  
            {

            }
        }        
    }

    private void Fill()
    {
        this.dtTaxes = dc.usp_Taxes_Select(null, txtNameSrch.TrimmedText).CopyToDataTable();
        gvTaxes.DataSource = this.dtTaxes;
        gvTaxes.DataBind();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        gvTaxes.Columns[7].Visible = MyContext.PageData.IsEdit;
        gvTaxes.Columns[8].Visible = MyContext.PageData.IsDelete;
        lnkAddNew.Visible = MyContext.PageData.IsAdd;
        mpeCreateNew.TargetControlID = lnkAddNew.Visible ? lnkAddNew.UniqueID : hfmpeCreateNew.UniqueID;
    }

    #endregion

    protected void lnkActive_Click(object sender, EventArgs e)
    {

        var id = ((sender as LinkButton).CommandArgument).ToInt();
        var tx = dc.Taxes.Where(x => x.ID == id).FirstOrDefault();
        txtPar.Text = tx.PercentageValue.ToExpressString();
        lblID.Text = id.ToExpressString();
        mpeEditParcent.Show();

        //if (tx != null)
        //{
        //    if (tx.IsActive == true)
        //    {
        //        tx.IsActive = false;
        //    }
        //    else
        //    {
        //        tx.IsActive = true;
        //    }
        //    dc.SubmitChanges();

        //    this.Fill();
        //}
    }

    public string GetTextActive(string id)
    {
        var id1 = id.ToIntOrDefault();
        var tx = dc.Taxes.Where(x => x.ID == id1).FirstOrDefault();
        if (tx != null)
        {
            if (!tx.IsActive.Value)
            {
                return "نشط";
            }
            else
            {
                return "غير نشط";
            }

        }
        return "";
    }
    protected void btnEditParcent_Click(object sender, EventArgs e)
    {
        var taxID = lblID.Text.ToIntOrDefault();
        var tx = dc.Taxes.Where(x => x.ID == taxID).FirstOrDefault();
        tx.PercentageValue = txtPar.Text.ToIntOrDefault();
        dc.SubmitChanges();
        this.Fill();
    }
    protected void BtnClearEdit_Click(object sender, EventArgs e)
    {

    }
}
