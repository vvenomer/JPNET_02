using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;
using Unity.Injection;
using Microsoft.Practices.Unity.Configuration;

namespace JPNET_02
{
    class Program
    {
        interface ILogger
        {
            void Log(string s);
        }
        class Logger : ILogger
        {
            public void Log(string s) { Console.WriteLine("logger: " + s); }
        }

        class Worker
        {
            public Worker(ILogger log) { m_log = log; }
            public void Work()
            {
                m_log.Log("begin");
                m_log.Log("end");
            }
            private ILogger m_log;
        }

        class NumLogger : ILogger
        {
            public NumLogger(int i = 0) { id = i; }
            public void Log(string s) { Console.WriteLine("logger(" + id++ + "): " + s); }
            private int id;
        }

        class MetWorker
        {
            public void Work()
            {
                m_log.Log("begin");
                m_log.Log("end");
            }
            public ILogger m_log { get; set; }
        }

        static void Main(string[] args)
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<ILogger, Logger>();

            var w = container.Resolve<Worker>();
            w.Work();

            //container.LoadConfiguration();

            container.RegisterType<ILogger, NumLogger>(
                "numlog",
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(1)
                );

            var w2 = container.Resolve<MetWorker>("numwork");
            w2.Work();

            Console.ReadKey();
        }
    }
}
