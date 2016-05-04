using AppKit;
using Foundation;

namespace Gochiusearch.Mac
{
    public partial class AppDelegate : NSApplicationDelegate
    {
        MainWindowController mainWindowController;

        public AppDelegate()
        {
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            mainWindowController = new MainWindowController();
            mainWindowController.Window.MakeKeyAndOrderFront(this);
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
            // Clear caches
            var d = NativeMethods.ContainerDirectory;
            foreach (var f in System.IO.Directory.EnumerateFiles(d, "*", System.IO.SearchOption.TopDirectoryOnly))
                System.IO.File.Delete(f);
        }

        public override bool ApplicationShouldHandleReopen(NSApplication sender, bool hasVisibleWindows)
        {
            if (!hasVisibleWindows)
            {
                mainWindowController.Window.MakeKeyAndOrderFront(this);
            }
            return true;
        }

        partial void NavigateToGithub(NSObject sender)
        {
            NSWorkspace.SharedWorkspace.OpenUrl(new NSUrl("https://github.com/ksasao/Gochiusearch"));
        }
    }
}
