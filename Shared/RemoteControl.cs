using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rssdp;

namespace SonyCameraRemoteControl
{
	/// <summary>
	/// Helper class to get started finding the device
	/// </summary>
	public class RemoteControl
	{
		/// <summary>
		/// Tries to find the device by uuid (async)
		/// </summary>
		/// <param name="uuid">The uuid of the device</param>
		/// <returns></returns>
		public static async Task<SonyCameraDevice> FindDeviceAsync(string uuid)
		{
			using (var deviceLocator = new SsdpDeviceLocator())
			{
				var results = await deviceLocator.SearchAsync("uuid:" + uuid).ConfigureAwait(false);

				if (results.Any())
				{
					return await SonyCameraDevice.GetSonyDevice(results.FirstOrDefault()).ConfigureAwait(false);
				} else
				{
					return null;
				}

			}
		}

		/// <summary>
		/// Tries to find the device by uuid
		/// </summary>
		/// <param name="uuid">The uuid of the device</param>
		/// <returns></returns>
		public static SonyCameraDevice FindDevice(string uuid, int timoutMilliseconds = 5000)
		{
			var task = FindDeviceAsync(uuid);

			// run synchrously
			var result = task.Result;

			if (task.IsCompleted && !task.IsFaulted)
				return task.Result;
			else if (task.Exception != null)
				throw task.Exception;
			else
				return null;
		}

		/// <summary>
		/// Searches for first sony camera (max.)
		/// </summary>
		/// <param name="timoutMilliseconds">Amount of milliseconds to wait for results.</param>
		/// <returns></returns>
		public static SonyCameraDevice SearchDevice(int timoutMilliseconds = 5000)
		{
			var _BroadcastListener = new SsdpDeviceLocator();
			SonyCameraDevice result = null;

			AutoResetEvent signal = new AutoResetEvent(false);
			EventHandler<DeviceAvailableEventArgs> handler = null;

			handler = async (s, e) =>
			{
				if (e.IsNewlyDiscovered)
				{
					result = await SonyCameraDevice.GetSonyDevice(e.DiscoveredDevice).ConfigureAwait(false);                    
					signal.Set();

				}
			};
			_BroadcastListener.DeviceAvailable += handler;

			Task.Run( ()=> {                
				_BroadcastListener.StartListeningForNotifications();
			});

			signal.WaitOne(timoutMilliseconds);

			_BroadcastListener.StopListeningForNotifications();
			_BroadcastListener.DeviceAvailable -= handler;

			return result;
		}

	}
}
