using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

public partial class Contacts_Vendors : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtContactData
    {
        get
        {
            if (Session["dtContactData" + this.WinID] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("Att_ID", typeof(int));
                dt.Columns.Add("AttName", typeof(string));
                dt.Columns.Add("Data", typeof(string));
                Session["dtContactData" + this.WinID] = dt;
            }
            return (DataTable)Session["dtContactData" + this.WinID];
        }

        set
        {
            Session["dtContactData" + this.WinID] = value;
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

    private int Contact_ID
    {
        get
        {
            if (ViewState["Contact_ID"] == null) return 0;
            return (int)ViewState["Contact_ID"];
        }

        set
        {
            ViewState["Contact_ID"] = value;
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

    #endregion

    #region Conrtol Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnUploadImage);
            this.SetEditMode();
            this.CheckSecurity();

            if (!Page.IsPostBack)
            {
                this.LoadControls();
                if (this.EditMode) this.FillVendorData();
            }

            ucNav.SourceDocTypeType_ID = 90;
            ucNav.EntryType = 2;
            ucNav.Res_ID = this.Contact_ID;
            ucNav.btnHandler += new CustomControls_ucNavigation.OnButtonClick(ucNav_btnHandler);
            ucNav.btnHandlerPrev += new CustomControls_ucNavigation.OnButtonClick(ucNav_btnHandlerPrev);
            ucNav.btnHandlerFirst += new CustomControls_ucNavigation.OnButtonClick(ucNav_btnHandlerFirst);
            ucNav.btnHandlerLast += new CustomControls_ucNavigation.OnButtonClick(ucNav_btnHandlerLast);
            ucNav.btnHandlerAddNew += new CustomControls_ucNavigation.OnButtonClick(ucNav_btnHandlerAddNew);
            ucNav.btnHandlerSearch += new CustomControls_ucNavigation.OnButtonClick(ucNav_btnHandlerSearch);

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    void ucNav_btnHandler(string strValue)
    {

        RefillForm(strValue);
    }
    void ucNav_btnHandlerPrev(string strValue)
    {
        RefillForm(strValue);
    }
    void ucNav_btnHandlerFirst(string strValue)
    {
        RefillForm(strValue);
    }
    void ucNav_btnHandlerLast(string strValue)
    {
        RefillForm(strValue);
    }
    void ucNav_btnHandlerAddNew(string strValue)
    {
        Response.Redirect(PageLinks.Vendors);
    }
    void ucNav_btnHandlerSearch(string strValue)
    {
        RefillForm(strValue);
    }

    private void RefillForm(string strValue)
    {
        if (!string.IsNullOrEmpty(strValue))
        {
            this.Contact_ID = strValue.ToInt();
            // this.EditMode = strValue.ToInt();
            this.EditMode = true;

            this.LoadControls();
            if (this.EditMode) this.FillVendorData();
        }
    }



    protected void Page_PreRender(object sender, EventArgs e)
    {
        this.CustomPage();
    }

    protected void txtStartFrom_TextChanged(object sender, EventArgs e)
    {
        try
        {
            var ratio = dc.fun_GetCurrentRatio(ddlCurrency.SelectedValue.ToInt(), txtStartFrom.Text.ToDate());
            txtRatio.Text = ratio == null ? string.Empty : ratio.ToExpressString();
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void txtOpenBalance_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //acBranch.IsRequired = 
                txtRatio.IsRequired = txtStartFrom.IsRequired = txtOpenBalance.IsNotEmpty;
            if (sender != null) this.FocusNextControl(sender);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSaveVendor_click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        int ChartofAccount_ID = 0;
        try
        {
            if (txtStartFrom.Text.ToDate() > DateTime.Now.Date && txtOpenBalance.Text.ToDecimalOrDefault() > 0)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
                trans.Rollback();
                return;
            }
            if (!this.EditMode) //insert
            {
                this.Contact_ID = dc.usp_ContactWithTax_Insert(acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), DocSerials.Vendor.ToInt(), txtName.TrimmedText, 'V', txtNotes.Text, this.ImageUrl, txtTaxNumber.TrimmedText,
                    TxtRegisterNo.Text.Trim(), TxtBuildingNo.Text.Trim(), TxtAddressAr.Text.Trim(), TxtAddressOther.Text.Trim(), TxtStreetNameAr.Text.Trim(), TxtStreetNameOther.Text.Trim(), TxtDistrictAr.Text.Trim(), TxtDistrictOther.Text.Trim(),
                    TxtCityAr.Text.Trim(), TxtCityOther.Text.Trim(), TxtCountryAr.Text.Trim(), TxtCountryOther.Text.Trim(), TxtPostalCode.Text.Trim(), TxtAdditonalNo.Text.Trim(), TxtOtherBuyerId.Text.Trim());

                ChartofAccount_ID = dc.usp_ChartOfAccount_Insert(txtName.TrimmedText, txtName.TrimmedText, acParentAccount.Value.ToInt(), true, acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimalOrDefault(), txtOpenBalance.Text.ToDecimalOrDefault(), txtStartFrom.Text.ToDate(), acOppsiteAccount.Value.ToNullableInt());

                if (ChartofAccount_ID == -2 || this.Contact_ID == -2)
                {
                    UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                    trans.Rollback();
                    return;
                }
                if (ChartofAccount_ID == -30)
                {
                    UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.InvalidParentBranch, string.Empty);
                    trans.Rollback();
                    return;
                }
                dc.usp_Vendors_Insert(this.Contact_ID, ChartofAccount_ID, acShipVia.Value.ToNullableInt(), ddlApplyCreditLimit.SelectedValue.ToBoolean(), txtCreditLimitValue.Text.ToDecimalOrDefault());

                if (chkCustomer.Checked)
                {
                    dc.usp_Customers_Insert(this.Contact_ID, ChartofAccount_ID, acArea.Value.ToNullableInt(), acShipVia.Value.ToNullableInt(), ddlApplyCreditLimit.SelectedValue.ToBoolean(), txtCreditLimitValue.Text.ToDecimal());

                }
            }
            else // update
            {
                var ctn = dc.Contacts.Where(x => x.ID == this.Contact_ID).FirstOrDefault();
                ctn.NBRTax = txtTaxNumber.TrimmedText;
                ctn.RegisterNo = TxtRegisterNo.Text;
                ctn.BuildingNo = TxtBuildingNo.Text;
                ctn.StreetNameAr = TxtStreetNameAr.Text;
                ctn.StreetNameOther = TxtStreetNameOther.Text;
                ctn.DistrictAr = TxtDistrictAr.Text;
                ctn.DistrictOther = TxtDistrictOther.Text;
                ctn.CityAr = TxtCityAr.Text;
                ctn.CityOther = TxtCityOther.Text;
                ctn.CountryAr = TxtCountryAr.Text;
                ctn.CountryOther = TxtCountryOther.Text;
                ctn.PostalCode = TxtPostalCode.Text;
                ctn.AdditionalNo = TxtAdditonalNo.Text;
                ctn.OtherBuyerId = TxtOtherBuyerId.Text;
                dc.SubmitChanges();



                var VenderChartofAccount_ID = dc.Vendors.Where(x => x.Contact_ID == this.Contact_ID).FirstOrDefault();


                var ChartofAccountvender_ID = dc.ChartOfAccounts.Where(x => x.ID == VenderChartofAccount_ID.ChartOfAccount_ID.Value.ToInt()).FirstOrDefault();
                ChartofAccountvender_ID.Parent_ID = acParentAccount.Value.ToNullableInt();
                dc.SubmitChanges();


                int result = dc.usp_Vendors_Update(this.Contact_ID, acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimalOrDefault(), txtName.TrimmedText, txtNotes.Text, this.ImageUrl, ddlApplyCreditLimit.SelectedValue.ToBoolean(), txtCreditLimitValue.Text.ToDecimalOrDefault(), acShipVia.Value.ToNullableInt(), txtOpenBalance.Text.ToDecimalOrDefault(), txtStartFrom.Text.ToDate(), acOppsiteAccount.Value.ToNullableInt());
                if (result == -2)
                {
                    UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                    trans.Rollback();
                    return;
                }
                if (result == -30)
                {
                    UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.InvalidParentBranch, string.Empty);
                    trans.Rollback();
                    return;
                }


                if (chkCustomer.Checked)
                {
                    var listCustomers = dc.Customers.Where(x => x.Contact_ID == this.Contact_ID).ToList();

                    var obj = listCustomers.FirstOrDefault(); //dc.usp_Customer_SelectByID(this.Contact_ID);
                    if (obj != null)
                    {
                        int resultCustomer = dc.usp_Customers_Update(this.Contact_ID, acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimalOrDefault(), txtName.TrimmedText, txtNotes.Text, this.ImageUrl, acArea.Value.ToNullableInt(), ddlApplyCreditLimit.SelectedValue.ToBoolean(), txtCreditLimitValue.Text.ToDecimalOrDefault(), acShipVia.Value.ToNullableInt(), txtOpenBalance.Text.ToDecimalOrDefault(), txtStartFrom.Text.ToDate(), acOppsiteAccount.Value.ToNullableInt());
                        if (resultCustomer == -2)
                        {
                            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                            trans.Rollback();
                            return;
                        }
                        if (resultCustomer == -30)
                        {
                            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.InvalidParentBranch, string.Empty);
                            trans.Rollback();
                            return;
                        }
                    }
                    else
                    {
                        var objectVendor = dc.Vendors.Where(x => x.Contact_ID == this.Contact_ID).FirstOrDefault();

                        dc.usp_Customers_Insert(this.Contact_ID, objectVendor.ChartOfAccount_ID, acArea.Value.ToNullableInt(), acShipVia.Value.ToNullableInt(), ddlApplyCreditLimit.SelectedValue.ToBoolean(), txtCreditLimitValue.Text.ToDecimal());

                    }
                }

            }

            foreach (DataRow r in this.dtContactData.Rows)
            {
                if (r.RowState == DataRowState.Deleted)
                {
                    dc.usp_ContactDetails_Delete(r["ID", DataRowVersion.Original].ToInt());
                }
                else if (r.RowState == DataRowState.Added)
                {
                    dc.usp_ContactDetails_insert(this.Contact_ID, r["Att_ID"].ToInt(), r["Data"].ToExpressString());
                }
            }
            LogAction(this.EditMode ? Actions.Edit : Actions.Add, txtName.Text, dc);
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, PageLinks.Vendors + "?ID=" + this.Contact_ID.ToExpressString(), PageLinks.VendorsList, PageLinks.Vendors);
            trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnReturn_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(PageLinks.VendorsList, false);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvContactData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvContactData.PageIndex = e.NewPageIndex;
            gvContactData.DataSource = this.dtContactData;
            gvContactData.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvContactData_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ID = gvContactData.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dtContactData.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            gvContactData.DataSource = this.dtContactData;
            gvContactData.DataBind();
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnAddContactDetail_click(object sender, EventArgs e)
    {
        try
        {
            this.dtContactData.Rows.Add(this.dtContactData.GetID("ID"), acContactDataType.Value, acContactDataType.Text, txtContactData.TrimmedText);
            gvContactData.DataSource = this.dtContactData;
            gvContactData.DataBind();
            acContactDataType.Clear();
            txtContactData.Clear();
            acContactDataType.AutoCompleteFocus();
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
            this.ImageUrl = fileName;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void ucNewAtt_NewAttributeCreated(string AttName, int AttID)
    {
        try
        {
            acShipVia.Refresh();
            acShipVia.Value = AttID.ToExpressString();
            this.FocusNextControl(lnkAddNewAtt);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Private Methods

    private void LoadControls()
    {
        this.dtContactData = null;
        acParentAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString();
        acOppsiteAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",,," + COA.Capital.ToInt().ToExpressString() + ",true";
        acOppsiteAccount.Value = COA.Capital.ToInt().ToExpressString();
        acParentAccount.Value = COA.Vendors.ToInt().ToExpressString();
        acBranch.ContextKey = string.Empty;
        acShipVia.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.ShipVia.ToInt().ToExpressString();
        acContactDataType.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.ContactData.ToInt().ToExpressString();

        ddlCurrency.DataSource = dc.usp_Currency_Select(false);
        ddlCurrency.DataTextField = "Name";
        ddlCurrency.DataValueField = "ID";
        ddlCurrency.DataBind();
        txtStartFrom.Text = this.MyContext.FiscalYearStartDate.ToString("d/M/yyyy");
        this.txtStartFrom_TextChanged(null, null);
        if (MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = MyContext.UserProfile.Branch_ID.ToExpressString();
            //acBranch.Enabled = false;
        }
        //var comp = dc.usp_Company_Select().FirstOrDefault();
        //txtCreditLimitValue.Text = comp.VendorCreditLimit.ToExpressString();
        //ddlApplyCreditLimit.SelectedValue = comp.UseVendorCreditLimit.ToExpressString();
    }

    private void SetEditMode()
    {
        if (Request["ID"] != null)
        {
            this.EditMode = true;
            this.Contact_ID = Request["ID"].ToInt();
        }
    }

    private void FillVendorData()
    {
        var Vendor = dc.usp_Vendor_SelectByID(this.Contact_ID).FirstOrDefault();
        txtName.Text = Vendor.Name;
        txtTaxNumber.Text = Vendor.NBRTax;
        acBranch.Value = Vendor.Branch_ID.ToExpressString();
        ddlCurrency.SelectedValue = Vendor.Currency_ID.ToExpressString();
        acShipVia.Value = Vendor.ShipVia.ToExpressString();
        ddlApplyCreditLimit.SelectedValue = Vendor.UseCreditLimit.ToExpressString();
        txtCreditLimitValue.Text = Vendor.CreditLimit.ToExpressString();

        TxtRegisterNo.Text=Vendor.RegisterNo;
        TxtBuildingNo.Text = Vendor.BuildingNo;
        TxtStreetNameAr.Text = Vendor.StreetNameAr;
        TxtStreetNameOther.Text = Vendor.StreetNameOther;
        TxtDistrictAr.Text = Vendor.DistrictAr;
        TxtDistrictOther.Text = Vendor.DistrictOther;
        TxtCityAr.Text = Vendor.CityAr;
        TxtCityOther.Text = Vendor.CityOther;
        TxtCountryAr.Text = Vendor.CountryAr;
        TxtCountryOther.Text = Vendor.CountryOther;
        TxtPostalCode.Text = Vendor.PostalCode;
        TxtAdditonalNo.Text = Vendor.AdditionalNo;
        TxtOtherBuyerId.Text = Vendor.OtherBuyerId;

        if (Vendor.OpenBalance.HasValue) txtOpenBalance.Text = Vendor.OpenBalance.ToExpressString();
        this.txtOpenBalance_TextChanged(null, null);
        if (Vendor.OpenBalanceDate.HasValue) txtStartFrom.Text = Vendor.OpenBalanceDate.Value.ToString("d/M/yyyy");
        this.txtStartFrom_TextChanged(null, null);
        if (Vendor.Ratio.HasValue) txtRatio.Text = Vendor.Ratio.Value.ToExpressString();
        if (File.Exists(Server.MapPath("~/Uploads/" + Vendor.Photo))) imgLogo.ImageUrl = "~/Uploads/" + Vendor.Photo;
        txtNotes.Text = Vendor.Notes;

        acParentAccount.Enabled = false;
        //acBranch.Enabled = 
            ddlCurrency.Enabled = txtStartFrom.Enabled = txtOpenBalance.Enabled = !Vendor.LockVendor.Value;
        acParentAccount.Value = Vendor.Parent_ID.ToExpressString();

        this.dtContactData = dc.usp_ContactDetails_Select(this.Contact_ID, this.MyContext.CurrentCulture.ToByte()).CopyToDataTable();
        gvContactData.DataSource = this.dtContactData;
        gvContactData.DataBind();

        var customer = dc.Customers.Where(x => x.Contact_ID == this.Contact_ID).FirstOrDefault();
        if (customer != null)
        {
            chkCustomer.Checked = true;
        }

    }

    private void CheckSecurity()
    {
        if (!this.MyContext.PageData.IsViewDoc && this.EditMode) Response.Redirect(PageLinks.Authorization, true);
        btnSaveVendor.Visible = (this.EditMode && this.MyContext.PageData.IsEdit) || (!this.EditMode && this.MyContext.PageData.IsAdd);
    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;

    }

    #endregion
}