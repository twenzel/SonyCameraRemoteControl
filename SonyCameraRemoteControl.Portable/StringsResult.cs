using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SonyCameraRemoteControl.Converter;

namespace SonyCameraRemoteControl
{
    /// <summary>
    ///  Common result class for string array values
	///  <example>
	///  {
	///  "id": 1,
	///     "result": [
	///         "Smart Remote Control",
	/// 		"1.0.0"
	///     ]
	///  }
	///  </example>
	/// </summary>
    public class StringsResult : ResultBase<string[]>
    {
        #region public methods
        public static StringsResult Parse(string response)
        {
			return JsonConvert.DeserializeObject<StringsResult>(response, new StringArrayConverter());
        }
        #endregion
    }
}
