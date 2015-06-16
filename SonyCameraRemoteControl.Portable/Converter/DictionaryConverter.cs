using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SonyCameraRemoteControl.Converter
{
    /// <summary>
    /// JSON converter for dictionary results
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class DictionaryConverter<TResult, TValue> : ResponseConverter<TResult, TValue>
        where TResult : ResultBase, new()
        where TValue : new()
    {
        protected override void DeserializeValue(TResult result, JObject jsonObject, JsonSerializer serializer)
        {
            result.Id = (string)jsonObject["id"];
            
            ResultBase<TValue> valueObject = result as ResultBase<TValue>;

            if (valueObject != null)
            {
                TValue value = new TValue();

                serializer.Populate(jsonObject["result"].First.CreateReader(), value);

                valueObject.Value = value;
            }
        }
    }
}
