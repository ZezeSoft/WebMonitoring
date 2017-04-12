using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeHeMo.DTO;

namespace WeHeMo.Business.Test
{
    public interface ITest
    {
        Guid Add(Guid customerID, string url);
        void SaveTestResult(Guid testID, int StatusCode);
        //Guid[] List();
        Dictionary<Guid,string> List();
        void Delete(Guid testID);
        DTO_TEST[] List(Guid customerID);
    }
}
