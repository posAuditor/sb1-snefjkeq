using System;
using System.Web.Security;
using XPRESS.Common;
using System.IO;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Web.Security;
using System.Configuration;
using System.Web.Http;

public partial class Main_Login : UICulturePage
{

    XpressDataContext dc = new XpressDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                if (User.Identity.IsAuthenticated) Response.Redirect(PageLinks.MainPage, false);

                if (Request["UserAlreadyInUse"] != null)
                {
                    lblMessage.Text = Resources.UserInfoMessages.UserAlreadyInUse;
                    lblMessage.ForeColor = System.Drawing.Color.FromArgb(230, 94, 94);
                }
                this.FillDatabases();

                // SetDataBase();

                //GlobalConfiguration.Configure(config =>
                //{
                //    config.MessageHandlers.Add(new LanguageMessageHandler(System.Threading.Thread.CurrentThread.CurrentCulture.Name));
                //    config.MapHttpAttributeRoutes();

                //    config.Routes.MapHttpRoute(
                //        name: "DefaultApi",
                //        routeTemplate: "api/{controller}/{action}/{id}",
                //        defaults: new { id = System.Web.Http.RouteParameter.Optional }
                //    );
                //});
            }

        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
        }
    }

    protected void RecoverPassword(object sender, EventArgs e)
    {
        try
        {
            MembershipUser u = Membership.GetUser(txtUser.Text);
            var upsd = u.ResetPassword();
            if (u != null && u.Email.Trim() != string.Empty && txtUser.Text.ToLower().Trim() != "xpress")
            {
                //txtPassword.Text = upsd;
                MailMessage message = new MailMessage("noreply@auditorerp.cloud", u.Email);
                message.From = new MailAddress("noreply@auditorerp.cloud", this.Title);
                message.IsBodyHtml = false;
                message.Subject = "New password for user: " + u.UserName;
                message.Body = "Your new password for username " + u.UserName + " is " + upsd + " at " + Request.Url.Host;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential("noreply@auditorerp.cloud", "password");
                smtp.Send(message);
                lblMessage.Text = Resources.UserInfoMessages.NewPasswordSent + " " + u.Email;
                lblMessage.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblMessage.Text = Resources.UserInfoMessages.UnableToRecover;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
        }
    }

    protected void ddlDatabase_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(dc.Connection.ConnectionString);
            builder.InitialCatalog = ddlDatabase.SelectedValue;
            //TODO Baterfy change database
            //builder.InitialCatalog = "Baterfy";
            dc.Connection.ConnectionString = builder.ConnectionString;
            System.Web.HttpCookie c = new System.Web.HttpCookie("ConnectionStringV2");
            c.Value = Server.UrlEncode(builder.ConnectionString);
            c.Expires = DateTime.Now.AddYears(ddlDatabase.SelectedIndex == 0 || ddlDatabase.Items.Count < 2 ? -1 : 1);
            Response.Cookies.Add(c);
            Response.Redirect(Request.Url.ToString(), false);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
        }
    }


    private void FillDatabases()
    {
        try
        {      // 




            //  System.Data.Common.DbConnectionStringBuilder builder = new System.Data.Common.DbConnectionStringBuilder();
            //  builder.ConnectionString = ConfigurationManager.ConnectionStrings["XpressConnectionString"].ConnectionString; ;
            ddlDatabase.DataSource = dc.usp_Databases_Select(ConfigurationManager.AppSettings.Get("DatabaseNameInit"));//builder["Initial Catalog"].ToString());
            ddlDatabase.DataTextField = "Name";
            ddlDatabase.DataValueField = "Name";
            ddlDatabase.DataBind();
            ddlDatabase.Items.Insert(0, new ListItem(Resources.Labels.Default, "-1"));
            if (ddlDatabase.Items.FindByValue(dc.Connection.Database.ToUpper()) != null)
            {
                ddlDatabase.Items.FindByValue(dc.Connection.Database.ToUpper()).Selected = true;
            }
            else
            {
                ddlDatabase.SelectedIndex = 0;
                this.ddlDatabase_SelectedIndexChanged(null, null);
            }
        }
        catch (Exception ex)
        {
            System.Web.HttpCookie c = new System.Web.HttpCookie("ConnectionStringV2");
            c.Value = null;
            c.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(c);
            Logger.LogError(ex);
        }
    }

    protected void Login_Click(object sender, EventArgs e)
    {
        try
        {

            if (Membership.ValidateUser(txtUser.Text, txtPassword.Text))
            //if (true)
            {

                FormsAuthentication.SetAuthCookie(txtUser.Text, false);
                global::MyContext context = new global::MyContext(Membership.GetUser(txtUser.Text));
                //var hrEmpl = dc.HR_Employees.Where(x => x.Contact_ID == context.UserProfile.Contact_ID).FirstOrDefault();
                //if (hrEmpl != null)
                //{
                //    if (!hrEmpl.IsStoped.Value || !hrEmpl.IsSystemUser.Value)
                //    {
                //        context = null;
                //        lblMessage.Text = Resources.Validations.invCred;
                //        lblMessage.ForeColor = System.Drawing.Color.FromArgb(230, 94, 94);
                //        return;
                //    }
                //}
                Session["PreferedCulture"] = (int)context.UserProfile.UserCulture;
                InitializeCulture();
                var z = PageLinks.MainPage;
                Response.Redirect(PageLinks.MainPage, true);
            }
            else
            {
                lblMessage.Text = Resources.Validations.invCred;
                lblMessage.ForeColor = System.Drawing.Color.FromArgb(230, 94, 94);
            }
        }
        catch (System.Threading.ThreadAbortException tex)
        {
            //dont log that
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
        }
    }
}