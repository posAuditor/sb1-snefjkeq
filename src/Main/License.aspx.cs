using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;

public partial class Main_License : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    private ServerData server
    {
        get
        {
            return (ServerData)HttpContext.Current.Session["X_Server"];
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        msgArabic.Visible = this.MyContext.CurrentCulture == ABCulture.Arabic;
        msgEng.Visible = this.MyContext.CurrentCulture == ABCulture.English;

        lblReasonNumber.Text = Request["No"].ToStringOrEmpty();
        lblReason.Text = Request["Reason"].ToStringOrEmpty();

        lblReasonNumberEng.Text = Request["No"].ToStringOrEmpty();
        lblReasonEng.Text = Request["Reason"].ToStringOrEmpty();
    }

    protected void lnkReLicense_Click(object sender, EventArgs e)
    {
        try
        {
            var company = dc.usp_Company_Select().FirstOrDefault();
            XItems.AuthSoapClient Auth = new XItems.AuthSoapClient("AuthSoap", "http://xsec.auditorerp.cloud/Auth.asmx");
            var response = Auth.RequestLic(this.server.ServerHash, this.getRawData(), this.server.ServerRawData);
            if (response.Customer_ID == 0 || string.IsNullOrEmpty(response.lic))
            {
                UserMessages.Message(null, Resources.UserInfoMessages.LicenseRequestDenied, string.Empty);
                return;
            }
            company.lic = response.lic;
            company.slic = response.Sign;
            dc.SubmitChanges();
            Auth.ACK(response.Customer_ID, response.Lic_ID);
            UserMessages.Message(null, Resources.UserInfoMessages.OperationSuccess, PageLinks.StartScreen);

        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }
    }

    private string getRawData()
    {
        string rawData = string.Empty;

        var customers = from data in dc.dbCustomers select new { data.ID, data.Contact_ID, ChartOfAccount_ID = data.ChartOfAccount_ID };
        var vendors = from data in dc.dbVendors select new { data.ID, data.Contact_ID, ChartOfAccount_ID = data.ChartOfAccount_ID };
        var employees = from data in dc.dbHR_Employees select new { data.ID, data.Contact_ID, ChartOfAccount_ID = data.SalaryChartOfAccount_ID };
        int? item = (from data in dc.dbItems select (int?)data.ID).Max();
        if (item == null) item = 0;

        var AllData = customers.Union(vendors).Union(employees).ToList();
        var SortedData = from data in AllData orderby data.Contact_ID select data.ID.ToString() + "-" + data.Contact_ID.ToString() + "-" + data.ChartOfAccount_ID.ToString();
        rawData += string.Join("\r\n", SortedData.ToArray());
        rawData += "X" + item.ToString();
        return rawData;
    }
}