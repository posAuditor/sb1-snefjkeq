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

public partial class Purchases_ReturnReceipt : UICulturePage
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

    private int ReturnReceipt_ID
    {
        get
        {
            if (ViewState["ReturnReceipt_ID"] == null) return 0;
            return (int)ViewState["ReturnReceipt_ID"];
        }

        set
        {
            ViewState["ReturnReceipt_ID"] = value;
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
            if (Session["dtItems_ReturnReceipt" + this.WinID] == null)
            {
                Session["dtItems_ReturnReceipt" + this.WinID] = dc.usp_ReturnReceiptDetails_Select(null).CopyToDataTable();
            }
            return (DataTable)Session["dtItems_ReturnReceipt" + this.WinID];
        }

        set
        {
            Session["dtItems_ReturnReceipt" + this.WinID] = value;
        }
    }

    private DataTable dtTaxes
    {
        get
        {
            if (Session["dtTaxes_ReturnReceipt" + this.WinID] == null)
            {
                Session["dtTaxes_ReturnReceipt" + this.WinID] = dc.usp_DocuemntTaxes_Select(null, null, true).CopyToDataTable();
            }
            return (DataTable)Session["dtTaxes_ReturnReceipt" + this.WinID];
        }

        set
        {
            Session["dtTaxes_ReturnReceipt" + this.WinID] = value;
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

    private int? FromReceipt_ID
    {
        get
        {
            return (int?)ViewState["FromReceipt_ID"];
        }

        set
        {
            ViewState["FromReceipt_ID"] = value;
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
            if (Session["dtAllTaxes_ReturnReceipt" + this.WinID] == null)
            {
                DataTable dtallTaxes = new DataTable();
                dtallTaxes.Columns.Add("Account_ID", typeof(int));
                dtallTaxes.Columns.Add("OnReceiptType", typeof(string));
                dtallTaxes.Columns.Add("DebitAmount", typeof(decimal));
                dtallTaxes.Columns.Add("CreditAmount", typeof(decimal));
                Session["dtAllTaxes_ReturnReceipt" + this.WinID] = dtallTaxes;
            }
            return (DataTable)Session["dtAllTaxes_ReturnReceipt" + this.WinID];
        }

        set
        {
            Session["dtAllTaxes_ReturnReceipt" + this.WinID] = value;
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
            gvItems.FormatNumber = MyContext.FormatNumber;
            if (!Page.IsPostBack)
            {
                this.DocRandomString.ToString();
                this.CheckSecurity();
                this.LoadControls();
                this.SetDefaults();
                if (EditMode)
                {
                    this.FillReturnReceipt();
                }
                else
                {
                    this.FillFromReceipt();
                }
                var comp = dc.usp_Company_Select().FirstOrDefault();
                txtCost.Enabled = !comp.IsPriceLoocked.Value;
            }
            ucNav.SourceDocTypeType_ID = DocumentsTableTypes.ReturnReceipt.ToInt();
            ucNav.EntryType = 1;
            ucNav.Res_ID = this.ReturnReceipt_ID;
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
                        case 6: LinkButton1.Focus();
                            break;
                        case 5: LinkButton1.Focus();
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





    protected void Page_PreRender(object sender, EventArgs e)
    {
        this.CustomPage();
    }

    #endregion

    #region control Events

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
            this.FilterItemsData();
            this.ShowAvailableQty();
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
            txtCost.Text = (dc.fun_GetItemDefaultCostByUOM(acItem.Value.ToNullableInt(), acUnit.Value.ToNullableInt()) / (txtRatio.Text.ToNullableDecimal() ?? 1)).Value.ToString("0.####");
            this.ShowAvailableQty();
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
            //if (txtQty.Text.ToDecimal() > lblAvailableQty.Text.ToDecimalOrDefault())
            //{
            //    UserMessages.Message(null, Resources.UserInfoMessages.QtyNotEnough, string.Empty);
            //    return;
            //}
            DataRow r = null;
            if (this.EditID == 0)
            {
                if (this.FromReceipt_ID != null)
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

            //if (ddlTvae.SelectedValue.ToNullableInt() == 2)
            //{
            //    acItemTax.Clear();
            //}

            r["Store_ID"] = acStore.Value;
            r["Category_ID"] = acCategory.Value;
            r["Item_ID"] = acItem.Value;
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
            r["ItemName"] = acItem.Text;
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

            lblTotalRow.Text = string.Empty;
            txtBarcode.Focus();


            //this.FocusNextControl(acStore);
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

            if (this.FromReceipt_ID != null)
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
            txtCost.Text = r["UnitCost"].ToExpressString();
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

            acStore.Enabled = (r["ReceiptDetail_ID"] == DBNull.Value);
            acItem.Enabled = (r["ReceiptDetail_ID"] == DBNull.Value);
            acCategory.Enabled = (r["ReceiptDetail_ID"] == DBNull.Value);
            txtBarcode.Enabled = (r["ReceiptDetail_ID"] == DBNull.Value);
            acBatchID.Enabled = (r["ReceiptDetail_ID"] == DBNull.Value);
            acUnit.Enabled = (r["ReceiptDetail_ID"] == DBNull.Value);
            txtCost.Enabled = (r["ReceiptDetail_ID"] == DBNull.Value);
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
            dc.usp_ReturnReceipt_Cancel(this.ReturnReceipt_ID, MyContext.UserProfile.Contact_ID);
            LogAction(Actions.Delete, txtSerial.Text, dc);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.ReturnReceipt + "?ID=" + this.ReturnReceipt_ID.ToExpressString(), PageLinks.ReturnReceiptsList, PageLinks.ReturnReceipt);
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

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {

            Response.Redirect("~/Report_Dev/PrintReturnReceiptDev.aspx?Invoice_ID=" + this.ReturnReceipt_ID + "&IsMaterla=1", false);

            //ReportDocument doc = new ReportDocument();
            //doc.Load(Server.MapPath("~\\Reports\\ReturnReceipt_Print ssb.rpt"));
            //doc.SetParameterValue("@ReturnReceipt_ID", this.ReturnReceipt_ID);
            ////doc.SetParameterValue("@DocTableType_ID", DocumentsTableTypes.ReturnReceipt.ToInt(), "DocumentTaxes.rpt");
            ////doc.SetParameterValue("@Doc_ID", this.ReturnReceipt_ID, "DocumentTaxes.rpt");
            ////doc.SetParameterValue("@IsReturn", true, "DocumentTaxes.rpt");

            //var receipt = dc.usp_ReturnReceipt_SelectByID(this.ReturnReceipt_ID);
            //var totalAmount = receipt.Select(x => x.GrossTotalAmount).FirstOrDefault();



            ////doc.SetParameterValue("@TheNo1", totalAmount.ToDecimalOrDefault(), "Tafkit.rpt");
            //doc.SetParameterValue("@TheNo2", "0", "Tafkit.rpt");

            //Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "ReturnReceipt"), false);
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
            doc.Load(Server.MapPath("~\\Reports\\PurchaseInvOrderOut_Print.rpt"));
            doc.SetParameterValue("@ReturnReceipt_ID", this.ReturnReceipt_ID);
            Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "OrderOut"), false);
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
            dc.usp_CancelReturnReceipt_Approvel(this.ReturnReceipt_ID);
            //trans.Commit();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.ReturnReceipt + "?ID=" + this.ReturnReceipt_ID.ToExpressString(), PageLinks.ReturnReceiptsList, PageLinks.ReturnReceipt);
            LogAction(Actions.NotApprove, txtSerial.Text, dc);
        }
        catch (Exception ex)
        {
            //trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }


    #endregion

    #region Private Methods

    protected void ddlTvae_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.Calculate();
    }

    private void FilterByBranchAndCurrency()
    {
        try
        {
            acVendor.ContextKey = "V," + acBranch.Value + "," + ddlCurrency.SelectedValue + ",";
            acCashAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + acBranch.Value + "," + ddlCurrency.SelectedValue + "," + COA.CashOnHand.ToInt().ToExpressString() + ",true";
            acStore.ContextKey = string.Empty + acBranch.Value;
            acCostCenter.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private void FilterItems()
    {
        acItem.ContextKey = ",, ,true";
        acItem.Clear();
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
        //txtCItem.Clear();
        txtQtyInNumber.Clear();
        acBatchID.ContextKey = acItem.Value + "," + acStore.Value;
        acUnit.ContextKey = string.Empty + acItem.Value;
        if (acItem.HasValue)
        {
            var item = dc.usp_Items_SelectByID(acItem.Value.ToNullableInt()).FirstOrDefault();
            txtCost.Text = (item.Cost / (txtRatio.Text.ToNullableDecimal() ?? 1)).ToString("0.####");
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
        acTax.ContextKey = acItemTax.ContextKey = DocumentsTableTypes.Receipt.ToInt().ToExpressString() + ",true";
        this.FilterItems();
        this.BindItemsGrid();
        lblCreatedBy.Text = MyContext.UserProfile.EmployeeName;
        gvTaxes.DataSource = this.dtTaxes;
        gvTaxes.DataBind();

    }

    private void BindItemsGrid()
    {
        this.Calculate(); txtCItem.Clear();
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
    }

    private void SetEditMode()
    {
        if (Request["ID"] != null)
        {
            this.EditMode = true;
            this.ReturnReceipt_ID = Request["ID"].ToInt();
        }

        if (Request["FromReceiptID"] != null)
        {
            this.FromReceipt_ID = Request["FromReceiptID"].ToInt();
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
        int Serial_ID = DocSerials.ReturnReceipt.ToInt();

        int Detail_ID = 0;
        if (!this.EditMode)
        {
            this.ReturnReceipt_ID = dc.usp_ReturnReceipt_Insert(acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimalOrDefault(), txtOperationDate.Text.ToDate(),
                                                     acVendor.Value.ToInt(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate,
                                                     approvedBY_ID, null, null, acAddress.Value.ToNullableInt(), acShipAddress.Value.ToNullableInt(), acPaymentAddress.Value.ToNullableInt(),
                                                     acTelephone.Value.ToNullableInt(), acCostCenter.Value.ToNullableInt(), txtNotes.Text, lblTotal.Text.ToDecimal(), txtPercentageDiscount.Text.ToDecimalOrDefault(),
                                                     txtCashDiscount.Text.ToDecimalOrDefault(), lblTotalDiscount.Text.ToDecimalOrDefault(), lblTotalTax.Text.ToDecimalOrDefault(), lblGrossTotal.Text.ToDecimalOrDefault(), txtFirstPaid.Text.ToDecimalOrDefault(), null, txtUserRefNo.Text, this.DocRandomString, this.FromReceipt_ID, acCashAccount.Value.ToNullableInt(), ddlTvae.SelectedValue.ToNullableInt(), 0);
            if (this.ReturnReceipt_ID > 0)
            {
                foreach (DataRow r in this.dtItems.Rows)
                {
                    if (r.RowState == DataRowState.Deleted) continue;
                    Detail_ID = dc.usp_ReturnReceiptDetails_Insert(this.ReturnReceipt_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["ReceiptDetail_ID"].ToNullableInt(), r["QtyInNumber"].ToDecimalOrDefault(),false,false,false);
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
                    dc.usp_DocuemntTaxes_Insert(DocumentsTableTypes.ReturnReceipt.ToInt(), this.ReturnReceipt_ID, r["Tax_ID"].ToInt());
                }
                if (IsApproving) this.InsertOperation();
                LogAction(IsApproving ? Actions.Approve : Actions.Add, Serial, dc);
            }
        }
        else
        {
            int Result = dc.usp_ReturnReceipt_Update(this.ReturnReceipt_ID, acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimalOrDefault(), txtOperationDate.Text.ToDate(),
                                                    acVendor.Value.ToInt(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate, approvedBY_ID,
                                                    null, null, acAddress.Value.ToNullableInt(), acShipAddress.Value.ToNullableInt(), acPaymentAddress.Value.ToNullableInt(), acTelephone.Value.ToNullableInt(),
                                                    acCostCenter.Value.ToNullableInt(), txtNotes.Text, lblTotal.Text.ToDecimal(), txtPercentageDiscount.Text.ToDecimalOrDefault(), txtCashDiscount.Text.ToDecimalOrDefault(),
                                                    lblTotalDiscount.Text.ToDecimalOrDefault(), lblTotalTax.Text.ToDecimalOrDefault(), lblGrossTotal.Text.ToDecimalOrDefault(), txtFirstPaid.Text.ToDecimalOrDefault(), txtUserRefNo.Text, this.DocRandomString, acCashAccount.Value.ToNullableInt(), ddlTvae.SelectedValue.ToNullableInt(), 0);
            if (Result > 0)
            {
                foreach (DataRow r in this.dtItems.Rows)
                {
                    if (r.RowState == DataRowState.Added)
                    {
                        Detail_ID = dc.usp_ReturnReceiptDetails_Insert(this.ReturnReceipt_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["ReceiptDetail_ID"].ToNullableInt(), r["QtyInNumber"].ToDecimalOrDefault(), false, false, false);
                    }
                    if (r.RowState == DataRowState.Modified)
                    {
                        Detail_ID = r["ID"].ToInt();
                        dc.usp_ReturnReceiptDetails_Update(r["ID"].ToInt(), r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), false, false, false);
                    }
                    if (r.RowState == DataRowState.Deleted)
                    {
                        dc.usp_ReturnReceiptDetails_Delete(r["ID", DataRowVersion.Original].ToInt());
                    }
                    if (IsApproving && r.RowState != DataRowState.Deleted)
                    {
                        if (!this.InsertICJ(Detail_ID, r))
                        {
                            Detail_ID = r.RowState == DataRowState.Added ? Detail_ID : r["ID"].ToInt();
                            trans.Rollback();
                            return false;
                        }
                    }
                }

                foreach (DataRow r in this.dtTaxes.Rows)
                {
                    if (r.RowState == DataRowState.Added)
                    {
                        dc.usp_DocuemntTaxes_Insert(DocumentsTableTypes.ReturnReceipt.ToInt(), this.ReturnReceipt_ID, r["Tax_ID"].ToInt());
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
        Serial = Serial == string.Empty ? string.Empty : " (" + Serial + ") ";
        UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess + Serial, PageLinks.ReturnReceipt + "?ID=" + this.ReturnReceipt_ID.ToExpressString(), PageLinks.ReturnReceiptsList, PageLinks.ReturnReceipt);
        return true;
    }

    private void InsertOperation()
    {
        decimal ratio = txtRatio.Text.ToDecimal();
        string serial = string.Empty;
        var company = dc.usp_Company_Select().FirstOrDefault();
        int ContactAccountID = dc.fun_getContactAccountID(acVendor.Value.ToInt()).Value;

        int Result = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.RetrunPurchases.ToInt(), ddlCurrency.SelectedValue.ToInt(), (this.Total + this.TotalCreditTax) * ratio, (this.Total + this.TotalCreditTax), ratio, txtNotes.Text);

        //Inventory
        dc.usp_OperationDetails_Insert(Result, company.InventoryAccount_ID, 0, (this.Total - this.TotalServices) * ratio, 0, (this.Total - this.TotalServices), null, this.ReturnReceipt_ID, DocumentsTableTypes.ReturnReceipt.ToInt());

        //Vendor
        dc.usp_OperationDetails_Insert(Result, ContactAccountID, (this.GrossTotal) * ratio, 0, (this.GrossTotal), 0, null, this.ReturnReceipt_ID, DocumentsTableTypes.ReturnReceipt.ToInt());

        //Discount
        if (this.TotalDiscount > 0)
        {
            dc.usp_OperationDetails_Insert(Result, company.PurchaseDiscountAccountID, this.TotalDiscount * ratio, 0, this.TotalDiscount, 0, null, this.ReturnReceipt_ID, DocumentsTableTypes.ReturnReceipt.ToInt());
        }

        //Services
        if (this.TotalServices > 0)
        {
            dc.usp_OperationDetails_Insert(Result, company.ServicesExpAccount_ID, 0, this.TotalServices * ratio, 0, this.TotalServices, null, this.ReturnReceipt_ID, DocumentsTableTypes.ReturnReceipt.ToInt());
        }

        //Taxes
        var GroupedTaxes = from tax in this.dtAllTaxes.AsEnumerable()
                           where tax.RowState != DataRowState.Deleted
                           group tax by new { Account_ID = tax.Field<int>("Account_ID"), OnReceiptType = tax.Field<string>("OnReceiptType") } into groupedTaxes
                           select new { Key = groupedTaxes.Key, DebitAmount = groupedTaxes.Sum(x => x.Field<decimal>("DebitAmount")), CreditAmount = groupedTaxes.Sum(x => x.Field<decimal>("CreditAmount")) };
        foreach (var tax in GroupedTaxes)
        {
            dc.usp_OperationDetails_Insert(Result, tax.Key.Account_ID, tax.DebitAmount * ratio, tax.CreditAmount * ratio, tax.DebitAmount, tax.CreditAmount, null, this.ReturnReceipt_ID, DocumentsTableTypes.ReturnReceipt.ToInt());

        }

        //CostCenter
        dc.usp_CostCenterOperation_Insert(acBranch.Value.ToNullableInt(), acCostCenter.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), this.GrossTotal * ratio, this.ReturnReceipt_ID, DocumentsTableTypes.ReturnReceipt.ToInt(), txtNotes.Text);
        this.InsertCashIn();
    }

    private bool InsertICJ(int Detail_ID, DataRow row)
    {
        int result = 0;
        bool? IsService = false;
        result = dc.usp_ICJ_ReturnReceipt(txtOperationDate.Text.ToDate(), row["Quantity"].ToDecimal(), row["Item_ID"].ToInt(), row["Uom_ID"].ToInt(), row["Total"].ToDecimal() * txtRatio.Text.ToDecimal(), row["Batch_ID"].ToNullableInt(), row["Store_ID"].ToInt(), acBranch.Value.ToNullableInt(), DocumentsTableTypes.ReturnReceipt.ToInt(), this.ReturnReceipt_ID, Detail_ID, DocumentsTableTypes.Receipt.ToInt(), row["ReceiptDetail_ID"].ToNullableInt(), ref IsService);
        if (IsService.Value) this.TotalServices += row["Total"].ToDecimal();

        if (result == -32)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.QtyReserved + " (" + row["StoreName"] + " : " + row["ItemName"].ToExpressString() + ")", string.Empty);
            return false;
        }
        if (result == -5)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.CantReturnMoreOriginal + " (" + row["StoreName"] + " : " + row["ItemName"].ToExpressString() + ")", string.Empty);
            return false;
        }
        if (result == -4)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.QtyNotEnough + " (" + row["StoreName"] + " : " + row["ItemName"].ToExpressString() + ")", string.Empty);
            return false;
        }
        return true;
    }

    private void FillReturnReceipt()
    {
        var receipt = dc.usp_ReturnReceipt_SelectByID(this.ReturnReceipt_ID).FirstOrDefault();

        ddlCurrency.SelectedValue = receipt.Currency_ID.ToExpressString();


        ddlTvae.SelectedValue = receipt.IsTax.ToExpressString();
        this.IsTaxFound = receipt.IsTax.ToIntOrDefault();

        OperationsView.SourceDocTypeType_ID = DocumentsTableTypes.ReturnReceipt.ToInt();
        OperationsView.Source_ID = this.ReturnReceipt_ID;



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
        this.FromReceipt_ID = receipt.FromReceiptID;

        if (!string.IsNullOrEmpty(receipt.FromReceiptSerial))
        {
            lblFromReceiptNo.Text = receipt.FromReceiptSerial;
            divPurchaseOrderNo.Visible = true;
            ddlCurrency.Enabled = false;
            acBranch.Enabled = false;
            acVendor.Enabled = false;
        }

        if (EditMode) //Not Important
        {
            txtSerial.Text = receipt.Serial;
            ucNav.SetText = receipt.Serial;

            this.DocRandomString = receipt.DocRandomString;
            lblCreatedBy.Text = receipt.CreatedByName;
            lblApprovedBy.Text = receipt.ApprovedBYName;
            this.ImgStatus = ((DocStatus)receipt.DocStatus_ID).ToExpressString();
            btnPrint.Visible = MyContext.PageData.IsPrint;
            btnPrintInventoryOrder.Visible = MyContext.PageData.IsPrint;
            pnlAddItem.Visible = (receipt.DocStatus_ID == 1);
            btnCancel.Visible = (receipt.DocStatus_ID == 1) && MyContext.PageData.IsDelete;
            btnApprove.Visible = (receipt.DocStatus_ID == 1) && MyContext.PageData.IsApprove;
            btnCancelApprove.Visible =! btnApprove.Visible && (receipt.DocStatus_ID == 2) && MyContext.PageData.IsNotApprove; ;
            btnSave.Visible = (receipt.DocStatus_ID == 1) && ((MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode));
            this.DocStatus_ID = receipt.DocStatus_ID.Value;
            OperationsView.Visible = !btnApprove.Visible;
        }

        //this.dtItems = dc.usp_ReturnReceiptDetails_Select(this.ReturnReceipt_ID).CopyToDataTable();

        var lstItems = dc.usp_ReturnReceiptDetails_Select(this.ReturnReceipt_ID).ToList();
        this.dtItems = lstItems.CopyToDataTable();
        if (lstItems.Count() > 0)
        {
            acStore.Value = lstItems.FirstOrDefault().Store_ID.ToExpressString();
        }

        this.dtTaxes = dc.usp_DocuemntTaxes_Select(DocumentsTableTypes.ReturnReceipt.ToInt(), this.ReturnReceipt_ID, true).CopyToDataTable();
        this.BindItemsGrid();
        gvTaxes.DataSource = this.dtTaxes;
        gvTaxes.DataBind();

    }

    private void FillFromReceipt()
    {
        if (this.FromReceipt_ID == null) return;
        var receipt = dc.usp_Receipt_SelectByID(this.FromReceipt_ID).FirstOrDefault();

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

        ddlCurrency.Enabled = false;
        acBranch.Enabled = false;
        acVendor.Enabled = false;


        this.dtItems = dc.usp_ReceiptDetailsForReturn_Select(this.FromReceipt_ID).CopyToDataTable();
        this.dtTaxes = dc.usp_DocuemntTaxes_Select(DocumentsTableTypes.Receipt.ToInt(), this.FromReceipt_ID, true).CopyToDataTable();
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
        btnPrint.Visible = MyContext.PrintAfterApprove || btnApprove.Visible;
        btnCancel.Visible = MyContext.PageData.IsDelete && EditMode;
        OperationsView.Visible = !btnApprove.Visible;
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
          
            r["Total"] = r["UnitCost"].ToDecimal() * r["Quantity"].ToDecimal();
            Total += r["UnitCost"].ToDecimal() * r["Quantity"].ToDecimal();
            this.TotalDiscount += ItemDiscount = (r["PercentageDiscount"].ToDecimal() * r["Total"].ToDecimal() * 0.01m) + r["CashDiscount"].ToDecimal();
            if (r["TaxOnReceiptType"] != DBNull.Value && r["Tax_ID"] != DBNull.Value)
            {
                r["TotalTax"] = ((r["Total"].ToDecimal() - ItemDiscount) * TaxValue * 0.01m);

                if (r["TaxOnReceiptType"].ToExpressString() == "C")
                {
                    this.dtAllTaxes.Rows.Add(r["TaxPurchaseAccountID"].ToInt(), r["TaxOnReceiptType"].ToExpressString(), 0, r["TotalTax"].ToDecimal());
                    this.TotalCreditTax += r["TotalTax"].ToDecimal();
                }
                else
                {
                    this.dtAllTaxes.Rows.Add(r["TaxPurchaseAccountID"].ToInt(), r["TaxOnReceiptType"].ToExpressString(), r["TotalTax"].ToDecimal(), 0);
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
            if (r["OnReceiptType"] != DBNull.Value)
            {
                DocTaxValue = ((this.GrossTotal - DocDiscount) * r["PercentageValue"].ToDecimal() * 0.01m);
                if (r["OnReceiptType"].ToExpressString() == "C")
                {
                    this.dtAllTaxes.Rows.Add(r["PurchaseAccountID"].ToInt(), r["OnReceiptType"].ToExpressString(), 0, DocTaxValue);
                    this.TotalCreditTax += DocTaxValue;
                }
                else
                {
                    this.dtAllTaxes.Rows.Add(r["PurchaseAccountID"].ToInt(), r["OnReceiptType"].ToExpressString(), DocTaxValue, 0);
                    this.TotalDebitTax += DocTaxValue;
                    DocTaxValue *= -1m;
                }
                DocTax += DocTaxValue;
            }
        }
        this.TotalDiscount += DocDiscount;
        this.TotalTax += DocTax;
        this.GrossTotal = this.GrossTotal - DocDiscount + DocTax;
        lblTotal.Text = this.Total.ToString(MyContext.FormatNumber);
        lblTotalDiscount.Text = this.TotalDiscount.ToString(MyContext.FormatNumber);
        lblTotalTax.Text = this.TotalTax.ToString(MyContext.FormatNumber);
        lblGrossTotal.Text = this.GrossTotal.ToString(MyContext.FormatNumber);
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

    private void InsertCashIn()
    {
        string Serial = string.Empty;
        decimal ratio = txtRatio.Text.ToDecimal();
        int? CashIn_ID = null;
        if (txtFirstPaid.Text.ToDecimalOrDefault() <= 0) return;
        int ContactChartOfAccount_ID = dc.fun_getContactAccountID(acVendor.Value.ToInt()).Value;
        CashIn_ID = dc.usp_Payments_Insert(txtOperationDate.Text.ToDate(), DateTime.Now, MyContext.UserProfile.Contact_ID, MyContext.UserProfile.Contact_ID, DateTime.Now, txtUserRefNo.TrimmedText, ref Serial, DocSerials.CashIn.ToInt(), txtNotes.Text, txtFirstPaid.Text.ToDecimal(), null, DocStatus.Approved.ToByte(), PaymentsTypes.CashIn.ToByte(), acBranch.Value.ToNullableInt(), txtRatio.Text.ToDecimal(), ddlCurrency.SelectedValue.ToInt(), this.ReturnReceipt_ID, DocumentsTableTypes.ReturnReceipt.ToInt(), this.DocRandomString + "_FromInvoice");
        if (!CashIn_ID.HasValue || CashIn_ID.Value <= 0) throw new Exception("Error Occured During inserting the cash document");
        dc.usp_PaymentsDetails_Insert(CashIn_ID, acCashAccount.Value.ToInt(), ContactChartOfAccount_ID, txtFirstPaid.Text.ToDecimal(), null, string.Empty, null);
        int Operation_ID = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref Serial, DocStatus.Approved.ToByte(), OperationTypes.CashIn.ToInt(), ddlCurrency.SelectedValue.ToInt(), txtFirstPaid.Text.ToDecimal() * ratio, txtFirstPaid.Text.ToDecimal(), ratio, null);
        dc.usp_OperationDetails_Insert(Operation_ID, ContactChartOfAccount_ID, 0, txtFirstPaid.Text.ToDecimal() * ratio, 0, txtFirstPaid.Text.ToDecimal(), null, CashIn_ID, DocumentsTableTypes.Payment_CashIn.ToInt());
        dc.usp_OperationDetails_Insert(Operation_ID, acCashAccount.Value.ToInt(), txtFirstPaid.Text.ToDecimal() * ratio, 0, txtFirstPaid.Text.ToDecimal(), 0, null, CashIn_ID, DocumentsTableTypes.Payment_CashIn.ToInt());
        dc.usp_SetCashDocForBills(this.ReturnReceipt_ID, CashIn_ID, DocumentsTableTypes.ReturnReceipt.ToInt());
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
        var totalPrice = txtQty.Text.ToDecimalOrDefault() * txtCost.Text.ToDecimalOrDefault();
        var calc = (totalPrice - txtItemCashDiscount.Text.ToDecimalOrDefault() - txtItemPercentageDiscount.Text.ToDecimalOrDefault() * (totalPrice) / 100);
        lblTotalRow.Text = (string.IsNullOrEmpty(acItemTax.Value) ? calc : (calc + (calc * (Tax != null ? Tax.PercentageValue.Value : 0) / 100))).ToExpressString();
    }

    #endregion



}
