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
        internal static XmlDocument _doc = new XmlDocument();
        internal static XmlNode _node;
        internal static XmlElement _elem;
        internal static string _path;


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
            //verifica se as configurações do appSettins estão em um arquivo externo
            var isAppSettingsExternalFile = !string.IsNullOrEmpty(config.AppSettings.File);

            if (isAppSettingsExternalFile)
            {
                var dirConfig = Path.GetDirectoryName(config.AppSettings.File);
                if (string.IsNullOrEmpty(dirConfig)) dirConfig = Path.GetDirectoryName(config.FilePath);
                _path = Path.Combine(dirConfig, Path.GetFileName(config.AppSettings.File));
                _doc.Load(_path);
            }
            else
            {
                _path = config.FilePath;
                _doc = loadConfigDocument(config);
            }

            _node = _doc.SelectSingleNode("/appSettings");
            _elem = (XmlElement)_node.SelectSingleNode(string.Format("//add[@key='{0}']", key));

            if (_node == null)
                throw new InvalidOperationException("appSettings section not found in config file.");

            try
            {
               
                if (_elem != null)
                {
                    // add value for key
                    //_elem.SetAttribute("value", value);
                    WebConfigurationManager.AppSettings.Set(key, value);
                }
                else
                {
                    // key was not found so create the 'add' element 
                    // and set it's key/value attributes 
                    _elem = _doc.CreateElement("add");
                    _elem.SetAttribute("key", key);
                    _elem.SetAttribute("value", value);
                    _node.AppendChild(_elem);
                }
                _doc.Save(_path);
                ConfigurationManager.RefreshSection("appSettings");
                
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