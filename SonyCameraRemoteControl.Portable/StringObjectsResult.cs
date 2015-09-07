using System;

namespace SonyCameraRemoteControl
{
	/// <summary>
	/// Common result class for nested string values
	/// <example>>
	/// {
	///  "id": 1,
	///     "result": [
	///         ["Smart Remote Control","1.0.0"]
	///     ]
	///  }
	/// </example>
	/// </summary>
	public class StringObjectsResult : ResultBase<string[]>
	{
		#region public methods
		public static StringObjectsResult Parse(string response)
		{
			return ResultBase<string[]>.Parse<StringObjectsResult>(response);
		}
		#endregion
	}
}

