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
using QRCoder;

public partial class Contacts_Customers : UICulturePage
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

    private DataTable dtContactMesure
    {
        get
        {
            if (Session["dtContactMesure" + this.WinID] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("Length", typeof(string));
                dt.Columns.Add("Shoulder", typeof(string));
                dt.Columns.Add("LengthSleeve", typeof(string));
                dt.Columns.Add("SizeChest", typeof(string));
                dt.Columns.Add("Neek", typeof(string));
                dt.Columns.Add("SizeHand", typeof(string));
                dt.Columns.Add("CupLength", typeof(string));
                dt.Columns.Add("Elbow", typeof(string));
                dt.Columns.Add("DateInformation", typeof(DateTime));
                Session["dtContactMesure" + this.WinID] = dt;
            }
            return (DataTable)Session["dtContactMesure" + this.WinID];
        }

        set
        {
            Session["dtContactMesure" + this.WinID] = value;
        }
    }

    private DataTable dtContactPrice
    {
        get
        {
            if (Session["dtContactPrice" + this.WinID] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("Contact_ID", typeof(int));
                dt.Columns.Add("Item_ID", typeof(int));
                dt.Columns.Add("Price", typeof(decimal));
                dt.Columns.Add("CreatedBy_ID", typeof(int));
                dt.Columns.Add("CreatedDate", typeof(DateTime));
                dt.Columns.Add("IsActive", typeof(bool));
                dt.Columns.Add("Name", typeof(string));
                Session["dtContactPrice" + this.WinID] = dt;
            }
            return (DataTable)Session["dtContactPrice" + this.WinID];
        }

        set
        {
            Session["dtContactPrice" + this.WinID] = value;
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


    //public void  GenerateQRCode(string name)
    //{
    //    string code = name;
    //    QRCodeGenerator qrGenerator = new QRCodeGenerator();
    //    QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
    //    System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
    //    imgBarCode.Height = 150;
    //    imgBarCode.Width = 150;
    //    using (Bitmap bitMap = qrCode.GetGraphic(20))
    //    {
    //        using (MemoryStream ms = new MemoryStream())
    //        {
    //            bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
    //            byte[] byteImage = ms.ToArray();
    //            imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
    //        }
    //        PlaceHolder1.Controls.Add(imgBarCode);
    //    }
    //}


    #region Page Event
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
                if (this.EditMode) this.FillCustomerData();
            }

            ucNav.SourceDocTypeType_ID = 90;
            ucNav.EntryType = 1;

            ucNav.Res_ID = this.Contact_ID;




        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    #endregion

    #region Conrtol Events

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

    protected void btnSaveCustomer_click(object sender, EventArgs e)
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
                this.Contact_ID = dc.usp_ContactWithTax_Insert(acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), DocSerials.Customer.ToInt(), txtName.TrimmedText, 'C', txtNotes.Text, this.ImageUrl, txtTaxNumber.TrimmedText,
                    TxtRegisterNo.Text.Trim(),TxtBuildingNo.Text.Trim(),TxtAddressAr.Text.Trim(),TxtAddressOther.Text.Trim(),TxtStreetNameAr.Text.Trim(),TxtStreetNameOther.Text.Trim(),TxtDistrictAr.Text.Trim(),TxtDistrictOther.Text.Trim(),
                    TxtCityAr.Text.Trim(),TxtCityOther.Text.Trim(),TxtCountryAr.Text.Trim(),TxtCountryOther.Text.Trim(),TxtPostalCode.Text.Trim(),TxtAdditonalNo.Text.Trim(),TxtOtherBuyerId.Text.Trim());

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
                dc.usp_Customers_Insert(this.Contact_ID, ChartofAccount_ID, acArea.Value.ToNullableInt(), acShipVia.Value.ToNullableInt(), ddlApplyCreditLimit.SelectedValue.ToBoolean(), txtCreditLimitValue.Text.ToDecimal());
                if (chkCustomer.Checked)
                {
                    dc.usp_Vendors_Insert(this.Contact_ID, ChartofAccount_ID, acShipVia.Value.ToNullableInt(), ddlApplyCreditLimit.SelectedValue.ToBoolean(), txtCreditLimitValue.Text.ToDecimalOrDefault());

                }

                var ctn = dc.Contacts.Where(x => x.ID == this.Contact_ID).FirstOrDefault();
                ctn.NBRTax = txtTaxNumber.TrimmedText;
                ctn.Price_ID = acPriceName.Value.ToNullableInt();
                dc.SubmitChanges();

            }
            else // update
            {
                var ctn = dc.Contacts.Where(x => x.ID == this.Contact_ID).FirstOrDefault();
                ctn.NBRTax = txtTaxNumber.TrimmedText;
                ctn.Price_ID = acPriceName.Value.ToNullableInt();
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


                var CustomersChartofAccount_ID = dc.Customers.Where(x => x.Contact_ID == this.Contact_ID).FirstOrDefault();


                var ChartofAccountCustomers_ID = dc.ChartOfAccounts.Where(x => x.ID == CustomersChartofAccount_ID.ChartOfAccount_ID.Value.ToInt()).FirstOrDefault();
                ChartofAccountCustomers_ID.Parent_ID = acParentAccount.Value.ToNullableInt();
                dc.SubmitChanges();



                int result = dc.usp_Customers_Update(this.Contact_ID, acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimalOrDefault(), txtName.TrimmedText, txtNotes.Text, this.ImageUrl, acArea.Value.ToNullableInt(), ddlApplyCreditLimit.SelectedValue.ToBoolean(), txtCreditLimitValue.Text.ToDecimalOrDefault(), acShipVia.Value.ToNullableInt(), txtOpenBalance.Text.ToDecimalOrDefault(), txtStartFrom.Text.ToDate(), acOppsiteAccount.Value.ToNullableInt());
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

                    var obj = dc.Vendors.Where(x => x.Contact_ID == this.Contact_ID).FirstOrDefault();
                    if (obj != null)
                    { //Vendor
                        int resultVendor = dc.usp_Vendors_Update(this.Contact_ID, acBranch.Value.ToNullableInt(), ddlCurrency.SelectedValue.ToInt(), txtRatio.Text.ToDecimalOrDefault(), txtName.TrimmedText, txtNotes.Text, this.ImageUrl, ddlApplyCreditLimit.SelectedValue.ToBoolean(), txtCreditLimitValue.Text.ToDecimalOrDefault(), acShipVia.Value.ToNullableInt(), txtOpenBalance.Text.ToDecimalOrDefault(), txtStartFrom.Text.ToDate(), acOppsiteAccount.Value.ToNullableInt());


                        if (resultVendor == -2)
                        {
                            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                            trans.Rollback();
                            return;
                        }
                        if (resultVendor == -30)
                        {
                            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.InvalidParentBranch, string.Empty);
                            trans.Rollback();
                            return;
                        }

                    }
                    else
                    {
                        var objectCustomer = dc.Customers.Where(x => x.Contact_ID == this.Contact_ID).FirstOrDefault();
                        dc.usp_Vendors_Insert(this.Contact_ID, objectCustomer.ChartOfAccount_ID, acShipVia.Value.ToNullableInt(), ddlApplyCreditLimit.SelectedValue.ToBoolean(), txtCreditLimitValue.Text.ToDecimalOrDefault());

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

            foreach (DataRow r in dtContactMesure.Rows)
            {
                switch (r.RowState)
                {
                    case DataRowState.Deleted:
                        dc.usp_ContactMesure_Delete(r["ID", DataRowVersion.Original].ToInt());
                        break;
                    case DataRowState.Added:
                        dc.usp_ContactMesure_insert(this.Contact_ID, r["Length"].ToString(), r["Shoulder"].ToString(),
                            r["Neek"].ToString(), r["LengthSleeve"].ToString(), r["SizeChest"].ToString(),
                            r["SizeHand"].ToString(), r["CupLength"].ToString(), r["Elbow"].ToString(),
                            r["DateInformation"].ToDate());
                        break;
                    case DataRowState.Modified:
                        dc.usp_ContactMesure_update(int.Parse(r["ID"].ToString()), r["Length"].ToString(), r["Shoulder"].ToString(),
                               r["Neek"].ToString(), r["LengthSleeve"].ToString(), r["SizeChest"].ToString(),
                               r["SizeHand"].ToString(), r["CupLength"].ToString(), r["Elbow"].ToString(),
                               r["DateInformation"].ToDate());
                        break;
                }
            }

            foreach (DataRow r in this.dtContactPrice.Rows)
            {
                if (r.RowState == DataRowState.Deleted)
                {
                    dc.usp_ContactPrice_Delete(r["ID", DataRowVersion.Original].ToInt());
                }
                else if (r.RowState == DataRowState.Added)
                {
                    var res = dc.usp_ContactPrice_INSERT(this.Contact_ID, r["Item_ID"].ToInt(), r["Price"].ToDecimalOrDefault(), r["CreatedBy_ID"].ToInt(), r["CreatedDate"].ToDate());
                    if (res == -1)
                    {
                        UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                        trans.Rollback();
                        return;
                    }
                }
                else if (r.RowState == DataRowState.Modified)
                {
                    var res = dc.usp_ContactPrice_Update(r["ID", DataRowVersion.Original].ToInt(), this.Contact_ID, r["Item_ID"].ToInt(), r["Price"].ToDecimalOrDefault());
                    if (res == -1)
                    {
                        UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                        trans.Rollback();
                        return;
                    }

                }
            }


            LogAction(this.EditMode ? Actions.Edit : Actions.Add, txtName.Text, dc);
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, PageLinks.Customers + "?ID=" + this.Contact_ID.ToExpressString(), PageLinks.CustomersList, PageLinks.Customers);
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
            Response.Redirect(PageLinks.CustomersList, false);
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


    protected void btnAddMesures_OnClick(object sender, EventArgs e)
    {
        try
        {
            DataRow r = null;
            if (this.EditID == 0)
            {
                //txtDateInformation 
                //this.dtContactMesure.Rows.Add(this.dtContactMesure.GetID("ID"),
                //    txtLength.Text,
                //    txtShoulder.Text,
                //    txtLengthSleeve.TrimmedText,
                //    txtSizeChest.Text,
                //    txtNeek.TrimmedText,
                //    txtSizeHand.TrimmedText,
                //    txtCupLength.TrimmedText,
                //    txtElbow.TrimmedText,
                //    DateTime.Now
                //    );

                r = this.dtContactMesure.NewRow();
                r["ID"] = this.dtContactMesure.GetID("ID");
                r["Length"] = txtLength.Text;
                r["Shoulder"] = txtShoulder.Text;
                r["LengthSleeve"] = txtLengthSleeve.TrimmedText;
                r["SizeChest"] = txtSizeChest.Text;
                r["Neek"] = txtNeek.TrimmedText;
                r["SizeHand"] = txtSizeHand.TrimmedText;
                r["CupLength"] = txtCupLength.TrimmedText;
                r["Elbow"] = txtElbow.TrimmedText;
                r["DateInformation"] = DateTime.Now;
                this.dtContactMesure.Rows.Add(r);


            }
            else
            {
                r = this.dtContactMesure.Select("ID=" + this.EditID)[0];
                r["Length"] = txtLength.Text;
                r["Shoulder"] = txtShoulder.Text;
                r["LengthSleeve"] = txtLengthSleeve.TrimmedText;
                r["SizeChest"] = txtSizeChest.Text;
                r["Neek"] = txtNeek.TrimmedText;
                r["SizeHand"] = txtSizeHand.TrimmedText;
                r["CupLength"] = txtCupLength.TrimmedText;
                r["Elbow"] = txtElbow.TrimmedText;
                this.EditID = 0;
            }
            gvContactMesure.DataSource = this.dtContactMesure;
            gvContactMesure.DataBind();

            txtLength.Clear();
            txtShoulder.Clear();
            txtLengthSleeve.Clear();
            txtNeek.Clear();
            txtSizeHand.Clear();
            txtCupLength.Clear();
            txtElbow.Clear();

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvContactMesure_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ID = gvContactMesure.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dtContactMesure.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            gvContactMesure.DataSource = this.dtContactMesure;
            gvContactMesure.DataBind();
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvContactMesure_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvContactMesure.PageIndex = e.NewPageIndex;
            gvContactMesure.DataSource = this.dtContactMesure;
            gvContactMesure.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvContactMesure_OnSelectedIndexChanged(object sender, EventArgs e)
    {


    }

    protected void gvContactMesure_OnSelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            var dataKey = gvContactMesure.DataKeys[e.NewSelectedIndex];
            if (dataKey != null)
                this.EditID = dataKey["ID"].ToInt();
            var r = this.dtContactMesure.Select("ID=" + this.EditID.ToExpressString())[0];
            txtLength.Text = r["Length"].ToExpressString();
            txtShoulder.Text = r["Shoulder"].ToExpressString();
            txtLengthSleeve.Text = r["LengthSleeve"].ToExpressString();
            txtSizeChest.Text = r["SizeChest"].ToExpressString();
            txtNeek.Text = r["Neek"].ToExpressString();
            txtSizeHand.Text = r["SizeHand"].ToExpressString();
            txtCupLength.Text = r["CupLength"].ToExpressString();
            txtElbow.Text = r["Elbow"].ToExpressString();


        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }




    protected void btnPriceSpec_Click(object sender, EventArgs e)
    {
        try
        {
            DataRow r = null;
            if (this.EditID == 0)
            {
                r = this.dtContactPrice.NewRow();
                r["ID"] = this.dtContactPrice.GetID("ID");
                r["Contact_ID"] = this.Contact_ID;
                r["Item_ID"] = acNameSrch.Value.ToIntOrDefault();
                r["Price"] = txtPrice.Text.ToDecimalOrDefault();
                r["CreatedBy_ID"] = MyContext.UserProfile.Contact_ID;
                r["CreatedDate"] = DateTime.Now;
                r["Name"] = acNameSrch.Text;
                this.dtContactPrice.Rows.Add(r);
            }
            else
            {
                r = this.dtContactPrice.Select("ID=" + this.EditID)[0];
                r["Contact_ID"] = this.Contact_ID;
                r["Item_ID"] = acNameSrch.Value.ToIntOrDefault();
                r["Price"] = txtPrice.Text.ToDecimalOrDefault();
                r["Name"] = acNameSrch.Text;
                this.EditID = 0;
            }
            gvPriceSpec.DataSource = this.dtContactPrice;
            gvPriceSpec.DataBind();

            txtPrice.Text = string.Empty;
            acNameSrch.Clear();

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void gvPriceSpec_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ID = gvPriceSpec.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dtContactPrice.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            gvPriceSpec.DataSource = this.dtContactPrice;
            gvPriceSpec.DataBind();
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void gvPriceSpec_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvPriceSpec.PageIndex = e.NewPageIndex;
            gvPriceSpec.DataSource = this.dtContactPrice;
            gvPriceSpec.DataBind();
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
        //TODO dtContactMesure
        this.dtContactMesure = null;
        this.dtContactPrice = null;
        acArea.ContextKey = string.Empty;
        acParentAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString();
        acOppsiteAccount.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",,," + COA.Capital.ToInt().ToExpressString() + ",true";
        acOppsiteAccount.Value = COA.Capital.ToInt().ToExpressString();
        acParentAccount.Value = COA.Customers.ToInt().ToExpressString();
        acBranch.ContextKey = string.Empty;
        acShipVia.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.ShipVia.ToInt().ToExpressString();
        acContactDataType.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.ContactData.ToInt().ToExpressString();
        acPriceName.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.Prices.ToInt().ToExpressString();

        ddlCurrency.DataSource = dc.usp_Currency_Select(false);
        ddlCurrency.DataTextField = "Name";
        ddlCurrency.DataValueField = "ID";
        ddlCurrency.DataBind();
        txtStartFrom.Text = this.MyContext.FiscalYearStartDate.ToString("d/M/yyyy");
        this.txtStartFrom_TextChanged(null, null);
        var comp = dc.usp_Company_Select().FirstOrDefault();
        txtCreditLimitValue.Text = comp.CustomerCreditLimit.ToExpressString();
        ddlApplyCreditLimit.SelectedValue = comp.UseCustomerCreditLimit.ToExpressString();
        if (MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = MyContext.UserProfile.Branch_ID.ToExpressString();
            //acBranch.Enabled = false;
        }

        //context  Items Product
        acNameSrch.ContextKey = ",,";
    }

    private void SetEditMode()
    {
        if (Request["ID"] != null)
        {
            this.EditMode = true;
            this.Contact_ID = Request["ID"].ToInt();
        }
    }

    private void FillCustomerData()
    {
        var customer = dc.usp_Customer_SelectByID(this.Contact_ID).FirstOrDefault();
        txtName.Text = customer.Name;
        txtTaxNumber.Text = customer.NBRTax;
        acBranch.Value = customer.Branch_ID.ToExpressString();
        ddlCurrency.SelectedValue = customer.Currency_ID.ToExpressString();
        acArea.Value = customer.Area_ID.ToExpressString();
        acShipVia.Value = customer.ShipVia.ToExpressString();
        ddlApplyCreditLimit.SelectedValue = customer.UseCreditLimit.ToExpressString();
        txtCreditLimitValue.Text = customer.CreditLimit.ToExpressString();

        TxtRegisterNo.Text = customer.RegisterNo;
        TxtBuildingNo.Text = customer.BuildingNo;
        TxtStreetNameAr.Text = customer.StreetNameAr;
        TxtStreetNameOther.Text = customer.StreetNameOther;
        TxtDistrictAr.Text = customer.DistrictAr;
        TxtDistrictOther.Text = customer.DistrictOther;
        TxtCityAr.Text = customer.CityAr;
        TxtCityOther.Text = customer.CityOther;
        TxtCountryAr.Text = customer.CountryAr;
        TxtCountryOther.Text = customer.CountryOther;
        TxtPostalCode.Text = customer.PostalCode;
        TxtAdditonalNo.Text = customer.AdditionalNo;
        TxtOtherBuyerId.Text = customer.OtherBuyerId;

        if (customer.OpenBalance.HasValue) txtOpenBalance.Text = customer.OpenBalance.ToExpressString();
        this.txtOpenBalance_TextChanged(null, null);
        if (customer.OpenBalanceDate.HasValue) txtStartFrom.Text = customer.OpenBalanceDate.Value.ToString("d/M/yyyy");
        this.txtStartFrom_TextChanged(null, null);
        if (customer.Ratio.HasValue) txtRatio.Text = customer.Ratio.Value.ToExpressString();
        if (File.Exists(Server.MapPath("~/Uploads/" + customer.Photo))) imgLogo.ImageUrl = "~/Uploads/" + customer.Photo;
        txtNotes.Text = customer.Notes;

        acParentAccount.Enabled = acOppsiteAccount.Enabled = false;
        //acBranch.Enabled = 
            ddlCurrency.Enabled = txtStartFrom.Enabled = txtOpenBalance.Enabled = !customer.LockCustomer.Value;
        acParentAccount.Value = customer.Parent_ID.ToExpressString();

        this.dtContactData = dc.usp_ContactDetails_Select(this.Contact_ID, this.MyContext.CurrentCulture.ToByte()).CopyToDataTable();
        gvContactData.DataSource = this.dtContactData;
        gvContactData.DataBind();

        this.dtContactPrice = dc.usp_ContactPrice_Select(this.Contact_ID, null).CopyToDataTable();
        gvPriceSpec.DataSource = this.dtContactPrice;
        gvPriceSpec.DataBind();
        //TODO dtContactMesure
        this.dtContactMesure = dc.usp_ContactMesure_Select(this.Contact_ID).CopyToDataTable(); //dc.ContactMesures.Where(x => x.Contact_ID == this.Contact_ID).CopyToDataTable();
        gvContactMesure.DataSource = this.dtContactMesure;
        gvContactMesure.DataBind();



        var ctn = dc.Contacts.Where(x => x.ID == this.Contact_ID).FirstOrDefault();
        acPriceName.Value = ctn.Price_ID.ToExpressString();
        string TestQrCode = "اسم العميل :" + customer.Name;
      //  GenerateQRCode(TestQrCode);



        var vendor = dc.Vendors.Where(x => x.Contact_ID == this.Contact_ID).FirstOrDefault();
        if (vendor != null)
        {
            chkCustomer.Checked = true;
        }
    }

    private void CheckSecurity()
    {
        if (!this.MyContext.PageData.IsViewDoc && this.EditMode) Response.Redirect(PageLinks.Authorization, true);
        btnSaveCustomer.Visible = (this.EditMode && this.MyContext.PageData.IsEdit) || (!this.EditMode && this.MyContext.PageData.IsAdd);
    }

    private void CustomPage()
    {
        acBranch.Visible = MyContext.Features.BranchesEnabled;
        acArea.Visible = MyContext.Features.AreasEnabled;

    }

    #endregion

    protected void gvPriceSpec_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            var dataKey = gvPriceSpec.DataKeys[e.NewSelectedIndex];
            if (dataKey != null)
                this.EditID = dataKey["ID"].ToInt();
            var r = this.dtContactPrice.Select("ID=" + this.EditID.ToExpressString())[0];
            txtPrice.Text = r["Price"].ToExpressString();
            acNameSrch.Value = r["Item_ID"].ToExpressString();


        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
}