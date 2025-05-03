using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tixxp.Core.Utilities.Results.Concrete;

public class ErrorDataResult<T> : DataResult<T>
{
    public ErrorDataResult(T data, string message) : base(data, false, message) { }
    public ErrorDataResult(string message) : base(default, false, message) { }
    public ErrorDataResult() : base(default, false) { }
}
