using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using XPRESS.Common;
using System.Web.Security;

public partial class Security_Users : UICulturePage
{

    XpressDataContext dc = new XpressDataContext();

    #region Session

    private DataTable dtUsers
    {
        get
        {
            return (DataTable)Session["dtUsers" + this.WinID];
        }

        set
        {
            Session["dtUsers" + this.WinID] = value;
        }
    }

    private DataTable dtRoles
    {
        get
        {
            return (DataTable)Session["dtRoles" + this.WinID];
        }

        set
        {
            Session["dtRoles" + this.WinID] = value;
        }
    }

    #endregion

    # region ViewState

    private Guid EditUserID
    {
        get
        {
            if (ViewState["EditUserID"] == null) return Guid.Empty;
            return (Guid)ViewState["EditUserID"];
        }

        set
        {
            ViewState["EditUserID"] = value;
        }
    }

    private Guid EditRoleID
    {
        get
        {
            if (ViewState["EditRoleID"] == null) return Guid.Empty;
            return (Guid)ViewState["EditRoleID"];
        }

        set
        {
            ViewState["EditRoleID"] = value;
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
                acEmployeeName.ContextKey = MyContext.UserProfile.Branch_ID + ",";
                acGroup.ContextKey = string.Empty;
                this.FillUsers();
                this.FillRoles();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    #endregion

    #region Control Events

    protected void btnResetSearch_Click(object sender, EventArgs e)
    {
        try
        {
            txtUserNameSrch.Clear();
            this.FillUsers();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnClearRoleSrch_Click(object sender, EventArgs e)
    {
        try
        {
            txtRoleNamesrch.Clear();
            this.FillRoles();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSearch_click(object sender, EventArgs e)
    {
        try
        {
            this.FillUsers();
            txtUserNameSrch.Focus();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSearchRoles_click(object sender, EventArgs e)
    {
        try
        {
            this.FillRoles();
            txtRoleNamesrch.Focus();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvUsers.PageIndex = e.NewPageIndex;
            this.FillUsers();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvRoles_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvRoles.PageIndex = e.NewPageIndex;
            this.FillRoles();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            Guid UserID = new Guid(gvUsers.DataKeys[e.RowIndex]["userID"].ToString());
            if (this.MyContext.UserProfile.UserId == UserID)
            {
                UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.CantDeleteCurrentUser, string.Empty);
                return;
            }
            MembershipUser u = Membership.GetUser(UserID);
            Membership.DeleteUser(u.UserName);
            LogAction(Actions.Delete, " المستخدمين: " + u.UserName, dc);
            this.FillUsers();
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvRoles_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            string RoleName = gvRoles.DataKeys[e.RowIndex]["RoleName"].ToString();

            if (Roles.IsUserInRole(RoleName))
            {
                UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.CantDeleteCurrentGroup, string.Empty);
                return;
            }

            if (Roles.GetUsersInRole(RoleName).Count() > 0)
            {
                UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.GroupHasUsers, string.Empty);
                return;
            }
            Roles.DeleteRole(RoleName);
            LogAction(Actions.Delete, "مجموعات المستخدمين: " + RoleName, dc);
            this.FillRoles();
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvRoles_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            this.EditRoleID = new Guid(gvRoles.DataKeys[e.NewSelectedIndex]["roleID"].ToString());
            txtRoleName.Text = gvRoles.DataKeys[e.NewSelectedIndex]["RoleName"].ToString();
            mpeCreateRole.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void gvUsers_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        try
        {
            this.EditUserID = new Guid(gvUsers.DataKeys[e.NewSelectedIndex]["userID"].ToString());
            DataRow dr = this.dtUsers.Select("UserID='" + this.EditUserID + "'")[0];
            txtNewUserName.Text = dr["UserName"].ToString();
            acEmployeeName.ContextKey = MyContext.UserProfile.Branch_ID + "," + dr["Contact_ID"].ToString();
            acEmployeeName.Enabled = this.EditUserID != new Guid("3fe2b6eb-cce6-404d-af1b-9751376c3683");
            acEmployeeName.Value = dr["Contact_ID"].ToString();
            txtEmail.Text = dr["Email"].ToString();
            txtPassword.IsRequired = false;
            ddlFavLang.SelectedValue = dr["UserCulture"].ToString();
            acGroup.Value = dr["RoleID"].ToString();
            mpeCreateUser.Show();
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
            this.ClosePopup(10);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnClearNewUser_click(object sener, EventArgs e)
    {
        try
        {
            txtNewUserName.Clear();
            acEmployeeName.Clear();
            txtEmail.Text = string.Empty;
            acGroup.Clear();
            mpeCreateUser.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSaveNewUser_click(object sender, EventArgs e)
    {
        try
        {
            if (MyContext.Features.WorkingMode == WorkingMode.Stores.ToByte() && this.dtUsers.Rows.Count >= 200)
            {
                UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.NoMoreUsers, string.Empty);
                mpeCreateUser.Show();
                return;
            }

            MembershipCreateStatus Status;
            if (this.EditUserID == Guid.Empty) //Insert
            {
                MembershipUser user = Membership.CreateUser(txtNewUserName.TrimmedText.ToLower(), txtPassword.Text, txtEmail.TrimmedText, "Question", "Answer", true, out Status);
                switch (Status)
                {
                    case MembershipCreateStatus.DuplicateEmail:
                        UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.EmailExists, string.Empty);
                        mpeCreateUser.Show();
                        break;
                    case MembershipCreateStatus.DuplicateUserName:
                        UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.UserNameExists, string.Empty);
                        mpeCreateUser.Show();
                        break;

                    case MembershipCreateStatus.Success:
                        Roles.AddUserToRole(user.UserName, acGroup.Text);
                        dc.usp_Users_insert(new Guid(user.ProviderUserKey.ToString()), acEmployeeName.Value.ToInt(), ddlFavLang.SelectedValue.ToByte());
                        LogAction(Actions.Add, " المستخدمين: " + user.UserName, dc);
                        this.FillUsers();
                        UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
                        this.ClosePopup(1);
                        break;
                    default:
                        throw new Exception("Error occured during creating a user");
                        break;
                }
            }
            else
            {
                int result = dc.usp_Users_update(this.EditUserID, txtNewUserName.TrimmedText.ToLower(), acEmployeeName.Value.ToInt(), txtEmail.TrimmedText, ddlFavLang.SelectedValue.ToByte(), new Guid(acGroup.Value));
                switch (result)
                {
                    case -2:
                        UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.EmailExists, string.Empty);
                        mpeCreateUser.Show();
                        break;
                    case -3:
                        UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.UserNameExists, string.Empty);
                        mpeCreateUser.Show();
                        break;
                    default:
                        if (txtPassword.IsNotEmpty)
                        {
                            MembershipUser u = Membership.GetUser(this.EditUserID);
                            u.ChangePassword(u.ResetPassword(), txtPassword.Text);
                        }
                        LogAction(Actions.Edit, " المستخدمين: " + txtNewUserName.TrimmedText.ToLower(), dc);
                        this.FillUsers();
                        UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
                        this.ClosePopup(1);

                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void btnSaveRole_click(object sender, EventArgs e)
    {
        try
        {
            if (this.EditRoleID == Guid.Empty)
            {
                if (Roles.RoleExists(txtRoleName.TrimmedText))
                {
                    UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.GroupExists, string.Empty);
                    mpeCreateRole.Show();
                    return;
                }
                Roles.CreateRole(txtRoleName.TrimmedText);
                LogAction(Actions.Add, "مجموعات المستخدمين: " + txtRoleName.TrimmedText, dc);
            }
            else
            {
                int result = dc.usp_UserGroups_update(this.EditRoleID, txtRoleName.Text);
                if (result == -2)
                {
                    UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.GroupExists, string.Empty);
                    mpeCreateRole.Show();
                    return;
                }
                LogAction(Actions.Edit, "مجموعات المستخدمين: " + txtRoleName.TrimmedText, dc);

            }
            this.FillRoles();
            this.ClosePopup(0);
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    protected void BtnClearRole_click(object sender, EventArgs e)
    {
        try
        {
            txtRoleName.Clear();
            mpeCreateRole.Show();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }




    #endregion

    #region Private Methods

    private void FillUsers()
    {
        this.dtUsers = dc.usp_Users_Select(null, txtUserNameSrch.Text, MyContext.UserProfile.Branch_ID).Where(x => x.Contact_ID != 1 || this.MyContext.UserProfile.UserId == new Guid("d7971e87-66aa-414f-9c00-ab3011a83a09")).CopyToDataTable();
        gvUsers.DataSource = this.dtUsers;
        gvUsers.DataBind();
        acEmployeeName.Refresh();
    }

    private void FillRoles()
    {
        this.dtRoles = dc.usp_UserGroups_Select(txtRoleNamesrch.Text).Where(x => x.RoleId != new Guid("ef5dfc9e-c034-47f1-8622-7befede90136") || this.MyContext.UserProfile.UserId == new Guid("d7971e87-66aa-414f-9c00-ab3011a83a09")).CopyToDataTable();
        gvRoles.DataSource = this.dtRoles;
        gvRoles.DataBind();
        acGroup.Refresh();
    }

    private void ClosePopup(byte PopupIndex)
    {
        if (MyContext.FastEntryEnabled)
        {
            if (PopupIndex == 0) mpeCreateRole.Show();
            if (PopupIndex == 1) mpeCreateUser.Show();
        }
        else
        {
            mpeCreateRole.Hide();
            mpeCreateUser.Hide();
        }

        txtNewUserName.Text = string.Empty;
        acEmployeeName.ContextKey = string.Empty;
        acEmployeeName.Clear();
        acEmployeeName.Enabled = true;
        txtEmail.Text = string.Empty;
        txtPassword.IsRequired = true;
        acGroup.Clear();
        txtRoleName.Clear();
        this.EditRoleID = Guid.Empty;
        this.EditUserID = Guid.Empty;
    }

    private void CheckSecurity()
    {
        if (!this.MyContext.PageData.IsViewDoc) Response.Redirect(PageLinks.Authorization, true);

        lnkAddNewRole.Visible = lnkAddNewUser.Visible = this.MyContext.PageData.IsAdd;
        mpeCreateRole.TargetControlID = this.MyContext.PageData.IsAdd ? lnkAddNewRole.UniqueID : hfmpeCreateRole.UniqueID;
        mpeCreateUser.TargetControlID = this.MyContext.PageData.IsAdd ? lnkAddNewUser.UniqueID : hfmpeCreateUser.UniqueID;
        //gvUsers.Columns[5].Visible = gvRoles.Columns[2].Visible = gvRoles.Columns[3].Visible = this.MyContext.PageData.IsEdit;
        gvUsers.Columns[6].Visible = gvRoles.Columns[4].Visible = this.MyContext.PageData.IsDelete;
    }

    #endregion

}