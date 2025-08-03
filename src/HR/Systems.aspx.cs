using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;

public partial class HR_Systems : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    #region Properties

    private DataTable dtSystems
    {
        get
        {
            return (DataTable)Session["dtSystems" + this.WinID];
        }

        set
        {
            Session["dtSystems" + this.WinID] = value;
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
            ddlSystemTypeSrch.SelectedIndex = 0;
            this.Fill();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvSystems_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvSystems.PageIndex = e.NewPageIndex;
            gvSystems.DataSource = this.dtSystems;
            gvSystems.DataBind();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvSystems_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {

            DataRow dr = this.dtSystems.Select("ID=" + gvSystems.DataKeys[e.NewSelectedIndex]["ID"].ToExpressString())[0];
            txtName.Text = dr["Name"].ToExpressString();
            ddlSystemType.SelectedValue = dr["SystemType"].ToExpressString();
            ddlSystemType.Enabled = false;
            this.EditID = gvSystems.DataKeys[e.NewSelectedIndex]["ID"].ToInt();
            mpeCreateNew.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvSystems_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int result = 0;
            result = dc.usp_HR_Systems_Delete(gvSystems.DataKeys[e.RowIndex]["ID"].ToInt());
            if (result == -6)
            {
                UserMessages.Message(null, Resources.UserInfoMessages.DataCantDelete, string.Empty);
                return;
            }
            LogAction(Actions.Delete, gvSystems.DataKeys[e.RowIndex]["Name"].ToExpressString(), dc);
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
            this.EditID = 0;
            ddlSystemType.Enabled = true;
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
                result = dc.usp_HR_Systems_Insert(txtName.TrimmedText, ddlSystemType.SelectedValue.ToByte());
            }
            else
            {
                result = dc.usp_HR_Systems_Update(this.EditID, txtName.TrimmedText, null, null, null);
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
            txtName.Clear();
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
        byte? SystemType = ddlSystemTypeSrch.SelectedIndex == 0 ? (byte?)null : ddlSystemTypeSrch.SelectedValue.ToByte();
        this.dtSystems = dc.usp_HR_Systems_Select(null, txtNameSrch.TrimmedText, SystemType).CopyToDataTable();
        gvSystems.DataSource = this.dtSystems;
        gvSystems.DataBind();
    }

    private void CheckSecurity()
    {
        if (!MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);
        gvSystems.Columns[3].Visible = MyContext.PageData.IsEdit;
        gvSystems.Columns[4].Visible = MyContext.PageData.IsDelete;
        lnkAddNew.Visible = MyContext.PageData.IsAdd;
        mpeCreateNew.TargetControlID = lnkAddNew.Visible ? lnkAddNew.UniqueID : hfmpeCreateNew.UniqueID;
    }

    #endregion
}