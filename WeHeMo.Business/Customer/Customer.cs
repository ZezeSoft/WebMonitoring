using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeHeMo.Business.Database;
using WeHeMo.DTO;

namespace WeHeMo.Business.Customer
{
    public class Customer : ICustomer
    {
        public Guid Add(string name, string email, string password)
        {
            var item = new CUSTOMER();
           
                item.ID = Guid.NewGuid();
                item.NAME = name;
                item.EMAIL = email;
                item.PASSWORD = password;
                item.CREATDATE = DateTime.Now;
            

            using (var dc = new WeHeMoDBDataContext())
            {
                dc.CUSTOMERs.InsertOnSubmit(item);
                dc.SubmitChanges();
            }

            return item.ID;
        }

        public Guid? Login(string email, string password)
        {
            using (var dc = new WeHeMoDBDataContext())
            {
                var id = dc.CUSTOMERs.Where(x => x.EMAIL == email && x.PASSWORD == password).Select(x => x.ID).FirstOrDefault();

                if (id == default(Guid))
                {
                    return null;
                }

                return id;
            }
        }

        public void Update(Guid customerID, string name, string email, string password)
        {
            using (var dc = new WeHeMoDBDataContext())
            {
                var item = dc.CUSTOMERs.Where(x => x.ID == customerID).First();

                item.NAME = name;
                item.EMAIL = email;
                item.PASSWORD = password;
                item.UPDATEDATE = DateTime.Now;

                dc.SubmitChanges();
            }
        }


        public DTO.DTO_CUSTOMER Get(Guid customerId)
        {
            using (var dc = new WeHeMoDBDataContext())
            {
                return (from c in dc.CUSTOMERs
                        where c.ID == customerId
                        select new DTO_CUSTOMER
                            {
                                Name = c.NAME,
                                Password = c.PASSWORD,
                                Email = c.EMAIL
                            }
                       ).FirstOrDefault();
            }
        }
    }
}
