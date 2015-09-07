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
	/// JSON converter for stirng arrays
	/// </summary>
	public class StringArrayConverter: ResponseConverter<StringsResult, string[]>
	{
		protected override void DeserializeValue(StringsResult result, JObject jsonObject, JsonSerializer serializer)
		{
			serializer.Populate(jsonObject.CreateReader(), result);
		}
	}
}
