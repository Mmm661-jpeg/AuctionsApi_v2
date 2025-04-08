using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionsApi_v2.Domain.UtilityModels
{
    public class OperationResult
    {
        public bool IsSuccces { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public OperationResult(bool isSuccess, string message, object data)
        {
            IsSuccces = isSuccess;
            Message = message;
            Data = data;
        }

        public static OperationResult Success(object data = null, string message = "Operation successfull")
        {
            return new OperationResult(true, message, data);
        }

        public static OperationResult Failure(string message = "Operation failed!")
        {
            return new OperationResult(false, message, null);
        }
    }
}
