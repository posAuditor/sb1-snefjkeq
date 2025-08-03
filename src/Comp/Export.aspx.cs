using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPRESS.Common;

public partial class Comp_Export : System.Web.UI.Page
{
    XpressDataContext dc = new XpressDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Items"]))
            {
                var dtGeneralJournalList = dc.usp_ExportItems().ToList();

                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=Items.xls");
                Response.ContentType = "application/ms-excel";
                Response.ContentEncoding = System.Text.Encoding.Unicode;
                Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
                WriteTsv(dtGeneralJournalList, Response.Output);
                Response.End();
            }
        }


        try
        {
            Response.Redirect(PageLinks.ItemsList, true);
        }
        catch (Exception ex)
        {
            Logger.LogError(Resources.UserInfoMessages.OperationFailed, ex);
        }

    }



    public void WriteTsv<T>(IEnumerable<T> data, TextWriter output)
    {
        PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
        foreach (PropertyDescriptor prop in props)
        {
            output.Write(prop.DisplayName); // header
            output.Write("\t");
        }
        output.WriteLine();
        foreach (T item in data)
        {
            foreach (PropertyDescriptor prop in props)
            {
                output.Write(prop.Converter.ConvertToString(
                     prop.GetValue(item)));
                output.Write("\t");
            }
            output.WriteLine();
        }
    }



    //public void WriteTsv<T>(IEnumerable<T> data, TextWriter output)
    //{
    //    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

    //    foreach (PropertyDescriptor prop in props)
    //    {
    //        output.Write("Trn Code	Trn No	Inv Date	Acc Code	Cost Acc Code	Amount	Description"); // header

    //    }

    //    foreach (T item in data)
    //    {

    //        foreach (PropertyDescriptor prop in props)
    //        {

    //            output.Write(prop.Converter.ConvertToString(prop.GetValue(item)));
    //            output.Write("\t");

    //        }

    //        output.WriteLine();
    //    }
    //}
}