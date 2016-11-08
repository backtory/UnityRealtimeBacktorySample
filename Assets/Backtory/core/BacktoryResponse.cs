using System;
using Sdk.Core.Models.Connectivity.Chat;

namespace Assets.Backtory.core
{
    public abstract class BacktoryResponse
    {
        protected BacktoryResponse(int code, string message, bool successful)
        {
            Code = code;
            Message = message;
            Successful = successful;
            //ErrorException = errorException;
        }

        protected BacktoryResponse(int code, bool successful) : this(code, null, successful) { }

        public int Code { get; private set; }
        private string _message;
        public bool Successful { get; private set; }
        // not stable
        //internal Exception ErrorException { get; private set; }

        public string Message
        {
            get
            {
                return _message.IsEmpty() ? ((BacktoryHttpStatusCode)Code).ToString() : _message;
            }
            private set { _message = value; }
        }

        public static BacktoryResponse<TRANSFORMED> Error<RAW, TRANSFORMED>(BacktoryResponse<RAW> backtoryResponse)
            where TRANSFORMED : class
            where RAW : class
        {
            return new BacktoryResponse<TRANSFORMED>(backtoryResponse.Code, 
                backtoryResponse.Message,/*backtoryResponse.Message*/ null, false);
        }

        
    }

    public class BacktoryResponse<T> : BacktoryResponse where T : class
    {
        public BacktoryResponse(int code, string message, T body, bool successful) :
            base(code, message, successful)
        {
            Body = body;
        }

        public BacktoryResponse(int code, T body, bool successful) : this(code, null, body, successful) { }

        public T Body { get; private set; }


        public static BacktoryResponse<T> Error(int code, string message)
        {
            return new BacktoryResponse<T>(code, message, null, false);
        }

        public static BacktoryResponse<T> Error(int code)
        {
            return Error(code, null);
        }

        public static BacktoryResponse<T> Success(int code, T body)
        {
            return new BacktoryResponse<T>(code, body, true);
        }

        public static BacktoryResponse<T> Unknown()
        {
            return Unknown(null);
        }

        public static BacktoryResponse<T> Unknown(string message)
        {
            return new BacktoryResponse<T>((int)BacktoryHttpStatusCode.Unknown, message, null, false);
        }

    }
}