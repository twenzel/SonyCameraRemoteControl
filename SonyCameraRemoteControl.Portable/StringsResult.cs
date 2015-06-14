using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonyCameraRemoteControl
{
    /// <summary>
    ///  Common result class for string array values
    /// </summary>
    public class StringsResult : ResultBase<string[]>
    {
        #region public methods
        public static StringsResult Parse(string response)
        {
            return ResultBase<string[]>.Parse<StringsResult>(response);
        }
        #endregion
    }
}
