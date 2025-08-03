using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.DynamicData;
using XPRESS.Common;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class Sales_Invoice : UICulturePage
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
            if (Session["dtItems_Invoice" + this.WinID] == null)
            {
                // Session["dtItems_Invoice" + this.WinID] = dc.usp_InvoiceDetails_Select(null).CopyToDataTable();
                Session["dtItems_Invoice" + this.WinID] = dc.usp_InvoiceDetailsWithDescription_Select(null).CopyToDataTable();
            }
            return (DataTable)Session["dtItems_Invoice" + this.WinID];
        }

        set
        {
            Session["dtItems_Invoice" + this.WinID] = value;
        }
    }

    private DataTable dtTaxes
    {
        get
        {
            if (Session["dtTaxes_Invoice" + this.WinID] == null)
            {
                Session["dtTaxes_Invoice" + this.WinID] = dc.usp_DocuemntTaxes_Select(null, null, false).CopyToDataTable();
            }
            return (DataTable)Session["dtTaxes_Invoice" + this.WinID];
        }

        set
        {
            Session["dtTaxes_Invoice" + this.WinID] = value;
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

    private int SalesOrderID
    {
        get
        {
            if (ViewState["SalesOrderID"] == null) return 0;
            return (int)ViewState["SalesOrderID"];
        }

        set
        {
            ViewState["SalesOrderID"] = value;
        }
    }

    private decimal TotalDebitTax
    {
        get
        {
            if (ViewState["TotalDebitTax"] == null) return 0;
            return (decimal)ViewState["TotalDebitTax"];
        }

        set
        {
            ViewState["TotalDebitTax"] = value;
        }
    }

    private decimal TotalCreditTax
    {
        get
        {
            if (ViewState["TotalCreditTax"] == null) return 0;
            return (decimal)ViewState["TotalCreditTax"];
        }

        set
        {
            ViewState["TotalCreditTax"] = value;
        }
    }

    private DataTable dtAllTaxes
    {
        get
        {
            if (Session["dtAllTaxes_Invoice" + this.WinID] == null)
            {
                DataTable dtallTaxes = new DataTable();
                dtallTaxes.Columns.Add("Account_ID", typeof(int));
                dtallTaxes.Columns.Add("OnInvoiceType", typeof(string));
                dtallTaxes.Columns.Add("DebitAmount", typeof(decimal));
                dtallTaxes.Columns.Add("CreditAmount", typeof(decimal));
                Session["dtAllTaxes_Invoice" + this.WinID] = dtallTaxes;
            }
            return (DataTable)Session["dtAllTaxes_Invoice" + this.WinID];
        }

        set
        {
            Session["dtAllTaxes_Invoice" + this.WinID] = value;
        }
    }

    private bool ConfirmationAnswered
    {
        get
        {
            if (ViewState["ConfirmationAnswered"] == null) return false;
            return (bool)ViewState["ConfirmationAnswered"];
        }

        set
        {
            ViewState["ConfirmationAnswered"] = value;
        }
    }

    private string ConfirmationMessage
    {
        get
        {
            if (ViewState["ConfirmationMessage"] == null)
            {
                ViewState["ConfirmationMessage"] = string.Empty;
            }
            return (string)ViewState["ConfirmationMessage"];
        }

        set
        {
            ViewState["ConfirmationMessage"] = value;
        }
    }

    private byte QuantityWarning
    {
        get
        {
            if (ViewState["QuantityWarning"] == null) return 0;
            return (byte)ViewState["QuantityWarning"];
        }

        set
        {
            ViewState["QuantityWarning"] = value;
        }
    }

    private decimal CalculatedSalesCost
    {
        get
        {
            if (ViewState["CalculatedSalesCost"] == null) return 0;
            return (decimal)ViewState["CalculatedSalesCost"];
        }

        set
        {
            ViewState["CalculatedSalesCost"] = value;
        }
    }

    private byte DocStatus_ID
    {
        get
        {
            if (ViewState["DocStatus_ID"] == null) return 0;
            return (byte)ViewState["DocStatus_ID"];
        }

        set
        {
            ViewState["DocStatus_ID"] = value;
        }
    }

    private bool IsCashInvoice
    {
        get
        {
            return Request.PathInfo == "/Cash";
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
                this.FillInvoice();
                this.VisibilityControl();
                this.txtQty.Text = "1";
                IsRequiresField();

            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private void VisibilityControl()
    {
        var obj = dc.usp_Company_Select().First();
        if (obj.IsDescribed != null)
        {
            // txtDescribed.Visible = obj.IsDescribed.Value;
            pnlItemdescribed.Visible = obj.IsDescribed.Value;
            //acItemDescribed.Visible =;
            acItemDescribed.IsRequired = obj.IsDescribed.Value;
        }
    }
    private void IsRequiresField()
    {
        foreach (var control in dc.usp_HiddenControls_Select(MyContext.PageData.PageID, MyContext.UserProfile.Contact_ID))
        {
            if (control.ControlUniqueID == "cph_txtPolicy")
            {
                txtPolicy.IsRequired = false;
            }
            if (control.ControlUniqueID == "cph_txtCode")
            {
                txtCode.IsRequired = false;
            }
            if (control.ControlUniqueID == "cph_txtInvoiceDate")
            {
                txtInvoiceDate.IsRequired = false;
            }
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        this.CustomPage();
        IsRequiresField();
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
            if (sender != null) this.ShowCustomerBalance();
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
            this.ShowCustomerBalance();
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
            if (sender != null) txtCost.Text = (dc.fun_GetItemDefaultPriceByUOM(acItem.Value.ToNullableInt(), acUnit.Value.ToNullableInt(), acPriceName.Value.ToNullableInt()) / (txtRatio.Text.ToNullableDecimal() ?? 1)).Value.ToString(NbrHashNeerDecimal);
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
            if (sender != null) txtCost.Text = (dc.fun_GetItemDefaultPriceByUOM(acItem.Value.ToNullableInt(), acUnit.Value.ToNullableInt(), acPriceName.Value.ToNullableInt()) / (txtRatio.Text.ToNullableDecimal() ?? 1)).Value.ToString(NbrHashNeerDecimal);
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
                if (!string.IsNullOrEmpty(txtCode.Text))
                {
                    //testing if code already exist or not
                    DataRow[] filteredRows = this.dtItems.Select("IDCodeOperation='" + txtCode.Text + "'");
                    if (filteredRows.Length > 0)
                    {
                        UserMessages.Message(null, Resources.UserInfoMessages.CodeAlreadyExist, string.Empty);
                        txtCode.Focus();
                        return;
                    }

                    var countCode = dc.InvoiceDetails.Count(x => x.IDCodeOperation == txtCode.Text);
                    if (countCode > 0)
                    {
                        UserMessages.Message(null, Resources.UserInfoMessages.CodeAlreadyExist, string.Empty);
                        txtCode.Focus();
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(txtPolicy.Text))
                {
                    //testing if Policy already exist or not
                    DataRow[] filteredPolicyRows = this.dtItems.Select("Policy='" + txtPolicy.Text + "'");
                    if (filteredPolicyRows.Length > 0)
                    {
                        UserMessages.Message(null, Resources.UserInfoMessages.PolicyAlreadyExist, string.Empty);
                        txtPolicy.Focus();
                        return;
                    }

                    var countPolicy = dc.InvoiceDetails.Count(x => x.Policy == txtPolicy.Text);
                    if (countPolicy > 0)
                    {
                        UserMessages.Message(null, Resources.UserInfoMessages.PolicyAlreadyExist, string.Empty);
                        txtPolicy.Focus();
                        return;
                    }
                }

                r = this.dtItems.NewRow();
                r["ID"] = this.dtItems.GetID("ID");

            }
            else
            {


                if (!string.IsNullOrEmpty(txtPolicy.Text))
                {
                    //testing if Policy already exist or not
                    DataRow[] filteredPolicyRows = this.dtItems.Select("Policy='" + txtPolicy.Text + "'");
                    if (filteredPolicyRows.Length > 1)
                    {
                        UserMessages.Message(null, Resources.UserInfoMessages.PolicyAlreadyExist, string.Empty);
                        txtPolicy.Focus();
                        return;
                    }

                    var countPolicy = dc.InvoiceDetails.Count(x => x.Policy == txtPolicy.Text);
                    if (countPolicy > 1)
                    {
                        UserMessages.Message(null, Resources.UserInfoMessages.PolicyAlreadyExist, string.Empty);
                        txtPolicy.Focus();
                        return;
                    }
                }

                r = this.dtItems.Select("ID=" + this.EditID)[0];
            }

            r["Store_ID"] = acStore.Value;
            r["Category_ID"] = acCategory.Value;
            r["Item_ID"] = acItem.Value;
            r["ItemName"] = acItem.Text;

            r["IDCodeOperation"] = txtCode.Text;
            r["Policy"] = txtPolicy.Text;
            r["Capacity"] = txtCapacity.Text;
            r["Capacities"] = txtCapacities.Text;
            r["ItemDescription"] = acItemDescribed.Value;
            r["DescribedName"] = acItemDescribed.Text;
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
            r["Barcode"] = txtBarcode.Text;
            r["CategoryName"] = acCategory.Text;

            //need Options
            r["InvoiceDate"] = txtInvoiceDate.Text.ToDateOrDBNULL();
            //رقم التشغيلة
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
            if (acStore.HasValue) this.FocusNextControl(acStore); else acStore.AutoCompleteFocus();
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

            var Items = dc.usp_Items_SelectByCategory(acCategory.Value.ToInt(), true);

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
                r["Capacity"] = txtCapacity.Text;
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
                r["InvoiceDate"] = txtInvoiceDate.Text.ToDateOrDBNULL();
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
            this.acItem_SelectedIndexChanged(null, null);
            txtCost.Text = r["UnitCost"].ToExpressString();
            // txtDescribed.Text = r["ItemDescription"].ToExpressString();
            acItemDescribed.Value = r["ItemDescription"].ToExpressString();
            txtQty.Text = r["Quantity"].ToExpressString();
            txtQtyInNumber.Text = r["QtyInNumber"].ToExpressString();
            acUnit.Value = r["Uom_ID"].ToExpressString();
            this.acUnit_SelectedIndexChanged(null, null);
            acItemTax.Value = r["Tax_ID"].ToExpressString();
            acBatchID.Value = r["Batch_ID"].ToExpressString();
            txtItemPercentageDiscount.Text = r["PercentageDiscount"].ToExpressString();
            txtItemCashDiscount.Text = r["CashDiscount"].ToExpressString();
            txtItemNotes.Text = r["Notes"].ToExpressString();
            txtCapacity.Text = r["Capacity"].ToExpressString();
            txtCapacities.Text = r["Capacities"].ToExpressString();
            txtBarcode.Text = r["Barcode"].ToExpressString();
            txtCode.Text = r["IDCodeOperation"].ToExpressString();
            txtPolicy.Text = r["Policy"].ToExpressString();
            txtInvoiceDate.Text = r["InvoiceDate"].ToExpressString();
            if (r["ProductionDate"].ToExpressString() != string.Empty) txtProductionDate.Text = r["ProductionDate"].ToDate().Value.ToString("d/M/yyyy"); ;
            if (r["ExpirationDate"].ToExpressString() != string.Empty) txtExpirationDate.Text = r["ExpirationDate"].ToDate().Value.ToString("d/M/yyyy");
            this.ShowAvailableQty(); //Called Twice but important
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
            if (acBranch.Value != null)
            {
                var getStores = dc.usp_StoresAutoCompelete_Select(int.Parse(acBranch.Value)).ToList();
                if (getStores.Count == 1)
                    acStore.Value = getStores.First().ID.ToString();

            }
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
            dc.usp_Invoice_Cancel(this.Invoice_ID, MyContext.UserProfile.Contact_ID);
            LogAction(Actions.Delete, txtSerial.Text, dc);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.Invoice + "?ID=" + this.Invoice_ID.ToExpressString(), PageLinks.InvoicesList + Request.PathInfo, PageLinks.Invoice);
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
            this.ShowCustomerBalance();
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

    protected void btnYes_click(object sender, EventArgs e)
    {
        try
        {
            this.ConfirmationAnswered = true;
            this.BtnApprove_Click(null, null);
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
            XpressDataContext dataContext = new XpressDataContext();
            var databaseName = dataContext.Connection.Database;
            if (databaseName == "TWFL")
            {
                ReportDocument doc = new ReportDocument();
                doc.Load(Server.MapPath("~\\Reports\\InvoicePrint.rpt"));
                doc.SetParameterValue("@Invoice_ID", this.Invoice_ID);
                doc.SetParameterValue("@Branch_ID", acBranch.Value.ToNullableInt());
                doc.SetParameterValue("BaseDir", HttpContext.Current.Server.MapPath("~\\Uploads\\"));
                doc.SetParameterValue("ByEmp", ((UICulturePage)HttpContext.Current.Handler).MyContext.UserProfile.EmployeeName);
                Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "Invoice"), false);
            }
            else
            {

                //  XpressDataContext dataContext = new XpressDataContext();

                ReportDocument doc = new ReportDocument();
                // doc.Load(Server.MapPath("~\\Reports\\Invoice_Print ssb.rpt"));
                doc.Load(Server.MapPath("~\\Reports\\Invoice_Print_receipt.rpt"));

                doc.SetParameterValue("@Invoice_ID", this.Invoice_ID);

                // doc.SetParameterValue("BaseDir", HttpContext.Current.Server.MapPath("~\\Uploads\\"));
                //doc.SetParameterValue("ByEmp", ((UICulturePage)HttpContext.Current.Handler).MyContext.UserProfile.EmployeeName);
                // doc.SetParameterValue("@Branch_ID", acBranch.Value.ToNullableInt());
                Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "Invoice"), false);


                //ReportDocument doc = new ReportDocument();
                //doc.Load(Server.MapPath("~\\Reports\\Invoice_Print - Copy.rpt"));
                //doc.SetParameterValue("@Invoice_ID", this.Invoice_ID);
                //doc.SetParameterValue("@DocTableType_ID", DocumentsTableTypes.Invoice.ToInt(), "DocumentTaxes.rpt");
                //doc.SetParameterValue("@Doc_ID", this.Invoice_ID, "DocumentTaxes.rpt");
                //doc.SetParameterValue("@IsReturn", false, "DocumentTaxes.rpt");
                //Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "Invoice"), false);
            }
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
        acItem.ContextKey = "," + acCategory.Value + "," + acStore.Value + ",true";
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
        // txtQty.Clear();
        txtQtyInNumber.Clear();
        acBatchID.ContextKey = acItem.Value + "," + acStore.Value + "," + acUnit.Value + ",True";
        acUnit.ContextKey = string.Empty + acItem.Value;
        acPriceName.ContextKey = string.Empty + acItem.Value;
        if (acItem.HasValue)
        {
            var item = dc.usp_Items_SelectByID(acItem.Value.ToNullableInt()).FirstOrDefault();
            txtCost.Text = (item.DefaultPrice.Value / (txtRatio.Text.ToNullableDecimal() ?? 1)).ToString(NbrHashNeerDecimal);
            txtItemPercentageDiscount.Text = item.DiscountPercentage.Value.ToExpressString();
            txtBarcode.Text = item.Barcode;
            if (item.Tax_ID.HasValue) acItemTax.Value = item.Tax_ID.Value.ToExpressString();
            acUnit.Value = item.UOM_ID.Value.ToExpressString();
            acCategory.Value = item.Category_ID.ToExpressString();
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
        this.QuantityWarning = dc.usp_Company_Select().FirstOrDefault().QuantityWarning.Value;

        acArea.ContextKey = string.Empty;
        acParentAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString();
        acParentAccount.Value = COA.Customers.ToInt().ToExpressString();

        if (ddlCurrency.SelectedValue != null)
        {
            var getbranch = dc.usp_Branchs_Select("", null).ToList();
            if (getbranch.Count == 1)
                acBranch.Value = getbranch.First().ID.ToString();
            acBranch_SelectedIndexChanged(null, null);
        }


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
        // acStore.Clear();
        if (MyContext.UserProfile.Store_ID != null) acStore.Value = MyContext.UserProfile.Store_ID.ToStringOrEmpty();
        acCategory.Clear();
        this.FilterItems();
        this.acItem_SelectedIndexChanged(null, null);
    }

    private void SetEditMode()
    {
        if (Request["ID"] != null)
        {
            this.EditMode = true;
            this.Invoice_ID = Request["ID"].ToInt();
        }
        if (Request["SalesOrderID"] != null)
        {
            this.SalesOrderID = Request["SalesOrderID"].ToInt();
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

        if (txtOperationDate.Text.ToDate() > DateTime.Now.Date)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
            trans.Rollback();
            return false;
        }

        if (this.dtItems.AsEnumerable().Where(x => x.RowState != DataRowState.Deleted && x.Field<int?>("Store_ID") == null).Any())
        {
            UserMessages.Message(null, Resources.UserInfoMessages.StoresRequired, string.Empty);
            trans.Rollback();
            return false;
        }

        if (txtFirstPaid.Text.ToDecimalOrDefault() > this.GrossTotal)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.FirstPaidGreaterThanTotal, string.Empty);
            trans.Rollback();
            return false;
        }

        this.CalculatedSalesCost = 0;
        this.ConfirmationMessage = string.Empty;
        if (IsApproving && !this.CheckCreditLmit())
        {
            trans.Rollback();
            return false;
        }

        string Serial = string.Empty;
        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        DateTime? ApproveDate = IsApproving ? DateTime.Now : (DateTime?)null;
        int? approvedBY_ID = IsApproving ? MyContext.UserProfile.Contact_ID : (int?)null;
        byte EntryType = this.IsCashInvoice ? (byte)2 : (byte)1;
        int Serial_ID = DocSerials.Invoice.ToInt();
        int? SalesOrderID = this.SalesOrderID == 0 ? (int?)null : this.SalesOrderID;
        int Detail_ID = 0;
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
                    // Detail_ID = dc.usp_InvoiceDetails_Insert(this.Invoice_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["Capacity"].ToString());
                    Detail_ID = dc.usp_InvoiceDetailsWithDesciption_Insert(this.Invoice_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["Capacity"].ToString(), r["ItemDescription"].ToString(), r["IDCodeOperation"].ToString(), r["Policy"].ToString(), r["InvoiceDate"].ToDate(), r["Capacities"].ToString(), false, false, false, false);

                    if (IsApproving)
                        if (!this.InsertICJ(Detail_ID, r))
                        {
                            trans.Rollback();
                            return false;
                        }
                }
                foreach (DataRow r in this.dtTaxes.Rows)
                {
                    if (r.RowState == DataRowState.Deleted) continue;
                    dc.usp_DocuemntTaxes_Insert(DocumentsTableTypes.Invoice.ToInt(), this.Invoice_ID, r["Tax_ID"].ToInt());
                }
                if (IsApproving) this.InsertOperation();
                LogAction(IsApproving ? Actions.Approve : Actions.Add, Serial, dc);
            }
        }
        else
        {
            int Result = dc.usp_Invoice_Update(this.Invoice_ID, acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimalOrDefault(), txtOperationDate.Text.ToDate(),
                                                    acCustomer.Value.ToInt(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate, approvedBY_ID,
                                                    null, null, acAddress.Value.ToNullableInt(), acShipAddress.Value.ToNullableInt(), acPaymentAddress.Value.ToNullableInt(), acTelephone.Value.ToNullableInt(),
                                                    acCostCenter.Value.ToNullableInt(), txtNotes.Text, lblTotal.Text.ToDecimal(), txtPercentageDiscount.Text.ToDecimalOrDefault(), txtCashDiscount.Text.ToDecimalOrDefault(), txtAdditionals.Text.ToDecimalOrDefault(),
                                                    lblTotalDiscount.Text.ToDecimalOrDefault(), lblTotalTax.Text.ToDecimalOrDefault(), lblGrossTotal.Text.ToDecimalOrDefault(), txtFirstPaid.Text.ToDecimalOrDefault(), txtUserRefNo.Text, this.DocRandomString, acSalesRep.Value.ToNullableInt(), acCashAccount.Value.ToNullableInt(),null);
            if (Result > 0)
            {
                foreach (DataRow r in this.dtItems.Rows)
                {
                    if (r.RowState == DataRowState.Added)
                    {
                        // Detail_ID = dc.usp_InvoiceDetails_Insert(this.Invoice_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["Capacity"].ToString());
                        Detail_ID = dc.usp_InvoiceDetailsWithDesciption_Insert(this.Invoice_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["Capacity"].ToString(), r["ItemDescription"].ToString(), r["IDCodeOperation"].ToString(), r["Policy"].ToString(), r["InvoiceDate"].ToDate(), r["Capacities"].ToString(), false, false, false, false);

                    }
                    if (r.RowState == DataRowState.Modified)
                    {
                        Detail_ID = r["ID"].ToInt();
                        dc.usp_InvoiceDetails_Update(r["ID"].ToInt(), r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), false);
                    }
                    if (r.RowState == DataRowState.Deleted)
                    {
                        dc.usp_InvoiceDetails_Delete(r["ID", DataRowVersion.Original].ToInt());
                    }
                    if (IsApproving && r.RowState != DataRowState.Deleted)
                    {
                        Detail_ID = r.RowState == DataRowState.Added ? Detail_ID : r["ID"].ToInt();
                        if (!this.InsertICJ(Detail_ID, r))
                        {
                            trans.Rollback();
                            return false;
                        }
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
                if (IsApproving) this.InsertOperation();
                LogAction(IsApproving ? Actions.Approve : Actions.Edit, txtSerial.Text, dc);
            }
        }
        if (this.ConfirmationMessage != string.Empty)
        {
            ltConfirmationMessage.Text = this.ConfirmationMessage;
            mpeConfirm.Show();
            trans.Rollback();
            return false;
        }

        Serial = Serial == string.Empty ? string.Empty : " (" + Serial + ") ";
        UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess + Serial, PageLinks.Invoice + Request.PathInfo + "?ID=" + this.Invoice_ID.ToExpressString(), PageLinks.InvoicesList, PageLinks.Invoice + Request.PathInfo);
        return true;
    }

    private void InsertOperation()
    {
        decimal ratio = txtRatio.Text.ToDecimal();
        decimal Additionals = txtAdditionals.Text.ToDecimalOrDefault();
        string serial = string.Empty;
        var company = dc.usp_Company_Select().FirstOrDefault();
        int ContactAccountID = dc.fun_getContactAccountID(acCustomer.Value.ToInt()).Value;
        var salesGiftAccountID = company.SalesGiftAccountID.ToIntOrDefault();


        int Result = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.Sales.ToInt(), ddlCurrency.SelectedValue.ToInt(), (this.Total + this.TotalCreditTax + Additionals) * ratio, (this.Total + this.TotalCreditTax + Additionals), ratio, txtNotes.Text);

        //ايراد المبيعات
        dc.usp_OperationDetails_Insert(Result, company.SellAccount_ID, 0, this.Total * ratio, 0, this.Total, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());

        //ايراد الاضافات
        if (Additionals > 0) dc.usp_OperationDetails_Insert(Result, company.InvoiceAdditionals_AccountID, 0, Additionals * ratio, 0, Additionals, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());


        //Customer
        dc.usp_OperationDetails_Insert(Result, ContactAccountID, (this.GrossTotal) * ratio, 0, (this.GrossTotal), 0, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());

        //Discount
        if (this.TotalDiscount > 0)
        {
            if (salesGiftAccountID > 0)
            {
                if (dtItems.Select("IsGift==1").Count() > 0)
                {
                    dc.usp_OperationDetails_Insert(Result, company.SalesGiftAccountID, this.TotalDiscount * ratio, 0, this.TotalDiscount, 0, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());

                }
                else
                {
                    dc.usp_OperationDetails_Insert(Result, company.SalesDiscountAccountID, this.TotalDiscount * ratio, 0, this.TotalDiscount, 0, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());

                }
            }
            else
            {
                dc.usp_OperationDetails_Insert(Result, company.SalesDiscountAccountID, this.TotalDiscount * ratio, 0, this.TotalDiscount, 0, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
            }

        }

        //Taxes
        var GroupedTaxes = from tax in this.dtAllTaxes.AsEnumerable()
                           where tax.RowState != DataRowState.Deleted
                           group tax by new { Account_ID = tax.Field<int>("Account_ID"), OnInvoiceType = tax.Field<string>("OnInvoiceType") } into groupedTaxes
                           select new { Key = groupedTaxes.Key, DebitAmount = groupedTaxes.Sum(x => x.Field<decimal>("DebitAmount")), CreditAmount = groupedTaxes.Sum(x => x.Field<decimal>("CreditAmount")) };
        foreach (var tax in GroupedTaxes)
        {
            dc.usp_OperationDetails_Insert(Result, tax.Key.Account_ID, tax.DebitAmount * ratio, tax.CreditAmount * ratio, tax.DebitAmount, tax.CreditAmount, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
        }

        if (this.CalculatedSalesCost > 0)
        {
            dc.usp_SalesCostOperation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), this.CalculatedSalesCost, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
        }

        //CostCenter Debit
        dc.usp_CostCenterOperation_Insert(acBranch.Value.ToNullableInt(), acCostCenter.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), this.CalculatedSalesCost + (this.TotalDiscount * ratio), this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt(), txtNotes.Text);
        //CostCenter Credit
        dc.usp_CostCenterOperation_Insert(acBranch.Value.ToNullableInt(), acCostCenter.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), (this.Total + Additionals) * ratio * -1, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt(), txtNotes.Text);
        this.InsertCashIn();
    }

    private bool InsertICJ(int Detail_ID, DataRow row)
    {
        decimal? SalesCost = 0;
        int result = dc.usp_ICJ_Invoice(txtOperationDate.Text.ToDate(), row["Quantity"].ToDecimal(), row["Item_ID"].ToInt(), row["Uom_ID"].ToInt(), row["Total"].ToDecimal() * txtRatio.Text.ToDecimal(), row["Batch_ID"].ToNullableInt(), row["Store_ID"].ToInt(), acBranch.Value.ToNullableInt(), DocumentsTableTypes.Invoice.ToInt(), this.Invoice_ID, Detail_ID, this.SalesOrderID, ref SalesCost);
        this.CalculatedSalesCost += SalesCost.Value;

        if ((result & ICJStoredProcFlags.QtyReserved.ToInt()) == ICJStoredProcFlags.QtyReserved.ToInt())
        {
            UserMessages.Message(null, Resources.UserInfoMessages.QtyReserved + " (" + row["StoreName"] + " : " + row["ItemName"] + " : " + row["BatchName"] + ")", string.Empty);
            this.ConfirmationMessage = string.Empty;
            return false;
        }

        if ((result & ICJStoredProcFlags.QtyNotEnoughAndStop.ToInt()) == ICJStoredProcFlags.QtyNotEnoughAndStop.ToInt())
        {
            UserMessages.Message(null, Resources.UserInfoMessages.QtyNotEnough + " (" + row["StoreName"] + " : " + row["ItemName"] + " : " + row["BatchName"] + ")", string.Empty);
            this.ConfirmationMessage = string.Empty;
            return false;
        }
        if ((result & ICJStoredProcFlags.BatchOrderWarning.ToInt()) == ICJStoredProcFlags.BatchOrderWarning.ToInt())
        {
            UserMessages.Message(null, Resources.UserInfoMessages.BatchOrderWarning + " (" + row["StoreName"] + " : " + row["ItemName"] + " : " + row["BatchName"] + ")", string.Empty);
            this.ConfirmationMessage = string.Empty;
            return false;
        }

        if ((result & ICJStoredProcFlags.BatchQtyNotEnough.ToInt()) == ICJStoredProcFlags.BatchQtyNotEnough.ToInt())
        {
            UserMessages.Message(null, Resources.UserInfoMessages.BatchQtyNotEnough + " (" + row["StoreName"] + " : " + row["ItemName"] + " : " + row["BatchName"] + ")", string.Empty);
            this.ConfirmationMessage = string.Empty;
            return false;
        }

        if ((result & ICJStoredProcFlags.QtyNotEnoughAndAsk.ToInt()) == ICJStoredProcFlags.QtyNotEnoughAndAsk.ToInt() && !this.ConfirmationAnswered)
        {
            this.ConfirmationMessage += "<br>\u2022 " + Resources.UserInfoMessages.QtyNotEnough + " (" + row["StoreName"] + " : " + row["ItemName"] + " : " + row["BatchName"] + ")";
        }
        if ((result & ICJStoredProcFlags.PriceLessThanCost.ToInt()) == ICJStoredProcFlags.PriceLessThanCost.ToInt() && !this.ConfirmationAnswered)
        {
            this.ConfirmationMessage += "<br><span style='color:red'> \u2022 " + Resources.UserInfoMessages.PriceLessThanCost + " (" + row["StoreName"] + " : " + row["ItemName"] + " : " + row["BatchName"] + ")</span>";
        }
        return true;
    }

    private void FillInvoice()
    {
        if (this.Invoice_ID == 0 && this.SalesOrderID == 0) return;
        var invoice = dc.usp_Invoice_SelectByID(EditMode ? this.Invoice_ID : this.SalesOrderID).FirstOrDefault();
        ddlCurrency.SelectedValue = invoice.Currency_ID.ToExpressString();
        txtRatio.Text = invoice.Ratio.ToExpressString();
        acBranch.Value = invoice.Branch_ID.ToStringOrEmpty();
        this.FilterByBranchAndCurrency();
        txtOperationDate.Text = invoice.OperationDate.Value.ToExpressString();
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
        if (invoice.SalesOrder_ID.HasValue) this.SalesOrderID = invoice.SalesOrder_ID.Value;

        if (!string.IsNullOrEmpty(invoice.SalesOrderSerial) || this.SalesOrderID != 0)
        {
            lblSalesOrderNo.Text = EditMode ? invoice.SalesOrderSerial : invoice.Serial;
            divSalesOrderNo.Visible = true;
            ddlCurrency.Enabled = false;
            acBranch.Enabled = false;
            acCustomer.Enabled = false;
            acSalesRep.Enabled = false;
        }

        if (EditMode)
        {
            txtSerial.Text = invoice.Serial;
            this.DocRandomString = invoice.DocRandomString;
            lblCreatedBy.Text = invoice.CreatedByName;
            lblApprovedBy.Text = invoice.ApprovedBYName;
            this.ImgStatus = ((DocStatus)invoice.DocStatus_ID).ToExpressString();
            btnPrint.Visible = MyContext.PageData.IsPrint;
            btnPrintInventoryOrder.Visible = MyContext.PageData.IsPrint;
            pnlAddItem.Visible = (invoice.DocStatus_ID == 1);
            btnCancel.Visible = (invoice.DocStatus_ID == 1) && MyContext.PageData.IsDelete;
            btnApprove.Visible = (invoice.DocStatus_ID == 1) && MyContext.PageData.IsApprove;
            btnSave.Visible = (invoice.DocStatus_ID == 1) && ((MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode));
            gvItems.Columns[gvItems.Columns.Count - 1].Visible = gvItems.Columns[gvItems.Columns.Count - 2].Visible = (invoice.DocStatus_ID == 1);
            gvTaxes.Columns[gvTaxes.Columns.Count - 1].Visible = (invoice.DocStatus_ID == 1);
            this.DocStatus_ID = invoice.DocStatus_ID.Value;
        }

        this.dtItems = dc.usp_InvoiceDetailsWithDescription_Select(EditMode ? this.Invoice_ID : this.SalesOrderID).CopyToDataTable();
        // this.dtItems = dc.usp_InvoiceDetails_Select(EditMode ? this.Invoice_ID : this.SalesOrderID).CopyToDataTable();
        this.dtTaxes = dc.usp_DocuemntTaxes_Select(DocumentsTableTypes.Invoice.ToInt(), EditMode ? this.Invoice_ID : this.SalesOrderID, false).CopyToDataTable();
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

        MyContext con = new MyContext(System.Web.Security.Membership.GetUser(), PageLinks.Customers, string.Empty);
        btnFastAddNew.Visible = con.PageData.IsAdd;
    }

    private void Calculate()
    {
        decimal ItemDiscount = 0;
        decimal DocDiscount = 0;
        decimal DocTax = 0;
        decimal Additionals = 0;
        decimal DocTaxValue = 0;
        this.Total = 0;
        this.GrossTotal = 0;
        this.TotalDiscount = 0;
        this.TotalTax = 0;
        this.TotalDebitTax = 0;
        this.TotalCreditTax = 0;
        this.dtAllTaxes.Rows.Clear();
        this.dtAllTaxes.AcceptChanges();
        foreach (DataRow r in this.dtItems.Rows)
        {
            ItemDiscount = 0;
            if (r.RowState == DataRowState.Deleted) continue;
            r["Total"] = r["UnitCost"].ToDecimal() * r["Quantity"].ToDecimal();
            Total += r["UnitCost"].ToDecimal() * r["Quantity"].ToDecimal();
            this.TotalDiscount += ItemDiscount = (r["PercentageDiscount"].ToDecimal() * r["Total"].ToDecimal() * 0.01m) + r["CashDiscount"].ToDecimal();
            if (r["TaxOnInvoiceType"] != DBNull.Value && r["Tax_ID"] != DBNull.Value)
            {
                r["TotalTax"] = ((r["Total"].ToDecimal() - ItemDiscount) * r["TaxPercentageValue"].ToDecimal() * 0.01m);

                if (r["TaxOnInvoiceType"].ToExpressString() == "C")
                {
                    this.dtAllTaxes.Rows.Add(r["TaxSalesAccountID"].ToInt(), r["TaxOnInvoiceType"].ToExpressString(), 0, r["TotalTax"].ToDecimal());
                    this.TotalCreditTax += r["TotalTax"].ToDecimal();
                }
                else
                {
                    this.dtAllTaxes.Rows.Add(r["TaxSalesAccountID"].ToInt(), r["TaxOnInvoiceType"].ToExpressString(), r["TotalTax"].ToDecimal(), 0);
                    this.TotalDebitTax += r["TotalTax"].ToDecimal();
                    r["TotalTax"] = r["TotalTax"].ToDecimal() * -1;
                }
                this.TotalTax += r["TotalTax"].ToDecimal();
            }
            r["GrossTotal"] = r["Total"].ToDecimal() - ItemDiscount + r["TotalTax"].ToDecimal();
            this.GrossTotal += r["Total"].ToDecimal() - ItemDiscount + r["TotalTax"].ToDecimal();
        }

        DocDiscount = (this.GrossTotal * txtPercentageDiscount.Text.ToDecimalOrDefault() * 0.01m) + txtCashDiscount.Text.ToDecimalOrDefault();
        foreach (DataRow r in this.dtTaxes.Rows)
        {
            if (r.RowState == DataRowState.Deleted) continue;
            if (r["OnInvoiceType"] != DBNull.Value)
            {
                DocTaxValue = ((this.GrossTotal - DocDiscount) * r["PercentageValue"].ToDecimal() * 0.01m);
                if (r["OnInvoiceType"].ToExpressString() == "C")
                {
                    this.dtAllTaxes.Rows.Add(r["SalesAccountID"].ToInt(), r["OnInvoiceType"].ToExpressString(), 0, DocTaxValue);
                    this.TotalCreditTax += DocTaxValue;
                }
                else
                {
                    this.dtAllTaxes.Rows.Add(r["SalesAccountID"].ToInt(), r["OnInvoiceType"].ToExpressString(), DocTaxValue, 0);
                    this.TotalDebitTax += DocTaxValue;
                    DocTaxValue *= -1m;
                }
                DocTax += DocTaxValue;
            }
        }
        Additionals = txtAdditionals.Text.ToDecimalOrDefault();
        this.TotalDiscount += DocDiscount;
        this.TotalTax += DocTax;
        this.GrossTotal = this.GrossTotal - DocDiscount + DocTax + Additionals;
        lblTotal.Text = this.Total.ToString(NbrHashNeerDecimal);
        lblTotalDiscount.Text = this.TotalDiscount.ToString(NbrHashNeerDecimal);
        lblAdditionals.Text = Additionals.ToString(NbrHashNeerDecimal);
        lblTotalTax.Text = this.TotalTax.ToString(NbrHashNeerDecimal);
        lblGrossTotal.Text = this.GrossTotal.ToString(NbrHashNeerDecimal);
        if ((acCashAccount.HasValue && Page.IsPostBack && this.SumFirstPaid) || this.IsCashInvoice)
        {
            txtFirstPaid.Text = lblGrossTotal.Text;
            this.txtFirstPaid_TextChanged(null, null);
        }
        this.ConfirmationAnswered = false;
        this.ConfirmationMessage = string.Empty;
        this.ShowCustomerBalance();

    }

    private void ShowAvailableQty()
    {
        lblAvailableQty.Text = string.Empty;
        if (acItem.HasValue)
        {
            var itemQtyFromInput = acItem.Value.ToInt();

            var funItemQty = dc.fun_ItemQty(itemQtyFromInput, acStore.Value.ToNullableInt(), acBatchID.Value.ToNullableInt(), acUnit.Value.ToNullableInt(), this.SalesOrderID);
            if (funItemQty != null)
            {
                decimal qty = funItemQty.Value;
                lblAvailableQty.Text = qty.ToExpressString();
                lblAvailableQty.ForeColor = qty <= 0 ? System.Drawing.Color.Red : System.Drawing.Color.Green;
            }
        }
    }

    private bool CheckCreditLmit()
    {

        if (this.ConfirmationAnswered) return true;
        var result = dc.fun_CheckCustomerCreditLimit(acCustomer.Value.ToInt(), this.GrossTotal - txtFirstPaid.Text.ToDecimalOrDefault()).Value;
        if (result == -9)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.CustomerCreditLimitExceeded, string.Empty);
            return false;
        }
        if (result == -8)
        {
            this.ConfirmationMessage = "<br>" + Resources.UserInfoMessages.CustomerCreditLimitWillExceed + "<br>";
        }
        return true;
    }

    private void InsertCashIn()
    {
        string Serial = string.Empty;
        decimal ratio = txtRatio.Text.ToDecimal();
        int? CashIn_ID = null;
        if (txtFirstPaid.Text.ToDecimalOrDefault() <= 0) return;
        int ContactChartOfAccount_ID = dc.fun_getContactAccountID(acCustomer.Value.ToInt()).Value;
        CashIn_ID = dc.usp_Payments_Insert(txtOperationDate.Text.ToDate(), DateTime.Now, MyContext.UserProfile.Contact_ID, MyContext.UserProfile.Contact_ID, DateTime.Now, txtUserRefNo.TrimmedText, ref Serial, DocSerials.CashIn.ToInt(), txtNotes.Text, txtFirstPaid.Text.ToDecimal(), null, DocStatus.Approved.ToByte(), PaymentsTypes.CashInCustomer.ToByte(), acBranch.Value.ToNullableInt(), txtRatio.Text.ToDecimal(), ddlCurrency.SelectedValue.ToInt(), this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt(), this.DocRandomString + "_FromInvoice");
        if (!CashIn_ID.HasValue || CashIn_ID.Value <= 0) throw new Exception("Error Occured During inserting the cash document");
        dc.usp_PaymentsDetails_Insert(CashIn_ID, acCashAccount.Value.ToInt(), ContactChartOfAccount_ID, txtFirstPaid.Text.ToDecimal(), null, string.Empty, null);

        int Operation_ID = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref Serial, DocStatus.Approved.ToByte(), OperationTypes.CashIn.ToInt(), ddlCurrency.SelectedValue.ToInt(), txtFirstPaid.Text.ToDecimal() * ratio, txtFirstPaid.Text.ToDecimal(), ratio, null);
        dc.usp_OperationDetails_Insert(Operation_ID, ContactChartOfAccount_ID, 0, txtFirstPaid.Text.ToDecimal() * ratio, 0, txtFirstPaid.Text.ToDecimal(), null, CashIn_ID, DocumentsTableTypes.Payment_CashIn.ToInt());
        dc.usp_OperationDetails_Insert(Operation_ID, acCashAccount.Value.ToInt(), txtFirstPaid.Text.ToDecimal() * ratio, 0, txtFirstPaid.Text.ToDecimal(), 0, null, CashIn_ID, DocumentsTableTypes.Payment_CashIn.ToInt());
        dc.usp_SetCashDocForBills(this.Invoice_ID, CashIn_ID, DocumentsTableTypes.Invoice.ToInt());
    }

    private void ShowCustomerBalance()
    {
        int? CustomerAccountID = (acCustomer.HasValue) ? dc.fun_getContactAccountID(acCustomer.Value.ToInt()) : (int?)null;
        decimal Balance = (CustomerAccountID == null) ? 0.0000m : dc.fun_GetAccountBalanceInForeign(CustomerAccountID, txtOperationDate.Text.ToDate(), acBranch.Value.ToNullableInt()).Value;
        lblCustomerBalanceBefore.Text = (Balance - (this.DocStatus_ID == 2 ? lblGrossTotal.Text.ToDecimalOrDefault() - txtFirstPaid.Text.ToDecimalOrDefault() : 0)).ToExpressString();
        lblCustomerBalanceAfter.Text = (lblCustomerBalanceBefore.Text.ToDecimalOrDefault() + lblGrossTotal.Text.ToDecimalOrDefault() - txtFirstPaid.Text.ToDecimalOrDefault()).ToExpressString();
    }

    private void ShowCustomerLastItemPrice()
    {
        lblLastCustomerPrice.Text = (acCustomer.HasValue && acItem.HasValue) ? dc.fun_CustomerLastItemPrice(acCustomer.Value.ToNullableInt(), acItem.Value.ToNullableInt()) : "0.0000";
    }

    private void CustomPage()
    {
        acCostCenter.Visible = MyContext.Features.CostCentersEnabled & !this.IsCashInvoice;
        acBatchID.Visible = MyContext.Features.BatchIDEnabled & !this.IsCashInvoice;
        txtProductionDate.Visible = MyContext.Features.BatchIDEnabled & !this.IsCashInvoice;
        txtExpirationDate.Visible = MyContext.Features.BatchIDEnabled & !this.IsCashInvoice;
        acItemTax.Visible = MyContext.Features.TaxesEnabled & !this.IsCashInvoice;
        taxSection.Visible = MyContext.Features.TaxesEnabled & !this.IsCashInvoice;
        taxTotal.Visible = MyContext.Features.TaxesEnabled & !this.IsCashInvoice;
        DiscountTotal.Visible = MyContext.Features.CashDiscountEnabled || MyContext.Features.PercentageDiscountEnabled;
        acBranch.IsRequired = acBranch.Visible = MyContext.Features.BranchesEnabled;
        txtCashDiscount.Visible = txtItemCashDiscount.Visible = MyContext.Features.CashDiscountEnabled;
        txtPercentageDiscount.Visible = txtItemPercentageDiscount.Visible = MyContext.Features.PercentageDiscountEnabled;

        ddlCurrency.Visible = lblCurrency.Visible = txtRatio.Visible = txtUserRefNo.Visible = acAddress.Visible =
        acShipAddress.Visible = acPaymentAddress.Visible = acTelephone.Visible = acPriceName.Visible = txtNotes.Visible =
        txtFirstPaid.Visible = txtItemNotes.Visible = acCategory.Visible = !this.IsCashInvoice;

        acSalesRep.Visible = MyContext.Features.SalesRepEnabled & !this.IsCashInvoice;

        foreach (DataControlField col in gvItems.Columns)
        {
            if (col.ItemStyle.CssClass == "BatchCol") col.Visible = MyContext.Features.BatchIDEnabled & !this.IsCashInvoice;
            if (col.ItemStyle.CssClass == "TaxCol") col.Visible = MyContext.Features.TaxesEnabled & !this.IsCashInvoice;
        }
    }

    private void SetDefaults()
    {
        if (Page.IsPostBack) return;
        var company = dc.usp_Company_Select().FirstOrDefault();
        var defaults = dc.usp_CashBillDefaults_Select(acBranch.Value.ToNullableInt(), MyContext.UserProfile.Contact_ID).First();

        if (company.AutoDate.Value || this.IsCashInvoice) txtOperationDate.Text = DateTime.Now.Date.ToString("d/M/yyyy");
        if (txtOperationDate.IsNotEmpty) this.ddlCurrency_SelectedIndexChanged(null, null);

        if (this.IsCashInvoice && defaults.DefaultCustomer_ID.HasValue) acCustomer.Value = defaults.DefaultCustomer_ID.ToExpressString();

        if (acCustomer.HasValue) this.acCustomer_SelectedIndexChanged(null, null);

        if ((defaults.UserHasCashAccount.Value || this.IsCashInvoice) && defaults.DefaultCashAccount_ID.HasValue) acCashAccount.Value = defaults.DefaultCashAccount_ID.ToExpressString();

        if ((defaults.UserHasStore.Value || this.IsCashInvoice) && defaults.DefaultStore_ID.HasValue) acStore.Value = defaults.DefaultStore_ID.ToExpressString();
        if (acStore.HasValue) this.acStore_SelectedIndexChanged(null, null);

        txtOperationDate.Enabled = !company.LockAutoDate.Value;
        acCashAccount.Enabled = !((MyContext.UserProfile.CashierAccount_ID != null) && ((MyContext.Features.BranchesEnabled && MyContext.UserProfile.Branch_ID != null) || !MyContext.Features.BranchesEnabled));
        acStore.Enabled = !((MyContext.UserProfile.Store_ID != null) && ((MyContext.Features.BranchesEnabled && MyContext.UserProfile.Branch_ID != null) || !MyContext.Features.BranchesEnabled));
        if (this.IsCashInvoice) ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "focusbarcode", "$(document).ready(function(){  setTimeout(function(){$('#cph_txtBarcode').focus();},500);});", true);
    }

    #endregion

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
}