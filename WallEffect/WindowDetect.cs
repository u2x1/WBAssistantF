using System;
using System.Threading;
using System.Windows.Automation;

namespace WPFWindow
{
    public class WindowDetect
    {
        public delegate void NewWindowOpenedHandler(string title);

        public event NewWindowOpenedHandler NewWindowOpened;
        
        public void Listen()
        {
            Automation.AddAutomationEventHandler(
                WindowPattern.WindowOpenedEvent,
                AutomationElement.RootElement,
                TreeScope.Children,
                OnWindowOpened);
            while (true)
            {
                Thread.Sleep(60000);
            }

            // Automation.RemoveAllEventHandlers();
        }

        private void OnWindowOpened(object sender, AutomationEventArgs e)
        {
            try
            {
                var element = sender as AutomationElement;
                if (element != null)
                {
                    Thread.Sleep(1000);
                    NewWindowOpened?.Invoke(element.Current.Name);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}