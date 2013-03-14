using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace WSPAPIPrototype
{
    /// <summary>
    /// Loyalty ws tester
    /// </summary>
    internal class Loyalty : IProcess
    {
        /// <summary>
        /// Processes this instance.
        /// </summary>
        public void Process()
        {
            PostCredits();
            GetCredits();
        }

        /// <summary>
        /// Posts the credits.
        /// </summary>
        private void PostCredits()
        {
            var httpWebRequest =
                (HttpWebRequest)
                WebRequest.Create(
                    "http://mercuryrestapi.azurewebsites.net/api/loyalty/credits?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a");
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new Credit().GetJson();

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string response = streamReader.ReadToEnd();
                    Trace.WriteLine(response);
                }
            }
        }

        /// <summary>
        /// Gets the credits.
        /// </summary>
        /// <returns></returns>
        private Credit GetCredits()
        {
            Credit credit;

            var httpWebRequest =
                (HttpWebRequest)
                WebRequest.Create(
                    "http://mercuryrestapi.azurewebsites.net/api/loyalty/credits?customerid=12345&timestamp=50394852&hash=3a23fb806103fb33d8518c7f505232674976217a");
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "GET";

            var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Credit));
                credit = (Credit)ser.ReadObject(streamReader.BaseStream);
            }

            return credit;
        }
    }

    /// <summary>
    /// Credit type
    /// </summary>
    public class Credit
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Credit" /> class. Just throw some data in to test with.
        /// </summary>
        public Credit()
        {
            CustomerIdentifier = 1;
            Description = "POS Purchase";
            EmployeeId = 1;
            StationId = 1;
            TicketId = 123;
            Revenue = 101.10;
            CardNumber = "1234123412341234";
            Language = "en-us";
            Version = "1.0";
            Delay = 1;
            TestMode = false;
            Match = "Test";
            DeviceId = 1;
        }

        /// <summary>
        /// Gets the json.
        /// </summary>
        /// <returns></returns>
        public string GetJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.None,
                                               new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
        }        

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier.
        /// </value>
        public int CustomerIdentifier { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the employee id.
        /// </summary>
        /// <value>
        /// The employee id.
        /// </value>
        public int EmployeeId { get; set; }
        /// <summary>
        /// Gets or sets the station id.
        /// </summary>
        /// <value>
        /// The station id.
        /// </value>
        public int StationId { get; set; }
        /// <summary>
        /// Gets or sets the ticket id.
        /// </summary>
        /// <value>
        /// The ticket id.
        /// </value>
        public int TicketId { get; set; }
        /// <summary>
        /// Gets or sets the revenue.
        /// </summary>
        /// <value>
        /// The revenue.
        /// </value>
        public double Revenue { get; set; }
        /// <summary>
        /// Gets or sets the card number.
        /// </summary>
        /// <value>
        /// The card number.
        /// </value>
        public string CardNumber { get; set; }
        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public string Language { get; set; }
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version { get; set; }
        /// <summary>
        /// Gets or sets the delay.
        /// </summary>
        /// <value>
        /// The delay.
        /// </value>
        public int Delay { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [test mode].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [test mode]; otherwise, <c>false</c>.
        /// </value>
        public bool TestMode { get; set; }
        /// <summary>
        /// Gets or sets the match.
        /// </summary>
        /// <value>
        /// The match.
        /// </value>
        public string Match { get; set; }
        /// <summary>
        /// Gets or sets the device id.
        /// </summary>
        /// <value>
        /// The device id.
        /// </value>
        public int DeviceId { get; set; }
    }
}
