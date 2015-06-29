using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UIControls.Core;

namespace TrackGearLibrary.Core
{
	public class MySettingProvider : SettingsProvider, ISettingsProviderService
	{
		public override string ApplicationName { get; set; }

		public override void Initialize(string name, NameValueCollection config)
		{
			base.Initialize(name ?? "MySettingProvider", config);
		}

		class SettingsStore
		{
			public List<KeyValuePair<string, object>> Settings;
		}

		public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
		{
			var dict = new Dictionary<string, object>();

			try
			{
				var path = GetSettingsPath();
				if (File.Exists(path))
				{
					var settingsJson = File.ReadAllText(path);

					var settingsObj = JsonConvert.DeserializeObject<SettingsStore>(settingsJson);

					foreach (var keyValuePair in settingsObj.Settings)
					{
						dict[keyValuePair.Key] = keyValuePair.Value;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.LogIt("Loading settings error: {0}", ex);
			}

			var values = new SettingsPropertyValueCollection();

			foreach (SettingsProperty property in collection)
			{
				object value;

				dict.TryGetValue(property.Name, out value);

				var propertyValue = new SettingsPropertyValue(property);

				if (value != null)
				{
					propertyValue.PropertyValue = value;
				}
				else if (property.DefaultValue != null)
				{
					propertyValue.SerializedValue = property.DefaultValue;
				}
				else
				{
					propertyValue.PropertyValue = null;
				}

				propertyValue.IsDirty = false;

				values.Add(propertyValue);
			}

			return values;
		}

		public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
		{
			var dirty = collection
				.Cast<SettingsPropertyValue>()
				.Where(pv => pv.IsDirty)
				.ToList()
			;

			if (dirty.Count == 0)
				return;

			var tuples = collection
				.Cast<SettingsPropertyValue>()
				.Select(p => new KeyValuePair<string, object>(p.Name, p.PropertyValue))
				.ToList()
			;

			var settingsObj = new SettingsStore { Settings = tuples };

			var settingsJson = JsonConvert.SerializeObject(settingsObj, Formatting.Indented);

			File.WriteAllText(GetSettingsPath(), settingsJson);

			dirty.ForEach(pv => pv.IsDirty = false);
		}

		public static string GetAppDataDirectory()
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Constants.AppDataDirName);
		}

		private static string GetSettingsPath()
		{
			var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
				Constants.AppDataDirName, "settings.json");
			return path;
		}

		SettingsProvider ISettingsProviderService.GetSettingsProvider(SettingsProperty property)
		{
			return this;
		}
	}
}
