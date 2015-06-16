using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SonyCameraRemoteControl.Converter;

namespace SonyCameraRemoteControl
{
    public class DictResult : ResultBase<Dictionary<string, object>>
    {
        #region public methods
        public static DictResult Parse(string response)
        {
            return JsonConvert.DeserializeObject<DictResult>(response, new DictionaryConverter<DictResult, Dictionary<string, object>>());
        }
        #endregion
    }
}
