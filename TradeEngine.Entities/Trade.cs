using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TradeEngine.Entities
{
    [DataContract]
    public class Trade
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public double Price { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        public DateTime SubmitDateTime { get; set; }

        public int Id { get; set; }

    }
}
