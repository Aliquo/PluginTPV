using AliquoTPV.Core;
using AliquoTPV.Extensibility;
using System;
using System.ComponentModel.Composition;
using System.Media;

namespace PluginTPV_Demo.Events
{
    [Export(typeof(AliquoTPV.Extensibility.Events))]
    [EventsMetadata()]
    public class SearchProductEvent : AliquoTPV.Extensibility.Events
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

                if (!String.IsNullOrWhiteSpace(values[0]) || !String.IsNullOrWhiteSpace(values[1]))
                {
                    decimal price = 0;
                    decimal.TryParse(values[1], out price);

                    sender.SalesAddProduct(values[0], price: price);
                    e.Handled = true;
                }                
            }
        }

    }
}
