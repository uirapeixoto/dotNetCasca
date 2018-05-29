using sso.ViewModel;
using System;
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

        public static xmlAppSettingsModel ConsultarNoXml(string key)
        {
            try
            {
                XElement xml = XElement.Load(ConfigurationManager.AppSettings.Get("roboSicaqSettings"));
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
    }
}