using System;

namespace SonyCameraRemoteControl
{
	/// <summary>
	/// Result class for empty results
	/// </summary>
	public class NoResult : ResultBase<object>
	{
		#region public methods
		public static NoResult Parse(string response)
		{
			return ResultBase<object>.Parse<NoResult>(response);
		}
		#endregion
	}
}

