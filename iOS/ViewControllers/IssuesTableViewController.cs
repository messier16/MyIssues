// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Foundation;
using MyIssues.DataAccess;
using MyIssues.Models;
using MyIssues2.iOS.Cells;
using UIKit;
using Humanizer;

namespace MyIssues2.iOS
{
	public partial class IssuesTableViewController : UITableViewController
	{
		public IssuesTableViewController(IntPtr handle) : base(handle)
		{
		}

		struct StoryboardId
		{
			public const string IssueCellIdentifier = "Issue Cell";
			public const string IssueNoMilestoneCellIdentifier = "Issue No Milestone Cell";
			public const string ViewIssueDetailSegue = "View Issue Detail";
			public const string ShowSettingsSegue = "Show Settings";
			public const string SwitchRepoSegue = "Switch Repo";
			public const string ShowLabelsSegue = "Show Labels";
		}

		Storage _storage;
		public long RepoId
		{
			get;
			set;
		}

		public string ViewingLabel
		{
			get;
			private set;
		}

		public override async void ViewDidAppear(bool animated)
		{
			System.Diagnostics.Debug.WriteLine($"Attempting to load {RepoId}");
			base.ViewDidAppear(animated);
			if (RepoId == 0)
			{
				OpenRepoSelector();
			}
			else
			{
				await LoadRepo();
			}
		}

		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();
			TableView.RowHeight = UITableView.AutomaticDimension;
			TableView.EstimatedRowHeight = 70;
			_storage = Storage.GetInstance();
			if (RepoId == 0)
				RepoId = await _storage.GetWorkingRepo();

		}

		Octokit.Repository _repo;

		async Task LoadRepo()
		{
			Contract.Ensures(Contract.Result<Task>() != null);
			try
			{
				await _storage.SetWorkingRepo(RepoId);
			}
			catch (Exception e)
			{
				OpenRepoSelector();
				return;
			}
			_repo = await _storage.GetRepo(RepoId);

			Title = _repo.Name;
			//UpdateIssues(await _storage.GetIssues());
			_storage.GetIssues(UpdateIssues);

		}

		void OpenRepoSelector()
		{
			PerformSegue(StoryboardId.SwitchRepoSegue, this);
		}

		List<Issue> _originalIssues;
		List<Issue> _shownIssues;
		void UpdateIssues(List<Issue> issues)
		{
			System.Diagnostics.Debug.WriteLine($"Load {RepoId} ::: {(issues?.Count ?? 0)}");
			_originalIssues = issues;
			if (String.IsNullOrEmpty(ViewingLabel))
			{
				_shownIssues = _originalIssues;
			}
			else
			{
				_shownIssues = _originalIssues.Where(i => i.Labels.Where(l => l.Name.Equals(ViewingLabel)).Any()).ToList();
			}
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
			return _shownIssues?.Count ?? 0;
		}


		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var issue = _shownIssues[indexPath.Row];
			if (!String.IsNullOrEmpty(issue.Milestone))
			{
				var cell = TableView.DequeueReusableCell(StoryboardId.IssueCellIdentifier, indexPath) as IssueCellView;
				if (cell != null)
				{
					cell.SetIssue(issue);
				}
				return cell;
			}
			else 
			{
				var cell = TableView.DequeueReusableCell(StoryboardId.IssueNoMilestoneCellIdentifier, indexPath) as IssueNoMilestoneCellView;
				if (cell != null)
				{
					cell.SetIssue(issue);
				}
				return cell;
			}
		}

		int _issueNumber;
		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			_issueNumber = _shownIssues[indexPath.Row].Number;
			this.PerformSegue(StoryboardId.ViewIssueDetailSegue, tableView);

		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{	
			return UITableView.AutomaticDimension;
		}

		public override nfloat EstimatedHeight(UITableView tableView, NSIndexPath indexPath)
		{
			return UITableView.AutomaticDimension;
		}
		#endregion


		#region Segues

		public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
		{
			if (StoryboardId.ViewIssueDetailSegue.Equals(segueIdentifier))
			{
				return _issueNumber != 0;
			}
			return base.ShouldPerformSegue(segueIdentifier, sender);
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{

			NavigationItem.BackBarButtonItem = new UIBarButtonItem(_repo.Name.Truncate(10), UIBarButtonItemStyle.Plain, null);//, //em = [[[UIBarButtonItem alloc] initWithTitle: @"Back" style: UIBarButtonItemStylePlain target:nil action:nil] autorelease]

			if (StoryboardId.ViewIssueDetailSegue.Equals(segue.Identifier))
			{
				var destination = segue.DestinationViewController.ContentViewController() as IssueTabBarViewController;
				destination.IssueNumber = _issueNumber;
			}
			else if (StoryboardId.ShowLabelsSegue.Equals(segue.Identifier))
			{
				var destination = segue.DestinationViewController.ContentViewController() as LabelsTableViewController;
				destination.LabelSelected += (s, selectedLabel) =>
				{
					System.Diagnostics.Debug.WriteLine($"Selected label: {selectedLabel}");
					ViewingLabel = selectedLabel;
				};
			}
			else
			{
				base.PrepareForSegue(segue, sender);
			}
		}


		#endregion
	}
}
