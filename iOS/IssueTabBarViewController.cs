// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;

namespace MyIssues2.iOS
{
	public partial class IssueTabBarViewController : UITabBarController
	{
		public IssueTabBarViewController (IntPtr handle) : base (handle)
		{
		}

		public int IssueNumber
		{
			get;
			set;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = $"Issue #{IssueNumber}";
		}
	}
}
