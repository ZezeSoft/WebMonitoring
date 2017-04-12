using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WeHeMo.DTO;

namespace WeHeMo.Service.Contract
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        Guid CustomerAdd(string name, string email, string password);
        [OperationContract]
        Guid? CustomerLogin(string email, string password);
        [OperationContract]
        DTO_CUSTOMER CustomerGet(Guid customerId);
        [OperationContract]
        void CustomerUpdate(Guid customerId, string name, string email,string password);
        [OperationContract]
        Guid TestAdd(Guid customerID, string url);
        [OperationContract]
        DTO_TEST[] TestList(Guid customerId);
        [OperationContract]
        DTO_TEST_REPORT[] TestReport(Guid testId);
        [OperationContract]
        void TestDelete(Guid testId);
    }
}
