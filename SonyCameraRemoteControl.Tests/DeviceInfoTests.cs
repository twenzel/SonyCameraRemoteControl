using System;
using NUnit.Framework;

namespace SonyCameraRemoteControl.Tests
{
	[TestFixture ()]
    public class DeviceInfoTests
    {
		[Test ()]
        public void ParseDescription()
        {
            var description = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root xmlns=\"urn:schemas-upnp-org:device-1-0\"><specVersion><major>1</major><minor>0</minor></specVersion><device><deviceType>urn:schemas-upnp-org:device:Basic:1</deviceType><friendlyName>DSC-WX300</friendlyName><manufacturer>Sony Corporation</manufacturer><manufacturerURL>http://www.sony.com/</manufacturerURL><modelDescription>SonyRemoteCamera</modelDescription><modelName>SonyImagingDevice</modelName><modelURL>http://www.sony.net/</modelURL><serialNumber></serialNumber><UDN>uuid:00000000-0005-0010-8000-1c994c0e8586</UDN><serviceList><service><serviceType>urn:schemas-sony-com:service:ScalarWebAPI:1</serviceType><serviceId>urn:schemas-sony-com:serviceId:ScalarWebAPI</serviceId><SCPDURL></SCPDURL><controlURL></controlURL><eventSubURL></eventSubURL></service></serviceList><av:X_ScalarWebAPI_DeviceInfo xmlns:av=\"urn:schemas-sony-com:av\"><av:X_ScalarWebAPI_Version>1.0</av:X_ScalarWebAPI_Version><av:X_ScalarWebAPI_ServiceList><av:X_ScalarWebAPI_Service><av:X_ScalarWebAPI_ServiceType>camera</av:X_ScalarWebAPI_ServiceType><av:X_ScalarWebAPI_ActionList_URL>http://10.0.0.1:10000/sony</av:X_ScalarWebAPI_ActionList_URL><av:X_ScalarWebAPI_AccessType /></av:X_ScalarWebAPI_Service></av:X_ScalarWebAPI_ServiceList></av:X_ScalarWebAPI_DeviceInfo></device></root>";

            var device = new SonyCameraDevice(new Uri("http://10.10.1.1"), description);

            Assert.AreEqual("DSC-WX300", device.FriendlyName);
            Assert.AreEqual("Sony Corporation", device.Manufacturer);
            Assert.AreEqual(new Uri("http://www.sony.com/"), device.ManufacturerUrl);
            Assert.AreEqual("SonyRemoteCamera", device.ModelDescription);
            Assert.AreEqual("SonyImagingDevice", device.ModelName);
            Assert.AreEqual(null, device.ModelNumber);
            Assert.AreEqual(new Uri("http://www.sony.net/"), device.ModelUrl);
            Assert.AreEqual(null, device.SerialNumber);
            Assert.AreEqual("uuid:00000000-0005-0010-8000-1c994c0e8586", device.Udn);
            Assert.AreEqual("00000000-0005-0010-8000-1c994c0e8586", device.Uuid);
            Assert.AreEqual(1, device.Endpoints.Count);
            Assert.AreEqual("http://10.0.0.1:10000/sony/camera", device.Endpoints["camera"]);
        }
    }
}
