using Microsoft.Extensions.Configuration;
using Ninject;
using System;
using System.IO;

namespace ApplicationUpdater
{
    public class Di
    {
        public IKernel Kernel { get; set; }

        public event EventHandler UpdateEvent;
        public event EventHandler ConfirmEvent;
        public event EventHandler ResultEvetnt;

        public Di(IKernel kernel, EventHandler updateEvent, EventHandler confirmEvent, EventHandler resultEvetnt)
        {
            Kernel = new StandardKernel();
            UpdateEvent = updateEvent;
            ConfirmEvent = confirmEvent;
            ResultEvetnt = resultEvetnt;
        }

        public void Build(string strategy)
        {
            var builder = new ConfigurationBuilder()
                               .SetBasePath(Directory.GetCurrentDirectory())
                               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Kernel.Bind<IISAplicationUpdater>().ToSelf();
            Kernel.Bind<IConfigurationRoot>().ToMethod(c => builder.Build());
            Kernel.Bind<IEnvironmentManager>().To<EnvironmentManager>();

            switch (strategy)
            {
                case "Selgros":
                    Kernel.Bind<IUpdateProcess>().To<SelgrosApplicationUpdateStrategy>().OnActivation(s =>
                                    {
                                        s.ConfirmEvent += ConfirmEvent;
                                        s.ResultEvetnt += ResultEvetnt;
                                        s.UpdateEvent += UpdateEvent;
                                    });
                    break;
                case "Orlen":
                    Kernel.Bind<IUpdateProcess>().To<OrlenApplicationUpdateStrategy>().OnActivation(s =>
                                    {
                                        s.ConfirmEvent += ConfirmEvent;
                                        s.ResultEvetnt += ResultEvetnt;
                                        s.UpdateEvent += UpdateEvent;
                                    });
                    break;

                default:
                    throw new Exception("No proper strategy set");
            }
        }

        public T GetService<T>()
        {
            return (T)Kernel.GetService(typeof(T));
        }
    }
}
