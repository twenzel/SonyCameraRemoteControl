using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SonyCameraRemoteControl.Converter;

namespace SonyCameraRemoteControl
{
    /// <summary>
    ///  Common result class for named values
    ///  <example>
    ///  {
    ///  "id": 1,
    ///     "result": [
    ///         {
    ///             "contShootingMode": "Spd Priority Cont.",
    ///             "candidate": [
    ///                 "Single",
    ///                 "Continuous",
    ///                 "Spd Priority Cont."
    ///                 ]
    ///         }
    ///     ]
    ///  }
    ///  </example>
    /// </summary>
    public class ValuesResult : ResultBase<Dictionary<string, object>>
    {
        #region public methods
        public static ValuesResult Parse(string response)
        {
            return JsonConvert.DeserializeObject<ValuesResult>(response, new MultipleObjectsConverter());
        }
        #endregion
    }
}
