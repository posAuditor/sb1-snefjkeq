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

public partial class Sales_ReturnInvoice : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private int IsTaxFound
    {
        get
        {
            if (ViewState["IsTaxFound"] == null) return 0;
            return (int)ViewState["IsTaxFound"];
        }

        set
        {
            ViewState["IsTaxFound"] = value;
        }
    }
    private int TypeTax
    {
        get
        {
            if (Session["TypeTax_Invoice" + this.WinID] == null)
            {
                // Session["dtItems_Invoice" + this.WinID] = dc.usp_InvoiceDetails_Select(null).CopyToDataTable();
                Session["TypeTax_Invoice" + this.WinID] = dc.usp_Company_Select().FirstOrDefault().TypeTax;
            }
            return (int)Session["TypeTax_Invoice" + this.WinID];
        }

        set
        {
            Session["TypeTax_Invoice" + this.WinID] = value;
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

    private int ReturnInvoice_ID
    {
        get
        {
            if (ViewState["ReturnInvoice_ID"] == null) return 0;
            return (int)ViewState["ReturnInvoice_ID"];
        }

        set
        {
            ViewState["ReturnInvoice_ID"] = value;
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
            if (Session["dtItems_ReturnInvoice" + this.WinID] == null)
            {
                Session["dtItems_ReturnInvoice" + this.WinID] = dc.usp_ReturnInvoiceDetailsIncludeTax_Select(null).CopyToDataTable();
            }
            return (DataTable)Session["dtItems_ReturnInvoice" + this.WinID];
        }

        set
        {
            Session["dtItems_ReturnInvoice" + this.WinID] = value;
        }
    }

    private DataTable dtTaxes
    {
        get
        {
            if (Session["dtTaxes_ReturnInvoice" + this.WinID] == null)
            {
                Session["dtTaxes_ReturnInvoice" + this.WinID] = dc.usp_DocuemntTaxes_Select(null, null, true).CopyToDataTable();
            }
            return (DataTable)Session["dtTaxes_ReturnInvoice" + this.WinID];
        }

        set
        {
            Session["dtTaxes_ReturnInvoice" + this.WinID] = value;
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

    private int? FromInvoice_ID
    {
        get
        {
            return (int?)ViewState["FromInvoice_ID"];
        }

        set
        {
            ViewState["FromInvoice_ID"] = value;
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
            if (Session["dtAllTaxes_ReturnInvoice" + this.WinID] == null)
            {
                DataTable dtallTaxes = new DataTable();
                dtallTaxes.Columns.Add("Account_ID", typeof(int));
                dtallTaxes.Columns.Add("OnInvoiceType", typeof(string));
                dtallTaxes.Columns.Add("DebitAmount", typeof(decimal));
                dtallTaxes.Columns.Add("CreditAmount", typeof(decimal));
                Session["dtAllTaxes_ReturnInvoice" + this.WinID] = dtallTaxes;
            }
            return (DataTable)Session["dtAllTaxes_ReturnInvoice" + this.WinID];
        }

        set
        {
            Session["dtAllTaxes_ReturnInvoice" + this.WinID] = value;
        }
    }

    private decimal CalculatedReturnSalesCost
    {
        get
        {
            if (ViewState["CalculatedReturnSalesCost"] == null) return 0;
            return (decimal)ViewState["CalculatedReturnSalesCost"];
        }

        set
        {
            ViewState["CalculatedReturnSalesCost"] = value;
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

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.gvItems.FormatNumber = MyContext.FormatNumber;
            this.SetEditMode();


            if (this.FromInvoice_ID > 0 || this.ReturnInvoice_ID > 0)
            {

            }
            else
            {
                UserMessages.Message(null, "لا  يمكن الارجاع من هنا", string.Empty);
                return;
            }

            if (!Page.IsPostBack)
            {
                this.DocRandomString.ToString();
                this.CheckSecurity();
                this.LoadControls();
                this.SetDefaults();
                if (EditMode)
                {
                    this.FillReturnInvoice();
                }
                else
                {
                    this.FillFromInvoice();
                }
                var comp = dc.usp_Company_Select().FirstOrDefault();
                txtCost.Enabled = !comp.IsPriceLoocked.Value;
                txtCost.Enabled = MyContext.PageData.IsPrice;

            }


            ucNav.SourceDocTypeType_ID = DocumentsTableTypes.ReturnInvoice.ToInt();
            ucNav.Res_ID = this.ReturnInvoice_ID;
            ucNav.IsPermShow = (MyContext.UserProfile.HasPermissionShow == false ? (int?)null : MyContext.UserProfile.Contact_ID);


            if (Page.IsPostBack)
            {
                WebControl wcICausedPostBack = (WebControl)GetControlThatCausedPostBack(sender as System.Web.UI.Page);
                WebControl wcICausedPostBack1 = (WebControl)GetControlThatCausedPostBack1(sender as System.Web.UI.Page);



                if (wcICausedPostBack != null && wcICausedPostBack1 != null)
                {
                    int indx = wcICausedPostBack1.TabIndex;

                    var nextControlIndex = indx + 1;
                    switch (nextControlIndex)
                    {
                        case 1: acItem.AutoCompleteFocus();
                            break;
                        case 2: txtQty.Focus();
                            break;
                        case 3: txtQty.Focus();
                            break;
                        case 4: txtCost.Focus();
                            break;
                        case 6: LinkButton1.Focus(); //txtItemCashDiscount.Focus();
                            break;
                        case 5: LinkButton1.Focus();// txtItemPercentageDiscount.Focus();
                            break;
                        case 7: LinkButton1.Focus();
                            break;
                        default:
                            txtBarcode.Focus();
                            break;
                    }



                }

            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected Control GetControlThatCausedPostBack1(System.Web.UI.Page page)
    {
        Control control = null;

        string ctrlname = page.Request.Params.Get("__EVENTTARGET");
        if (ctrlname.StartsWith("cph_ac"))
        {   //""ctl00$cph$acItem$hfAutocomplete""
            ctrlname = "ctl00$" + ctrlname.Replace("_", "$");
        }
        if (ctrlname != null && ctrlname != string.Empty)
        {
            control = page.FindControl(ctrlname);
        }
        else
        {
            foreach (string ctl in page.Request.Form)
            {
                Control c = page.FindControl(ctl);
                if (c is System.Web.UI.WebControls.Button || c is System.Web.UI.WebControls.ImageButton)
                {
                    control = c;
                    break;
                }
            }
        }
        return control;

    }
    protected Control GetControlThatCausedPostBack(System.Web.UI.Page page)
    {
        Control control = null;

        string ctrlname = page.Request.Params.Get("__EVENTTARGET");
        if (ctrlname.StartsWith("cph_ac"))
        {   //""ctl00$cph$acItem$hfAutocomplete""
            ctrlname = "ctl00$" + ctrlname.Replace("_", "$").Replace("txt", "hf").Replace("Text", ""); ;
        }
        if (ctrlname != null && ctrlname != string.Empty)
        {
            control = page.FindControl(ctrlname);
        }
        else
        {
            foreach (string ctl in page.Request.Form)
            {
                Control c = page.FindControl(ctl);
                if (c is System.Web.UI.WebControls.Button || c is System.Web.UI.WebControls.ImageButton)
                {
                    control = c;
                    break;
                }
            }
        }
        return control;

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
            // if (acItem.HasValue) txtQty.Focus(); else this.FocusNextControl(acStore);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        this.CustomPage();
        ucNewBatchID.TargetControlID = pnlAddItem.Visible && lnkNewBatch.Visible ? lnkNewBatch.UniqueID : hfNewBatch.UniqueID;
    }
    #endregion

    #region control Events

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
            //  if (sender != null) this.FocusNextControl(sender);
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
            txtCost.Text = (dc.fun_GetItemDefaultPriceByUOM(acItem.Value.ToNullableInt(), acUnit.Value.ToNullableInt(), acPriceName.Value.ToNullableInt()) / (txtRatio.Text.ToNullableDecimal() ?? 1)).Value.ToString(MyContext.FormatNumber);
            this.ShowAvailableQty();
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
            if (sender != null) txtCost.Text = (dc.fun_GetItemDefaultPriceByUOM(acItem.Value.ToNullableInt(), acUnit.Value.ToNullableInt(), acPriceName.Value.ToNullableInt()) / (txtRatio.Text.ToNullableDecimal() ?? 1)).Value.ToString(MyContext.FormatNumber);
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

            DataRow r = null;
            if (this.EditID == 0)
            {
                if (this.FromInvoice_ID != null)
                {
                    UserMessages.Message(null, Resources.UserInfoMessages.CanNOTAddItems, string.Empty);
                    return;
                }
                r = this.dtItems.NewRow();
                r["ID"] = this.dtItems.GetID("ID");

            }
            else
            {
                r = this.dtItems.Select("ID=" + this.EditID)[0];
            }
            if (ddlTvae.SelectedValue.ToNullableInt() == 2)
            {
                acItemTax.Clear();
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
                if (acItemDescribed.Value != null)
                {
                    r["Item_ID"] = acItemDescribed.Value;
                    r["ItemName"] = acItemDescribed.Text;
                }
                else
                {
                    r["Item_ID"] = acItem.Value;
                    r["ItemName"] = acItem.Text;
                }
            }
            r["UnitCost"] = txtCost.Text;
            r["UnitCostEvaluate"] = txtCost.Text;
            r["Quantity"] = txtQty.Text;
            r["QtyInNumber"] = txtQtyInNumber.Text.ToDecimalOrDefault();
            r["Uom_ID"] = acUnit.Value;
            r["UOMName"] = acUnit.Text;
            r["Batch_ID"] = acBatchID.Value.ToIntOrDBNULL();
            r["BatchName"] = acBatchID.Text;
            r["PercentageDiscount"] = txtItemPercentageDiscount.Text.ToDecimalOrDefault();
            r["CashDiscount"] = txtItemCashDiscount.Text.ToDecimalOrDefault();
            r["TotalTax"] = 0;
            r["Notes"] = txtItemNotes.Text;
            r["StoreName"] = acStore.Text;
            r["ItemName"] = acItem.Text;
            r["Barcode"] = txtBarcode.Text;
            r["CategoryName"] = acCategory.Text;
            r["ProductionDate"] = txtProductionDate.Text.ToDateOrDBNULL();
            r["ExpirationDate"] = txtExpirationDate.Text.ToDateOrDBNULL();


            // var Tax = dc.usp_Taxes_Select(acItemTax.Value.ToInt(), string.Empty).FirstOrDefault();

            ////  r["TotalCostBeforTax"] = txtCost.Text.ToDecimalOrDefault() * txtQty.Text.ToDecimalOrDefault();
            //decimal taxInclude = TypeTax == 2 ? (Decimal.Divide((Tax != null ? Tax.PercentageValue.Value : 0), (100M + (Tax != null ? Tax.PercentageValue.Value : 0)))) : Decimal.Divide((Tax != null ? Tax.PercentageValue.Value : 0), (100M));
            //decimal unitCost = TypeTax == 2 ? (txtCost.Text.ToDecimalOrDefault() - txtCost.Text.ToDecimalOrDefault() * taxInclude) : txtCost.Text.ToDecimalOrDefault();
            //r["TotalCostBeforTax"] = Math.Round(unitCost * txtQty.Text.ToDecimalOrDefault(), 3).ToDecimalOrDefault();




            if (acItemTax.HasValue)
            {
                var Tax = dc.usp_Taxes_Select(acItemTax.Value.ToInt(), string.Empty).FirstOrDefault();
                r["TaxName"] = Tax.Name;
                r["Tax_ID"] = acItemTax.Value;
                r["TaxPercentageValue"] = Tax.PercentageValue;
                if (Tax.OnInvoiceType.HasValue) r["TaxOnInvoiceType"] = Tax.OnInvoiceType == 'C' ? 'D' : 'C';
                if (Tax.OnReceiptType.HasValue) r["TaxOnReceiptType"] = Tax.OnReceiptType == 'C' ? 'D' : 'C';
                if (Tax.OnDocCreditType.HasValue) r["TaxOnDocCreditType"] = Tax.OnDocCreditType == 'C' ? 'D' : 'C';
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

            if (this.FromInvoice_ID != null)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.CanNOTAddItems, string.Empty);
                return;
            }

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

                if (acItemTax.HasValue)
                {
                    var Tax = dc.usp_Taxes_Select(acItemTax.Value.ToInt(), string.Empty).FirstOrDefault();
                    r["TaxName"] = Tax.Name;
                    r["Tax_ID"] = acItemTax.Value;
                    r["TaxPercentageValue"] = Tax.PercentageValue;
                    if (Tax.OnInvoiceType.HasValue) r["TaxOnInvoiceType"] = Tax.OnInvoiceType == 'C' ? 'D' : 'C';
                    if (Tax.OnReceiptType.HasValue) r["TaxOnReceiptType"] = Tax.OnReceiptType == 'C' ? 'D' : 'C';
                    if (Tax.OnDocCreditType.HasValue) r["TaxOnDocCreditType"] = Tax.OnDocCreditType == 'C' ? 'D' : 'C';
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
            // txtCost.Text = r["UnitCost"].ToExpressString();
            // txtCost.Text = r["UnitCostEvaluate"].ToExpressString();
            txtCost.Text = r["UnitCostEvaluate"].ToExpressString();

            txtQty.Text = r["Quantity"].ToExpressString();
            txtQtyInNumber.Text = r["QtyInNumber"].ToExpressString();
            acUnit.Value = r["Uom_ID"].ToExpressString();
            acItemTax.Value = r["Tax_ID"].ToExpressString();
            acBatchID.Value = r["Batch_ID"].ToExpressString();
            txtItemPercentageDiscount.Text = r["PercentageDiscount"].ToExpressString();
            txtItemCashDiscount.Text = r["CashDiscount"].ToExpressString();
            txtItemNotes.Text = r["Notes"].ToExpressString();
            txtBarcode.Text = r["Barcode"].ToExpressString();
            if (r["ProductionDate"].ToExpressString() != string.Empty) txtProductionDate.Text = r["ProductionDate"].ToDate().Value.ToString("d/M/yyyy"); ;
            if (r["ExpirationDate"].ToExpressString() != string.Empty) txtExpirationDate.Text = r["ExpirationDate"].ToDate().Value.ToString("d/M/yyyy");

            acStore.Enabled = (r["InvoiceDetail_ID"] == DBNull.Value);
            acItem.Enabled = (r["InvoiceDetail_ID"] == DBNull.Value);
            acCategory.Enabled = (r["InvoiceDetail_ID"] == DBNull.Value);
            txtBarcode.Enabled = (r["InvoiceDetail_ID"] == DBNull.Value);
            acBatchID.Enabled = (r["InvoiceDetail_ID"] == DBNull.Value);
            acUnit.Enabled = (r["InvoiceDetail_ID"] == DBNull.Value);
            txtCost.Enabled = (r["InvoiceDetail_ID"] == DBNull.Value);
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
            if (acBatchID.HasValue)
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
                if (Tax.OnInvoiceType.HasValue) r["OnInvoiceType"] = Tax.OnInvoiceType == 'C' ? 'D' : 'C';
                if (Tax.OnReceiptType.HasValue) r["OnReceiptType"] = Tax.OnReceiptType == 'C' ? 'D' : 'C';
                if (Tax.OnDocCreditType.HasValue) r["OnDocCreditType"] = Tax.OnDocCreditType == 'C' ? 'D' : 'C';
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
            dc.usp_ReturnInvoice_Cancel(this.ReturnInvoice_ID, MyContext.UserProfile.Contact_ID);
            LogAction(Actions.Delete, txtSerial.Text, dc);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.ReturnInvoice + "?ID=" + this.ReturnInvoice_ID.ToExpressString(), PageLinks.ReturnInvoicesList, PageLinks.ReturnInvoice);
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

    protected void ucNewBatchID_NewBatchCreated(string BatchName, string ProductionDate, string ExiprationDate, int Batch_ID)
    {
        try
        {
            if (!acBatchID.Enabled) return;
            acBatchID.Refresh();
            acBatchID.Value = Batch_ID.ToExpressString();
            this.acBatchID_SelectedIndexChanged(null, null);
            this.FocusNextControl(lnkNewBatch);
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

            Response.Redirect("~/Report_Dev/PrintReturninvoice.aspx?Invoice_ID=" + this.ReturnInvoice_ID + "&IsMaterla=1", false);

            //var returnInvoice = dc.usp_ReturnInvoice_SelectByID(this.ReturnInvoice_ID);
            //var totalAmount = returnInvoice.Select(x => x.GrossTotalAmount).FirstOrDefault();


            //ReportDocument doc = new ReportDocument();
            //doc.Load(Server.MapPath("~\\Reports\\ReturnInvoice_Print ssb.rpt"));
            //doc.SetParameterValue("@ReturnInvoice_ID", this.ReturnInvoice_ID);
            ////  doc.SetParameterValue("@DocTableType_ID", DocumentsTableTypes.ReturnInvoice.ToInt(), "DocumentTaxes.rpt");
            //// doc.SetParameterValue("@Doc_ID", this.ReturnInvoice_ID, "DocumentTaxes.rpt");
            ////doc.SetParameterValue("@IsReturn", true, "DocumentTaxes.rpt");
            //doc.SetParameterValue("BaseDir", HttpContext.Current.Server.MapPath("~\\Uploads\\"));
            //doc.SetParameterValue("ByEmp", ((UICulturePage)HttpContext.Current.Handler).MyContext.UserProfile.EmployeeName);
            //doc.SetParameterValue("@Branch_ID", acBranch.Value.ToNullableInt());
            //doc.SetParameterValue("@TheNo1", totalAmount.ToDecimalOrDefault().ToString(), "Tafkit.rpt");
            //doc.SetParameterValue("@TheNo2", "0", "Tafkit.rpt");

            //Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "ReturnInvoice"), false);
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
            doc.Load(Server.MapPath("~\\Reports\\InvOrderIn_Print.rpt"));
            doc.SetParameterValue("@ReturnInvoice_ID", this.ReturnInvoice_ID);
            Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "OrderIn"), false);
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

    protected void txtQty_TextChanged(object sender, EventArgs e)
    {

        CalculateTotalRow();
    }
    protected void btnCancelApprove_Click(object sender, EventArgs e)
    {
        //System.Data.Common.DbTransaction trans;
        //dc.Connection.Open();
        //trans = dc.Connection.BeginTransaction();
        //dc.Transaction = trans;
        try
        {
            dc.usp_CancelReturnInvoice_Approvel(this.ReturnInvoice_ID);
            //trans.Commit();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.ReturnInvoice + "?ID=" + this.ReturnInvoice_ID.ToExpressString(), PageLinks.ReturnInvoicesList, PageLinks.ReturnInvoice);
            LogAction(Actions.NotApprove, txtSerial.Text, dc);
        }
        catch (Exception ex)
        {
            //trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void ddlTvae_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.Calculate();
    }
    protected void acStore_SelectedIndexChanged(object sender, AutoCompleteEventArgs e)
    {
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
    }



    #endregion

    #region Private Methods
    private void CalculateTotalRow()
    {
        var Tax = dc.usp_Taxes_Select(acItemTax.Value.ToInt(), string.Empty).FirstOrDefault();


        decimal taxInclude = Decimal.Divide((Tax != null ? Tax.PercentageValue.Value : 0), (100M + (Tax != null ? Tax.PercentageValue.Value : 0)));
        decimal unitCost = txtCost.Text.ToDecimalOrDefault() - txtCost.Text.ToDecimalOrDefault() * taxInclude;

        var totalPrice = txtQty.Text.ToDecimalOrDefault() * unitCost;
        var calc = (totalPrice - txtItemCashDiscount.Text.ToDecimalOrDefault() - txtItemPercentageDiscount.Text.ToDecimalOrDefault() * (totalPrice) / 100);
        lblTotalRow.Text = (string.IsNullOrEmpty(acItemTax.Value) ? calc : (calc + (calc * (Tax != null ? Tax.PercentageValue.Value : 0) / 100))).ToExpressString();



    }
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
        acItem.ContextKey = "," + acCategory.Value + ",";
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
        //txtCItem.Clear();
        txtQtyInNumber.Clear();
        acBatchID.ContextKey = acItem.Value + ",";
        ucNewBatchID.ItemID = acItem.Value;
        acUnit.ContextKey = string.Empty + acItem.Value;
        acPriceName.ContextKey = string.Empty + acItem.Value;
        if (acItem.HasValue)
        {
            var item = dc.usp_Items_SelectByID(acItem.Value.ToNullableInt()).FirstOrDefault();
            txtCost.Text = (item.DefaultPrice.Value / (txtRatio.Text.ToNullableDecimal() ?? 1)).ToString(MyContext.FormatNumber);
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

        ddlTvae.Visible = MyContext.IsViewTaxInDocument;
        ddlCurrency.DataSource = dc.usp_Currency_Select(false);
        ddlCurrency.DataTextField = "Name";
        ddlCurrency.DataValueField = "ID";
        ddlCurrency.DataBind();
        this.FilterByBranchAndCurrency();
        acCategory.ContextKey = string.Empty;
        acTax.ContextKey = acItemTax.ContextKey = DocumentsTableTypes.Invoice.ToInt().ToExpressString() + ",true";
        this.FilterItems();
        this.BindItemsGrid();
        lblCreatedBy.Text = MyContext.UserProfile.EmployeeName;
        gvTaxes.DataSource = this.dtTaxes;
        gvTaxes.DataBind();


        #region Payment Methode

        var listSos = dc.SettingPointOs.Where(c => c.IsActive.Value).ToList().Select(c => new PaymentMethodeCls { Name = c.Name, ID = c.ID }).ToList();
        listSos.Add(new PaymentMethodeCls { Name = "اجـل", ID = -1 });
        listSos.Add(new PaymentMethodeCls { Name = "نقدي", ID = 0 });
        ddlPaymentMethode.DataSource = listSos.OrderBy(c => c.ID).ToList();
        ddlPaymentMethode.DataTextField = "Name";
        ddlPaymentMethode.DataValueField = "ID";
        ddlPaymentMethode.DataBind();

        #endregion


    }

    private void BindItemsGrid()
    {
        this.Calculate();
        gvItems.DataSource = this.dtItems;
        gvItems.DataBind();
        acBranch.Enabled = ddlTvae.Enabled = (gvItems.Rows.Count == 0) && (this.MyContext.UserProfile.Branch_ID == null);
    }

    private void ClearItemForm()
    {
        //acStore.Clear();
        if (MyContext.UserProfile.Store_ID != null) acStore.Value = MyContext.UserProfile.Store_ID.ToStringOrEmpty();
        acCategory.Clear();
        this.FilterItems();
        this.acItem_SelectedIndexChanged(null, null);
        acStore.Enabled = true;
        acItem.Enabled = true;
        acCategory.Enabled = true;
        txtBarcode.Enabled = true;
        acBatchID.Enabled = true;
        acUnit.Enabled = true;
        txtCost.Enabled = true;
        txtCItem.Clear();
    }

    private void SetEditMode()
    {
        if (Request["ID"] != null)
        {
            this.EditMode = true;
            this.ReturnInvoice_ID = Request["ID"].ToInt();
        }

        if (Request["FromInvoiceID"] != null)
        {
            this.FromInvoice_ID = Request["FromInvoiceID"].ToInt();
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
        this.CalculatedReturnSalesCost = 0;
        this.ConfirmationMessage = string.Empty;
        string Serial = string.Empty;
        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        DateTime? ApproveDate = IsApproving ? DateTime.Now : (DateTime?)null;
        int? approvedBY_ID = IsApproving ? MyContext.UserProfile.Contact_ID : (int?)null;
        int Serial_ID = DocSerials.ReturnInvoice.ToInt();
        int Detail_ID = 0;

        decimal firstpaid = 0;

        if (ddlPaymentMethode.SelectedValue != "-1")
        {
            var paymentMethods = dc.SettingPointOs.Where(c => c.IsActive.Value).ToList();
            var IDPM = ddlPaymentMethode.SelectedValue.ToIntOrDefault();
            var ChoosedPayMethodes_ID = paymentMethods.Where(c => c.ID == IDPM).FirstOrDefault();
            if (ChoosedPayMethodes_ID != null)
            {
                firstpaid = lblGrossTotal.Text.ToDecimalOrDefault();
            }
        }

        if (!this.EditMode)
        {
            this.ReturnInvoice_ID = dc.usp_ReturnInvoice_Insert(acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimalOrDefault(), txtOperationDate.Text.ToDate(),
                                                     acCustomer.Value.ToInt(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate,
                                                     approvedBY_ID, null, null, acAddress.Value.ToNullableInt(), acShipAddress.Value.ToNullableInt(), acPaymentAddress.Value.ToNullableInt(),
                                                     acTelephone.Value.ToNullableInt(), acCostCenter.Value.ToNullableInt(), txtNotes.Text, lblTotal.Text.ToDecimal(), txtPercentageDiscount.Text.ToDecimalOrDefault(),
                                                     txtCashDiscount.Text.ToDecimalOrDefault(), txtAdditionals.Text.ToDecimalOrDefault(), lblTotalDiscount.Text.ToDecimalOrDefault(),
                                                     lblTotalTax.Text.ToDecimalOrDefault(),
                                                     lblGrossTotal.Text.ToDecimalOrDefault(), firstpaid,//txtFirstPaid.Text.ToDecimalOrDefault(),
                                                     null, txtUserRefNo.Text, this.DocRandomString,
                                                     this.FromInvoice_ID, acSalesRep.Value.ToNullableInt(),
                                                     acCashAccount.Value.ToNullableInt(),
                                                     ddlTvae.SelectedValue.ToNullableInt(), 0,ddlPaymentMethode.SelectedValue.ToNullableInt());
            if (this.ReturnInvoice_ID > 0)
            {
                foreach (DataRow r in this.dtItems.Rows)
                {
                    if (r.RowState == DataRowState.Deleted) continue;
                    Detail_ID = dc.usp_ReturnInvoiceDetailsIncludeTax_Insert(this.ReturnInvoice_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["InvoiceDetail_ID"].ToNullableInt(), r["QtyInNumber"].ToDecimalOrDefault(), r["UnitCostEvaluate"].ToDecimalOrDefault(),false,false,false);
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
                    dc.usp_DocuemntTaxes_Insert(DocumentsTableTypes.ReturnInvoice.ToInt(), this.ReturnInvoice_ID, r["Tax_ID"].ToInt());
                }
                //if (IsApproving) dc.usp_ICJ_NegativeRefill(txtOperationDate.Text.ToDate(), acBranch.Value.ToNullableInt(), DocumentsTableTypes.ReturnInvoice.ToInt(), this.ReturnInvoice_ID);
                if (IsApproving) if (!this.InsertOperation()) { trans.Rollback(); return false; }
                LogAction(IsApproving ? Actions.Approve : Actions.Add, Serial, dc);
            }
        }
        else
        {
            int Result = dc.usp_ReturnInvoice_Update(this.ReturnInvoice_ID, acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimalOrDefault(), txtOperationDate.Text.ToDate(),
                                                    acCustomer.Value.ToInt(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate, approvedBY_ID,
                                                    null, null, acAddress.Value.ToNullableInt(), acShipAddress.Value.ToNullableInt(), acPaymentAddress.Value.ToNullableInt(), acTelephone.Value.ToNullableInt(),
                                                    acCostCenter.Value.ToNullableInt(), txtNotes.Text, lblTotal.Text.ToDecimal(), txtPercentageDiscount.Text.ToDecimalOrDefault(), txtCashDiscount.Text.ToDecimalOrDefault(), txtAdditionals.Text.ToDecimalOrDefault(),
                                                    lblTotalDiscount.Text.ToDecimalOrDefault(), lblTotalTax.Text.ToDecimalOrDefault(),
                                                    lblGrossTotal.Text.ToDecimalOrDefault(), firstpaid,// txtFirstPaid.Text.ToDecimalOrDefault(), 
                                                    txtUserRefNo.Text, this.DocRandomString, acSalesRep.Value.ToNullableInt(), acCashAccount.Value.ToNullableInt(), ddlTvae.SelectedValue.ToNullableInt(), 0,ddlPaymentMethode.SelectedValue.ToNullableInt());
            if (Result > 0)
            {
                foreach (DataRow r in this.dtItems.Rows)
                {
                    if (r.RowState == DataRowState.Added)
                    {
                        Detail_ID = dc.usp_ReturnInvoiceDetailsIncludeTax_Insert(this.ReturnInvoice_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["InvoiceDetail_ID"].ToNullableInt(), r["QtyInNumber"].ToDecimalOrDefault(), r["UnitCostEvaluate"].ToDecimalOrDefault(), false, false, false);
                    }
                    if (r.RowState == DataRowState.Modified)
                    {
                        Detail_ID = r["ID"].ToInt();
                        dc.usp_ReturnInvoiceDetailsIncludeTax_Update(r["ID"].ToInt(), r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["UnitCostEvaluate"].ToDecimalOrDefault(), false, false, false);
                    }
                    if (r.RowState == DataRowState.Deleted)
                    {
                        dc.usp_ReturnInvoiceDetails_Delete(r["ID", DataRowVersion.Original].ToInt());
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
                        dc.usp_DocuemntTaxes_Insert(DocumentsTableTypes.ReturnInvoice.ToInt(), this.ReturnInvoice_ID, r["Tax_ID"].ToInt());
                    }
                    if (r.RowState == DataRowState.Deleted)
                    {
                        dc.usp_DocuemntTaxes_Delete(r["ID", DataRowVersion.Original].ToInt());
                    }
                }
                // if (IsApproving) dc.usp_ICJ_NegativeRefill(txtOperationDate.Text.ToDate(), acBranch.Value.ToNullableInt(), DocumentsTableTypes.ReturnInvoice.ToInt(), this.ReturnInvoice_ID);
                if (IsApproving) if (!this.InsertOperation()) { trans.Rollback(); return false; }
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
        if (this.ReturnInvoice_ID > 0)
        {
            var rInv = dc.ReturnInvoices.Where(c => c.ID == this.ReturnInvoice_ID).FirstOrDefault();
            if (rInv != null)
            {
                rInv.typePayment_ID = ddlPaymentMethode.SelectedValue.ToIntOrDefault();
                dc.SubmitChanges();
            }

        }

        Serial = Serial == string.Empty ? string.Empty : " (" + Serial + ") ";
        UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess + Serial, PageLinks.ReturnInvoice + "?ID=" + this.ReturnInvoice_ID.ToExpressString(), PageLinks.ReturnInvoicesList, PageLinks.ReturnInvoice);
        return true;
    }

    private bool InsertOperation()
    {
        decimal ratio = txtRatio.Text.ToDecimal();
        decimal Additionals = txtAdditionals.Text.ToDecimalOrDefault();
        string serial = string.Empty;
        var company = dc.usp_Company_Select().FirstOrDefault();
        int ContactAccountID = dc.fun_getContactAccountID(acCustomer.Value.ToInt()).Value;

        int Result = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.ReturnSales.ToInt(), ddlCurrency.SelectedValue.ToInt(), (this.Total + this.TotalDebitTax + Additionals) * ratio, (this.Total + this.TotalDebitTax + Additionals), ratio, txtNotes.Text);

        //ايراد مبيعات
        dc.usp_OperationDetails_Insert(Result, company.SellAccount_ID, (this.Total) * ratio, 0, (this.Total), 0, null, this.ReturnInvoice_ID, DocumentsTableTypes.ReturnInvoice.ToInt());

        //ايراد الاضافات
        if (Additionals > 0) dc.usp_OperationDetails_Insert(Result, company.InvoiceAdditionals_AccountID, Additionals * ratio, 0, Additionals, 0, null, this.ReturnInvoice_ID, DocumentsTableTypes.ReturnInvoice.ToInt());

        //Customer
        // dc.usp_OperationDetails_Insert(Result, ContactAccountID, 0, (this.GrossTotal) * ratio, 0, (this.GrossTotal), null, this.ReturnInvoice_ID, DocumentsTableTypes.ReturnInvoice.ToInt());

        //Customer
        if (ddlPaymentMethode.SelectedValue != "-1")
        {
            var Accou_ID = 0;
            var paymentMethods = dc.SettingPointOs.Where(c => c.IsActive.Value).ToList();
            var IDPM = ddlPaymentMethode.SelectedValue.ToIntOrDefault();
            var ChoosedPayMethodes_ID = paymentMethods.Where(c => c.ID == IDPM).FirstOrDefault();

            if (ChoosedPayMethodes_ID != null)
            {
                if (MyContext.UserProfile.CashierAccount_ID.ToInt() > 0 && ddlPaymentMethode.SelectedValue == "0")
                {
                    Accou_ID = MyContext.UserProfile.CashierAccount_ID.ToInt();
                }
                else
                {
                    Accou_ID = ChoosedPayMethodes_ID.SalesAccountID.ToIntOrDefault();
                }
            }
            else
            {
                if (MyContext.UserProfile.CashierAccount_ID.ToInt() > 0 && ddlPaymentMethode.SelectedValue == "0")
                {
                    Accou_ID = MyContext.UserProfile.CashierAccount_ID.ToInt();
                }
            }

            if (Accou_ID == 0)
            {
                UserMessages.Message(null, "حساب الصندوق غير معرف", string.Empty);
                return false;
            }
            if (Accou_ID != 0)
            {
                dc.usp_OperationDetails_Insert(Result, Accou_ID, 0, (this.GrossTotal) * ratio, 0, (this.GrossTotal), null, this.ReturnInvoice_ID, DocumentsTableTypes.ReturnInvoice.ToInt());
            }
            else
            {
                dc.usp_OperationDetails_Insert(Result, ContactAccountID, 0, (this.GrossTotal) * ratio, 0, (this.GrossTotal), null, this.ReturnInvoice_ID, DocumentsTableTypes.ReturnInvoice.ToInt());
            }
        }
        else
        {
            dc.usp_OperationDetails_Insert(Result, ContactAccountID, 0, (this.GrossTotal) * ratio, 0, (this.GrossTotal), null, this.ReturnInvoice_ID, DocumentsTableTypes.ReturnInvoice.ToInt());
        }

        //Discount
        if (this.TotalDiscount > 0)
        {
            dc.usp_OperationDetails_Insert(Result, company.SalesDiscountAccountID, 0, this.TotalDiscount * ratio, 0, this.TotalDiscount, null, this.ReturnInvoice_ID, DocumentsTableTypes.ReturnInvoice.ToInt());
        }

        //Taxes
        var GroupedTaxes = from tax in this.dtAllTaxes.AsEnumerable()
                           where tax.RowState != DataRowState.Deleted
                           group tax by new { Account_ID = tax.Field<int>("Account_ID"), OnInvoiceType = tax.Field<string>("OnInvoiceType") } into groupedTaxes
                           select new { Key = groupedTaxes.Key, DebitAmount = groupedTaxes.Sum(x => x.Field<decimal>("DebitAmount")), CreditAmount = groupedTaxes.Sum(x => x.Field<decimal>("CreditAmount")) };
        foreach (var tax in GroupedTaxes)
        {
            dc.usp_OperationDetails_Insert(Result, tax.Key.Account_ID, tax.DebitAmount * ratio, tax.CreditAmount * ratio, tax.DebitAmount, tax.CreditAmount, null, this.ReturnInvoice_ID, DocumentsTableTypes.ReturnInvoice.ToInt());
        }

        if (this.CalculatedReturnSalesCost > 0)
        {
            dc.usp_ReturnSalesCostOperation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), this.CalculatedReturnSalesCost, this.ReturnInvoice_ID, DocumentsTableTypes.ReturnInvoice.ToInt());
        }

        //CostCenter debit
        dc.usp_CostCenterOperation_Insert(acBranch.Value.ToNullableInt(), acCostCenter.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), (this.Total + Additionals) * ratio, this.ReturnInvoice_ID, DocumentsTableTypes.ReturnInvoice.ToInt(), txtNotes.Text);
        //CostCenter credit
        dc.usp_CostCenterOperation_Insert(acBranch.Value.ToNullableInt(), acCostCenter.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), (this.CalculatedReturnSalesCost + (this.TotalDiscount * ratio)) * -1, this.ReturnInvoice_ID, DocumentsTableTypes.ReturnInvoice.ToInt(), txtNotes.Text);
        // this.InsertCashOut();

        return true;
    }

    private bool InsertICJIn(int Detail_ID, usp_ProductionOrderDetailsForFinalItem_SelectResult row)
    {


        decimal? Cost = 0;
        int result = 0;

        result = dc.usp_ICJ_ProductionOrder_IN(txtOperationDate.Text.ToDate(), row.Quantity.ToDecimal(), row.Item_ID.ToIntOrDefault(), null, ref Cost, null, acStore.Value.ToInt(), acBranch.Value.ToNullableInt(), DocumentsTableTypes.ReturnInvoice.ToInt(), this.ReturnInvoice_ID, null, null);

        if (result == -6)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.CantReceiveFinalItem, string.Empty);
            return false;
        }
        if (result == -666)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.VeryLowCost, string.Empty);
            return false;
        }
        return true;
    }

    private bool InsertICJ(int Detail_ID, DataRow row)
    {
        int result = 0;
        decimal? ReturnSalesCost = 0;

        var lstMaterials = dc.usp_ProductionOrderDetailsForFinalItem_Select(row["Item_ID"].ToInt(), row["Store_ID"].ToInt(), row["Quantity"].ToDecimal()).ToList();//.CopyToDataTable();
        foreach (var item in lstMaterials)
        {
            var res = InsertICJIn(Detail_ID, item);
        }

        if (!lstMaterials.Any())
        {
            result = dc.usp_ICJ_ReturnInvoice(txtOperationDate.Text.ToDate(), row["Quantity"].ToDecimal(), row["Item_ID"].ToInt(), row["Uom_ID"].ToInt(), row["Total"].ToDecimal() * txtRatio.Text.ToDecimal(), row["Batch_ID"].ToNullableInt(), row["Store_ID"].ToInt(), acBranch.Value.ToNullableInt(), DocumentsTableTypes.ReturnInvoice.ToInt(), this.ReturnInvoice_ID, Detail_ID, DocumentsTableTypes.ReturnInvoice.ToInt(), row["InvoiceDetail_ID"].ToNullableInt(), ref ReturnSalesCost);
            this.CalculatedReturnSalesCost += ReturnSalesCost.Value;
            if (result == -5)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.CantReturnMoreOriginal + " (" + row["StoreName"] + " : " + row["ItemName"].ToExpressString() + ")", string.Empty);
                return false;
            }
            if (result == -10)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.CantReturnNegativeQty + " (" + row["StoreName"] + " : " + row["ItemName"].ToExpressString() + ")", string.Empty);
                return false;
            }
            if (result == -7 && !this.ConfirmationAnswered)
            {
                this.ConfirmationMessage += "<br>\u2022 " + Resources.UserInfoMessages.ReturnQtyMoreThanSold + " (" + row["StoreName"] + " : " + row["ItemName"] + ")";
            }
        }
        return true;
    }

    private void FillReturnInvoice()
    {
        var invoice = dc.usp_ReturnInvoice_SelectByID(this.ReturnInvoice_ID).FirstOrDefault();

        ddlCurrency.SelectedValue = invoice.Currency_ID.ToExpressString();
        ddlTvae.SelectedValue = invoice.IsTax.ToExpressString();
        this.IsTaxFound = invoice.IsTax.ToIntOrDefault();

        OperationsView.SourceDocTypeType_ID = DocumentsTableTypes.ReturnInvoice.ToInt();
        OperationsView.Source_ID = this.ReturnInvoice_ID;

        ddlPaymentMethode.SelectedValue = invoice.typePayment_ID.ToExpressString();
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
        acSalesRep.Value = invoice.SalesRep_ID.ToStringOrEmpty();
        txtCost.Enabled = MyContext.PageData.IsPrice;
        this.FromInvoice_ID = invoice.FromInvoiceID;

        if (!string.IsNullOrEmpty(invoice.FromInvoiceSerial))
        {
            lblFromInvoiceNo.Text = invoice.FromInvoiceSerial;
            divFromInvNo.Visible = true;
            ddlCurrency.Enabled = false;
            acBranch.Enabled = false;
            acCustomer.Enabled = false;
        }

        if (EditMode) //check Not Important
        {
            txtSerial.Text = invoice.Serial;
            ucNav.SetText = invoice.Serial;
            this.DocRandomString = invoice.DocRandomString;
            lblCreatedBy.Text = invoice.CreatedByName;
            lblApprovedBy.Text = invoice.ApprovedBYName;
            this.ImgStatus = ((DocStatus)invoice.DocStatus_ID).ToExpressString();
            btnPrint.Visible = MyContext.PageData.IsPrint;
            btnPrintInventoryOrder.Visible = MyContext.PageData.IsPrint;
            pnlAddItem.Visible = (invoice.DocStatus_ID == 1);
            btnCancel.Visible = (invoice.DocStatus_ID == 1) && MyContext.PageData.IsDelete;
            btnApprove.Visible = (invoice.DocStatus_ID == 1) && MyContext.PageData.IsApprove;
            btnCancelApprove.Visible = !btnApprove.Visible && (invoice.DocStatus_ID == 2) && MyContext.PageData.IsNotApprove;
            btnSave.Visible = (invoice.DocStatus_ID == 1) && ((MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode));
            acSalesRep.Value = invoice.SalesRep_ID.ToStringOrEmpty();
            OperationsView.Visible = !btnApprove.Visible;
            txtCost.Enabled = MyContext.PageData.IsPrice;
            //  gvItems.Columns[gvItems.Columns.Count - 1].Visible = gvItems.Columns[gvItems.Columns.Count - 2].Visible = (invoice.DocStatus_ID == 1);
            // gvTaxes.Columns[gvTaxes.Columns.Count - 1].Visible = (invoice.DocStatus_ID == 1);
            this.DocStatus_ID = invoice.DocStatus_ID.Value;
        }

        //this.dtItems = dc.usp_ReturnInvoiceDetailsIncludeTax_Select(this.ReturnInvoice_ID).CopyToDataTable();

        var lstItems = dc.usp_ReturnInvoiceDetailsIncludeTax_Select(this.ReturnInvoice_ID).ToList();
        this.dtItems = lstItems.CopyToDataTable();
        if (lstItems.Count() > 0)
        {
            acStore.Value = lstItems.FirstOrDefault().Store_ID.ToExpressString();
        }

        this.dtTaxes = dc.usp_DocuemntTaxes_Select(DocumentsTableTypes.ReturnInvoice.ToInt(), this.ReturnInvoice_ID, true).CopyToDataTable();
        this.BindItemsGrid();
        gvTaxes.DataSource = this.dtTaxes;
        gvTaxes.DataBind();

    }

    private void FillFromInvoice()
    {
        if (this.FromInvoice_ID == null) return;
        var invoice = dc.usp_Invoice_SelectByID(this.FromInvoice_ID).FirstOrDefault();

        ddlCurrency.SelectedValue = invoice.Currency_ID.ToExpressString();
        txtRatio.Text = invoice.Ratio.ToExpressString();
        acBranch.Value = invoice.Branch_ID.ToStringOrEmpty();
        this.FilterByBranchAndCurrency();
        txtOperationDate.Text = invoice.OperationDate.Value.ToString("d/M/yyyy");
        acCostCenter.Value = invoice.CostCenter_ID.ToStringOrEmpty();
        txtUserRefNo.Text = invoice.UserRefNo;
        acCustomer.Value = invoice.Contact_ID.ToExpressString();
        this.acCustomer_SelectedIndexChanged(null, null);
        acSalesRep.Value = invoice.SalesRep_ID.ToStringOrEmpty();
        acTelephone.Value = invoice.Telephone_ID.ToStringOrEmpty();
        acAddress.Value = invoice.DefaultAddress_ID.ToStringOrEmpty();
        acShipAddress.Value = invoice.ShipToAddress_ID.ToStringOrEmpty();
        acPaymentAddress.Value = invoice.PaymentAddress_ID.ToStringOrEmpty();
        acCashAccount.Value = invoice.CashAccount_ID.ToStringOrEmpty();
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

        ddlCurrency.Enabled = false;
        acBranch.Enabled = false;
        acCustomer.Enabled = false;
        acSalesRep.Enabled = false;

        txtCost.Enabled = MyContext.PageData.IsPrice;
        //  this.dtItems = dc.usp_InvoiceDetailsForReturn_Select(this.FromInvoice_ID).CopyToDataTable();


        var lstItems = dc.usp_InvoiceDetailsForReturn_Select(this.FromInvoice_ID).ToList();
        this.dtItems = lstItems.CopyToDataTable();
        if (lstItems.Count() > 0)
        {
            acStore.Value = lstItems.FirstOrDefault().Store_ID.ToExpressString();
        }


        this.dtTaxes = dc.usp_DocuemntTaxes_Select(DocumentsTableTypes.Invoice.ToInt(), this.FromInvoice_ID, true).CopyToDataTable();
        this.BindItemsGrid();
        gvTaxes.DataSource = this.dtTaxes;
        gvTaxes.DataBind();

    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);




        btnSave.Visible = (MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode);
        btnApprove.Visible = MyContext.PageData.IsApprove;

        btnCancelApprove.Visible = !btnApprove.Visible && MyContext.PageData.IsNotApprove;
        btnCancel.Visible = MyContext.PageData.IsDelete && EditMode;
        OperationsView.Visible = !btnApprove.Visible;

        txtCost.Enabled = MyContext.PageData.IsPrice;
    }

    private void Calculate()
    {
        decimal ItemDiscount = 0;
        decimal DocDiscount = 0;
        decimal DocTax = 0;
        decimal DocTaxValue = 0;
        decimal Additionals = 0;
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
            if (r.RowState == DataRowState.Deleted) continue;
            decimal TaxValue = r["TaxPercentageValue"].ToDecimalOrDefault();
            if (this.EditMode)
            {
                if (this.IsTaxFound == 2)
                {
                    TaxValue = 0;
                }
                else
                {

                }
            }
            else
            {
                if (ddlTvae.SelectedValue == "2")
                {
                    TaxValue = 0;
                }
            }

            ItemDiscount = 0;
            decimal taxInclude = (TypeTax == 2 ? Decimal.Divide(TaxValue, (100 + TaxValue)) : 0);
            var uc = r["UnitCostEvaluate"].ToDecimalOrDefault();
            var taxUc = r["UnitCostEvaluate"].ToDecimalOrDefault() * taxInclude;
            r["UnitCost"] = uc - taxUc;



            r["Total"] = r["UnitCost"].ToDecimal() * r["Quantity"].ToDecimal();
            Total += r["UnitCost"].ToDecimal() * r["Quantity"].ToDecimal();
            this.TotalDiscount += ItemDiscount = (r["PercentageDiscount"].ToDecimal() * r["Total"].ToDecimal() * 0.01m) + r["CashDiscount"].ToDecimal();
            if (r["TaxOnInvoiceType"] != DBNull.Value && r["Tax_ID"] != DBNull.Value)
            {
                r["TotalTax"] = ((r["Total"].ToDecimal() - ItemDiscount) * TaxValue * 0.01m);

                if (r["TaxOnInvoiceType"].ToExpressString() == "C")
                {
                    this.dtAllTaxes.Rows.Add(r["TaxSalesAccountID"].ToInt(), r["TaxOnInvoiceType"].ToExpressString(), 0, r["TotalTax"].ToDecimal());
                    this.TotalCreditTax += r["TotalTax"].ToDecimal();
                    r["TotalTax"] = r["TotalTax"].ToDecimal() * -1;
                }
                else
                {
                    this.dtAllTaxes.Rows.Add(r["TaxSalesAccountID"].ToInt(), r["TaxOnInvoiceType"].ToExpressString(), r["TotalTax"].ToDecimal(), 0);
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
            if (r["OnInvoiceType"] != DBNull.Value)
            {
                DocTaxValue = ((this.GrossTotal - DocDiscount) * r["PercentageValue"].ToDecimal() * 0.01m);
                if (r["OnInvoiceType"].ToExpressString() == "C")
                {
                    this.dtAllTaxes.Rows.Add(r["SalesAccountID"].ToInt(), r["OnInvoiceType"].ToExpressString(), 0, DocTaxValue);
                    this.TotalCreditTax += DocTaxValue;
                    DocTaxValue *= -1m;
                }
                else
                {
                    this.dtAllTaxes.Rows.Add(r["SalesAccountID"].ToInt(), r["OnInvoiceType"].ToExpressString(), DocTaxValue, 0);
                    this.TotalDebitTax += DocTaxValue;
                }
                DocTax += DocTaxValue;
            }
        }
        Additionals = txtAdditionals.Text.ToDecimalOrDefault();
        this.TotalDiscount += DocDiscount;
        this.TotalTax += DocTax;
        this.GrossTotal = this.GrossTotal - DocDiscount + DocTax + Additionals;
        lblTotal.Text = this.Total.ToString(MyContext.FormatNumber);
        lblTotalDiscount.Text = this.TotalDiscount.ToString(MyContext.FormatNumber);
        lblAdditionals.Text = Additionals.ToString(MyContext.FormatNumber);
        lblTotalTax.Text = this.TotalTax.ToString(MyContext.FormatNumber);
        lblGrossTotal.Text = this.GrossTotal.ToString(MyContext.FormatNumber);
        if (acCashAccount.HasValue && Page.IsPostBack && this.SumFirstPaid)
        {
            txtFirstPaid.Text = lblGrossTotal.Text;
            this.txtFirstPaid_TextChanged(null, null);
        }
        this.ConfirmationAnswered = false;
        this.ConfirmationMessage = string.Empty;
    }

    private void ShowAvailableQty()
    {
        lblAvailableQty.Text = string.Empty;
        lblCustomerSoldQty.Text = string.Empty;
        if (acItem.HasValue || acItemDescribed.HasValue)
        {
            decimal Qty = dc.fun_ItemQty(acItem.Value.ToInt(), acStore.Value.ToNullableInt(), acBatchID.Value.ToNullableInt(), acUnit.Value.ToNullableInt(), null).Value;
            lblAvailableQty.Text = Qty.ToExpressString();
            lblAvailableQty.ForeColor = Qty <= 0 ? System.Drawing.Color.Red : System.Drawing.Color.Green;
            if (acCustomer.HasValue) lblCustomerSoldQty.Text = dc.fun_CustomerSoldItemQuantity(acCustomer.Value.ToNullableInt(), acItem.Value.ToNullableInt(), acBatchID.Value.ToNullableInt(), acUnit.Value.ToNullableInt()).Value.ToExpressString();
        }
    }

    private void InsertCashOut()
    {
        string Serial = string.Empty;
        decimal ratio = txtRatio.Text.ToDecimal();
        int? CashOut_ID = null;
        if (txtFirstPaid.Text.ToDecimalOrDefault() <= 0) return;
        int ContactChartOfAccount_ID = dc.fun_getContactAccountID(acCustomer.Value.ToInt()).Value;
        CashOut_ID = dc.usp_Payments_Insert(txtOperationDate.Text.ToDate(), DateTime.Now, MyContext.UserProfile.Contact_ID, MyContext.UserProfile.Contact_ID, DateTime.Now, txtUserRefNo.TrimmedText, ref Serial, DocSerials.CashOut.ToInt(), txtNotes.Text, txtFirstPaid.Text.ToDecimal(), null, DocStatus.Approved.ToByte(), PaymentsTypes.CashOut.ToByte(), acBranch.Value.ToNullableInt(), txtRatio.Text.ToDecimal(), ddlCurrency.SelectedValue.ToInt(), this.ReturnInvoice_ID, DocumentsTableTypes.ReturnInvoice.ToInt(), this.DocRandomString + "_FromReturnInvoice");
        if (!CashOut_ID.HasValue || CashOut_ID.Value <= 0) throw new Exception("Error Occured During inserting the cash document");
        dc.usp_PaymentsDetails_Insert(CashOut_ID, ContactChartOfAccount_ID, acCashAccount.Value.ToInt(), txtFirstPaid.Text.ToDecimal(), null, string.Empty, null);

        int Operation_ID = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref Serial, DocStatus.Approved.ToByte(), OperationTypes.CashOut.ToInt(), ddlCurrency.SelectedValue.ToInt(), txtFirstPaid.Text.ToDecimal() * ratio, txtFirstPaid.Text.ToDecimal(), ratio, null);
        dc.usp_OperationDetails_Insert(Operation_ID, acCashAccount.Value.ToInt(), 0, txtFirstPaid.Text.ToDecimal() * ratio, 0, txtFirstPaid.Text.ToDecimal(), null, CashOut_ID, DocumentsTableTypes.Payament_CashOut.ToInt());
        dc.usp_OperationDetails_Insert(Operation_ID, ContactChartOfAccount_ID, txtFirstPaid.Text.ToDecimal() * ratio, 0, txtFirstPaid.Text.ToDecimal(), 0, null, CashOut_ID, DocumentsTableTypes.Payament_CashOut.ToInt());
        dc.usp_SetCashDocForBills(this.ReturnInvoice_ID, CashOut_ID, DocumentsTableTypes.ReturnInvoice.ToInt());
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
        lnkNewBatch.Visible = MyContext.Features.BatchIDEnabled;
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
        //if (acStore.HasValue) this.acStore_SelectedIndexChanged(null, null);

        txtOperationDate.Enabled = !company.LockAutoDate.Value;
        acCashAccount.Enabled = !((MyContext.UserProfile.CashierAccount_ID != null) && ((MyContext.Features.BranchesEnabled && MyContext.UserProfile.Branch_ID != null) || !MyContext.Features.BranchesEnabled));
        acStore.Enabled = !((MyContext.UserProfile.Store_ID != null) && ((MyContext.Features.BranchesEnabled && MyContext.UserProfile.Branch_ID != null) || !MyContext.Features.BranchesEnabled));
    }

    #endregion
}