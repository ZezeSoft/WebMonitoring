using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using WeHeMo.Service.Contract;

namespace WeHeMo.UI.Controllers
{
    public class CustomerController : Controller
    {
        //
        // GET: /Customer/
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(DTO.DTO_CUSTOMER model)
        {
            BasicHttpBinding myBinding = new BasicHttpBinding();
            EndpointAddress myEndpoint = new EndpointAddress("http://localhost:3243/Service.svc");
            ChannelFactory<IService> myChannel = new ChannelFactory<IService>(myBinding, myEndpoint);
            var channel = myChannel.CreateChannel();

            var customerId = channel.CustomerLogin(model.Email, model.Password);
            if (customerId == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı");
            }
            Session["CustomerId"] = customerId;
            return RedirectToAction("List","Test");
            
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(DTO.DTO_CUSTOMER model)
        {
            BasicHttpBinding myBinding = new BasicHttpBinding();
            EndpointAddress myEndpoint = new EndpointAddress("http://localhost:3243/Service.svc");
            ChannelFactory<IService> myChannel = new ChannelFactory<IService>(myBinding, myEndpoint);
            var channel = myChannel.CreateChannel();

            channel.CustomerAdd(model.Name, model.Email, model.Password);

            return RedirectToAction("Login");
        }
        public ActionResult Profile()
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

            var model = channel.CustomerGet(customerId);

            return View(model);
        }
        [HttpPost]
        public ActionResult Profile(DTO.DTO_CUSTOMER model)
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
            
            channel.CustomerUpdate(customerId, model.Name, model.Email, model.Password);
            ModelState.AddModelError("", "Profiliniz başarıyla güncellendi");
            return View();
        }

	}
}