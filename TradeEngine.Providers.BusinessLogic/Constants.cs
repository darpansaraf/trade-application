using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeEngine.Providers.BusinessLogic
{
    internal static class Constants
    {
        internal static class ResponseStatus
        {
            internal const string Success = "Success";
            internal const string Failure = "Failure";
        }

        internal static class TradeType
        {
            internal const string BUY = "BUY";
            internal const string SELL = "SELL";
        }

        internal static class ErrorMessages
        {
            internal const string TradeCannotBeNull = "Trade cannot be null";
            internal const string EmptyStockName = "Stock Name cannot be Empty";
            internal const string InvalidStockPrice = "Invalid Stock Price";
            internal const string InvalidStockQuantity = "Invalid Stock Qunatity";
            internal const string EmptyTradeType = "Trade Type cannot be Empty";
            internal const string InvalidTradeType = "Invalid Trade Type. Trade Type can either be BUY or SELL";
        }


    }
}
