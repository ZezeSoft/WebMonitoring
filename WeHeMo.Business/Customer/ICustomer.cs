using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeHeMo.DTO;

namespace WeHeMo.Business.Customer
{
    public interface ICustomer
    {
        Guid Add(string name, string email, string password);
        Guid? Login(string email, string password);
        void Update(Guid customerID,string name, string email, string password);
        DTO_CUSTOMER Get(Guid customerId);
    }
}
