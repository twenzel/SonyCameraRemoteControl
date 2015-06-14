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

using various AOI methods:
```csharp
var result = await _device.StartLiveView();

var result = await _device.TakePhoto();
```



More information about Sony Camera Remote API:

https://developer.sony.com/2013/11/29/how-to-develop-an-app-using-the-camera-remote-api-2/

