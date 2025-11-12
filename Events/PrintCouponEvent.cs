using AliquoTPV.Core;
using AliquoTPV.Extensibility;
using System;
using System.ComponentModel.Composition;
using System.Data;
using System.Windows.Forms;

namespace PluginTPV_Demo.Events
{

    [Export(typeof(AliquoTPV.Extensibility.Events))]
    [EventsMetadata()]
    public class PrintCouponEvent : AliquoTPV.Extensibility.Events
    {

        public PrintCouponEvent()
        {
            PrintAfter += Events_PrintAfter;
        }


        private void Events_PrintAfter(IHost sender, PrintEventArgs e)
        {

            // code example with functionality to print using Fast Report for coupon printing
            try
            {
                // the possibility of printing a coupon is assessed if is a invoice and is not a copy
                if (e.Type == PrintDocumentType.SalesInvoice && !e.IsCopy)
                {
                    var invoiceData = sender.GetQueryTable($"SELECT FechaEntrega, ImporteNeto FROM Notas WHERE IdFactura={e.DocumentId}");
                    var invoiceDate = (DateTime)invoiceData.Rows[0]["FechaEntrega"];
                    var invoiceAmount = (decimal)invoiceData.Rows[0]["ImporteNeto"];

                    // print a coupon for 2% of the sales amount if it exceeds the amount of 30
                    if (invoiceAmount >= 30m)
                    {
                        // we create a schema to pass it to the report
                        var data = new DataSet();

                        var table = data.Tables.Add("Vale");
                        table.Columns.Add("Empresa", typeof(string));
                        table.Columns.Add("Tienda", typeof(string));
                        table.Columns.Add("Fecha", typeof(DateTime));
                        table.Columns.Add("Importe", typeof(decimal));

                        var environment = sender.GetEnvironmentParameters();

                                              
                        DataRow row = table.NewRow();
                        row["Empresa"] = environment.Company.Name;
                        row["Tienda"] = environment.Store.Name;
                        row["Fecha"] = invoiceDate.AddMonths(1);
                        row["Importe"] = Math.Round(invoiceAmount * 0.02m, 2);
                        table.Rows.Add(row);


                        // ATTENTION: the following line must be commented out because
                        // only used to generate the schema and use it in the report,
                        data.WriteXml(System.IO.Path.Combine(Application.StartupPath, "Plugin\\Resources\\coupon_schema.xml"), XmlWriteMode.WriteSchema);

                        // send the printout using the host
                        sender.ReportPrint(System.IO.Path.Combine(Application.StartupPath, @"Plugin\Resources\coupon.frx"), e.PrinterName, 1, data);
                    }

                }
            }

            catch (Exception ex)
            {
                // check the error and display the message
                sender.ShowException(ex);
            }

        }

    }
}