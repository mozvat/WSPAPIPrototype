using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace WSPAPIPrototype
{
    class Gift : IProcess
    {
        public void Process()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Post an individual tender transaction
        /// </summary>
        private static void PostTransaction()
        {
            //TODO https://github.com/mozvat/WSPAPIPrototype/issues/6
            var result = string.Empty;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://mercuryrestapi.azurewebsites.net/api/payments/Transactions?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a");
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";

            Card postTransaction = new Card();
            postTransaction.Account = "Gift Card num";
            postTransaction.Amount = "4.00";
            postTransaction.TranCode = "";
            postTransaction.TranType = "Sale";
            postTransaction.Account = "Gift";



            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new Card().GetJson();

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                    Trace.WriteLine(result);
                }
            }

            #region JSON Deserialization
            //Put a break point here and checkout the RootObject data. 
            Card ro = Newtonsoft.Json.JsonConvert.DeserializeObject<Card>(result);
            #endregion
        }
    }
}
