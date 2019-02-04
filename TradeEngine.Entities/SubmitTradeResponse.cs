using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TradeEngine.Entities
{
    [DataContract]
    public class SubmitTradeResponse : Response
    {
        [DataMember]
        public string TradeId { get; set; }
    }
}
