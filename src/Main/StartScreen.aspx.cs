using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;

public partial class Main_StartScreen : UICulturePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.SetWorkingMode();
        StartScreen.Visible = MyContext.PageData.IsViewDoc;
        ConErr2.Visible = ConErr.Visible = Request["ConErr"] != null;


        //lnkPund.NavigateUrl = "~/AccountingReports/AccountStatment.aspx";
        //lnkBanks.NavigateUrl = "~/AccountingReports/AccountStatment.aspx";
        //lnkPurshase.NavigateUrl = "~/AccountingReports/AccountStatment.aspx";
        //lnkSales.NavigateUrl = "~/AccountingReports/AccountStatment.aspx";
        //lnkValueCustomer.NavigateUrl = "~/AccountingReports/CustomerStatment.aspx";
        //lnkVendors.NavigateUrl = "~/AccountingReports/VendorStatment.aspx";
        //lnkVendorsServices.NavigateUrl = PageLinks.VendorsList;
        //lnkClientService.NavigateUrl = PageLinks.CustomersList;

        


    }

    private void SetWorkingMode()
    {
        switch (MyContext.Features.WorkingMode)
        {
            case (byte)WorkingMode.HR:
                // MainImage.Src = "~/Images/splash_hr.jpg";
                break;
            case (byte)WorkingMode.Stores:
                // MainImage.Src = "~/Images/splash_stores.jpg";
                break;
            case (byte)WorkingMode.Xpress:
                // MainImage.Src = "~/Images/splash.jpg";
                break;
        }
    }

}