using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeEngine.Entities;

namespace TradeEngine.Providers.BusinessLogic
{
    interface ITradeProvider
    {
        Task<SubmitTradeResponse> EnqueueTradeRequest(Trade tradeRQ);
        Task<TradeExecutionResponse> GetTrades(string stockName);
    }
}
