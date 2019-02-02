using Newtonsoft.Json;

namespace AccountOwnerServer.Models
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
 
 
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}