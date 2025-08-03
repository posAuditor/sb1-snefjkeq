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

public partial class Purchases_ReceiptEntry : UICulturePage
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

    private int Receipt_ID
    {
        get
        {
            if (ViewState["Receipt_ID"] == null) return 0;
            return (int)ViewState["Receipt_ID"];
        }

        set
        {
            ViewState["Receipt_ID"] = value;
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
            if (Session["dtItems_Receipt" + this.WinID] == null)
            {
                //Session["dtItems_Receipt" + this.WinID] = dc.usp_ReceiptDetails_Select(null).CopyToDataTable();
                Session["dtItems_Receipt" + this.WinID] = dc.usp_ReceiptDetailsWithDescription_Select(null).CopyToDataTable();
            }
            return (DataTable)Session["dtItems_Receipt" + this.WinID];
        }

        set
        {
            Session["dtItems_Receipt" + this.WinID] = value;
        }
    }

    private DataTable dtTaxes
    {
        get
        {
            if (Session["dtTaxes_Receipt" + this.WinID] == null)
            {
                Session["dtTaxes_Receipt" + this.WinID] = dc.usp_DocuemntTaxes_Select(null, null, false).CopyToDataTable();
            }
            return (DataTable)Session["dtTaxes_Receipt" + this.WinID];
        }

        set
        {
            Session["dtTaxes_Receipt" + this.WinID] = value;
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

    private int PurchaseOrderID
    {
        get
        {
            if (ViewState["PurchaseOrderID"] == null) return 0;
            return (int)ViewState["PurchaseOrderID"];
        }

        set
        {
            ViewState["PurchaseOrderID"] = value;
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
            if (Session["dtAllTaxes_Receipt" + this.WinID] == null)
            {
                DataTable dtallTaxes = new DataTable();
                dtallTaxes.Columns.Add("Account_ID", typeof(int));
                dtallTaxes.Columns.Add("OnReceiptType", typeof(string));
                dtallTaxes.Columns.Add("DebitAmount", typeof(decimal));
                dtallTaxes.Columns.Add("CreditAmount", typeof(decimal));
                Session["dtAllTaxes_Receipt" + this.WinID] = dtallTaxes;
            }
            return (DataTable)Session["dtAllTaxes_Receipt" + this.WinID];
        }

        set
        {
            Session["dtAllTaxes_Receipt" + this.WinID] = value;
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

    private decimal TotalServices
    {
        get
        {
            if (ViewState["TotalServices"] == null) return 0;
            return (decimal)ViewState["TotalServices"];
        }

        set
        {
            ViewState["TotalServices"] = value;
        }
    }

    #endregion

    #region Page Events

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
                this.FilterItemsDescribed();
                this.FillReceipt();
                this.VisibilityControl();
                this.txtQty.Text = "1";

                this.IsRequiresField();
                var comp = dc.usp_Company_Select().FirstOrDefault();
                txtCost.Enabled = !comp.IsPriceLoocked.Value;
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
            pnlItemdescribed.Visible = obj.IsDescribed.Value;
            acItemDescribed.IsRequired = obj.IsDescribed.Value;
            gvItems.Columns[3].Visible = obj.IsDescribed.Value;
        }
    }

    private void IsRequiresField()
    {
        foreach (var control in dc.usp_HiddenControls_Select(MyContext.PageData.PageID, MyContext.UserProfile.Contact_ID))
        {
            if (control.ControlUniqueID == "cph_txtPolicy")
            {
                txtPolicy.IsRequired = false;
                gvItems.Columns[6].Visible = false;
            }
            if (control.ControlUniqueID == "cph_txtCode")
            {
                txtCode.IsRequired = false;
                gvItems.Columns[5].Visible = false;
            }
            if (control.ControlUniqueID == "cph_txtInvoiceDate")
            {
                txtInvoiceDate.IsRequired = false;
            }

            if (control.ControlUniqueID == "cph_txtCapacity")
            {

                gvItems.Columns[8].Visible = false;
            }
            if (control.ControlUniqueID == "cph_txtCapacities")
            {

                gvItems.Columns[9].Visible = false;
            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        this.CustomPage();
        ucNewBatchID.TargetControlID = pnlAddItem.Visible && lnkNewBatch.Visible ? lnkNewBatch.UniqueID : hfNewBatch.UniqueID;
        ucNewItemDescribed.TargetControlID = lnkNewDescribed.Visible ? lnkNewDescribed.UniqueID : hfNewDescribed.UniqueID;
        IsRequiresField();
    }

    #endregion

    #region control Events

    protected void acVendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            acShipAddress.ContextKey = acVendor.Value + "," + ContactDetailsTypes.ShipAddress.ToInt().ToExpressString();
            acAddress.ContextKey = acVendor.Value + "," + ContactDetailsTypes.DefaultAddress.ToInt().ToExpressString();
            acPaymentAddress.ContextKey = acVendor.Value + "," + ContactDetailsTypes.PaymentAddress.ToInt().ToExpressString();
            acTelephone.ContextKey = acVendor.Value + "," + ContactDetailsTypes.DefaultTelephone.ToInt().ToExpressString();
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
            txtCost.Text = (dc.fun_GetItemDefaultCostByUOM(acItem.Value.ToNullableInt(), acUnit.Value.ToNullableInt()) / (txtRatio.Text.ToNullableDecimal() ?? 1)).Value.ToString("0.########");
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
            var idItem = int.Parse(acItem.Value);
            var obj = dc.Items.FirstOrDefault(x => x.ID == idItem);
            if (obj != null && obj.MaxQty != 0 && obj.MaxQty <= decimal.Parse(txtQty.Text))
            {
                UserMessages.Message(null, Resources.UserInfoMessages.MaxQtyPerMessage + " (  " + obj.MaxQty.ToString() + "  ) ", string.Empty);
                return;
            }

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

                    var countCode = dc.ReceiptDetails.Count(x => x.IDCodeOperation == txtCode.Text);
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
                    DataRow[] filteredPolicyRows = this.dtItems.Select("Policy=" + txtPolicy.Text);
                    if (filteredPolicyRows.Length > 0)
                    {
                        UserMessages.Message(null, Resources.UserInfoMessages.PolicyAlreadyExist, string.Empty);
                        txtPolicy.Focus();
                        return;
                    }

                    var countPolicy = dc.ReceiptDetails.Count(x => x.Policy == txtPolicy.Text);
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
                if (!string.IsNullOrEmpty(txtCode.Text))
                {
                    //testing if code already exist or not
                    DataRow[] filteredRows = this.dtItems.Select("[IDCodeOperation]='" + txtCode.Text + "'");
                    if (filteredRows.Length > 1)
                    {
                        UserMessages.Message(null, Resources.UserInfoMessages.CodeAlreadyExist, string.Empty);
                        txtCode.Focus();
                        return;
                    }

                    var countCode = dc.ReceiptDetails.Count(x => x.IDCodeOperation == txtCode.Text);
                    if (countCode > 1)
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
                    if (filteredPolicyRows.Length > 1)
                    {
                        UserMessages.Message(null, Resources.UserInfoMessages.PolicyAlreadyExist, string.Empty);
                        txtPolicy.Focus();
                        return;
                    }

                    var countPolicy = dc.ReceiptDetails.Count(x => x.Policy == txtPolicy.Text);
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
            var objCompany = dc.usp_Company_Select().First();
            if (objCompany.IsDescribed == null || !objCompany.IsDescribed.Value)
            {
                r["Item_ID"] = acItem.Value;
                r["ItemName"] = acItem.Text;
            }
            else
            {

                r["Item_ID"] = acItem.Value;
                r["ItemName"] = acItem.Text;

            }
            r["IDCodeOperation"] = txtCode.Text;
            r["Policy"] = txtPolicy.Text;
            //r["ItemDescription"] = txtDescribed.Text;
            r["ItemDescription"] = this.acItemDescribed.Value;
            r["DescribedName"] = this.acItemDescribed.Text;

            r["UnitCost"] = txtCost.Text;
            r["Quantity"] = txtQty.Text;
            r["QtyInNumber"] = txtQtyInNumber.Text.ToDecimalOrDefault();
            r["Capacity"] = txtCapacity.Text;
            r["Capacities"] = txtCapacities.Text;

            r["Uom_ID"] = acUnit.Value;
            r["UOMName"] = acUnit.Text;
            r["Batch_ID"] = acBatchID.Value.ToIntOrDBNULL();
            r["BatchName"] = acBatchID.Text;
            r["PercentageDiscount"] = txtItemPercentageDiscount.Text.ToDecimalOrDefault();
            r["CashDiscount"] = txtItemCashDiscount.Text.ToDecimalOrDefault();
            r["TotalTax"] = 0;
            r["Notes"] = txtItemNotes.Text;
            r["StoreName"] = acStore.Text;
            // r["ItemName"] = acItem.Text;
            r["Barcode"] = txtBarcode.Text;
            r["CategoryName"] = acCategory.Text;
            r["ProductionDate"] = txtProductionDate.Text.ToDateOrDBNULL();
            r["ExpirationDate"] = txtExpirationDate.Text.ToDateOrDBNULL();
            r["ReceiptDate"] = txtInvoiceDate.Text.ToDateOrDBNULL();
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

            var Items = dc.usp_Items_SelectByCategory(acCategory.Value.ToInt(), true);

            foreach (var item in Items)
            {

                r = this.dtItems.NewRow();
                r["ID"] = this.dtItems.GetID("ID");
                r["Store_ID"] = acStore.Value;
                r["Category_ID"] = acCategory.Value;
                r["Item_ID"] = item.ID;
                r["IDCodeOperation"] = txtCode.Text;
                r["Policy"] = txtPolicy.Text;
                r["UnitCost"] = txtCost.Text;
                r["Quantity"] = txtQty.Text;
                r["QtyInNumber"] = txtQtyInNumber.Text.ToDecimalOrDefault();
                r["Capacity"] = txtCapacity.Text;
                r["Capacities"] = txtCapacities.Text;
                r["Uom_ID"] = acUnit.Value;
                r["UOMName"] = acUnit.Text;
                r["Batch_ID"] = acBatchID.Value.ToIntOrDBNULL();
                r["BatchName"] = acBatchID.Text;
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
                r["ReceiptDate"] = txtInvoiceDate.Text.ToDateOrDBNULL();
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
            //txtDescribed.val = r["ItemDescription"].ToExpressString();
            this.acItemDescribed.Value = r["ItemDescription"].ToExpressString();
            txtQty.Text = r["Quantity"].ToExpressString();
            txtCode.Text = r["IDCodeOperation"].ToExpressString();
            txtPolicy.Text = r["Policy"].ToExpressString();
            txtInvoiceDate.Text = r["ReceiptDate"].ToExpressString();
            txtQtyInNumber.Text = r["QtyInNumber"].ToExpressString();
            txtCapacity.Text = r["Capacity"].ToExpressString();

            txtCapacities.Text = r["Capacities"].ToExpressString();
            acUnit.Value = r["Uom_ID"].ToExpressString();
            acItemTax.Value = r["Tax_ID"].ToExpressString();
            acBatchID.Value = r["Batch_ID"].ToExpressString();
            txtItemPercentageDiscount.Text = r["PercentageDiscount"].ToExpressString();
            txtItemCashDiscount.Text = r["CashDiscount"].ToExpressString();
            txtItemNotes.Text = r["Notes"].ToExpressString();
            txtBarcode.Text = r["Barcode"].ToExpressString();
            if (r["ProductionDate"].ToExpressString() != string.Empty) txtProductionDate.Text = r["ProductionDate"].ToDate().Value.ToString("d/M/yyyy"); ;
            if (r["ExpirationDate"].ToExpressString() != string.Empty) txtExpirationDate.Text = r["ExpirationDate"].ToDate().Value.ToString("d/M/yyyy");
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

    protected void ucNewBatchID_NewBatchCreated(string BatchName, string ProductionDate, string ExiprationDate, int Batch_ID)
    {
        try
        {
            acBatchID.Refresh();
            acBatchID.Value = Batch_ID.ToExpressString();
            txtProductionDate.Text = ProductionDate;
            txtExpirationDate.Text = ExiprationDate;
            this.FocusNextControl(lnkNewBatch);
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
            if (acBatchID.HasValue)
            {
                var batch = dc.usp_ItemsBatch_Select(acBatchID.Value.ToInt(), acItem.Value.ToInt(), null, null, false).FirstOrDefault();
                if (batch.ProductionDate.HasValue) txtProductionDate.Text = batch.ProductionDate.Value.ToString("d/M/yyyy");
                if (batch.ExpirationDate.HasValue) txtExpirationDate.Text = batch.ExpirationDate.Value.ToString("d/M/yyyy");
            }
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
            dc.usp_Receipt_Cancel(this.Receipt_ID, MyContext.UserProfile.Contact_ID);
            LogAction(Actions.Delete, txtSerial.Text, dc);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.ReceiptShortcut + "?ID=" + this.Receipt_ID.ToExpressString(), PageLinks.ReceiptsList, PageLinks.ReceiptShortcut);
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
            int Contact_ID = dc.usp_Contact_Insert(MyContext.UserProfile.Branch_ID, ddlFastAddCurrency.SelectedValue.ToInt(), DocSerials.Vendor.ToInt(), txtFastAddName.TrimmedText, 'V', string.Empty, null);
            int ChartofAccount_ID = dc.usp_ChartOfAccount_Insert(txtFastAddName.TrimmedText, txtFastAddName.TrimmedText, acParentAccount.Value.ToInt(), true, MyContext.UserProfile.Branch_ID, ddlFastAddCurrency.SelectedValue.ToInt(), null, null, null, null);

            if (ChartofAccount_ID == -2 || Contact_ID == -2)
            {
                UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                trans.Rollback();
                mpeFastAddNew.Show();
                return;
            }
            dc.usp_Vendors_Insert(Contact_ID, ChartofAccount_ID, null, false, 0);

            trans.Commit();
            if (acVendor.Enabled) acVendor.Value = Contact_ID.ToExpressString();
            LogAction(Actions.Add, "اضافة مورد سريع: " + txtFastAddName.TrimmedText, dc);
            this.CloseFastAddNewPopup_Click(null, null);
            this.FocusNextControl(lnkAddNewVendor);
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
            mpeFastAddNew.Hide();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        XpressDataContext dataContext = new XpressDataContext();
        try
        {
            var databaseName = dataContext.Connection.Database;

            ReportDocument doc = new ReportDocument();
            var receipt = dc.usp_Receipt_SelectByID(this.Receipt_ID);
            var totalAmount = receipt.Select(x => x.GrossTotalAmount).FirstOrDefault();




            doc.Load(Server.MapPath("~\\Reports\\Receipt_Print ssb.rpt"));
            doc.SetParameterValue("@Receipt_ID", this.Receipt_ID);
            //doc.SetParameterValue("@DocTableType_ID", DocumentsTableTypes.Receipt.ToInt(), "DocumentTaxes.rpt");
            //doc.SetParameterValue("@Doc_ID", this.Receipt_ID, "DocumentTaxes.rpt");
            //doc.SetParameterValue("@IsReturn", false, "DocumentTaxes.rpt");

            doc.SetParameterValue("@TheNo1", totalAmount.ToDecimalOrDefault().ToString(), "Tafkit.rpt");
            doc.SetParameterValue("@TheNo2","0","Tafkit.rpt");
            Response.Redirect(
                PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "Receipt"), false);

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnPrintOrderIn_Click(object sender, EventArgs e)
    {
        try
        {
            ReportDocument doc = new ReportDocument();
            doc.Load(Server.MapPath("~\\Reports\\PurchaseInvOrderIn_Print.rpt"));
            doc.SetParameterValue("@Receipt_ID", this.Receipt_ID);
            Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "OrderIn"), false);
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
            acVendor.ContextKey = "V," + acBranch.Value + "," + ddlCurrency.SelectedValue + ",";
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
        acItem.ContextKey = "," + acCategory.Value + ",,true";
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
        txtBarcode.Clear();
        txtItemNotes.Clear();
         txtQty.Clear();
        // txtCItem.Clear();
        txtQtyInNumber.Clear();
        txtCapacity.Clear();
        txtCapacities.Clear();
        acBatchID.ContextKey = acItem.Value + ",";
        ucNewBatchID.ItemID = acItem.Value;
        ucNewItemDescribed.ItemID = acItem.Value;
        acUnit.ContextKey = string.Empty + acItem.Value;
        if (acItem.HasValue)
        {
            var item = dc.usp_Items_SelectByID(acItem.Value.ToNullableInt()).FirstOrDefault();
            txtCost.Text = (item.Cost / (txtRatio.Text.ToNullableDecimal() ?? 1)).ToString("0.########");
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
        acTax.ContextKey = acItemTax.ContextKey = DocumentsTableTypes.Receipt.ToInt().ToExpressString() + ",false";
        this.FilterItems();
        this.BindItemsGrid();
        lblCreatedBy.Text = MyContext.UserProfile.EmployeeName;
        gvTaxes.DataSource = this.dtTaxes;
        gvTaxes.DataBind();

        acParentAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString();
        acParentAccount.Value = COA.Vendors.ToInt().ToExpressString();
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
            this.Receipt_ID = Request["ID"].ToInt();
        }

        if (Request["PurchaseOrderID"] != null)
        {
            this.PurchaseOrderID = Request["PurchaseOrderID"].ToInt();
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

        if (txtFirstPaid.Text.ToDecimalOrDefault() > this.GrossTotal)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.FirstPaidGreaterThanTotal, string.Empty);
            trans.Rollback();
            return false;
        }


        this.TotalServices = 0;
        string Serial = string.Empty;
        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        DateTime? ApproveDate = IsApproving ? DateTime.Now : (DateTime?)null;
        int? approvedBY_ID = IsApproving ? MyContext.UserProfile.Contact_ID : (int?)null;
        byte EntryType = 2;
        int Serial_ID = DocSerials.ReceiptEntry.ToInt();
        int? PurchaseOrderID = this.PurchaseOrderID == 0 ? (int?)null : this.PurchaseOrderID;
        int Detail_ID = 0;
        if (!this.EditMode)
        {

            this.Receipt_ID = dc.usp_Receipt_Insert(acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimalOrDefault(), txtOperationDate.Text.ToDate(),
                                                     acVendor.Value.ToInt(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate,
                                                     approvedBY_ID, null, null, acAddress.Value.ToNullableInt(), acShipAddress.Value.ToNullableInt(), acPaymentAddress.Value.ToNullableInt(),
                                                     acTelephone.Value.ToNullableInt(), acCostCenter.Value.ToNullableInt(), txtNotes.Text, lblTotal.Text.ToDecimal(), txtPercentageDiscount.Text.ToDecimalOrDefault(),
                                                     txtCashDiscount.Text.ToDecimalOrDefault(), lblTotalDiscount.Text.ToDecimalOrDefault(), lblTotalTax.Text.ToDecimalOrDefault(), lblGrossTotal.Text.ToDecimalOrDefault(), txtFirstPaid.Text.ToDecimalOrDefault(), null, txtUserRefNo.Text, this.DocRandomString, EntryType, PurchaseOrderID, acCashAccount.Value.ToNullableInt(), null);



            if (this.Receipt_ID > 0)
            {
                foreach (DataRow r in this.dtItems.Rows)
                {
                    if (r.RowState == DataRowState.Deleted) continue;
                    // Detail_ID = dc.usp_ReceiptDetails_Insert(this.Receipt_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["Capacity"].ToString());
                    Detail_ID = dc.usp_ReceiptDetailsWithDesciption_Insert(this.Receipt_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["Capacity"].ToString(), r["ItemDescription"].ToString(), r["IDCodeOperation"].ToString(), r["Policy"].ToString(), r["ReceiptDate"].ToDate(), r["Capacities"].ToString());



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
                    dc.usp_DocuemntTaxes_Insert(DocumentsTableTypes.Receipt.ToInt(), this.Receipt_ID, r["Tax_ID"].ToInt());
                }
               // if (IsApproving) dc.usp_ICJ_NegativeRefill(txtOperationDate.Text.ToDate(), acBranch.Value.ToNullableInt(), DocumentsTableTypes.Receipt.ToInt(), this.Receipt_ID);
              //  if (IsApproving) this.InsertOperation();
                LogAction(IsApproving ? Actions.Approve : Actions.Add, Serial, dc);
            }
        }
        else
        {
            int Result = dc.usp_Receipt_Update(this.Receipt_ID, acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimalOrDefault(), txtOperationDate.Text.ToDate(),
                                                    acVendor.Value.ToInt(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate, approvedBY_ID,
                                                    null, null, acAddress.Value.ToNullableInt(), acShipAddress.Value.ToNullableInt(), acPaymentAddress.Value.ToNullableInt(), acTelephone.Value.ToNullableInt(),
                                                    acCostCenter.Value.ToNullableInt(), txtNotes.Text, lblTotal.Text.ToDecimal(), txtPercentageDiscount.Text.ToDecimalOrDefault(), txtCashDiscount.Text.ToDecimalOrDefault(),
                                                    lblTotalDiscount.Text.ToDecimalOrDefault(), lblTotalTax.Text.ToDecimalOrDefault(), lblGrossTotal.Text.ToDecimalOrDefault(), txtFirstPaid.Text.ToDecimalOrDefault(), txtUserRefNo.Text, this.DocRandomString, acCashAccount.Value.ToNullableInt(), null);
            if (Result > 0)
            {
                foreach (DataRow r in this.dtItems.Rows)
                {
                    if (r.RowState == DataRowState.Added)
                    {
                        //Detail_ID = dc.usp_ReceiptDetails_Insert(this.Receipt_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["Capacity"].ToString());
                        Detail_ID = dc.usp_ReceiptDetailsWithDesciption_Insert(this.Receipt_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["Capacity"].ToString(), r["ItemDescription"].ToString(), r["IDCodeOperation"].ToString(), r["Policy"].ToString(), r["ReceiptDate"].ToDate(), r["Capacities"].ToString());
                    }
                    if (r.RowState == DataRowState.Modified)
                    {
                        Detail_ID = r["ID"].ToInt();
                        dc.usp_ReceiptDetails_Update(r["ID"].ToInt(), r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault());
                    }
                    if (r.RowState == DataRowState.Deleted)
                    {
                        dc.usp_ReceiptDetails_Delete(r["ID", DataRowVersion.Original].ToInt());
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
                        dc.usp_DocuemntTaxes_Insert(DocumentsTableTypes.Receipt.ToInt(), this.Receipt_ID, r["Tax_ID"].ToInt());
                    }
                    if (r.RowState == DataRowState.Deleted)
                    {
                        dc.usp_DocuemntTaxes_Delete(r["ID", DataRowVersion.Original].ToInt());
                    }
                }
               // if (IsApproving) dc.usp_ICJ_NegativeRefill(txtOperationDate.Text.ToDate(), acBranch.Value.ToNullableInt(), DocumentsTableTypes.Receipt.ToInt(), this.Receipt_ID);
               // if (IsApproving) this.InsertOperation();
                LogAction(IsApproving ? Actions.Approve : Actions.Edit, txtSerial.Text, dc);
            }
        }
        Serial = Serial == string.Empty ? string.Empty : " (" + Serial + ") ";
        UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess + Serial, PageLinks.ReceiptEntry + "?ID=" + this.Receipt_ID.ToExpressString(), PageLinks.ReceiptsListEntry, PageLinks.ReceiptEntry);
        return true;
    }

  

    private bool InsertICJ(int Detail_ID, DataRow row)
    {
        int result = 0;
        bool? IsService = false;
        decimal ItemTotal = row["Total"].ToDecimal() * txtRatio.Text.ToDecimal();
        result = dc.usp_ICJ_Receipt(txtOperationDate.Text.ToDate(), row["Quantity"].ToDecimal(), row["Item_ID"].ToInt(), row["Uom_ID"].ToInt(), ItemTotal, row["Batch_ID"].ToNullableInt(), row["Store_ID"].ToInt(), acBranch.Value.ToNullableInt(), DocumentsTableTypes.Receipt.ToInt(), this.Receipt_ID, Detail_ID, ref IsService);
        if (IsService.Value) this.TotalServices += row["Total"].ToDecimal();

        if (result == -666)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.VeryLowCost + " (" + row["StoreName"] + " : " + row["ItemName"].ToExpressString() + ")", string.Empty);
            return false;
        }
        return true;
    }

    private void FillReceipt()
    {
        if (this.Receipt_ID == 0 && this.PurchaseOrderID == 0) return;
        var receipt = dc.usp_Receipt_SelectByID(EditMode ? this.Receipt_ID : this.PurchaseOrderID).FirstOrDefault();
        ddlCurrency.SelectedValue = receipt.Currency_ID.ToExpressString();
        txtRatio.Text = receipt.Ratio.ToExpressString();
        acBranch.Value = receipt.Branch_ID.ToStringOrEmpty();
        this.FilterByBranchAndCurrency();
        txtOperationDate.Text = receipt.OperationDate.Value.ToString("d/M/yyyy");
        acCostCenter.Value = receipt.CostCenter_ID.ToStringOrEmpty();
        txtUserRefNo.Text = receipt.UserRefNo;
        acVendor.Value = receipt.Contact_ID.ToExpressString();
        this.acVendor_SelectedIndexChanged(null, null);
        acTelephone.Value = receipt.Telephone_ID.ToStringOrEmpty();
        acAddress.Value = receipt.DefaultAddress_ID.ToStringOrEmpty();
        acShipAddress.Value = receipt.ShipToAddress_ID.ToStringOrEmpty();
        acPaymentAddress.Value = receipt.PaymentAddress_ID.ToStringOrEmpty();
        acCashAccount.Value = receipt.CashAccount_ID.ToStringOrEmpty();
        txtCashDiscount.Text = receipt.CashDiscount.ToExpressString();
        txtPercentageDiscount.Text = receipt.PercentageDiscount.ToExpressString();
        txtFirstPaid.Text = receipt.FirstPaid.ToExpressString();
        this.txtFirstPaid_TextChanged(null, null);
        lblTotal.Text = receipt.TotalAmount.ToExpressString();
        lblTotalDiscount.Text = receipt.TotalDiscount.ToExpressString();
        lblTotalTax.Text = receipt.TotalTax.ToExpressString();
        lblGrossTotal.Text = receipt.GrossTotalAmount.ToExpressString();
        txtNotes.Text = receipt.Notes;

        if (!string.IsNullOrEmpty(receipt.PurchaseOrderSerial) || this.PurchaseOrderID != 0)
        {
            lblPurchaseOrderNo.Text = EditMode ? receipt.PurchaseOrderSerial : receipt.Serial;
            divPurchaseOrderNo.Visible = true;
            ddlCurrency.Enabled = false;
            acBranch.Enabled = false;
            acVendor.Enabled = false;
        }

        if (EditMode)
        {
            txtSerial.Text = receipt.Serial;
            this.DocRandomString = receipt.DocRandomString;
            lblCreatedBy.Text = receipt.CreatedByName;
            lblApprovedBy.Text = receipt.ApprovedBYName;
            this.ImgStatus = ((DocStatus)receipt.DocStatus_ID).ToExpressString();
            btnPrint.Visible = MyContext.PageData.IsPrint;
            btnPrintInventoryOrder.Visible = MyContext.PageData.IsPrint;
            pnlAddItem.Visible = (receipt.DocStatus_ID == 1);
            btnCancel.Visible = (receipt.DocStatus_ID == 1) && MyContext.PageData.IsDelete;
            btnApprove.Visible = (receipt.DocStatus_ID == 1) && MyContext.PageData.IsApprove;
            btnSave.Visible = (receipt.DocStatus_ID == 1) && ((MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode));
            btnCancelApprove.Visible = !btnApprove.Visible && (receipt.DocStatus_ID == 2) && MyContext.PageData.IsNotApprove;
            gvItems.Columns[gvItems.Columns.Count - 1].Visible = gvItems.Columns[gvItems.Columns.Count - 2].Visible = (receipt.DocStatus_ID == 1);
            gvTaxes.Columns[gvTaxes.Columns.Count - 1].Visible = (receipt.DocStatus_ID == 1);
        }
        this.dtItems = dc.usp_ReceiptDetailsWithDescription_Select(EditMode ? this.Receipt_ID : this.PurchaseOrderID).CopyToDataTable();

        // this.dtItems = dc.usp_ReceiptDetails_Select(EditMode ? this.Receipt_ID : this.PurchaseOrderID).CopyToDataTable();

        this.dtTaxes = dc.usp_DocuemntTaxes_Select(DocumentsTableTypes.Receipt.ToInt(), EditMode ? this.Receipt_ID : this.PurchaseOrderID, false).CopyToDataTable();
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
        MyContext con = new MyContext(System.Web.Security.Membership.GetUser(), PageLinks.Vendors, string.Empty);
        btnFastAddNew.Visible = con.PageData.IsAdd;
    }

    private void Calculate()
    {
        decimal ItemDiscount = 0;
        decimal DocDiscount = 0;
        decimal DocTax = 0;
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
            if (r["TaxOnReceiptType"] != DBNull.Value && r["Tax_ID"] != DBNull.Value)
            {
                r["TotalTax"] = ((r["Total"].ToDecimal() - ItemDiscount) * r["TaxPercentageValue"].ToDecimal() * 0.01m);

                if (r["TaxOnReceiptType"].ToExpressString() == "C")
                {
                    this.dtAllTaxes.Rows.Add(r["TaxPurchaseAccountID"].ToInt(), r["TaxOnReceiptType"].ToExpressString(), 0, r["TotalTax"].ToDecimal());
                    this.TotalCreditTax += r["TotalTax"].ToDecimal();
                    r["TotalTax"] = r["TotalTax"].ToDecimal() * -1;
                }
                else
                {
                    this.dtAllTaxes.Rows.Add(r["TaxPurchaseAccountID"].ToInt(), r["TaxOnReceiptType"].ToExpressString(), r["TotalTax"].ToDecimal(), 0);
                    this.TotalDebitTax += r["TotalTax"].ToDecimal();
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
            if (r["OnReceiptType"] != DBNull.Value)
            {
                DocTaxValue = ((this.GrossTotal - DocDiscount) * r["PercentageValue"].ToDecimal() * 0.01m);
                if (r["OnReceiptType"].ToExpressString() == "C")
                {
                    this.dtAllTaxes.Rows.Add(r["PurchaseAccountID"].ToInt(), r["OnReceiptType"].ToExpressString(), 0, DocTaxValue);
                    this.TotalCreditTax += DocTaxValue;
                    DocTaxValue *= -1m;
                }
                else
                {
                    this.dtAllTaxes.Rows.Add(r["PurchaseAccountID"].ToInt(), r["OnReceiptType"].ToExpressString(), DocTaxValue, 0);
                    this.TotalDebitTax += DocTaxValue;
                }
                DocTax += DocTaxValue;
            }
        }
        this.TotalDiscount += DocDiscount;
        this.TotalTax += DocTax;
        this.GrossTotal = this.GrossTotal - DocDiscount + DocTax;
        lblTotal.Text = this.Total.ToString("0.########");
        lblTotalDiscount.Text = this.TotalDiscount.ToString("0.########");
        lblTotalTax.Text = this.TotalTax.ToString("0.########");
        lblGrossTotal.Text = this.GrossTotal.ToString("0.########");
        if (acCashAccount.HasValue && Page.IsPostBack && this.SumFirstPaid)
        {
            txtFirstPaid.Text = lblGrossTotal.Text;
            this.txtFirstPaid_TextChanged(null, null);
        }
    }

 

    private void CustomPage()
    {
        acCostCenter.Visible = MyContext.Features.CostCentersEnabled;
        acBatchID.Visible = MyContext.Features.BatchIDEnabled;
        txtProductionDate.Visible = MyContext.Features.BatchIDEnabled;
        txtExpirationDate.Visible = MyContext.Features.BatchIDEnabled;
        lnkNewBatch.Visible = MyContext.Features.BatchIDEnabled;

        acItemTax.Visible = MyContext.Features.TaxesEnabled;
        taxSection.Visible = MyContext.Features.TaxesEnabled;
        taxTotal.Visible = MyContext.Features.TaxesEnabled;
        DiscountTotal.Visible = MyContext.Features.CashDiscountEnabled || MyContext.Features.PercentageDiscountEnabled;
        acBranch.IsRequired = acBranch.Visible = MyContext.Features.BranchesEnabled;
        txtCashDiscount.Visible = txtItemCashDiscount.Visible = MyContext.Features.CashDiscountEnabled;
        txtPercentageDiscount.Visible = txtItemPercentageDiscount.Visible = MyContext.Features.PercentageDiscountEnabled;

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
        //if (acStore.HasValue) this.acStore_SelectedIndexChanged(null, null);

        txtOperationDate.Enabled = !company.LockAutoDate.Value;
        acCashAccount.Enabled = !((MyContext.UserProfile.CashierAccount_ID != null) && ((MyContext.Features.BranchesEnabled && MyContext.UserProfile.Branch_ID != null) || !MyContext.Features.BranchesEnabled));
        acStore.Enabled = !((MyContext.UserProfile.Store_ID != null) && ((MyContext.Features.BranchesEnabled && MyContext.UserProfile.Branch_ID != null) || !MyContext.Features.BranchesEnabled));
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

    protected void gvItems_OnPreRender(object sender, EventArgs e)
    {
        //foreach (var control in dc.usp_HiddenControls_Select(MyContext.PageData.PageID, MyContext.UserProfile.Contact_ID))
        //{
        //    if (control.ControlUniqueID == "cph_txtPolicy")
        //    {
        //        gvItems.Columns[7].Visible = false;
        //    }
        //    if (control.ControlUniqueID == "cph_txtCode")
        //    {
        //        gvItems.Columns[6].Visible = false;
        //    }
        //}
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

    protected void btnAddItemException_OnClick(object sender, EventArgs e)
    {

        try
        {
            var idItem = int.Parse(acItem.Value);
            var obj = dc.Items.FirstOrDefault(x => x.ID == idItem);
            if (obj != null && obj.MaxQty != 0 && obj.MaxQty <= decimal.Parse(txtQty.Text))
            {
                UserMessages.Message(null, Resources.UserInfoMessages.MaxQtyPerMessage + " (  " + obj.MaxQty.ToString() + "  ) ", string.Empty);
                return;
            }

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

                    var countCode = dc.ReceiptDetails.Count(x => x.IDCodeOperation == txtCode.Text);
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
                    DataRow[] filteredPolicyRows = this.dtItems.Select("Policy=" + txtPolicy.Text);
                    if (filteredPolicyRows.Length > 0)
                    {
                        UserMessages.Message(null, Resources.UserInfoMessages.PolicyAlreadyExist, string.Empty);
                        txtPolicy.Focus();
                        return;
                    }

                    var countPolicy = dc.ReceiptDetails.Count(x => x.Policy == txtPolicy.Text);
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
                if (!string.IsNullOrEmpty(txtCode.Text))
                {
                    //testing if code already exist or not
                    DataRow[] filteredRows = this.dtItems.Select("[IDCodeOperation]='" + txtCode.Text + "'");
                    if (filteredRows.Length > 1)
                    {
                        UserMessages.Message(null, Resources.UserInfoMessages.CodeAlreadyExist, string.Empty);
                        txtCode.Focus();
                        return;
                    }

                    var countCode = dc.ReceiptDetails.Count(x => x.IDCodeOperation == txtCode.Text);
                    if (countCode > 1)
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
                    if (filteredPolicyRows.Length > 1)
                    {
                        UserMessages.Message(null, Resources.UserInfoMessages.PolicyAlreadyExist, string.Empty);
                        txtPolicy.Focus();
                        return;
                    }

                    var countPolicy = dc.ReceiptDetails.Count(x => x.Policy == txtPolicy.Text);
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
            var objCompany = dc.usp_Company_Select().First();
            if (objCompany.IsDescribed == null || !objCompany.IsDescribed.Value)
            {
                r["Item_ID"] = acItem.Value;
                r["ItemName"] = acItem.Text;
            }
            else
            {

                r["Item_ID"] = acItem.Value;
                r["ItemName"] = acItem.Text;

            }
            r["IDCodeOperation"] = txtCode.Text;
            r["Policy"] = txtPolicy.Text;
            //r["ItemDescription"] = txtDescribed.Text;
            r["ItemDescription"] = this.acItemDescribed.Value;
            r["DescribedName"] = this.acItemDescribed.Text;

            r["UnitCost"] = txtCost.Text;
            r["Quantity"] = txtQty.Text;
            r["QtyInNumber"] = txtQtyInNumber.Text.ToDecimalOrDefault();
            r["Capacity"] = txtCapacity.Text;
            r["Capacities"] = txtCapacities.Text;

            r["Uom_ID"] = acUnit.Value;
            r["UOMName"] = acUnit.Text;
            r["Batch_ID"] = acBatchID.Value.ToIntOrDBNULL();
            r["BatchName"] = acBatchID.Text;
            r["PercentageDiscount"] = txtItemPercentageDiscount.Text.ToDecimalOrDefault();
            r["CashDiscount"] = txtItemCashDiscount.Text.ToDecimalOrDefault();
            r["TotalTax"] = 0;
            r["Notes"] = txtItemNotes.Text;
            r["StoreName"] = acStore.Text;
            // r["ItemName"] = acItem.Text;
            r["Barcode"] = txtBarcode.Text;
            r["CategoryName"] = acCategory.Text;
            r["ProductionDate"] = txtProductionDate.Text.ToDateOrDBNULL();
            r["ExpirationDate"] = txtExpirationDate.Text.ToDateOrDBNULL();
            r["ReceiptDate"] = txtInvoiceDate.Text.ToDateOrDBNULL();
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
            acItemDescribed.Clear();
            // this.ClearItemForm();
            this.BindItemsGrid();
            this.FocusNextControl(acStore);
            this.EditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }


    }

    protected void btnCancelApprove_Click(object sender, EventArgs e)
    {
        //System.Data.Common.DbTransaction trans;
        //dc.Connection.Open();
        //trans = dc.Connection.BeginTransaction();
        //dc.Transaction = trans;
        try
        {
            dc.usp_CancelReceipt_Approvel(this.Receipt_ID);
            LogAction(Actions.NotApprove, txtSerial.Text, dc);
            // trans.Commit();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.ReceiptEntry + "?ID=" + this.Receipt_ID.ToExpressString(), PageLinks.ReceiptsListEntry, PageLinks.ReceiptEntry);

        }
        catch (Exception ex)
        {
            // trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
}