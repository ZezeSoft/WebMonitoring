using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using WeHeMo.Business.Database;
using WeHeMo.DTO;

namespace WeHeMo.Business.Test
{
    public class Test : ITest
    {
        public Guid Add(Guid customerID, string url)
        {
            var item = new TEST()
            {
                ID =Guid.NewGuid(),
                CUSTOMERID = customerID,
                URL = url,
                CREATEDATE = DateTime.Now
            };

            using (var dc = new WeHeMoDBDataContext())
            {
                dc.TESTs.InsertOnSubmit(item);
                dc.SubmitChanges();
            }

            return item.ID;
        }

        public void SaveTestResult(Guid testID, int statusCode)
        {
            using (var dc = new WeHeMoDBDataContext())
            {
                var statusID = dc.STATUSCODEs.Where(x => x.CODE == statusCode).Select(x => x.ID).First();

                var item = new TESTRESULT()
                {
                    ID = Guid.NewGuid(),
                    STATUSCODEID = statusID,
                    TESTID = testID,
                    CREATDATE = DateTime.Now
                };

                dc.TESTRESULTs.InsertOnSubmit(item);

                var test = dc.TESTs.Where(x => x.ID == testID).First();

                test.STATUSCODE = statusCode;
                test.TESTDATE = DateTime.Now;

                dc.SubmitChanges();
                if (statusCode != 200)
                {
                    var email = dc.TESTs.Where(x => x.ID == testID).Select(x => new { x.URL,x.CUSTOMER.EMAIL }).First();

                    var client = new SmtpClient();
                    client.Host = "srvm07.trwww.com";
                    client.Port = 587;
                    client.Credentials = new NetworkCredential("wehemo@exedra.com.tr", "W3hemomail");
                    var mailMessage = new MailMessage();
                    var error = dc.STATUSCODEs.Where(x=> x.CODE==statusCode).Select(x=>x.DETAIL).First();
                    mailMessage.Body = "Sayın Yetkili<br>"+ email.URL+" adresine sahip sayfanızda sorun var.<br>Alınan Hata : <spanstyle:color:red>"+error+"</span>";
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Subject = "Konu Test";
                    mailMessage.To.Add("zeynep_ekici@yahoo.com.tr");
                    
                    mailMessage.From = new MailAddress("wehemo@exedra.com.tr");
                    client.Send(mailMessage);


                    //var sendSuccessMail = statusCode==200 && test.STATUSCODE && test.STATUSCODE!=200 && test.STATUSCODE!=200;

                }
            }
        }

        public Dictionary<Guid, string> List()
        {
            using (var dc = new WeHeMoDBDataContext())
            {
                return dc.TESTs.Select(x => new {x.ID,x.URL }).ToDictionary(x=>x.ID,x=>x.URL);
            }
        }

        public void Delete(Guid testID)
        {
            using (var dc = new WeHeMoDBDataContext())
            {
                var item = dc.TESTs.Where(x => x.ID == testID).First();
                dc.TESTs.DeleteOnSubmit(item);
                dc.SubmitChanges();
            }
        }

        public DTO_TEST[] List(Guid customerID)
        {
            using (var dc = new WeHeMoDBDataContext())
            {
                return (from t in dc.TESTs
                        join s in dc.STATUSCODEs on t.STATUSCODE equals s.CODE
                        into sTemp
                        from sResult in sTemp.DefaultIfEmpty() // left join
                        where t.CUSTOMERID == customerID
                        orderby t.URL
                        select new DTO_TEST
                        {
                            ID = t.ID,
                            Status = sResult.DETAIL,
                            Url = t.URL,
                            Date = t.TESTDATE,
                            Succeed = t.STATUSCODE == null ? default(bool?) : (t.STATUSCODE == 200)//Succeed nullable olduğu için null da dönebilmeli burası, sadece true yada false değil
                        }).ToArray();
            }

        }

        public DTO_TEST_REPORT[] GetTestReport(Guid testId)
        {
            using (var dc = new WeHeMoDBDataContext())
            {
                return (
                            from tr in dc.TESTRESULTs
                            join s in dc.STATUSCODEs on tr.STATUSCODEID equals s.ID
                            where tr.TESTID == testId
                            select new DTO_TEST_REPORT
                            {
                                Description = s.DETAIL,
                                TestDate = tr.CREATDATE,
                                Succeed = (s.CODE == 200)
                            }
                       ).ToArray();
            }
        }

        public void StatusEnumToWeHeMoStatusTable(params Type[] types)
        {
            using (var db = new WeHeMoDBDataContext())
            {
                foreach (var type in types)
                {
                    var names = type.GetEnumNames();
                    var values = type.GetEnumValues();

                    for (int i = 0; i < type.GetEnumNames().Length; i++)
                    {
                        if (!db.STATUSCODEs.Any(K => K.CODE == (int)values.GetValue(i)))
                        {
                            var code = (int)((HttpStatusCode)values.GetValue(i));
                            db.STATUSCODEs.InsertOnSubmit(new STATUSCODE {  DETAIL = names[i],  CODE = code });
                            db.SubmitChanges();
                        }
                    }
                }
            }
        }

        private void Window_Loaded(object sender)
        {
            StatusEnumToWeHeMoStatusTable(typeof(HttpStatusCode), typeof(WebExceptionStatus));
        }
    }
}
