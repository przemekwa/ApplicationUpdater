using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ApplicationUpdater.Processes
{
    public class EditWebConfig : ProcessBase, IProcess<UpdateModel>
    {
        public ProcesEventResult Process(UpdateModel model)
        {
            var webConfigPath = new FileInfo(Path.Combine(model.IntepubDirectory.FullName, "Web.config"));

            if (webConfigPath.Exists == false)
            {
                throw new Exception("Web.config nie istnieje");
            }

            var xmlDoc = XDocument.Load(webConfigPath.FullName);

            var appVersionElement = xmlDoc
                .Element("configuration")
                .Element("appSettings")
                .Elements("add")
                .SingleOrDefault(s=>s.Attribute("key").Value == "AppVersion");
            
            if (appVersionElement == null)
            {
                throw new Exception("Nie można zmianić numeru wersji");
            }

            appVersionElement.SetAttributeValue("value", model.Version);

            xmlDoc.Save(webConfigPath.FullName);

            UpdateProcess($"Ustawiono wersję w web.config na { model.Version}");

            return new ProcesEventResult
            {
                Result = true
            };
        }
    }
}
