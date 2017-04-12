using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WeHeMo.Business.Test;

namespace WeHeMo.WindowsService.Tester
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ThreadStart threadStart = new ThreadStart(Testing);
            Thread thread = new Thread(threadStart);
            thread.Start();
        }

        protected override void OnStop()
        {
        }
        protected static void Testing()
        {
            var test = new Test();

            while (true)
            {
                var testList = test.List();
                foreach (var item in testList)
                {
                    var testId = item.Key;
                    var url = item.Value;
                    var statusCode = default(int);

                    try
                    {
                        var request = (HttpWebRequest)WebRequest.Create(url);

                        using (var response = (HttpWebResponse)request.GetResponse())
                        {
                            statusCode = (int)response.StatusCode;
                        }
                    }
                    catch (WebException ex)
                    {

                        if (ex.Response == null)
                        {
                            statusCode = (int)ex.Status;
                        }
                        else
                        {
                            statusCode = (int)((HttpWebResponse)ex.Response).StatusCode;
                        }
                    }
                    test.SaveTestResult(testId, statusCode);
                }
            }
        }
    }
}
