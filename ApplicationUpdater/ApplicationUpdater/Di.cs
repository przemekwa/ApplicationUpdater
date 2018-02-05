using Microsoft.Extensions.Configuration;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

        public void Build()
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Kernel.Bind<IISAplicationUpdater>().ToSelf();
            Kernel.Bind<IConfigurationRoot>().ToMethod(c => builder.Build());

            Kernel.Bind<IUpdateProcess>().ToMethod(c =>
            {
                var result = new SelgrosApplicationUpdateStrategy(builder.Build());

                result.UpdateEvent += UpdateEvent;
                result.ConfirmEvent += ConfirmEvent;
                result.ResultEvetnt += ResultEvetnt;

                return result;
            });
        }

        public T GetService<T>()
        {
            return (T)Kernel.GetService(typeof(T));
        }
    }
}
