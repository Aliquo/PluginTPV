using AliquoTPV.Core;
using AliquoTPV.Extensibility;
using System;
using System.ComponentModel.Composition;
using System.Data;
using System.Windows.Forms;
using Convert = AliquoTPV.Core.Convert;

namespace PluginTPV_Demo.Events
{

    [Export(typeof(AliquoTPV.Extensibility.Events))]
    [EventsMetadata()]
    public class PrintCouponEvent : AliquoTPV.Extensibility.Events
    {
       
        public PrintCouponEvent()
        {
            PrintAfter += Eventos_PrintAfter;
        }


        private void Eventos_PrintAfter(IHost sender, PrintEventArgs e)
        {

            // code example with functionality to print using Crystal Report for coupon printing
            try
            {

                if (e.Type == PrintDocumentType.SalesDocument || e.Type == PrintDocumentType.SalesInvoice)
                {

                    // the possibility of printing a voucher is assessed if is a ticket or invoice
                    // and is not a copy, moreover, there must be data
                    if (!e.IsCopy && e.Data != null && e.Data.Tables.Contains("Notas") && e.Data.Tables["Notas"].Rows.Count > 0)
                    {

                        var docDate = Convert.ValueToDateTime(e.Data.Tables["Notas"].Rows[0]["Fecha"]);
                        var docAmount = Convert.ValueToDecimal(e.Data.Tables["Notas"].Rows[0]["ImporteTotal"]);

                        // print a coupon for 2% of the sales amount if it exceeds the amount of 30
                        if (docAmount >= 30m)
                        {
                            // we create a schema to pass it to the report
                            var data = new DataSet();

                            data.Tables.Add(e.Data.Tables["PRM_Entorno"].Copy());

                            data.Tables.Add("Vale");
                            {
                                var withBlock = data.Tables["Vale"];
                                withBlock.Columns.Add("Fecha", typeof(DateTime));
                                withBlock.Columns.Add("Importe", typeof(decimal));

                                // calculate the amount of the coupon
                                docAmount = Math.Round(docAmount * 0.02m, 2);
                                withBlock.Rows.Add(docDate, docAmount);
                            }


                            // ATTENTION: the following line must be commented out because
                            // only used to generate the schema and use it in the report,
                            // data.WriteXml(IO.Path.Combine(Application.StartupPath, "Plugin\Resources\coupon_schema.xml"), XmlWriteMode.WriteSchema)

                            // send the printout using the host
                            sender.ReportPrint(System.IO.Path.Combine(Application.StartupPath, @"Plugin\Resources\coupon.rpt"), e.PrinterName, 1, data);
                        }
                    }

                         
                }
            }

            catch (Exception ex)
            {
                // check the error and display the message
                sender.ShowError("Plugin (PrintAfter)", ex);
            }

        }

    }
}