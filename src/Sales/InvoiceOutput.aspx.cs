
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


public partial class Sales_InvoiceOutput : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties


    private int InflaID
    {
        get
        {
            if (ViewState["InflaID"] == null) return 0;
            return (int)ViewState["InflaID"];
        }

        set
        {
            ViewState["InflaID"] = value;
        }
    }

    private int Item_ID
    {
        get
        {
            if (ViewState["Item_ID"] == null) return 0;
            return (int)ViewState["Item_ID"];
        }

        set
        {
            ViewState["Item_ID"] = value;
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

    private int InvoiceInit_ID
    {
        get
        {
            if (ViewState["InvoiceInit_ID"] == null) return 0;
            return (int)ViewState["InvoiceInit_ID"];
        }

        set
        {
            ViewState["InvoiceInit_ID"] = value;
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


    private void SetEditMode()
    {
        if (Request["ID"] != null)
        {
            this.EditMode = false;
            this.InvoiceInit_ID = Request["ID"].ToInt();
        }
        if (Request["Output_ID"] != null)
        {
            this.EditMode = false;
            this.Invoice_ID = Request["Output_ID"].ToInt();
        }

    }


    private DataTable dtItems
    {
        get
        {
            if (Session["dtItems_Invoice" + this.WinID] == null)
            {
                // Session["dtItems_Invoice" + this.WinID] = dc.usp_InvoiceDetails_Select(null).CopyToDataTable();
                Session["dtItems_Invoice" + this.WinID] = dc.usp_InvoiceDetailsIncludeTax_Select(null).CopyToDataTable();
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
            if (ViewState["PRID"] == null) return 0;
            return (int)ViewState["PRID"];
        }

        set
        {
            ViewState["PRID"] = value;
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
            this.gvItems.FormatNumber = MyContext.FormatNumber;
            if (!Page.IsPostBack)
            {
                this.DocRandomString.ToString();
                this.CheckSecurity();
                this.LoadControls();
                this.SetDefaults();
                if (this.InvoiceInit_ID > 0)
                {
                    this.FillInvoice();
                }
                if (this.Invoice_ID > 0)
                {
                    this.FillOutputInvoice();
                }

                this.VisibilityControl();
                this.txtQty.Text = "1";
                IsRequiresField();


            }
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
                        case 6: chkCado.Focus();
                            break;
                        case 5: chkCado.Focus();
                            break;
                        case 7: chkCado.Focus();
                            break;
                        case 8: LinkButton1.Focus();
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
    private void setFocus(string focusFlag)
    {
        string s = "<SCRIPT language='javascript'>document.getElementById('" + focusFlag + "').focus(); </SCRIPT>";
        Page.RegisterStartupScript("focus", s);
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
    private void VisibilityControl()
    {
        var obj = dc.usp_Company_Select().First();
        if (obj.IsDescribed != null)
        {
            // txtDescribed.Visible = obj.IsDescribed.Value;
            pnlItemdescribed.Visible = obj.IsDescribed.Value;
            //acItemDescribed.Visible =;
            acItemDescribed.IsRequired = obj.IsDescribed.Value;
            // gvItems.Columns[3].Visible = obj.IsDescribed.Value;
        }
    }
    private void IsRequiresField()
    {
        //foreach (var control in dc.usp_HiddenControls_Select(MyContext.PageData.PageID, MyContext.UserProfile.Contact_ID))
        //{
        //    if (control.ControlUniqueID == "cph_txtPolicy")
        //    {
        //        txtPolicy.IsRequired = false;
        //        gvItems.Columns[6].Visible = false;
        //    }
        //    if (control.ControlUniqueID == "cph_txtCode")
        //    {
        //        txtCode.IsRequired = false;
        //        gvItems.Columns[5].Visible = false;
        //    }
        //    if (control.ControlUniqueID == "cph_txtInvoiceDate")
        //    {
        //        txtInvoiceDate.IsRequired = false;
        //    }

        //    if (control.ControlUniqueID == "cph_txtCapacity")
        //    {

        //        gvItems.Columns[8].Visible = false;
        //    }
        //    if (control.ControlUniqueID == "cph_txtCapacities")
        //    {

        //        gvItems.Columns[9].Visible = false;
        //    }
        //}
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
            acCustomerMesure.ContextKey = acCustomer.Value + "," + "111";
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
            //  if (sender != null) this.FocusNextControl(sender);
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
            // if (sender != null) this.FocusNextControl(sender);
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
            // if (sender != null) this.FocusNextControl(sender);
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
            //if (sender != null) this.FocusNextControl(sender);
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
            //  acUnit.Focus();

            var itsUnits = dc.usp_ItemsUnits_Select(acItem.Value.ToIntOrDefault()).ToList().Where(x => x.IsFavorite == true).FirstOrDefault();
            if (itsUnits != null)
            {
                acUnit.Value = itsUnits.Unit_ID.ToExpressString();
            }


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
            if (string.IsNullOrEmpty(acPriceName.Value))
            {


                if (sender != null) txtCost.Text = (dc.fun_GetItemDefaultPriceByUOM(acItem.Value.ToNullableInt(), acUnit.Value.ToNullableInt(), acPriceName.Value.ToNullableInt()) / (txtRatio.Text.ToNullableDecimal() ?? 1)).Value.ToString("0.########");
                if (sender != null) this.ShowAvailableQty();
                // if (sender != null) this.FocusNextControl(sender);
                txtQty.Focus();
            }
            else
            {
                if (sender != null) txtCost.Text = (dc.fun_GetItemPriceDefaultPriceByUOM(acItem.Value.ToNullableInt(), acUnit.Value.ToNullableInt(), acPriceName.Value.ToNullableInt()) / (txtRatio.Text.ToNullableDecimal() ?? 1)).Value.ToString("0.########");
                if (sender != null) this.ShowAvailableQty();
            }
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
            if (sender != null) txtCost.Text = (dc.fun_GetItemPriceDefaultPriceByUOM(acItem.Value.ToNullableInt(), acUnit.Value.ToNullableInt(), acPriceName.Value.ToNullableInt()) / (txtRatio.Text.ToNullableDecimal() ?? 1)).Value.ToString("0.########");
            // if (sender != null) this.FocusNextControl(sender);
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
            // if (acItem.HasValue) txtQty.Focus(); else this.FocusNextControl(acStore);
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


            if (chkCado.Checked)
            {
                mpeOffer.Show();
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
            //r["UnitCostEvaluate"] = txtCost.Text;
            r["UnitCostEvaluate"] = 0;
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
            lblTotalRow.Text = "";
            txtBarcode.Focus();
            //if (acStore.HasValue) this.FocusNextControl(acStore); else acStore.AutoCompleteFocus();
            this.EditID = 0;
            // txtFirstPaid.Text = lblGrossTotal.Text;
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

                acItem.ContextKey = ",,,true";

            }
            // if (sender != null) this.FocusNextControl(sender);
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
            //  if (sender != null) this.FocusNextControl(sender);
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
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.Invoice + "?ID=" + this.Invoice_ID.ToExpressString(), PageLinks.InvoicesList + Request.PathInfo, PageLinks.InvoiceShortcut);
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

            // if (sender != null) this.FocusNextControl(sender);
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
            // txtFirstPaid.Text = lblGrossTotal.Text;
            //if (sender != null) this.FocusNextControl(sender);
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
            dc.usp_ContactDetails_insert(Contact_ID, 8, txtMobileNumner.Text.Trim());
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
            //Response.Redirect("~/Report_Dev/PrintInnoiceeDev.aspx?Invoice_ID=" + this.InvoiceInit_ID + "&IsMaterla=3");
            Response.Redirect("~/Report_Dev/PrintInnoiceeOutDev.aspx?Invoice_ID=" + this.InvoiceInit_ID + "&IsMaterla=3");

            // XpressDataContext dataContext = new XpressDataContext();

            // ReportDocument doc = new ReportDocument();
            // doc.Load(Server.MapPath("~\\Reports\\Invoice_Print.rpt"));
            // doc.SetParameterValue("@Invoice_ID", this.Invoice_ID);
            // doc.SetParameterValue("@DocTableType_ID", DocumentsTableTypes.Invoice.ToInt(), "DocumentTaxes.rpt");
            // doc.SetParameterValue("@Doc_ID", this.Invoice_ID, "DocumentTaxes.rpt");
            // doc.SetParameterValue("@IsReturn", false, "DocumentTaxes.rpt");
            // //doc.SetParameterValue("BaseDir", HttpContext.Current.Server.MapPath("~\\Uploads\\"));
            ////doc.SetParameterValue("ByEmp", ((UICulturePage)HttpContext.Current.Handler).MyContext.UserProfile.EmployeeName);
            //// doc.SetParameterValue("@Branch_ID", acBranch.Value.ToNullableInt());
            // Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "Invoice"), false);


            //XpressDataContext dataContext = new XpressDataContext();

            //ReportDocument doc = new ReportDocument();
            //doc.Load(Server.MapPath("~\\Reports\\Invoice_Print ssb.rpt"));
            //doc.SetParameterValue("@Invoice_ID", this.Invoice_ID);

            ////doc.SetParameterValue("BaseDir", HttpContext.Current.Server.MapPath("~\\Uploads\\"));
            ////doc.SetParameterValue("ByEmp", ((UICulturePage)HttpContext.Current.Handler).MyContext.UserProfile.EmployeeName);
            ////doc.SetParameterValue("@Branch_ID", acBranch.Value.ToNullableInt());
            //Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "Invoice"), false);


            //  XpressDataContext dataContext = new XpressDataContext();

            //ReportDocument doc = new ReportDocument();
            //doc.Load(Server.MapPath("~\\Reports\\Invoice_Print ssb.rpt"));
            //doc.SetParameterValue("@Invoice_ID", this.Invoice_ID);

            //doc.SetParameterValue("BaseDir", HttpContext.Current.Server.MapPath("~\\Uploads\\"));
            //doc.SetParameterValue("ByEmp", ((UICulturePage)HttpContext.Current.Handler).MyContext.UserProfile.EmployeeName);
            //doc.SetParameterValue("@Branch_ID", acBranch.Value.ToNullableInt());
            //Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "Invoice"), false);





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


            Response.Redirect("~/Report_Dev/PrintInnoiceeOutDev.aspx?Invoice_ID=" + this.Invoice_ID);




            //ReportDocument doc = new ReportDocument();
            //doc.Load(Server.MapPath("~\\Reports\\InvOrderOut_Print.rpt"));
            //doc.SetParameterValue("@Invoice_ID", this.Invoice_ID);
            //Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "OrderOut"), false);
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


    protected void acItemDescribed_OnSelectedIndexChanged(object sender, AutoCompleteEventArgs e)
    {
        try
        {

            this.ShowAvailableQty();

            //if (sender != null) this.FocusNextControl(sender);
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
            //this.ClearItemForm();
            //txtCost.Clear();
            acItemDescribed.Clear();
            this.BindItemsGrid();
            if (acStore.HasValue) this.FocusNextControl(acStore); else acStore.AutoCompleteFocus();
            this.EditID = 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void btnPrint_Dev_Click(object sender, EventArgs e)
    {
        try
        {


            Response.Redirect("~/Report_Dev/PrintInnoiceeDev.aspx?Invoice_ID=" + this.Invoice_ID + "&IsMaterla=2");


            //Response.Redirect("~/Report_Dev/PrintInnoiceeDev.aspx?Invoice_ID=" + this.Invoice_ID);
        }
        catch (Exception)
        {

            throw;
        }

    }
    protected void btnPrint_Design_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Report_Dev/PrintBarcodeDesign.aspx?Invoice_ID=" + this.Invoice_ID);
    }
    protected void txtQty_TextChanged(object sender, EventArgs e)
    {

        CalculateTotalRow();
        txtCost.Focus();
        // this.FocusNextControl(sender);
    }

    //private void CalculateTotalRow()
    //{
    //    //var Tax = dc.usp_Taxes_Select(acItemTax.Value.ToInt(), string.Empty).FirstOrDefault();
    //    //var totalPrice = txtQty.Text.ToDecimalOrDefault() * txtCost.Text.ToDecimalOrDefault();
    //    //var calc = (totalPrice - txtItemCashDiscount.Text.ToDecimalOrDefault() - txtItemPercentageDiscount.Text.ToDecimalOrDefault() * (totalPrice) / 100);
    //    //lblTotalRow.Text = (string.IsNullOrEmpty(acItemTax.Value) ? calc : (calc + (calc * (Tax != null ? Tax.PercentageValue.Value : 0) / 100))).ToExpressString();
    //}



    protected void txtCost_TextChanged(object sender, EventArgs e)
    {
        CalculateTotalRow();

        //  this.FocusNextControl(sender);
    }

    protected void BtnAccountstatementIteme_Click(object sender, EventArgs e)
    {

        //  ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('~/AccountingReports/CustomerAccountStatementByItems.aspx','_blank')", true);
        Response.Redirect1("~/AccountingReports/CustomerAccountStatementByItems.aspx", "_blank", "");
    }
    protected void BtnAccountstatement_Click(object sender, EventArgs e)
    {
        Response.Redirect1("~/AccountingReports/CustomerStatment.aspx", "_blank", "");
        //Response.Redirect("~/AccountingReports/CustomerStatment.aspx"); 
    }

    protected void btnCancelApprove_Click(object sender, EventArgs e)
    {
        //System.Data.Common.DbTransaction trans;
        //dc.Connection.Open();
        //trans = dc.Connection.BeginTransaction();
        //dc.Transaction = trans;
        try
        {
            dc.usp_CancelInvoice_Approvel(this.Invoice_ID);
            LogAction(Actions.NotApprove, txtSerial.Text, dc);
            //trans.Commit();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.InvoiceShortcut + "?ID=" + this.Invoice_ID.ToExpressString(), PageLinks.InvoicesList, PageLinks.InvoiceShortcut);

        }
        catch (Exception ex)
        {
            // trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnAddOkOffer_Click(object sender, EventArgs e)
    {
        try
        {
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
            r["Item_ID"] = acItem.Value;
            r["ItemName"] = acItem.Text;

            r["IDCodeOperation"] = txtCode.Text;
            r["Policy"] = txtPolicy.Text;
            r["Capacity"] = txtCapacity.Text;
            r["Capacities"] = txtCapacities.Text;
            r["ItemDescription"] = acItemDescribed.Value;
            r["DescribedName"] = acItemDescribed.Text;
            r["UnitCost"] = txtCost.Text;
            r["UnitCostEvaluate"] = txtCost.Text;
            r["Quantity"] = txtQty.Text;
            r["QtyInNumber"] = txtQtyInNumber.Text.ToDecimalOrDefault();
            r["Uom_ID"] = acUnit.Value;
            r["UOMName"] = acUnit.Text;
            r["Batch_ID"] = (acBatchID.HasValue && acBatchID.Value.ToNullableInt() != 0) ? acBatchID.Value.ToIntOrDBNULL() : DBNull.Value;
            r["BatchName"] = (acBatchID.HasValue && acBatchID.Value.ToNullableInt() != 0) ? acBatchID.Text.Substring(0, acBatchID.Text.IndexOf((char)65279)) : string.Empty;
            r["PercentageDiscount"] = chkSame.Checked ? 100 : txtItemPercentageDiscount.Text.ToDecimalOrDefault();
            r["IsGift"] = chkSame.Checked ? true : false;




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
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }

        try
        {


            if (txtQtyOffer.Text.ToDecimalOrDefault() > 0)
            {
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
                r["IsGift"] = true;
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
                r["Quantity"] = txtQtyOffer.Text;
                r["QtyInNumber"] = txtQtyInNumber.Text.ToDecimalOrDefault();
                r["Uom_ID"] = acUnit.Value;
                r["UOMName"] = acUnit.Text;
                r["Batch_ID"] = (acBatchID.HasValue && acBatchID.Value.ToNullableInt() != 0) ? acBatchID.Value.ToIntOrDBNULL() : DBNull.Value;
                r["BatchName"] = (acBatchID.HasValue && acBatchID.Value.ToNullableInt() != 0) ? acBatchID.Text.Substring(0, acBatchID.Text.IndexOf((char)65279)) : string.Empty;
                r["PercentageDiscount"] = txtQtyOffer.Text.ToDecimalOrDefault() > 0 ? 100 : txtItemPercentageDiscount.Text.ToDecimalOrDefault();
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
            }




            // txtFirstPaid.Text = lblGrossTotal.Text;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }

        this.ClearItemForm();
        this.BindItemsGrid();
        lblTotalRow.Text = "";
        txtBarcode.Focus();
        chkCado.Checked = false;
        //if (acStore.HasValue) this.FocusNextControl(acStore); else acStore.AutoCompleteFocus();
        this.EditID = 0;
    }


    protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Label lbl = e.Row.FindControl("lblGift") as Label;
        Image img = e.Row.FindControl("imgGift") as Image;
        if (img != null)
        {
            img.Visible = lbl != null ? lbl.Text.ToBooleanOrDefault() : false;
        }


        //Checking the RowType of the Row  
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ////If Salary is less than 10000 than set the row Background Color to Cyan  
            //if (Convert.ToInt32(e.Row.Cells[3].Text) < 10000)
            //{

            //}
        }
    }





    #endregion

    #region Private Methods
    private void FilterByBranchAndCurrency()
    {
        try
        {
            acCustomer.ContextKey = "C," + acBranch.Value + "," + ddlCurrency.SelectedValue + ",";
            //acCustomer.ContextKey = "xx," + acBranch.Value + "," + ddlCurrency.SelectedValue + ",";

            //acCustomer.ContextKey = "C," + acBranch.Value + "," + ddlCurrency.SelectedValue + ",";
            acStore.ContextKey = string.Empty + acBranch.Value;
            if (MyContext.UserProfile.Branch_ID == null)
            {
                acCostCenter.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",false,";
                //acCashAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",," + ddlCurrency.SelectedValue + "," + COA.CashOnHand.ToInt().ToExpressString() + ",true";

            }
            else
            {
                acCostCenter.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",false," + acBranch.Value;
                // acCashAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + acBranch.Value + "," + ddlCurrency.SelectedValue + "," + COA.CashOnHand.ToInt().ToExpressString() + ",true";

            }


        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private void FilterItems()
    {
        acItem.ContextKey = "," + acCategory.Value + "," + acStore.Value + ",true";
        //acItem.ContextKey = ",,,true";
        acItem.Clear();
        this.FilterItemsData();
    }
    private void FilterItemsDescribed()
    {
        acItemDescribed.ContextKey = "," + acCategory.Value + "!" + acItem.Value + ",,true";
        var list = dc.Items.Where(x => x.ID == acItem.Value.ToNullableInt()).ToList();
        if (list.Any())
        {
            if (list.First().Type == 's')
            {
                acItemDescribed.IsRequired = false;
            }
        }
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
        //txtCItem.Clear();
        txtQty.Clear();
        txtQtyInNumber.Clear();
        acBatchID.ContextKey = acItem.Value + "," + acStore.Value + "," + acUnit.Value + ",True";
        acUnit.ContextKey = string.Empty + acItem.Value;

        acPriceName.ContextKey = string.Empty + acItem.Value;
        if (acItem.HasValue)
        {
            var item = dc.usp_Items_SelectByID(acItem.Value.ToNullableInt()).FirstOrDefault();
            txtCost.Text = (item.DefaultPrice.Value / (txtRatio.Text.ToNullableDecimal() ?? 1)).ToString("0.########");
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
        this.QuantityWarning = dc.usp_Company_Select().FirstOrDefault().QuantityWarning.Value;

        acArea.ContextKey = string.Empty;
        acdrivers.ContextKey = string.Empty;
        // acParentAccount.ContextKey =",";
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
        // acBranch.Enabled = (gvItems.Rows.Count == 0) && (this.MyContext.UserProfile.Branch_ID == null);
    }

    private void ClearItemForm()
    {
        // acStore.Clear();
        if (MyContext.UserProfile.Store_ID != null) acStore.Value = MyContext.UserProfile.Store_ID.ToStringOrEmpty();
        acCategory.Clear();
        txtCItem.Clear();
        txtQtyOffer.Clear();
        chkSame.Checked = false;
        this.FilterItems();
        this.acItem_SelectedIndexChanged(null, null);


    }

    //private void SetEditMode()
    //{
    //    if (Request["ID"] != null)
    //    {
    //        this.EditMode = true;
    //        this.Invoice_ID = Request["ID"].ToInt();
    //    }
    //    if (Request["SalesOrderID"] != null)
    //    {
    //        this.SalesOrderID = Request["SalesOrderID"].ToInt();
    //    }
    //}

    private bool Save(bool IsApproving, System.Data.Common.DbTransaction trans)
    {
        if (dtItems.AsEnumerable().Where(r => r.RowState != DataRowState.Deleted).Count() == 0)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.OneItemRequired, string.Empty);
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



        this.CalculatedSalesCost = 0;
        this.ConfirmationMessage = string.Empty;
        //if (IsApproving && !this.CheckCreditLmit())
        //{
        //    trans.Rollback();
        //    return false;
        //}

        string Serial = string.Empty;
        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        DateTime? ApproveDate = IsApproving ? DateTime.Now : (DateTime?)null;
        int? approvedBY_ID = IsApproving ? MyContext.UserProfile.Contact_ID : (int?)null;
        byte EntryType = 10;
        int Serial_ID = DocSerials.InvoiceOutput.ToInt();
        int? SalesOrderID = this.SalesOrderID == 0 ? (int?)null : this.SalesOrderID;
        int Detail_ID = 0;
        var cmp = dc.usp_Company_Select().FirstOrDefault();
        var nbrReceived = dc.Invoices.Where(x => x.EntryType == EntryType && x.SalesOrder_ID == this.InvoiceInit_ID).ToList();
        if (!this.EditMode)
        {
            this.Invoice_ID = dc.usp_InvoiceDateDelivery_Insert(acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimalOrDefault(), txtOperationDate.Text.ToDate(),
                                                       acCustomer.Value.ToInt(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate,
                                                       approvedBY_ID, null, null, acAddress.Value.ToNullableInt(), acShipAddress.Value.ToNullableInt(), acPaymentAddress.Value.ToNullableInt(),
                                                       acTelephone.Value.ToNullableInt(), acCostCenter.Value.ToNullableInt(), txtNotes.Text, lblTotal.Text.ToDecimal(), txtPercentageDiscount.Text.ToDecimalOrDefault(),
                                                       txtCashDiscount.Text.ToDecimalOrDefault(), txtAdditionals.Text.ToDecimalOrDefault(), lblTotalDiscount.Text.ToDecimalOrDefault(), lblTotalTax.Text.ToDecimalOrDefault(),
                                                       lblGrossTotal.Text.ToDecimalOrDefault(), txtFirstPaid.Text.ToDecimalOrDefault(), null, txtUserRefNo.Text, this.DocRandomString, EntryType,
                                                       this.InvoiceInit_ID, acSalesRep.Value.ToNullableInt(), acCashAccount.Value.ToNullableInt(), acCustomerMesure.Value.ToNullableInt(),
                                                       txtDeliveryDate.Text.ToDate(), acdrivers.Value.ToNullableInt(), 1, 1, "","", "", 
                                                       txtProjectRef.Text, txtContactPerson.Text,string.Empty, txtCustomerRepresentative.Text,null,"");
            if (this.Invoice_ID > 0)
            {
                foreach (DataRow r in this.dtItems.Rows)
                {
                    if (r.RowState == DataRowState.Deleted) continue;
                    if (r["QuantityRecived"].ToDecimal() > 0)
                    {
                        var lstIDReceiptReceived = nbrReceived.Select(x => x.ID).ToList();

                        var qtyReceObject = dc.InvoiceDetails.Where(x => lstIDReceiptReceived.Contains(x.Invoice_ID.Value) && x.Item_ID == r["Item_ID"].ToInt()).Sum(z => z.Quantity).ToDecimalOrDefault();
                        var OrderPDetailSumQuatity = dc.InvoiceDetails.Where(x => x.Invoice_ID == this.InvoiceInit_ID && x.Item_ID == r["Item_ID"].ToInt()).Sum(c => c.Quantity).ToDecimalOrDefault();

                        if (qtyReceObject + r["QuantityRecived"].ToDecimal() > OrderPDetailSumQuatity)
                        {
                            trans.Rollback();
                            UserMessages.Message(null, " هذا الصنف كميته التي تسلم اكبر من كمية الفاتورة  " + " (" + r["StoreName"] + " : " + r["ItemName"].ToExpressString() + ")", string.Empty);
                            return false;
                        }

                        r["Quantity"] = r["QuantityRecived"].ToDecimal();
                        Detail_ID = dc.usp_InvoiceDetailsIncludeTax_Insert(this.Invoice_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["QuantityRecived"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["Capacity"].ToString(), r["ItemDescription"].ToString(), r["IDCodeOperation"].ToString(), r["Policy"].ToString(), r["InvoiceDate"].ToDate(), r["Capacities"].ToString(), r["IsGift"].ToBooleanOrDefault(), r["UnitCostEvaluate"].ToDecimalOrDefault(),
                            false,null,null);
                        if (IsApproving)
                            if (!this.InsertICJ(Detail_ID, r))
                            {
                                trans.Rollback();
                                return false;
                            }
                    }
                }

                if (IsApproving) this.InsertOperation();
                LogAction(IsApproving ? Actions.Approve : Actions.Add, Serial, dc);
            }
        }
        else
        {
            int Result = dc.usp_InvoiceDelivery_Update(this.Invoice_ID, acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimalOrDefault(), txtOperationDate.Text.ToDate(),
                                                    acCustomer.Value.ToInt(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate, approvedBY_ID,
                                                    null, null, acAddress.Value.ToNullableInt(), acShipAddress.Value.ToNullableInt(), acPaymentAddress.Value.ToNullableInt(), acTelephone.Value.ToNullableInt(),
                                                    acCostCenter.Value.ToNullableInt(), txtNotes.Text, lblTotal.Text.ToDecimal(), txtPercentageDiscount.Text.ToDecimalOrDefault(), txtCashDiscount.Text.ToDecimalOrDefault(), txtAdditionals.Text.ToDecimalOrDefault(),
                                                    lblTotalDiscount.Text.ToDecimalOrDefault(), lblTotalTax.Text.ToDecimalOrDefault(), lblGrossTotal.Text.ToDecimalOrDefault(),
                                                    txtFirstPaid.Text.ToDecimalOrDefault(), txtUserRefNo.Text, this.DocRandomString, acSalesRep.Value.ToNullableInt(),
                                                    acCashAccount.Value.ToNullableInt(), txtDeliveryDate.Text.ToDate(), acdrivers.Value.ToNullableInt(), 1, 1, string.Empty,string.Empty, string.Empty, txtProjectRef.Text, 
                                                    txtContactPerson.Text,string.Empty, txtCustomerRepresentative.Text);
            if (Result > 0)
            {
                foreach (DataRow r in this.dtItems.Rows)
                {

                    var lstIDReceiptReceived = nbrReceived.Select(x => x.ID).ToList();

                    var qtyReceObject = dc.InvoiceDetails.Where(x => lstIDReceiptReceived.Contains(x.Invoice_ID.Value) && x.Item_ID == r["Item_ID"].ToInt()).Sum(z => z.Quantity);
                    var OrderPDetailSumQuatity = dc.ReceiptDetails.Where(x => x.Receipt_ID == SalesOrderID && x.Item_ID == r["Item_ID"].ToInt()).Sum(c => c.Quantity);


                    if (qtyReceObject + r["QuantityRecived"].ToDecimal() > OrderPDetailSumQuatity)
                    {
                        trans.Rollback();
                        UserMessages.Message(null, " هذا الصنف كميته التي تسلم اكبر من كمية الفاتورة  " + " (" + r["StoreName"] + " : " + r["ItemName"].ToExpressString() + ")", string.Empty);
                        return false;
                    }

                    if (r["QuantityRecived"] != DBNull.Value && r["QuantityRecived"].ToDecimal() != r["Quantity"].ToDecimal())
                    {
                        UserMessages.Message(null, "لا يمكن الاعتماد بعض الاصناف غير مستلمة", string.Empty);
                        trans.Rollback();
                        return false;
                    }

                    if (r["QuantityRecived"].ToDecimal() > 0)
                    {

                        r["Quantity"] = r["QuantityRecived"].ToDecimal();

                        if (r.RowState == DataRowState.Added)
                        {
                            Detail_ID = dc.usp_InvoiceDetailsIncludeTax_Insert(this.Invoice_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["QuantityRecived"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["Capacity"].ToString(), r["ItemDescription"].ToString(), r["IDCodeOperation"].ToString(), r["Policy"].ToString(), r["InvoiceDate"].ToDate(), r["Capacities"].ToString(), r["IsGift"].ToBooleanOrDefault(), r["UnitCostEvaluate"].ToDecimalOrDefault(),
                                false,null,null);

                            //Detail_ID = dc.usp_ReceiptDetailsWithDesciption_Insert(this.Receipt_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["QuantityRecived"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["Capacity"].ToString(), r["ItemDescription"].ToString(), r["IDCodeOperation"].ToString(), r["Policy"].ToString(), r["ReceiptDate"].ToDate(), r["Capacities"].ToString());
                        }
                        if (r.RowState == DataRowState.Modified)
                        {
                            Detail_ID = r["ID"].ToInt();
                            dc.usp_InvoiceDetailsIncludeTax_Update(r["ID"].ToInt(), r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["IsGift"].ToBooleanOrDefault(), r["UnitCostEvaluate"].ToDecimalOrDefault(),
                                false,null,null);
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
        UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess + Serial, PageLinks.InvoiceOutput + Request.PathInfo + "?ID=" + this.Invoice_ID.ToExpressString(), PageLinks.InvoicesOutputList, PageLinks.InvoiceOutput + Request.PathInfo);
        return true;
    }

    private void InsertOperation()
    {
        if (this.CalculatedSalesCost > 0)
        {
            dc.usp_SalesCostOperation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), this.CalculatedSalesCost, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
        }
    }



    private bool InsertICJ(int Detail_ID, DataRow row)
    {
        decimal? SalesCost = 0;


        decimal? Cost = 0;



        var QtyWorningFlag = dc.usp_Company_Select().FirstOrDefault().QuantityWarning;
        if (QtyWorningFlag == ICJStoredProcFlags.QtyReserved.ToInt())
        {
            var funItemQty = dc.fun_ItemQty(row["Item_ID"].ToInt(), row["Store_ID"].ToInt(), null, row["Uom_ID"].ToInt(), this.SalesOrderID);
            if (funItemQty < row["Quantity"].ToDecimal())
            {
                UserMessages.Message(null, Resources.UserInfoMessages.QtyReserved + " (" + row["StoreName"] + " : " + row["ItemName"] + " : " + row["BatchName"] + ")", string.Empty);
                this.ConfirmationMessage = string.Empty;
                return false;
            }


        }

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


    private Tuple<bool, string> InsertOutICJ(int Detail_ID, int id, usp_ProductionOrderDetailsForFinalItem_SelectResult row, DateTime txtOperationDate, int acBranch)
    {
        decimal? Cost = 0;
        int result = 0;

        result = dc.usp_ICJ_ProductionOrder_Out(txtOperationDate.ToDate(), row.Quantity.ToDecimal(), row.Item_ID.ToInt(), null, 0, null, row.Store_ID.ToInt(), acBranch.ToNullableInt(), DocumentsTableTypes.Invoice.ToInt(), id, Detail_ID, ref Cost);


        if (result == -32)
        {
            //UserMessages.Message(null, Resources.UserInfoMessages.QtyReserved + " (" + row["StoreName"] + " : " + row["ItemName"].ToExpressString() + ")", string.Empty);
            return new Tuple<bool, string>(false, Resources.UserInfoMessages.QtyReserved + " (" + row.StoreName + " : " + row.ItemName.ToExpressString() + ")");
        }
        if (result == -4)
        {
            // UserMessages.Message(null, Resources.UserInfoMessages.QtyNotEnough + " (" + row["StoreName"] + " : " + row["ItemName"] + ")", string.Empty);
            // return false;
            return new Tuple<bool, string>(false, Resources.UserInfoMessages.QtyNotEnough + " (" + row.StoreName + " : " + row.ItemName.ToExpressString() + ")");

        }
        return new Tuple<bool, string>(true, "Ok");
    }





    private bool InsertICJNotCosumMaterial(int Detail_ID, DataRow row)
    {

        var QtyWorningFlag = dc.usp_Company_Select().FirstOrDefault().QuantityWarning;
        if (QtyWorningFlag == ICJStoredProcFlags.QtyNotEnoughAndAsk.ToInt())
        {
            var funItemQty = dc.fun_ItemQty(row["Item_ID"].ToInt(), row["Store_ID"].ToInt(), null, row["Uom_ID"].ToInt(), this.SalesOrderID);
            if (funItemQty < row["Quantity"].ToDecimal())
            {
                UserMessages.Message(null, Resources.UserInfoMessages.QtyNotEnough + " (" + row["StoreName"] + " : " + row["ItemName"] + " : " + row["BatchName"] + ")", string.Empty);
                this.ConfirmationMessage = string.Empty;
                return false;
            }


        }
        if (QtyWorningFlag == ICJStoredProcFlags.QtyNotEnoughAndStop.ToInt())
        {
            var funItemQty = dc.fun_ItemQty(row["Item_ID"].ToInt(), row["Store_ID"].ToInt(), null, row["Uom_ID"].ToInt(), this.SalesOrderID);
            if (funItemQty < row["Quantity"].ToDecimal())
            {
                UserMessages.Message(null, Resources.UserInfoMessages.QtyNotEnough + " (" + row["StoreName"] + " : " + row["ItemName"] + " : " + row["BatchName"] + ")", string.Empty);
                this.ConfirmationMessage = string.Empty;
                return false;
            }


        }





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


        if (this.InvoiceInit_ID == 0 && this.SalesOrderID == 0) return;
        var invoice = dc.usp_Invoice_SelectByID(this.InvoiceInit_ID).FirstOrDefault();
        ddlCurrency.SelectedValue = invoice.Currency_ID.ToExpressString();
        txtRatio.Text = invoice.Ratio.ToExpressString();
        acBranch.Value = invoice.Branch_ID.ToStringOrEmpty();
        this.FilterByBranchAndCurrency();
        txtOperationDate.Text = invoice.OperationDate.Value.ToExpressString();
        acCostCenter.Value = invoice.CostCenter_ID.ToStringOrEmpty();
        txtUserRefNo.Text = invoice.UserRefNo;
        if (invoice.DateDelivery != null)
            txtDeliveryDate.Text = invoice.DateDelivery.Value.ToExpressString();
        // this.DocStatus_ID = invoice.DocStatus_ID.ToByteOrDefault();
        acCustomer.Value = invoice.Contact_ID.ToExpressString();
        this.acCustomer_SelectedIndexChanged(null, null);
        acTelephone.Value = invoice.Telephone_ID.ToStringOrEmpty();
        acAddress.Value = invoice.DefaultAddress_ID.ToStringOrEmpty();
        acShipAddress.Value = invoice.ShipToAddress_ID.ToStringOrEmpty();
        acPaymentAddress.Value = invoice.PaymentAddress_ID.ToStringOrEmpty();
        // acCashAccount.Value = invoice.CashAccount_ID.ToStringOrEmpty();
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
        acdrivers.Value = invoice.Driver_ID.ToStringOrEmpty();
        if (invoice.SalesOrder_ID.HasValue) this.SalesOrderID = invoice.SalesOrder_ID.Value;

        if (!string.IsNullOrEmpty(invoice.SalesOrderSerial) || this.SalesOrderID != 0)
        {
            lblSalesOrderNo.Text = EditMode ? invoice.SalesOrderSerial : invoice.Serial;
            divSalesOrderNo.Visible = true;
            ddlCurrency.Enabled = false;
            // acBranch.Enabled = false;
            acCustomer.Enabled = false;
            acSalesRep.Enabled = false;
        }

        if (this.Invoice_ID > 0)
        {
            // txtSerial.Text = invoice.Serial;
            this.DocRandomString = invoice.DocRandomString;
            lblCreatedBy.Text = invoice.CreatedByName;
            lblApprovedBy.Text = invoice.ApprovedBYName;
            this.ImgStatus = ((DocStatus)invoice.DocStatus_ID).ToExpressString();
            btnPrint.Visible = MyContext.PageData.IsPrint;
            btnPrintInventoryOrder.Visible = MyContext.PageData.IsPrint;

            btnApprove.Visible = MyContext.PageData.IsApprove;//--&& (invoice.DocStatus_ID == 1);
            pnlAddItem.Visible = (invoice.DocStatus_ID == 1);
            btnCancel.Visible = (invoice.DocStatus_ID == 1) && MyContext.PageData.IsDelete;

            btnSave.Visible = (invoice.DocStatus_ID == 1) && ((MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode));

            //gvItems.Columns[gvItems.Columns.Count - 1].Visible = gvItems.Columns[gvItems.Columns.Count - 2].Visible = (invoice.DocStatus_ID == 1);
            // gvTaxes.Columns[gvTaxes.Columns.Count - 1].Visible = (invoice.DocStatus_ID == 1);
            // this.DocStatus_ID = invoice.DocStatus_ID.Value;
        }

        // this.dtItems = dc.usp_InvoiceDetailsIncludeTax_Select(EditMode ? this.Invoice_ID : this.SalesOrderID).CopyToDataTable();

        //this.dtItems = dc.usp_ReceiptDetailsWithDescription_Select(EditMode ? this.Receipt_ID : this.PurchaseOrderID).CopyToDataTable();
        //var lstItems = dc.usp_InvoiceDetailsIncludeTax_Select(this.SalesOrderID).ToList();
        var lstItems = dc.usp_InvoiceDetailsIncludeTax_Select(this.InvoiceInit_ID).ToList();
        this.dtItems = lstItems.CopyToDataTable();
        if (lstItems.Count() > 0)
        {
            acStore.Value = lstItems.FirstOrDefault().Store_ID.ToExpressString();
        }


        // this.dtItems = dc.usp_InvoiceDetails_Select(EditMode ? this.Invoice_ID : this.SalesOrderID).CopyToDataTable();
        this.dtTaxes = dc.usp_DocuemntTaxes_Select(DocumentsTableTypes.Invoice.ToInt(), EditMode ? this.Invoice_ID : this.SalesOrderID, false).CopyToDataTable();
        this.BindItemsGrid();
        gvTaxes.DataSource = this.dtTaxes;
        gvTaxes.DataBind();


    }

    private void FillOutputInvoice()
    {


        if (this.Invoice_ID == 0) return;
        var invoice = dc.usp_Invoice_SelectByID(this.Invoice_ID).FirstOrDefault();
        ddlCurrency.SelectedValue = invoice.Currency_ID.ToExpressString();
        txtRatio.Text = invoice.Ratio.ToExpressString();
        acBranch.Value = invoice.Branch_ID.ToStringOrEmpty();
        this.FilterByBranchAndCurrency();
        txtOperationDate.Text = invoice.OperationDate.Value.ToExpressString();
        acCostCenter.Value = invoice.CostCenter_ID.ToStringOrEmpty();
        txtUserRefNo.Text = invoice.UserRefNo;
        if (invoice.DateDelivery != null)
            txtDeliveryDate.Text = invoice.DateDelivery.Value.ToExpressString();
        // this.DocStatus_ID = invoice.DocStatus_ID.ToByteOrDefault();
        acCustomer.Value = invoice.Contact_ID.ToExpressString();
        this.acCustomer_SelectedIndexChanged(null, null);
        acTelephone.Value = invoice.Telephone_ID.ToStringOrEmpty();
        acAddress.Value = invoice.DefaultAddress_ID.ToStringOrEmpty();
        acShipAddress.Value = invoice.ShipToAddress_ID.ToStringOrEmpty();
        acPaymentAddress.Value = invoice.PaymentAddress_ID.ToStringOrEmpty();
        // acCashAccount.Value = invoice.CashAccount_ID.ToStringOrEmpty();
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
        acdrivers.Value = invoice.Driver_ID.ToStringOrEmpty();
        if (invoice.SalesOrder_ID.HasValue) this.SalesOrderID = invoice.SalesOrder_ID.Value;

        if (!string.IsNullOrEmpty(invoice.SalesOrderSerial) || this.SalesOrderID != 0)
        {
            lblSalesOrderNo.Text = EditMode ? invoice.SalesOrderSerial : invoice.Serial;
            divSalesOrderNo.Visible = true;
            ddlCurrency.Enabled = false;
            // acBranch.Enabled = false;
            acCustomer.Enabled = false;
            acSalesRep.Enabled = false;
        }

        if (this.Invoice_ID > 0)
        {
             txtSerial.Text = invoice.Serial;
            this.DocRandomString = invoice.DocRandomString;
            lblCreatedBy.Text = invoice.CreatedByName;
            lblApprovedBy.Text = invoice.ApprovedBYName;
            this.ImgStatus = ((DocStatus)invoice.DocStatus_ID).ToExpressString();
            btnPrint.Visible = MyContext.PageData.IsPrint;
            btnPrintInventoryOrder.Visible = MyContext.PageData.IsPrint;

            btnApprove.Visible = MyContext.PageData.IsApprove && (invoice.DocStatus_ID == 1);
            pnlAddItem.Visible = (invoice.DocStatus_ID == 1);
            btnCancel.Visible = (invoice.DocStatus_ID == 1) && MyContext.PageData.IsDelete;

            btnSave.Visible = (invoice.DocStatus_ID == 1) && ((MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode));

            //gvItems.Columns[gvItems.Columns.Count - 1].Visible = gvItems.Columns[gvItems.Columns.Count - 2].Visible = (invoice.DocStatus_ID == 1);
            // gvTaxes.Columns[gvTaxes.Columns.Count - 1].Visible = (invoice.DocStatus_ID == 1);
            // this.DocStatus_ID = invoice.DocStatus_ID.Value;
        }

        // this.dtItems = dc.usp_InvoiceDetailsIncludeTax_Select(EditMode ? this.Invoice_ID : this.SalesOrderID).CopyToDataTable();

        //this.dtItems = dc.usp_ReceiptDetailsWithDescription_Select(EditMode ? this.Receipt_ID : this.PurchaseOrderID).CopyToDataTable();
        //var lstItems = dc.usp_InvoiceDetailsIncludeTax_Select(this.SalesOrderID).ToList();
        var lstItems = dc.usp_InvoiceDetailsIncludeTax_Select(this.Invoice_ID).ToList();
        this.dtItems = lstItems.CopyToDataTable();
        if (lstItems.Count() > 0)
        {
            acStore.Value = lstItems.FirstOrDefault().Store_ID.ToExpressString();
        }


        // this.dtItems = dc.usp_InvoiceDetails_Select(EditMode ? this.Invoice_ID : this.SalesOrderID).CopyToDataTable();
        this.dtTaxes = dc.usp_DocuemntTaxes_Select(DocumentsTableTypes.Invoice.ToInt(), EditMode ? this.Invoice_ID : this.SalesOrderID, false).CopyToDataTable();
        this.BindItemsGrid();
        gvTaxes.DataSource = this.dtTaxes;
        gvTaxes.DataBind();


    }

    private void CheckSecurity()
    {
        //if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        btnSave.Visible = (MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode);
        btnApprove.Visible = MyContext.PageData.IsApprove;
        btnCancelApprove.Visible = !btnApprove.Visible && MyContext.PageData.IsNotApprove;
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
            decimal taxInclude = (TypeTax == 2 ? Decimal.Divide(r["TaxPercentageValue"].ToDecimalOrDefault(), (100 + r["TaxPercentageValue"].ToDecimalOrDefault())) : 0);
            var uc = r["UnitCostEvaluate"].ToDecimalOrDefault();
            var taxUc = r["UnitCostEvaluate"].ToDecimalOrDefault() * taxInclude;
            r["UnitCost"] = uc - taxUc;


            r["Total"] = r["UnitCost"].ToDecimalOrDefault() * r["Quantity"].ToDecimal();
            Total += r["UnitCost"].ToDecimalOrDefault() * r["Quantity"].ToDecimal();
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
        lblTotal.Text = this.Total.ToString("0.########");
        lblTotalDiscount.Text = this.TotalDiscount.ToString("0.########");
        lblAdditionals.Text = Additionals.ToString("0.########");
        lblTotalTax.Text = this.TotalTax.ToString("0.########");
        lblGrossTotal.Text = this.GrossTotal.ToString("0.########");
        if ((acCashAccount.HasValue && Page.IsPostBack && this.SumFirstPaid) || this.IsCashInvoice)
        {
            txtFirstPaid.Text = lblGrossTotal.Text;
            this.txtFirstPaid_TextChanged(null, null);
        }
        this.ConfirmationAnswered = false;
        this.ConfirmationMessage = string.Empty;
        this.ShowCustomerBalance();

    }




    //private void Calculate()
    //{
    //    decimal ItemDiscount = 0;
    //    decimal DocDiscount = 0;
    //    decimal DocTax = 0;
    //    decimal Additionals = 0;
    //    decimal DocTaxValue = 0;
    //    this.Total = 0;
    //    this.GrossTotal = 0;
    //    this.TotalDiscount = 0;
    //    this.TotalTax = 0;
    //    this.TotalDebitTax = 0;
    //    this.TotalCreditTax = 0;
    //    this.dtAllTaxes.Rows.Clear();
    //    this.dtAllTaxes.AcceptChanges();
    //    foreach (DataRow r in this.dtItems.Rows)
    //    {
    //        ItemDiscount = 0;
    //        if (r.RowState == DataRowState.Deleted) continue;
    //        r["Total"] = r["UnitCost"].ToDecimal() * r["Quantity"].ToDecimal();
    //        Total += r["UnitCost"].ToDecimal() * r["Quantity"].ToDecimal();
    //        this.TotalDiscount += ItemDiscount = (r["PercentageDiscount"].ToDecimal() * r["Total"].ToDecimal() * 0.01m) + r["CashDiscount"].ToDecimal();
    //        if (r["TaxOnInvoiceType"] != DBNull.Value && r["Tax_ID"] != DBNull.Value)
    //        {
    //            r["TotalTax"] = ((r["Total"].ToDecimal() - ItemDiscount) * r["TaxPercentageValue"].ToDecimal() * 0.01m);

    //            if (r["TaxOnInvoiceType"].ToExpressString() == "C")
    //            {
    //                this.dtAllTaxes.Rows.Add(r["TaxSalesAccountID"].ToInt(), r["TaxOnInvoiceType"].ToExpressString(), 0, r["TotalTax"].ToDecimal());
    //                this.TotalCreditTax += r["TotalTax"].ToDecimal();
    //            }
    //            else
    //            {
    //                this.dtAllTaxes.Rows.Add(r["TaxSalesAccountID"].ToInt(), r["TaxOnInvoiceType"].ToExpressString(), r["TotalTax"].ToDecimal(), 0);
    //                this.TotalDebitTax += r["TotalTax"].ToDecimal();
    //                r["TotalTax"] = r["TotalTax"].ToDecimal() * -1;
    //            }
    //            this.TotalTax += r["TotalTax"].ToDecimal();
    //        }
    //        r["GrossTotal"] = r["Total"].ToDecimal() - ItemDiscount + r["TotalTax"].ToDecimal();
    //        this.GrossTotal += r["Total"].ToDecimal() - ItemDiscount + r["TotalTax"].ToDecimal();
    //    }

    //    DocDiscount = (this.GrossTotal * txtPercentageDiscount.Text.ToDecimalOrDefault() * 0.01m) + txtCashDiscount.Text.ToDecimalOrDefault();
    //    foreach (DataRow r in this.dtTaxes.Rows)
    //    {
    //        if (r.RowState == DataRowState.Deleted) continue;
    //        if (r["OnInvoiceType"] != DBNull.Value)
    //        {
    //            DocTaxValue = ((this.GrossTotal - DocDiscount) * r["PercentageValue"].ToDecimal() * 0.01m);
    //            if (r["OnInvoiceType"].ToExpressString() == "C")
    //            {
    //                this.dtAllTaxes.Rows.Add(r["SalesAccountID"].ToInt(), r["OnInvoiceType"].ToExpressString(), 0, DocTaxValue);
    //                this.TotalCreditTax += DocTaxValue;
    //            }
    //            else
    //            {
    //                this.dtAllTaxes.Rows.Add(r["SalesAccountID"].ToInt(), r["OnInvoiceType"].ToExpressString(), DocTaxValue, 0);
    //                this.TotalDebitTax += DocTaxValue;
    //                DocTaxValue *= -1m;
    //            }
    //            DocTax += DocTaxValue;
    //        }
    //    }
    //    Additionals = txtAdditionals.Text.ToDecimalOrDefault();
    //    this.TotalDiscount += DocDiscount;
    //    this.TotalTax += DocTax;
    //    this.GrossTotal = this.GrossTotal - DocDiscount + DocTax + Additionals;
    //    lblTotal.Text = this.Total.ToString("0.########");
    //    lblTotalDiscount.Text = this.TotalDiscount.ToString("0.########");
    //    lblAdditionals.Text = Additionals.ToString("0.########");
    //    lblTotalTax.Text = this.TotalTax.ToString("0.########");
    //    lblGrossTotal.Text = this.GrossTotal.ToString("0.########");
    //    if ((acCashAccount.HasValue && Page.IsPostBack && this.SumFirstPaid) || this.IsCashInvoice)
    //    {
    //        txtFirstPaid.Text = lblGrossTotal.Text;
    //        this.txtFirstPaid_TextChanged(null, null);
    //    }
    //    this.ConfirmationAnswered = false;
    //    this.ConfirmationMessage = string.Empty;
    //    this.ShowCustomerBalance();

    //}

    private void ShowAvailableQty()
    {
        lblAvailableQty.Text = string.Empty;


        if (acItemDescribed.HasValue)
        {
            var itemQtyFromInput = acItem.Value.ToInt();

            var funItemQty = dc.fun_ItemDescribedQty(itemQtyFromInput, acItemDescribed.Value, null, null, null, null);

            if (funItemQty != null)
            {
                decimal qty = funItemQty.Value;
                lblAvailableQty.Text = qty.ToExpressString();
                lblAvailableQty.ForeColor = qty <= 0 ? System.Drawing.Color.Red : System.Drawing.Color.Green;
            }
        }
        else if (acItem.HasValue)
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

        Label1.Text = (acCustomer.HasValue && acItem.HasValue) ? dc.fun_CustomerLastItemPrice(acCustomer.Value.ToNullableInt(), acItem.Value.ToNullableInt()).ToDecimalOrDefault().ToString("0.##") : "0.00";
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

    private void CalculateTotalRow()
    {

        var Tax = dc.usp_Taxes_Select(acItemTax.Value.ToInt(), string.Empty).FirstOrDefault();


        decimal taxInclude = TypeTax == 2 ? (Decimal.Divide((Tax != null ? Tax.PercentageValue.Value : 0), (100M + (Tax != null ? Tax.PercentageValue.Value : 0)))) : Decimal.Divide((Tax != null ? Tax.PercentageValue.Value : 0), (100M));
        decimal unitCost = TypeTax == 2 ? (txtCost.Text.ToDecimalOrDefault() - txtCost.Text.ToDecimalOrDefault() * taxInclude) : txtCost.Text.ToDecimalOrDefault();

        var totalPrice = txtQty.Text.ToDecimalOrDefault() * unitCost;
        var calc = (totalPrice - txtItemCashDiscount.Text.ToDecimalOrDefault() - txtItemPercentageDiscount.Text.ToDecimalOrDefault() * (totalPrice) / 100);

        decimal val1 = calc * (Tax != null ? Tax.PercentageValue.Value : 0) / 100;
        lblTotalRow.Text = (string.IsNullOrEmpty(acItemTax.Value) ? calc : (calc + val1)).ToExpressString();
        //lblTotalRowBeforTax.Text = calc.ToExpressString();

        //var Tax = dc.usp_Taxes_Select(acItemTax.Value.ToInt(), string.Empty).FirstOrDefault();


        //decimal taxInclude = Decimal.Divide((Tax != null ? Tax.PercentageValue.Value : 0), (100M + (Tax != null ? Tax.PercentageValue.Value : 0)));
        //decimal unitCost = txtCost.Text.ToDecimalOrDefault() - txtCost.Text.ToDecimalOrDefault() * taxInclude;

        //var totalPrice = txtQty.Text.ToDecimalOrDefault() * unitCost;
        //var calc = (totalPrice - txtItemCashDiscount.Text.ToDecimalOrDefault() - txtItemPercentageDiscount.Text.ToDecimalOrDefault() * (totalPrice) / 100);
        //lblTotalRow.Text = (string.IsNullOrEmpty(acItemTax.Value) ? calc : (calc + (calc * (Tax != null ? Tax.PercentageValue.Value : 0) / 100))).ToExpressString();





        //if (Tax == null)
        //{
        //    decimal taxInclude = Decimal.Divide((Tax != null ? Tax.PercentageValue.Value : 0), (100M + Tax.PercentageValue.Value));
        //    decimal unitCost = txtCost.Text.ToDecimalOrDefault() - txtCost.Text.ToDecimalOrDefault() * taxInclude;

        //    var totalPrice = txtQty.Text.ToDecimalOrDefault() * unitCost;
        //    var calc = (totalPrice - txtItemCashDiscount.Text.ToDecimalOrDefault() - txtItemPercentageDiscount.Text.ToDecimalOrDefault() * (totalPrice) / 100);
        //    lblTotalRow.Text = (txtQty.Text.ToDecimalOrDefault() * unitCost).ToExpressString();
        //}

    }




    #endregion

    protected void btnViewQty_Click(object sender, EventArgs e)
    {

    }
    protected void btnViewQtyClose_Click(object sender, EventArgs e)
    {

    }
    protected void lnkViewQty_Click(object sender, EventArgs e)
    {

        gvViewQty.DataSource = dc.usp_GetQtyBItemInStore(acItem.Value.ToIntOrDefault()).CopyToDataTable();
        gvViewQty.DataBind();

        mpeViewQty.Show();
    }
    protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AddInflamation")
        {
            var spl = e.CommandArgument.ToString().Split(',');

            this.InflaID = Convert.ToInt32(e.CommandArgument);

            mpeInflamation.Show();
        }
    }
    protected void btnInflamationAddNewAddNew_Click(object sender, EventArgs e)
    {
        DataRow r = this.dtItems.Select("ID=" + this.InflaID)[0];

        //var qty = dc.InvoiceInflammations.Where(x => x.Invoice_ID == this.Invoice_ID && x.InvoiceDetail_ID == this.InflaID).Sum(y => y.Quantity).ToDecimalOrDefault();
        //if (qty + txtQuatityInflamation.Text.ToDecimal() > r["Quantity"].ToDecimal())
        //{
        //    UserMessages.Message(null, "المستلمة اكبر من الكمية الحقيقية", string.Empty);
        //    return;
        //}
        r["QuantityRecived"] = txtQuatityInflamation.Text;
        gvItems.DataSource = dtItems;
        gvItems.DataBind();

    }
    protected void BtnCancelInflamationAddNew_Click(object sender, EventArgs e)
    {

    }
}
public static class ResponseHelper
{
    public static void Redirect1(this HttpResponse response, string url, string target, string windowFeatures)
    {

        if ((String.IsNullOrEmpty(target) || target.Equals("_self", StringComparison.OrdinalIgnoreCase)) && String.IsNullOrEmpty(windowFeatures))
        {
            response.Redirect(url);
        }
        else
        {
            System.Web.UI.Page page = (System.Web.UI.Page)HttpContext.Current.Handler;

            if (page == null)
            {
                throw new InvalidOperationException("Cannot redirect to new window outside Page context.");
            }
            url = page.ResolveClientUrl(url);

            string script;
            if (!String.IsNullOrEmpty(windowFeatures))
            {
                script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            }
            else
            {
                script = @"window.open(""{0}"", ""{1}"");";
            }
            script = String.Format(script, url, target, windowFeatures);
            ScriptManager.RegisterStartupScript(page, typeof(System.Web.UI.Page), "Redirect", script, true);
        }
    }
}