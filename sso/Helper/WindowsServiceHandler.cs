using System;
using System.Configuration;
using System.ServiceProcess;

namespace sso.Helper
{
    public static class WindowsServiceHandler
    {
        public static void RestartService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                if (service.Status.Equals(ServiceControllerStatus.Stopped))
                {
                    StartService(serviceName, timeoutMilliseconds);
                }else if (service.Status.Equals(ServiceControllerStatus.Running))
                {
                    StopService(serviceName, timeoutMilliseconds);
                    StartService(serviceName, timeoutMilliseconds);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void StopService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void StartService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
                // count the rest of the timeout
                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void StartServiceByName(string serviceName)
        {
            ServiceController serviceController = new ServiceController(serviceName);
            try
            {
                serviceController.MachineName = ConfigurationManager.AppSettings["ServerName"]; //this is my computer name "dt-corp-pms-04";
                serviceController.ServiceName = ConfigurationManager.AppSettings["ServiceName"]; //This is my Service name"Service1";
                serviceController.Start();
            }
            catch (Exception ex)
            {
                if (serviceController.Status == ServiceControllerStatus.Running)
                    serviceController.Stop();
            }

        }


    }
}