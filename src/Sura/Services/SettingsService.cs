using System;
using System.Collections.Generic;
using System.Web;
using Raven.Client;
using Settings = Sura.Models.Settings;

namespace Sura.Services
{
    public interface ISettingsService
    {
        Settings Load();
        void Save(Settings settings, bool reset = false);
    }

    public class SettingsService : ISettingsService
    {
        private readonly IDocumentSession _session;

        public SettingsService(IDocumentSession session)
        {
            _session = session;
        }

        public Settings Load()
        {
            var defaultSettings = new Settings();

            Settings settings;
            if (HttpContext.Current.Request.IsAuthenticated == false)
            {
                using (_session.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromMinutes(15)))
                {
                    settings = _session.Load<Settings>(defaultSettings.Id);
                }
            }
            else
            {
                settings = _session.Load<Settings>(defaultSettings.Id);
            }

            if (settings == null)
            {
                _session.Store(settings = defaultSettings);
                _session.SaveChanges();
            }

            return settings;
        }

        public void Save(Settings settings, bool reset)
        {
            var setting = reset ? new Settings() : settings;

            _session.Store(setting);
            _session.SaveChanges();
        }
    }
}