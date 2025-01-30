using AliquoTPV.Core;
using AliquoTPV.Extensibility;
using System.ComponentModel.Composition;

namespace PluginTPV_Demo.Events
{
    [Export(typeof(AliquoTPV.Extensibility.Events))]
    [EventsMetadata()]
    internal class SearchProductEvent : AliquoTPV.Extensibility.Events
    {
        public SearchProductEvent()
        {
            this.SearchProduct += Events_SearchProduct;
        }

        private void Events_SearchProduct(IHost sender, SearchEventArgs e)
        {
            // Example to replace the product search functionality.
            if (e.Value != null && e.Value.ToString().Contains("#"))
            {
                var values = e.Value.ToString().Split('#');

                sender.SalesAddProduct(values[0], price: Convert.ValueToDecimal(values[1]));
                e.Handled = true;
            }
        }

    }
}
