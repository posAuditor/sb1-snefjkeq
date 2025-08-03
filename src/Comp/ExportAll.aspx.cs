using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Comp_ExportAll : System.Web.UI.Page
{
    XpressDataContext dc = new XpressDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        //var listCt = dc.usp_ExportAll(Request.QueryString["ID"]).ToList();
        //Response.Clear();
        //Response.AddHeader("content-disposition", "attachment;filename=" + Request.QueryString["ID"] + ".xls");
        //Response.ContentType = "application/ms-excel";
        //Response.ContentEncoding = System.Text.Encoding.Unicode;
        //Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
        //WriteTsv(listCt, Response.Output);
        //Response.End();

        switch (Request.QueryString["ID"])
        {
            case "Customers":
                var listCt = dc.Contacts.Where(x => x.IsActive == true && x.ContactType == 'C').Select(g => new { الاسم = g.Name }).ToList();
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + Request.QueryString["ID"] + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.ContentEncoding = System.Text.Encoding.Unicode;
                Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
                WriteTsv(listCt, Response.Output);
                Response.End();
                break;
            case "Vendors":
                var listVendor = dc.Contacts.Where(x => x.IsActive == true && x.ContactType == 'V').Select(g => new { الاسم = g.Name }).ToList();
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + Request.QueryString["ID"] + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.ContentEncoding = System.Text.Encoding.Unicode;
                Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
                WriteTsv(listVendor, Response.Output);
                Response.End();
                break;
            case "Items":
                var listItems = dc.usp_ExportItem().ToList();
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + Request.QueryString["ID"] + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.ContentEncoding = System.Text.Encoding.Unicode;
                Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
                WriteTsv(listItems, Response.Output);
                Response.End();
                break;
            case "Operations":
                var listOperations = dc.usp_ExportOperation(string.Empty);
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + Request.QueryString["ID"] + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.ContentEncoding = System.Text.Encoding.Unicode;
                Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
                WriteTsv(listOperations, Response.Output);
                Response.End();
                break;
            case "Employees":
                var lisEmployees = dc.usp_ExportEmployees(string.Empty);
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + Request.QueryString["ID"] + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.ContentEncoding = System.Text.Encoding.Unicode;
                Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
                WriteTsv(lisEmployees, Response.Output);
                Response.End();
                break;

        }




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