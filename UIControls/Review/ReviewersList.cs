using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using TrackGearLibrary.Core;
using UIControls.Core;

namespace RallyToolsCore.Review
{
	public partial class ReviewersList : Form
	{
		public ReviewersList()
		{
			InitializeComponent();

			// read reviewers
			var list = ReadReviewersFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", "conf\\reviewers.txt"));
			var myList = ReadReviewersFromFile(Path.Combine(MySettingProvider.GetAppDataDirectory(), "reviewers.txt"));

			var blist = new BindingList<Reviewer>(list);
			bindingSource1.DataSource = blist;
			reviewersGrid.DataSource = bindingSource1;
			bindingNavigator1.BindingSource = bindingSource1;
		}

		static List<Reviewer> ReadReviewersFromFile(string path)
		{
			if (!File.Exists(path))
				return new List<Reviewer>();

			try
			{
				return File.ReadAllLines(path)
					.Where(l => !string.IsNullOrWhiteSpace(l))
					.Select(l => l.Split('\t'))
					.Where(arr => arr.Length == 2)
					.Select(r => new Reviewer { Name = r[0], EMail = r[1] })
					.ToList()
				;
			}
			catch(Exception ex)
			{
				Logger.LogIt("Load Reviewers: {0}", ex);
				return new List<Reviewer>();
			}
		}
	}
}
