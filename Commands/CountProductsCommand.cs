using AliquoTPV.Extensibility;
using System;
using System.ComponentModel.Composition;
using System.Data;

namespace PluginTPV_Demo.Commands
{

    [Export(typeof(Command))]
    [CommandItemMetadata(ViewType.Sales, Description = "Cuenta la cantidad de artículos", FileImage = @"Resources\count_products.png")]
    public class CountProductsCommand : Command
    {

        public CountProductsCommand()
        {
            this.Execute += CountProductsCommand_Execute;
        }


        private void CountProductsCommand_Execute(IHost sender, ExecuteEventArgs e)
        {

            try
            {
                var data = sender.SalesGetDocument();

                if (data is null)
                {
                    sender.ShowMessage("No se ha encontrado ninguna venta.", "Contar artículos");
                }
                else
                {
                    // if data exists then the data is searched for and the quantities will be accumulated
                    decimal quantity = 0;

                    foreach (DataTable table in data.Tables)
                    {
                        if (sender.IsTableSales(table.TableName))
                        {
                            foreach (DataRow row in table.Rows)
                            {
                                if (!row.IsNull("Cantidad"))
                                {
                                    quantity += Convert.ToDecimal(row["Cantidad"]);
                                }
                            }

                            break;
                        }
                    }

                    sender.ShowMessage($"La venta actual contiene {quantity} artículos.", "Contar artículos");
                }
            }

            catch (Exception ex)
            {
                sender.ShowError("Plugin contar artículos", ex);
            }

        }
    }
}