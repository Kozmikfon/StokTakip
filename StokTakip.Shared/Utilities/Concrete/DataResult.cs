using StokTakip.Shared.Utilities.Abstract;
using StokTakip.Shared.Utilities.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StokTakip.Shared.Utilities.Concrete
{
    public class DataResult<T> : IDataResult<T>
    {
        public DataResult(ResultStatus resultStatus,T data)
        {
            ResultStatus = resultStatus;
            Data = data;
        }
        public DataResult(ResultStatus resultStatus, string info, T data)
        {
            ResultStatus = resultStatus;
            Info = info;
            Data = data;            
        }
        public DataResult(ResultStatus resultStatus, string info, T data, Exception exception)
        {
            ResultStatus = resultStatus;
            Info = info;
            Data = data;
            Exception = exception;
        }

        public T Data { get; }

        public ResultStatus ResultStatus { get; }

        public string Info { get; }

        public Exception Exception { get; }

        //new DataResult<DepoDTO>(ResultStatus.Error,"",Data);


        
    }
}
