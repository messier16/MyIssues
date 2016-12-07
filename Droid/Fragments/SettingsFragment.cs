using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Preferences;
using MyIssues.Droid.Activities;

namespace MyIssues.Droid.Fragments
{
    [Register("MyIssues.Droid.Fragments.SettingsFragment")]
    public class SettingsFragment : PreferenceFragmentCompat
    {
        public override void OnCreatePreferences(Bundle savedInstanceState, string rootKey)
        {
            
            AddPreferencesFromResource(Resource.Xml.Preferences);

			var aboutPreference = FindPreference("about");
			aboutPreference.PreferenceClick += (sender, e) => {
				var i = new Intent(this.Context, typeof(ThanksActivity));
				StartActivity(i);
			};
        }
    }
}