using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeEngine.Entities;

namespace TradeEngine.Providers.BusinessLogic
{
    public class TradeProvider : ITradeProvider
    {
        private static List<Trade> _tradeQueue;

        public TradeProvider()
        {
            _tradeQueue = new List<Trade>();
        }

        public async Task<SubmitTradeResponse> EnqueueTradeRequest(Trade trade)
        {
            var submitTradeResponse = new SubmitTradeResponse() { Status = Constants.ResponseStatus.Success };
            try
            {
                var validationError = ValidateTrade(trade);
                if (validationError !=null)
                {
                    submitTradeResponse.Status = Constants.ResponseStatus.Failure;
                    submitTradeResponse.Error = validationError;
                }
                else
                {
                    trade.SubmitDateTime = DateTime.Now;
                    /*
                     * NOTE: Currently we are saving our stocks in a static list.
                     * Tomorrow, may be we might same them in the database.
                     * That is where async and await will be most useful.
                     */
                    _tradeQueue.Add(trade);
                    var currentQueueCount = _tradeQueue.Count();
                    trade.Id = currentQueueCount;
                    submitTradeResponse.TradeId = "R" + trade.Id;
                }
            }
            catch(Exception ex)
            {
                submitTradeResponse.Error = GetError("500", "An Error occurred while submitting Trade request.");
                submitTradeResponse.Status = Constants.ResponseStatus.Failure;
            }
            return submitTradeResponse;
        }

        private Error ValidateTrade(Trade trade)
        {
            var errorMessage = string.Empty;
            if (trade == null)
                errorMessage = Constants.ErrorMessages.TradeCannotBeNull;
            else if (string.IsNullOrEmpty(trade.Name))
                errorMessage = Constants.ErrorMessages.EmptyStockName;
            else if (trade.Price <= 0)
                errorMessage = Constants.ErrorMessages.InvalidStockPrice;
            else if (trade.Quantity <= 0)
                errorMessage = Constants.ErrorMessages.InvalidStockQuantity;
            else if (string.IsNullOrEmpty(trade.Type))
                errorMessage = Constants.ErrorMessages.EmptyTradeType;
            else
            {
                if (!(trade.Type == Constants.TradeType.BUY || trade.Type == Constants.TradeType.SELL))
                    errorMessage = Constants.ErrorMessages.InvalidTradeType;
            }
            if (string.IsNullOrEmpty(errorMessage))
                return null;
            return new Error()
            {
                ErrorCode = "999",
                ErrorMessage = errorMessage
            };
        }

        public async Task<TradeExecutionResponse> GetTrades(string stockName)
        {
            var tradeExecutionResponse = new TradeExecutionResponse() { TradeExecutions = new List<TradeExecution>(),Status = "Success" };
            if (_tradeQueue.Find(x => x.Name == stockName) != null)
            {
                while (true)
                {
                    try
                    {
                        if (!CanTradeBePerformed(stockName))
                        {
                            if (tradeExecutionResponse.TradeExecutions.Count == 0)
                            {
                                tradeExecutionResponse.Error = GetError("500", "No trade can be performed for Stock: " + stockName);
                                tradeExecutionResponse.Status = Constants.ResponseStatus.Failure;
                            }
                            break;
                        }

                        var sellRequests = _tradeQueue.FindAll(x => x.Name == stockName && x.Type == Constants.TradeType.SELL);
                        if (sellRequests != null && sellRequests.Count > 0)
                        {
                            sellRequests = (from sr in sellRequests orderby sr.Price select sr).ToList();
                            var lowestSellRequest = sellRequests[0];

                            var buyRequests = _tradeQueue.FindAll(x => x.Name == stockName && x.Type == Constants.TradeType.BUY);
                            buyRequests = (from br in buyRequests orderby br.Price descending select br).ToList();
                            if (buyRequests != null && buyRequests.Count > 0)
                            {
                                var highestBuyRequest = buyRequests[0];
                                if (lowestSellRequest.Quantity >= highestBuyRequest.Quantity && highestBuyRequest.Price >= lowestSellRequest.Price)
                                {
                                    tradeExecutionResponse.TradeExecutions.Add(new TradeExecution()
                                    {
                                        BuyRequestId = highestBuyRequest.Id,
                                        SellRequestId = lowestSellRequest.Id,
                                        Price = lowestSellRequest.Price,
                                        Quantity = highestBuyRequest.Quantity
                                    });
                                    _tradeQueue.Find(x => x.Id == lowestSellRequest.Id).Quantity = lowestSellRequest.Quantity - highestBuyRequest.Quantity;
                                    _tradeQueue.Remove(highestBuyRequest);
                                }
                                else
                                {
                                    if (lowestSellRequest.Price <= highestBuyRequest.Price)
                                    {
                                        tradeExecutionResponse.TradeExecutions.Add(new TradeExecution()
                                        {
                                            BuyRequestId = highestBuyRequest.Id,
                                            SellRequestId = lowestSellRequest.Id,
                                            Price = lowestSellRequest.Price,
                                            Quantity = lowestSellRequest.Quantity
                                        });
                                        _tradeQueue.Find(x => x.Id == highestBuyRequest.Id).Quantity = highestBuyRequest.Quantity - lowestSellRequest.Quantity;
                                        _tradeQueue.Remove(lowestSellRequest);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        tradeExecutionResponse.Error = GetError("500", "An Error occurred while fetching trades on Stock: " + stockName);
                        tradeExecutionResponse.Status = Constants.ResponseStatus.Failure;
                    }
                }
            }
            else
            {
                tradeExecutionResponse.Error = GetError("404", "No Trade Requests found for Stock: " + stockName);
                tradeExecutionResponse.Status = Constants.ResponseStatus.Failure;
            }
            return tradeExecutionResponse;
        }

        private Error GetError(string errorCode, string errorMessage)
        {
            return new Error()
            {
                ErrorCode = errorCode,
                ErrorMessage = errorMessage
            };
        }

        private bool CanTradeBePerformed(string stockName)
        {
            /*
             * NOTE: A trade can only be performed when the BUY request price is higher than or equal to the SELL Request Price.
             * The code below checks whether there exist a BUY request whose price is greater than or equal to a SELL request.
             * If Yes it returns 'true' otherwise 'false'
             */
            var buyRequests = _tradeQueue.FindAll(x => x.Name == stockName && x.Type == Constants.TradeType.BUY);
            buyRequests = (from br in buyRequests orderby br.Price descending select br).ToList();

            if (buyRequests != null && buyRequests.Count > 0)
            {
                var highestBuyRequest = buyRequests[0];
                var sellRequests = _tradeQueue.FindAll(x => x.Name == stockName && x.Type == Constants.TradeType.SELL);
                sellRequests = (from sr in sellRequests orderby sr.Price select sr).ToList();
                if (sellRequests != null && sellRequests.Count > 0)
                {
                    var lowestSellRequest = sellRequests[0];
                    if (highestBuyRequest.Price >= lowestSellRequest.Price)
                        return true;
                }
                return false;
            }
            return false;
        }
    }
}
