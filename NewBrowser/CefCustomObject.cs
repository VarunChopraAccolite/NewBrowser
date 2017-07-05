using CefSharp;
using CefSharp.WinForms;
using System.Diagnostics;
using System.Windows.Forms;

namespace NewBrowser
{
    class CefCustomObject
    {
        // Declare a local instance of chromium and the main form in order to execute things from here in the main thread
        private static ChromiumWebBrowser _instanceBrowser = null;

        // The form class needs to be changed according to yours
        private static Form1 _instanceMainForm = null;

        public CefCustomObject(ChromiumWebBrowser originalBrowser, Form1 mainForm)
        {
            _instanceBrowser = originalBrowser;
            _instanceMainForm = mainForm;
        }

        public void showDevTools()
        {
            if (_instanceMainForm.isBrowserInitialized)
            {
                _instanceBrowser.ShowDevTools();
            }
        }

        public void opencmd()
        {
            ProcessStartInfo start = new ProcessStartInfo("cmd.exe", "/c pause");
            Process.Start(start);
        }

        public void openGoogle()
        {
            // start web url
            var startUrl = "http://google.com";
            _instanceBrowser.Load(startUrl);
        }

        public void openDetectMyBrowser()
        {
            // start web url
            var startUrl = "http://detectmybrowser.com/";
            _instanceBrowser.Load(startUrl);
        }
    }
}
