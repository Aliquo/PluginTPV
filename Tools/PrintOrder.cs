using AliquoTPV.Extensibility;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;


namespace PluginTPV_Demo.Tools
{
    internal class PrintOrder : IDisposable
    {

        private IHost _host;
        private DataRow _row;

        public PrintOrder(IHost host, DataRow row)
        {
            _host = host;
            _row = row;
        }

        /// <summary>Method to print the order</summary>
        internal void Print(string printerName)
        {
         
            try
            {
                // we use a class of type PrintDocument for printing
                var printDoc = new PrintDocument();

                printDoc.PrinterSettings.PrinterName = printerName;
                printDoc.DocumentName = "Pedido";

                // assign the event method for each page to be printed
                printDoc.PrintPage += PrintDocument_PrintPage;

                // we indicate that we want to print
                printDoc.Print();
            }

            catch (Exception ex)
            {
                // the error is returned to the POS to be checked by the Aliquo TPV
                throw ex;
            }

        }

        /// <summary>Event to occur each time a new page is printed</summary>
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            float x, y;

            x = e.MarginBounds.Left;
            y = e.MarginBounds.Top;

            // print horizontal line
            e.Graphics.DrawLine(Pens.Black, x, y, e.MarginBounds.Right, y);
            y += 10f;

            // set the font and text formatting so that it can be printed in a centred position
            var prFont = new Font("Arial", 20f, FontStyle.Bold);

            var stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            // define the rectangle where the text is to be centred
            var rect1 = new Rectangle((int)Math.Round(x), (int)Math.Round(y), e.MarginBounds.Width, prFont.Height);

            // print the order and the amount
            var docNum = Convert.ToInt32(_row["NumPedido"]);
            var docAmount = Convert.ToDecimal(_row["ImporteTotal"]);
            e.Graphics.DrawString(string.Format("Pedido nº {0} por un importe de {1}€", docNum, docAmount), prFont, Brushes.Black, rect1, stringFormat);
            y += prFont.GetHeight(e.Graphics) + 20f;

            // change the font for the rest of the printouts
            prFont = new Font("Arial", 8f, FontStyle.Italic);

            // obtain company information
            var envPrm = _host.GetEnvironmentParameters();

            if (envPrm != null)
            {
                // print company information
                if (envPrm.Company != null)
                {
                    e.Graphics.DrawString(string.Format("{0} ({1})", envPrm.Company.Name, envPrm.Company.VATNumber), prFont, Brushes.Gray, x, y);
                    y += prFont.GetHeight(e.Graphics);
                }

                // print store information
                if (envPrm.Store != null)
                {
                    e.Graphics.DrawString(envPrm.Store.Name, prFont, Brushes.Gray, x, y);
                    y += prFont.GetHeight(e.Graphics);
                }

                // print terminal information
                if (envPrm.Store != null)
                {
                    e.Graphics.DrawString(string.Format("Emitido por {0}", envPrm.TerminalName), prFont, Brushes.Gray, x, y);
                    y += prFont.GetHeight(e.Graphics);
                }
            }

            // print expiration date
            var fecha = Convert.ToDateTime(_row["Fecha"]).AddDays(30d);
            e.Graphics.DrawString(string.Format("Válido hasta {1}", envPrm.Company.Name, fecha.ToShortDateString()), prFont, Brushes.Gray, x, y);
            y += prFont.GetHeight(e.Graphics) + 10f;

            // print horizontal line
            e.Graphics.DrawLine(Pens.Black, x, y, e.MarginBounds.Right, y);

            // is indicated that there are no more pages
            e.HasMorePages = false;

        }

        public void Dispose()
        {
            _row = null;
            _host = null;
        }
    }
}
