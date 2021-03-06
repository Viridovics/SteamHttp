using System;
using System.Net;
using AirHttp.Responses.Interfaces;

namespace AirHttp.Responses
{
    internal class AirHttpResponse<T> : AirHttpResponse, IAirHttpResponse<T>
    {
        private T _value;

        internal static IAirHttpResponse<T> CreateFaultedResponseWithValue(Exception e)
        {
            return new AirHttpResponse<T>
            {
                Failed = true,
                FaultException = e
            };
        }

        internal static IAirHttpResponse<T> CreateSuccessfulResponseWithValue(HttpWebResponse httpWebRespons, T responseObject)
        {
            return new AirHttpResponse<T>
            {
                Failed = false,
                Value = responseObject,
                OriginalResponse = httpWebRespons
            };
        }

        public T Value
        {
            get
            {
                if (Failed)
                {
                    throw new InvalidOperationException("Response is failed. See details in FaultException", FaultException);
                }
                return _value;
            }
            private set
            {
                _value = value;
            }
        }
    }
}