using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SonyCameraRemoteControl.Converter
{
    /// <summary>
    /// Converter class for exting json serializer functionality
    /// </summary>
    public class ResponseConverter<TResult, TValue> : JsonConverter 
		where TResult : ResultBase, new()
    {            
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);

            TResult result = new TResult();            

            var errorProperty = jsonObject.Properties().FirstOrDefault(p => p.Name == "error");

            if (errorProperty != null && errorProperty.HasValues && errorProperty.Value.Type != JTokenType.Null)
            {
                result.ErrorCode = (int)errorProperty.Value.First;
                result.ErrorText = (string)errorProperty.Value.First.Next;                
            }

            DeserializeValue(result, jsonObject, serializer);

            return result;
        }

        protected virtual void DeserializeValue(TResult result, JObject jsonObject, JsonSerializer serializer)
        {
			//serializer.Populate(jsonObject.CreateReader(), result);
			result.Id = (string)jsonObject["id"];

			ResultBase<TValue> valueObject = result as ResultBase<TValue>;

			if (valueObject != null && jsonObject["result"] != null)
			{
				List<TValue> results = new List<TValue> ();

				serializer.Populate(jsonObject["result"].CreateReader(), results);

				if (results.Count > 0)
					valueObject.Value = results[0];
			}
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TResult) || objectType == typeof(ResultBase<>);
        }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }
    } 
}
