using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Accounting_ExportJEntry : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        XpressDataContext dc = new XpressDataContext();
        int? Currency_ID = null;
        var dtGeneralJournalList = dc.usp_GeneralJournal_Export(null, null, null, "", Currency_ID, null, null).ToList();

        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=Test.xls");
        Response.ContentType = "application/ms-excel";
        Response.ContentEncoding = System.Text.Encoding.Unicode;
        Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());


        WriteTsv(dtGeneralJournalList, Response.Output);
        Response.End();




    }


    public void WriteTsv<T>(IEnumerable<T> data, TextWriter output)
    {
        PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

        foreach (PropertyDescriptor prop in props)
        {
            //if (prop.DisplayName != "ID")
            // {
            output.Write(prop.DisplayName); // header
            output.Write("\t");
            // }


        }
        output.WriteLine();

        foreach (T item in data)
        {
            var j = 0;
            foreach (PropertyDescriptor prop in props)
            {
                // if (j != 0)
                //  {
                output.Write(prop.Converter.ConvertToString(
                     prop.GetValue(item)));
                output.Write("\t");
                // }
                //j++;
            }

            output.WriteLine();
        }
    }
}