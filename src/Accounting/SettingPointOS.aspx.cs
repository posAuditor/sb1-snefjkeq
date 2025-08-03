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
using System.Configuration;

public partial class Accounting_SettingPointOS : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtSettingPointOS
    {
        get
        {
            return (DataTable)Session["dtSettingPointOS" + this.WinID];
        }

        set
        {
            Session["dtSettingPointOS" + this.WinID] = value;
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

    private string ImageUrlPos
    {
        get
        {
            return (string)ViewState["ImageUrlPos"];
        }

        set
        {
            ViewState["ImageUrlPos"] = value;
        }
    }

    #endregion

    #region Page events
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnUploadImage);
            this.CheckSecurity();
            if (!Page.IsPostBack)
            {
                this.LoadControls();
                this.Fill();
                this.FillOffer();
                this.FillPrinter();
                this.FillDescription();
                this.FillDescriptionReady();
                this.LoadControlsTypeTransporter();
                this.FillTypeTransporter();
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

    protected void gvSettingPointOS_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvSettingPointOS.PageIndex = e.NewPageIndex;
            gvSettingPointOS.DataSource = this.dtSettingPointOS;
            gvSettingPointOS.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvSettingPointOS_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            DataRow dr = this.dtSettingPointOS.Select("ID=" + gvSettingPointOS.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            txtName.Text = dr["Name"].ToExpressString();
            txtPercentageValue.Text = dr["PercentageValue"].ToExpressString();
            acPurchaseAccount.Value = dr["PurchaseAccountID"].ToExpressString();
            acSalesAccount.Value = dr["SalesAccountID"].ToExpressString();
            ddlOnSales.SelectedValue = dr["OnInvoiceType"].ToExpressString();
            ddlOnPurchases.SelectedValue = dr["OnReceiptType"].ToExpressString();
            ddlOnDocCredit.SelectedValue = dr["OnDocCreditType"].ToExpressString();
            this.EditID = gvSettingPointOS.DataKeys[e.NewSelectedIndex]["ID"].ToInt();


            var sposIOrU = dc.SettingPointOs.Where(c => c.ID == this.EditID).FirstOrDefault();
            if (sposIOrU != null)
            {
                chkIsCash.Checked = sposIOrU.IsCash.Value;
            }
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvSettingPointOS_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_SettingPointOS_Delete(gvSettingPointOS.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvSettingPointOS.DataKeys[e.RowIndex]["Name"].ToExpressString(), dc);
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
                result = dc.usp_SettingPointOS_Insert(txtName.TrimmedText, txtPercentageValue.Text.ToDecimal(), OnSalesType, OnPurchaseType, OnDocCreditType, acSalesAccount.Value.ToInt(), acPurchaseAccount.Value.ToInt(), this.ImageUrlPos);
            }
            else
            {
                result = dc.usp_SettingPointOS_Update(this.EditID, txtName.TrimmedText, txtPercentageValue.Text.ToDecimal(), OnSalesType, OnPurchaseType, OnDocCreditType, acSalesAccount.Value.ToInt(), acPurchaseAccount.Value.ToInt(), this.ImageUrlPos);
            }
            var sposIOrU = dc.SettingPointOs.Where(c => c.ID == result).FirstOrDefault();
            if (sposIOrU != null)
            {
                sposIOrU.IsCash = chkIsCash.Checked;
            }

            if (result == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                mpeCreateNew.Show();
                return;
            }
            dc.SubmitChanges();
            //if (result == -31)
            //{
            //    UserMessages.Message(null, Resources.UserInfoMessages.BranchlessReq, string.Empty);
            //    mpeCreateNew.Show();
            //    return;
            //}
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
        acName.ContextKey = string.Empty;
        acItemsame.ContextKey = string.Empty;
        acCustomer.ContextKey = "C,,,";
        var obj = dc.AppPos.First();
        chkItemDeleted.Checked = obj.LineInvoiceHasDeleted.Value;
        chkDiscount.Checked = obj.HaveDiscount.Value;
        acCustomer.Value = obj.DefaultCustomer.ToIntOrDefault().ToExpressString();
        chkClogingDays.Checked = obj.LocalCloseDay.Value;
        ddlTypeTax.SelectedValue = obj.TypeTax.Value.ToExpressString();
        acCategory.ContextKey = string.Empty;
        Configuration myConfiguration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
        ddlDatabase.DataSource = dc.usp_Databases_Select(myConfiguration.AppSettings.Settings["DatabaseNameInit"].Value.ToString());
        ddlDatabase.DataTextField = "Name";
        ddlDatabase.DataValueField = "Name";
        ddlDatabase.DataBind();


    }

    private void Fill()
    {
        this.dtSettingPointOS = dc.usp_SettingPointOS_Select(null, txtNameSrch.TrimmedText).CopyToDataTable();
        gvSettingPointOS.DataSource = this.dtSettingPointOS;
        gvSettingPointOS.DataBind();
    }




    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        gvSettingPointOS.Columns[4].Visible = MyContext.PageData.IsEdit;
        gvSettingPointOS.Columns[5].Visible = MyContext.PageData.IsDelete;
        lnkAddNew.Visible = MyContext.PageData.IsAdd;
        mpeCreateNew.TargetControlID = lnkAddNew.Visible ? lnkAddNew.UniqueID : hfmpeCreateNew.UniqueID;
    }

    #endregion

    #region Offer


    private DataTable dtOffer
    {
        get
        {
            return (DataTable)Session["dtOffer" + this.WinID];
        }

        set
        {
            Session["dtOffer" + this.WinID] = value;
        }
    }
    private int EditOfferID
    {
        get
        {
            if (ViewState["EditOfferID"] == null) return 0;
            return (int)ViewState["EditOfferID"];
        }

        set
        {
            ViewState["EditOfferID"] = value;
        }
    }
    private void FillOffer()
    {
        this.dtOffer = dc.usp_Offer_Select(null, null, false).CopyToDataTable();
        gvOffer.DataSource = this.dtOffer;
        gvOffer.DataBind();
    }
    protected void gvOffer_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_Offer_Delete(gvOffer.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvOffer.DataKeys[e.RowIndex]["NameOffer"].ToExpressString(), dc);
            this.Fill();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void gvOffer_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvOffer.PageIndex = e.NewPageIndex;
            gvOffer.DataSource = this.dtSettingPointOS;
            gvOffer.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void gvOffer_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            DataRow dr = this.dtOffer.Select("ID=" + gvOffer.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            txtNameOffer.Text = dr["NameOffer"].ToExpressString();
            txtOfferCashDiscount.Text = dr["Discount"].ToExpressString();
            txtOfferPercentageDiscount.Text = dr["DiscountParcentage"].ToExpressString();
            txtDateFromOffer.Text = dr["FromDate"].ToExpressString();
            txtDateToOffer.Text = dr["ToDate"].ToExpressString();
            txtQuantityOffer.Text = dr["Quantity"].ToExpressString();




            //txtPercentageValue.Text = dr["PercentageValue"].ToExpressString();
            //acPurchaseAccount.Value = dr["PurchaseAccountID"].ToExpressString();
            //acSalesAccount.Value = dr["SalesAccountID"].ToExpressString();
            //ddlOnSales.SelectedValue = dr["OnInvoiceType"].ToExpressString();
            //ddlOnPurchases.SelectedValue = dr["OnReceiptType"].ToExpressString();
            //ddlOnDocCredit.SelectedValue = dr["OnDocCreditType"].ToExpressString();
            this.EditOfferID = gvOffer.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            mpeOffer.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void btnSaveOffer_Click(object sender, EventArgs e)
    {
        try
        {
            int result = 0;


            if (this.EditOfferID == 0) //insert
            {
                result = dc.usp_Offer_Insert(txtNameOffer.TrimmedText, DateTime.Now, true, false, txtOfferCashDiscount.Text.ToDecimal(), txtOfferPercentageDiscount.Text.ToDecimal(), txtDateFromOffer.Text.ToDate(), txtDateToOffer.Text.ToDate(), txtQuantityOffer.Text.ToDecimal());
            }
            else
            {
                result = dc.usp_Offer_Update(this.EditOfferID, txtNameOffer.TrimmedText, DateTime.Now, true, false, txtOfferCashDiscount.Text.ToDecimal(), txtOfferPercentageDiscount.Text.ToDecimal(), txtDateFromOffer.Text.ToDate(), txtDateToOffer.Text.ToDate(), txtQuantityOffer.Text.ToDecimal());
            }
            if (result == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                mpeOffer.Show();
                return;
            }

            LogAction(this.EditID == 0 ? Actions.Add : Actions.Edit, txtName.TrimmedText, dc);
            this.FillOffer();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void btnClearOffer_Click(object sender, EventArgs e)
    {

    }
    protected void lnkDiscountCustomer_Click(object sender, EventArgs e)
    {
        var id = int.Parse((sender as LinkButton).CommandArgument.ToString());
        this.EditID = id;
        FillOfferCustomer(id);
        mpeOfferCustomer.Show();
    }
    #endregion

    #region OfferCusomer


    private DataTable dtOfferCusomer
    {
        get
        {
            return (DataTable)Session["dtOfferCusomer" + this.WinID];
        }
        set
        {
            Session["dtOfferCusomer" + this.WinID] = value;
        }
    }
    private int EditOfferCustomerID
    {
        get
        {
            if (ViewState["EditOfferCustomerID"] == null) return 0;
            return (int)ViewState["EditOfferCustomerID"];
        }

        set
        {
            ViewState["EditOfferCustomerID"] = value;
        }
    }
    private void FillOfferCustomer(int offerId)
    {

        this.dtOfferCusomer = dc.usp_OfferCustomer_Select(offerId).ToList().CopyToDataTable();
        gvCustomerOffer.DataSource = this.dtOfferCusomer;
        gvCustomerOffer.DataBind();
    }

    protected void lnkDiscountItems_Click(object sender, EventArgs e)
    {
        var id = int.Parse((sender as LinkButton).CommandArgument.ToString());
        this.EditID = id;
        FillOfferItems(id);
        mpeOfferItems.Show();
    }
    protected void btnOfferCustomer_Click(object sender, EventArgs e)
    {
        try
        {
            int result = 0;


            if (this.EditOfferCustomerID == 0) //insert
            {
                result = dc.usp_OfferCustomer_Insert(acName.Value.ToInt(), this.EditID, false, txtCashDiscountOfferCustomer.Text.ToDecimalOrDefault(), txtParcentDiscountOfferCustomer.Text.ToDecimalOrDefault(), txtCoupon.Text.ToExpressString());
            }
            else
            {
                result = dc.usp_OfferCustomer_Update(this.EditOfferCustomerID, acName.Value.ToInt(), this.EditID, txtCashDiscountOfferCustomer.Text.ToDecimalOrDefault(), txtParcentDiscountOfferCustomer.Text.ToDecimalOrDefault(), txtCoupon.Text.ToExpressString());
            }
            if (result == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                mpeOffer.Show();
                return;
            }

            LogAction(this.EditID == 0 ? Actions.Add : Actions.Edit, txtName.TrimmedText, dc);


            this.EditOfferID = 0;
            this.FillOfferCustomer(this.EditID);

            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
            mpeOfferCustomer.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void gvCustomerOffer_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvCustomerOffer.PageIndex = e.NewPageIndex;
            gvCustomerOffer.DataSource = this.dtSettingPointOS;
            gvCustomerOffer.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void gvCustomerOffer_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            DataRow dr = this.dtOfferCusomer.Select("ID=" + gvCustomerOffer.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            acName.Value = dr["Contact_ID"].ToExpressString();
            txtCashDiscountOfferCustomer.Text = dr["CashDiscount"].ToExpressString();
            txtParcentDiscountOfferCustomer.Text = dr["ParcentageDiscount"].ToExpressString();
            this.EditOfferCustomerID = gvCustomerOffer.DataKeys[e.NewSelectedIndex]["ID"].ToInt();

            mpeOfferCustomer.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void gvCustomerOffer_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_OfferCustomer_Delete(gvCustomerOffer.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvCustomerOffer.DataKeys[e.RowIndex]["Name"].ToExpressString(), dc);
            this.FillOfferCustomer(this.EditID);
            mpeOfferCustomer.Show();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    #endregion

    #region OfferItems


    private DataTable dtOfferItems
    {
        get
        {
            return (DataTable)Session["dtOfferItems" + this.WinID];
        }
        set
        {
            Session["dtOfferItems" + this.WinID] = value;
        }
    }
    private int EditOfferItemsID
    {
        get
        {
            if (ViewState["EditOfferItemsID"] == null) return 0;
            return (int)ViewState["EditOfferItemsID"];
        }

        set
        {
            ViewState["EditOfferItemsID"] = value;
        }
    }
    private void FillOfferItems(int offerId)
    {

        this.dtOfferCusomer = dc.usp_OfferItem_Select(offerId).ToList().CopyToDataTable();
        gvItemsOffer.DataSource = this.dtOfferCusomer;
        gvItemsOffer.DataBind();
    }

    protected void acCategory_SelectedIndexChanged(object sender, AutoCompleteEventArgs e)
    {
        try
        {
            acItemsame.Clear();
            acItemsame.ContextKey = "," + acCategory.Value + ",,";
            mpeOfferItems.Show();

            //if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void btnOfferItems_Click1(object sender, EventArgs e)
    {
        try
        {
            int result = 0;


            if (this.EditOfferItemsID == 0) //insert
            {
                if (string.IsNullOrEmpty(acItemsame.Value))
                {
                    var listItemsByCategory = dc.Items.Where(x => x.Category_ID == acCategory.Value.ToInt()).ToList();
                    foreach (var item in listItemsByCategory)
                    {
                        result = dc.usp_OfferItem_Insert(item.ID, this.EditID, false, txtCashDiscountOfferItems.Text.ToDecimalOrDefault(), txtParcentDiscountOfferItems.Text.ToDecimalOrDefault());
                    }
                }
                else
                {
                    result = dc.usp_OfferItem_Insert(acItemsame.Value.ToInt(), this.EditID, false, txtCashDiscountOfferItems.Text.ToDecimalOrDefault(), txtParcentDiscountOfferItems.Text.ToDecimalOrDefault());
                }
            }
            else
            {
                result = dc.usp_OfferItem_Update(this.EditOfferItemsID, acItemsame.Value.ToInt(), this.EditID, txtCashDiscountOfferItems.Text.ToDecimalOrDefault(), txtParcentDiscountOfferItems.Text.ToDecimalOrDefault());
            }
            if (result == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                mpeOffer.Show();
                return;
            }

            LogAction(this.EditID == 0 ? Actions.Add : Actions.Edit, txtName.TrimmedText, dc);


            this.EditOfferID = 0;
            this.FillOfferItems(this.EditID);

            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
            mpeOfferItems.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void gvItemsOffer_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvItemsOffer.PageIndex = e.NewPageIndex;
            gvItemsOffer.DataSource = this.dtSettingPointOS;
            gvItemsOffer.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void gvItemsOffer_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            DataRow dr = this.dtOfferCusomer.Select("ID=" + gvItemsOffer.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            acItemsame.Value = dr["Item_ID"].ToExpressString();
            txtCashDiscountOfferItems.Text = dr["CashDiscount"].ToExpressString();
            txtParcentDiscountOfferItems.Text = dr["ParcentageDiscount"].ToExpressString();
            this.EditOfferItemsID = gvItemsOffer.DataKeys[e.NewSelectedIndex]["ID"].ToInt();

            mpeOfferItems.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void gvItemsOffer_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_OfferItem_Delete(gvItemsOffer.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvItemsOffer.DataKeys[e.RowIndex]["Name"].ToExpressString(), dc);
            this.FillOfferItems(this.EditID);
            mpeOfferItems.Show();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    #endregion

    #region Type Trans

    #region Properties

    private DataTable dtTypeTran
    {
        get
        {
            return (DataTable)Session["dtTypeTran" + this.WinID];
        }

        set
        {
            Session["dtTypeTran" + this.WinID] = value;
        }
    }

    private int EditIDTypeTran
    {
        get
        {
            if (ViewState["EditIDTypeTran"] == null) return 0;
            return (int)ViewState["EditIDTypeTran"];
        }

        set
        {
            ViewState["EditIDTypeTran"] = value;
        }
    }

    private string ImageUrl
    {
        get
        {
            return (string)ViewState["ImageUrl"];
        }

        set
        {
            ViewState["ImageUrl"] = value;
        }
    }

    #endregion

    #region Page events




    #endregion

    #region Control Events





    protected void gvTypeTranTypeTransporter_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvTypeTranTypeTransporter.PageIndex = e.NewPageIndex;
            gvTypeTranTypeTransporter.DataSource = this.dtTypeTran;
            gvTypeTranTypeTransporter.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvTypeTranTypeTransporter_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            this.EditIDTypeTran = int.Parse(gvTypeTranTypeTransporter.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString());
            var DefaultCurrency_ID = dc.usp_Company_Select().First().Currency_ID;

            DataRow dr = this.dtTypeTran.Select("ID=" + gvTypeTranTypeTransporter.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            txtNameTypeTransporter.Text = dr["NameType"].ToExpressString();
            chkIsFavorit.Checked = dr["Favorite"].ToBooleanOrDefault();


            mpeCreateNewTypeTransporter.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvTypeTranTypeTransporter_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_TypeTransporter_delete(gvTypeTranTypeTransporter.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvTypeTranTypeTransporter.DataKeys[e.RowIndex]["NameType"].ToExpressString(), dc);
            this.FillTypeTransporter();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    //protected void ClosePopup_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        this.ClearFormTypeTransporter();
    //        //acDefaultCashAccount_ID.Enabled = acDefaultCustomer.Enabled = acDefaultStore.Enabled = false;
    //        this.EditIDTypeTran = 0;
    //        if (sender == null && MyContext.FastEntryEnabled)
    //        {
    //            mpeCreateNewTypeTransporter.Show();
    //        }
    //        else
    //        {
    //            mpeCreateNewTypeTransporter.Hide();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
    //    }
    //}

    protected void btnSaveNewTypeTransporter_click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {
            int result = 0;
            System.Data.Linq.Binary imageLogo = null;

            if (this.ImageUrl != null) imageLogo = new System.Data.Linq.Binary(System.IO.File.ReadAllBytes(Server.MapPath("~\\uploads\\" + this.ImageUrl)));


            if (this.EditIDTypeTran == 0) //insert
            {
                result = dc.usp_TypeTransporter_Insert(txtNameTypeTransporter.TrimmedText, chkIsFavorit.Checked);
            }
            else
            {
                result = dc.usp_TypeTransporter_Update(this.EditIDTypeTran, txtNameTypeTransporter.TrimmedText, chkIsFavorit.Checked);

                if (result == -2)
                {
                    UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                    trans.Rollback();
                    mpeCreateNewTypeTransporter.Show();
                    return;
                }
            }

            LogAction(this.EditIDTypeTran == 0 ? Actions.Add : Actions.Edit, txtNameTypeTransporter.TrimmedText, dc);
            this.FillTypeTransporter();
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

    protected void BtnClearNewTypeTransporter_click(object sender, EventArgs e)
    {
        try
        {
            this.ClearFormTypeTransporter();
            mpeCreateNewTypeTransporter.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnUploadImage_Click(object sender, EventArgs e)
    {
        try
        {

            mpeCreateNewTypeTransporter.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private void ClearFormTypeTransporter()
    {
        txtNameTypeTransporter.Clear();

        this.ImageUrl = null;
    }

    private void LoadControlsTypeTransporter()
    {

    }

    private void FillTypeTransporter()
    {
        this.dtTypeTran = dc.usp_TypeTransporter_Select(string.Empty, MyContext.UserProfile.Branch_ID).CopyToDataTable();
        gvTypeTranTypeTransporter.DataSource = this.dtTypeTran;
        gvTypeTranTypeTransporter.DataBind();
    }



    #endregion


    #endregion

    protected void btnSaveCompanyData_Click(object sender, EventArgs e)
    {
        var obj = dc.AppPos.First();
        obj.LineInvoiceHasDeleted = chkItemDeleted.Checked;
        obj.HaveDiscount = chkDiscount.Checked;
        obj.LocalCloseDay = chkClogingDays.Checked;
        obj.TypeTax = ddlTypeTax.SelectedValue.ToInt();
        obj.DefaultCustomer = acCustomer.Value.ToIntOrDefault();

        dc.SubmitChanges();


        var filePath = (Server.MapPath("~/")).Replace("\\Auditor", "") + "API\\API_POS\\Web.config";

        var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = filePath };
        var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

        var section = (ConnectionStringsSection)configuration.GetSection("connectionStrings");


        System.Data.Common.DbConnectionStringBuilder builder = new System.Data.Common.DbConnectionStringBuilder();
        builder.ConnectionString = section.ConnectionStrings["XpressConnectionString"].ConnectionString;

        builder["Initial Catalog"] = ddlDatabase.SelectedItem.Text;



        section.ConnectionStrings["XpressConnectionString"].ConnectionString = @"data source=" + builder["data source"] + ";initial catalog=" + builder["Initial Catalog"] + ";user id=" + builder["user id"] + ";password=" + builder["password"] + "";
        configuration.Save();
    }
    protected void btnUploadImage_Click1(object sender, EventArgs e)
    {
        try
        {
            if (!fpLogo.HasFile) return;
            string fileName = null;
            do
            {
                fileName = Guid.NewGuid() + Path.GetExtension(fpLogo.PostedFile.FileName);
            }
            while (File.Exists(Server.MapPath("~\\uploads\\" + fileName)));

            Bitmap originalBMP = new Bitmap(fpLogo.FileContent);
            Bitmap newBMP = new Bitmap(originalBMP, 200, 150);
            Graphics objGraphics = Graphics.FromImage(newBMP);
            objGraphics.SmoothingMode = SmoothingMode.AntiAlias;
            objGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            objGraphics.DrawImage(originalBMP, 0, 0, 200, 150);
            newBMP.Save(Server.MapPath("~\\uploads\\" + fileName)); ;
            imgLogo.ImageUrl = "~/uploads/" + fileName;
            this.ImageUrlPos = fileName;
            mpeCreateNew.Show();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Your Comment", "ShowModalPopup();", true);
        }
        catch (Exception ex)
        {
            mpeCreateNew.Show();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }



    protected void Button1_Click(object sender, EventArgs e)
    {

    }
    protected void Button2_Click(object sender, EventArgs e)
    {

        var appP = new AppPrinter();
        appP.PrinterName = txtPrinterName.Text;
        dc.AppPrinters.InsertOnSubmit(appP);
        dc.SubmitChanges();
        ModalPopupExtender1.Show();
        gvPrinter.DataSource = dc.AppPrinters.CopyToDataTable();
        gvPrinter.DataBind();

    }
    protected void Button3_Click(object sender, EventArgs e)
    {

    }
    protected void gvPrinter_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {

            int id = gvPrinter.DataKeys[e.RowIndex]["ID"].ToInt();
            var nameprinter = gvPrinter.DataKeys[e.RowIndex]["PrinterName"].ToExpressString();
            var obj = dc.AppPrinters.Where(x => x.Id == id).FirstOrDefault();
            var listitem = dc.Items.Where(x => x.PrinterName_ID == id).ToList();
            if (listitem.Any())
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            dc.AppPrinters.DeleteOnSubmit(obj);
            dc.SubmitChanges();
            gvPrinter.DataSource = dc.AppPrinters.CopyToDataTable();
            gvPrinter.DataBind();
            LogAction(Actions.Delete, nameprinter, dc);

            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void gvPrinter_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvPrinter.PageIndex = e.NewPageIndex;
            gvPrinter.DataSource = dc.AppPrinters.CopyToDataTable();
            gvPrinter.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void gvPrinter_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            //DataRow dr = this.dtSettingPointOS.Select("ID=" + gvSettingPointOS.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            //txtName.Text = dr["Name"].ToExpressString();
            //txtPercentageValue.Text = dr["PercentageValue"].ToExpressString();
            //acPurchaseAccount.Value = dr["PurchaseAccountID"].ToExpressString();
            //acSalesAccount.Value = dr["SalesAccountID"].ToExpressString();
            //ddlOnSales.SelectedValue = dr["OnInvoiceType"].ToExpressString();
            //ddlOnPurchases.SelectedValue = dr["OnReceiptType"].ToExpressString();
            //ddlOnDocCredit.SelectedValue = dr["OnDocCreditType"].ToExpressString();
            //this.EditID = gvSettingPointOS.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            //mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    private void FillPrinter()
    {
        gvPrinter.DataSource = dc.AppPrinters.CopyToDataTable();
        gvPrinter.DataBind();
    }

    private void FillDescription()
    {
        gvDescription.DataSource = dc.DetailsDescriptions.CopyToDataTable();
        gvDescription.DataBind();
    }
    private void FillDescriptionReady()
    {
        gvDescriptionReady.DataSource = dc.DescriptionReadies.CopyToDataTable();
        gvDescriptionReady.DataBind();
    }

    protected void btnSaveDescription_Click(object sender, EventArgs e)
    {
        var appP = new DetailsDescription();
        appP.Description = txtDescription.Text;
        dc.DetailsDescriptions.InsertOnSubmit(appP);
        dc.SubmitChanges();
        mpeDescription.Show();
        FillDescription();
        txtDescription.Text = string.Empty;
    }


    protected void gvDescription_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {

            int id = gvDescription.DataKeys[e.RowIndex]["Id"].ToInt();
            var Description = gvDescription.DataKeys[e.RowIndex]["Description"].ToExpressString();
            var obj = dc.DetailsDescriptions.Where(x => x.Id == id).FirstOrDefault();

            dc.DetailsDescriptions.DeleteOnSubmit(obj);
            dc.SubmitChanges();
            FillDescription();
            LogAction(Actions.Delete, Description, dc);

            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }


    protected void gvDescription_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvDescription.PageIndex = e.NewPageIndex;
            gvDescription.DataSource = dc.DetailsDescriptions.CopyToDataTable();
            gvDescription.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvDescriptionReady_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {

            int id = gvDescriptionReady.DataKeys[e.RowIndex]["Id"].ToInt();
            var Description = gvDescriptionReady.DataKeys[e.RowIndex]["Description"].ToExpressString();
            var obj = dc.DescriptionReadies.Where(x => x.ID == id).FirstOrDefault();

            dc.DescriptionReadies.DeleteOnSubmit(obj);
            dc.SubmitChanges();
            FillDescriptionReady();
            LogAction(Actions.Delete, Description, dc);

            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void gvDescriptionReady_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvDescription.PageIndex = e.NewPageIndex;
            gvDescription.DataSource = dc.DescriptionReadies.CopyToDataTable();
            gvDescription.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void btnDescriptionReady_Click(object sender, EventArgs e)
    {
        var appP = new DescriptionReady();
        appP.Description = txtDescriptionReady.Text;
        dc.DescriptionReadies.InsertOnSubmit(appP);
        dc.SubmitChanges();
        mpeDescriptionReady.Show();
        FillDescriptionReady();
        txtDescriptionReady.Text = string.Empty;
    }
}





