using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using XPRESS.Common;

public partial class Security_MyProfile : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            txtEmail.Text = Membership.GetUser(this.MyContext.UserProfile.UserName).Email;
            ddlFavLang.SelectedValue = this.MyContext.UserProfile.UserCulture.ToByte().ToString();

            if (!this.MyContext.PageData.IsViewDoc || MyContext.UserProfile.UserName.ToLower().Trim() == "xpress")
                Response.Redirect(PageLinks.Authorization, true);
            btnSave.Visible = this.MyContext.PageData.IsEdit;
        }
    }


    protected void btnSave_click(object sender, EventArgs e)
    {
        try
        {
            if (!Membership.ValidateUser(this.MyContext.UserProfile.UserName, txtOldPassword.Text))
            {
                UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.InvOldPassword, string.Empty);
                return;
            }
            MembershipUser u = Membership.GetUser(this.MyContext.UserProfile.UserName);
            if (txtNewPassword.IsNotEmpty)
            {
                u.ChangePassword(u.ResetPassword(), txtNewPassword.Text);
            }
            int result = dc.usp_Users_update(this.MyContext.UserProfile.UserId, null, null, txtEmail.TrimmedText, ddlFavLang.SelectedValue.ToByte(), null);
            if (result == -2)
            {
                UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.EmailExists, string.Empty);
                return;
            }
            LogAction(Actions.Edit, string.Empty, dc);
            UserMessages.Message(this.MyContext.PageData.PageTitle, Resources.UserInfoMessages.OperationSuccess, string.Empty);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }


    protected void btnClear_click(object sender, EventArgs e)
    {
        try
        {
            txtNewPassword.Clear();
            txtOldPassword.Clear();
            txtEmail.Clear();
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }
}