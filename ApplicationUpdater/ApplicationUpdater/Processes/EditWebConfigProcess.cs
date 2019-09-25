using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ApplicationUpdater.Processes
{
    public class EditWebConfigProcess : ProcessBase, IProcess<UpdateModel>
    {
        public EditWebConfigProcess(IConfigurationRoot configurationRoot ) : base(configurationRoot, "Edit Web.Config file")
        {
        }


        public ProcesEventResult Process(UpdateModel model)
        {
            var webConfigPath = new FileInfo(Path.Combine(model.UserParams.IntepubDirectory.FullName, "Web.config"));

            if (webConfigPath.Exists == false)
            {
                throw new Exception("Web.config file does not exist");
            }

            var xmlDoc = XDocument.Load(webConfigPath.FullName);

            var appVersionElement = xmlDoc
                .Element("configuration")
                .Element("appSettings")
                .Elements("add")
                .SingleOrDefault(s=>s.Attribute("key").Value == ConfigurationRoot["Web.Config.AppSettings.Version.KeyName"]);
            
            if (appVersionElement == null)
            {
                return GetProcesEventResult(Consts.ProcesEventResult.Error);
            }

            appVersionElement.SetAttributeValue("value", model.UserParams.Version);

            xmlDoc.Save(webConfigPath.FullName);

            UpdateProcess($"The version { model.UserParams.Version} has been set in web.config");

            return GetProcesEventResult(Consts.ProcesEventResult.Successful);
        }
    }
}
