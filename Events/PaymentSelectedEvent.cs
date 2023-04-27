using AliquoTPV.Core;
using AliquoTPV.Extensibility;
using System.ComponentModel.Composition;

namespace PluginTPV_Demo.Events
{
    [Export(typeof(AliquoTPV.Extensibility.Events))]
    [EventsMetadata()]
    internal class PaymentSelectedEvent : AliquoTPV.Extensibility.Events
    {
        public PaymentSelectedEvent()
        {
            this.PaymentSelected += PaymentSelectedEvent_PaymentSelected;
        }

        private void PaymentSelectedEvent_PaymentSelected(IHost sender, PaymentEventArgs e)
        {

            // card payments are intercepted to simulate payment by device
            if (e.PaymentMethodType == PaymentMethodType.Card)
            {
                e.AmountPaid = e.AmountPending;
                e.AmountDelivered = e.AmountPending;
                e.Handled= true;
            }
        }
    }
}
