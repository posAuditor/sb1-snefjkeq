using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class FixedAssets_PeriodicMaintainenceExpenses : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtExpenses
    {
        get
        {
            return (DataTable)Session["dtPeriodicMaintainenceExpenses" + this.WinID];
        }

        set
        {
            Session["dtPeriodicMaintainenceExpenses" + this.WinID] = value;
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

    protected void gvExpenses_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvExpenses.PageIndex = e.NewPageIndex;
            gvExpenses.DataSource = this.dtExpenses;
            gvExpenses.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvExpenses_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {

            DataRow dr = this.dtExpenses.Select("ID=" + gvExpenses.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            txtName.Text = dr["ExpenseName"].ToExpressString();
            txtPeriodicFollowUp.Text = dr["PeriodicFollowUp"].ToExpressString();
            ddlPeriodType.SelectedValue = dr["PeriodType"].ToExpressString();
            acAsset.Value = dr["AssetID"].ToExpressString();
            this.EditID = gvExpenses.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            acAsset.Enabled = false;
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvExpenses_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_PeriodicMaintenanceExpenses_Delete(gvExpenses.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvExpenses.DataKeys[e.RowIndex]["ExpenseName"].ToExpressString(), dc);
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
            acAsset.Enabled = true;
            this.ClearForm();
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
        try
        {
            int result = 0;

            if (this.EditID == 0) //insert
            {
                result = dc.usp_PeriodicMaintenanceExpenses_Insert(txtName.TrimmedText, acAsset.Value.ToInt(), txtPeriodicFollowUp.Text.ToInt(), ddlPeriodType.SelectedValue.ToByte());
            }
            else
            {
                result = dc.usp_PeriodicMaintenanceExpenses_Update(this.EditID, txtName.TrimmedText, txtPeriodicFollowUp.Text.ToInt(), ddlPeriodType.SelectedValue.ToByte());
            }
            if (result == -2)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.NameAlreadyExists, string.Empty);
                mpeCreateNew.Show();
                return;
            }
            LogAction(this.EditID == 0 ? Actions.Add : Actions.Edit, txtName.TrimmedText, dc);
            this.Fill();
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
            this.ClearForm();
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
        acAsset.ContextKey = "-1,,False";
    }

    private void ClearForm()
    {
        txtName.Clear();
        txtPeriodicFollowUp.Clear();
        if (acAsset.Enabled) acAsset.Clear();
        ddlPeriodType.SelectedIndex = 0;
    }

    private void Fill()
    {
        this.dtExpenses = dc.usp_PeriodicMaintenanceExpenses_Select(txtNameSrch.TrimmedText, null).CopyToDataTable();
        gvExpenses.DataSource = this.dtExpenses;
        gvExpenses.DataBind();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        gvExpenses.Columns[4].Visible = MyContext.PageData.IsEdit;
        gvExpenses.Columns[5].Visible = MyContext.PageData.IsDelete;
        lnkAddNew.Visible = MyContext.PageData.IsAdd;
        mpeCreateNew.TargetControlID = lnkAddNew.Visible ? lnkAddNew.UniqueID : hfmpeCreateNew.UniqueID;
    }

    #endregion
}