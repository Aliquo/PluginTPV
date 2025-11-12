using AliquoTPV.Extensibility;
using System;
using System.ComponentModel.Composition;
using System.Data;

namespace PluginTPV_Demo.Commands
{

    [Export(typeof(Command))]
    [CommandItemMetadata(ViewType.Sales, Description = "Cuenta las ventas del día", FileImage = @"Resources\action.png")]
    public class CountDailySalesCommand : Command
    {

        public CountDailySalesCommand()
        {
            this.Execute += Command_Execute;           
        }


        private void Command_Execute(IHost sender, ExecuteEventArgs e)
        {

            try
            {
                var data = sender.GetQueryTable("SELECT SUM(ImporteNETO) as Total FROM Notas WHERE CodTipoNota='C' and FechaEntrega=convert(date, getdate())");

                if (data is null || data.Rows.Count==0)
                {
                    sender.ShowMessage("No se ha encontrado ninguna venta.", "Ventas del día");
                }
                else
                {
                    decimal amount = 0;
                    if (!data.Rows[0].IsNull("Total"))
                        amount = (decimal)data.Rows[0]["Total"];

                    sender.ShowMessage($"El importe de venta del día son {amount.ToString("N2")}€.", "Ventas del día");
                }
            }

            catch (Exception ex)
            {
                sender.ShowException(ex);
            }

        }
    }
}