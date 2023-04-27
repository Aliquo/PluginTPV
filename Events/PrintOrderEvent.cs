using AliquoTPV.Core;
using AliquoTPV.Extensibility;
using System.ComponentModel.Composition;

namespace PluginTPV_Demo.Events
{

    [Export(typeof(AliquoTPV.Extensibility.Events))]
    [EventsMetadata()]
    public class PrintOrderEvent : AliquoTPV.Extensibility.Events
    {

        public PrintOrderEvent()
        {
            PrintBefore += Eventos_PrintBefore;
        }

        private void Eventos_PrintBefore(IHost sender, PrintEventArgs e)
        {

            // example to replace the order printing functionality
            if (e.Type == PrintDocumentType.SalesOrder)
            {

                // if data exists then another printing system is used instead of Crystal Report 
                if (e.Data != null && e.Data.Tables.Contains("Notas") && e.Data.Tables["Notas"].Rows.Count > 0)
                {
                    // indicates that the usual printing system is to be replaced
                    e.Handled = true;

                    using (var printOrder = new Tools.PrintOrder(sender, e.Data.Tables["Notas"].Rows[0]))
                    {
                        printOrder.Print(e.PrinterName);
                    }
                }
            }
            
        }

    }
}