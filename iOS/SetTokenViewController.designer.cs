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
	[Register ("SetTokenViewController")]
	partial class SetTokenViewController
	{
		[Outlet]
		UIKit.UITextView TokenTextView { get; set; }

		[Action ("ContinueButton:")]
		partial void ContinueButton (Foundation.NSObject sender);

		[Action ("LearnMoreClick:")]
		partial void LearnMoreClick (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (TokenTextView != null) {
				TokenTextView.Dispose ();
				TokenTextView = null;
			}
		}
	}
}
