using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using XPRESS.Common;

public partial class Inv_Items : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

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

    private int ItemMaterial_ID
    {
        get
        {
            if (ViewState["ItemMaterial_ID"] == null) return 0;
            return (int)ViewState["ItemMaterial_ID"];
        }

        set
        {
            ViewState["ItemMaterial_ID"] = value;
        }
    }
    private int ItemCompose_ID
    {
        get
        {
            if (ViewState["ItemCompose_ID"] == null) return 0;
            return (int)ViewState["ItemCompose_ID"];
        }

        set
        {
            ViewState["ItemCompose_ID"] = value;
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

    private DataTable dtPrices
    {
        get
        {
            if (Session["dtPrices" + this.WinID] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("PriceName_ID", typeof(int));
                dt.Columns.Add("Price", typeof(decimal));
                dt.Columns.Add("PriceName", typeof(string));
                dt.Columns.Add("UOM_ID", typeof(string));
                dt.Columns.Add("UOMName", typeof(string));
                Session["dtPrices" + this.WinID] = dt;
            }
            return (DataTable)Session["dtPrices" + this.WinID];
        }

        set
        {
            Session["dtPrices" + this.WinID] = value;
        }
    }

    private DataTable dtUnits
    {
        get
        {
            if (Session["dtUnits" + this.WinID] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("Unit_ID", typeof(int));
                dt.Columns.Add("Ratio", typeof(decimal));
                dt.Columns.Add("UnitName", typeof(string));
                dt.Columns.Add("Price", typeof(decimal));
                dt.Columns.Add("Barcode", typeof(string));
                dt.Columns.Add("IsFavorite", typeof(bool));
                Session["dtUnits" + this.WinID] = dt;
            }
            return (DataTable)Session["dtUnits" + this.WinID];
        }

        set
        {
            Session["dtUnits" + this.WinID] = value;
        }
    }

    private DataTable dtRawMat
    {
        get
        {
            if (Session["dtRawMat" + this.WinID] == null)
            {
                Session["dtRawMat" + this.WinID] = dc.usp_ItemsMaterials_Select(null).CopyToDataTable();
            }
            return (DataTable)Session["dtRawMat" + this.WinID];
        }

        set
        {
            Session["dtRawMat" + this.WinID] = value;
        }
    }

    private DataTable dtRawCompose
    {
        get
        {
            if (Session["dtRawCompose" + this.WinID] == null)
            {
                Session["dtRawCompose" + this.WinID] = dc.usp_ItemsCompose_Select(null).CopyToDataTable();
            }
            return (DataTable)Session["dtRawCompose" + this.WinID];
        }

        set
        {
            Session["dtRawCompose" + this.WinID] = value;
        }
    }

    private DataTable dtCommission
    {
        get
        {
            if (Session["dtCommission" + this.WinID] == null)
            {
                Session["dtCommission" + this.WinID] = dc.usp_ItemsCommission_Select(null).CopyToDataTable();
            }
            return (DataTable)Session["dtCommission" + this.WinID];
        }

        set
        {
            Session["dtCommission" + this.WinID] = value;
        }
    }


    private DataTable dtItemdescribed
    {
        get
        {
            if (Session["dtItemdescribed" + this.WinID] == null)
            {
                Session["dtItemdescribed" + this.WinID] = dc.usp_ItemDescribed_Select(null).CopyToDataTable();
            }
            return (DataTable)Session["dtItemdescribed" + this.WinID];
        }

        set
        {
            Session["dtItemdescribed" + this.WinID] = value;
        }
    }


    private DataTable dtMinQty
    {
        get
        {
            if (Session["dtMinQty" + this.WinID] == null)
            {
                Session["dtMinQty" + this.WinID] = dc.usp_ItemsUnderLimits_Select(null).CopyToDataTable();
            }
            return (DataTable)Session["dtMinQty" + this.WinID];
        }

        set
        {
            Session["dtMinQty" + this.WinID] = value;
        }
    }

    private bool IsLockedItem
    {
        get
        {
            if (ViewState["IsLockedItem"] == null) return false;
            return (bool)ViewState["IsLockedItem"];
        }

        set
        {
            ViewState["IsLockedItem"] = value;
        }
    }

    private int ItemUnit_ID
    {
        get
        {
            if (ViewState["ItemUnit_ID"] == null) return 0;
            return (int)ViewState["ItemUnit_ID"];
        }

        set
        {
            ViewState["ItemUnit_ID"] = value;
        }
    }


    #endregion

    #region Page Events
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
                if (this.EditMode) this.FillItemData();


                var its = dc.Items.Where(c => c.ID == this.Item_ID).ToList();
                if (its.Any())
                {
                    var it = its.First();
                    chkItemCuisine.Checked = (it.IsCuisine != null ? it.IsCuisine.Value : false);
                    CheHideItem.Checked = (it.HideItem != null ? it.HideItem.Value : false);
                    // acPrinterName.Value = it.PrinterName_ID .ToExpressString();
                    txtQtyItems.Text = it.PrinterName_ID.ToExpressString();
                    acAccountRelated.Value = it.Account_ID.ToExpressString();
                    chkIsBalance.Checked = it.IsItemBalance.ToBooleanOrDefault();
                }
            }
            ucNav.SourceDocTypeType_ID = 0;

            //ucNav.btnHandler += new CustomControls_ucNavigation.OnButtonClick(ucNav_btnHandler);
            //ucNav.btnHandlerPrev += new CustomControls_ucNavigation.OnButtonClick(ucNav_btnHandlerPrev);
            //ucNav.btnHandlerFirst += new CustomControls_ucNavigation.OnButtonClick(ucNav_btnHandlerFirst);
            //ucNav.btnHandlerLast += new CustomControls_ucNavigation.OnButtonClick(ucNav_btnHandlerLast);
            //ucNav.btnHandlerAddNew += new CustomControls_ucNavigation.OnButtonClick(ucNav_btnHandlerAddNew);
            //ucNav.btnHandlerSearch += new CustomControls_ucNavigation.OnButtonClick(ucNav_btnHandlerSearch);



            if (this.Item_ID == 0)
            {
                btnSaveDescribred.Enabled = false;
                lblExplain.Visible = true;
            }
            else
            {
                btnSaveDescribred.Enabled = true;
                lblExplain.Visible = false;
            }



        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }


    //void ucNav_btnHandler(string strValue)
    //{

    //    RefillForm(strValue);
    //}
    //void ucNav_btnHandlerPrev(string strValue)
    //{
    //    RefillForm(strValue);
    //}
    //void ucNav_btnHandlerFirst(string strValue)
    //{
    //    RefillForm(strValue);
    //}
    //void ucNav_btnHandlerLast(string strValue)
    //{
    //    RefillForm(strValue);
    //}
    //void ucNav_btnHandlerAddNew(string strValue)
    //{
    //    Response.Redirect(PageLinks.Items);
    //}
    //void ucNav_btnHandlerSearch(string strValue)
    //{
    //    RefillForm(strValue);
    //}


    //private void RefillForm(string strValue)
    //{
    //    if (!string.IsNullOrEmpty(strValue))
    //    {
    //        this.Item_ID = strValue.ToInt();
    //        // this.EditMode = strValue.ToInt();
    //        this.EditMode = true;

    //        this.LoadControls();
    //        if (this.EditMode) this.FillItemData();


    //        var its = dc.Items.Where(c => c.ID == this.Item_ID).ToList();
    //        if (its.Any())
    //        {
    //            var it = its.First();
    //            chkItemCuisine.Checked = (it.IsCuisine != null ? it.IsCuisine.Value : false);
    //            // acPrinterName.Value = it.PrinterName_ID .ToExpressString();
    //            txtQtyItems.Text = it.PrinterName_ID.ToExpressString();

    //        }
    //    }
    //}





    protected void Page_PreRender(object sender, EventArgs e)
    {
        this.CustomPage();
    }
    #endregion

    #region Controls Events

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

    protected void ucNewPrice_NewAttributeCreated(string AttName, int AttID)
    {
        try
        {
            acPriceName.Refresh();
            acPriceName.Value = AttID.ToExpressString();
            this.FocusNextControl(lnkAddNewPriceName);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void ucNewUnit_NewAttributeCreated(string AttName, int AttID)
    {
        try
        {
            acSmallestUnit.Refresh();
            acBiggerUnit.Refresh();
            //if (acSmallestUnit.Enabled) 
            acSmallestUnit.Value = AttID.ToExpressString();
            this.FocusNextControl(lnkbtnAddNewUnit);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void ucNewBiggerUnit_NewAttributeCreated(string AttName, int AttID)
    {
        try
        {
            acSmallestUnit.Refresh();
            acBiggerUnit.Refresh();
            if (acBiggerUnit.Enabled) acBiggerUnit.Value = AttID.ToExpressString();
            this.FocusNextControl(lnkAddNewBiggerUnit);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnaddPrice_click(object sender, EventArgs e)
    {
        try
        {
            var isExists = (from data in this.dtPrices.AsEnumerable() where data.RowState != DataRowState.Deleted && data.Field<int>("Uom_ID") == acUom_ID.Value.ToInt() && data.Field<int>("PriceName_ID") == acPriceName.Value.ToInt() select data).Any();
            if (isExists)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.PriceExists, string.Empty);
                return;
            }
            this.dtPrices.Rows.Add(this.dtPrices.GetID("ID"), acPriceName.Value.ToInt(), txtPrice.Text.ToDecimal(), acPriceName.Text, acUom_ID.Value.ToIntOrDefault(), acUom_ID.Text.ToExpressString());
            gvPrices.DataSource = this.dtPrices;
            gvPrices.DataBind();
            acPriceName.Clear();
            txtPrice.Clear();
            acPriceName.AutoCompleteFocus();

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvUnits_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvUnits.PageIndex = e.NewPageIndex;
            gvUnits.DataSource = this.dtUnits;
            gvUnits.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvUnits_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ID = gvUnits.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dtUnits.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            gvUnits.DataSource = this.dtUnits;
            gvUnits.DataBind();
            // acSmallestUnit.Enabled = (gvUnits.Rows.Count == 0 && !this.IsLockedItem);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvPrices_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvPrices.PageIndex = e.NewPageIndex;
            gvPrices.DataSource = this.dtPrices;
            gvPrices.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvPrices_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ID = gvPrices.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dtPrices.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            gvPrices.DataSource = this.dtPrices;
            gvPrices.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnAddUnit_click(object sender, EventArgs e)
    {
        try
        {
            if (!acSmallestUnit.HasValue)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.SmallestUnitRequired, string.Empty);
                return;
            }

            if (this.ItemUnit_ID == 0)
            {
                var isExists = (from data in this.dtUnits.AsEnumerable() where data.RowState != DataRowState.Deleted && data.Field<int>("Unit_ID") == acBiggerUnit.Value.ToInt() select data).Any();
                if (isExists || acSmallestUnit.Value.ToIntOrDefault() == acBiggerUnit.Value.ToInt())
                {
                    UserMessages.Message(null, Resources.UserInfoMessages.UnitExists, string.Empty);
                    return;
                }
                this.dtUnits.Rows.Add(this.dtUnits.GetID("ID"), acBiggerUnit.Value.ToInt(), txtUnitRatio.Text.ToDecimal(), acBiggerUnit.Text, txtPriceUnite.Text.ToDecimal(), txtBarcodeUnite.Text, chkIsFavorit.Checked);

            }
            else
            {

                DataRow dr = this.dtUnits.Select("ID=" + this.ItemUnit_ID.ToExpressString())[0];
                dr[1] = acBiggerUnit.Value;
                dr[2] = txtUnitRatio.Text;
                dr[5] = txtBarcodeUnite.Text;
                dr[4] = txtPriceUnite.Text;
                dr[7] = chkIsFavorit.Checked;
            }




            gvUnits.DataSource = this.dtUnits;
            gvUnits.DataBind();
            acBiggerUnit.Clear();
            txtUnitRatio.Clear();
            // acSmallestUnit.Enabled = (gvUnits.Rows.Count == 0 && !this.IsLockedItem);
            acBiggerUnit.AutoCompleteFocus();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }


    protected void btnSave_click(object sender, EventArgs e)
    {        
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        int result = 0;
        try
        {
            if (!this.EditMode) //insert
            {
                result = this.Item_ID = dc.usp_Items_insert(txtName.TrimmedText, acCategory.Value.ToInt(), ddlItemType.SelectedValue.ToCharArray()[0], txtCost.Text.ToDecimal(), acSmallestUnit.Value.ToInt(), acTax.Value.ToNullableInt(), txtMaxQty.Text.ToDecimalOrDefault(), txtMinQty.Text.ToDecimalOrDefault(), this.ImageUrl, txtNotes.Text, txtBarcode.TrimmedText, txtDefaultPrice.Text.ToDecimalOrDefault(), txtPercentageDiscount.Text.ToDecimalOrDefault(), txtCodeItem.TrimmedText, txtNameEn.TrimmedText, txtMaxDicountCash.Text.ToDecimalOrDefault(), txtMaxDuscountParcent.Text.ToDecimalOrDefault(),
                    chkIsUseTax.Checked, chkIsTaxIncludedInPurchase.Checked, chkIsTaxIncludedInSale.Checked);
            }
            else
            {
                result = dc.usp_Items_update(this.Item_ID, txtName.TrimmedText, acCategory.Value.ToInt(), ddlItemType.SelectedValue.ToCharArray()[0], txtCost.Text.ToDecimal(), acSmallestUnit.Value.ToInt(), acTax.Value.ToNullableInt(), txtMaxQty.Text.ToDecimalOrDefault(), txtMinQty.Text.ToDecimalOrDefault(), this.ImageUrl, txtNotes.Text, txtBarcode.TrimmedText, txtDefaultPrice.Text.ToDecimalOrDefault(), txtPercentageDiscount.Text.ToDecimalOrDefault(), txtCodeItem.TrimmedText, txtNameEn.TrimmedText, txtMaxDicountCash.Text.ToDecimalOrDefault(), txtMaxDuscountParcent.Text.ToDecimalOrDefault(),
                    chkIsUseTax.Checked, chkIsTaxIncludedInPurchase.Checked, chkIsTaxIncludedInSale.Checked);
            }



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

            var its = dc.Items.Where(c => c.ID == this.Item_ID).ToList();
            if (its.Any())
            {
                var it = its.First();
                it.IsCuisine = chkItemCuisine.Checked;
                it.HideItem = CheHideItem.Checked;
                it.PrinterName_ID = txtQtyItems.Text.ToNullableInt();
                it.QuantityProductRaw = txtQuantityProductRaw.Text.ToDecimalOrDefault();
                it.Account_ID = acAccountRelated.Value.ToNullableInt();
                it.MiniLevelPrice = txtMiSalePrice.Text.ToDecimalOrDefault();
                it.IsItemBalance = chkIsBalance.Checked;
            }


            foreach (DataRow r in this.dtUnits.Rows)
            {
                if (r.RowState == DataRowState.Deleted)
                {
                    dc.usp_ItemsUnits_Delete(r["ID", DataRowVersion.Original].ToInt());
                }
                else if (r.RowState == DataRowState.Modified)
                {
                    dc.usp_ItemsUnits_Update(r["ID", DataRowVersion.Original].ToInt(), r["Unit_ID"].ToInt(), r["Ratio"].ToDecimal(), r["Price"].ToDecimal(), r["Barcode"].ToExpressString(), r["IsFavorite"].ToBooleanOrDefault());



                }
                else if (r.RowState == DataRowState.Added)
                {
                    var idUnit = dc.usp_ItemsUnits_Insert(this.Item_ID, r["Unit_ID"].ToInt(), r["Ratio"].ToDecimal(), r["Price"].ToDecimal(), r["Barcode"].ToExpressString(), r["IsFavorite"].ToBooleanOrDefault());

                }
            }

            if (txtDefaultPrice.Text.ToDecimalOrDefault() < txtCost.Text.ToDecimalOrDefault())
            {
                UserMessages.Message(null, Resources.UserInfoMessages.PriceCantBeLessThanCost, string.Empty);
                trans.Rollback();
                return;
            }

            foreach (DataRow r in this.dtPrices.Rows)
            {
                if (r.RowState == DataRowState.Deleted)
                {
                    dc.usp_ItemsPrices_Delete(r["ID", DataRowVersion.Original].ToInt());
                }
                else if (r.RowState == DataRowState.Added)
                {
                    dc.usp_ItemsPrices_Insert(this.Item_ID, r["PriceName_ID"].ToInt(), r["Price"].ToDecimal(), r["UOM_ID"].ToIntOrDefault());
                }
                if (r.RowState != DataRowState.Deleted)
                {
                    if (r["Price"].ToDecimal() < txtCost.Text.ToDecimalOrDefault())
                    {
                        UserMessages.Message(null, Resources.UserInfoMessages.PriceCantBeLessThanCost, string.Empty);
                        trans.Rollback();
                        return;
                    }
                }
            }

            foreach (DataRow r in this.dtRawMat.Rows)
            {
                if (r.RowState == DataRowState.Deleted)
                {
                    dc.usp_ItemsMaterials_Delete(r["ID", DataRowVersion.Original].ToInt());
                }
                else if (r.RowState == DataRowState.Added)
                {
                    dc.usp_ItemsMaterials_Insert(this.Item_ID, r["Material_ID"].ToInt(), r["Quantity"].ToDecimal());
                }
                if (r.RowState != DataRowState.Deleted)
                {
                    if (!MyContext.AllowProductionItemMatInMat)
                    {
                        if (ddlItemType.SelectedValue != "c")
                        {
                            UserMessages.Message(null, Resources.UserInfoMessages.OnlyFinalItemsHasRaws, string.Empty);
                            trans.Rollback();
                            return;
                        }
                    }

                }
            }
            foreach (DataRow r in this.dtRawCompose.Rows)
            {
                if (r.RowState == DataRowState.Deleted)
                {
                    dc.usp_ItemsCompose_Delete(r["ID", DataRowVersion.Original].ToInt());
                }
                else if (r.RowState == DataRowState.Added)
                {
                    dc.usp_ItemsCompose_Insert(this.Item_ID, r["Material_ID"].ToInt(), r["Quantity"].ToDecimal());
                }
                if (r.RowState != DataRowState.Deleted)
                {
                    if (ddlItemType.SelectedValue != "i")
                    {
                        UserMessages.Message(null, "الاصناف يجب ان تكون مخزنية", string.Empty);
                        trans.Rollback();
                        return;
                    }
                }
            }

            foreach (DataRow r in this.dtCommission.Rows)
            {
                if (r.RowState == DataRowState.Deleted)
                {
                    dc.usp_ItemsItemCommission_Delete(r["ID", DataRowVersion.Original].ToInt());
                }
                else if (r.RowState == DataRowState.Added)
                {
                    dc.usp_ItemsItemCommission_Insert(this.Item_ID, r["FirstValue"].ToInt(), r["LastValue"].ToInt(), r["ParcentValue"].ToInt());
                }
                if (r.RowState != DataRowState.Deleted)
                {
                    if (r["FirstValue"].ToInt() > r["LastValue"].ToInt())
                    {
                        UserMessages.Message(null, "الرقم الاول اصغر من الرقم الثاني", string.Empty);
                        trans.Rollback();
                        return;
                    }
                }
            }

            foreach (DataRow r in this.dtMinQty.Rows)
            {
                if (r.RowState == DataRowState.Deleted)
                {
                    dc.usp_ItemsUnderLimits_Delete(r["ID", DataRowVersion.Original].ToInt());
                }
                else if (r.RowState == DataRowState.Added)
                {
                    dc.usp_ItemsUnderLimits_Insert(this.Item_ID, r["Store_ID"].ToInt(), r["MinQty"].ToDecimal());
                }
            }
            LogAction(this.EditMode ? Actions.Edit : Actions.Add, txtName.Text, dc);
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, PageLinks.Items + "?ID=" + this.Item_ID.ToExpressString(), PageLinks.ItemsList, PageLinks.Items);
            trans.Commit();
            dc.SubmitChanges();
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
            Response.Redirect(PageLinks.ItemsList, false);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnAddRawMat_click(object sender, EventArgs e)
    {
        try
        {
            DataRow r = this.dtRawMat.NewRow();
            r["ID"] = this.dtRawMat.GetID("ID");
            r["ItemName"] = acRawMat.Text;
            r["Material_ID"] = acRawMat.Value.ToInt();
            r["Quantity"] = txtRawMatQuantity.Text;

            var isExists = (from data in this.dtRawMat.AsEnumerable() where data.RowState != DataRowState.Deleted && data.Field<int>("Material_ID") == acRawMat.Value.ToInt() select data).Any();
            if (isExists)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.RawMatExists, string.Empty);
                return;
            }

            this.dtRawMat.Rows.Add(r);
            gvRawMats.DataSource = this.dtRawMat;
            gvRawMats.DataBind();
            acRawMat.Clear();
            txtRawMatQuantity.Clear();
            acRawMat.AutoCompleteFocus();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnAddMinQty_click(object sender, EventArgs e)
    {
        try
        {
            DataRow r = this.dtMinQty.NewRow();
            r["ID"] = this.dtMinQty.GetID("ID");
            r["StoreName"] = acStore.Text;
            r["Store_ID"] = acStore.Value.ToInt();
            r["MinQty"] = txtMinQty_Multi.Text;

            var isExists = (from data in this.dtMinQty.AsEnumerable() where data.RowState != DataRowState.Deleted && data.Field<int>("Store_ID") == acStore.Value.ToInt() select data).Any();
            if (isExists)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                return;
            }

            this.dtMinQty.Rows.Add(r);
            gvMinQtys.DataSource = this.dtMinQty;
            gvMinQtys.DataBind();
            acStore.Clear();
            txtMinQty_Multi.Clear();
            acStore.AutoCompleteFocus();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvRawMats_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvRawMats.PageIndex = e.NewPageIndex;
            gvRawMats.DataSource = this.dtRawMat;
            gvRawMats.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvRawMats_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ID = gvRawMats.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dtRawMat.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            gvRawMats.DataSource = this.dtRawMat;
            gvRawMats.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvMinQtys_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvMinQtys.PageIndex = e.NewPageIndex;
            gvMinQtys.DataSource = this.dtMinQty;
            gvMinQtys.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvMinQtys_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ID = gvMinQtys.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dtMinQty.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            gvMinQtys.DataSource = this.dtMinQty;
            gvMinQtys.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }


    protected void lnkEdit_OnClick(object sender, EventArgs e)
    {
        var idEval = sender as LinkButton;
        var id = int.Parse(idEval.CommandArgument);
        ItemMaterial_ID = id;
        //    dc.usp_ItemsMaterials_Update(,id)
        var itemMeterials = dc.ItemsMaterials.FirstOrDefault(x => x.ID == id);
        txtRawMatQuantity.Text = itemMeterials.Quantity.ToString();
        acRawMat.Clear();
        acRawMat.Value = itemMeterials.Material_ID.ToString();
        btnEdit.Visible = true;
        btnAddRawMat.Visible = false;

    }

    protected void btnEdit_OnClick(object sender, EventArgs e)
    {

        var itemMeterials = dc.ItemsMaterials.FirstOrDefault(x => x.ID == ItemMaterial_ID);
        if (itemMeterials != null)
        {
            itemMeterials.Material_ID = acRawMat.Value.ToIntOrDefault();
            itemMeterials.Quantity = txtRawMatQuantity.Text.ToDecimalOrDefault();
            dc.SubmitChanges();
            btnEdit.Visible = false;
            btnAddRawMat.Visible = true;
            ItemMaterial_ID = 0;

            this.dtRawMat = dc.usp_ItemsMaterials_Select(this.Item_ID).CopyToDataTable();

            gvRawMats.DataSource = this.dtRawMat;
            gvRawMats.DataBind();

        }

    }

    protected void btnSaveCommission_OnClick(object sender, EventArgs e)
    {

        try
        {
            DataRow r = this.dtCommission.NewRow();
            r["ID"] = this.dtCommission.GetID("ID");
            r["ItemName"] = txtName.Text;
            r["FirstValue"] = txtFirstValue.Text;
            r["LastValue"] = txtLastValue.Text;
            r["ParcentValue"] = txtPercent.Text;

            //var isExists = (from data in this.dtCommission.AsEnumerable() where data.RowState != DataRowState.Deleted && data.Field<int>("Material_ID") == acRawMat.Value.ToInt() select data).Any();
            //if (isExists)
            //{
            //    UserMessages.Message(null, Resources.UserInfoMessages.RawMatExists, string.Empty);
            //    return;
            //}

            this.dtCommission.Rows.Add(r);
            gvCommission.DataSource = this.dtCommission;
            gvCommission.DataBind();
            //acRawMat.Clear();
            //txtRawMatQuantity.Clear();
            //acRawMat.AutoCompleteFocus();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }



    protected void gvCommission_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        try
        {
            int ID = gvCommission.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dtCommission.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            gvCommission.DataSource = this.dtCommission;
            gvCommission.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvCommission_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvCommission.PageIndex = e.NewPageIndex;
            gvCommission.DataSource = this.dtCommission;
            gvCommission.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }

    }

    protected void lnkCommissionEdit_OnClick(object sender, EventArgs e)
    {


    }
    protected void btnSaveDescribred_Click(object sender, EventArgs e)
    {
        try
        {
            if (this.Item_ID != 0)
            {

                int result = dc.usp_ItemsDescribed_Insert(this.Item_ID, txtItemDescribed.TrimmedText, !string.IsNullOrEmpty(txtPriceItemDescribed.Text) ? txtPriceItemDescribed.Text.ToDecimal() : 0);
                if (result == -2)
                {
                    UserMessages.Message(null, Resources.UserInfoMessages.BatchExists, string.Empty);

                    return;
                }
                this.dtItemdescribed = dc.usp_ItemDescribed_Select(this.Item_ID).CopyToDataTable();


                gvItemdescribed.DataSource = this.dtItemdescribed;
                gvItemdescribed.DataBind();

                txtItemDescribed.Clear();
                txtPriceItemDescribed.Clear();
            }
            else
            {
                UserMessages.Message(null, "يجب حفظ المادة اولا قبل انشاء مادة موصوفة لها", string.Empty);
            }

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void gvItemdescribed_PageIndexChanged(object sender, EventArgs e)
    {

    }
    protected void gvItemdescribed_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvItemdescribed.PageIndex = e.NewPageIndex;

            gvItemdescribed.DataSource = this.dtItemdescribed;
            gvItemdescribed.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }


    protected void lnkEditUnit_OnClick(object sender, EventArgs e)
    {
        try
        {

            LinkButton lnk = (sender) as LinkButton;
            int ID = lnk.CommandArgument.ToInt();
            DataRow dr = this.dtUnits.Select("ID=" + ID.ToExpressString())[0];

            acBiggerUnit.Value = dr[1].ToExpressString();
            txtUnitRatio.Text = dr[2].ToExpressString();
            txtBarcodeUnite.Text = dr[5].ToExpressString();
            txtPriceUnite.Text = dr[4].ToExpressString();
            chkIsFavorit.Checked = dr[7].ToBooleanOrDefault();

            btnAddUnit.Text = Resources.Labels.Save;
            this.ItemUnit_ID = ID;
        }
        catch (Exception ex)
        {
            acBiggerUnit.Clear();
            txtUnitRatio.Text = "";
            txtBarcodeUnite.Text = string.Empty;
            txtPriceUnite.Text = string.Empty;
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }

    }

    protected void gvRawCompose_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ID = gvRawCompose.DataKeys[e.RowIndex]["ID"].ToInt();
            DataRow dr = this.dtRawCompose.Select("ID=" + ID.ToExpressString())[0];
            dr.Delete();
            gvRawCompose.DataSource = this.dtRawCompose;
            gvRawCompose.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void gvRawCompose_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvRawCompose.PageIndex = e.NewPageIndex;
            gvRawCompose.DataSource = this.dtRawCompose;
            gvRawCompose.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void lnkEditRawCompose_Click(object sender, EventArgs e)
    {
        var idEval = sender as LinkButton;
        var id = int.Parse(idEval.CommandArgument);
        ItemCompose_ID = id;
        //    dc.usp_ItemsMaterials_Update(,id)
        var itemCompose = dc.ItemsComposes.FirstOrDefault(x => x.ID == id);
        txtQtyRawCompose.Text = itemCompose.Quantity.ToString();
        acRawMat.Clear();
        acItemRawCompose.Value = itemCompose.Material_ID.ToString();
        btnEditCompose.Visible = true;
        btnAddRawCompose.Visible = false;
    }
    protected void btnRawCompose_Click(object sender, EventArgs e)
    {
        try
        {
            DataRow r = this.dtRawCompose.NewRow();
            r["ID"] = this.dtRawCompose.GetID("ID");
            r["ItemName"] = acItemRawCompose.Text;
            r["Material_ID"] = acItemRawCompose.Value.ToInt();
            r["Quantity"] = txtQtyRawCompose.Text;

            var isExists = (from data in this.dtRawCompose.AsEnumerable() where data.RowState != DataRowState.Deleted && data.Field<int>("Material_ID") == acItemRawCompose.Value.ToInt() select data).Any();
            if (isExists)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.RawMatExists, string.Empty);
                return;
            }

            this.dtRawCompose.Rows.Add(r);
            gvRawCompose.DataSource = this.dtRawCompose;
            gvRawCompose.DataBind();
            acItemRawCompose.Clear();
            txtQtyRawCompose.Clear();
            acItemRawCompose.AutoCompleteFocus();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    protected void btnEditCompose_Click(object sender, EventArgs e)
    {
        var itemCompose = dc.ItemsComposes.FirstOrDefault(x => x.ID == this.ItemCompose_ID);
        if (itemCompose != null)
        {
            itemCompose.Material_ID = acItemRawCompose.Value.ToIntOrDefault();
            itemCompose.Quantity = txtQtyRawCompose.Text.ToDecimalOrDefault();
            dc.SubmitChanges();
            btnEdit.Visible = false;
            btnAddRawCompose.Visible = true;
            ItemCompose_ID = 0;

            this.dtRawCompose = dc.usp_ItemsCompose_Select(this.Item_ID).CopyToDataTable();

            gvRawCompose.DataSource = this.dtRawCompose;
            gvRawCompose.DataBind();

        }
    }


    #endregion

    #region Private Methods

    private void LoadControls()
    {
        acRawMat.ContextKey = ",,";
        acItemRawCompose.ContextKey = ",,";
        acPriceName.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.Prices.ToInt().ToExpressString();
        acSmallestUnit.ContextKey = acBiggerUnit.ContextKey = acUom_ID.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + "," + GeneralAttributes.UOM.ToInt().ToExpressString();
        acCategory.ContextKey = string.Empty;
        //acItemsNotCategory.ContextKey = string.Empty;
        acTax.ContextKey = string.Empty;
        this.dtPrices = this.dtUnits = null;
        this.dtRawMat = this.dtMinQty = null;
        this.dtRawCompose = null;
        txtBarcode.Text = dc.fun_GenerateBarcode();
        acSmallestUnit.Value = "15";
        acStore.ContextKey = string.Empty;
        acPrinterName.ContextKey = string.Empty;
        acAccountRelated.ContextKey = this.MyContext.CurrentCulture.ToByte().ToExpressString() + ",,,,true";
        //var company = dc.usp_Company_Select().First();
        //if (company.IsDescribed != null)
        //    chkIsDescription.Visible = company.IsDescribed.Value;

    }

    private void CheckSecurity()
    {
        if (!this.MyContext.PageData.IsViewDoc && this.EditMode) Response.Redirect(PageLinks.Authorization, true);
        btnSave.Visible = (this.EditMode && this.MyContext.PageData.IsEdit) || (!this.EditMode && this.MyContext.PageData.IsAdd);
    }

    private void SetEditMode()
    {
        if (Request["ID"] != null)
        {
            this.EditMode = true;
            this.Item_ID = Request["ID"].ToInt();
        }
    }

    private void FillItemData()
    {
        var item = dc.usp_Items_SelectByID(this.Item_ID).FirstOrDefault();
        txtMaxDicountCash.Text = item.MaxDicountCash.ToExpressString();
        txtMaxDuscountParcent.Text = item.MaxDicountParcent.ToExpressString();
        txtQuantityProductRaw.Text = item.QuantityProductRaw.ToExpressString();
        txtMiSalePrice.Text = item.MiniLevelPrice.ToExpressString();
        txtName.Text = item.Name;
        txtBarcode.Text = item.Barcode;
        txtCodeItem.Text = item.CodeItem;
        acCategory.Value = item.Category_ID.ToExpressString();
        // acItemsNotCategory.Value = item.Desc
        //if (item.Is_Desciption != null && item.Is_Desciption.Value)
        //{
        //    acItemsNotCategory.Value = item.Description_Item_Id.ToExpressString();
        //}

        var Prices = dc.usp_ItemPrices_AvergeAndLast(this.Item_ID, item.UOM_ID).FirstOrDefault();
        if (Prices != null)
        {
            txtAveragePurchasePrice.Text = Prices.AvergePurchsePrice.ToExpressString();
            txtlastPurchasePrice.Text = Prices.LastPurchsePrice.ToExpressString();
        }

        txtNameEn.Text = item.NameEN;
        ddlItemType.SelectedValue = item.Type.ToExpressString();
        txtMinQty.Text = item.MiniQty.ToExpressString();
        txtMaxQty.Text = item.MaxQty.ToExpressString();
        acTax.Value = item.Tax_ID.ToExpressString();
        txtCost.Text = item.Cost.ToExpressString();
        txtPercentageDiscount.Text = item.DiscountPercentage.ToExpressString();
        txtDefaultPrice.Text = item.DefaultPrice.ToExpressString();
        txtNotes.Text = item.Description;
        acSmallestUnit.Value = item.UOM_ID.ToExpressString();

        chkIsUseTax.Checked = item.IsUseTax.ToBooleanOrDefault();        
        chkIsTaxIncludedInPurchase.Checked = item.IsTaxIncludedInPurchase.ToBooleanOrDefault();
        chkIsTaxIncludedInSale.Checked = item.IsTaxIncludedInSale.ToBooleanOrDefault();

        if (File.Exists(Server.MapPath("~/Uploads/" + item.LogoUrl))) imgLogo.ImageUrl = "~/Uploads/" + item.LogoUrl;

        this.dtPrices = dc.usp_ItemsPrices_Select(this.Item_ID).CopyToDataTable();
        this.dtUnits = dc.usp_ItemsUnits_Select(this.Item_ID).CopyToDataTable();
        this.dtRawMat = dc.usp_ItemsMaterials_Select(this.Item_ID).CopyToDataTable();
        this.dtMinQty = dc.usp_ItemsUnderLimits_Select(this.Item_ID).CopyToDataTable();
        this.dtCommission = dc.usp_ItemsCommission_Select(this.Item_ID).CopyToDataTable();
        this.dtItemdescribed = dc.usp_ItemDescribed_Select(this.Item_ID).CopyToDataTable();
        this.dtRawCompose = dc.usp_ItemsCompose_Select(this.Item_ID).CopyToDataTable();


        gvPrices.DataSource = this.dtPrices;
        gvPrices.DataBind();
        gvUnits.DataSource = this.dtUnits;
        gvUnits.DataBind();
        gvRawMats.DataSource = this.dtRawMat;
        gvRawMats.DataBind();
        gvCommission.DataSource = this.dtCommission;
        gvCommission.DataBind();

        gvItemdescribed.DataSource = this.dtItemdescribed;
        gvItemdescribed.DataBind();

        gvMinQtys.DataSource = this.dtMinQty;
        gvMinQtys.DataBind();

        gvRawCompose.DataSource = this.dtRawCompose;
        gvRawCompose.DataBind();


        this.IsLockedItem = item.LockItemUpdate.Value;
        //acSmallestUnit.Enabled = (gvUnits.Rows.Count == 0 && !this.IsLockedItem);
        //gvUnits.Columns[2].Visible = !this.IsLockedItem;
        ddlItemType.Enabled = !this.IsLockedItem;




    }

    private void CustomPage()
    {
        acTax.Visible = MyContext.Features.TaxesEnabled;
    }

    #endregion
}