using sso.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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

        public static void EditarConfiguracao(string key, string value)
        {
            if(!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
            {
                Configuration objConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                AppSettingsSection objAppsettings = (AppSettingsSection)objConfig.GetSection("appSettings");
                //Edit
                if (objAppsettings != null)
                {
                    objAppsettings.Settings[key].Value = value;
                    objConfig.Save();
                }
            }
        }
    }
}