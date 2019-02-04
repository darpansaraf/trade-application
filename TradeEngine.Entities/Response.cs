using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TradeEngine.Entities
{
    [DataContract]
    public class Response
    {
        [DataMember]
        public Error Error { get; set; }

        [DataMember]
        public string Status { get; set; }
    }
}
