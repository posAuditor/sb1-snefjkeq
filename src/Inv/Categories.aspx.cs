using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class Inv_Categories : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtItemsCategories
    {
        get
        {
            return (DataTable)Session["dtItemsCategories" + this.WinID];
        }

        set
        {
            Session["dtItemsCategories" + this.WinID] = value;
        }
    }


    private DataTable dtItemsStoriesCategories
    {
        get
        {
            return (DataTable)Session["dtItemsStoriesCategories" + this.WinID];
        }

        set
        {
            Session["dtItemsStoriesCategories" + this.WinID] = value;
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

    private int IDStories
    {
        get
        {
            if (ViewState["IDStories"] == null) return 0;
            return (int)ViewState["IDStories"];
        }

        set
        {
            ViewState["IDStories"] = value;
        }
    }


    #endregion

    #region Page events
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.CheckSecurity();
            if (!Page.IsPostBack)
            {
                this.LoadControls();
                this.Fill();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
    #endregion

    #region Control Events

    protected void btnSearch_click(object sender, EventArgs e)
    {
        try
        {
            this.Fill();
            txtNameSrch.Focus();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnClearSrch_Click(object sender, EventArgs e)
    {
        try
        {
            txtNameSrch.Clear();
            this.Fill();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvCategories_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvCategories.PageIndex = e.NewPageIndex;
            gvCategories.DataSource = this.dtItemsCategories;
            gvCategories.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvCategories_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {

            DataRow dr = this.dtItemsCategories.Select("ID=" + gvCategories.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            txtName.Text = dr["MonoName"].ToExpressString();
            acParentCategory.Value = dr["parent_ID"].ToExpressString();
            chkPos.Checked = dr["ISCatPos"].ToBooleanOrDefault();
            acPrinterName.Value = dr["PrinterName_ID"].ToExpressString();
            chkIsNotViewInInvoice.Checked = dr["IsNotViewInInvoice"].ToBooleanOrDefault();
            chkIsNotViewInReceipt.Checked = dr["IsNotViewInReceipt"].ToBooleanOrDefault();
            chkIsUseTax.Checked = dr["IsUseTax"].ToBooleanOrDefault();
            acTax.Value = dr["Tax_ID"].ToDecimalOrDefault().ToString();
            chkIsTaxIncludedInPurchase.Checked= dr["IsTaxIncludedInPurchase"].ToBooleanOrDefault();
            chkIsTaxIncludedInSale.Checked = dr["IsTaxIncludedInSale"].ToBooleanOrDefault();
            this.EditID = gvCategories.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvCategories_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_ItemsCategories_Delete(gvCategories.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvCategories.DataKeys[e.RowIndex]["Name"].ToExpressString(), dc);
            this.Fill();
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void ClosePopup_Click(object sender, EventArgs e)
    {
        try
        {
            txtName.Clear();
            acParentCategory.Clear();
            this.EditID = 0;
            if (sender == null && MyContext.FastEntryEnabled)
            {
                mpeCreateNew.Show();
            }
            else
            {
                mpeCreateNew.Hide();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSaveNew_click(object sender, EventArgs e)
    {
        //try
        //{
        //    int result = 0;

        //    if (this.EditID == acParentCategory.Value.ToNullableInt())
        //    {
        //        UserMessages.Message(null, Resources.UserInfoMessages.CatSelfparent, string.Empty);
        //        mpeCreateNew.Show();
        //        return;
        //    }

        //    if (this.EditID == 0) //insert
        //    {
        //        result = dc.usp_ItemsCategories_Insert(txtName.TrimmedText, acParentCategory.Value.ToNullableInt());
        //    }
        //    else
        //    {
        //        result = dc.usp_ItemsCategories_Update(this.EditID, txtName.TrimmedText, acParentCategory.Value.ToNullableInt());
        //    }
        //    if (result == -2)
        //    {
        //        UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
        //        mpeCreateNew.Show();
        //        return;
        //    }
        //    LogAction(this.EditID == 0 ? Actions.Add : Actions.Edit, txtName.TrimmedText, dc);
        //    this.Fill();
        //    this.ClosePopup_Click(null, null);
        //    UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        //}
        //catch (Exception ex)
        //{
        //    Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        //}


        try
        {
            int result = 0;

            if (this.EditID == acParentCategory.Value.ToNullableInt())
            {
                UserMessages.Message(null, Resources.UserInfoMessages.CatSelfparent, string.Empty);
                mpeCreateNew.Show();
                return;
            }

            if (this.EditID == 0) //insert
            {
                result = dc.usp_ItemsCategories_Insert(txtName.TrimmedText, acParentCategory.Value.ToNullableInt(), chkPos.Checked, acPrinterName.Value.ToIntOrDefault(), chkIsNotViewInInvoice.Checked, chkIsNotViewInReceipt.Checked, 
                    chkIsUseTax.Checked, acTax.Value.ToNullableInt(), chkIsTaxIncludedInPurchase.Checked, chkIsTaxIncludedInSale.Checked);
            }
            else
            {
                result = dc.usp_ItemsCategories_Update(this.EditID, txtName.TrimmedText, acParentCategory.Value.ToNullableInt(), chkPos.Checked, acPrinterName.Value.ToIntOrDefault(), chkIsNotViewInInvoice.Checked, chkIsNotViewInReceipt.Checked,
                    chkIsUseTax.Checked, acTax.Value.ToNullableInt(), chkIsTaxIncludedInPurchase.Checked, chkIsTaxIncludedInSale.Checked);
            }
            if (result == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                mpeCreateNew.Show();
                return;
            }
            LogAction(this.EditID == 0 ? Actions.Add : Actions.Edit, txtName.TrimmedText, dc);
            this.Fill();
            acPrinterName.Clear();
            this.ClosePopup_Click(null, null);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnClearNew_click(object sender, EventArgs e)
    {
        try
        {
            txtName.Clear();
            acParentCategory.Clear();
            mpeCreateNew.Show();
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

        acStore.ContextKey = string.Empty;
        acParentCategory.ContextKey = string.Empty;
        acPrinterName.ContextKey = string.Empty;
        acTax.ContextKey = string.Empty;
    }

    private void Fill()
    {        
        this.dtItemsCategories = dc.usp_ItemsCategories_Select(txtNameSrch.TrimmedText).CopyToDataTable();
        gvCategories.DataSource = this.dtItemsCategories;
        gvCategories.DataBind();
        acParentCategory.Refresh();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        gvCategories.Columns[2].Visible = MyContext.PageData.IsEdit;
        gvCategories.Columns[3].Visible = MyContext.PageData.IsDelete;
        lnkAddNew.Visible = MyContext.PageData.IsAdd;
        mpeCreateNew.TargetControlID = lnkAddNew.Visible ? lnkAddNew.UniqueID : hfmpeCreateNew.UniqueID;
    }

    #endregion




    protected void lnkRelatedBranch_Click(object sender, EventArgs e)
    {
        int Index = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
        this.IDStories = gvCategories.DataKeys[Index]["ID"].ToInt();
        this.FillStoresItemCate();
        ModalPopupExtender1.Show();
    }

    public void FillStoresItemCate()
    {
        var list = dc.ItemsCategories.Where(x => x.ID == this.IDStories).ToList();
        if (list.Any())
        {
            var objItem = list.First();
            var stringStores = objItem.StoresIDS.ToExpressString();
            var listStories = stringStores.Split(',');
            if (listStories.Any())
            {
                var l = dc.Stores.Where(x => listStories.Contains(x.ID.ToString())).ToList();

                this.dtItemsStoriesCategories = l.Select(c => new StoresItemCategory() { ID = c.ID, Name = c.Name }).CopyToDataTable();

            }
            else
            {
                this.dtItemsStoriesCategories = (new List<StoresItemCategory>()).CopyToDataTable();
            }
        }
        else
        {
            this.dtItemsStoriesCategories = (new List<StoresItemCategory>()).CopyToDataTable();
        }

        gvStoresCatergory.DataSource = this.dtItemsStoriesCategories;
        gvStoresCatergory.DataBind();
        ModalPopupExtender1.Show();
    }
    protected void btnSaveStoresItemsCategory_Click(object sender, EventArgs e)
    {
        if (this.IDStories > 0)
        {
            var list = dc.ItemsCategories.Where(x => x.ID == this.IDStories).ToList();
            if (list.Any())
            {
                var objItem = list.First();
                var stringStores = objItem.StoresIDS.ToExpressString();
                var listStories = stringStores.Split(',');
                if (listStories.Any())
                {
                    if (!listStories.Contains(acStore.Value.ToExpressString()))
                    {
                        objItem.StoresIDS = acStore.Value.ToExpressString() + "," + objItem.StoresIDS;
                    }
                }
                else
                {
                    objItem.StoresIDS = acStore.Value.ToExpressString();
                }
                UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
                dc.SubmitChanges();
                this.FillStoresItemCate(); Fill();
                ModalPopupExtender1.Show();
            }
        }
    }
    protected void gvStoresCatergory_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (this.IDStories > 0)
            {
                var list = dc.ItemsCategories.Where(x => x.ID == this.IDStories).ToList();
                if (list.Any())
                {
                    var objItem = list.First();
                    var stringStores = objItem.StoresIDS.ToExpressString();
                    var listStories = stringStores.Split(',');
                    if (listStories.Any())
                    {
                        objItem.StoresIDS = "";
                        foreach (var item in listStories)
                        {
                            if (item!=gvStoresCatergory.DataKeys[e.RowIndex]["ID"].ToString())
                            {
                                objItem.StoresIDS = item + "," + objItem.StoresIDS;
                            }

                        }
                        objItem.StoresIDS = objItem.StoresIDS.TrimEnd(',');
                       
                    }
                    

                    dc.SubmitChanges();
                    this.FillStoresItemCate(); Fill();
                }
            }
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
}

public class StoresItemCategory
{
    public int ID { get; set; }
    public string Name { get; set; }
}