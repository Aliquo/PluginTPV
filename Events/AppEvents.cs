using AliquoTPV.Core;
using AliquoTPV.Extensibility;
using System;
using System.ComponentModel.Composition;

namespace PluginTPV_Demo.Events
{
    [Export(typeof(AliquoTPV.Extensibility.Events))]
    [EventsMetadata()]
    internal class AppEvents : AliquoTPV.Extensibility.Events
    {
        public AppEvents()
        {
            this.AppLoaded += AppEvents_AppLoaded;
            this.AppClosed += AppEvents_AppClosed;
        }

        private void AppEvents_AppLoaded(IHost sender, EventArgs e)
        {
            Console.WriteLine("Loaded application...");
        }

        private void AppEvents_AppClosed(IHost sender, EventArgs e)
        {
            Console.WriteLine("Closed application...");
        }
              
    }
}
