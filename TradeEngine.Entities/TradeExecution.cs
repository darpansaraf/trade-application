using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TradeEngine.Entities
{
    [DataContract]
    public class TradeExecution
    {
        [DataMember]
        public int BuyRequestId { get; set; }

        [DataMember]
        public int SellRequestId { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public double Price { get; set; }
    }
}
