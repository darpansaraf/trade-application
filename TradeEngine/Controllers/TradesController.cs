using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TradeEngine.Entities;
using TradeEngine.Providers.BusinessLogic;

namespace TradeEngine.Controllers
{
    public class TradesController : ApiController
    {
        private static TradeProvider _tradeProvider;

        public TradesController()
        {
            if (_tradeProvider == null)
                _tradeProvider = new TradeProvider();
        }

        [HttpGet]
        [Route("trades/{stockName}")]
        public async Task<TradeExecutionResponse> Get(string stockName)
        {
            var tradeExecutionResponse = new TradeExecutionResponse();
            try
            {
                tradeExecutionResponse = await _tradeProvider.GetTrades(stockName);
            }
            catch (Exception ex)
            {
                tradeExecutionResponse.Status = "Failure";
                tradeExecutionResponse.Error = new Error()
                {
                    ErrorCode = "500",
                    ErrorMessage = "Internal Server Error"
                };
            }
            return tradeExecutionResponse;
        }

        [HttpPost]
        [Route("trades/submit")]
        public async Task<SubmitTradeResponse> Post([FromBody]Trade trade)
        {
            var response = new SubmitTradeResponse();
            try
            {
                response = await _tradeProvider.EnqueueTradeRequest(trade);
            }
            catch (Exception ex)
            {
                response.Status = "Failure";
                response.Error = new Error()
                {
                    ErrorCode = "500",
                    ErrorMessage = "Internal Server Error"
                };
            }
            return response;
        }
    }
}
