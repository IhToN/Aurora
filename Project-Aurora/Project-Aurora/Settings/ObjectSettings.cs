﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Settings
{
    public class ObjectSettings<T> : NotifyPropertyChangedEx where T : Settings.SettingsBase
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        protected string SettingsSavePath { get; set; }
        public T Settings { get; protected set; }

        public ObjectSettings() : this("")
        {

        }
        public ObjectSettings(string prefix)
        {
            this.SettingsSavePath = Path.Combine(App.AppDataDirectory, prefix + this.GetType().Name + ".json");
        }

        public void SaveSettings()
        {
            this.SaveSettings(typeof(T));
        }

        //TODO: Async save
        protected void SaveSettings(Type settingsType)
        {
            if (Settings == null)
                Settings = (T)Activator.CreateInstance(settingsType);

            string dir = Path.GetDirectoryName(SettingsSavePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllText(SettingsSavePath, JsonConvert.SerializeObject(Settings, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented }));
        }

        public void LoadSettings()
        {
            this.LoadSettings(typeof(T));
        }

        protected virtual void LoadSettings(Type settingsType)
        {
            if (File.Exists(SettingsSavePath))
            {
                try
                {
                    Settings = (T)JsonConvert.DeserializeObject(File.ReadAllText(SettingsSavePath), settingsType, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
                }
                catch (Exception exc)
                {
                    logger.Error($"Exception occured while loading \"{this.GetType().Name}\" Settings.\nException:" + exc);
                    SaveSettings(settingsType);
                }
            }
            else
                SaveSettings(settingsType);
        }

        public virtual void ResetSettings()
        {
            Settings = null;
            SaveSettings();
        }
    }
}
