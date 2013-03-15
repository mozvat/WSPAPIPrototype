using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;

namespace WSPAPIPrototype
{
    public  class Payment : IProcess
    {
        public void Process()
        {
            PostTransaction();
            GetTransaction();
            DeleteTransaction();
            PostBatch();
            GetBatch();
        }

        /// <summary>
        /// Delete an individual Transaction: Reversal/Void?
        /// </summary>
        private static void DeleteTransaction()
        {
            //TODO https://github.com/mozvat/WSPAPIPrototype/issues/9
            //How do we handle all the different Return/Voids with and without a cardswipe.
            var result = string.Empty;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://mercuryrestapi.azurewebsites.net/api/payments/Transactions?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a&InvoiceNum=1234");
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "DELETE";
            var response = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }
        }


        /// <summary>
        /// Posts a Batch
        /// </summary>
        private static void PostBatch()
        {
            var result = string.Empty;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://mercuryrestapi.azurewebsites.net/api/payments/Batches?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a");
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new PostBatch().GetJson();

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
            PostBatch ro = Newtonsoft.Json.JsonConvert.DeserializeObject<PostBatch>(result);
            #endregion

        }


        /// <summary>
        /// Get a Batch
        /// </summary>
        private static void GetBatch()
        {
            //TODO https://github.com/mozvat/WSPAPIPrototype/issues/10
            var result = string.Empty;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://mercuryrestapi.azurewebsites.net/api/payments/Batches?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a&MerchantID=1234");
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            var response = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }
        }

        /// <summary>
        /// Get an individual tender transaction
        /// </summary>
        public static void GetTransaction()
        {
            //TODO https://github.com/mozvat/WSPAPIPrototype/issues/8
            var result = string.Empty;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://mercuryrestapi.azurewebsites.net/api/payments/Transactions?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a&MerchantID=1234&InvoiceNum=1234");
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            var response = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }
        }


        /// <summary>
        /// Post an individual tender transaction
        /// </summary>
        public static void PostTransaction()
        {
            //TODO https://github.com/mozvat/WSPAPIPrototype/issues/6
            var result = string.Empty;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://mercuryrestapi.azurewebsites.net/api/payments/Transactions?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a");
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                Card pt = new WSPAPIPrototype.Card();
                pt.Account = "Track 2 swipe";
                pt.Amount = "4.00";
                pt.InvoiceNo = "1234";
                pt.TranType = "Credit";
                pt.TranCode = "Sale";
            
                string json = pt.GetJson();

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

        public static void PostPayPal()
        {
            //TODO https://github.com/mozvat/WSPAPIPrototype/issues/6
            var result = string.Empty;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://mercuryrestapi.azurewebsites.net/api/payments/Transactions?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a");
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                AlternativePayment pt = new WSPAPIPrototype.AlternativePayment();
                pt.MobilePhone = "blah";
                pt.PicID = "binary crap";

                string json = pt.GetJson();

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




    public class PostBatch
    {
            public string IpAddress { get; set; }
            public string IpPort { get; set; }
            public string MerchantId { get; set; }
            public string TerminalId { get; set; }
            public string OperatorId { get; set; }
            public string TranCode { get; set; }
            public string SequenceNo { get; set; }
            public string TerminalName { get; set; }
            public string ShiftId { get; set; }
            public string Signature { get; set; }

            /// <summary>
            /// Gets the json.
            /// </summary>
            /// <returns></returns>
            public string GetJson()
            {
                return JsonConvert.SerializeObject(this, Formatting.None,
                                                   new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }   
    }

    //API.MPS.com/Transaction



    public class AlternativePayment
    {
        public string PicID { get; set; }
        public string MobilePhone { get; set; }
        public string TranType { get; set; }  //PayPal, DWOLLA, 
        public string TranCode { get; set; }  //GetID,Sale
        /// <summary>
        /// Gets the json.
        /// </summary>
        /// <returns></returns>
        public string GetJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.None,
                                               new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        } 
    }



    //http://mercuryrestapi.azurewebsites.net/api/payments/Transactions?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a

    //http://mercuryrestapi.azurewebsites.net/api/payments/Transactions/Credit/Sale/?
    //http://mercuryrestapi.azurewebsites.net/api/payments/Transactions

   

    public class Card
    {
        public string TranType { get; set; }  //Credit/Gift/Loyalty/Debit/
        public string TranCode { get; set; }  //Sale/Void/Return/Add/Subtract/Redeem/Issue/
        public string InvoiceNo { get; set; }
        public string Account { get; set; }
        public string Amount { get; set; }
        //Are for keyed entry
        public string EXPDate { get; set; }
        public string CVV { get; set; }
        public string AVS { get; set; }

        /// <summary>
        /// Gets the json.
        /// </summary>
        /// <returns></returns>
        public string GetJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.None,
                                               new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }     
    }

    public class RootObject
    {
        //TODO https://github.com/mozvat/WSPAPIPrototype/issues/6
        public int TransactionId { get; set; }
        //TODO Remove OperatorID property
        public int OperatorId { get; set; }
        public string TranType { get; set; }
        public string TranCode { get; set; }
        public int InvoiceNo { get; set; }
        //TODO Remove RefNo property
        public int RefNo { get; set; }
        public object Account { get; set; }
        public int AccountType { get; set; }
        public object Amount { get; set; }
        //TODO Add EXP
        //TODO Add CVV
        //TODO Add AVS
    }
}
