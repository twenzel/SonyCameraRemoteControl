using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using SonyCameraRemoteControl.Converter;

namespace SonyCameraRemoteControl
{
    /// <summary>
    /// Base class for all results
    /// </summary>
    public abstract class ResultBase<T> : ResultBase
    {
        #region properties       
        public T Value { get; set; }
        #endregion

        #region public methods
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Error))
                return String.Format("Error {0}: {1}", ErrorCode, Error);
            else
                return Value.ToString();
        }
        #endregion

        #region protected methods      
        /// <summary>
        /// Parse and converts the resposne
        /// </summary>
        /// <param name="response">The response string</param>
        /// <returns></returns>
        public static TResult Parse<TResult>(string response) where TResult: ResultBase, new()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] { new ResponseConverter<TResult, T>() });
            return serializer.Deserialize<TResult>(response);
        }
        #endregion
    }

    /// <summary>
    /// Base class for all results
    /// </summary>
    public abstract class ResultBase
    {
        #region properties
        public string Error { get; set; }
        public Int32 ErrorCode { get; set; }
        public string Id { get; set; }

        public bool HasError
        {
            get { return !string.IsNullOrEmpty(Error);}
        }
        #endregion

        #region public methods
        public override string ToString()
        {
            if (HasError)
                return String.Format("Error {0}: {1}", ErrorCode, Error);
            else
                return base.ToString();
        }
        #endregion        
    }
}
