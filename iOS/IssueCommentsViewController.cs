// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Collections.Generic;
using Foundation;
using MyIssues.DataAccess;
using MyIssues.Models;
using UIKit;

namespace MyIssues2.iOS
{
	public partial class IssueCommentsViewController : UITableViewController
	{
		public IssueCommentsViewController (IntPtr handle) : base (handle)
		{
		}

		struct StoryboardId
		{
			public const string CommentViewCellIdentifier = "Comment View Cell";
		}

		Storage _storage;
		int _issueNumber;
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			TableView.AllowsSelection = false;

			TableView.RowHeight = UITableView.AutomaticDimension;
			TableView.EstimatedRowHeight = 50;

			var controller = TabBarController as IssueTabBarViewController;
			_issueNumber = controller.IssueNumber;
			_storage = Storage.GetInstance();

            var loadComments = _storage.GetIssueComments(_issueNumber);

			loadComments.Subscribe(CargaComentarios);
			TableView.ContentInset = new UIEdgeInsets(0, 0, 40, 0);

			NavigationItem.RightBarButtonItem = new UIBarButtonItem("toolbar_reply.png",  UIBarButtonItemStyle.Plain, null);
		}

		IReadOnlyList<IssueComment> _comments;
		void CargaComentarios(IReadOnlyList<IssueComment> comments)
		{
			_comments = comments;
			InvokeOnMainThread(() => 
			{
				TableView.ReloadData();
			});
		}

		#region Table stuff
		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return _comments?.Count ?? 0;
		}


		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = TableView.DequeueReusableCell(StoryboardId.CommentViewCellIdentifier, indexPath) as CommentViewCell;

			if (cell != null)
			{
				var comment = _comments[indexPath.Row];
				cell.SetComment(comment);

			}

			return cell;
		}

		public override nfloat EstimatedHeight(UITableView tableView, NSIndexPath indexPath)
		{
			return UITableView.AutomaticDimension;
		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return UITableView.AutomaticDimension;
		}

		#endregion
	}
}

