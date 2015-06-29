using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace RallyToolsCore.Properties
{
	partial class Settings
	{
		public Settings()
		{
			// ensure all user scope properties has custom provider
			var props = GetType()
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(pi => pi.GetCustomAttributes(typeof(System.Configuration.UserScopedSettingAttribute), false).Any() && !pi.GetCustomAttributes(typeof(System.Configuration.SettingsProviderAttribute), false).Any())
				.ToArray()
			;

			if(props.Length != 0)
			{
				var text = "Some properties uses not custom provider:\n"  + string.Join("\n", props.Select(pi => pi.Name));
				MessageBox.Show(text, "Settings error");
				throw new ApplicationException(text);
			}
		}
	}
}
