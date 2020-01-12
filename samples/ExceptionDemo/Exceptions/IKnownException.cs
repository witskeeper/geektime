using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExceptionDemo.Exceptions
{
    public interface IKnownException
    {
        public string Message { get; }

        public int ErrorCode { get; }

        public object[] ErrorData { get; }
    }
}
