using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonyCameraRemoteControl
{
    /// <summary>
    /// Common result class for string values
    /// </summary>
    public class StringResult : ResultBase<string>
    {
        #region public methods
        public static StringResult Parse(string response)
        {
            return ResultBase<string>.Parse<StringResult>(response);
        }
        #endregion
    }
}
