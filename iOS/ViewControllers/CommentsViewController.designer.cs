// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MyIssues2.iOS
{
	[Register ("CommentsViewController")]
	partial class CommentsViewController
	{
		[Outlet]
		UIKit.UIButton ReplyButton { get; set; }

		[Outlet]
		UIKit.UITableView TableView { get; set; }

		[Action ("ReplyToIssueTapped:")]
		partial void ReplyToIssueTapped (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ReplyButton != null) {
				ReplyButton.Dispose ();
				ReplyButton = null;
			}

			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}
		}
	}
}
