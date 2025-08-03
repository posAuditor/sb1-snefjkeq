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
using System.IO;
using OfficeOpenXml;
using System.Threading;

public partial class Sales_InvoiceForm : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties
    private DataTable dtPaymentDetailsGroup
    {
        get
        {
            if (Session["dtPaymentDetailsGroup" + this.WinID] == null)
            {
                Session["dtPaymentDetailsGroup" + this.WinID] = dc.usp_InvoicePayment(null, 0).CopyToDataTable();
            }
            return (DataTable)Session["dtPaymentDetailsGroup" + this.WinID];
        }

        set
        {
            Session["dtPaymentDetailsGroup" + this.WinID] = value;
        }
    }
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

    private int Qauta_ID
    {
        get
        {
            if (ViewState["Qauta_ID"] == null) return 0;
            return (int)ViewState["Qauta_ID"];
        }

        set
        {
            ViewState["Qauta_ID"] = value;
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

    private decimal TotalGift
    {
        get
        {
            if (ViewState["TotalGift"] == null) return 0;
            return (decimal)ViewState["TotalGift"];
        }

        set
        {
            ViewState["TotalGift"] = value;
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

    public bool IsViewSerialNumber
    {
        get
        {
            return MyContext.Features.IsViewSerial;
        }


    }

    private DataTable dtAllPayment
    {
        get
        {
            if (Session["dtAllPayment_Invoice" + this.WinID] == null)
            {
                Session["dtAllPayment_Invoice" + this.WinID] = dc.usp_OperationGPaymentMethode_Select(null, null).CopyToDataTable();
            }
            return (DataTable)Session["dtAllPayment_Invoice" + this.WinID];
        }

        set
        {
            Session["dtAllPayment_Invoice" + this.WinID] = value;
        }
    }




    #endregion

    #region PageEvents
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnUpload);
            this.SetEditMode();
            this.gvItems.FormatNumber = MyContext.FormatNumber;
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

                var comp = dc.usp_Company_Select().FirstOrDefault();
                txtCost.Enabled = !comp.IsPriceLoocked.Value;

                txtCost.Enabled = MyContext.PageData.IsPrice;

                if (Request.QueryString["DateDelivery"] != null)
                {
                    var date = (Request.QueryString["DateDelivery"]).Split('-');
                    txtDeliveryDate.Text = (new DateTime(date[0].ToIntOrDefault(), date[1].ToIntOrDefault(), date[2].ToIntOrDefault())).ToString("d/M/yyyy");
                }


            }
            Items_CategoriesData masterpage = (Items_CategoriesData)(Page.Master);
            ucNav.Res_ID = this.Invoice_ID;
            ucNav.SourceDocTypeType_ID = DocumentsTableTypes.Invoice.ToInt();
            ucNav.EntryType = this.IsCashInvoice ? (byte)2 : (byte)1;
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
                        case 6: chkCado.Focus(); //txtItemCashDiscount.Focus();
                            break;
                        case 5: chkCado.Focus();// txtItemPercentageDiscount.Focus();
                            break;
                        case 7: chkCado.Focus();
                            break;
                        case 8: LinkButton1.Focus();
                            break;
                        default:
                            txtBarcode.Focus();
                            break;
                    }


                    //var ctrl = from control in wcICausedPostBack.Parent.Controls.OfType<WebControl>()
                    //           where control.TabIndex > indx
                    //           select control;
                    //ctrl.DefaultIfEmpty(wcICausedPostBack).First().Focus();
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


    protected void btnYesCollection_Click(object sender, EventArgs e)
    {
        try
        {
            this.ConfirmationAnswered = true;
            this.btnApproveAccounting_Click(null, null);

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void acCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            acCustomerMesure.ContextKey = acCustomer.Value + "," + "111";
            acShipAddress.ContextKey = acCustomer.Value + "," + ContactDetailsTypes.ShipAddress.ToInt().ToExpressString();
            acAddress.ContextKey = acCustomer.Value + "," + ContactDetailsTypes.DefaultAddress.ToInt().ToExpressString();
            acPaymentAddress.ContextKey = acCustomer.Value + "," + ContactDetailsTypes.PaymentAddress.ToInt().ToExpressString();
            acTelephone.ContextKey = acCustomer.Value + "," + ContactDetailsTypes.DefaultTelephone.ToInt().ToExpressString();
            if (!MyContext.UserProfile.SalesRepToCustomer.ToBooleanOrDefault())
            {
                acSalesRep.ContextKey = acCustomer.Value + ",," + acBranch.Value;
            }

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
            //  if (sender != null) this.FocusNextControl(sender);
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
            // this.FillQtyStoreGroupList();
            //  this.FillItemeList_price();
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


                if (sender != null) txtCost.Text = (dc.fun_GetItemDefaultPriceByUOM(acItem.Value.ToNullableInt(), acUnit.Value.ToNullableInt(), acPriceName.Value.ToNullableInt()) / (txtRatio.Text.ToNullableDecimal() ?? 1)).Value.ToString(NbrHashNeerDecimal);
                if (sender != null) this.ShowAvailableQty();
                // if (sender != null) this.FocusNextControl(sender);
                txtQty.Focus();
            }
            else
            {
                if (sender != null) txtCost.Text = (dc.fun_GetItemPriceDefaultPriceByUOM(acItem.Value.ToNullableInt(), acUnit.Value.ToNullableInt(), acPriceName.Value.ToNullableInt()) / (txtRatio.Text.ToNullableDecimal() ?? 1)).Value.ToString(NbrHashNeerDecimal);
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
            if (sender != null) txtCost.Text = (dc.fun_GetItemPriceDefaultPriceByUOM(acItem.Value.ToNullableInt(), acUnit.Value.ToNullableInt(), acPriceName.Value.ToNullableInt()) / (txtRatio.Text.ToNullableDecimal() ?? 1)).Value.ToString(NbrHashNeerDecimal);
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
            r["SerialNumber"] = txtSerialNumber.Text;
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
            r["PercentageDiscount"] = txtItemPercentageDiscount.Text.ToDecimalOrDefault();
            r["CashDiscount"] = txtItemCashDiscount.Text.ToDecimalOrDefault();
            r["TotalTax"] = 0;
            r["Notes"] = txtItemNotes.Text;
            r["StoreName"] = acStore.Text;
            r["Barcode"] = txtBarcode.Text;
            r["CategoryName"] = acCategory.Text;
            var Tax = dc.usp_Taxes_Select(acItemTax.Value.ToInt(), string.Empty).FirstOrDefault();

            //  r["TotalCostBeforTax"] = txtCost.Text.ToDecimalOrDefault() * txtQty.Text.ToDecimalOrDefault();
            decimal taxInclude = TypeTax == 2 ? (Decimal.Divide((Tax != null ? Tax.PercentageValue.Value : 0), (100M + (Tax != null ? Tax.PercentageValue.Value : 0)))) : Decimal.Divide((Tax != null ? Tax.PercentageValue.Value : 0), (100M));
            decimal unitCost = TypeTax == 2 ? (txtCost.Text.ToDecimalOrDefault() - txtCost.Text.ToDecimalOrDefault() * taxInclude) : txtCost.Text.ToDecimalOrDefault();
            r["TotalCostBeforTax"] = Math.Round(unitCost * txtQty.Text.ToDecimalOrDefault(), 3).ToDecimalOrDefault();
            var Acc = dc.usp_ItemsAccount_Select(acItem.Value.ToIntOrDefault()).FirstOrDefault();
            if (Acc != null)
            {
                r["Account_ID"] = (Acc.Account_ID.ToIntOrDefault());
            }
            else
            {
                r["Account_ID"] = null;
            }


            //need Options
            r["InvoiceDate"] = txtInvoiceDate.Text.ToDateOrDBNULL();
            //رقم التشغيلة
            r["ProductionDate"] = txtProductionDate.Text.ToDateOrDBNULL();
            r["ExpirationDate"] = txtExpirationDate.Text.ToDateOrDBNULL();
            if (acItemTax.HasValue)
            {

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

            if (this.dtItems.Rows.Count > 0)
            {
                var objs = this.dtItems.Select().FirstOrDefault();
                if (objs != null)
                {
                    acStore.Value = objs["Store_ID"].ToExpressString();
                    acStore.Enabled = false;
                }

            }

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
            // txtCost.Text = r["UnitCost"].ToExpressString();
            txtCost.Text = r["UnitCostEvaluate"].ToExpressString();

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


            if (!string.IsNullOrEmpty(r["SerialNumber"].ToExpressString()))
            {
                var sn = r["SerialNumber"].ToExpressString().Split(';');
                var snWithoutCommaPoint = string.Join("\n", sn); ;
                txtSerialNumber.Text = snWithoutCommaPoint;

            }

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
            if (acBranch.Value != null)
            {
                var getStores = dc.usp_StoresAutoCompelete_Select(int.Parse(acBranch.Value)).ToList();
                if (getStores.Count == 1)
                    acStore.Value = getStores.First().ID.ToString();

                acItem.ContextKey = ",,,true,1";

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
            {


                trans.Commit();
                dc.SubmitChanges();
            }
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
            //decimal valueAmountPay = 0;
            //if (this.Invoice_ID > 0)
            //{
            //    this.dtAllPayment = dc.usp_OperationGPaymentMethode_Select(this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt()).CopyToDataTable();
            //}

            //foreach (GridViewRow gvRow in gvPay.Rows)
            //{
            //    var amt = ((TextBox)gvRow.FindControl("txtAmountPay")).Text.ToDecimalOrDefault(); ;
            //    valueAmountPay += amt;
            //    var id = gvPay.DataKeys[gvRow.RowIndex]["ID"].ToInt();
            //    var Posted_ID = gvPay.DataKeys[gvRow.RowIndex]["Posted_ID"].ToIntOrDefault();
            //    DataRow r = null;
            //    if (Posted_ID == 0)
            //    {
            //        r = this.dtAllPayment.NewRow();
            //        r["ID"] = 0;
            //        r["Price"] = amt;
            //        r["GPaymentMethode_ID"] = id;
            //    }
            //    else
            //    {

            //        r = this.dtAllPayment.Select("ID=" + Posted_ID)[0];
            //        r["Price"] = amt;
            //        r["GPaymentMethode_ID"] = id;
            //    }

            //    if (Posted_ID == 0) this.dtAllPayment.Rows.Add(r);
            //}

            //if (valueAmountPay > 0 && valueAmountPay > this.lblGrossTotal.Text.ToDecimalOrDefault())
            //{
            //    UserMessages.Message(null, "المبلغ المدفوع اكبر من مبلغ الفاتورة", string.Empty);
            //    mpeInvoiceDistributePay.Show();
            //}



            if (this.Save(true, trans))
            {
                trans.Commit();

            }
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
            // acCashAccount.IsRequired = (txtFirstPaid.Text.ToDecimalOrDefault() != 0);
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
            this.BindItemsGrid();
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

            int Contact_ID = dc.usp_ContactRax_Insert(MyContext.UserProfile.Branch_ID, ddlFastAddCurrency.SelectedValue.ToInt(), DocSerials.Customer.ToInt(), txtFastAddName.TrimmedText, 'C', string.Empty, null, txtTaxNumber.TrimmedText,
                null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
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
            txtTaxNumber.Clear();
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



            ReportDocument doc = new ReportDocument();
            doc.Load(Server.MapPath("~\\Reports\\Invoice_Print ssb.rpt"));
            doc.SetParameterValue("@Invoice_ID", this.Invoice_ID);
            Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "Invoice"), false);


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
            var ObJUserGrouo = dc.aspnet_UsersInRoles.Where(x => x.UserId == MyContext.UserProfile.UserId).FirstOrDefault();




            decimal Count_PrtInvoiceId = 0;
            Count_PrtInvoiceId = dc.fun_countPrintCount(ObJUserGrouo.RoleId, MyContext.UserProfile.Contact_ID, 1, this.Invoice_ID).Value;
            if (Count_PrtInvoiceId == 0) return;

            dc.usp_PrintCount_Insert(1, this.Invoice_ID, ObJUserGrouo.RoleId, MyContext.UserProfile.Contact_ID);






            if (DropDownList1.SelectedValue == "1")
            {
                // Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "Invoice"), false);
                Response.Redirect("~/Report_Dev/PrintInnoiceeDev.aspx?Invoice_ID=" + this.Invoice_ID + "&IsMaterla=1", false);
            }
            if (DropDownList1.SelectedValue == "2")
            {
                Response.Redirect("~/Report_Dev/PrintInnoiceeDev.aspx?Invoice_ID=" + this.Invoice_ID + "&IsMaterla=2");
            }
            if (DropDownList1.SelectedValue == "3")
            {
                Response.Redirect("~/Report_Dev/PrintInnoiceeDev.aspx?Invoice_ID=" + this.Invoice_ID + "&IsMaterla=3");
            }

        }
        catch (Exception)
        {

            throw;
        }

    }
    protected void btnPrint_Design_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Report_Dev/PrintBarcodeDesign.aspx?Invoice_ID=" + this.Invoice_ID);
        //Response.Redirect("~/Report_Dev/PrintInnoiceeDev.aspx?Invoice_ID=" + this.Invoice_ID);
        //Response.Redirect("~/Report_Dev/PrintInnoiceeDev.aspx?Invoice_ID=" + this.Invoice_ID + "&IsMaterla=1");
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
        try
        {
            dc.usp_CancelInvoice_Approvel(this.Invoice_ID);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.InvoiceShortcut + "?ID=" + this.Invoice_ID.ToExpressString(), PageLinks.InvoicesList, PageLinks.InvoiceShortcut);
            LogAction(Actions.NotApprove, txtSerial.Text, dc);
        }
        catch (Exception ex)
        {
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
    protected void btnFastAddNewItemeCard_Click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        int result = 0;
        try
        {

            result = this.Item_ID = dc.usp_Items_insert(ID = txtNameItemeCard.TrimmedText,
                    acCategoryItemeCard.Value.ToInt(),
                    ddlItemTypeItemeCard.SelectedValue.ToCharArray()[0],
                    txtCostItemeCard.Text.ToDecimal(),
                    acSmallestUnit.Value.ToInt(),
                    acTaxItemeCard.Value.ToNullableInt(),
                    txtMaxQtyItemeCard.Text.ToDecimalOrDefault(),
                    txtMinQtyItemeCard.Text.ToDecimalOrDefault(),
                   null,
                    "",
                    txtBarcodeItemeCard.TrimmedText,
                    txtDefaultPriceItemeCard.Text.ToDecimalOrDefault(),
                    txtPercentageDiscountItemeCard.Text.ToDecimalOrDefault(),
                    txtCodeItemeCard.TrimmedText,
                    txtNameItemeCard.TrimmedText, 0, 0,
                    false, false, false);

            //result = this.Item_ID = dc.usp_Items_insert(ID = txtNameItemeCard.TrimmedText,
            //       acCategory.Value.ToInt(),
            //       ddlItemTypeItemeCard.SelectedValue.ToCharArray()[0],
            //       txtCost.Text.ToDecimal(),
            //       acSmallestUnit.Value.ToInt(),
            //       acTax.Value.ToNullableInt(),
            //       txtMaxQtyItemeCard.Text.ToDecimalOrDefault(),
            //       txtMinQtyItemeCard.Text.ToDecimalOrDefault(),
            //       this.ImageUrl,
            //       txtNotes.Text,
            //       txtBarcode.TrimmedText,
            //       txtDefaultPriceItemeCard.Text.ToDecimalOrDefault(),
            //       txtPercentageDiscount.Text.ToDecimalOrDefault(),
            //       txtCodeItem.TrimmedText,
            //       txtNameItemeCard.TrimmedText);




            if (result == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                trans.Rollback();
                return;
            }
            if (result == -3)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.BarcodeExists, string.Empty);
                trans.Rollback();
                return;
            }
            if (result == -4)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.CodeItemAlreadyExist, string.Empty);
                trans.Rollback();
                return;
            }




            if (txtDefaultPriceItemeCard.Text.ToDecimalOrDefault() < txtCostItemeCard.Text.ToDecimalOrDefault())
            {
                UserMessages.Message(null, Resources.UserInfoMessages.PriceCantBeLessThanCost, string.Empty);
                trans.Rollback();
                return;
            }






            trans.Commit();
            dc.SubmitChanges();

            acItem.ContextKey = ",,,true,1";

            txtBarcodeItemeCard.Text = dc.fun_GenerateBarcode();
            txtCodeItemeCard.Text = dc.fun_GenerateBarcode();
            this.BtnCancelAddNewItemeCard_Click(null, null);

            acItem.Clear();

            var item = dc.usp_Items_Select(txtBarcode.TrimmedText, string.Empty, null, null, null, true).FirstOrDefault();
            acItem.Value = item.ID.ToExpressString();
            acItem.Clear();
            this.acItem_SelectedIndexChanged(null, null);
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void BtnCancelAddNewItemeCard_Click(object sender, EventArgs e)
    {
        txtCostItemeCard.Clear();
        txtMaxQtyItemeCard.Clear();
        txtMinQtyItemeCard.Clear();
        txtDefaultPriceItemeCard.Clear();
        txtPercentageDiscountItemeCard.Clear();
        txtNameItemeCard.Clear();


    }
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
    protected void lnkSerialNumberView_Click(object sender, EventArgs e)
    {
        //if (!this.EditMode)
        //{
        int Item_ID = (sender as LinkButton).CommandArgument.Split(':')[1].ToInt();
        DataRow[] filteredRows = this.dtItems.Select("Item_ID='" + Item_ID + "'");
        if (filteredRows.Length > 0)
        {
            lblSerialNumberView.Text = filteredRows.FirstOrDefault()["SerialNumber"].ToExpressString();
        }

        mpeSerialNumberView.Show();

    }
    protected void btnAddSerial_Click(object sender, EventArgs e)
    {

    }
    protected void Button12_Click(object sender, EventArgs e)
    {

    }
    protected void lnkSelectSerialNumber_Click(object sender, EventArgs e)
    {
        mpeSerialNumber.Show();
    }
    protected void btnBtnApproveAndPay_Click(object sender, EventArgs e)
    {
        if (dtItems.Rows.Count == 0) { UserMessages.Message(null, "أضف سطر على الاقل", string.Empty); return; }
        var listItem = new List<usp_OperationGPaymentMethode_SelectResult>();
        if (this.Invoice_ID > 0)
        {
            listItem = dc.usp_OperationGPaymentMethode_Select(this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt()).ToList();
        }

        var listGMP = dc.usp_GenerateMethodePaymentByBranch_Select(acBranch.Value.ToInt(), string.Empty).ToList();

        foreach (var item in listGMP)
        {
            var obj = listItem.Where(x => x.GPaymentMethode_ID == item.ID).FirstOrDefault();
            if (obj != null)
            {
                item.Posted_ID = obj.ID.Value;
            }


        }
        this.dtPaymentDetailsGroup = listGMP.CopyToDataTable();
        gvPay.DataSource = this.dtPaymentDetailsGroup;
        gvPay.DataBind();

        foreach (GridViewRow gvRow in gvPay.Rows)
        {
            var id = gvPay.DataKeys[gvRow.RowIndex]["ID"].ToInt();
            var obj = listItem.Where(x => x.GPaymentMethode_ID == id).FirstOrDefault();
            if (obj != null)
            {
                ((TextBox)gvRow.FindControl("txtAmountPay")).Text = obj.Price.ToExpressString();

            }
        }



        mpeInvoiceDistributePay.Show();
    }
    protected void ddlPaymentMethode_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadPaymntMethode();
    }
    protected void btnApproveAccounting_Click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {
            if (this.SaveAccounting(true, trans))
                trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void btnInfoChange_Click(object sender, EventArgs e)
    {
        var qty = txtQty.Text.ToDecimalOrDefault();
        var cost = txtCost.Text.ToDecimalOrDefault();
        var NewValue = txtInfo.Text.ToDecimalOrDefault();
        var OldValue = qty * cost;

        var res = NewValue * 15 / 115;
        txtItemCashDiscount.Text = Math.Round(res, 2).ToExpressString();
        CalculateTotalRow();

    }

    #endregion

    #region Private Methods

    private void FilterByBranchAndCurrency()
    {
        try
        {

            if (!MyContext.UserProfile.SalesRepToCustomer.ToBooleanOrDefault())
            {
                acCustomer.ContextKey = "C," + acBranch.Value + "," + ddlCurrency.SelectedValue + ",";
            }
            else

                acCustomer.ContextKey = "C," + acBranch.Value + "," + ddlCurrency.SelectedValue + "," + MyContext.UserProfile.Area_ID.ToExpressString();

            //acStore.ContextKey = string.Empty + acBranch.Value + "," + this.MyContext.UserProfile.UserId;
            acStore.ContextKey = string.Empty + acBranch.Value + "," + this.MyContext.UserProfile.UserId;



            if (MyContext.UserProfile.Branch_ID == null)
            {
                acCostCenter.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",false,";
                acCashAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",," + ddlCurrency.SelectedValue + "," + COA.CashOnHand.ToInt().ToExpressString() + ",false,false";

            }
            else
            {
                acCostCenter.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",false," + acBranch.Value;
                acCashAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + acBranch.Value + "," + ddlCurrency.SelectedValue + "," + COA.CashOnHand.ToInt().ToExpressString() + ",false,false";

            }


        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private void FilterItems()
    {
        acItem.ContextKey = ",,,true,1";
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
            txtCost.Text = (item.DefaultPrice.Value / (txtRatio.Text.ToNullableDecimal() ?? 1)).ToString(NbrHashNeerDecimal);
            txtItemPercentageDiscount.Text = item.DiscountPercentage.Value.ToExpressString();
            txtBarcode.Text = item.Barcode;
            if (item.Tax_ID.HasValue) acItemTax.Value = item.Tax_ID.Value.ToExpressString();
            acUnit.Value = item.UOM_ID.Value.ToExpressString();
            acCategory.Value = item.Category_ID.ToExpressString();
            txtCItem.Text = item.CodeItem.ToString();

            if (acCustomer.HasValue)
            {
                var ListPriceCustomer = dc.usp_ContactPrice_Select(acCustomer.Value.ToInt(), acItem.Value.ToInt()).ToList();
                if (ListPriceCustomer.Any())
                {
                    txtCost.Text = ListPriceCustomer.First().Price.ToExpressString();
                }

            }
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

        if (MyContext.UserProfile.SalesRepToCustomer.ToBooleanOrDefault())
        {
            acSalesRep.ContextKey = ",," + acBranch.Value;
        }

        ddlTvae.Visible = MyContext.IsViewTaxInDocument;
        var currency = dc.usp_Currency_Select(false).ToList();
        ddlCurrency.DataSource = currency;
        ddlCurrency.DataTextField = "Name";
        ddlCurrency.DataValueField = "ID";
        ddlCurrency.DataBind();

        ddlFastAddCurrency.DataSource = currency;
        ddlFastAddCurrency.DataTextField = "Name";
        ddlFastAddCurrency.DataValueField = "ID";
        ddlFastAddCurrency.DataBind();

        acCategoryItemeCard.ContextKey = string.Empty;
        acSmallestUnit.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.UOM.ToInt().ToExpressString();
        acTaxItemeCard.ContextKey = string.Empty;

        txtBarcodeItemeCard.Text = dc.fun_GenerateBarcode();
        txtCodeItemeCard.Text = dc.fun_GenerateBarcode();

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

        //Settings
        col_gift_Header.Visible = col_gift_Edit.Visible = gvItems.Columns[gvItems.Columns.Count - 4].Visible = MyContext.InvoiceSettings.IsViewGift;
        ViewSN.Visible = EditXSN.Visible = gvItems.Columns[gvItems.Columns.Count - 5].Visible = MyContext.Features.IsViewSerial;



        var listSos = dc.SettingPointOs.Where(c => c.IsActive.Value).ToList().Select(c => new PaymentMethodeCls { Name = c.Name, ID = c.ID }).ToList();
        listSos.Add(new PaymentMethodeCls { Name = "", ID = -10 });
        listSos.Add(new PaymentMethodeCls { Name = "اجـل", ID = -1 });
        listSos.Add(new PaymentMethodeCls { Name = "نقدي", ID = 0 });
        ddlPaymentMethode.DataSource = listSos.OrderBy(c => c.ID).ToList();
        ddlPaymentMethode.DataTextField = "Name";
        ddlPaymentMethode.DataValueField = "ID";
        ddlPaymentMethode.DataBind();

        LoadPaymntMethode();
        //  txtFirstPaid.Visible = false;
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
        // acStore.Clear();
        if (MyContext.UserProfile.Store_ID != null) acStore.Value = MyContext.UserProfile.Store_ID.ToStringOrEmpty();
        acCategory.Clear();
        txtCItem.Clear();
        txtQtyOffer.Clear();
        lblTotalRowBeforTax.Text = string.Empty;
        chkSame.Checked = false;
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

        if (ddlPaymentMethode.SelectedValue=="-10")
        {
            UserMessages.Message(null, "إختر طريقة الدفع", string.Empty);
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

        //if (txtFirstPaid.Text.ToDecimalOrDefault() > Math.Round(this.GrossTotal, 4))
        //{
        //    UserMessages.Message(null, Resources.UserInfoMessages.FirstPaidGreaterThanTotal, string.Empty);
        //    trans.Rollback();
        //    return false;
        //}

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
        var cmp = dc.usp_Company_Select().FirstOrDefault();

        decimal firstpaid = 0;

        if (ddlPaymentMethode.SelectedValue != "-1")
        {
            var paymentMethods = dc.SettingPointOs.Where(c => c.IsActive.Value).ToList();
            var IDPM = ddlPaymentMethode.SelectedValue.ToIntOrDefault();
            var ChoosedPayMethodes_ID = paymentMethods.Where(c => c.ID == IDPM).FirstOrDefault();
            if (ChoosedPayMethodes_ID != null)
            {
                // var isCash = ChoosedPayMethodes_ID.IsCash.ToBooleanOrDefault();
                // if (isCash)
                // {
                firstpaid = lblGrossTotal.Text.ToDecimalOrDefault();
                // }
            }
        }





        if (!this.EditMode)
        {



            this.Invoice_ID = dc.usp_InvoiceDateDelivery_Insert(acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimalOrDefault(), txtOperationDate.Text.ToDate(),
                                             acCustomer.Value.ToInt(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate,
                                             approvedBY_ID, null, null, acAddress.Value.ToNullableInt(), acShipAddress.Value.ToNullableInt(), acPaymentAddress.Value.ToNullableInt(),
                                             acTelephone.Value.ToNullableInt(), acCostCenter.Value.ToNullableInt(), txtNotes.Text, lblTotal.Text.ToDecimal(), txtPercentageDiscount.Text.ToDecimalOrDefault(),
                                             txtCashDiscount.Text.ToDecimalOrDefault(), txtAdditionals.Text.ToDecimalOrDefault(), lblTotalDiscount.Text.ToDecimalOrDefault(), lblTotalTax.Text.ToDecimalOrDefault(),
                                             lblGrossTotal.Text.ToDecimalOrDefault(), firstpaid, null, txtUserRefNo.Text, this.DocRandomString, EntryType, SalesOrderID, acSalesRep.Value.ToNullableInt(),
                                             acCashAccount.Value.ToNullableInt(), acCustomerMesure.Value.ToNullableInt(), txtDeliveryDate.Text.ToDate(), acdrivers.Value.ToNullableInt(),
                                             ddlTvae.SelectedValue.ToNullableInt(), 0, txtCustomerName.Text,string.Empty, txtCustomerMobile.Text, txtProjectRef.Text, txtContactPerson.Text, string.Empty, 
                                             txtCustomerRepresentative.Text, ddlPaymentMethode.SelectedValue.ToIntOrDefault(),string.Empty);

            //this.Invoice_ID = dc.usp_InvoiceDateDelivery_Insert(acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimalOrDefault(), txtOperationDate.Text.ToDate(),
            //                                         acCustomer.Value.ToInt(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate,
            //                                         approvedBY_ID, null, null, acAddress.Value.ToNullableInt(), acShipAddress.Value.ToNullableInt(), acPaymentAddress.Value.ToNullableInt(),
            //                                         acTelephone.Value.ToNullableInt(), acCostCenter.Value.ToNullableInt(), txtNotes.Text, lblTotal.Text.ToDecimal(), txtPercentageDiscount.Text.ToDecimalOrDefault(),
            //                                         txtCashDiscount.Text.ToDecimalOrDefault(), txtAdditionals.Text.ToDecimalOrDefault(), lblTotalDiscount.Text.ToDecimalOrDefault(), lblTotalTax.Text.ToDecimalOrDefault(),
            //                                         lblGrossTotal.Text.ToDecimalOrDefault(), firstpaid, null, 
            //                                         txtUserRefNo.Text, this.DocRandomString, EntryType, SalesOrderID, acSalesRep.Value.ToNullableInt(),
            //                                         acCashAccount.Value.ToNullableInt(), acCustomerMesure.Value.ToNullableInt(), txtDeliveryDate.Text.ToDate(), 
            //                                         acdrivers.Value.ToNullableInt(),
            //                                         ddlTvae.SelectedValue.ToNullableInt(), 0, txtCustomerName.Text, txtCustomerMobile.Text,);
            //txtFirstPaid.Text.ToDecimalOrDefault()
            if (this.SalesOrderID != 0)
            {
                dc.usp_SalesOrderafterInvoice_Insert(this.SalesOrderID);
            }
            if (Qauta_ID > 0)
            {
                dc.usp_QuatoAfterInvoice_Insert(this.Qauta_ID, this.Invoice_ID);
            }
            if (this.Invoice_ID > 0)
            {

                foreach (DataRow r in this.dtItems.Rows)
                {
                    if (r.RowState != DataRowState.Deleted)
                    {

                     var ListPrice = dc.usp_ItemPrices_AvergeAndLast(r["Item_ID"].ToIntOrDefault(), r["Uom_iD"].ToInt()).ToList();
                        if (ListPrice.Any())
                        {
                            var o = ListPrice.FirstOrDefault();
                            if (o.Price1 > r["UnitCost"].ToDecimal() && o.Price1 > 0)
                            {
                                UserMessages.Message(null, "السعر اقل من سعر  حد الادني", string.Empty);
                                trans.Rollback();
                                return false;
                            }
                        }
                    }
                }


                foreach (DataRow r in this.dtItems.Rows)
                {
                    if (r.RowState == DataRowState.Deleted) continue;
                    Detail_ID = dc.usp_InvoiceDetailsIncludeTax_Insert(this.Invoice_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["Capacity"].ToString(), r["ItemDescription"].ToString(), r["IDCodeOperation"].ToString(), r["Policy"].ToString(), r["InvoiceDate"].ToDate(), r["Capacities"].ToString(), r["IsGift"].ToBooleanOrDefault(), r["UnitCostEvaluate"].ToDecimalOrDefault(),
                        false,null,null);

                    if (IsApproving)
                        if (cmp.ConsumptionRawMaterials.Value)
                        {
                            if (!this.InsertICJ(Detail_ID, r))
                            {
                                trans.Rollback();
                                return false;
                            }

                        }
                        else
                        {
                            if (!this.InsertICJNotCosumMaterial(Detail_ID, r))
                            {
                                trans.Rollback();
                                return false;
                            }
                        }

                    if (MyContext.Features.IsViewSerial)
                    {
                        var serailNumberConcate = r["SerialNumber"].ToExpressString().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        foreach (var sn in serailNumberConcate)
                        {
                            if (!string.IsNullOrEmpty(sn))
                            {
                                var res = dc.OperationSerialNumber_Bay(r["Item_ID"].ToInt(), DocumentsTableTypes.Receipt.ToInt(), this.Invoice_ID, sn.Trim());
                                if (res == -1)
                                {
                                    UserMessages.Message(null, "هذا السريل مباع " + sn + "", string.Empty);
                                    trans.Rollback();
                                    return false;
                                }
                                if (res == -2)
                                {
                                    UserMessages.Message(null, "هذا السريل  عليك إدخالة للنظام " + sn + "", string.Empty);
                                    trans.Rollback();
                                    return false;
                                }
                            }
                        }
                    }
                }


                //foreach (DataRow item in this.dtAllPayment.Rows)
                //{
                //    if (item.RowState == DataRowState.Deleted) continue;
                //    if (item.RowState == DataRowState.Added)
                //    {
                //        dc.usp_OperationGPaymentMethode_CRUD(0, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt(), item["GPaymentMethode_ID"].ToIntOrDefault(), item["Price"].ToDecimalOrDefault(), DateTime.Now.ToDate());
                //    }
                //    if (item.RowState == DataRowState.Modified)
                //    {
                //        dc.usp_OperationGPaymentMethode_CRUD(item["ID"].ToInt(), this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt(), item["GPaymentMethode_ID"].ToIntOrDefault(), item["Price"].ToDecimalOrDefault(), DateTime.Now.ToDate());
                //    }
                //    //if (item.RowState == DataRowState.Deleted)
                //    //{
                //    //    dc.usp_OperationGPaymentMethode_Delete(item["ID", DataRowVersion.Original].ToInt());
                //    //}
                //}

                foreach (DataRow r in this.dtTaxes.Rows)
                {
                    if (r.RowState == DataRowState.Deleted) continue;
                    dc.usp_DocuemntTaxes_Insert(DocumentsTableTypes.Invoice.ToInt(), this.Invoice_ID, r["Tax_ID"].ToInt());
                }
                if (IsApproving)
                    if (!this.InsertOperation())
                    {
                        trans.Rollback();
                        return false;
                    }
                LogAction(IsApproving ? Actions.Approve : Actions.Add, Serial, dc);
            }
        }
        else
        {
            int Result = dc.usp_InvoiceDelivery_Update(this.Invoice_ID, acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimalOrDefault(), txtOperationDate.Text.ToDate(),
                                                    acCustomer.Value.ToInt(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate, approvedBY_ID,
                                                    null, null, acAddress.Value.ToNullableInt(), acShipAddress.Value.ToNullableInt(), acPaymentAddress.Value.ToNullableInt(), acTelephone.Value.ToNullableInt(),
                                                    acCostCenter.Value.ToNullableInt(), txtNotes.Text, lblTotal.Text.ToDecimal(), txtPercentageDiscount.Text.ToDecimalOrDefault(), txtCashDiscount.Text.ToDecimalOrDefault(), txtAdditionals.Text.ToDecimalOrDefault(),
                                                    lblTotalDiscount.Text.ToDecimalOrDefault(), lblTotalTax.Text.ToDecimalOrDefault(), lblGrossTotal.Text.ToDecimalOrDefault(), firstpaid, txtUserRefNo.Text, this.DocRandomString, acSalesRep.Value.ToNullableInt(), acCashAccount.Value.ToNullableInt(), txtDeliveryDate.Text.ToDate(), acdrivers.Value.ToNullableInt(),
                                                    ddlTvae.SelectedValue.ToNullableInt(), 0, txtCustomerName.Text,string.Empty, txtCustomerMobile.Text, txtProjectRef.Text, txtContactPerson.Text,string.Empty, txtCustomerRepresentative.Text);

            if (this.SalesOrderID != 0)
            {
                dc.usp_SalesOrderafterInvoice_Insert(this.SalesOrderID);
            }
            if (Result > 0)
            {
                foreach (DataRow r in this.dtItems.Rows)
                {
                    var ListPrice = dc.usp_ItemPrices_AvergeAndLast(r["Item_ID"].ToIntOrDefault(), r["Uom_iD"].ToInt()).ToList();
                    if (ListPrice.Any())
                    {
                        var o = ListPrice.FirstOrDefault();
                        if (o.Price1 > r["UnitCost"].ToDecimal() && o.Price1 > 0)
                        {
                            UserMessages.Message(null, "السعر اقل من سعر  حد الادني", string.Empty);
                            trans.Rollback();
                            return false;
                        }
                    }
                }

                foreach (DataRow r in this.dtItems.Rows)
                {
                    if (r.RowState == DataRowState.Added)
                    {
                        Detail_ID = dc.usp_InvoiceDetailsIncludeTax_Insert(this.Invoice_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["Capacity"].ToString(), r["ItemDescription"].ToString(), r["IDCodeOperation"].ToString(), r["Policy"].ToString(), r["InvoiceDate"].ToDate(), r["Capacities"].ToString(), r["IsGift"].ToBooleanOrDefault(), r["UnitCostEvaluate"].ToDecimalOrDefault(),
                            false,null,null);
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

                        if (cmp.ConsumptionRawMaterials.Value)
                        {
                            if (!this.InsertICJ(Detail_ID, r)) { trans.Rollback(); return false; }
                        }
                        else
                        {
                            if (!this.InsertICJNotCosumMaterial(Detail_ID, r)) { trans.Rollback(); return false; }
                        }
                    }

                    if (MyContext.Features.IsViewSerial)
                    {
                        var serailNumberConcate = r["SerialNumber"].ToExpressString().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        foreach (var sn in serailNumberConcate)
                        {
                            var res = dc.OperationSerialNumber_Bay(r["Item_ID"].ToInt(), DocumentsTableTypes.Receipt.ToInt(), this.Invoice_ID, sn.Trim());
                            if (res == -1)
                            {
                                UserMessages.Message(null, "هذا السريل مباع " + sn + "", string.Empty);
                                trans.Rollback();
                                return false;
                            }
                            if (res == -2)
                            {
                                UserMessages.Message(null, "هذا السريل  عليك إدخالة للنظام " + sn + "", string.Empty);
                                trans.Rollback();
                                return false;
                            }
                        }
                    }
                }
                //foreach (DataRow item in this.dtAllPayment.Rows)
                //{
                //    if (item.RowState == DataRowState.Deleted) continue;
                //    //if (item.RowState == DataRowState.Added)
                //    //{
                //    //    dc.usp_OperationGPaymentMethode_CRUD(0, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt(), item[""].ToIntOrDefault(), item["Price"].ToDecimalOrDefault(), DateTime.Now.ToDate());
                //    //}
                //    //if (item.RowState == DataRowState.Modified)
                //    //{
                //    dc.usp_OperationGPaymentMethode_CRUD(item["ID"].ToInt(), this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt(), item["GPaymentMethode_ID"].ToIntOrDefault(), item["Price"].ToDecimalOrDefault(), DateTime.Now.ToDate());
                //    //}
                //    //if (item.RowState == DataRowState.Deleted)
                //    //{
                //    //    dc.usp_OperationGPaymentMethode_Delete(item["ID", DataRowVersion.Original].ToInt());
                //    //}
                //}

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
                if (IsApproving)
                    if (!this.InsertOperation())
                    {

                        trans.Rollback();
                        return false;
                    }
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


        //var invs = dc.Invoices.Where(c => c.ID == this.Invoice_ID).ToList();
        //if (invs.Any())
        //{
        //    var inv = invs.FirstOrDefault();

        //    inv.typePayment_ID = ddlPaymentMethode.SelectedValue.ToIntOrDefault();
        //    dc.SubmitChanges();
        //}


        Serial = Serial == string.Empty ? string.Empty : " (" + Serial + ") ";
        UserMessages.MessageWithPtint(null, Resources.UserInfoMessages.OperationSuccess + Serial, PageLinks.InvoiceShortcut + Request.PathInfo + "?ID=" + this.Invoice_ID.ToExpressString(), PageLinks.InvoicesList, PageLinks.InvoiceShortcut + Request.PathInfo, "~/Report_Dev/PrintInnoiceeDev.aspx?Invoice_ID=" + this.Invoice_ID + "&IsMaterla=1");
        return true;
    }

    private bool InsertOperation()
    {
        decimal ratio = txtRatio.Text.ToDecimal();
        decimal Additionals = txtAdditionals.Text.ToDecimalOrDefault();
        string serial = string.Empty;
        var company = dc.usp_Company_Select().FirstOrDefault();
        int ContactAccountID = dc.fun_getContactAccountID(acCustomer.Value.ToInt()).Value;

        int Result = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.Sales.ToInt(), ddlCurrency.SelectedValue.ToInt(), (this.Total + this.TotalCreditTax + Additionals) * ratio, (this.Total + this.TotalCreditTax + Additionals), ratio, txtNotes.Text);

        List<ClsIyrad> lstIyrad = new List<ClsIyrad>();



        foreach (DataRow r in this.dtItems.Rows)
        {
            if (r.RowState != DataRowState.Deleted)
            {
                // r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal()
                lstIyrad.Add(new ClsIyrad()
                {
                    Account_ID = r["Account_ID"].ToIntOrDefault(),
                    Item_ID = r["Item_ID"].ToIntOrDefault(),
                    Qty = r["Quantity"].ToDecimalOrDefault(),
                    UniteCost = r["UnitCost"].ToDecimalOrDefault()

                });

            }
        }

        var listGrouping = lstIyrad.GroupBy(c => c.Account_ID).ToList();
        foreach (var item in listGrouping)
        {

            var tot = item.ToList().Sum(c => c.Qty * c.UniteCost);

            if (item.Key != 0)
            {
                //ايراد المبيعات
                dc.usp_OperationDetails_Insert(Result, item.Key, 0, tot * ratio, 0, tot, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
            }
            else
            {
                //ايراد المبيعات
                dc.usp_OperationDetails_Insert(Result, company.SellAccount_ID, 0, tot * ratio, 0, tot, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
            }


        }
        ////ايراد المبيعات
        //dc.usp_OperationDetails_Insert(Result, company.SellAccount_ID, 0, this.Total * ratio, 0, this.Total, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());









        //ايراد الاضافات
        if (Additionals > 0) dc.usp_OperationDetails_Insert(Result, company.InvoiceAdditionals_AccountID, 0, Additionals * ratio, 0, Additionals, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());

        //Customer
        if (ddlPaymentMethode.SelectedValue != "-1")
        {
            var Accou_ID = 0;
            var paymentMethods = dc.SettingPointOs.Where(c => c.IsActive.Value).ToList();
            var IDPM = ddlPaymentMethode.SelectedValue.ToIntOrDefault();
            var ChoosedPayMethodes_ID = paymentMethods.Where(c => c.ID == IDPM).FirstOrDefault();


            if (ChoosedPayMethodes_ID != null)
            {
                if (MyContext.UserProfile.CashierAccount_ID > 0 && ddlPaymentMethode.SelectedValue == "0")
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
                if (MyContext.UserProfile.CashierAccount_ID > 0 && ddlPaymentMethode.SelectedValue == "0")
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
                dc.usp_OperationDetails_Insert(Result, Accou_ID, (this.GrossTotal) * ratio, 0, (this.GrossTotal), 0, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
                dc.usp_OperationDetails_Insert(Result, ContactAccountID, 0, (this.GrossTotal) * ratio, 0, (this.GrossTotal), null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
                dc.usp_OperationDetails_Insert(Result, ContactAccountID, (this.GrossTotal) * ratio, 0, (this.GrossTotal), 0, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
            
            }
            else
            {
                dc.usp_OperationDetails_Insert(Result, ContactAccountID, (this.GrossTotal) * ratio, 0, (this.GrossTotal), 0, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
            }
        }
        else
        {
            dc.usp_OperationDetails_Insert(Result, ContactAccountID, (this.GrossTotal) * ratio, 0, (this.GrossTotal), 0, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
        }


        //Discount
        if (this.TotalDiscount > 0)
        {
            dc.usp_OperationDetails_Insert(Result, company.SalesDiscountAccountID, this.TotalDiscount * ratio, 0, this.TotalDiscount, 0, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
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




        //foreach (DataRow item in this.dtAllPayment.Rows)
        //{
        //    if (item.RowState == DataRowState.Deleted) continue;
        //    if (item["Price"].ToDecimalOrDefault() > 0)
        //    {
        //        var AccountPayMethode_ID = dc.usp_GenerateMethodePayment_Select(item["GPaymentMethode_ID"].ToIntOrDefault(), string.Empty).FirstOrDefault();
        //        this.InsertCashIn(item["Price"].ToDecimalOrDefault(), AccountPayMethode_ID.Account_ID.Value);
        //    }
        //}



        //if (ddlPaymentMethode.SelectedIndex != 0)
        //{
        //    var paymentMethods = dc.SettingPointOs.Where(c => c.IsActive.Value).ToList();
        //    var IDPM = ddlPaymentMethode.SelectedValue.ToIntOrDefault();
        //    var ChoosedPayMethodes_ID = paymentMethods.Where(c => c.ID == IDPM).FirstOrDefault();
        //    if (ChoosedPayMethodes_ID != null)
        //    {
        //        if (ChoosedPayMethodes_ID.IsCash.ToBooleanOrDefault())
        //        {
        //            this.InsertCashIn(ChoosedPayMethodes_ID.SalesAccountID.ToIntOrDefault());
        //        }
        //        else
        //        {
        //            this.InsertBankIn(ChoosedPayMethodes_ID.SalesAccountID.ToIntOrDefault());
        //        }
        //    }
        //}

        return true;
    }

    //private void InsertOperation()
    //{

    //    //decimal ratio = txtRatio.Text.ToDecimal();
    //    //decimal Additionals = txtAdditionals.Text.ToDecimalOrDefault();
    //    //string serial = string.Empty;
    //    //var company = dc.usp_Company_Select().FirstOrDefault();
    //    //int ContactAccountID = dc.fun_getContactAccountID(acCustomer.Value.ToInt()).Value;
    //    //var salesGiftAccountID = company.SalesGiftAccountID.ToIntOrDefault();

    //    //int Result = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.Sales.ToInt(), ddlCurrency.SelectedValue.ToInt(), (this.Total + this.TotalCreditTax + Additionals) * ratio, (this.Total + this.TotalCreditTax + Additionals), ratio, txtNotes.Text);

    //    ////ايراد المبيعات
    //    //dc.usp_OperationDetails_Insert(Result, company.SellAccount_ID, 0, this.Total * ratio, 0, this.Total, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());

    //    ////ايراد الاضافات
    //    //if (Additionals > 0) dc.usp_OperationDetails_Insert(Result, company.InvoiceAdditionals_AccountID, 0, Additionals * ratio, 0, Additionals, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());


    //    ////Customer
    //    //dc.usp_OperationDetails_Insert(Result, ContactAccountID, (this.GrossTotal) * ratio, 0, (this.GrossTotal), 0, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());


    //    //if (this.TotalDiscount > 0)
    //    //{
    //    //    if (salesGiftAccountID > 0)
    //    //    {
    //    //        if (dtItems.Select("IsGift=1").Count() > 0)
    //    //        {
    //    //            dc.usp_OperationDetails_Insert(Result, company.SalesGiftAccountID, this.TotalDiscount * ratio, 0, this.TotalDiscount, 0, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());

    //    //        }
    //    //        else
    //    //        {
    //    //            dc.usp_OperationDetails_Insert(Result, company.SalesDiscountAccountID, this.TotalDiscount * ratio, 0, this.TotalDiscount, 0, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());

    //    //        }
    //    //    }
    //    //    else
    //    //    {
    //    //        dc.usp_OperationDetails_Insert(Result, company.SalesDiscountAccountID, this.TotalDiscount * ratio, 0, this.TotalDiscount, 0, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
    //    //    }

    //    //}
    //    //////Discount
    //    ////if (this.TotalDiscount > 0)
    //    ////{
    //    ////    dc.usp_OperationDetails_Insert(Result, company.SalesDiscountAccountID, this.TotalDiscount * ratio, 0, this.TotalDiscount, 0, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
    //    ////}

    //    ////Taxes
    //    //var GroupedTaxes = from tax in this.dtAllTaxes.AsEnumerable()
    //    //                   where tax.RowState != DataRowState.Deleted
    //    //                   group tax by new { Account_ID = tax.Field<int>("Account_ID"), OnInvoiceType = tax.Field<string>("OnInvoiceType") } into groupedTaxes
    //    //                   select new { Key = groupedTaxes.Key, DebitAmount = groupedTaxes.Sum(x => x.Field<decimal>("DebitAmount")), CreditAmount = groupedTaxes.Sum(x => x.Field<decimal>("CreditAmount")) };
    //    //foreach (var tax in GroupedTaxes)
    //    //{
    //    //    dc.usp_OperationDetails_Insert(Result, tax.Key.Account_ID, tax.DebitAmount * ratio, tax.CreditAmount * ratio, tax.DebitAmount, tax.CreditAmount, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
    //    //}

    //    //if (this.CalculatedSalesCost > 0)
    //    //{
    //    //    dc.usp_SalesCostOperation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), this.CalculatedSalesCost, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
    //    //}

    //    ////CostCenter Debit
    //    //dc.usp_CostCenterOperation_Insert(acBranch.Value.ToNullableInt(), acCostCenter.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), this.CalculatedSalesCost + (this.TotalDiscount * ratio), this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt(), txtNotes.Text);
    //    ////CostCenter Credit
    //    //dc.usp_CostCenterOperation_Insert(acBranch.Value.ToNullableInt(), acCostCenter.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), (this.Total + Additionals) * ratio * -1, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt(), txtNotes.Text);
    //    //this.InsertCashIn();










    //    decimal ratio = txtRatio.Text.ToDecimal();
    //    decimal Additionals = txtAdditionals.Text.ToDecimalOrDefault();
    //    string serial = string.Empty;
    //    var company = dc.usp_Company_Select().FirstOrDefault();
    //    int ContactAccountID = dc.fun_getContactAccountID(acCustomer.Value.ToInt()).Value;
    //    var salesGiftAccountID = company.SalesGiftAccountID.ToIntOrDefault();

    //    int Result = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.Sales.ToInt(), ddlCurrency.SelectedValue.ToInt(), (this.Total + this.TotalCreditTax + Additionals) * ratio, (this.Total + this.TotalCreditTax + Additionals), ratio, txtNotes.Text);


    //    decimal Total_Sales = this.Total - TotalServices;

    //    //ايراد المبيعات
    //    if (Total_Sales > 0)
    //    {
    //        dc.usp_OperationDetails_Insert(Result, company.SellAccount_ID, 0, Total_Sales * ratio, 0, Total_Sales, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
    //    }



    //    //ايراد الاضافات
    //    if (Additionals > 0) dc.usp_OperationDetails_Insert(Result, company.InvoiceAdditionals_AccountID, 0, Additionals * ratio, 0, Additionals, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());


    //    //Services
    //    if (this.TotalServices > 0)
    //    {
    //        dc.usp_OperationDetails_Insert(Result, company.ServicesExpAccount_ID, 0, this.TotalServices * ratio, 0, this.TotalServices, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());

    //    }

    //    ////Gift
    //    //if (this.TotalServices > 0)
    //    //{
    //    //    dc.usp_OperationDetails_Insert(Result, company.SalesGiftAccountID, 0, this.TotalGift * ratio, 0, this.TotalGift, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());

    //    //}

    //    //Begin Payment

    //    #region Payment


    //    decimal valueAmountPay = 0;
    //    foreach (DataRow item in this.dtAllPayment.Rows)
    //    {
    //        valueAmountPay += item["Price"].ToDecimalOrDefault();

    //        if (item.RowState == DataRowState.Deleted) continue;
    //        if (item["Price"].ToDecimalOrDefault() > 0)
    //        {
    //            var AccountPayMethode_ID = dc.usp_GenerateMethodePayment_Select(item["GPaymentMethode_ID"].ToIntOrDefault(), string.Empty).FirstOrDefault();
    //            dc.usp_OperationDetails_Insert(Result, AccountPayMethode_ID.Account_ID, (item["Price"].ToDecimalOrDefault()) * ratio, 0, (item["Price"].ToDecimalOrDefault()), 0, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
    //        }
    //    }
    //    if (this.GrossTotal - valueAmountPay > 0)
    //    {
    //        //Customer
    //        dc.usp_OperationDetails_Insert(Result, ContactAccountID, (this.GrossTotal - valueAmountPay) * ratio, 0, (this.GrossTotal - valueAmountPay), 0, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
    //    }
    //    //decimal valueAmountPay = 0;
    //    //foreach (GridViewRow gvRow in gvPay.Rows)
    //    //{


    //    //    var id = gvPay.DataKeys[gvRow.RowIndex]["ID"].ToInt();
    //    //    var Posted_ID = gvPay.DataKeys[gvRow.RowIndex]["Posted_ID"].ToIntOrDefault();


    //    //}

    //    #endregion

    //    //End Payment



    //    if (this.TotalDiscount > 0)
    //    {
    //        if (salesGiftAccountID > 0)
    //        {


    //            if (dtItems.Select("IsGift=1").Count() > 0)
    //            {

    //                dc.usp_OperationDetails_Insert(Result, company.SalesGiftAccountID, this.TotalDiscount * ratio, 0, this.TotalDiscount, 0, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());

    //            }
    //            else
    //            {

    //                dc.usp_OperationDetails_Insert(Result, company.SalesDiscountAccountID, this.TotalDiscount * ratio, 0, this.TotalDiscount, 0, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());

    //            }


    //        }
    //        else
    //        {
    //            dc.usp_OperationDetails_Insert(Result, company.SalesDiscountAccountID, this.TotalDiscount * ratio, 0, this.TotalDiscount, 0, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
    //        }

    //    }
    //    ////Discount
    //    //if (this.TotalDiscount > 0)
    //    //{
    //    //    dc.usp_OperationDetails_Insert(Result, company.SalesDiscountAccountID, this.TotalDiscount * ratio, 0, this.TotalDiscount, 0, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
    //    //}

    //    //Taxes
    //    var GroupedTaxes = from tax in this.dtAllTaxes.AsEnumerable()
    //                       where tax.RowState != DataRowState.Deleted
    //                       group tax by new { Account_ID = tax.Field<int>("Account_ID"), OnInvoiceType = tax.Field<string>("OnInvoiceType") } into groupedTaxes
    //                       select new { Key = groupedTaxes.Key, DebitAmount = groupedTaxes.Sum(x => x.Field<decimal>("DebitAmount")), CreditAmount = groupedTaxes.Sum(x => x.Field<decimal>("CreditAmount")) };
    //    foreach (var tax in GroupedTaxes)
    //    {
    //        dc.usp_OperationDetails_Insert(Result, tax.Key.Account_ID, tax.DebitAmount * ratio, tax.CreditAmount * ratio, tax.DebitAmount, tax.CreditAmount, null, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
    //    }

    //    if (this.CalculatedSalesCost > 0)
    //    {
    //        dc.usp_SalesCostOperation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), this.CalculatedSalesCost, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt());
    //    }

    //    //CostCenter Debit
    //    dc.usp_CostCenterOperation_Insert(acBranch.Value.ToNullableInt(), acCostCenter.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), this.CalculatedSalesCost + (this.TotalDiscount * ratio), this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt(), txtNotes.Text);
    //    //CostCenter Credit
    //    dc.usp_CostCenterOperation_Insert(acBranch.Value.ToNullableInt(), acCostCenter.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), (this.Total + Additionals) * ratio * -1, this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt(), txtNotes.Text);
    //    this.InsertCashIn();
    //}

    private void InsertCashIn(Decimal PaidAmount, int AccountPayMethode_ID)
    {
        string Serial = string.Empty;
        decimal ratio = txtRatio.Text.ToDecimal();
        int? CashIn_ID = null;
        if (PaidAmount <= 0) return;
        int ContactChartOfAccount_ID = dc.fun_getContactAccountID(acCustomer.Value.ToInt()).Value;
        CashIn_ID = dc.usp_Payments_Insert(txtOperationDate.Text.ToDate(), DateTime.Now, MyContext.UserProfile.Contact_ID, MyContext.UserProfile.Contact_ID, DateTime.Now, txtUserRefNo.TrimmedText, ref Serial, DocSerials.CashIn.ToInt(), txtNotes.Text, PaidAmount, null, DocStatus.Approved.ToByte(), PaymentsTypes.CashInCustomer.ToByte(), acBranch.Value.ToNullableInt(), txtRatio.Text.ToDecimal(), ddlCurrency.SelectedValue.ToInt(), this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt(), this.DocRandomString + "_FromInvoice");
        if (!CashIn_ID.HasValue || CashIn_ID.Value <= 0) throw new Exception("Error Occured During inserting the cash document");
        dc.usp_PaymentsDetails_Insert(CashIn_ID, AccountPayMethode_ID, ContactChartOfAccount_ID, PaidAmount, null, string.Empty, null);

        int Operation_ID = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref Serial, DocStatus.Approved.ToByte(), OperationTypes.CashIn.ToInt(), ddlCurrency.SelectedValue.ToInt(), PaidAmount * ratio, PaidAmount, ratio, null);
        dc.usp_OperationDetails_Insert(Operation_ID, ContactChartOfAccount_ID, 0, PaidAmount * ratio, 0, PaidAmount, null, CashIn_ID, DocumentsTableTypes.Payment_CashIn.ToInt());
        dc.usp_OperationDetails_Insert(Operation_ID, AccountPayMethode_ID, PaidAmount * ratio, 0, PaidAmount, 0, null, CashIn_ID, DocumentsTableTypes.Payment_CashIn.ToInt());
        dc.usp_SetCashDocForBills(this.Invoice_ID, CashIn_ID, DocumentsTableTypes.Invoice.ToInt());
    }

    #region ICJ
    private bool InsertICJ(int Detail_ID, DataRow row)
    {
        decimal? SalesCost = 0;



        var lstMaterials = dc.usp_ProductionOrderDetailsForFinalItem_Select(row["Item_ID"].ToInt(), row["Store_ID"].ToInt(), row["Quantity"].ToDecimal()).ToList();//.CopyToDataTable();
        foreach (var item in lstMaterials)
        {
            var res = InsertOutICJ(Detail_ID, this.Invoice_ID, item, txtOperationDate.Text.ToDate().Value, acBranch.Value.ToIntOrDefault());
            if (!res.Item1)
            {
                UserMessages.Message(null, res.Item2, string.Empty);
                this.ConfirmationMessage = string.Empty;
                return false;


            }
        }

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

        if (!lstMaterials.Any())
        {





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
        }
        else
        {

        }
        return true;
    }

    private Tuple<bool, string> InsertOutICJ(int Detail_ID, int id, usp_ProductionOrderDetailsForFinalItem_SelectResult row, DateTime txtOperationDate, int acBranch)
    {
        decimal? Cost = 0;
        int result = 0;

        result = dc.usp_ICJ_ItemMaterial_Out(txtOperationDate.ToDate(), row.Quantity.ToDecimal(), row.Item_ID.ToInt(), null, 0, null, row.Store_ID.ToInt(), acBranch.ToNullableInt(), DocumentsTableTypes.Invoice.ToInt(), id, Detail_ID, ref Cost);
        this.CalculatedSalesCost += Cost.Value;

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



    #endregion

    private void FillInvoice()
    {
        var idGene = Request.QueryString["GenerateInoice_ID"];
        var Qt_ID = Request.QueryString["Qt_ID"];

        if (string.IsNullOrEmpty(idGene) && string.IsNullOrEmpty(Qt_ID))
        {

            if (this.Invoice_ID == 0 && this.SalesOrderID == 0) return;
            var invoice = dc.usp_Invoice_SelectByID(EditMode ? this.Invoice_ID : this.SalesOrderID).FirstOrDefault();

            OperationsView.SourceDocTypeType_ID = DocumentsTableTypes.Invoice.ToInt();
            OperationsView.Source_ID = this.Invoice_ID;

            ddlCurrency.SelectedValue = invoice.Currency_ID.ToExpressString();
            txtRatio.Text = invoice.Ratio.ToExpressString();
            acBranch.Value = invoice.Branch_ID.ToStringOrEmpty();

            ddlTvae.SelectedValue = invoice.IsTax.ToExpressString();
            this.IsTaxFound = invoice.IsTax.ToIntOrDefault();


            this.FilterByBranchAndCurrency();
            txtOperationDate.Text = invoice.OperationDate.Value.ToExpressString();
            acCostCenter.Value = invoice.CostCenter_ID.ToStringOrEmpty();
            txtUserRefNo.Text = invoice.UserRefNo;
            if (invoice.DateDelivery != null)
                txtDeliveryDate.Text = invoice.DateDelivery.Value.ToExpressString();

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
            // txtFirstPaid.Text = invoice.FirstPaid.ToExpressString();
            this.txtFirstPaid_TextChanged(null, null);
            lblTotal.Text = invoice.TotalAmount.ToExpressString();
            lblTotalDiscount.Text = invoice.TotalDiscount.ToExpressString();
            lblTotalTax.Text = invoice.TotalTax.ToExpressString();
            lblGrossTotal.Text = invoice.GrossTotalAmount.ToExpressString();
            txtNotes.Text = invoice.Notes;
            acdrivers.Value = invoice.Driver_ID.ToStringOrEmpty();
            txtCustomerName.Text = invoice.CustomerName;
            txtCustomerMobile.Text = invoice.CustomerMobile;
            ddlPaymentMethode.SelectedValue = invoice.typePayment_ID.ToExpressString();
            txtCost.Enabled = MyContext.PageData.IsPrice;
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

            if (EditMode)
            {
                txtSerial.Text = invoice.Serial;
                ucNav.SetText = invoice.Serial;
                this.DocRandomString = invoice.DocRandomString;
                lblCreatedBy.Text = invoice.CreatedByName;
                lblApprovedBy.Text = invoice.ApprovedBYName;
                this.ImgStatus = ((DocStatus)invoice.DocStatus_ID).ToExpressString();
                btnPrint.Visible = MyContext.PageData.IsPrint && (invoice.DocStatus_ID == 2);
                btnPrintInventoryOrder.Visible = MyContext.PageData.IsPrint;
                pnlAddItem.Visible = (invoice.DocStatus_ID == 1);
                btnCancel.Visible = (invoice.DocStatus_ID == 1) && MyContext.PageData.IsDelete;
                btnApprove.Visible = btnBtnApproveAndPay.Visible = (invoice.DocStatus_ID == 1) && MyContext.PageData.IsApprove;
                //btnCancelApprove.Visible = !btnApprove.Visible && (invoice.DocStatus_ID == 2) && MyContext.PageData.IsNotApprove;
                btnCancelApprove.Visible = !btnApprove.Visible && (invoice.DocStatus_ID == 2) && MyContext.PageData.IsNotApprove;
                btnApproveAccounting.Visible = (invoice.DocStatus_ID == 1) && MyContext.PageData.IsApproveAccounting;
                btnSave.Visible = (invoice.DocStatus_ID == 1) && ((MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode));
                //gvItems.Columns[gvItems.Columns.Count - 1].Visible = gvItems.Columns[gvItems.Columns.Count - 2].Visible = (invoice.DocStatus_ID == 1);
                // gvTaxes.Columns[gvTaxes.Columns.Count - 1].Visible = (invoice.DocStatus_ID == 1);
                btnPrint.Visible = MyContext.PageData.IsPrint && (invoice.DocStatus_ID == 2);
                OperationsView.Visible = !btnApprove.Visible;
                this.DocStatus_ID = invoice.DocStatus_ID.Value;
                txtCost.Enabled = MyContext.PageData.IsPrice;
            }

            // this.dtItems = dc.usp_InvoiceDetailsIncludeTax_Select(EditMode ? this.Invoice_ID : this.SalesOrderID).CopyToDataTable();


            var lstItems = dc.usp_InvoiceDetailsIncludeTax_Select(EditMode ? this.Invoice_ID : this.SalesOrderID).ToList();
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
        else if (!string.IsNullOrEmpty(Qt_ID))
        {
            var idInv = int.Parse(Qt_ID);
            this.Qauta_ID = idInv;
            var invoice = dc.usp_InvoiceQuota_SelectByID(idInv).FirstOrDefault();
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

            this.txtFirstPaid_TextChanged(null, null);
            lblTotal.Text = invoice.TotalAmount.ToExpressString();
            lblTotalDiscount.Text = invoice.TotalDiscount.ToExpressString();
            lblTotalTax.Text = invoice.TotalTax.ToExpressString();
            lblGrossTotal.Text = invoice.GrossTotalAmount.ToExpressString();
            txtNotes.Text = invoice.Notes;
            invoice.DocStatus_ID = 1;
            this.ImgStatus = ((DocStatus)invoice.DocStatus_ID).ToExpressString();


            btnPrintInventoryOrder.Visible = MyContext.PageData.IsPrint;
            pnlAddItem.Visible = (invoice.DocStatus_ID == 1);
            btnCancel.Visible = (invoice.DocStatus_ID == 1) && MyContext.PageData.IsDelete;
            btnApprove.Visible = (invoice.DocStatus_ID == 1) && MyContext.PageData.IsApprove;
            btnSave.Visible = (invoice.DocStatus_ID == 1) && ((MyContext.PageData.IsAdd) || (MyContext.PageData.IsEdit));

            this.DocStatus_ID = invoice.DocStatus_ID.Value;
            btnPrint.Visible = MyContext.PageData.IsPrint && (invoice.DocStatus_ID == 2);

            //this.dtItems = dc.usp_InvoiceDetailsIncludeTax_Select(idInv).CopyToDataTable();
            var listInvoiceDetailsQota = dc.usp_InvoiceQuotaDetails_Select(idInv).ToList();
            foreach (var item in listInvoiceDetailsQota)
            {


                DataRow r = this.dtItems.NewRow();
                r["ID"] = this.dtItems.GetID("ID");


                r["SerialNumber"] = string.Empty;
                r["Store_ID"] = item.Store_ID;
                r["Category_ID"] = item.Category_ID;
                r["Item_ID"] = item.Item_ID;
                r["ItemName"] = item.ItemName;

                r["IDCodeOperation"] = string.Empty;
                r["Policy"] = string.Empty;
                r["Capacity"] = string.Empty;
                r["Capacities"] = string.Empty;
                r["ItemDescription"] = string.Empty;
                r["DescribedName"] = string.Empty;
                r["UnitCost"] = item.UnitCost;
                r["UnitCostEvaluate"] = item.UnitCost;
                r["Quantity"] = item.Quantity;
                r["QtyInNumber"] = 0;
                r["Uom_ID"] = item.Uom_ID;
                r["UOMName"] = item.UOMName;
                r["Batch_ID"] = DBNull.Value;
                r["BatchName"] = string.Empty;
                r["PercentageDiscount"] = 0;
                r["CashDiscount"] = 0;
                r["TotalTax"] = 0;
                r["Notes"] = item.Notes;
                r["StoreName"] = item.StoreName;
                r["Barcode"] = item.Barcode;
                r["CategoryName"] = item.CategoryName;
                var Tax = dc.usp_Taxes_Select(item.Tax_ID.ToIntOrDefault(), string.Empty).FirstOrDefault();
                decimal taxInclude = TypeTax == 2 ? (Decimal.Divide((Tax != null ? Tax.PercentageValue.Value : 0), (100M + (Tax != null ? Tax.PercentageValue.Value : 0)))) : Decimal.Divide((Tax != null ? Tax.PercentageValue.Value : 0), (100M));
                decimal unitCost = TypeTax == 2 ? (txtCost.Text.ToDecimalOrDefault() - txtCost.Text.ToDecimalOrDefault() * taxInclude) : txtCost.Text.ToDecimalOrDefault();
                r["TotalCostBeforTax"] = Math.Round(item.UnitCost.ToDecimal() * item.Quantity.ToDecimal(), 3).ToDecimalOrDefault();
                if (Tax != null)
                {
                    r["TaxName"] = item.TaxName;
                    r["Tax_ID"] = Tax.ID;
                    r["TaxPercentageValue"] = Tax.PercentageValue;
                    if (Tax.OnInvoiceType.HasValue) r["TaxOnInvoiceType"] = Tax.OnInvoiceType;
                    if (Tax.OnReceiptType.HasValue) r["TaxOnReceiptType"] = Tax.OnReceiptType;
                    if (Tax.OnDocCreditType.HasValue) r["TaxOnDocCreditType"] = Tax.OnDocCreditType;
                    r["TaxSalesAccountID"] = Tax.SalesAccountID;
                    r["TaxPurchaseAccountID"] = Tax.PurchaseAccountID;
                }


                r["Total"] = 0;
                r["GrossTotal"] = 0;
                if (this.EditID == 0) this.dtItems.Rows.Add(r);
            }



            this.dtTaxes = dc.usp_DocuemntTaxes_Select(DocumentsTableTypes.Invoice.ToInt(), idInv, false).CopyToDataTable();
            this.BindItemsGrid();
            gvTaxes.DataSource = this.dtTaxes;
            gvTaxes.DataBind();
        }
        else
        {


            var idInv = int.Parse(idGene);


            var invoice = dc.usp_Invoice_SelectByID(idInv).FirstOrDefault();
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
            // txtFirstPaid.Text = invoice.FirstPaid.ToExpressString();
            this.txtFirstPaid_TextChanged(null, null);
            lblTotal.Text = invoice.TotalAmount.ToExpressString();
            lblTotalDiscount.Text = invoice.TotalDiscount.ToExpressString();
            lblTotalTax.Text = invoice.TotalTax.ToExpressString();
            lblGrossTotal.Text = invoice.GrossTotalAmount.ToExpressString();
            txtNotes.Text = invoice.Notes;
            invoice.DocStatus_ID = 1;



            // txtSerial.Text = invoice.Serial;
            //  this.DocRandomString = invoice.DocRandomString;
            //lblCreatedBy.Text = invoice.CreatedByName;
            // lblApprovedBy.Text = invoice.ApprovedBYName;
            this.ImgStatus = ((DocStatus)invoice.DocStatus_ID).ToExpressString();


            btnPrintInventoryOrder.Visible = MyContext.PageData.IsPrint;
            pnlAddItem.Visible = (invoice.DocStatus_ID == 1);
            btnCancel.Visible = (invoice.DocStatus_ID == 1) && MyContext.PageData.IsDelete;
            btnApprove.Visible = (invoice.DocStatus_ID == 1) && MyContext.PageData.IsApprove;
            btnSave.Visible = (invoice.DocStatus_ID == 1) && ((MyContext.PageData.IsAdd) || (MyContext.PageData.IsEdit));
            //gvItems.Columns[gvItems.Columns.Count - 1].Visible = gvItems.Columns[gvItems.Columns.Count - 2].Visible = (invoice.DocStatus_ID == 1);
            // gvTaxes.Columns[gvTaxes.Columns.Count - 1].Visible = (invoice.DocStatus_ID == 1);
            this.DocStatus_ID = invoice.DocStatus_ID.Value;
            btnPrint.Visible = MyContext.PageData.IsPrint && (invoice.DocStatus_ID == 2);




            this.dtItems = dc.usp_InvoiceDetailsIncludeTax_Select(idInv).CopyToDataTable();
            // this.dtItems = dc.usp_InvoiceDetails_Select(EditMode ? this.Invoice_ID : this.SalesOrderID).CopyToDataTable();
            this.dtTaxes = dc.usp_DocuemntTaxes_Select(DocumentsTableTypes.Invoice.ToInt(), idInv, false).CopyToDataTable();
            this.BindItemsGrid();
            gvTaxes.DataSource = this.dtTaxes;
            gvTaxes.DataBind();
        }

    }

    private void CheckSecurity()
    {
        //if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        btnSave.Visible = (MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode);
        btnApprove.Visible = btnBtnApproveAndPay.Visible = MyContext.PageData.IsApprove;
        btnCancelApprove.Visible = !btnApprove.Visible && MyContext.PageData.IsNotApprove;
        btnCancel.Visible = MyContext.PageData.IsDelete && EditMode;
        btnApproveAccounting.Visible = MyContext.PageData.IsApproveAccounting;


        txtCost.Enabled = MyContext.PageData.IsPrice;

        //btnPrint.Visible = btnPrintInventoryOrder.Visible = btnPrint_Design.Visible = DropDownList1.Visible = MyContext.PrintAfterApprove || btnApprove.Visible;
        btnPrint.Visible = btnCancelApprove.Visible && MyContext.PrintAfterApprove;
        MyContext con = new MyContext(System.Web.Security.Membership.GetUser(), PageLinks.Customers, string.Empty);
        btnFastAddNew.Visible = con.PageData.IsAdd;
        OperationsView.Visible = !btnApprove.Visible;

        lnkGetPriceIteme.Visible = MyContext.PageData.IsViewPriceIteme;
        lnkGroupStoreIteme.Visible = MyContext.PageData.ViewBalanceIteme;

        lblPrice.Visible = !MyContext.PageData.IsViewPriceIteme;
        lblQty.Visible = !MyContext.PageData.ViewBalanceIteme;



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
        decimal TotalItem = 0;

        TotalServices = 0;
        foreach (DataRow r in this.dtItems.Rows)
        {
            if (r.RowState == DataRowState.Deleted) continue;
            decimal TaxValue = r["TaxPercentageValue"].ToDecimalOrDefault();
            if (this.EditMode)
            {
                if (this.IsTaxFound == 2)
                    TaxValue = 0;
            }
            else
            {
                if (ddlTvae.SelectedValue == "2")
                    TaxValue = 0;
            }

            ItemDiscount = 0;


            decimal taxInclude = (TypeTax == 2 ? Decimal.Divide(TaxValue, (100 + TaxValue)) : 0);
            var uc = r["UnitCostEvaluate"].ToDecimalOrDefault();
            var taxUc = r["UnitCostEvaluate"].ToDecimalOrDefault() * taxInclude;
            r["UnitCost"] = uc - taxUc;
            r["Total"] = r["UnitCost"].ToDecimalOrDefault() * r["Quantity"].ToDecimalOrDefault();
            Total += r["UnitCost"].ToDecimalOrDefault() * r["Quantity"].ToDecimalOrDefault();

            this.TotalDiscount += ItemDiscount = (r["PercentageDiscount"].ToDecimalOrDefault() * r["Total"].ToDecimalOrDefault() * 0.01m) + r["CashDiscount"].ToDecimalOrDefault();
            var cca = r["UnitCost"].ToDecimalOrDefault() * r["Quantity"].ToDecimalOrDefault() - ItemDiscount;//this.TotalDiscount;
            TotalItem += cca;
            var pDisc = cca * txtPercentageDiscount.Text.ToDecimalOrDefault() * 0.01m;
            r["TotalCostBeforTax"] = (r["UnitCost"].ToDecimalOrDefault() * r["Quantity"].ToDecimalOrDefault()) - (r["PercentageDiscount"].ToDecimalOrDefault() * r["Total"].ToDecimalOrDefault() * 0.01m) - r["CashDiscount"].ToDecimalOrDefault(); ;
            if (r["TaxOnInvoiceType"] != DBNull.Value && r["Tax_ID"] != DBNull.Value)
            {
                r["TotalTax"] = (((cca - pDisc - txtCashDiscount.Text.ToDecimalOrDefault() / this.dtItems.Rows.Count)) * TaxValue * 0.01m);
                if (r["TaxOnInvoiceType"].ToExpressString() == "C")
                {
                    this.dtAllTaxes.Rows.Add(r["TaxSalesAccountID"].ToInt(), r["TaxOnInvoiceType"].ToExpressString(), 0, r["TotalTax"].ToDecimalOrDefault());
                    this.TotalCreditTax += r["TotalTax"].ToDecimalOrDefault();
                }
                else
                {
                    this.dtAllTaxes.Rows.Add(r["TaxSalesAccountID"].ToInt(), r["TaxOnInvoiceType"].ToExpressString(), r["TotalTax"].ToDecimalOrDefault(), 0);
                    this.TotalDebitTax += r["TotalTax"].ToDecimalOrDefault();
                    r["TotalTax"] = r["TotalTax"].ToDecimalOrDefault() * -1;
                }
                this.TotalTax += r["TotalTax"].ToDecimalOrDefault();
            }

            r["GrossTotal"] = (r["UnitCostEvaluate"].ToDecimalOrDefault() * r["Quantity"].ToDecimal()
                                 - ItemDiscount
                                 + r["TotalTax"].ToDecimalOrDefault()).ToString(NbrHashNeerDecimal).ToDecimalOrDefault(); //- ItemDiscount //+ r["TotalTax"].ToDecimal();
            decimal gTotalRow = (
                                r["UnitCost"].ToDecimalOrDefault() * r["Quantity"].ToDecimal()
                              - ItemDiscount
                              + r["TotalTax"].ToDecimalOrDefault()).ToString(NbrHashNeerDecimal).ToDecimalOrDefault();

            r["GrossTotalVirtual"] = gTotalRow - ((gTotalRow * txtPercentageDiscount.Text.ToDecimalOrDefault() * 0.01m) + txtCashDiscount.Text.ToDecimalOrDefault()) / dtItems.Rows.Count;



            this.GrossTotal += r["Total"].ToDecimalOrDefault(); //- pDisc - ItemDiscount + r["TotalTax"].ToDecimal();

            int Item_IDD = Convert.ToInt32(r["Item_ID"].ToInt());
            var ObjTypeItems = dc.Items.FirstOrDefault(x => x.ID == Item_IDD);

            if (ObjTypeItems.Type.ToString() == "s")
            {
                this.TotalServices = Convert.ToDecimal((r["UnitCost"].ToDecimalOrDefault() * r["Quantity"].ToDecimal()).ToString(NbrHashNeerDecimal));
            }
            if (Convert.ToBoolean(r["IsGift"].ToBooleanOrDefault()) == true)
            {
                this.TotalGift += Convert.ToDecimal((r["UnitCost"].ToDecimalOrDefault() * r["Quantity"].ToDecimal()).ToString(NbrHashNeerDecimal));
            }
        }

        DocDiscount = (this.GrossTotal * txtPercentageDiscount.Text.ToDecimalOrDefault() * 0.01m) + txtCashDiscount.Text.ToDecimalOrDefault();

        //foreach (DataRow r in this.dtTaxes.Rows)
        //{
        //    if (r.RowState == DataRowState.Deleted) continue;
        //    if (r["OnInvoiceType"] != DBNull.Value)
        //    {
        //        DocTaxValue = ((this.GrossTotal - DocDiscount) * r["PercentageValue"].ToDecimal() * 0.01m);
        //        if (r["OnInvoiceType"].ToExpressString() == "C")
        //        {
        //            this.dtAllTaxes.Rows.Add(r["SalesAccountID"].ToInt(), r["OnInvoiceType"].ToExpressString(), 0, DocTaxValue);
        //            this.TotalCreditTax += DocTaxValue;
        //        }
        //        else
        //        {
        //            this.dtAllTaxes.Rows.Add(r["SalesAccountID"].ToInt(), r["OnInvoiceType"].ToExpressString(), DocTaxValue, 0);
        //            this.TotalDebitTax += DocTaxValue;
        //            DocTaxValue *= -1m;
        //        }
        //        DocTax += DocTaxValue;
        //    }
        //}





        Additionals = txtAdditionals.Text.ToDecimalOrDefault();
        this.TotalDiscount += DocDiscount;
        this.TotalTax += DocTax;
        this.GrossTotal = this.GrossTotal - TotalDiscount + this.TotalTax + Additionals;
        //lblTotal.Text = (this.Total - DocDiscount).ToString(NbrHashNeerDecimal);
        lblTotal.Text = (this.Total).ToString(MyContext.FormatNumber);
        lblTotalDiscount.Text = this.TotalDiscount.ToString(MyContext.FormatNumber);
        lblAdditionals.Text = Additionals.ToString(MyContext.FormatNumber);
        lblTotalTax.Text = this.TotalTax.ToString(MyContext.FormatNumber);
        lblGrossTotal.Text = this.GrossTotal.ToString(MyContext.FormatNumber);
        if ((acCashAccount.HasValue && Page.IsPostBack && this.SumFirstPaid) || this.IsCashInvoice)
        {
            // txtFirstPaid.Text = lblGrossTotal.Text;
            this.txtFirstPaid_TextChanged(null, null);
        }
        this.ConfirmationAnswered = false;
        this.ConfirmationMessage = string.Empty;
        this.ShowCustomerBalance();

    }

    protected void ddlTvae_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.Calculate();
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
    //        decimal taxInclude = (TypeTax == 2 ? Decimal.Divide(r["TaxPercentageValue"].ToDecimalOrDefault(), (100 + r["TaxPercentageValue"].ToDecimalOrDefault())) : 0);
    //        var uc = r["UnitCostEvaluate"].ToDecimalOrDefault();
    //        var taxUc = r["UnitCostEvaluate"].ToDecimalOrDefault() * taxInclude;
    //        r["UnitCost"] = uc - taxUc;


    //        r["Total"] = r["UnitCost"].ToDecimalOrDefault() * r["Quantity"].ToDecimal();
    //        Total += r["UnitCost"].ToDecimalOrDefault() * r["Quantity"].ToDecimal();
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
    //    lblTotal.Text = this.Total.ToString(NbrHashNeerDecimal);
    //    lblTotalDiscount.Text = this.TotalDiscount.ToString(NbrHashNeerDecimal);
    //    lblAdditionals.Text = Additionals.ToString(NbrHashNeerDecimal);
    //    lblTotalTax.Text = this.TotalTax.ToString(NbrHashNeerDecimal);
    //    lblGrossTotal.Text = this.GrossTotal.ToString(NbrHashNeerDecimal);
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
    private void FillItemeList_price()
    {

        var lstInvoices1 = dc.usp_GetItemPrice_Select(acItem.Value.ToInt()).ToList();
        this.dtItemePrice = lstInvoices1.CopyToDataTable();
        gvPriceList1.DataSource = this.dtItemePrice;
        gvPriceList1.DataBind();
        mpeCollect.Show();
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
        var str = dc.Stores.Where(c => c.ID == MyContext.UserProfile.Store_ID).FirstOrDefault();
        if (MyContext.UserProfile.Store_ID != null && str != null)
        {
            var lstQtyList = dc.usp_GetQtyItemeStoreGroup_Select(acItem.Value.ToInt()).Where(c => c.StoreName.Trim() == str.Name.Trim()).ToList();
            this.dtQtyItemeStoreGroup = lstQtyList.CopyToDataTable();
            gvQtyStoreList1.DataSource = this.dtQtyItemeStoreGroup;
            gvQtyStoreList1.DataBind();
            mpeGQiteme.Show();
        }
        else
        {
            var lstQtyList = dc.usp_GetQtyItemeStoreGroup_Select(acItem.Value.ToInt()).ToList();
            this.dtQtyItemeStoreGroup = lstQtyList.CopyToDataTable();
            gvQtyStoreList1.DataSource = this.dtQtyItemeStoreGroup;
            gvQtyStoreList1.DataBind();
            mpeGQiteme.Show();
        }

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
    private bool CheckCreditLmit()
    {

        if (this.ConfirmationAnswered) return true;
        var result = dc.fun_CheckCustomerCreditLimit(acCustomer.Value.ToInt(), this.GrossTotal - lblGrossTotal.Text.ToDecimalOrDefault()).Value;
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

    //private void InsertCashIn()
    //{
    //    string Serial = string.Empty;
    //    decimal ratio = txtRatio.Text.ToDecimal();
    //    int? CashIn_ID = null;
    //    if (lblGrossTotal.Text.ToDecimalOrDefault() <= 0) return;
    //    int ContactChartOfAccount_ID = dc.fun_getContactAccountID(acCustomer.Value.ToInt()).Value;
    //    CashIn_ID = dc.usp_Payments_Insert(txtOperationDate.Text.ToDate(), DateTime.Now, MyContext.UserProfile.Contact_ID, MyContext.UserProfile.Contact_ID, DateTime.Now, txtUserRefNo.TrimmedText, ref Serial, DocSerials.CashIn.ToInt(), txtNotes.Text, txtFirstPaid.Text.ToDecimal(), null, DocStatus.Approved.ToByte(), PaymentsTypes.CashInCustomer.ToByte(), acBranch.Value.ToNullableInt(), txtRatio.Text.ToDecimal(), ddlCurrency.SelectedValue.ToInt(), this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt(), this.DocRandomString + "_FromInvoice");
    //    if (!CashIn_ID.HasValue || CashIn_ID.Value <= 0) throw new Exception("Error Occured During inserting the cash document");
    //    dc.usp_PaymentsDetails_Insert(CashIn_ID, acCashAccount.Value.ToInt(), ContactChartOfAccount_ID, txtFirstPaid.Text.ToDecimal(), null, string.Empty, null);

    //    int Operation_ID = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref Serial, DocStatus.Approved.ToByte(), OperationTypes.CashIn.ToInt(), ddlCurrency.SelectedValue.ToInt(), txtFirstPaid.Text.ToDecimal() * ratio, txtFirstPaid.Text.ToDecimal(), ratio, null);
    //    dc.usp_OperationDetails_Insert(Operation_ID, ContactChartOfAccount_ID, 0, txtFirstPaid.Text.ToDecimal() * ratio, 0, txtFirstPaid.Text.ToDecimal(), null, CashIn_ID, DocumentsTableTypes.Payment_CashIn.ToInt());
    //    dc.usp_OperationDetails_Insert(Operation_ID, acCashAccount.Value.ToInt(), txtFirstPaid.Text.ToDecimal() * ratio, 0, txtFirstPaid.Text.ToDecimal(), 0, null, CashIn_ID, DocumentsTableTypes.Payment_CashIn.ToInt());
    //    dc.usp_SetCashDocForBills(this.Invoice_ID, CashIn_ID, DocumentsTableTypes.Invoice.ToInt());
    //}

    private void InsertCashIn(int account_ID)
    {
        string Serial = string.Empty;
        decimal ratio = txtRatio.Text.ToDecimal();
        int? CashIn_ID = null;
        if (lblGrossTotal.Text.ToDecimalOrDefault() <= 0) return;
        int ContactChartOfAccount_ID = dc.fun_getContactAccountID(acCustomer.Value.ToInt()).Value;
        CashIn_ID = dc.usp_Payments_Insert(txtOperationDate.Text.ToDate(), DateTime.Now, MyContext.UserProfile.Contact_ID, MyContext.UserProfile.Contact_ID, DateTime.Now, txtUserRefNo.TrimmedText, ref Serial, DocSerials.CashIn.ToInt(), txtNotes.Text, lblGrossTotal.Text.ToDecimal(), null, DocStatus.Approved.ToByte(), PaymentsTypes.CashInCustomer.ToByte(), acBranch.Value.ToNullableInt(), txtRatio.Text.ToDecimal(), ddlCurrency.SelectedValue.ToInt(), this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt(), this.DocRandomString + "_FromInvoice");
        if (!CashIn_ID.HasValue || CashIn_ID.Value <= 0) throw new Exception("Error Occured During inserting the cash document");
        dc.usp_PaymentsDetails_Insert(CashIn_ID, account_ID, ContactChartOfAccount_ID, lblGrossTotal.Text.ToDecimal(), null, string.Empty, null);

        int Operation_ID = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref Serial, DocStatus.Approved.ToByte(), OperationTypes.CashIn.ToInt(), ddlCurrency.SelectedValue.ToInt(), lblGrossTotal.Text.ToDecimal() * ratio, lblGrossTotal.Text.ToDecimal(), ratio, null);
        dc.usp_OperationDetails_Insert(Operation_ID, ContactChartOfAccount_ID, 0, lblGrossTotal.Text.ToDecimal() * ratio, 0, lblGrossTotal.Text.ToDecimal(), null, CashIn_ID, DocumentsTableTypes.Payment_CashIn.ToInt());
        dc.usp_OperationDetails_Insert(Operation_ID, account_ID, lblGrossTotal.Text.ToDecimal() * ratio, 0, lblGrossTotal.Text.ToDecimal(), 0, null, CashIn_ID, DocumentsTableTypes.Payment_CashIn.ToInt());
        dc.usp_SetCashDocForBills(this.Invoice_ID, CashIn_ID, DocumentsTableTypes.Invoice.ToInt());
    }


    private void InsertBankIn(int account_ID)
    {
        string Serial = string.Empty;
        decimal ratio = txtRatio.Text.ToDecimal();
        int? BankIn_ID = null;
        if (lblGrossTotal.Text.ToDecimalOrDefault() <= 0) return;
        int ContactChartOfAccount_ID = dc.fun_getContactAccountID(acCustomer.Value.ToInt()).Value;
        BankIn_ID = dc.usp_Payments_Insert(txtOperationDate.Text.ToDate(), DateTime.Now, MyContext.UserProfile.Contact_ID, MyContext.UserProfile.Contact_ID, DateTime.Now, txtUserRefNo.TrimmedText, ref Serial, DocSerials.BankDeposit.ToInt(), txtNotes.Text, lblGrossTotal.Text.ToDecimal(), null, DocStatus.Approved.ToByte(), PaymentsTypes.BankDepositCustomer.ToByte(), acBranch.Value.ToNullableInt(), txtRatio.Text.ToDecimal(), ddlCurrency.SelectedValue.ToInt(), this.Invoice_ID, DocumentsTableTypes.Invoice.ToInt(), this.DocRandomString + "_FromInvoice");
        if (!BankIn_ID.HasValue || BankIn_ID.Value <= 0) throw new Exception("Error Occured During inserting the cash document");
        dc.usp_PaymentsDetails_Insert(BankIn_ID, account_ID, ContactChartOfAccount_ID, lblGrossTotal.Text.ToDecimal(), null, string.Empty, null);

        int Operation_ID = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref Serial, DocStatus.Approved.ToByte(), OperationTypes.BankDeposit.ToInt(), ddlCurrency.SelectedValue.ToInt(), lblGrossTotal.Text.ToDecimal() * ratio, lblGrossTotal.Text.ToDecimal(), ratio, null);
        dc.usp_OperationDetails_Insert(Operation_ID, ContactChartOfAccount_ID, 0, lblGrossTotal.Text.ToDecimal() * ratio, 0, lblGrossTotal.Text.ToDecimal(), null, BankIn_ID, DocumentsTableTypes.Payment_BankDeposit.ToInt());
        dc.usp_OperationDetails_Insert(Operation_ID, account_ID, lblGrossTotal.Text.ToDecimal() * ratio, 0, lblGrossTotal.Text.ToDecimal(), 0, null, BankIn_ID, DocumentsTableTypes.Payment_BankDeposit.ToInt());
        dc.usp_SetCashDocForBills(this.Invoice_ID, BankIn_ID, DocumentsTableTypes.Invoice.ToInt());
    }


    private void ShowCustomerBalance()
    {
        int? CustomerAccountID = (acCustomer.HasValue) ? dc.fun_getContactAccountID(acCustomer.Value.ToInt()) : (int?)null;
        decimal Balance = (CustomerAccountID == null) ? 0.0000m : dc.fun_GetAccountBalanceInForeign(CustomerAccountID, txtOperationDate.Text.ToDate(), acBranch.Value.ToNullableInt()).Value;
        lblCustomerBalanceBefore.Text = (Balance - (this.DocStatus_ID == 2 ? lblGrossTotal.Text.ToDecimalOrDefault() - lblGrossTotal.Text.ToDecimalOrDefault() : 0)).ToString(MyContext.FormatNumber);
        lblCustomerBalanceAfter.Text = (lblCustomerBalanceBefore.Text.ToDecimalOrDefault() + lblGrossTotal.Text.ToDecimalOrDefault() - lblGrossTotal.Text.ToDecimalOrDefault()).ToString(MyContext.FormatNumber);
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
            // txtFirstPaid.Visible = txtItemNotes.Visible = acCategory.Visible = !this.IsCashInvoice;

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
        lblTotalRow.Text = Math.Round((string.IsNullOrEmpty(acItemTax.Value) ? calc : (calc + val1)), 2).ToExpressString();
        lblTotalRowBeforTax.Text = Math.Round(calc, 3).ToExpressString();




    }

    private void LoadPaymntMethode()
    {

        //txtFirstPaid.Text = "";
        //txtFirstPaid.Enabled = true;
        //if (ddlPaymentMethode.SelectedIndex != 0)
        //{
        //    var paymentMethods = dc.SettingPointOs.Where(c => c.IsActive.Value).ToList();
        //    var IDPM = ddlPaymentMethode.SelectedValue.ToIntOrDefault();
        //    var ChoosedPayMethodes_ID = paymentMethods.Where(c => c.ID == IDPM).FirstOrDefault();
        //    if (ChoosedPayMethodes_ID != null)
        //    {
        //        if (ChoosedPayMethodes_ID.IsCash.ToBooleanOrDefault())
        //        {
        //            txtFirstPaid.Text = lblGrossTotal.Text;
        //            txtFirstPaid.Enabled = false;
        //        }
        //        else
        //        {
        //            txtFirstPaid.Text = lblGrossTotal.Text;

        //            txtFirstPaid.Enabled = false;
        //            txtFirstPaid.IsRequired = true;
        //        }
        //    }
        //}
        //else
        //{
        //    txtFirstPaid.IsRequired = false;
        //    txtFirstPaid.Enabled = false;
        //}
    }


    private bool SaveAccounting(bool IsApproving, System.Data.Common.DbTransaction trans)
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

        //if (txtFirstPaid.Text.ToDecimalOrDefault() > Math.Round(this.GrossTotal, 4))
        //{
        //    UserMessages.Message(null, Resources.UserInfoMessages.FirstPaidGreaterThanTotal, string.Empty);
        //    trans.Rollback();
        //    return false;
        //}

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
        var cmp = dc.usp_Company_Select().FirstOrDefault();

        decimal firstpaid = 0;

        if (ddlPaymentMethode.SelectedValue != "-1")
        {
            var paymentMethods = dc.SettingPointOs.Where(c => c.IsActive.Value).ToList();
            var IDPM = ddlPaymentMethode.SelectedValue.ToIntOrDefault();
            var ChoosedPayMethodes_ID = paymentMethods.Where(c => c.ID == IDPM).FirstOrDefault();
            if (ChoosedPayMethodes_ID != null)
            {
                // var isCash = ChoosedPayMethodes_ID.IsCash.ToBooleanOrDefault();
                // if (isCash)
                // {
                firstpaid = lblGrossTotal.Text.ToDecimalOrDefault();
                // }
            }
        }


        if (!this.EditMode)
        {

            this.Invoice_ID = dc.usp_InvoiceDateDelivery_Insert(acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimalOrDefault(), txtOperationDate.Text.ToDate(),
                                                     acCustomer.Value.ToInt(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate,
                                                     approvedBY_ID, null, null, acAddress.Value.ToNullableInt(), acShipAddress.Value.ToNullableInt(), acPaymentAddress.Value.ToNullableInt(),
                                                     acTelephone.Value.ToNullableInt(), acCostCenter.Value.ToNullableInt(), txtNotes.Text, lblTotal.Text.ToDecimal(), txtPercentageDiscount.Text.ToDecimalOrDefault(),
                                                     txtCashDiscount.Text.ToDecimalOrDefault(), txtAdditionals.Text.ToDecimalOrDefault(), lblTotalDiscount.Text.ToDecimalOrDefault(), lblTotalTax.Text.ToDecimalOrDefault(),
                                                     lblGrossTotal.Text.ToDecimalOrDefault(), firstpaid, null, txtUserRefNo.Text, this.DocRandomString, EntryType, SalesOrderID, acSalesRep.Value.ToNullableInt(), acCashAccount.Value.ToNullableInt(), acCustomerMesure.Value.ToNullableInt(), txtDeliveryDate.Text.ToDate(), acdrivers.Value.ToNullableInt(),
                                                      ddlTvae.SelectedValue.ToNullableInt(), 0, txtCustomerName.Text,string.Empty, txtCustomerMobile.Text, txtProjectRef.Text,
                                                      txtContactPerson.Text,string.Empty, txtCustomerRepresentative.Text, ddlPaymentMethode.SelectedValue.ToIntOrDefault(), "");
            if (this.Invoice_ID > 0)
            {
                foreach (DataRow r in this.dtItems.Rows)
                {
                    if (r.RowState == DataRowState.Deleted) continue;
                    Detail_ID = dc.usp_InvoiceDetailsIncludeTax_Insert(this.Invoice_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["Capacity"].ToString(), r["ItemDescription"].ToString(), r["IDCodeOperation"].ToString(), r["Policy"].ToString(), r["InvoiceDate"].ToDate(), r["Capacities"].ToString(), r["IsGift"].ToBooleanOrDefault(), r["UnitCostEvaluate"].ToDecimalOrDefault(),
                        false,null,null);


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
            //int Result = dc.usp_InvoiceDelivery_Update(this.Invoice_ID, acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimalOrDefault(), txtOperationDate.Text.ToDate(),
            //                                        acCustomer.Value.ToInt(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate, approvedBY_ID,
            //                                        null, null, acAddress.Value.ToNullableInt(), acShipAddress.Value.ToNullableInt(), acPaymentAddress.Value.ToNullableInt(), acTelephone.Value.ToNullableInt(),
            //                                        acCostCenter.Value.ToNullableInt(), txtNotes.Text, lblTotal.Text.ToDecimal(), txtPercentageDiscount.Text.ToDecimalOrDefault(), txtCashDiscount.Text.ToDecimalOrDefault(), txtAdditionals.Text.ToDecimalOrDefault(),
            //                                        lblTotalDiscount.Text.ToDecimalOrDefault(), lblTotalTax.Text.ToDecimalOrDefault(), lblGrossTotal.Text.ToDecimalOrDefault(), firstpaid, txtUserRefNo.Text, this.DocRandomString, acSalesRep.Value.ToNullableInt(), acCashAccount.Value.ToNullableInt(), txtDeliveryDate.Text.ToDate(), acdrivers.Value.ToNullableInt(),
            //                                         txtProjectRef.Text, txtContactPerson.Text, txtCustomerRepresentative.Text);


            int Result = dc.usp_InvoiceDelivery_Update(this.Invoice_ID, acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimalOrDefault(), txtOperationDate.Text.ToDate(),
                                                   acCustomer.Value.ToInt(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate, approvedBY_ID,
                                                   null, null, acAddress.Value.ToNullableInt(), acShipAddress.Value.ToNullableInt(), acPaymentAddress.Value.ToNullableInt(), acTelephone.Value.ToNullableInt(),
                                                   acCostCenter.Value.ToNullableInt(), txtNotes.Text, lblTotal.Text.ToDecimal(), txtPercentageDiscount.Text.ToDecimalOrDefault(), txtCashDiscount.Text.ToDecimalOrDefault(), txtAdditionals.Text.ToDecimalOrDefault(),
                                                   lblTotalDiscount.Text.ToDecimalOrDefault(), lblTotalTax.Text.ToDecimalOrDefault(), lblGrossTotal.Text.ToDecimalOrDefault(), firstpaid, txtUserRefNo.Text, this.DocRandomString, acSalesRep.Value.ToNullableInt(), acCashAccount.Value.ToNullableInt(), txtDeliveryDate.Text.ToDate(), acdrivers.Value.ToNullableInt(),
                                                   ddlTvae.SelectedValue.ToNullableInt(), 0, txtCustomerName.Text,string.Empty, txtCustomerMobile.Text, txtProjectRef.Text, txtContactPerson.Text,string.Empty, txtCustomerRepresentative.Text);

            if (Result > 0)
            {
                foreach (DataRow r in this.dtItems.Rows)
                {
                    if (r.RowState == DataRowState.Added)
                    {
                        Detail_ID = dc.usp_InvoiceDetailsIncludeTax_Insert(this.Invoice_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), r["UnitCost"].ToDecimal(), r["Quantity"].ToDecimal(), r["Uom_iD"].ToInt(), r["Batch_ID"].ToNullableInt(), r["Tax_ID"].ToNullableInt(), r["PercentageDiscount"].ToDecimalOrDefault(), r["CashDiscount"].ToDecimalOrDefault(), r["TotalTax"].ToDecimalOrDefault(), r["Notes"].ToExpressString(), r["QtyInNumber"].ToDecimalOrDefault(), r["Capacity"].ToString(), r["ItemDescription"].ToString(), r["IDCodeOperation"].ToString(), r["Policy"].ToString(), r["InvoiceDate"].ToDate(), r["Capacities"].ToString(), r["IsGift"].ToBooleanOrDefault(), r["UnitCostEvaluate"].ToDecimalOrDefault(),
                            false,null,null);

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
            mpeConfirmCollection.Show();
            trans.Rollback();
            return false;
        }

        Serial = Serial == string.Empty ? string.Empty : " (" + Serial + ") ";
        // UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess + Serial, PageLinks.InvoiceShortcut + Request.PathInfo + "?ID=" + this.Invoice_ID.ToExpressString(), PageLinks.InvoicesList, PageLinks.InvoiceShortcut + Request.PathInfo);
        UserMessages.MessageWithPtint(null, Resources.UserInfoMessages.OperationSuccess + Serial, PageLinks.InvoiceShortcut + Request.PathInfo + "?ID=" + this.Invoice_ID.ToExpressString(), PageLinks.InvoicesList, PageLinks.InvoiceShortcut + Request.PathInfo, "~/Report_Dev/PrintInnoiceeDev.aspx?Invoice_ID=" + this.Invoice_ID + "&IsMaterla=1");

        return true;
    }

    #endregion

    #region Import Invoice From Excel
    protected void btnImportFromFile_Click(object sender, EventArgs e)
    {
        ImportCorrection();
    }
    private DataTable dtFile
    {
        get
        {
            return (DataTable)Session["dtFile" + this.WinID];
        }

        set
        {
            Session["dtFile" + this.WinID] = value;
        }
    }
    private DataTable dtAllAdded
    {
        get
        {
            if (Session["dtAllAdded" + this.WinID] == null)
            {
                DataTable dtallTaxes = new DataTable();
                dtallTaxes.Columns.Add("ID", typeof(int));
                dtallTaxes.Columns.Add("Name", typeof(string));
                dtallTaxes.Columns.Add("Name_ID", typeof(int));
                dtallTaxes.Columns.Add("Value", typeof(string));
                dtallTaxes.Columns.Add("Value_ID", typeof(int));
                Session["dtAllAdded" + this.WinID] = dtallTaxes;
            }
            return (DataTable)Session["dtAllAdded" + this.WinID];
        }

        set
        {
            Session["dtAllTaxes_Invoice" + this.WinID] = value;
        }
    }
    private string NameFile
    {
        get
        {
            if (ViewState["NameFile"] == null) return string.Empty;
            return (string)ViewState["NameFile"];
        }

        set
        {
            ViewState["NameFile"] = value;
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        var lstHeader = new List<ExportClass>();

        string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
        string fileName = "ExcelData" + DateTime.Now.Ticks + Extension;
        FileUpload1.SaveAs(Server.MapPath("~\\uploads\\Excel\\" + fileName));
        string ExcelFilePath = Server.MapPath("~\\uploads\\Excel\\" + fileName);

        dtFile = new DataTable();
        byte[] bin = File.ReadAllBytes(ExcelFilePath);
        using (MemoryStream stream = new MemoryStream(bin))

            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                    {
                        try
                        {
                            var rowExcp = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column];
                            int oo = 0;
                            foreach (var cell in rowExcp)
                            {
                                var t = new ExportClass() { ID = oo++, Name = cell.Text };
                                lstHeader.Add(t);
                            }

                            if (rbHDR.SelectedIndex == 0)
                            {
                                foreach (var cell in rowExcp)
                                {
                                    dtFile.Columns.Add(new DataColumn(cell.Text));
                                }
                            }
                            else
                            {
                                for (int k = worksheet.Dimension.Start.Column; k <= worksheet.Dimension.End.Column; k++)
                                {
                                    dtFile.Columns.Add(new DataColumn("F" + k.ToString()));

                                }
                            }

                            for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                            {
                                var row = worksheet.Cells[i, 1, i, worksheet.Dimension.End.Column];

                                DataRow newRow = dtFile.NewRow();

                                foreach (var cell in row)
                                {
                                    newRow[cell.Start.Column - 1] = cell.Text;
                                }

                                dtFile.Rows.Add(newRow);
                            }
                        }
                        catch (Exception exDetails)
                        {

                            UserMessages.Message(null, "يجب ان يكون شيت واحد", string.Empty);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                UserMessages.Message(null, "يجب ان يكون نوع الاكسل .xlsx", string.Empty);
            }

        ddlPropertiesValue.DataSource = lstHeader;
        ddlPropertiesValue.DataValueField = "ID";
        ddlPropertiesValue.DataTextField = "Name";
        ddlPropertiesValue.DataBind();


        GridView1.DataSource = dtFile;
        GridView1.DataBind();
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.DataSource = dtFile;

        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();
    }
    protected void Button17_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlPropertiesValue.SelectedValue))
        {
            DataRow[] filteredRows = this.dtAllAdded.Select("Name_ID='" + ddlProperties.SelectedValue + "'");
            if (filteredRows.Length > 0)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                return;
            }

            DataRow r = null;
            r = this.dtAllAdded.NewRow();
            r["ID"] = this.dtAllAdded.GetID("ID");
            r["Name"] = ddlProperties.SelectedItem.Text; ;
            r["Name_ID"] = ddlProperties.SelectedValue.ToInt();
            r["Value"] = ddlPropertiesValue.SelectedItem.Text;
            r["Value_ID"] = ddlPropertiesValue.SelectedValue.ToInt();
            this.dtAllAdded.Rows.Add(r);
            this.BindItemsGrid1();
        }
        else
        {
            UserMessages.Message(null, "بيانات غير مكتملة", string.Empty);
        }

    }
    private bool ImportCorrection()
    {

        var its = dc.Items.ToList();
        var Units = dc.usp_GeneralAttributes_Select(0, 14).ToList();

        for (int i = 0; i < dtFile.Rows.Count; i++)
        {
            var row = dtFile.Rows[i];

            //Qauntity Propertie
            DataRow[] filteredRows = this.dtAllAdded.Select("Name_ID=1");

            var obj = filteredRows[0];
            var indexQty = (obj["Value_ID"].ToExpressString()).ToInt();



            //Barcode Propertie
            DataRow[] filteredStoreRows = this.dtAllAdded.Select("Name_ID=0");

            var objStore = filteredStoreRows[0];
            var index = (objStore["Value_ID"].ToExpressString()).ToInt();



            //unit Propertie
            DataRow[] filteredUnitRows = this.dtAllAdded.Select("Name_ID=2");

            var objUnit = filteredUnitRows[0];
            var indexUnit = (objUnit["Value_ID"].ToExpressString()).ToInt();


            //unit Propertie
            DataRow[] filteredCostRows = this.dtAllAdded.Select("Name_ID=3");

            var objCost = filteredCostRows[0];
            var indexCost = (objCost["Value_ID"].ToExpressString()).ToInt();




            var valueBarcode = row[index].ToExpressString();
            var valueQty = row[indexQty].ToDecimalOrDefault();
            var valueUnit = row[indexUnit].ToExpressString();
            var valueCost = row[indexCost].ToDecimalOrDefault();


            var item = its.Where(c => c.Barcode == valueBarcode || c.Name.Contains(valueBarcode)).FirstOrDefault();


            if (item != null)
            {


                DataRow r = null;
                r = this.dtItems.NewRow();



                r["SerialNumber"] = txtSerialNumber.Text;
                r["Store_ID"] = acStore.Value;
                r["Category_ID"] = item.Category_ID;
                r["Item_ID"] = item.ID;
                r["ItemName"] = item.Name;

                r["IDCodeOperation"] = txtCode.Text;
                r["Policy"] = txtPolicy.Text;
                r["Capacity"] = txtCapacity.Text;
                r["Capacities"] = txtCapacities.Text;
                r["ItemDescription"] = acItemDescribed.Value;
                r["DescribedName"] = acItemDescribed.Text;
                if (valueCost > 0)
                {
                    r["UnitCost"] = valueCost;
                    r["UnitCostEvaluate"] = valueCost;

                }
                else
                {
                    r["UnitCost"] = item.Cost;
                    r["UnitCostEvaluate"] = item.Cost;

                }

                r["Quantity"] = valueQty;
                r["QtyInNumber"] = 0;

                if (valueUnit == string.Empty)
                {
                    var uName = Units.Where(c => c.ID == item.UOM_ID.ToInt()).FirstOrDefault().Name;
                    r["UOMName"] = uName;
                    r["Uom_ID"] = item.UOM_ID;
                }
                else
                {
                    var unt = Units.Where(c => c.Name == valueUnit).ToList();
                    if (unt.Any())
                    {
                        r["Uom_ID"] = unt.First().ID;
                        r["UOMName"] = valueUnit;
                    }
                    else
                    {
                        var uName = Units.Where(c => c.ID == item.UOM_ID.ToInt()).FirstOrDefault().Name;
                        r["UOMName"] = uName;
                        r["Uom_ID"] = item.UOM_ID;
                    }
                }


                r["PercentageDiscount"] = 0;
                r["CashDiscount"] = 0;
                r["TotalTax"] = 0;
                r["Notes"] = "";
                //r["StoreName"] = acStore.Text;
                r["Barcode"] = valueBarcode;

                var Tax = dc.usp_Taxes_Select(item.Tax_ID, string.Empty).FirstOrDefault();

                decimal taxInclude = TypeTax == 2 ? (Decimal.Divide((Tax != null ? Tax.PercentageValue.Value : 0), (100M + (Tax != null ? Tax.PercentageValue.Value : 0)))) : Decimal.Divide((Tax != null ? Tax.PercentageValue.Value : 0), (100M));
                //decimal unitCost = TypeTax == 2 ? (item.Cost.ToDecimalOrDefault() - item.Cost.ToDecimalOrDefault() * taxInclude) : item.Cost.ToDecimalOrDefault();
                //r["TotalCostBeforTax"] = Math.Round(unitCost * valueQty.ToDecimalOrDefault(), 3).ToDecimalOrDefault();





                //need Options
                r["InvoiceDate"] = txtOperationDate.Text.ToDate();

                if (item.Tax_ID != null)
                {

                    r["TaxName"] = Tax.Name;
                    r["Tax_ID"] = item.Tax_ID;
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
            this.Calculate();
            BindItemsGrid();
        }
        return true;


    }
    private void BindItemsGrid1()
    {

        gvExportList.DataSource = this.dtAllAdded;
        gvExportList.DataBind();

    }
    #endregion

}

public class ClsIyrad
{
    public int Item_ID { get; set; }
    public decimal UniteCost { get; set; }
    public int? Account_ID { get; set; }
    public decimal Qty { get; set; }
}

