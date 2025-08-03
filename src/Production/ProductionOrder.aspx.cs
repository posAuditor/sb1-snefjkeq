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

public partial class Production_ProductionOrder : UICulturePage
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

    private int ProductionOrder_ID
    {
        get
        {
            if (ViewState["ProductionOrder_ID"] == null) return 0;
            return (int)ViewState["ProductionOrder_ID"];
        }

        set
        {
            ViewState["ProductionOrder_ID"] = value;
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

    private DataTable dtItemsMaterials
    {
        get
        {
            if (Session["dtItemsMaterials_ProdOrder" + this.WinID] == null)
            {
                Session["dtItemsMaterials_ProdOrder" + this.WinID] = dc.usp_ProductionOrderDetails_Select(null).CopyToDataTable();
            }
            return (DataTable)Session["dtItemsMaterials_ProdOrder" + this.WinID];
        }

        set
        {
            Session["dtItemsMaterials_ProdOrder" + this.WinID] = value;
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
                if (EditMode) this.FillProductionOrder();
            }

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        this.CustomPage();
        ucNewBatchID.TargetControlID = lnkNewBatch.Visible ? lnkNewBatch.UniqueID : hfNewBatch.UniqueID;
    }

    #endregion

    #region Control Events

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
            if (acBatchID.HasValue && acBatchID.Value.ToNullableInt() != 0)
            {
                var batch = dc.usp_ItemsBatch_Select(acBatchID.Value.ToInt(), acFinalItem.Value.ToInt(), null, null, false).FirstOrDefault();
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

    protected void acFinalItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (sender != null) this.FocusNextControl(sender);
            acBatchID.ContextKey = acFinalItem.Value + ",";
            ucNewBatchID.ItemID = acFinalItem.Value;
            if (!acFinalItem.HasValue || !acRawStore.HasValue || txtFinalItemQty.Text.ToDecimalOrDefault() <= 0 || sender == null) return;
            if (this.EditMode)
            {
                (from data in this.dtItemsMaterials.AsEnumerable() where data.RowState != DataRowState.Deleted select data).ToList().ForEach(x => x.Delete());

                var Materials = dc.usp_ProductionOrderDetailsForFinalItem_Select(acFinalItem.Value.ToInt(), acRawStore.Value.ToInt(), txtFinalItemQty.Text.ToDecimal()).ToList();
                foreach (var row in Materials)
                {
                    DataRow r = this.dtItemsMaterials.NewRow();
                    r["ID"] = this.dtItemsMaterials.GetID("ID");
                    r["Store_ID"] = row.Store_ID;
                    r["Category_ID"] = row.Category_ID;
                    r["Item_ID"] = row.Item_ID;
                    r["Quantity"] = row.Quantity;
                    r["Notes"] = row.Notes;
                    r["StoreName"] = row.StoreName;
                    r["ItemName"] = row.ItemName;
                    r["Barcode"] = row.Barcode;
                    r["CategoryName"] = row.CategoryName;
                    r["Total"] = 0;
                    r["GrossTotal"] = 0;
                    this.dtItemsMaterials.Rows.Add(r);
                }
            }
            else
            {
                this.dtItemsMaterials = dc.usp_ProductionOrderDetailsForFinalItem_Select(acFinalItem.Value.ToInt(), acRawStore.Value.ToInt(), txtFinalItemQty.Text.ToDecimal()).CopyToDataTable();
            }
            this.BindItemsGrid();
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
            this.FilterItemsData();
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


            if ((txtQty.Text.ToDecimal() > lblAvailableQty.Text.ToDecimalOrDefault()) && this.QuantityWarning == 2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.QtyNotEnough, string.Empty);
                return;
            }
            DataRow r = null;
            if (this.EditID == 0)
            {
                r = this.dtItemsMaterials.NewRow();
                r["ID"] = this.dtItemsMaterials.GetID("ID");

            }
            else
            {
                r = this.dtItemsMaterials.Select("ID=" + this.EditID)[0];
            }

            r["Store_ID"] = acStore.Value;
            r["Category_ID"] = acCategory.Value;
            r["Item_ID"] = acItem.Value;



            r["Quantity"] = txtQty.Text;
            r["Notes"] = txtItemNotes.Text;
            r["StoreName"] = acStore.Text;
            r["ItemName"] = acItem.Text;
            r["Barcode"] = txtBarcode.Text;
            r["CategoryName"] = acCategory.Text;


            r["Total"] = 0;
            r["GrossTotal"] = 0;
            if (this.EditID == 0) this.dtItemsMaterials.Rows.Add(r);

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
            DataRow r = this.dtItemsMaterials.Select("ID=" + this.EditID.ToExpressString())[0];

            acStore.Value = r["Store_ID"].ToExpressString();
            acCategory.Value = r["Category_ID"].ToExpressString();
            this.acCategory_SelectedIndexChanged(null, null);
            acItem.Value = r["Item_ID"].ToExpressString();
            this.acItem_SelectedIndexChanged(null, null);
            txtQty.Text = r["Quantity"].ToExpressString();
            txtItemNotes.Text = r["Notes"].ToExpressString();
            txtBarcode.Text = r["Barcode"].ToExpressString();
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
            DataRow dr = this.dtItemsMaterials.Select("ID=" + ID.ToExpressString())[0];
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
            dc.usp_ProductionOrder_Cancel(this.ProductionOrder_ID, MyContext.UserProfile.Contact_ID);
            LogAction(Actions.Delete, txtSerial.Text, dc);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.Invoice + "?ID=" + this.ProductionOrder_ID.ToExpressString(), PageLinks.InvoicesList + Request.PathInfo, PageLinks.Invoice);
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
            doc.Load(Server.MapPath("~\\Reports\\ProductionOrder_Print.rpt"));
            doc.SetParameterValue("@ID", this.ProductionOrder_ID);
            Response.Redirect(PageLinks.Print + "?File=" + doc.ExportToPDF(acBranch.Value.ToNullableInt(), "ProductionOrder"), false);
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
            if (!MyContext.AllowProductionItemMatInMat)
                acFinalItem.ContextKey = "c,,";
            else
                acFinalItem.ContextKey = ",,";
            acRawStore.ContextKey = acStore.ContextKey = string.Empty + acBranch.Value;
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private void FilterItems()
    {
        acItem.ContextKey = "," + acCategory.Value + "," + acStore.Value;
        acItem.Clear();
        this.FilterItemsData();
    }

    private void FilterItemsData()
    {
        txtBarcode.Clear();
        txtItemNotes.Clear();
        txtQty.Clear();
        if (acItem.HasValue)
        {
            var item = dc.usp_Items_SelectByID(acItem.Value.ToNullableInt()).FirstOrDefault();
            txtBarcode.Text = item.Barcode;
            acCategory.Value = item.Category_ID.ToExpressString();
        }
    }

    private void LoadControls()
    {
        this.dtItemsMaterials = null;
        acBranch.ContextKey = string.Empty;
        if (this.MyContext.UserProfile.Branch_ID != null)
        {
            acBranch.Value = this.MyContext.UserProfile.Branch_ID.ToExpressString();
            acBranch.Enabled = false;
        }

        this.FilterByBranchAndCurrency();
        acCategory.ContextKey = string.Empty;
        this.FilterItems();
        this.BindItemsGrid();
        lblCreatedBy.Text = MyContext.UserProfile.EmployeeName;
        this.QuantityWarning = dc.usp_Company_Select().FirstOrDefault().QuantityWarning.Value;
        var company = dc.usp_Company_Select().FirstOrDefault();
        if (company.AutoDate.Value) txtOperationDate.Text = DateTime.Now.Date.ToString("d/M/yyyy");
    }

    private void BindItemsGrid()
    {
        gvItems.DataSource = this.dtItemsMaterials;
        gvItems.DataBind();
        acBranch.Enabled = (gvItems.Rows.Count == 0) && (this.MyContext.UserProfile.Branch_ID == null);
    }

    private void ClearItemForm()
    {
        acStore.Clear();
        acCategory.Clear();
        this.FilterItems();
        this.acItem_SelectedIndexChanged(null, null);
    }

    private void SetEditMode()
    {
        if (Request["ID"] != null)
        {
            this.EditMode = true;
            this.ProductionOrder_ID = Request["ID"].ToInt();
        }
    }



    private bool Save(bool IsApproving, System.Data.Common.DbTransaction trans)
    {
        if (dtItemsMaterials.AsEnumerable().Where(r => r.RowState != DataRowState.Deleted).Count() == 0)
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

        if (this.dtItemsMaterials.AsEnumerable().Where(x => x.RowState != DataRowState.Deleted && x.Field<int?>("Store_ID") == null).Any())
        {
            UserMessages.Message(null, Resources.UserInfoMessages.StoresRequired, string.Empty);
            trans.Rollback();
            return false;
        }

        this.CalculatedSalesCost = 0;

        string Serial = string.Empty;
        byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
        DateTime? ApproveDate = IsApproving ? DateTime.Now : (DateTime?)null;
        int? approvedBY_ID = IsApproving ? MyContext.UserProfile.Contact_ID : (int?)null;
        int Serial_ID = DocSerials.ProductionOrder.ToInt();
        int Detail_ID = 0;
        if (!this.EditMode)
        {

            this.ProductionOrder_ID = dc.usp_ProductionOrder_Insert(acBranch.Value.ToNullableInt(), acFinalItem.Value.ToInt(), txtFinalItemQty.Text.ToDecimal(), acRawStore.Value.ToInt(), acBatchID.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate, approvedBY_ID, null, null, txtNotes.TrimmedText, txtUserRefNo.TrimmedText, this.DocRandomString);
            if (this.ProductionOrder_ID > 0)
            {
                foreach (DataRow r in this.dtItemsMaterials.Rows)
                {
                  
                    if (r.RowState == DataRowState.Deleted) continue;
                    Detail_ID = dc.usp_ProductionOrderDetails_Insert(this.ProductionOrder_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), 0, r["Quantity"].ToDecimal(), 0, r["Notes"].ToExpressString());
                    if (IsApproving)
                        if (!this.InsertICJ(Detail_ID, r, 1))
                        {
                            trans.Rollback();
                            return false;
                        }
                }

                if (IsApproving) this.InsertOperation();
                dc.usp_ProductionOrderExtraData_Update(this.ProductionOrder_ID, this.CalculatedSalesCost, null, null, null, null);
                LogAction(IsApproving ? Actions.Approve : Actions.Add, Serial, dc);
            }
        }
        else
        {
            int Result = dc.usp_ProductionOrder_Update(this.ProductionOrder_ID, acBranch.Value.ToNullableInt(), acFinalItem.Value.ToInt(), txtFinalItemQty.Text.ToDecimal(), acRawStore.Value.ToInt(), acBatchID.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref Serial, Serial_ID, DocStatus_ID, ApproveDate, approvedBY_ID, null, null, txtNotes.Text, txtUserRefNo.Text);
            if (Result > 0)
            {
                foreach (DataRow r in this.dtItemsMaterials.Rows)
                {

                 
                     
                    if (r.RowState == DataRowState.Added)
                    {
                        Detail_ID = dc.usp_ProductionOrderDetails_Insert(this.ProductionOrder_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), 0, r["Quantity"].ToDecimal() , 0, r["Notes"].ToExpressString());
                    }
                    if (r.RowState == DataRowState.Modified)
                    {
                        Detail_ID = r["ID"].ToInt();
                        dc.usp_ProductionOrderDetails_Update(r["ID"].ToInt(), r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), 0, r["Quantity"].ToDecimal() , 0, r["Notes"].ToExpressString());
                    }
                    if (r.RowState == DataRowState.Deleted)
                    {
                        dc.usp_ProductionOrderDetails_Delete(r["ID", DataRowVersion.Original].ToInt());
                    }
                    if (IsApproving && r.RowState != DataRowState.Deleted)
                    {
                        Detail_ID = r.RowState == DataRowState.Added ? Detail_ID : r["ID"].ToInt();
                        if (!this.InsertICJ(Detail_ID, r, 1))
                        {
                            trans.Rollback();
                            return false;
                        }
                    }
                }

                if (IsApproving) this.InsertOperation();
                dc.usp_ProductionOrderExtraData_Update(this.ProductionOrder_ID, this.CalculatedSalesCost, null, null, null, null);
                LogAction(IsApproving ? Actions.Approve : Actions.Edit, txtSerial.Text, dc);
            }
        }

        Serial = Serial == string.Empty ? string.Empty : " (" + Serial + ") ";
        UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess + Serial, PageLinks.ProductionOrder + Request.PathInfo + "?ID=" + this.ProductionOrder_ID.ToExpressString(), PageLinks.ProductionOrderList, PageLinks.ProductionOrder);
        return true;
    }


    //private bool Save(bool IsApproving, System.Data.Common.DbTransaction trans)
    //{
    //    if (dtItemsMaterials.AsEnumerable().Where(r => r.RowState != DataRowState.Deleted).Count() == 0)
    //    {
    //        UserMessages.Message(null, Resources.UserInfoMessages.OneItemRequired, string.Empty);
    //        trans.Rollback();
    //        return false;
    //    }


    //    if (txtOperationDate.Text.ToDate() > DateTime.Now.Date)
    //    {
    //        UserMessages.Message(null, Resources.UserInfoMessages.DateBiggerThanToday, string.Empty);
    //        trans.Rollback();
    //        return false;
    //    }

    //    if (this.dtItemsMaterials.AsEnumerable().Where(x => x.RowState != DataRowState.Deleted && x.Field<int?>("Store_ID") == null).Any())
    //    {
    //        UserMessages.Message(null, Resources.UserInfoMessages.StoresRequired, string.Empty);
    //        trans.Rollback();
    //        return false;
    //    }

    //    this.CalculatedSalesCost = 0;

    //    string Serial = string.Empty;
    //    byte DocStatus_ID = IsApproving ? DocStatus.Approved.ToByte() : DocStatus.Current.ToByte();
    //    DateTime? ApproveDate = IsApproving ? DateTime.Now : (DateTime?)null;
    //    int? approvedBY_ID = IsApproving ? MyContext.UserProfile.Contact_ID : (int?)null;
    //    int Serial_ID = DocSerials.ProductionOrder.ToInt();
    //    int Detail_ID = 0;
    //    if (!this.EditMode)
    //    {

    //        this.ProductionOrder_ID = dc.usp_ProductionOrder_Insert(acBranch.Value.ToNullableInt(), acFinalItem.Value.ToInt(), txtFinalItemQty.Text.ToDecimal(), acRawStore.Value.ToInt(), acBatchID.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref Serial, Serial_ID, DocStatus_ID, DateTime.Now, MyContext.UserProfile.Contact_ID, ApproveDate, approvedBY_ID, null, null, txtNotes.TrimmedText, txtUserRefNo.TrimmedText, this.DocRandomString);
    //        if (this.ProductionOrder_ID > 0)
    //        {
    //            foreach (DataRow r in this.dtItemsMaterials.Rows)
    //            {
    //                if (r.RowState == DataRowState.Deleted) continue;
    //                Detail_ID = dc.usp_ProductionOrderDetails_Insert(this.ProductionOrder_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), 0, r["Quantity"].ToDecimal(), 0, r["Notes"].ToExpressString());
    //                if (IsApproving)
    //                    if (!this.InsertICJ(Detail_ID, r))
    //                    {
    //                        trans.Rollback();
    //                        return false;
    //                    }
    //            }

    //            if (IsApproving) this.InsertOperation();
    //            dc.usp_ProductionOrderExtraData_Update(this.ProductionOrder_ID, this.CalculatedSalesCost, null, null, null, null);
    //            LogAction(IsApproving ? Actions.Approve : Actions.Add, Serial, dc);
    //        }
    //    }
    //    else
    //    {
    //        int Result = dc.usp_ProductionOrder_Update(this.ProductionOrder_ID, acBranch.Value.ToNullableInt(), acFinalItem.Value.ToInt(), txtFinalItemQty.Text.ToDecimal(), acRawStore.Value.ToInt(), acBatchID.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref Serial, Serial_ID, DocStatus_ID, ApproveDate, approvedBY_ID, null, null, txtNotes.Text, txtUserRefNo.Text);
    //        if (Result > 0)
    //        {
    //            foreach (DataRow r in this.dtItemsMaterials.Rows)
    //            {
    //                if (r.RowState == DataRowState.Added)
    //                {
    //                    Detail_ID = dc.usp_ProductionOrderDetails_Insert(this.ProductionOrder_ID, r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), 0, r["Quantity"].ToDecimal(), 0, r["Notes"].ToExpressString());
    //                }
    //                if (r.RowState == DataRowState.Modified)
    //                {
    //                    Detail_ID = r["ID"].ToInt();
    //                    dc.usp_ProductionOrderDetails_Update(r["ID"].ToInt(), r["Store_ID"].ToInt(), r["Item_ID"].ToInt(), 0, r["Quantity"].ToDecimal(), 0, r["Notes"].ToExpressString());
    //                }
    //                if (r.RowState == DataRowState.Deleted)
    //                {
    //                    dc.usp_ProductionOrderDetails_Delete(r["ID", DataRowVersion.Original].ToInt());
    //                }
    //                if (IsApproving && r.RowState != DataRowState.Deleted)
    //                {
    //                    Detail_ID = r.RowState == DataRowState.Added ? Detail_ID : r["ID"].ToInt();
    //                    if (!this.InsertICJ(Detail_ID, r))
    //                    {
    //                        trans.Rollback();
    //                        return false;
    //                    }
    //                }
    //            }

    //            if (IsApproving) this.InsertOperation();
    //            dc.usp_ProductionOrderExtraData_Update(this.ProductionOrder_ID, this.CalculatedSalesCost, null, null, null, null);
    //            LogAction(IsApproving ? Actions.Approve : Actions.Edit, txtSerial.Text, dc);
    //        }
    //    }

    //    Serial = Serial == string.Empty ? string.Empty : " (" + Serial + ") ";
    //    UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess + Serial, PageLinks.ProductionOrder + Request.PathInfo + "?ID=" + this.ProductionOrder_ID.ToExpressString(), PageLinks.ProductionOrderList, PageLinks.ProductionOrder);
    //    return true;
    //}

    private void InsertOperation()
    {
        string serial = string.Empty;
        var company = dc.usp_Company_Select().FirstOrDefault();

        if (this.CalculatedSalesCost > 0)
        {
            int Result = dc.usp_Operation_Insert(acBranch.Value.ToNullableInt(), txtOperationDate.Text.ToDate(), ref serial, DocStatus.Approved.ToByte(), OperationTypes.TakeItemsMaterialsOut.ToInt(), company.Currency_ID, this.CalculatedSalesCost, this.CalculatedSalesCost, 1, txtNotes.Text);

            //المخزون دائن
            dc.usp_OperationDetails_Insert(Result, company.InventoryAccount_ID, 0, this.CalculatedSalesCost, 0, this.CalculatedSalesCost, null, this.ProductionOrder_ID, DocumentsTableTypes.ProductionOrder.ToInt());

            //الحساب المقابل
            dc.usp_OperationDetails_Insert(Result, COA.RawMaterialExpenses.ToInt(), this.CalculatedSalesCost, 0, this.CalculatedSalesCost, 0, null, this.ProductionOrder_ID, DocumentsTableTypes.ProductionOrder.ToInt());
        }
    }



    private bool InsertICJ(int Detail_ID, DataRow row,decimal quantityProdcutRaw)
    {
        decimal? Cost = 0;
        int result = 0;

        result = dc.usp_ICJ_ProductionOrder_Out(txtOperationDate.Text.ToDate(), row["Quantity"].ToDecimal() / quantityProdcutRaw, row["Item_ID"].ToInt(), null, 0, null, row["Store_ID"].ToInt(), acBranch.Value.ToNullableInt(), DocumentsTableTypes.ProductionOrder.ToInt(), this.ProductionOrder_ID, Detail_ID, ref Cost);
        this.CalculatedSalesCost += Cost.Value;

        if (result == -32)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.QtyReserved + " (" + row["StoreName"] + " : " + row["ItemName"].ToExpressString() + ")", string.Empty);
            return false;
        }
        if (result == -4)
        {
            UserMessages.Message(null, Resources.UserInfoMessages.QtyNotEnough + " (" + row["StoreName"] + " : " + row["ItemName"] + ")", string.Empty);
            return false;
        }
        return true;
    }



    //private bool InsertICJ(int Detail_ID, DataRow row)
    //{
    //    decimal? Cost = 0;
    //    int result = 0;

    //    result = dc.usp_ICJ_ProductionOrder_Out(txtOperationDate.Text.ToDate(), row["Quantity"].ToDecimal(), row["Item_ID"].ToInt(), null, 0, null, row["Store_ID"].ToInt(), acBranch.Value.ToNullableInt(), DocumentsTableTypes.ProductionOrder.ToInt(), this.ProductionOrder_ID, Detail_ID, ref Cost);
    //    this.CalculatedSalesCost += Cost.Value;

    //    if (result == -32)
    //    {
    //        UserMessages.Message(null, Resources.UserInfoMessages.QtyReserved + " (" + row["StoreName"] + " : " + row["ItemName"].ToExpressString() + ")", string.Empty);
    //        return false;
    //    }
    //    if (result == -4)
    //    {
    //        UserMessages.Message(null, Resources.UserInfoMessages.QtyNotEnough + " (" + row["StoreName"] + " : " + row["ItemName"] + ")", string.Empty);
    //        return false;
    //    }
    //    return true;
    //}

    private void FillProductionOrder()
    {
        var order = dc.usp_ProductionOrder_SelectByID(this.ProductionOrder_ID).FirstOrDefault();
        acBranch.Value = order.Branch_ID.ToStringOrEmpty();
        this.FilterByBranchAndCurrency();
        txtOperationDate.Text = order.OperationDate.Value.ToString("d/M/yyyy");
        txtUserRefNo.Text = order.UserRefNo;
        acRawStore.Value = order.RawStore_ID.ToExpressString();
        acFinalItem.Value = order.Item_ID.ToExpressString();
        this.acFinalItem_SelectedIndexChanged(null, null);
        txtFinalItemQty.Text = order.Quantity.ToExpressString();
        acBatchID.Value = order.Batch_ID.ToStringOrEmpty();
        this.acBatchID_SelectedIndexChanged(null, null);
        txtNotes.Text = order.Notes;
        txtSerial.Text = order.Serial;
        this.DocRandomString = order.DocRandomString;
        lblCreatedBy.Text = order.CreatedByName;
        lblApprovedBy.Text = order.ApprovedBYName;
        this.ImgStatus = ((DocStatus)order.DocStatus_ID).ToExpressString();
        btnPrint.Visible = MyContext.PageData.IsPrint;
        pnlAddItem.Visible = (order.DocStatus_ID == 1);
        btnCancel.Visible = (order.DocStatus_ID == 1) && MyContext.PageData.IsDelete;
        btnApprove.Visible = (order.DocStatus_ID == 1) && MyContext.PageData.IsApprove;
        btnSave.Visible = (order.DocStatus_ID == 1) && ((MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode));
        btnCancelApprove.Visible = !btnApprove.Visible;
        
        gvItems.Columns[gvItems.Columns.Count - 1].Visible = gvItems.Columns[gvItems.Columns.Count - 2].Visible = (order.DocStatus_ID == 1);

        this.DocStatus_ID = order.DocStatus_ID.Value;
        this.dtItemsMaterials = dc.usp_ProductionOrderDetails_Select(this.ProductionOrder_ID).CopyToDataTable();
        this.BindItemsGrid();

    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        btnSave.Visible = (MyContext.PageData.IsAdd && !EditMode) || (MyContext.PageData.IsEdit && EditMode);
        btnApprove.Visible = MyContext.PageData.IsApprove;
        btnCancel.Visible = MyContext.PageData.IsDelete && EditMode;
    }

    private void ShowAvailableQty()
    {
        lblAvailableQty.Text = string.Empty;
        if (acItem.HasValue)
        {
            decimal Qty = dc.fun_ItemQty(acItem.Value.ToInt(), acStore.Value.ToNullableInt(), null, null, 0).Value;
            lblAvailableQty.Text = Qty.ToExpressString();
            lblAvailableQty.ForeColor = Qty <= 0 ? System.Drawing.Color.Red : System.Drawing.Color.Green;
        }
    }

    private void CustomPage()
    {
        acBranch.IsRequired = acBranch.Visible = MyContext.Features.BranchesEnabled;
        acBatchID.Visible = MyContext.Features.BatchIDEnabled;
        txtProductionDate.Visible = MyContext.Features.BatchIDEnabled;
        txtExpirationDate.Visible = MyContext.Features.BatchIDEnabled;
        lnkNewBatch.Visible = MyContext.Features.BatchIDEnabled;
    }

    #endregion
    protected void btnCancelApprove_Click(object sender, EventArgs e)
    {
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {
            dc.usp_ProductionOrderCancelApprovel(this.ProductionOrder_ID);
            trans.Commit();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.ProductionOrder + "?ID=" + this.ProductionOrder_ID.ToExpressString(), PageLinks.InvoicesList, PageLinks.ProductionOrder);

        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
}