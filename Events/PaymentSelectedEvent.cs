using AliquoTPV.Core;
using AliquoTPV.Extensibility;
using System.ComponentModel.Composition;

namespace PluginTPV_Demo.Events
{
    [Export(typeof(AliquoTPV.Extensibility.Events))]
    [EventsMetadata()]
    public class PaymentSelectedEvent : AliquoTPV.Extensibility.Events
    {
        public PaymentSelectedEvent()
        {
            this.PaymentEditBefore += Events_PaymentEditBefore;
        }

        private void Events_PaymentEditBefore(IHost sender, PaymentEventArgs e)
        {

            // card payments are intercepted to simulate payment by device
            if (e.PaymentMethodType == PaymentMethodType.Card)
            {
                e.Payment.Amount = e.AmountPending;
                e.Payment.AmountDelivered = e.AmountPending;
                e.Handled= true;
            }
        }
    }
}
