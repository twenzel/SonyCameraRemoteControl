using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SonyCameraRemoteControl.Converter;
using Newtonsoft.Json;

namespace SonyCameraRemoteControl
{
    /// <summary>
    /// Base class for all results
    /// </summary>
	public abstract class ResultBase<T> : ResultBase
    {
        #region properties    
        [JsonProperty("result")]
        public T Value { get; set; }
        #endregion

        #region public methods
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(ErrorText))
                return String.Format("Error {0}: {1}", ErrorCode, ErrorText);
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
			return JsonConvert.DeserializeObject<TResult> (response, new ResponseConverter<TResult, T>());
        }
        #endregion
    }

    /// <summary>
    /// Base class for all results
    /// </summary>
    public abstract class ResultBase
    {
        #region properties        
        public string ErrorText { get; set; }
        public Int32 ErrorCode { get; set; }
        public string Id { get; set; }

        public bool HasError
        {
            get { return !string.IsNullOrEmpty(ErrorText);}
        }
        #endregion

        #region public methods
        public override string ToString()
        {
            if (HasError)
                return String.Format("Error {0}: {1}", ErrorCode, ErrorText);
            else
                return base.ToString();
        }
        #endregion        
    }
}
