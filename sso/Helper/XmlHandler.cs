using sso.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Configuration;
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

        public static Configuration SetWebAppSettings(this Configuration config, string key, string value)
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

                XmlNode node = doc.SelectSingleNode(string.Format("/appSettings/add[@key='{0}']", key));
                if (node == null)
                {
                    XmlElement elem = doc.CreateElement("add");

                    elem.SetAttribute("key", key);
                    elem.SetAttribute("value", value);

                    doc.SelectSingleNode("/appSettings").AppendChild(elem);
                }
                else
                {

                    XmlElement elem = (XmlElement)node.SelectSingleNode(string.Format("//add[@key='{0}']", key));
                    elem.SetAttribute("value", value);
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

            config = WebConfigurationManager.OpenWebConfiguration("~");

            return config;
        }

        public static void WriteSetting(Configuration config, string key, string value)
        {
            // load config document for current assembly
            XmlDocument doc = loadConfigDocument(config);

            // retrieve appSettings node
            XmlNode node = doc.SelectSingleNode("//appSettings");

            if (node == null)
                throw new InvalidOperationException("appSettings section not found in config file.");

            try
            {
                // select the 'add' element that contains the key
                XmlElement elem = (XmlElement)node.SelectSingleNode(string.Format("//add[@key='{0}']", key));

                if (elem != null)
                {
                    // add value for key
                    elem.SetAttribute("value", value);
                }
                else
                {
                    // key was not found so create the 'add' element 
                    // and set it's key/value attributes 
                    elem = doc.CreateElement("add");
                    elem.SetAttribute("key", key);
                    elem.SetAttribute("value", value);
                    node.AppendChild(elem);
                }
                doc.Save(config.FilePath);
            }
            catch
            {
                throw;
            }
        }

        private static XmlDocument loadConfigDocument(Configuration config)
        {
            XmlDocument doc = null;
            try
            {
                doc = new XmlDocument();
                doc.Load(config.FilePath);
                return doc;
            }
            catch (System.IO.FileNotFoundException e)
            {
                throw new Exception("No configuration file found.", e);
            }
        }
    }
}