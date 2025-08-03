using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CustomControls_ucLover : System.Web.UI.UserControl
{
    XpressDataContext dc = new XpressDataContext();

    public bool IsFavorite
    {
        get
        {
            if (ViewState["IsFavorite"] == null) return false;
            return (bool)ViewState["IsFavorite"];
        }

        set
        {
            ViewState["IsFavorite"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
            MyContext con = new MyContext(System.Web.Security.Membership.GetUser(), Request.AppRelativeCurrentExecutionFilePath, string.Empty);
            var lst = dc.Favorits.Where(x => x.Contacty_ID == con.UserProfile.Contact_ID && x.Page_ID == con.PageData.PageID).ToList();
            if (lst.Any())
            {
               // IsFavorite = true;
                idfav.Attributes.Add("style", "font-size: 30px; padding-top: 0; color: red!important");
            }
            else
            {
               // IsFavorite = false;
                idfav.Attributes.Add("style", "font-size: 30px; padding-top: 0; color: blue!important");
            }
      
    }
    protected void lnkFavorit_Click(object sender, EventArgs e)
    {

        MyContext con = new MyContext(System.Web.Security.Membership.GetUser(), Request.AppRelativeCurrentExecutionFilePath, string.Empty);
        var lst = dc.Favorits.Where(x => x.Contacty_ID == con.UserProfile.Contact_ID && x.Page_ID == con.PageData.PageID).ToList();
        if (lst.Any())
        {
            dc.Favorits.DeleteOnSubmit(lst.First());
            idfav.Attributes.Add("style", "font-size: 30px; padding-top: 0; color: blue!important");
        }
        else
        {
            dc.Favorits.InsertOnSubmit(new Favorit() { Contacty_ID = con.UserProfile.Contact_ID, Page_ID = con.PageData.PageID });
            idfav.Attributes.Add("style", "font-size: 30px; padding-top: 0; color: red!important");
        }
        dc.SubmitChanges();
    }
}