using System;
using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace SonyCameraRemoteControl
{
	/// <summary>
	/// Client to retrieve the live preview stream
	/// </summary>
	public class StreamingClient
	{
		private bool _isActive = false;
		private object _lockObject = new object();
		private HttpWebRequest _webRequest;

		public event EventHandler<ImageArgs> ImageRetrieved;

		public StreamingClient ()
		{
		}

		public void Start(string url)
		{
			lock (_lockObject) {
				if (!_isActive) {
					_isActive = true;
					Task.Run (() => consumeStream(url));
				}
			}
		}

		public void Stop()
		{
			lock (_lockObject) {
				_isActive = false;
			}
		}

		#region private methods
		private void consumeStream(string url)
		{
			_webRequest = HttpWebRequest.CreateHttp (url);  
			_webRequest.Method = "GET";
			var response = _webRequest.GetResponseAsync().Result as HttpWebResponse;
			var stream = response.GetResponseStream ();

			var reader = new BinaryReader (stream);
			//var buffer = new char[8];
			//var count = reader.ReadBlock (buffer, 0, 8);
			var buffer = reader.ReadBytes(8);

			while (buffer.Length > 0) {
				// the first 8 byte contains
				// startbyte (1)
				// payloedType (1)
				// sequenceNumber (2)
				// timeStamp (4)

				if (isActive ()) {
					// we're only using the payloadType
					var payloadType = buffer [1];

					// read for JPEG image
					if (readPayload ((payloadType & 0x01) == 0x01, reader))
						buffer = reader.ReadBytes (8);
					else
						break;
				} else
					break;
			}
				
			//stream.Close ();
		}

		/// <summary>
		/// Get payload data of JPEG image
		/// </summary>
		/// <param name="isImage">If set to <c>true</c> is image.</param>
		private bool readPayload(bool isImage, BinaryReader reader)
		{
			int jpegDataSize = 0;
			int jpegPaddingSize = 0;

			// check for first 4 bytes
			if (detectPayloadHeader(reader))
			{
				// get JPEG data size
				//char[] jData = new char[3];
				//count = reader.ReadBlock (jData, 0, 3);
				var jData = reader.ReadBytes(3);
				if (jData.Length > 0)
				{
					jpegDataSize = bytesToInt (jData);

					// get JPEG padding size
					//char[] jPad = new char[1];
					//count = reader.ReadBlock (jPad, 0, 1);
					var jPad = reader.ReadBytes(1);
					if (jPad.Length > 0)
					{
						jpegPaddingSize = bytesToInt(jPad);

						// remove 120 bytes from stream
						//char[] b1 = new char[120];
						//count = reader.ReadBlock (b1, 0, 120);
						var b1 = reader.ReadBytes(120);

						if (b1.Length > 0)
						{
							// read JPEG image
							//byte[] jpegData =new byte[jpegDataSize];
							//count = reader.ReadBlock (jpegData, 0, jpegDataSize);
							var jpegData = reader.ReadBytes(jpegDataSize);

							if (jpegData.Length > 0)
							{
								if (isImage) {
//									NSData *imageData =
//										[[NSData alloc] initWithBytes:jpegData length:jpegDataSize];
//									UIImage *tempImage = [UIImage imageWithData:imageData];
//									dispatch_async(dispatch_get_main_queue(),
//										^{ [_viewDelegate didFetchImage:tempImage]; });

									if (ImageRetrieved != null)
										ImageRetrieved(this, new ImageArgs() {JpegData = jpegData});
								}

								// remove JPEG padding data
								//char[] padData = new char[jpegPaddingSize];
								//return reader.ReadBlock(padData, 0, jpegPaddingSize);
								return reader.ReadBytes(jpegPaddingSize).Length > 0;
							}
						}
					}
				}
			}

			return false;
		}

		private bool detectPayloadHeader(BinaryReader reader)
		{
			//char[] buffer = new char[4];

			int[] checkByte = new int[4];
			checkByte[0] = 0x24;
			checkByte[1] = 0x35;
			checkByte[2] = 0x68;
			checkByte[3] = 0x79;

			var buffer = reader.ReadBytes(4);
			while(buffer.Length > 0)
			{
				// check if the read bytes are the check bytes
				if (buffer [0] == checkByte [0]
				    && buffer [1] == checkByte [1]
				    && buffer [2] == checkByte [2]
				    && buffer [3] == checkByte [3])
					return true;
				else if (isActive ())
					// In case the data is corrupted and first 4 bytes are not checkBytes, this
					// loop will find the checkBytes.
					// NOTE : not used in general cases

					// read next 4 bytes
					buffer = reader.ReadBytes (4);
				else
					break;
			}
				
			return false;
		}

		private int bytesToInt(byte[] bytes)
		{
			int val = 0;
			for (int i = 0; i < bytes.Length; i++) {
				val = (val << 8) | (bytes[i] & 0xff);
			}
			return val;
		}

		private bool isActive()
		{
			lock (_lockObject) {
				return _isActive;
			}
		}
		#endregion
	}
}

public class ImageArgs : EventArgs
{
	public byte[] JpegData { get; set;}
}