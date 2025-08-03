using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;

public partial class Main_StartScreenDP : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        this.SetWorkingMode();
        var comp = dc.usp_Company_Select().FirstOrDefault();
        var url = Page.ResolveClientUrl("~/Uploads/" + comp.Background);
        if (!string.IsNullOrEmpty(comp.Background))
        {
            img.Visible = true;
            img.ImageUrl = url;
        }
        else
        {
            img.Visible = false;
        }

        var company = dc.usp_Company_Select().FirstOrDefault();

        ItemsDetailss C = new ItemsDetailss();

        /*string LicRawData = C.Decrypt(company.lic);

        var dF = DateTime.ParseExact(LicRawData.Split("@".ToCharArray())[1], "dd/MM/yyyy", CultureInfo.InvariantCulture);
        var dn = DateTime.UtcNow.AddHours(2).Date;
        lblVersionDay.Text = (dF - dn).TotalDays == 0 ? " إنتهى الترخيص " : " ينتهي الترخيص بعد " + (dF - dn).TotalDays.ToExpressString(); ;
        */
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