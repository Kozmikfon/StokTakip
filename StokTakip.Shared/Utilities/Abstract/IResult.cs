using StokTakip.Shared.Utilities.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Shared.Utilities.Abstract
{
    public interface IResult
    {
        public ResultStatus ResultStatus { get; }
        public string Info { get; }
        public Exception Exception { get; }
    }
}
