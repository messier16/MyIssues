// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using Humanizer;
using MyIssues.Models;
using UIKit;

namespace MyIssues2.iOS
{
	public partial class CommentViewCell : UITableViewCell
	{
		public CommentViewCell (IntPtr handle) : base (handle)
		{
		}

		internal void SetComment(IssueComment comment)
		{
			UsernameLabel.Text = comment.Author;
			CreatedAtLabel.Text = comment.CreatedAt.Humanize();
			BodyLabel.AttributedText = comment.Body.FromMarkdown();

		}
	}
}