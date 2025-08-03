using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class Sales_SalesOrder : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties
    private int TypeTax
    {
        get
        {
            if (Session["TypeTax_Invoice" + this.WinID] == null)
            { 
                Session["TypeTax_Invoice" + this.WinID] = dc.usp_Company_Select().FirstOrDefault().TypeTax;
            }
            return (int)Session["TypeTax_Invoice" + this.WinID];
        }

        set
        {
            Session["TypeTax_Invoice" + this.WinID] = value;
        }
    }
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

    public byte DocStatus_ID
    {
        get
        {
            if (ViewState["DocStatus_ID"] == null) return 1;
            return (byte)ViewState["DocStatus_ID"];
        }

        set
        {
            ViewState["DocStatus_ID"] = value;
        }
    }
    private DataTable dtQtyItemeStoreGroup
    {
        get
        {
            return (DataTable)Session["dtQtyItemeStoreGroup" + this.WinID];
        }

        set
        {
            Session["dtQtyItemeStoreGroup" + this.WinID] = value;
        }
    }

    private DataTable dtItemePrice
    {
        get
        {
            return (DataTable)Session["dtItemePrice" + this.WinID];
        }

        set
        {
            Session["dtItemePrice" + this.WinID] = value;
        }
    }



    private int Invoice_ID
    {
        get
        {
            if (ViewState["Invoice_ID"] == null) return 0;
            return (int)ViewState["Invoice_ID"];
        }

        set
        {
            ViewState["Invoice_ID"] = value;
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

    private DataTable dtItems
    {
        get
        {
            if (Session["dtItems_SalesORDER" + this.WinID] == null)
            {
                //Session["dtItems_SalesORDER" + this.WinID] = dc.usp_InvoiceDetails_Select(null).CopyToDataTable();
                Session["dtItems_SalesORDER" + this.WinID] = dc.usp_InvoiceDetailsIncludeTax_Select(null).CopyToDataTable();

            }
            return (DataTable)Session["dtItems_SalesORDER" + this.WinID];
        }

        set
        {
            Session["dtItems_SalesORDER" + this.WinID] = value;
        }
    }

    private DataTable dtTaxes
    {
        get
        {
            if (Session["dtTaxes_SalesORDER" + this.WinID] == null)
            {
                Session["dtTaxes_SalesORDER" + this.WinID] = dc.usp_DocuemntTaxes_Select(null, null, false).CopyToDataTable();
            }
            return (DataTable)Session["dtTaxes_SalesORDER" + this.WinID];
        }

        set
        {
            Session["dtTaxes_SalesORDER" + this.WinID] = value;
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

    private decimal Total
    {
        get
        {
            if (ViewState["Total"] == null) return 0;
            return (decimal)ViewState["Total"];
        }

        set
        {
            ViewState["Total"] = value;
        }
    }

    private decimal GrossTotal
    {
        get
        {
            if (ViewState["GrossTotal"] == null) return 0;
            return (decimal)ViewState["GrossTotal"];
        }

        set
        {
            ViewState["GrossTotal"] = value;
        }
    }

    private decimal TotalDiscount
    {
        get
        {
            if (ViewState["TotalDiscount"] == null) return 0;
            return (decimal)ViewState["TotalDiscount"];
        }

        set
        {
            ViewState["TotalDiscount"] = value;
        }
    }

    private decimal TotalTax
    {
        get
        {
            if (ViewState["TotalTax"] == null) return 0;
            return (decimal)ViewState["TotalTax"];
        }

        set
        {
            ViewState["TotalTax"] = value;
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

    private bool SumFirstPaid
    {
        get
        {
            if (ViewState["SumFirstPaid"] == null)
            {
                ViewState["SumFirstPaid"] = dc.usp_Company_Select().First().SumFirstPaid;
            }
            return (bool)ViewState["SumFirstPaid"];
        }

        set
        {
            ViewState["SumFirstPaid"] = value;
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
                this.SetDefaults();
                if (EditMode) this.FillInvoice();
                VisibiliyControl();
                var comp = dc.usp_Company_Select().FirstOrDefault();
                txtCost.Enabled = !comp.IsPriceLoocked.Value;
            }

            ucNav.SourceDocTypeType_ID = DocumentsTableTypes.Invoice.ToInt();
            ucNav.EntryType = (byte)1;
            ucNav.Res_ID = this.Invoice_ID;
            ucNav.IsPermShow = (MyContext.UserProfile.HasPermissionShow == false ? (int?)null : MyContext.UserProfile.Contact_ID);

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

      


    protected void txtCodeItem_TextChanged(object sender, EventArgs e)
    {
        try
        {
            acItem.Clear();
            if (txtCItem.IsNotEmpty)
            {
                var item = dc.usp_ItemsCode_Select(txtCItem.TrimmedText, string.Empty, null, null, null, true).FirstOrDefault();
                if (item != null) acItem.Value = item.ID.ToExpressString();
            }
            this.acItem_SelectedIndexChanged(null, null);
            if (acItem.HasValue) txtQty.Focus(); else this.FocusNextControl(acStore);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private void VisibiliyControl()
    {
        var obj = dc.usp_Company_Select().First();
        if (obj.IsDescribed != null)
        {

            acItemDescribed.Visible = obj.IsDescribed.Value;
            acItemDescribed.IsRequired = obj.IsDescribed.Value;
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        this.CustomPage();
    }

    #endregion

    #region Control Events
    protected void acCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            acShipAddress.ContextKey = acCustomer.Value + "," + ContactDetailsTypes.ShipAddress.ToInt().ToExpressString();
            acAddress.ContextKey = acCustomer.Value + "," + ContactDetailsTypes.DefaultAddress.ToInt().ToExpressString();
            acPaymentAddress.ContextKey = acCustomer.Value + "," + ContactDetailsTypes.PaymentAddress.ToInt().ToExpressString();
            acTelephone.ContextKey = acCustomer.Value + "," + ContactDetailsTypes.DefaultTelephone.ToInt().ToExpressString();
            acSalesRep.ContextKey = acCustomer.Value + ",," + acBranch.Value;
            if (!this.EditMode)
            {
                int? DefaultRep_ID = dc.fun_GetDefaultSalesRep_ID(acCustomer.Value.ToNullableInt(), null);
                if (DefaultRep_ID.HasValue) acSalesRep.Value = DefaultRep_ID.ToExpressString();
            }
            if (sender != null) this.FocusNextControl(sender);
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
            var ratio = dc.fun_GetCurrentRatio(ddlCurrency.SelectedValue.ToInt(), txtOperationDate.Text.ToDate());
            txtRatio.Text = ratio == null ? string.Empty : ratio.ToExpressString();
            if (sender != null) this.FocusNextControl(sender);
            this.FilterByBranchAndCurrency();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void acStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (this.EditID == 0) this.FilterItems();
            this.ShowAvailableQty();
            if (dtItems.Rows.Count > 0)
            {
                if (acStore.HasValue)
                {
                    foreach (DataRow item in dtItems.Rows)
                    {
                        item["Store_ID"] = acStore.Value.ToIntOrDefault();
                    }
                }

            }
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
            this.FilterItems();
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void acItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FilterItemsDescribed();
            this.FilterItemsData();
            this.ShowAvailableQty();
            this.ShowCustomerLastItemPrice();
            this.FillQtyStoreGroupList();
            this.FillItemeList_price();
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void acUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtProductionDate.Clear();
            txtExpirationDate.Clear();
            acBatchID.Clear();
            acBatchID.ContextKey = acItem.Value + "," + acStore.Value + "," + acUnit.Value + ",True";
            if (sender != null) txtCost.Text = (dc.fun_GetItemDefaultPriceByUOM(acItem.Value.ToNullableInt(), acUnit.Value.ToNullableInt(), acPriceName.Value.ToNullableInt()) / (txtRatio.Text.ToNullableDecimal() ?? 1)).Value.ToString("0.####");
            if (sender != null) this.ShowAvailableQty();
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void acPriceName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (sender != null) txtCost.Text = (dc.fun_GetItemDefaultPriceByUOM(acItem.Value.ToNullableInt(), acUnit.Value.ToNullableInt(), acPriceName.Value.ToNullableInt()) / (txtRatio.Text.ToNullableDecimal() ?? 1)).Value.ToString("0.####");
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtBarcode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            acItem.Clear();
            if (txtBarcode.IsNotEmpty)
            {
                var item = dc.usp_Items_Select(txtBarcode.TrimmedText, string.Empty, null, null, null, true).FirstOrDefault();
                if (item != null) acItem.Value = item.ID.ToExpressString();
            }
            this.acItem_SelectedIndexChanged(null, null);
            if (acItem.HasValue) txtQty.Focus(); else this.FocusNextControl(acStore);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnAddItem_click(object sender, EventArgs e)
    {
        try
        {

            if (!string.IsNullOrEmpty(txtCapacities.Text) && !string.IsNullOrEmpty(txtCapacity.Text))

                if (decimal.Parse(txtCapacities.Text.Split('-').Length.ToString()) != decimal.Parse(txtCapacity.Text))
                {

                    UserMessages.Message(null, Resources.UserInfoMessages.CapacityDistributionIsIncorrect, string.Empty);
                    return;
                }
            DataRow r = null;
            if (this.EditID == 0)
            {
                r = this.dtItems.NewRow();
                r["ID"] = this.dtItems.GetID("ID");

            }
            else
            {
                r = this.dtItems.Select("ID=" + this.EditID)[0];
            }

            r["Store_ID"] = acStore.Value;
            r["Category_ID"] = acCategory.Value;
            //var objCompany = dc.usp_Company_Select().First();

            r["Item_ID"] = acItem.Value;
            r["ItemName"] = acItem.Text;
            r["ItemDescription"] = acItemDescribed.Value;
            r["DescribedName"] = acItemDescribed.Text;
            r["UnitCost"] = txtCost.Text;
            r["Quantity"] = txtQty.Text;
            r["Capacity"] = txtCapacity.Text;
            r["Capacities"] = txtCapacities.Text;
            r["QtyInNumber"] = txtQtyInNumber.Text.ToDecimalOrDefault();
            r["Uom_ID"] = acUnit.Value;
            r["UOMName"] = acUnit.Text;
            r["Batch_ID"] = (acBatchID.HasValue && acBatchID.Value.ToNullableInt() != 0) ? acBatchID.Value.ToIntOrDBNULL() : DBNull.Value;
            r["BatchName"] = (acBatchID.HasValue && acBatchID.Value.ToNullableInt() != 0) ? acBatchID.Text.Substring(0, acBatchID.Text.IndexOf((char)65279)) : string.Empty;
            r["PercentageDiscount"] = txtItemPercentageDiscount.Text.ToDecimalOrDefault();
            r["CashDiscount"] = txtItemCashDiscount.Text.ToDecimalOrDefault();
            r["TotalTax"] = 0;
            r["Notes"] = txtItemNotes.Text;
            r["StoreName"] = acStore.Text;

            r["Barcode"] = txtBarcode.Text;
            r["CategoryName"] = acCategory.Text;
            r["ProductionDate"] = txtProductionDate.Text.ToDateOrDBNULL();
            r["ExpirationDate"] = txtExpirationDate.Text.ToDateOrDBNULL();

            if (acItemTax.HasValue)
            {
                var Tax = dc.usp_Taxes_Select(acItemTax.Value.ToInt(), string.Empty).FirstOrDefault();
                r["TaxName"] = Tax.Name;
                r["Tax_ID"] = acItemTax.Value;
                r["TaxPercentageValue"] = Tax.PercentageValue;
                if (Tax.OnInvoiceType.HasValue) r["TaxOnInvoiceType"] = Tax.OnInvoiceType;
                if (Tax.OnReceiptType.HasValue) r["TaxOnReceiptType"] = Tax.OnReceiptType;
                if (Tax.OnDocCreditType.HasValue) r["TaxOnDocCreditType"] = Tax.OnDocCreditType;
                r["TaxSalesAccountID"] = Tax.SalesAccountID;
                r["TaxPurchaseAccountID"] = Tax.PurchaseAccountID;
            }
            else
            {
                r["TaxName"] = DBNull.Value;
                r["Tax_ID"] = DBNull.Value;
                r["TaxPercentageValue"] = DBNull.Value;
                r["TaxOnInvoiceType"] = DBNull.Value;
                r["TaxOnReceiptType"] = DBNull.Value;
                r["TaxOnDocCreditType"] = DBNull.Value;
                r["TaxSalesAccountID"] = DBNull.Value;
                r["TaxPurchaseAccountID"] = DBNull.Value;
            }

            r["Total"] = 0;
            r["GrossTotal"] = 0;
            if (this.EditID == 0) this.dtItems.Rows.Add(r);

            this.ClearItemForm();
            this.BindItemsGrid();
            this.FocusNextControl(acStore);
            this.EditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnAddItemGroup_click(object sender, EventArgs e)
    {
        try
        {
            DataRow r = null;
            if (this.EditID != 0) return;

            if (acBatchID.HasValue)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.NoGroupWithBatch, string.Empty);
                return;
            }

            var Items = dc.usp_Items_SelectByCategory(acCategory.Value.ToInt(), false);

            foreach (var item in Items)
            {

                r = this.dtItems.NewRow();
                r["ID"] = this.dtItems.GetID("ID");
                r["Store_ID"] = acStore.Value;
                r["Category_ID"] = acCategory.Value;
                r["Item_ID"] = item.ID;
                r["UnitCost"] = txtCost.Text;
                r["Quantity"] = txtQty.Text;
                r["QtyInNumber"] = txtQtyInNumber.Text.ToDecimalOrDefault();
                r["Uom_ID"] = acUnit.Value;
                r["UOMName"] = acUnit.Text;
                r["Batch_ID"] = (acBatchID.HasValue && acBatchID.Value.ToNullableInt() != 0) ? acBatchID.Value.ToIntOrDBNULL() : DBNull.Value;
                r["BatchName"] = (acBatchID.HasValue && acBatchID.Value.ToNullableInt() != 0) ? acBatchID.Text.Substring(0, acBatchID.Text.IndexOf((char)65279)) : string.Empty;
                r["PercentageDiscount"] = txtItemPercentageDiscount.Text.ToDecimalOrDefault();
                r["CashDiscount"] = txtItemCashDiscount.Text.ToDecimalOrDefault();
                r["TotalTax"] = 0;
                r["Notes"] = txtItemNotes.Text;
                r["StoreName"] = acStore.Text;
                r["ItemName"] = item.Name;
                r["Barcode"] = item.Barcode;
                r["CategoryName"] = acCategory.Text;
                r["ProductionDate"] = txtProductionDate.Text.ToDateOrDBNULL();
                r["ExpirationDate"] = txtExpirationDate.Text.ToDateOrDBNULL();

                if (acItemTax.HasValue)
                {
                    var Tax = dc.usp_Taxes_Select(acItemTax.Value.ToInt(), string.Empty).FirstOrDefault();
                    r["TaxName"] = Tax.Name;
                    r["Tax_ID"] = acItemTax.Value;
                    r["TaxPercentageValue"] = Tax.PercentageValue;
                    if (Tax.OnInvoiceType.HasValue) r["TaxOnInvoiceType"] = Tax.OnInvoiceType;
                    if (Tax.OnReceiptType.HasValue) r["TaxOnReceiptType"] = Tax.OnReceiptType;
                    if (Tax.OnDocCreditType.HasValue) r["TaxOnDocCreditType"] = Tax.OnDocCreditType;
                    r["TaxSalesAccountID"] = Tax.SalesAccountID;
                    r["TaxPurchaseAccountID"] = Tax.PurchaseAccountID;
                }
                else
                {
                    r["TaxName"] = DBNull.Value;
                    r["Tax_ID"] = DBNull.Value;
                    r["TaxPercentageValue"] = DBNull.Value;
                    r["TaxOnInvoiceType"] = DBNull.Value;
                    r["TaxOnReceiptType"] = DBNull.Value;
                    r["TaxOnDocCreditType"] = DBNull.Value;
                    r["TaxSalesAccountID"] = DBNull.Value;
                    r["TaxPurchaseAccountID"] = DBNull.Value;
                }

                r["Total"] = 0;
                r["GrossTotal"] = 0;
                this.dtItems.Rows.Add(r);
            }

            this.ClearItemForm();
            this.BindItemsGrid();
            if (acStore.HasValue) this.FocusNextControl(acStore); else acStore.AutoCompleteFocus();
            this.EditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnClearItem_Click(object sender, EventArgs e)
    {
        try
        {
            this.ClearItemForm();
            acStore.AutoCompleteFocus();
            this.EditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvItems.PageIndex = e.NewPageIndex;
            this.BindItemsGrid();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvItems_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            this.EditID = gvItems.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            DataRow r = this.dtItems.Select("ID=" + this.EditID.ToExpressString())[0];

            acStore.Value = r["Store_ID"].ToExpressString();
            acCategory.Value = r["Category_ID"].ToExpressString();
            this.acCategory_SelectedIndexChanged(null, null);
            acItem.Value = r["Item_ID"].ToExpressString();
            this.acItemDescribed.Value = r["ItemDescription"].ToExpressString();
            this.acItem_SelectedIndexChanged(null, null);
            txtCost.Text = r["UnitCost"].ToExpressString();
            txtQty.Text = r["Quantity"].ToExpressString();
            txtCapacity.Text = r["Capacity"].ToExpressString();
            txtCapacities.Text = r["Capacities"].ToExpressString();
            txtQtyInNumber.Text = r["QtyInNumber"].ToExpressString();
            acUnit.Value = r["Uom_ID"].ToExpressString();
            this.acUnit_SelectedIndexChanged(null, null);
            acItemTax.Value = r["Tax_ID"].ToExpressString();
            acBatchID.Value = r["Batch_ID"].ToExpressString();
            txtItemPercentageDiscount.Text = r["PercentageDiscount"].ToExpressString();
            txtItemCashDiscount.Text = r["CashDiscount"].ToExpressString();
            txtItemNotes.Text = r["Notes"].ToExpressString();
            txtBarcode.Text = r["Barcode"].ToExpressString();
            if (r["ProductionDate"].ToExpressString() != string.Empty) txtProductionDate.Text = r["ProductionDate"].ToDate().Value.ToString("d/M/yyyy"); ;
            if (r["ExpirationDate"].ToExpressString() != string.Empty) txtExpirationDate.Text = r["ExpirationDate"].ToDate().Value.ToString("d/M/yyyy");
            this.ShowAvailableQty(); //Called Twice but important

            this.CalculateTotalRow();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ID = gvItems.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dtItems.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            this.ClearItemForm();
            this.BindItemsGrid();
            this.EditID = 0;
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
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

    protected void acBatchID_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtProductionDate.Clear();
            txtExpirationDate.Clear();
            if (acBatchID.HasValue && acBatchID.Value.ToNullableInt() != 0)
            {
                var batch = dc.usp_ItemsBatch_Select(acBatchID.Value.ToInt(), acItem.Value.ToInt(), null, null, false).FirstOrDefault();
                if (batch.ProductionDate.HasValue) txtProductionDate.Text = batch.ProductionDate.Value.ToString("d/M/yyyy");
                if (batch.ExpirationDate.HasValue) txtExpirationDate.Text = batch.ExpirationDate.Value.ToString("d/M/yyyy");
            }
            this.ShowAvailableQty();
            if (sender != null) this.FocusNextControl(sender);
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

    protected void gvTaxes_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ID = gvTaxes.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dtTaxes.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            this.Calculate();
            gvTaxes.DataSource = this.dtTaxes;
            gvTaxes.DataBind();
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnAddTax_click(object sender, EventArgs e)
    {
        try
        {
            DataRow r = this.dtTaxes.NewRow();
            r["ID"] = this.dtTaxes.GetID("ID");
            r["Tax_ID"] = acTax.Value;
            if (acTax.HasValue)
            {
                var Tax = dc.usp_Taxes_Select(acTax.Value.ToInt(), string.Empty).FirstOrDefault();
                r["Name"] = Tax.Name;
                r["PercentageValue"] = Tax.PercentageValue;
                if (Tax.OnInvoiceType.HasValue) r["OnInvoiceType"] = Tax.OnInvoiceType;
                if (Tax.OnReceiptType.HasValue) r["OnReceiptType"] = Tax.OnReceiptType;
                if (Tax.OnDocCreditType.HasValue) r["OnDocCreditType"] = Tax.OnDocCreditType;
                r["SalesAccountID"] = Tax.SalesAccountID;
                r["PurchaseAccountID"] = Tax.PurchaseAccountID;
            }
            this.dtTaxes.Rows.Add(r);
            this.Calculate();
            gvTaxes.DataSource = this.dtTaxes;
            gvTaxes.DataBind();
            acTax.Clear();
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
            dc.usp_SalesOrder_Cancel(this.Invoice_ID, MyContext.UserProfile.Contact_ID);
            LogAction(Actions.Delete, txtSerial.Text, dc);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.SalesOrder + "?ID=" + this.Invoice_ID.ToExpressString(), PageLinks.SalesOrdersList + Request.PathInfo, PageLinks.SalesOrder);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtFirstPaid_TextChanged(object sender, EventArgs e)
    {
        try
        {
            acCashAccount.IsRequired = (txtFirstPaid.Text.ToDecimalOrDefault() != 0);
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtPercentageDiscount_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtPercentageDiscount.Text.ToDecimalOrDefault() > 100) txtPercentageDiscount.Text = "0";
            this.Calculate();
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnFastAddNew_click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {
            var company = dc.usp_Company_Select().FirstOrDefault();

            int Contact_ID = dc.usp_Contact_Insert(MyContext.UserProfile.Branch_ID, ddlFastAddCurrency.SelectedValue.ToInt(), DocSerials.Customer.ToInt(), txtFastAddName.TrimmedText, 'C', string.Empty, null);
            int ChartofAccount_ID = dc.usp_ChartOfAccount_Insert(txtFastAddName.TrimmedText, txtFastAddName.TrimmedText, acParentAccount.Value.ToInt(), true, MyContext.UserProfile.Branch_ID, ddlFastAddCurrency.SelectedValue.ToInt(), null, null, null, null);

            if (ChartofAccount_ID == -2 || Contact_ID == -2)
            {
                UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                trans.Rollback();
                mpeFastAddNew.Show();
                return;
            }
            dc.usp_Customers_Insert(Contact_ID, ChartofAccount_ID, acArea.Value.ToNullableInt(), null, company.UseCustomerCreditLimit, company.CustomerCreditLimit);

            trans.Commit();
            if (acCustomer.Enabled) acCustomer.Value = Contact_ID.ToExpressString();
            LogAction(Actions.Add, "اضافة عميل سريع: " + txtFastAddName.TrimmedText, dc);
            this.CloseFastAddNewPopup_Click(null, null);
            this.FocusNextControl(lnkAddNewCustomer);
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void CloseFastAddNewPopup_Click(object sender, EventArgs e)
    {
        try
        {
            txtFastAddName.Clear();
            acArea.Clear();
            mpeFastAddNew.Hide();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Report_Dev/PrintOrderInnoiceeDev.aspx?Invoice_ID=" + this.Invoice_ID + "&IsMaterla=1", false);

            //ReportDocument doc = new ReportDocument();
            //doc.Load(Server.MapPath("~\\Reports\\Invoice_Print ssb.rpt"));
            //doc.SetParameterValue("@Invoice_ID", this.Invoice_ID);

            //doc.SetParameterValue("BaseDir", HttpContext.Current.Server.MapPath("~\\Uploads\\"));
            //doc.SetParameterValue("ByEmp", ((UICulturePage)HttpContext.Current.Handler).MyContext.UserProfile.EmployeeName);
            //doc.SetParameterValue("@Branch_ID", acBranch.Value.ToNullableInt());

            ////ReportDocument doc = new ReportDocument();
            ////doc.Load(Server.MapPath("~\\Reports\\Invoice_Print.rpt"));
            ////doc.SetParameterValue("@Invoice_ID", this.Invoice_ID);
            ////doc.SetParameterValue("@DocTableType_ID", DocumentsTableTypes.Invoice.ToInt(), "DocumentTaxes.rpt");
            ////doc.SetParameterValue("@Doc_ID", this.Invoice_ID, "DocumentTaxes.rpt");
            ////doc.SetParameterValue("@IsReturn", false, "DocumentTaxes.rpt");
            //Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "SalesOrder"), false);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnPrintOrderOut_Click(object sender, EventArgs e)
    {
        try
        {
            ReportDocument doc = new ReportDocument();
            doc.Load(Server.MapPath("~\\Reports\\InvOrderOut_Print.rpt"));
            doc.SetParameterValue("@Invoice_ID", this.Invoice_ID);
            Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "OrderOut"), false);
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
            acCustomer.ContextKey = "C," + acBranch.Value + "," + ddlCurrency.SelectedValue + ",";
            acCashAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + acBranch.Value + "," + ddlCurrency.SelectedValue + "," + COA.CashOnHand.ToInt().ToExpressString() + ",true";
            acStore.ContextKey = string.Empty + acBranch.Value;
            acCostCenter.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",false," + acBranch.Value;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private void FilterItems()
    {
        acItem.ContextKey = ",,";
        acItem.Clear();
        this.FilterItemsData();
    }
    private void FilterItemsDescribed()
    {
        acItemDescribed.ContextKey = "," + acCategory.Value + "!" + acItem.Value + ",,true";
        acItemDescribed.Clear();
        this.FilterItemsData();
    }
    private void FilterItemsData()
    {
        txtCost.Clear();
        txtItemPercentageDiscount.Clear();
        txtItemCashDiscount.Clear();
        txtProductionDate.Clear();
        txtExpirationDate.Clear();
        acBatchID.Clear();
        acItemTax.Clear();
        acUnit.Clear();
        acPriceName.Clear();
        txtBarcode.Clear();
        txtItemNotes.Clear();
        txtQty.Clear();
        txtQtyInNumber.Clear();
        // txtCItem.Clear();

        acBatchID.ContextKey = acItem.Value + "," + acStore.Value + "," + acUnit.Value + ",True";
        acUnit.ContextKey = string.Empty + acItem.Value;
        acPriceName.ContextKey = string.Empty + acItem.Value;
        if (acItem.HasValue)
        {
            var item = dc.usp_Items_SelectByID(acItem.Value.ToNullableInt()).FirstOrDefault();
            txtCost.Text = (item.DefaultPrice.Value / (txtRatio.Text.ToNullableDecimal() ?? 1)).ToString("0.####");
            txtItemPercentageDiscount.Text = item.DiscountPercentage.Value.ToExpressString();
            txtBarcode.Text = item.Barcode;
            if (item.Tax_ID.HasValue) acItemTax.Value = item.Tax_ID.Value.ToExpressString();
            acUnit.Value = item.UOM_ID.Value.ToExpressString();
            acCategory.Value = item.Category_ID.ToExpressString();
            txtCItem.Text = item.CodeItem.ToString();
        }
    }

    private void LoadControls()
    {
        this.dtItems = null;
        this.dtTaxes = null;
        acBranch.ContextKey = string.Empty;
        if (this.MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = this.MyContext.UserProfile.Branch_ID.ToExpressString();
            acBranch.Enabled = false;
        }
        var currency = dc.usp_Currency_Select(false).ToList();
        ddlCurrency.DataSource = currency;
        ddlCurrency.DataTextField = "Name";
        ddlCurrency.DataValueField = "ID";
        ddlCurrency.DataBind();

        ddlFastAddCurrency.DataSource = currency;
        ddlFastAddCurrency.DataTextField = "Name";
        ddlFastAddCurrency.DataValueField = "ID";
        ddlFastAddCurrency.DataBind();

        this.FilterByBranchAndCurrency();
        acCategory.ContextKey = string.Empty;
        acTax.ContextKey = acItemTax.ContextKey = DocumentsTableTypes.Invoice.ToInt().ToExpressString() + ",false";
        this.FilterItems();
        this.BindItemsGrid();
        lblCreatedBy.Text = MyContext.UserProfile.EmployeeName;
        gvTaxes.DataSource = this.dtTaxes;
        gvTaxes.DataBind();

        acArea.ContextKey = string.Empty;
        acParentAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString();
        acParentAccount.Value = COA.Customers.ToInt().ToExpressString();

    }

    private void BindItemsGrid()
    {
        this.Calculate();
        gvItems.DataSource = this.dtItems;
        gvItems.DataBind();
        acBranch.Enabled = (gvItems.Rows.Count == 0) && (this.MyContext.UserProfile.Branch_ID == null);
    }

    private void ClearItemForm()
    {
        //acStore.Clear();
        if (MyContext.UserProfile.Store_ID != null) acStore.Value = MyContext.UserProfile.Store_ID.ToStringOrEmpty();
        acCategory.Clear();
        this.FilterItems(); txtCItem.Clear();
        this.acItem_SelectedIndexChanged(null, null);
    }

    private void SetEditMode()
    {
        if (Request["ID"] != null)
        {
            this.EditMode = true;
            this.Invoice_ID = Request["ID"].ToInt();
        }
    }

    private bool Save(bool IsApproving, System.Data.Common.DbTransaction trans)
    {
        if (dtItems.AsEnumerable().Where(r => r.RowState != DataRowState.Deleted).Count() == 0)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.OneItemRequired, string.Empty);
            trans.Rollback();
            return false;
        }
        if (this.Total <= 0 || this.GrossTotal < 0)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.TotalIsNotValid, string.Empty);
            trans.Rollback();
            return false;
        }

        if (txtFirstPaid.Text.ToDecimalOrDefault() > this.GrossTotal)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.FirstPaidGreaterThanTotal, string.Empty);
            trans.Rollback();
            return false;
        }

        string Serial = string.Empty;
        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        DateTime? ApproveDate = IsApproving ? DateTime.Now : (DateTime?)null;
        int? approvedBY_ID = IsApproving ? MyContext.UserProfile.Contact_ID : (int?)null;
        byte EntryType = 0;
        int Serial_ID = DocSerials.SalesOrder.ToInt();
        int? SalesOrderID = null;
        if (!this.EditMode)
        {
            //TODO INVOICE contact Mesure
            this.Invoice_ID = dc.usp_Invoice_Insert(acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimalOrDefault(), txtOperationDate.Text.ToDate(),
                                                     acCustomer.Value.ToInt(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate,
                                                     approvedBY_ID, null, null, acAddress.Value.ToNullableInt(), acShipAddress.Value.ToNullableInt(), acPaymentAddress.Value.ToNullableInt(),
                                                     acTelephone.Value.ToNullableInt(), acCostCenter.Value.ToNullableInt(), txtNotes.Text, lblTotal.Text.ToDecimal(), txtPercentageDiscount.Text.ToDecimalOrDefault(),
                                                     txtCashDiscount.Text.ToDecimalOrDefault(), txtAdditionals.Text.ToDecimalOrDefault(), lblTotalDiscount.Text.ToDecimalOrDefault(), lblTotalTax.Text.ToDecimalOrDefault(), lblGrossTotal.Text.ToDecimalOrDefault(), txtFirstPaid.Text.ToDecimalOrDefault(), null, txtUserRefNo.Text, this.DocRandomString, EntryType, SalesOrderID, acSalesRep.Value.ToNullableInt(), acCashAccount.Value.ToNullableInt(), null,null);
            if (this.Invoice_ID > 0)
            {
                foreach (DataRow r in this.dtItems.Rows)
                {
                    if (r.RowState == DataRowState.Deleted) continue;
                    // dc.usp_InvoiceDetails_Insert(this.Invoice_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(),"");
                    //dc.usp_InvoiceDetailsWithDesciption_Insert(this.Invoice_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), "", r["ItemDescription"].ToString(), "", "", null, "",false,false,null,false,false);
                    dc.usp_InvoiceDetailsIncludeTax_Insert(this.Invoice_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["Capacity"].ToString(), r["ItemDescription"].ToString(), r["IDCodeOperation"].ToString(), r["Policy"].ToString(), r["InvoiceDate"].ToDate(), r["Capacities"].ToString(), r["IsGift"].ToBooleanOrDefault(), r["UnitCostEvaluate"].ToDecimalOrDefault(),
                        false,false,false);

                }
                foreach (DataRow r in this.dtTaxes.Rows)
                {
                    if (r.RowState == DataRowState.Deleted) continue;
                    dc.usp_DocuemntTaxes_Insert(DocumentsTableTypes.Invoice.ToInt(), this.Invoice_ID, r["Tax_ID"].ToInt());
                }
                LogAction(IsApproving ? Actions.Approve : Actions.Add, Serial, dc);
            }
        }
        else
        {
            //TODO Order
            //Here Test  for Order Save or not 
            var lst = dc.usp_Invoice_Select(acBranch.Value.ToNullableInt(), null, "",
                acCustomer.Value.ToInt(), null, null, "", 1, null, null, 0,-1,null).ToList();
            var lstById = lst.Where(x => x.ID < this.Invoice_ID).ToList();
            if (lstById.Count == 0 || dc.usp_Company_Select().First().IsOrder == false)
            {
                int Result = dc.usp_Invoice_Update(this.Invoice_ID, acBranch.Value.ToNullableInt(),
                       ddlCurrency.SelectedValue.ToInt(),
                       txtRatio.Text.ToDecimalOrDefault(),
                       txtOperationDate.Text.ToDate(),
                       acCustomer.Value.ToInt(), ref Serial, Serial_ID, DocStatus_ID,
                       DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate, approvedBY_ID,
                       null, null, acAddress.Value.ToNullableInt(), acShipAddress.Value.ToNullableInt(),
                       acPaymentAddress.Value.ToNullableInt(), acTelephone.Value.ToNullableInt(),
                       acCostCenter.Value.ToNullableInt(), txtNotes.Text, lblTotal.Text.ToDecimal(),
                       txtPercentageDiscount.Text.ToDecimalOrDefault(), txtCashDiscount.Text.ToDecimalOrDefault(),
                       txtAdditionals.Text.ToDecimalOrDefault(),
                       lblTotalDiscount.Text.ToDecimalOrDefault(), lblTotalTax.Text.ToDecimalOrDefault(),
                       lblGrossTotal.Text.ToDecimalOrDefault(), txtFirstPaid.Text.ToDecimalOrDefault(),
                       txtUserRefNo.Text, this.DocRandomString, acSalesRep.Value.ToNullableInt(), acCashAccount.Value.ToNullableInt(),null);


                if (Result > 0)
                {
                    foreach (DataRow r in this.dtItems.Rows)
                    {
                        if (r.RowState == DataRowState.Added)
                        {
                            // dc.usp_InvoiceDetails_Insert(this.Invoice_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["Capacity"].ToString());
                            dc.usp_InvoiceDetailsWithDesciption_Insert(this.Invoice_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["Capacity"].ToString(), r["ItemDescription"].ToString(), "", "", null, "", false, false, false, false);

                        }
                        if (r.RowState == DataRowState.Modified)
                        {
                            //dc.usp_InvoiceDetails_Update(r["ID"].ToInt(), r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(),false);
                            dc.usp_InvoiceDetailsIncludeTax_Update(r["ID"].ToInt(), r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["IsGift"].ToBooleanOrDefault(), r["UnitCostEvaluate"].ToDecimalOrDefault(),
                                false,null,null);
                        }
                        if (r.RowState == DataRowState.Deleted)
                        {
                            dc.usp_InvoiceDetails_Delete(r["ID", DataRowVersion.Original].ToInt());
                        }
                    }

                    foreach (DataRow r in this.dtTaxes.Rows)
                    {
                        if (r.RowState == DataRowState.Added)
                        {
                            dc.usp_DocuemntTaxes_Insert(DocumentsTableTypes.Invoice.ToInt(), this.Invoice_ID, r["Tax_ID"].ToInt());
                        }
                        if (r.RowState == DataRowState.Deleted)
                        {
                            dc.usp_DocuemntTaxes_Delete(r["ID", DataRowVersion.Original].ToInt());
                        }
                    }
                    LogAction(IsApproving ? Actions.Approve : Actions.Edit, txtSerial.Text, dc);
                }



            }
            else
            {
                UserMessages.Message(null, Resources.Labels.ThereAreOrdersThatMustBeApproved, PageLinks.SalesOrder);
                return false;
            }



        } Serial = Serial == string.Empty ? string.Empty : " (" + Serial + ") ";
        UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess + Serial, PageLinks.SalesOrder + "?ID=" + this.Invoice_ID.ToExpressString(), PageLinks.SalesOrdersList, PageLinks.SalesOrder);
        return true;

    }

    private void FillInvoice()
    {
        var company = dc.usp_Company_Select().First();
        var invoice = dc.usp_Invoice_SelectByID(this.Invoice_ID).FirstOrDefault();
        ddlCurrency.SelectedValue = invoice.Currency_ID.ToExpressString();
        txtRatio.Text = invoice.Ratio.ToExpressString();
        acBranch.Value = invoice.Branch_ID.ToStringOrEmpty();
        this.FilterByBranchAndCurrency();
        txtOperationDate.Text = invoice.OperationDate.Value.ToString("d/M/yyyy");
        acCostCenter.Value = invoice.CostCenter_ID.ToStringOrEmpty();
        txtUserRefNo.Text = invoice.UserRefNo;
        acCustomer.Value = invoice.Contact_ID.ToExpressString();
        this.acCustomer_SelectedIndexChanged(null, null);
        acTelephone.Value = invoice.Telephone_ID.ToStringOrEmpty();
        acAddress.Value = invoice.DefaultAddress_ID.ToStringOrEmpty();
        acShipAddress.Value = invoice.ShipToAddress_ID.ToStringOrEmpty();
        acPaymentAddress.Value = invoice.PaymentAddress_ID.ToStringOrEmpty();
        acCashAccount.Value = invoice.CashAccount_ID.ToStringOrEmpty();
        acSalesRep.Value = invoice.SalesRep_ID.ToStringOrEmpty();
        txtCashDiscount.Text = invoice.CashDiscount.ToExpressString();
        txtAdditionals.Text = invoice.Additionals.ToExpressString();
        txtPercentageDiscount.Text = invoice.PercentageDiscount.ToExpressString();
        txtFirstPaid.Text = invoice.FirstPaid.ToExpressString();
        this.txtFirstPaid_TextChanged(null, null);
        lblTotal.Text = invoice.TotalAmount.ToExpressString();
        lblTotalDiscount.Text = invoice.TotalDiscount.ToExpressString();
        lblTotalTax.Text = invoice.TotalTax.ToExpressString();
        lblGrossTotal.Text = invoice.GrossTotalAmount.ToExpressString();
        txtNotes.Text = invoice.Notes;
        txtSerial.Text = invoice.Serial;
        ucNav.SetText = invoice.Serial;
        this.DocRandomString = invoice.DocRandomString;
        lblCreatedBy.Text = invoice.CreatedByName;
        lblApprovedBy.Text = invoice.ApprovedBYName;
        lblSalesOrderNo.Text = invoice.SalesOrderSerial;
        divSalesOrderNo.Visible = !string.IsNullOrEmpty(invoice.SalesOrderSerial);
        this.DocStatus_ID = invoice.DocStatus_ID.Value;
        this.ImgStatus = ((DocStatus)invoice.DocStatus_ID).ToExpressString();
        btnPrint.Visible = MyContext.PageData.IsPrint;
        btnPrintInventoryOrder.Visible = MyContext.PageData.IsPrint;
        pnlAddItem.Visible = (invoice.DocStatus_ID == 1);
        btnCancel.Visible = ((invoice.DocStatus_ID == 1) || (invoice.DocStatus_ID == 2 && company.ReserveQty.Value && !invoice.OrderHasInv.Value)) && MyContext.PageData.IsDelete;
        btnApprove.Visible = (invoice.DocStatus_ID == 1) && MyContext.PageData.IsApprove;
        btnSave.Visible = (invoice.DocStatus_ID == 1) && ((MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode));
        //btnPrint.Visible = MyContext.PrintAfterApprove || btnApprove.Visible;
        
        
       // gvItems.Columns[gvItems.Columns.Count - 1].Visible = gvItems.Columns[gvItems.Columns.Count - 2].Visible = (invoice.DocStatus_ID == 1);
        //gvTaxes.Columns[gvTaxes.Columns.Count - 1].Visible = (invoice.DocStatus_ID == 1);


        btnCancelApprove.Visible = !btnApprove.Visible && (invoice.DocStatus_ID == 2) && MyContext.PageData.IsNotApprove;


        //this.dtItems = dc.usp_InvoiceDetails_Select(this.Invoice_ID).CopyToDataTable();
        //  this.dtItems = dc.usp_InvoiceDetailsWithDescription_Select(this.Invoice_ID).CopyToDataTable();

        var lstItems = dc.usp_InvoiceDetailsWithDescription_Select(this.Invoice_ID).ToList();
        this.dtItems = lstItems.CopyToDataTable();
        if (lstItems.Count() > 0)
        {
            acStore.Value = lstItems.FirstOrDefault().Store_ID.ToExpressString();
        }




        this.dtTaxes = dc.usp_DocuemntTaxes_Select(DocumentsTableTypes.Invoice.ToInt(), this.Invoice_ID, false).CopyToDataTable();
        this.BindItemsGrid();
        gvTaxes.DataSource = this.dtTaxes;
        gvTaxes.DataBind();

    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        btnSave.Visible = (MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode);
        btnApprove.Visible = MyContext.PageData.IsApprove;
        btnCancel.Visible = MyContext.PageData.IsDelete && EditMode;
        btnCancelApprove.Visible = !btnApprove.Visible && MyContext.PageData.IsNotApprove;
        btnPrint.Visible = MyContext.PrintAfterApprove || btnApprove.Visible;
        MyContext con = new MyContext(System.Web.Security.Membership.GetUser(), PageLinks.Customers, string.Empty);
        btnFastAddNew.Visible = con.PageData.IsAdd;


        lnkGetPriceIteme.Visible = MyContext.PageData.IsViewPriceIteme;
        lnkGroupStoreIteme.Visible = MyContext.PageData.ViewBalanceIteme;


    }

    private void Calculate()
    {
        decimal ItemDiscount = 0;
        decimal DocDiscount = 0;
        decimal DocTax = 0;
        decimal Additionals = 0;
        this.Total = 0;
        this.GrossTotal = 0;
        this.TotalDiscount = 0;
        this.TotalTax = 0;
        foreach (DataRow r in this.dtItems.Rows)
        {
            ItemDiscount = 0;
            if (r.RowState == DataRowState.Deleted) continue;
            r["Total"] = r["UnitCost"].ToDecimal() * r["Quantity"].ToDecimal();
            Total += r["UnitCost"].ToDecimal() * r["Quantity"].ToDecimal();
            this.TotalDiscount += ItemDiscount = (r["PercentageDiscount"].ToDecimal() * r["Total"].ToDecimal() * 0.01m) + r["CashDiscount"].ToDecimal();
            if (r["TaxOnInvoiceType"] != DBNull.Value && r["Tax_ID"] != DBNull.Value)
            {
                r["TotalTax"] = ((r["Total"].ToDecimal() - ItemDiscount) * r["TaxPercentageValue"].ToDecimal() * 0.01m) * (r["TaxOnInvoiceType"].ToExpressString() == "C" ? 1 : -1);
                this.TotalTax += r["TotalTax"].ToDecimal();
            }
            r["GrossTotal"] = r["Total"].ToDecimal() - ItemDiscount + r["TotalTax"].ToDecimal();
            GrossTotal += r["Total"].ToDecimal() - ItemDiscount + r["TotalTax"].ToDecimal();
        }

        DocDiscount = (this.GrossTotal * txtPercentageDiscount.Text.ToDecimalOrDefault() * 0.01m) + txtCashDiscount.Text.ToDecimalOrDefault();
        foreach (DataRow r in this.dtTaxes.Rows)
        {
            if (r.RowState == DataRowState.Deleted) continue;
            if (r["OnInvoiceType"] != DBNull.Value)
            {
                DocTax += ((this.GrossTotal - DocDiscount) * r["PercentageValue"].ToDecimal() * 0.01m * (r["OnInvoiceType"].ToExpressString() == "C" ? 1 : -1));
            }
        }
        Additionals = txtAdditionals.Text.ToDecimalOrDefault();
        this.TotalDiscount += DocDiscount;
        this.TotalTax += DocTax;
        this.GrossTotal = this.GrossTotal - DocDiscount + DocTax + Additionals;
        lblTotal.Text = this.Total.ToString("0.####");
        lblTotalDiscount.Text = this.TotalDiscount.ToString("0.####");
        lblAdditionals.Text = Additionals.ToString("0.####");
        lblTotalTax.Text = this.TotalTax.ToString("0.####");
        lblGrossTotal.Text = this.GrossTotal.ToString("0.####");
        if (acCashAccount.HasValue && Page.IsPostBack && this.SumFirstPaid)
        {
            txtFirstPaid.Text = lblGrossTotal.Text;
            this.txtFirstPaid_TextChanged(null, null);
        }
    }

    private void ShowAvailableQty()
    {
        lblAvailableQty.Text = string.Empty;
        if (acItem.HasValue)
        {
            decimal Qty = dc.fun_ItemQty(acItem.Value.ToInt(), acStore.Value.ToNullableInt(), acBatchID.Value.ToNullableInt(), acUnit.Value.ToNullableInt(), null).Value;
            lblAvailableQty.Text = Qty.ToExpressString();
            lblAvailableQty.ForeColor = Qty <= 0 ? System.Drawing.Color.Red : System.Drawing.Color.Green;
        }
    }

    private void ShowCustomerLastItemPrice()
    {
        lblLastCustomerPrice.Text = (acCustomer.HasValue && acItem.HasValue) ? dc.fun_CustomerLastItemPrice(acCustomer.Value.ToNullableInt(), acItem.Value.ToNullableInt()) : "0.0000";
    }

    private void CustomPage()
    {
        acCostCenter.Visible = MyContext.Features.CostCentersEnabled;
        acBatchID.Visible = MyContext.Features.BatchIDEnabled;
        txtProductionDate.Visible = MyContext.Features.BatchIDEnabled;
        txtExpirationDate.Visible = MyContext.Features.BatchIDEnabled;
        acItemTax.Visible = MyContext.Features.TaxesEnabled;
        taxSection.Visible = MyContext.Features.TaxesEnabled;
        taxTotal.Visible = MyContext.Features.TaxesEnabled;
        DiscountTotal.Visible = MyContext.Features.CashDiscountEnabled || MyContext.Features.PercentageDiscountEnabled;
        acBranch.IsRequired = acBranch.Visible = MyContext.Features.BranchesEnabled;
        txtCashDiscount.Visible = txtItemCashDiscount.Visible = MyContext.Features.CashDiscountEnabled;
        txtPercentageDiscount.Visible = txtItemPercentageDiscount.Visible = MyContext.Features.PercentageDiscountEnabled;

        acSalesRep.Visible = MyContext.Features.SalesRepEnabled;

        foreach (DataControlField col in gvItems.Columns)
        {
            if (col.ItemStyle.CssClass == "BatchCol") col.Visible = MyContext.Features.BatchIDEnabled;
            if (col.ItemStyle.CssClass == "TaxCol") col.Visible = MyContext.Features.TaxesEnabled;
        }
    }

    private void SetDefaults()
    {
        if (Page.IsPostBack) return;
        var company = dc.usp_Company_Select().FirstOrDefault();
        var defaults = dc.usp_CashBillDefaults_Select(acBranch.Value.ToNullableInt(), MyContext.UserProfile.Contact_ID).First();

        if (company.AutoDate.Value) txtOperationDate.Text = DateTime.Now.Date.ToString("d/M/yyyy");
        if (txtOperationDate.IsNotEmpty) this.ddlCurrency_SelectedIndexChanged(null, null);

        if (defaults.UserHasCashAccount.Value && defaults.DefaultCashAccount_ID.HasValue) acCashAccount.Value = defaults.DefaultCashAccount_ID.ToExpressString();
        if (defaults.UserHasStore.Value && defaults.DefaultStore_ID.HasValue) acStore.Value = defaults.DefaultStore_ID.ToExpressString();
        if (acStore.HasValue) this.acStore_SelectedIndexChanged(null, null);

        txtOperationDate.Enabled = !company.LockAutoDate.Value;
        acCashAccount.Enabled = !((MyContext.UserProfile.CashierAccount_ID != null) && ((MyContext.Features.BranchesEnabled && MyContext.UserProfile.Branch_ID != null) || !MyContext.Features.BranchesEnabled));
        acStore.Enabled = !((MyContext.UserProfile.Store_ID != null) && ((MyContext.Features.BranchesEnabled && MyContext.UserProfile.Branch_ID != null) || !MyContext.Features.BranchesEnabled));
    }

    private void CalculateTotalRow()
    { 
        var Tax = dc.usp_Taxes_Select(acItemTax.Value.ToInt(), string.Empty).FirstOrDefault();
        decimal taxInclude = TypeTax == 2 ? (Decimal.Divide((Tax != null ? Tax.PercentageValue.Value : 0), (100M + (Tax != null ? Tax.PercentageValue.Value : 0)))) : Decimal.Divide((Tax != null ? Tax.PercentageValue.Value : 0), (100M));
        decimal unitCost = TypeTax == 2 ? (txtCost.Text.ToDecimalOrDefault() - txtCost.Text.ToDecimalOrDefault() * taxInclude) : txtCost.Text.ToDecimalOrDefault();
        var totalPrice = txtQty.Text.ToDecimalOrDefault() * unitCost;
        var calc = (totalPrice - txtItemCashDiscount.Text.ToDecimalOrDefault() - txtItemPercentageDiscount.Text.ToDecimalOrDefault() * (totalPrice) / 100);
        decimal val1 = calc * (Tax != null ? Tax.PercentageValue.Value : 0) / 100;
        lblTotalRow.Text = Math.Round((string.IsNullOrEmpty(acItemTax.Value) ? calc : (calc + val1)), 2).ToExpressString();
        lblTotalRowBeforTax.Text = Math.Round(calc, 3).ToExpressString();
    }

    #endregion

    protected void ucNewItemDescribed_OnNewItemDescribedCreated(string itemdescribedid, string price, int attid)
    {
        try
        {
            acItemDescribed.Refresh();
            acItemDescribed.Value = itemdescribedid.ToExpressString();
            this.FocusNextControl(lnkNewDescribed);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }

    }


    protected void txtCapacities_OnTextChanged(object sender, EventArgs e)
    {
        var lst = txtCapacities.Text.Split('-');
        decimal sum = 0;
        foreach (string t in lst)
        {

            var value = t;
            decimal number = 0;
            if (Decimal.TryParse(value, out number))
                sum += number;
        }
        lblQTyterminal.Text = sum.ToString();
        txtCapacity.Text = txtCapacities.Text.Split('-').Length.ToString();
    }

    private void FillItemeList_price()
    {

        var lstInvoices1 = dc.usp_GetItemPrice_Select(acItem.Value.ToInt()).ToList();
        this.dtItemePrice = lstInvoices1.CopyToDataTable();
        gvPriceList1.DataSource = this.dtItemePrice;
        gvPriceList1.DataBind();
    }
    protected void ClosePopup_Click(object sender, EventArgs e)
    {
        try
        {
            mpeCollect.Hide();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private void FillQtyStoreGroupList()
    {

        var lstQtyList = dc.usp_GetQtyItemeStoreGroup_Select(acItem.Value.ToInt()).ToList();
        this.dtQtyItemeStoreGroup = lstQtyList.CopyToDataTable();
        gvQtyStoreList1.DataSource = this.dtQtyItemeStoreGroup;
        gvQtyStoreList1.DataBind();
    }
    protected void lnkGetPriceIteme_Click(object sender, EventArgs e)
    {
        FillItemeList_price();
    }
    protected void lnkGroupIteme_Click(object sender, EventArgs e)
    {
        FillQtyStoreGroupList();
    }
    protected void Button6_Click(object sender, EventArgs e)
    {
        try
        {
            mpeGQiteme.Hide();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnCancelApprove_Click1(object sender, EventArgs e)
    {
        try
        {
            dc.usp_CancelInvoice_Approvel(this.Invoice_ID);
            //trans.Commit();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.InvoiceShortcut + "?ID=" + this.Invoice_ID.ToExpressString(), PageLinks.InvoicesList, PageLinks.InvoiceShortcut);

        }
        catch (Exception ex)
        {
            // trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    //protected void txtCodeItem_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        acItem.Clear();
    //        if (txtCItem.IsNotEmpty)
    //        {
    //            var item = dc.usp_ItemsCode_Select(txtCItem.TrimmedText, string.Empty, null, null, null, true).FirstOrDefault();
    //            if (item != null) acItem.Value = item.ID.ToExpressString();
    //        }
    //        this.acItem_SelectedIndexChanged(null, null);
    //        if (acItem.HasValue) txtQty.Focus(); else this.FocusNextControl(acStore);
    //    }
    //    catch (Exception ex)
    //    {
    //        Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
    //    }
    //}
    protected void txtQty_TextChanged(object sender, EventArgs e)
    {
        this.CalculateTotalRow();
    }
    protected void txtCost_TextChanged(object sender, EventArgs e)
    {
        this.CalculateTotalRow();
    }
}