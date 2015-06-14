using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonyCameraRemoteControl.Converter
{   
    /// <summary>
    /// Converter class for exting JS serializer functionality
    /// </summary>
//    public class ResponseConverter<TResult, TValue> : JavaScriptConverter where TResult : ResultBase, new()
//    {
//        public override IEnumerable<Type> SupportedTypes
//        {
//            //Define the ListItemCollection as a supported type. 
//            get { return new ReadOnlyCollection<Type>(new List<Type>(new Type[] { typeof(TResult), typeof(ResultBase<>) })); }
//        }
//
//        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
//        {
//            throw new NotImplementedException();
//        }
//
//        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
//        {
//            if (dictionary == null)
//                throw new ArgumentNullException("dictionary");
//
//            TResult result = new TResult();
//
//            if (dictionary.ContainsKey("id"))
//                result.Id = dictionary["id"].ToString();
//
//            if (dictionary.ContainsKey("error"))
//            {                
//                ArrayList errorInfo = dictionary["error"] as ArrayList;
//
//                if (errorInfo != null)
//                {
//                    if (errorInfo.Count == 2)
//                    {
//                        result.ErrorCode = (int)errorInfo[0];
//                        result.Error = (string)errorInfo[1];
//                    }
//                    else if (errorInfo.Count == 1)
//                    {
//                        result.Error = Convert.ToString(errorInfo[0]);
//                    }
//                }                
//            }
//
//                                          
//            if (dictionary.ContainsKey("result"))
//            {
//
//                if (DeserializeValue(result, dictionary["result"], serializer))
//                {
//                    ResultBase<TValue> t = result as ResultBase<TValue>;
//
//                    if (t != null)
//                        t.Value = serializer.ConvertToType<TValue>(dictionary["result"]);
//                }
//            }
//            
//            return result;
//        }
//
//        protected virtual bool DeserializeValue(TResult result, object value, JavaScriptSerializer serializer)
//        {
//            return true;
//        }
//    }
}
