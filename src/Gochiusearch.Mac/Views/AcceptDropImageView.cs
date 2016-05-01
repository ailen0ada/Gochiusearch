﻿using System;
using AppKit;
using Foundation;
using System.Diagnostics;
using CoreGraphics;
using System.Linq;
using CoreAnimation;
using System.Security.Policy;
using MobileCoreServices;

namespace Gochiusearch.Mac
{
    [global::Foundation.Register("AcceptDropImageView")]
    public class AcceptDropImageView : NSImageView
    {
        public AcceptDropImageView(IntPtr handle)
            : base(handle)
        {
        }

        public AcceptDropImageView(CGRect r)
            : base(r)
        {
            WantsLayer = true;
        }

        public event EventHandler<DropEventArgs> FileDropped;

        public event EventHandler<DropEventArgs> ImageUrlDropped;

        public override void ViewDidMoveToSuperview()
        {
            RegisterForDraggedTypes(new string[] { NSPasteboard.NSFilenamesType, NSPasteboard.NSTiffType });
        }

        public override NSDragOperation DraggingEntered(NSDraggingInfo sender)
        {
            var item = sender.DraggingPasteboard.PasteboardItems.First();
            NSUrl url;
            if (item.Types.Any(x => x == "public.url"))
            {
                url = new NSUrl(item.GetStringForType("public.url"));
            }
            else if (item.Types.Any(x => x == "public.file-url"))
            {
                url = new NSUrl(item.GetStringForType("public.file-url"));

            }
            else {
                return NSDragOperation.None;
            }
            return CanConformsToImageUTI(url)
                             ? NSDragOperation.Link
                                 : NSDragOperation.None;
        }

        public override bool PerformDragOperation(NSDraggingInfo sender)
        {
            var item = sender.DraggingPasteboard.PasteboardItems.First();
            if (item.Types.Any(x => x == "public.url"))
            {
                var url = new NSUrl(item.GetStringForType("public.url"));
                ImageUrlDropped?.Invoke(this, new DropEventArgs(url));
            }
            else if (item.Types.Any(x => x == "public.file-url"))
            {
                var url = new NSUrl(item.GetStringForType("public.file-url"));
                FileDropped?.Invoke(this, new DropEventArgs(url));
            }
            else
            {
                return false;
            }
            return true;
        }

        private bool CanConformsToImageUTI(NSUrl url)
        {
            var uti = UTType.CreatePreferredIdentifier(UTType.TagClassFilenameExtension, url.PathExtension, null);
            return UTType.ConformsTo(uti, UTType.Image);
        }
    }

    public class DropEventArgs
    {
        public DropEventArgs(NSUrl url)
        {
            Payload = url;
        }

        public NSUrl Payload { get; }
    }
}

