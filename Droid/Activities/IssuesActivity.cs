﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System.Reactive.Linq;
using MyIssues.Droid.Adapters;
using MyIssues.Droid.Controls;
using MyIssues.DataAccess;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using MyIssues.Droid.Activities;
using Android.Support.V4.Graphics.Drawable;
using Android.Graphics;

namespace MyIssues.Droid
{

    [Activity(Label = "@string/issues_activity_label",
        Theme = "@style/MyTheme")]
    public class IssuesActivity : AppCompatActivity, SwipeRefreshLayout.IOnRefreshListener
    {

        struct RequestCodes
        {
            public const int SelectRepo = 1;
            public const int ChangeSettings = 2;
            public const int FilterByLabel = 3;
        }

        MyIssues.Droid.Controls.RecyclerViewEmptySupport _issuesListView;
        IssuesAdapter IssuesAdapter => _issuesListView?.GetAdapter() as IssuesAdapter;
        SwipeRefreshLayout _refreshLayout;
        RecyclerView.LayoutManager _layoutManager;
        ProgressBar progress_horizontal;
        Storage _storage;
        Octokit.Repository _repo;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Issues);
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.Toolbar);
            SetSupportActionBar(toolbar);

            _refreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.SwipeIssuesContainer);

            _refreshLayout.SetOnRefreshListener(this);
            _storage = Storage.GetInstance();


            _issuesListView = FindViewById<RecyclerViewEmptySupport>(Resource.Id.IssuesListView);


            progress_horizontal = FindViewById<ProgressBar>(Resource.Id.ProgressHorizontal);

            var view = FindViewById(Resource.Id.EmptyListView);
            _issuesListView.EmptyView = view;
            _layoutManager = new LinearLayoutManager(this);
            _issuesListView.SetLayoutManager(_layoutManager);

            long repo = await _storage.GetWorkingRepo();
            if (repo == 0)
            {
                OpenRepoSelector();
            }
            else
            {
                await LoadRepo(repo);
            }
        }

        private async Task LoadRepo(long repoId)
        {
            progress_horizontal.Visibility = ViewStates.Visible;
            _issuesListView.IsLoading = true;
            progress_horizontal.Indeterminate = true;
            try
            {
                await _storage.SetWorkingRepo(repoId);
            }
            catch (Exception e)
            {
                OpenRepoSelector();
                return;
            }
            _repo = await _storage.GetRepo(repoId);

            Title = _repo.Name;
            var observableIssues = _storage.GetIssues();

            observableIssues.Subscribe(UpdateIssues);

            var adapter = new IssuesAdapter(new List<Models.Issue>());
            adapter.OnIssueSelected += (selected) =>
            {
                Intent intent = new Intent(this, typeof(IssueMainActivity));
                intent.PutExtra("id", selected.Id);
                intent.PutExtra("number", selected.Number);
                intent.PutExtra("title", selected.Title);
                StartActivity(intent);
            };

            _issuesListView.IsLoading = false;
            _issuesListView.SetAdapter(adapter);

            progress_horizontal.Visibility = ViewStates.Gone;
        }

        private Handler updateHandler;
        void UpdateIssues(List<Models.Issue> issues)
        {
            using (var h = new Handler(Looper.MainLooper))
                h.Post(() =>
                {
                    (_issuesListView.GetAdapter() as IssuesAdapter).Update(issues);
                    _refreshLayout.Refreshing = false;
                });
        }

        void OpenRepoSelector()
        {
            var i = new Intent(this, typeof(ReposActivity));
            StartActivityForResult(i, RequestCodes.SelectRepo);
        }

        string _selectedLabel;

        protected override async void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == RequestCodes.SelectRepo && resultCode == Result.Ok)
            {
                var repoId = data.GetLongExtra("repoId", 0);
                await LoadRepo(repoId);
            }
            else if (requestCode == RequestCodes.ChangeSettings && resultCode == Result.Ok)
            {
                var didChangeRepo = data.GetBooleanExtra("didChangeRepo", false);
                if (didChangeRepo)
                {
                    var repoId = data.GetLongExtra("repoId", 0);
                    if (repoId != (_repo?.Id ?? 0))
                    {
                        await LoadRepo(repoId);
                    }
                }
            }
            else if (requestCode == RequestCodes.FilterByLabel && resultCode == Result.Ok)
            {
                _selectedLabel = data.GetStringExtra("selectedLabel");
                InvalidateOptionsMenu();
                IssuesAdapter.Filter.InvokeFilter(_selectedLabel);
            }
            else
            {
                base.OnActivityResult(requestCode, resultCode, data);
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.IssuesMenu, menu);
			menu.TintIcons();

            var viewLabels = menu.FindItem(Resource.Id.ViewLabelsMenu);
            var deleteFilter = menu.FindItem(Resource.Id.DeleteLabelFilter);

            if (String.IsNullOrEmpty(_selectedLabel))
            {
                deleteFilter.SetVisible(false);
                viewLabels.SetVisible(true);
            }
            else
            {
                deleteFilter.SetVisible(true);
                viewLabels.SetVisible(false);
            }


            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.DeleteLabelFilter:
                    _selectedLabel = null;
                    IssuesAdapter.Filter.InvokeFilter(_selectedLabel);
                    InvalidateOptionsMenu();
                    break;
                case Resource.Id.ViewLabelsMenu:
                    var labelsIntent = new Intent(this, typeof(LabelsActivity));
                    StartActivityForResult(labelsIntent, RequestCodes.FilterByLabel);
                    break;
                case Resource.Id.ViewSettingsMenu:
                    var settingsIntent = new Intent(this, typeof(SettingsActivity));
                    StartActivityForResult(settingsIntent, RequestCodes.ChangeSettings);
                    break;
                default:
                    return base.OnOptionsItemSelected(item);
            }
            return true;
        }

        public async void OnRefresh()
        {
            _refreshLayout.Refreshing = true;
            var observableIssues = _storage.GetIssues();
            observableIssues.Subscribe(UpdateIssues);
        }
    }
}
