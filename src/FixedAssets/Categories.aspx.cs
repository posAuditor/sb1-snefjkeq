using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class FixedAssets_Categories : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtAssetsCategories
    {
        get
        {
            return (DataTable)Session["dtAssetsCategories" + this.WinID];
        }

        set
        {
            Session["dtAssetsCategories" + this.WinID] = value;
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
            gvCategories.DataSource = this.dtAssetsCategories;
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

            DataRow dr = this.dtAssetsCategories.Select("ID=" + gvCategories.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            txtName.Text = dr["Name"].ToExpressString();
            ddlDep.SelectedValue = dr["DepMethod"].ToExpressString();
            this.ddlDep_SelectedIndexChanged(null, null);
            ddlReDep.SelectedValue = dr["ReDepMethod"].ToExpressString();
            if (!dr["IsDeletable"].ToBoolean()) ddlReDep.Enabled = ddlDep.Enabled = dr["IsDeletable"].ToBoolean();
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
            result = dc.usp_AssetCategories_Delete(gvCategories.DataKeys[e.RowIndex]["ID"].ToInt());
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
            ddlDep.SelectedIndex = 0;
            ddlReDep.SelectedIndex = 0;
            this.ddlDep_SelectedIndexChanged(null, null);
            ddlDep.Enabled = true;
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
        System.Data.Common.DbTransaction trans;
        dc.Connection.Open();
        trans = dc.Connection.BeginTransaction();
        dc.Transaction = trans;
        try
        {

            int result = 0;

            if (this.EditID == 0) //insert
            {
                result = dc.usp_AssetCategories_Insert(txtName.TrimmedText, ddlDep.SelectedValue.ToByte(), ddlReDep.SelectedValue.ToByte());
            }
            else
            {
                result = dc.usp_AssetCategories_Update(this.EditID, txtName.TrimmedText, ddlDep.SelectedValue.ToByte(), ddlReDep.SelectedValue.ToByte());
            }
            if (result == -2)
            {
                trans.Rollback();
                UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                mpeCreateNew.Show();
                return;
            }
            LogAction(this.EditID == 0 ? Actions.Add : Actions.Edit, txtName.TrimmedText, dc);
            this.Fill();
            this.ClosePopup_Click(null, null);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, string.Empty);
            trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnClearNew_click(object sender, EventArgs e)
    {
        try
        {
            txtName.Clear();
            if (ddlDep.Enabled)
            {
                ddlDep.SelectedIndex = 0;
                this.ddlDep_SelectedIndexChanged(null, null);
            }
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void ddlDep_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlReDep.SelectedIndex = 0;
            ddlReDep.Enabled = ddlDep.SelectedIndex != 0;
            if (sender != null) this.FocusNextControl(sender);
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

    }

    private void Fill()
    {
        this.dtAssetsCategories = dc.usp_AssetCategories_Select(txtNameSrch.TrimmedText).CopyToDataTable();
        gvCategories.DataSource = this.dtAssetsCategories;
        gvCategories.DataBind();

    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        gvCategories.Columns[1].Visible = MyContext.PageData.IsEdit;
        gvCategories.Columns[2].Visible = MyContext.PageData.IsDelete;
        lnkAddNew.Visible = MyContext.PageData.IsAdd;
        mpeCreateNew.TargetControlID = lnkAddNew.Visible ? lnkAddNew.UniqueID : hfmpeCreateNew.UniqueID;
    }

    #endregion
}