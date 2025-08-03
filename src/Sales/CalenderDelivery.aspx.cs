using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;
public partial class Sales_CalenderDelivery : UICulturePage
{
    XpressDataContext dc = new XpressDataContext();

    private DataTable dtCurrenteInvoice
    {
        get
        {
            if (Session["dtCurrenteInvoice" + this.WinID] == null)
            {
                Session["dtCurrenteInvoice" + this.WinID] = dc.Invoices.Where(x => x.DateDelivery == DateTime.Now.Date);
            }
            return (DataTable)Session["dtCurrenteInvoice" + this.WinID];
        }
        set
        {
            Session["dtCurrenteInvoice" + this.WinID] = value;
        }
    }
    public DateTime CurrentDate
    {
        get
        {
            if (Session["CurrentDate_Invoice" + this.WinID] == null)
                return DateTime.Now;
            return (DateTime)Session["CurrentDate_Invoice" + this.WinID];
        }
        set
        {
            Session["CurrentDate_Invoice" + this.WinID] = value;
        }
    }
    public List<InvoiceCalender> Generate(int number)
    {
        List<InvoiceCalender> lst = new List<InvoiceCalender>();
        if (number == 1)
        {
            return new List<InvoiceCalender> {
                  new InvoiceCalender() { Day = "",NumberInvoice=0 } };
        }
        else if (number == 2)
        {
            return new List<InvoiceCalender> {
                   new InvoiceCalender() { Day = "" ,NumberInvoice=0}
                ,  new InvoiceCalender() { Day = "" ,NumberInvoice=0} };
        }
        else if (number == 3)
        {
            return new List<InvoiceCalender> {
                  new InvoiceCalender() { Day = "",NumberInvoice=0 }
                , new InvoiceCalender() { Day = "" ,NumberInvoice=0}
                , new InvoiceCalender() { Day = "" ,NumberInvoice=0}};
        }
        else if (number == 4)
        {
            return new List<InvoiceCalender> {
                  new InvoiceCalender() { Day = "",NumberInvoice=0 }
                , new InvoiceCalender() { Day = "",NumberInvoice=0 }
                , new InvoiceCalender() { Day = "",NumberInvoice=0 }
                , new InvoiceCalender() { Day = "",NumberInvoice=0 }};
        }
        else if (number == 5)
        {
            return new List<InvoiceCalender> {
                  new InvoiceCalender() { Day = "",NumberInvoice=0 }
                , new InvoiceCalender() { Day = "",NumberInvoice=0 }
                , new InvoiceCalender() { Day = "",NumberInvoice=0 }
                , new InvoiceCalender() { Day = "",NumberInvoice=0 }
                , new InvoiceCalender() { Day = "",NumberInvoice=0 }};
        }
        else if (number == 6)
        {
            return new List<InvoiceCalender> { 
                  new InvoiceCalender() { Day = "",NumberInvoice=0 }
                , new InvoiceCalender() { Day = "",NumberInvoice=0 }
                , new InvoiceCalender() { Day = "",NumberInvoice=0 }
                , new InvoiceCalender() { Day = "",NumberInvoice=0 }
                , new InvoiceCalender() { Day = "",NumberInvoice=0 }
                , new InvoiceCalender() { Day = "",NumberInvoice=0 }};
        }
        return lst;


    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Fill();
        }
    }
    public string GetMonthNamesByCulture(CultureInfo culture, int number)
    {

        return culture.DateTimeFormat.GetMonthName(number);


    }
    private void Fill()
    {
        var lst = new List<InvoiceCalender>();
        int i = 0;
        var ls = GetDates(CurrentDate.Year, CurrentDate.Month).ToList();

        lblMonth.Text = GetMonthNamesByCulture(new CultureInfo("ar-AE"), CurrentDate.Month);
        lblYear.Text = CurrentDate.Year.ToString();
        foreach (var item in ls)
        {
            if (i == 0)
            {
                var dow = item.DayOfWeek;
                switch (dow)
                {
                    case DayOfWeek.Monday:
                        break;
                    case DayOfWeek.Tuesday:
                        lst.AddRange(Generate(1));
                        break;
                    case DayOfWeek.Wednesday:
                        lst.AddRange(Generate(2));
                        break;
                    case DayOfWeek.Thursday:
                        lst.AddRange(Generate(3));
                        break;
                    case DayOfWeek.Friday:
                        lst.AddRange(Generate(4));
                        break;
                    case DayOfWeek.Saturday:
                        lst.AddRange(Generate(5));
                        break;
                    case DayOfWeek.Sunday:
                        lst.AddRange(Generate(6));
                        break;
                }
            }
            var dateGenerate = new DateTime(CurrentDate.Year, CurrentDate.Month, item.Day);
            var countInvoice = dc.Invoices.Count(x => x.DateDelivery == dateGenerate);
            lst.Add(new InvoiceCalender()
            {
                Day = item.Day.ToString(),
                NumberInvoice = countInvoice
            });
            i++;
        }
        rptDays.DataSource = lst;
        rptDays.DataBind();
    }

    public static List<DateTime> GetDates(int year, int month)
    {
        return Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                         .Select(day => new DateTime(year, month, day)) // Map each day to a date
                         .ToList(); // Load dates into a list
    }

    public string GetStyle(string val, int number, int numberInvoice)
    {
        if (val == string.Empty)
            return "";
        else
            if (numberInvoice > 0)
                if (int.Parse(val) == number)
                    return "DaysInvoiceActive";
                else
                    return "DaysInvoice";
            else
            {
                if (number == int.Parse(val))
                {
                    return "active";
                }
            }
        return string.Empty;
    }
    public bool GetStyleVisibility(string val, int number, int numberInvoice)
    {
        if (val == string.Empty)
            return false;
        else
            if (numberInvoice > 0)
                return false;
            else
            {
                if (val.ToIntOrDefault() >= number && CurrentDate.Month >= DateTime.Now.Month)
                {
                    return true;
                }
                else
                    if (CurrentDate.Month > DateTime.Now.Month && val.ToIntOrDefault() <= number)
                    {
                        return true;
                    }
                return false;

            }
        return false;
    }




    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        CurrentDate = CurrentDate.AddMonths(-1);
        Fill();
    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        CurrentDate = CurrentDate.AddMonths(1);
        Fill();
    }
    protected void lnkBtnAddInvoice_Click(object sender, EventArgs e)
    {
        var cmd = (sender as LinkButton).CommandArgument;
        Response.Redirect(PageLinks.InvoiceShortcut + "?DateDelivery=" + CurrentDate.Year.ToExpressString() + "-" + CurrentDate.Month.ToExpressString() + "-" + cmd);
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        var cmd = int.Parse((sender as LinkButton).CommandArgument);
        var year = CurrentDate.Year;
        var Month = CurrentDate.Month;
        var date = new DateTime(year, Month, cmd);
        gvItemsInvoice.DataSource = dc.Invoices.Where(x => x.DateDelivery == date).ToList();
        gvItemsInvoice.DataBind();
        mpeCalender.Show();
    }
    protected void lnkButtonMoveInvoice_Click(object sender, EventArgs e)
    {
        var cmd = (sender as LinkButton).CommandArgument;
        Response.Redirect(PageLinks.InvoiceShortcut + "?ID=" + cmd);
    }
}
public class InvoiceCalender
{

    public int NumberInvoice
    {
        get;
        set;
    }
    public string Day { get; set; }

}