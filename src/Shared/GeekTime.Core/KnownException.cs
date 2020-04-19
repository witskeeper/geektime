using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekTime
{
    public class KnownException : IKnownException
    {
        public string Message { get; private set; }

        public int ErrorCode { get; private set; }

        public object[] ErrorData { get; private set; }

        public readonly static IKnownException Unknown = new KnownException { Message = "未知错误", ErrorCode = 9999 };

        public static IKnownException FromKnownException(IKnownException exception)
        {
            return new KnownException { Message = exception.Message, ErrorCode = exception.ErrorCode, ErrorData = exception.ErrorData };
        }
    }
}
