# SonyCameraRemoteControl
.NET implementation of the Sony Camera Remote API.

Implements the SSDP (using RSSDP) discovery of the devices and the API method calling.

Should be also usable for iOS and Android (using Xamarin).

Not all API methods are wrapped but you can use the base methods to do this.

Connect to a camera via Uuid:
```csharp
SonyCameryDevice device = RemoteControl.FindDevice("00000000-0005-0010-8000-1c994c0e8586"); 
```

Search for a device:
```csharp
SonyCameryDevice device = RemoteControl.SearchDevice();
```

Initialize the device (retrieving API version and available methods) [Optional]
```csharp
var initResult = await _device.Initialize();
```


using various AOI methods:
```csharp
var result = await _device.StartLiveView();

var result = await _device.TakePhoto();
```

Retrieving the live preview stream:
```csharp
_streamingClient = new StreamingClient ();
_streamingClient.ImageRetrieved += (s, args) => showImageData (args.JpegData);
_streamingClient.Error += (sender, e) => showError("Streaming error - " + e.ErrorMessage);

// start and show live preview
var result = await _device.StartLiveView ();
if (!result.HasError)
_streamingClient.Start (result.Value);


// stop preview
if (_streamingClient != null) {
	_streamingClient.Stop ();

	Result =  _device.StopLiveView ();
} 
```


More information about Sony Camera Remote API:

https://developer.sony.com/2013/11/29/how-to-develop-an-app-using-the-camera-remote-api-2/

