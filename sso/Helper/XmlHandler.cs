using sso.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace sso.Helper
{
    public static class XmlHandler
    {
        public static bool EditarChaveValorArquivoConfiguracao(string key, string value, string arquivoConfiguracao)
        {
            try
            {
                XElement xml = XElement.Load(ConfigurationManager.AppSettings.Get(arquivoConfiguracao));
                XElement x = xml.Elements("appSettings")
                    .Descendants()
                    .Where(p => p.Attribute("key").Value.Equals(key)).First();

                if (x != null)
                {
                    x.Attribute("key").SetValue(key);
                    x.Attribute("value").SetValue(value);
                }
                xml.Save(ConfigurationManager.AppSettings.Get(arquivoConfiguracao));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static xmlAppSettingsModel ConsultarNoXml(string key, string configFile)
        {
            try
            {
                XElement xml = XElement.Load(ConfigurationManager.AppSettings.Get(configFile));
                XElement x = xml.Elements("appSettings")
                    .Descendants()
                    .Where(p => p.Attribute("key").Value.Equals(key)).First();

                var setting = new xmlAppSettingsModel();
                if (x != null)
                {
                    setting.key = x.Attribute("key").Value;
                    setting.Value = x.Attribute("value").Value;
                }
                return setting;
            }
            catch (Exception)
            {

                return new xmlAppSettingsModel();
            }
        }

        public static IEnumerable<xmlAppSettingsModel> ListarParametros(string configFile)
        {
            List<xmlAppSettingsModel> roboConfig = new List<xmlAppSettingsModel>();
            XElement xml = XElement.Load(ConfigurationManager.AppSettings.Get(configFile));
            var element = xml.Element("appSettings");
            var nodes = element.Nodes();

            foreach (var x in xml.Element("appSettings").Descendants())
            {
                xmlAppSettingsModel p = new xmlAppSettingsModel()
                {
                    key = x.Attribute("key").Value,
                    Value = x.Attribute("value").Value,
                };
                roboConfig.Add(p);
            }

            return roboConfig;
        }

        public static Configuration SetAppSettings(this Configuration config, string key, string value)
        {
            if (config == null) return config;

            var isAppSettingsExternalFile = !string.IsNullOrEmpty(config.AppSettings.File);

            if (isAppSettingsExternalFile)
            {
                var dirConfig = Path.GetDirectoryName(config.AppSettings.File);
                if (string.IsNullOrEmpty(dirConfig)) dirConfig = Path.GetDirectoryName(config.FilePath);

                string path = Path.Combine(dirConfig, Path.GetFileName(config.AppSettings.File));

                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                XmlNode node = doc.SelectSingleNode("/appSettings/add[@key='" + key + "']");
                if (node == null)
                {
                    XmlElement elem = doc.CreateElement("add");

                    elem.SetAttribute("key", key);
                    elem.SetAttribute("value", value);

                    doc.SelectSingleNode("/appSettings").AppendChild(elem);
                }
                else
                {
                    node.Attributes["value"].Value = value;
                }

                using (XmlTextWriter writer = new XmlTextWriter(path, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    doc.WriteTo(writer);
                    writer.Flush();
                    writer.Close();
                }
            }
            else
            {
                AppSettingsSection appSettings = config.AppSettings;

                if (appSettings.Settings[key] == null)
                {
                    appSettings.Settings.Add(new KeyValueConfigurationElement(key, value));
                }
                else
                {
                    appSettings.Settings[key].Value = value;
                }

                config.Save();
            }

            ConfigurationManager.RefreshSection("appSettings");

            config = ConfigurationManager.OpenMachineConfiguration();

            return config;
        }
    }
}