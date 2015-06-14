using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SonyCameraRemoteControl.Converter;

namespace SonyCameraRemoteControl
{
    public class DictResult : ResultBase<Dictionary<string, object>>
    {
        #region public methods
        public static DictResult Parse(string response)
        {
//            JavaScriptSerializer serializer = new JavaScriptSerializer();
//            serializer.RegisterConverters(new JavaScriptConverter[] { new DictionaryConverter<DictResult, Dictionary<string, object>>() });
//            return serializer.Deserialize<DictResult>(response);
			return ResultBase<Dictionary<string, object>>.Parse<DictResult>(response);
        }
        #endregion
    }
}
