using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;

public partial class Main_MyDefault : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();
    protected MyContext MyLocalContext = new MyContext();

    private List<NotificationsMenu> dtNotifications
    {
        get
        {
            return (List<NotificationsMenu>)Session["dtNotifications" + this.WinID];
        }

        set
        {
            Session["dtNotifications" + this.WinID] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!User.Identity.IsAuthenticated) //FormsAuthentication.RedirectToLoginPage();
            {
                string jScript = "window.top.location.href = '" + PageLinks.LoginPageForce + "';";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "forceParentLoad", jScript, true);
            }
            if (!Page.IsPostBack)
            {
                SetWorkingMode();
                XpressDataContext dataContext = new XpressDataContext();
                var dtSideNavigation = (from sideNav in dataContext.usp_PageMenu_Select(this.MyLocalContext.UserProfile.Contact_ID)
                                        where isPageExists(sideNav.rawPageUrl)
                                        select sideNav).OrderBy(r=>r.Parent_ID).CopyToDataTable();
                
                string PageNameCol = this.MyContext.CurrentCulture == ABCulture.Arabic ? "PageName" : "PageName_en";
                string Right = Resources.Labels.CssRight;
                string Left = Resources.Labels.CssLeft;

                string sidemenu = DrawTreeOfRoles(dtSideNavigation, 0, PageNameCol, Right, Left);
                sidemenu = "<div class='nicescroll-bar' style='text-align:" + Right + ";padding-" + Right + ": 10px; text-color:black'>" + sidemenu + "</div>";
                myDiv.InnerHtml = sidemenu;

                try
                {
                    /*lnkAttachments.Attributes.Add("DocumentURI", Request.AppRelativeCurrentExecutionFilePath + Request.Url.Query);
                    lnkAttachments.Attributes.Add("DocumentPath", Request.AppRelativeCurrentExecutionFilePath);
                    lnkAttachments.Attributes.Add("DocumentPathInfo", Request.PathInfo);
                    lnkAttachments.Visible = this.MyContext.PageData.IsAttach && (this.Req_ID != null || Request["ID"] != null) && Request["ViewInPopupMode"] == null;
                    if (lnkAttachments.Visible) lnkAttachments.Text += string.Format("({0})", dc.usp_Attachments_Select(Request.AppRelativeCurrentExecutionFilePath + Request.Url.Query).Count());*/
                }
                catch (Exception ex)
                {
                    Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
                }

                lblFiscalYearStartDate.Text = lblFiscalYearStartDate2.Text = this.MyLocalContext.FiscalYearStartDate.ToString("d/M/yyyy");
                lblFiscalYearStartDate2.Text = this.MyLocalContext.FiscalYearStartDate.ToString("d/M/yyyy");
                lblDatabaseName2.Text = dataContext.Connection.Database;
                if (!string.IsNullOrWhiteSpace(MyLocalContext.UserProfile.BranchName))
                    lblBranchName.Text = MyLocalContext.UserProfile.BranchName;
                else lblBranchName.Text = string.Empty;
                lblTheme.Text = Resources.Labels.Theme;
                lblDatabaseName.Text = lblDatabaseName2.Text = dataContext.Connection.Database;

                var company = dc.usp_Company_Select().FirstOrDefault();

                ItemsDetailss C = new ItemsDetailss();

                /*string LicRawData = C.Decrypt(company.lic);

                var dF = DateTime.ParseExact(LicRawData.Split("@".ToCharArray())[1], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var dn = DateTime.UtcNow.AddHours(2).Date;
                lblVersionDay.Text = " ينتهي الترخيص بعد " + (dF - dn).TotalDays.ToExpressString(); ;
                lblVersionDay2.Text = " ينتهي الترخيص بعد " + (dF - dn).TotalDays.ToExpressString(); ;*/
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
        }

        var favorits = dc.Favorits.Where(x => x.Contacty_ID == MyLocalContext.UserProfile.Contact_ID).ToList();
        string favoritshtml = string.Empty;
        DataAccessLayer DAL = new DataAccessLayer();
        foreach (var item in favorits)
        {
            var QueryResult = DAL.ExecuteAnQuery("select * from Pages where ID=" + item.Page_ID);
            if (QueryResult != null && QueryResult.Rows.Count == 1)
            {
                var alt = MyLocalContext.CurrentCulture == ABCulture.Arabic ? QueryResult.Rows[0]["PageName"] : QueryResult.Rows[0]["PageName_en"];
                string PageUrl = QueryResult.Rows[0]["PageUrl"].ToString();
                favoritshtml = favoritshtml +
                    "<a href='#' class='col-md-1'><img class='img-thumbnail' height='50' width='50' " +
                    "alt='" + alt + "' title='" + alt + "' src='" + Page.ResolveClientUrl(QueryResult.Rows[0]["ImageUrl"].ToString()) +
                    "' onclick=\"$('html').scrollTop(0);document.getElementById('MainIframe').src='" + Page.ResolveClientUrl(PageUrl) + "';resizeIframeMy();\"" +
                    "/></a>";
            }
        }

        shortcutsDiv.InnerHtml = "<div class='text-center'>" + favoritshtml + "</div>";
    }

    private void SetWorkingMode()
    {
        switch (MyLocalContext.Features.WorkingMode)
        {
            case (byte)WorkingMode.HR:
                //home_logo.Src = "~/Images/logo_hr.png";
                this.Title = "Auditor HR";
                break;
            case (byte)WorkingMode.Stores:
                //home_logo.Src = "~/Images/logo_stores.png";
                this.Title = "Auditor Stores";
                break;
            case (byte)WorkingMode.Xpress:
                // home_logo.Src = "~/Images/logo.png";
                this.Title = "Auditor Erp";
                break;
        }
    }

    string DrawTreeOfRoles(System.Data.DataTable Pages, Int64 Id, string PageNameCol, string right, string left)
    {
        string Result = string.Empty;
        for (int i = 0; i < Pages.Rows.Count; i++)
        {
            Int64 Parent_ID = 0;
            if (Pages.Rows[i]["Parent_ID"] != DBNull.Value)
                Parent_ID = Convert.ToInt64(Pages.Rows[i]["Parent_ID"]);            
            if (Parent_ID == Id)
            {
                /*bool Value = false;
                if (Roles.Rows[i]["Value"] != DBNull.Value)
                    Value = Convert.ToBoolean(Roles.Rows[i]["Value"]);*/

                if (true)
                {
                    Int64 NavigationLevel = -1;
                    if (Pages.Rows[i]["NavigationLevel"] != DBNull.Value)
                        NavigationLevel = Convert.ToInt64(Pages.Rows[i]["NavigationLevel"]);

                    bool SearchFlage = NavigationLevel == 1;
                    if (!SearchFlage)
                    {
                        bool IsExpanded = false;
                        string collapseClass = "collapse";
                        string Style1 = "",Style2="";
                        if (Parent_ID == 0)
                        {
                            Style1 = "font-size:18px;";
                            Style2 = "height:50px;";
                        }
                        Int64 ID = Convert.ToInt64(Pages.Rows[i]["ID"]);
                        string TagId = "Node" + ID;
                        string Href = "#Node" + ID;
                        string SpanId = "Span" + ID;
                        if (Pages.Columns.Contains("IsExpanded") && Pages.Rows[i]["IsExpanded"] != DBNull.Value)
                        {
                            IsExpanded = Convert.ToBoolean(Pages.Rows[i]["IsExpanded"]);
                            collapseClass = IsExpanded ? string.Empty : "collapse";
                        }
                        Result += "<a href='#' class='roleFont' data-toggle='collapse' style='text-align:" + right + ";color:white;vertical-align:bottom;"+Style1+" ' " +
                           "onclick=\"$('#" + SpanId + "').toggleClass('fa fa-minus').toggleClass('fa fa-plus');$('#" + TagId + "').toggle();\">" +
                           "<i class='fa fa-angle-" + left + "' style='" + Style2 + "'></i> " + Pages.Rows[i][PageNameCol] + "<span id='" + SpanId + "' class='fa fa-plus' style='float:" + left +
                           ";position: absolute;" + left + ": 10px;margin-top:10px;'/></a><br/>" +
                           "<ul class='"+ collapseClass + " list-unstyled myUL' id='" + TagId + "' style='text-align:" + right + ";padding-" + right + ":20px;padding-" + left + ":10px;color:white;'>" +
                            DrawTreeOfRoles(Pages, ID, PageNameCol, right, left) + "</ul>";
                    }
                    else
                    {
                        string ActionUrl = "#";
                        if (Pages.Rows[i]["PageUrl"] != DBNull.Value)
                        {
                            string ID = Pages.Rows[i]["ID"].ToString();
                            string Title = Pages.Rows[i][PageNameCol].ToString();
                            string PageUrl = Pages.Rows[i]["PageUrl"].ToString();
                            ActionUrl = Page.ResolveClientUrl(PageUrl);
                            //ActionUrl = PageUrl;                            
                            Result += "<a href='javascript:void(0);' class= 'roleFont' data-link='" + ActionUrl + "'" +
                                //" onclick=\"$('html').scrollTop(0);document.getElementById('MainIframe').src='" + Page.ResolveClientUrl(PageUrl) + "';resizeIframeMy();\"" +
                                " onclick=\"setForm('" + Page.ResolveClientUrl(PageUrl) + "');\"" +
                                " style = 'text-align:" + right + ";color:white'>" +
                                           "<i class='fa fa-angle-" + left + "'></i> " + Pages.Rows[i][PageNameCol] + "</a><br/>";
                        }
                        else
                        {
                            Result += "<a href='javascript:void(0);' class= 'roleFont' data-link='" + ActionUrl + "' style = 'text-align:" + right + ";color:white'><i class='fa fa-angle-" + left + "'></i>" + Pages.Rows[i][PageNameCol] + "</a><br/>";
                        }
                    }
                }
            }
        }
        return Result;
    }

    private bool isPageExists(string url)
    {
        if (string.IsNullOrEmpty(url)) return true;
        return File.Exists(Server.MapPath(url));
    }

    protected override void OnInit(EventArgs e)
    {
        this.MyContext = new MyContext();
        base.OnInit(e);
    }

    protected void lnkChangeLang_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["PreferedCulture"] == null)
            {
                Session["PreferedCulture"] = (int)ABCulture.Arabic;
            }
            else if (Session["PreferedCulture"].ToInt() == (int)ABCulture.English)
            {
                Session["PreferedCulture"] = (int)ABCulture.Arabic;
            }
            else if (Session["PreferedCulture"].ToInt() == (int)ABCulture.Arabic)
            {
                Session["PreferedCulture"] = (int)ABCulture.English;
            }
            Response.Redirect(Page.Request.Url.ToString(), false);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            Response.Redirect(Page.Request.Url.ToString(), false);
        }
    }

    protected void lnkLogOut_Click(object sender, EventArgs e)
    {
        try
        {
            int culture = 0;
            if (Session["PreferedCulture"] != null && Session["PreferedCulture"].ToString() != string.Empty)
            {
                culture = Session["PreferedCulture"].ToInt();
            }
            Session.Clear();
            Session["PreferedCulture"] = culture;

            FormsAuthentication.SignOut();
            // FormsAuthentication.RedirectToLoginPage();
            string jScript = "window.top.location.href = '" + PageLinks.LoginPageForce + "';";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "forceParentLoad", jScript, true);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
        }
    }

    [WebMethod]
    public static List<usp_CurrentDocuments_SelectResult> GetCurrentNotificationDocs()
    {
        #region Function
        XpressDataContext dc = new XpressDataContext();
        MyContext MyLocalContext = new MyContext();
        var dtCurrentNotificationDocs = dc.usp_CurrentDocuments_Select(0, null).ToList();
        #endregion
        return dtCurrentNotificationDocs;
    }

    /******************************************************************/
    [WebMethod]
    public static List<NotificationsMenu> GetListNotification()
    {

        List<NotificationsMenu> dtNotifications = new List<NotificationsMenu>();
        if (dtNotifications == null)
        {
            dtNotifications = new List<NotificationsMenu>();
        }
        dtNotifications = FillgvItemsMinQty(dtNotifications);
        dtNotifications = FillgvReceivedChecks(dtNotifications);
        dtNotifications = FillgvIssuedChecks(dtNotifications);
        dtNotifications = FillgvPeriodicMaintain(dtNotifications);
        dtNotifications = FillgvExpiredItems(dtNotifications);
        dtNotifications = FillgvLoansInstallments(dtNotifications);
        dtNotifications = FillgvEmployeesVisa(dtNotifications);
        dtNotifications = FillgvCustomersInstallments(dtNotifications);
        dtNotifications = FillAttachment(dtNotifications);
        return dtNotifications;
    }

    private static decimal GetDecimal(decimal? val)
    {
        string number = Convert.ToString(val);
        if (number.Contains("."))
        {
            while (number.Substring(number.Length - 1) == "0")
            {
                number = number.Substring(0, number.Length - 1);
            }
        }
        return Convert.ToDecimal(number);
    }

    #region temsMinQty
    private static List<NotificationsMenu> FillgvItemsMinQty(List<NotificationsMenu> dtNotifications)
    {
        XpressDataContext dc = new XpressDataContext();
        //this.dtItemsMinQty = dc.usp_Notf_UnderMinQty().ToList();
        dtNotifications.AddRange(dc.usp_Notf_UnderMinQty().Select(x => new NotificationsMenu(x.ItemName + " " + x.Qty + " : " + x.MiniQty, Resources.Labels.UnderMinQty)));
        return dtNotifications;
    }

    private static List<NotificationsMenu> FillAttachment(List<NotificationsMenu> dtNotifications)
    {
        XpressDataContext dc = new XpressDataContext();
        //this.dtItemsMinQty = dc.usp_Notf_UnderMinQty().ToList();
        var list = dc.usp_Notf_AttachmentExpiration();
        dtNotifications.AddRange(list.Select(x => new NotificationsMenu(x.EndDate.ToDate().Value + " " + x.Name + " : " + x.FileName, " مدة الوثيقة  ستنتهي")));
        return dtNotifications;
    }
    #endregion

    #region ReceivedChecks
    private static List<NotificationsMenu> FillgvReceivedChecks(List<NotificationsMenu> dtNotifications)
    {
        XpressDataContext dc = new XpressDataContext();
        MyContext MyLocalContext = new MyContext();
        //this.dtReceivedChecks = dc.usp_Notf_ReceivedChecks(MyContext.UserProfile.Branch_ID).CopyToDataTable();
        dtNotifications.AddRange(dc.usp_Notf_ReceivedChecks(MyLocalContext.UserProfile.Branch_ID).Select(x => new NotificationsMenu(x.AccountNames + " " + x.Serial + " " + x.IssueDate + " " + x.TotalAmount, Resources.Labels.ReceivedChecks)));
        return dtNotifications;
    }
    #endregion

    #region IssuedCheck
    private static List<NotificationsMenu> FillgvIssuedChecks(List<NotificationsMenu> dtNotifications)
    {
        XpressDataContext dc = new XpressDataContext();
        MyContext MyLocalContext = new MyContext();
        //this.dtIssuedChecks = dc.usp_Notf_IssuedChecks(MyContext.UserProfile.Branch_ID).CopyToDataTable();
        dtNotifications.AddRange(dc.usp_Notf_IssuedChecks(MyLocalContext.UserProfile.Branch_ID).Select(x => new NotificationsMenu(x.AccountNames + " " + x.Serial + " " + x.TotalAmount, Resources.Labels.IssuedChecks)));
        return dtNotifications;
    }
    #endregion

    #region PeriodicMaintain
    private static List<NotificationsMenu> FillgvPeriodicMaintain(List<NotificationsMenu> dtNotifications)
    {
        XpressDataContext dc = new XpressDataContext();
        MyContext MyLocalContext = new MyContext();
        //this.dtPeriodicMaintain = dc.usp_Notf_PeriodicMaintain(MyContext.UserProfile.Branch_ID).CopyToDataTable();
        dtNotifications.AddRange(dc.usp_Notf_PeriodicMaintain(MyLocalContext.UserProfile.Branch_ID).Select(x => new NotificationsMenu(x.AssetName + " " + x.ExpenseName, Resources.Labels.AssetsPeriodicMaintainFollowUP)));
        return dtNotifications;
    }
    #endregion

    #region ExpiredItems
    private static List<NotificationsMenu> FillgvExpiredItems(List<NotificationsMenu> dtNotifications)
    {
        XpressDataContext dc = new XpressDataContext();
        MyContext MyLocalContext = new MyContext();
        //this.dtExpiredItems = dc.usp_Notf_ItemsExpiration().CopyToDataTable();
        dtNotifications.AddRange(dc.usp_Notf_ItemsExpiration().Select(x => new NotificationsMenu(x.ItemName + " ( " + GetDecimal(x.Qty) + " ) " + x.ExpirationDate.Value.ToShortDateString(), Resources.Labels.ExpiredItems)));
        return dtNotifications;
    }
    #endregion

    #region LoansInstallments
    private static List<NotificationsMenu> FillgvLoansInstallments(List<NotificationsMenu> dtNotifications)
    {
        XpressDataContext dc = new XpressDataContext();
        MyContext MyLocalContext = new MyContext();
        // this.dtLoansInstallments = dc.usp_Notf_LoansInstallemnts(MyContext.UserProfile.Branch_ID).CopyToDataTable();
        dtNotifications.AddRange(dc.usp_Notf_LoansInstallemnts(MyLocalContext.UserProfile.Branch_ID).Select(x => new NotificationsMenu(x.Serial + " " + x.InstallmentDate + " " + x.Total, Resources.Labels.LoansInstallments)));
        return dtNotifications;
    }
    #endregion

    #region CustomersInstallments
    private static List<NotificationsMenu> FillgvCustomersInstallments(List<NotificationsMenu> dtNotifications)
    {
        XpressDataContext dc = new XpressDataContext();
        MyContext MyLocalContext = new MyContext();
        //this.dtCustomersInstallments = dc.usp_Notf_CustomerInstallemnts(MyContext.UserProfile.Branch_ID).CopyToDataTable();
        dtNotifications.AddRange(dc.usp_Notf_CustomerInstallemnts(MyLocalContext.UserProfile.Branch_ID).Select(x => new NotificationsMenu(x.Serial, Resources.Labels.CustomersInstallments)));
        return dtNotifications;
    }
    #endregion

    #region EmployeesVisa
    private static List<NotificationsMenu> FillgvEmployeesVisa(List<NotificationsMenu> dtNotifications)
    {
        XpressDataContext dc = new XpressDataContext();
        MyContext MyLocalContext = new MyContext();
        // this.dtEmployeesVisa = dc.usp_Notf_EmployeesVisa(MyContext.UserProfile.Branch_ID).CopyToDataTable();
        dtNotifications.AddRange(dc.usp_Notf_EmployeesVisa(MyLocalContext.UserProfile.Branch_ID).Select(x => new NotificationsMenu(x.Serial, Resources.Labels.EmployeesVisa)));
        return dtNotifications;
    }
    #endregion                
}