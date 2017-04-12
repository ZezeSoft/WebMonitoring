using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WeHeMo.Business.Customer;
using WeHeMo.Business.Test;
using WeHeMo.Service.Contract;

namespace WeHeMo.Service.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service.svc or Service.svc.cs at the Solution Explorer and start debugging.
    public class Service : IService
    {
        public Guid CustomerAdd(string name, string email, string password)
        {
            var customer = new Customer();
            return customer.Add(name, email, password);
        }

        public Guid? CustomerLogin(string email, string password)
        {
            var customerLogin = new Customer();
            return customerLogin.Login(email, password);
        }

        public DTO.DTO_CUSTOMER CustomerGet(Guid customerId)
        {
            var customerGet = new Customer();
            return customerGet.Get(customerId);
        }

        public void CustomerUpdate(Guid customerId, string name, string email, string password)
        {
            var updateCustomer = new Customer();
            updateCustomer.Update(customerId, name, email, password);
        }

        public Guid TestAdd(Guid customerID, string url)
        {
            var testAdd = new Test();
            return testAdd.Add(customerID, url);
        }

        public DTO.DTO_TEST[] TestList(Guid customerId)
        {
            var testList = new Test();
            return testList.List(customerId);
        }

        public DTO.DTO_TEST_REPORT[] TestReport(Guid testId)
        {
            var testReport = new Test();
            return testReport.GetTestReport(testId);
        }

        public void TestDelete(Guid testId)
        {
            var testDelete = new Test();
            testDelete.Delete(testId);
        }
    }
}
