using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using WeHeMo.Service.Contract;

namespace WeHeMo.UI.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(DTO.DTO_TEST model)
        {
            BasicHttpBinding myBinding = new BasicHttpBinding();
            EndpointAddress myEndpoint = new EndpointAddress("http://localhost:3243/Service.svc");
            ChannelFactory<IService> myChannel = new ChannelFactory<IService>(myBinding, myEndpoint);
            var channel = myChannel.CreateChannel();

            var obj = Session["CustomerId"];
            if (obj == null)
            {
                RedirectToAction("Login");
            }
            var customerId = (Guid)obj;

            channel.TestAdd(customerId, model.Url);

            return View();
        }

        public ActionResult List()
        {
            BasicHttpBinding myBinding = new BasicHttpBinding();
            EndpointAddress myEndpoint = new EndpointAddress("http://localhost:3243/Service.svc");
            ChannelFactory<IService> myChannel = new ChannelFactory<IService>(myBinding, myEndpoint);
            var channel = myChannel.CreateChannel();

            var obj = Session["CustomerId"];
            if (obj == null)
            {
                RedirectToAction("Login");
            }
            var customerId = (Guid)obj;

            var model = channel.TestList(customerId);

            return View(model.ToList());
        }
        public ActionResult Report(Guid? id)
        {
            if (id.Value == null)
            {
                return RedirectToAction("List");
            }

            var channel = OpenChannel();
            var model = channel.TestReport(id.Value);

            return View(model.ToList());
        }
        public ActionResult Delete(Guid? id)
        {
            var channel = OpenChannel();
            channel.TestDelete(id.Value);

            return RedirectToAction("Report");
        }
        public static IService OpenChannel()
        {
            BasicHttpBinding myBinding = new BasicHttpBinding();
            EndpointAddress myEndpoint = new EndpointAddress("http://localhost:3243/Service.svc");
            ChannelFactory<IService> myChannel = new ChannelFactory<IService>(myBinding, myEndpoint);
            var channel = myChannel.CreateChannel();
            return channel;
        }
    }
}