using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Common.Client;

namespace WpfApp1.Common.Server
{
    public class ApiServer
    {
        private HttpWebRequest wbr { get; set; }

        public ApiServer()
        {

            wbr = (HttpWebRequest)WebRequest.Create("http://localhost:6002/api/Forks/GetForks");
        }
        public List<Fork> GetForks()
        {
            try
            {
                wbr.Headers.Add("Authorization", "Bearer ghfudjkdkdl33333dddddd");
                var response = wbr.GetResponse();
                string resultForks = null;
                using(Stream stream = response.GetResponseStream())
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        resultForks = reader.ReadToEnd();
                    }
                }
                List<Fork> allForks = JsonConvert.DeserializeObject<List<Fork>>(resultForks);
                if(allForks != null && allForks.Count > 0)
                {
                    return allForks;
                }
                return new List<Fork>();
            }
            catch(Exception ex)
            {
                File.WriteAllText("logger.txt", ex.Message);   
            }
            return new List<Fork>();
        }
    }
}
