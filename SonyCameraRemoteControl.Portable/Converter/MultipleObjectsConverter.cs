using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SonyCameraRemoteControl.Converter
{    
    /// <summary>
    /// JSON converter for multi level objects
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class MultipleObjectsConverter : ResponseConverter<ValuesResult, Dictionary<string, object>>
    {
        protected override void DeserializeValue(ValuesResult result, JObject jsonObject, JsonSerializer serializer)
        {
            result.Id = (string)jsonObject["id"];

            ValuesResult valueObject = result as ValuesResult;

			if (valueObject != null && jsonObject["result"] != null)
            {                
                JsonReader reader = jsonObject["result"].CreateReader();

                object val = ReadChild(reader);
                List<object> listResult = val as List<object>;

                if (listResult != null)
                    valueObject.Value = (Dictionary<string, object>)listResult[0]; 
                else
                    valueObject.Value = (Dictionary<string, object>)val;                        
            }
        }

        private object ReadChild(JsonReader reader)
        {           
            object result = null;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndArray || reader.TokenType == JsonToken.EndObject)
                    break;
                else if (reader.TokenType == JsonToken.StartArray)
                {
                    return ReadList(reader);
                }
                else if (reader.TokenType == JsonToken.StartObject)
                {                   
                    return ReadDictionary(reader);                  
                }                                                                                                           
                else
                {                    
                    return reader.Value;
                }
            }

           return result;
        }

        private List<object> ReadList(JsonReader reader)
        {
            List<object> listResult = new List<object>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndArray || reader.TokenType == JsonToken.EndObject)
                    break;
                else if (reader.TokenType == JsonToken.StartArray)
                {
                    listResult.Add(ReadList(reader));
                }
                else if (reader.TokenType == JsonToken.StartObject)
                {
                    listResult.Add(ReadDictionary(reader));
                }                
                else
                {
                    listResult.Add(reader.Value);
                }
            }

            return listResult;
        }

        private Dictionary<string, object> ReadDictionary(JsonReader reader)
        {            
            Dictionary<string, object> dictResult =  new Dictionary<string, object>(); 

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndArray || reader.TokenType == JsonToken.EndObject)
                    break;                
                else if (reader.TokenType == JsonToken.PropertyName) // dictionary entry
                {
                    string readerValue = reader.Value.ToString();
                    dictResult.Add(readerValue, ReadChild(reader));
                }
            }

            return dictResult;
        }
    }
}
