using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeHeMo.Business.Test;
using System.Net;

namespace WeHeMo.Tester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var testBusiness = new Test();

            while(true)
            {
                var testler = testBusiness.List();
                foreach (var test in testler)
                {
                    var testId = test.Key;
                    var url = test.Value;
                    var statusCode = default(int);
                    try
                    {
                        var request = (HttpWebRequest)WebRequest.Create(url);

                        using(var response = (HttpWebResponse)request.GetResponse())
                        {
                            statusCode = (int)response.StatusCode;
                        }
                    }
                    catch (WebException ex)
                    {

                        if (ex.Response==null )
                        {
                            statusCode = (int)ex.Status;
                        }
                        else
                        {
                            statusCode = (int)((HttpWebResponse)ex.Response).StatusCode;
                        }
                    }
                    testBusiness.SaveTestResult(testId, statusCode);

                }
            }


        }

    }
}
