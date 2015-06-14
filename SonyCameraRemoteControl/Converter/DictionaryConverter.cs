using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SonyCameraRemoteControl.Converter
{
    /// <summary>
    /// JSON converter for dictionary results
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class DictionaryConverter<TResult, TValue> : ResponseConverter<TResult, TValue> where TResult : ResultBase, new()
    {
        protected override bool DeserializeValue(TResult result, object value, JavaScriptSerializer serializer)
        {
            ResultBase<TValue> t = result as ResultBase<TValue>;

            if (t != null)
            {
                ArrayList list = (ArrayList)value;
                t.Value = serializer.ConvertToType<TValue>(list[0]);

                return false;
            }

            return base.DeserializeValue(result, value,serializer);
        }
    }
}
