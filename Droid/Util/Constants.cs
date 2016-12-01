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

namespace MyIssues.Droid.Util
{
    class Constants
    {
    public static string[] KEYBOARD_SHORTCUTS = {"*", "-", "_", "#", "!", ":", ">"};
    public static string[] KEYBOARD_SHORTCUTS_BRACKETS = { "(", ")", "[", "]"};
    public static string[] KEYBOARD_SMART_SHORTCUTS = {"()", "[]"};
    public static string UTF_CHARSET = "utf-8";

    public static string MD_EXT = ".md";
    }
}