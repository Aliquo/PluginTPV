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
            this.AppLoaded += Events_AppLoaded;
            this.AppClosed += Events_AppClosed;
        }

        private void Events_AppLoaded(IHost sender, EventArgs e)
        {
            Console.WriteLine("Loaded application...");
        }

        private void Events_AppClosed(IHost sender, EventArgs e)
        {
            Console.WriteLine("Closed application...");
        }
              
    }
}
