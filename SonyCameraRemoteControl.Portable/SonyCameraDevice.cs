using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Rssdp;
using Rssdp.Infrastructure;

namespace SonyCameraRemoteControl
{
    /// <summary>
    /// The camera device info class
    /// </summary>
    public class SonyCameraDevice
    {
        private const string sony_ns = "{urn:schemas-sony-com:av}";
        private const string upnp_ns = "{urn:schemas-upnp-org:device-1-0}";
        private static HttpClient s_DefaultHttpClient;

        private string _Udn;

        /// <summary>
		/// Deserialisation constructor.
		/// </summary>
		/// <param name="location">The url from which the device description document was retrieved.</param>
		/// <param name="deviceDescriptionXml">The device description XML as a string.</param>
		/// <exception cref="System.ArgumentNullException">Thrown if the <paramref name="deviceDescriptionXml"/> or <paramref name="location"/> arguments are null.</exception>
		/// <exception cref="System.ArgumentException">Thrown if the <paramref name="deviceDescriptionXml"/> argument is empty.</exception>
        public SonyCameraDevice(Uri location, string deviceDescriptionXml)            
		{
            if (deviceDescriptionXml == null) throw new ArgumentNullException("deviceDescriptionXml");
            if (deviceDescriptionXml.Length == 0) throw new ArgumentException("deviceDescriptionXml cannot be an empty string.", "deviceDescriptionXml");

            this.Location = location;
            LoadFromDescriptionDocument(deviceDescriptionXml);            
		}


        #region public methods
        /// <summary>
        /// Gets the camery device out of the ssdp device
        /// </summary>
        /// <param name="discoveredDevice"></param>
        /// <returns></returns>
        public async static Task<SonyCameraDevice> GetSonyDevice(DiscoveredSsdpDevice discoveredDevice)
        {
            var rawDescriptionDocument = await GetDefaultClient().GetAsync(discoveredDevice.DescriptionLocation).ConfigureAwait(false);
            rawDescriptionDocument.EnsureSuccessStatusCode();

            // Not using ReadAsStringAsync() here as some devices return the content type as utf-8 not UTF-8,
            // which causes an (unneccesary) exception.
            var data = await rawDescriptionDocument.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

            return new SonyCameraDevice(discoveredDevice.DescriptionLocation, System.Text.UTF8Encoding.UTF8.GetString(data, 0, data.Length));
        }

        /// <summary>
        /// Sends a little hello
        /// </summary>
        /// <returns></returns>
        public async Task<StringResult> HelloAsync()
        {
            var result = await SendRequestAsync(Endpoints["camera"], "echo", "Hello camera");

            return StringResult.Parse(result);
        }

        /// <summary>
        /// Gets a list of available apis
        /// </summary>
        /// <returns></returns>
        public async Task<StringsResult> GetAvailableAPIList()
        {
            CheckCameraEndpoint();

            var result = await SendRequestAsync(Endpoints["camera"], "getAvailableApiList");

            return StringsResult.Parse(result);
        }

        /// <summary>
        /// Gets a list of available apis
        /// </summary>
        /// <returns></returns>
        public async Task<StringsResult> GetApplicationInfo()
        {
            CheckCameraEndpoint();

            var result = await SendRequestAsync(Endpoints["camera"], "getApplicationInfo");

            return StringsResult.Parse(result);
        }

        /// <summary>
        /// Gets current camera shooting mode
        /// </summary>
        /// <returns></returns>
        public async Task<StringResult> GetShootMode()
        {
            CheckCameraEndpoint();

            var result = await SendRequestAsync(Endpoints["camera"], "getShootMode");

            return StringResult.Parse(result);
        }

        /// <summary>
        /// This API provides a function to get the supported shoot modes.
        /// The client should use "getAvailableShootMode" to get the available parameters at the moment.
        /// </summary>
        /// <returns>A list of supported shoot modes</returns>
        public async Task<StringsResult> GetSupportedShootMode()
        {
            CheckCameraEndpoint();

            var result = await SendRequestAsync(Endpoints["camera"], "getSupportedShootMode");

            return StringsResult.Parse(result);
        }

        /// <summary>
        /// This API provides a function to get current shoot mode and the available shoot modes at the moment.
        /// The available parameters can be changed by user operations and calling APIs.
        /// </summary>
        /// <returns>0:Current shoot mode, 1:A list of available shoot modes</returns>
        public async Task<ValuesResult> GetAvailableShootMode()
        {
            CheckCameraEndpoint();

            var result = await SendRequestAsync(Endpoints["camera"], "getAvailableShootMode");

            return ValuesResult.Parse(result);
        }

        /// <summary>
        /// Set a value of shooting mode.
        /// </summary>
        /// <returns></returns>
        public async Task<StringResult> SetShootMode(string mode)
        {
            CheckCameraEndpoint();

            var result = await SendRequestAsync(Endpoints["camera"], "setShootMode", mode);

            return StringResult.Parse(result);
        }

        /// <summary>
        /// This API provides a function to set up camera for shooting function. Some camera models need this
        /// API call before starting liveview, capturing still image, recording movie, or accessing all other camera
        /// shooting functions.
        /// </summary>
        /// <returns></returns>
        public async Task<StringResult> StartRecMode()
        {
            CheckCameraEndpoint();

            var result = await SendRequestAsync(Endpoints["camera"], "startRecMode");

            return StringResult.Parse(result);
        }

        /// <summary>
        /// This API provides a function to stop shooting functions.
        /// </summary>
        /// <returns></returns>
        public async Task<StringResult> StopRecMode()
        {
            CheckCameraEndpoint();

            var result = await SendRequestAsync(Endpoints["camera"], "stopRecMode");

            return StringResult.Parse(result);
        }

        /// <summary>
        /// This API provides a function to start liveview.
        /// </summary>
        /// <returns>URL of liveview</returns>
        public async Task<StringResult> StartLiveView()
        {
            CheckCameraEndpoint();

            var result = await SendRequestAsync(Endpoints["camera"], "startLiveview");

            return StringResult.Parse(result);
        }

        /// <summary>
        /// This API provides a function to start liveview.
        /// </summary>
        /// <param name="size">Liveview size. e.g. "M"</param>
        /// <returns>URL of liveview</returns>
        public async Task<StringResult> StartLiveViewWithSize(string size)
        {
            CheckCameraEndpoint();

            var result = await SendRequestAsync(Endpoints["camera"], "startLiveviewWithSize", size);

            return StringResult.Parse(result);
        }

        /// <summary>
        /// This API provides a function to stop liveview.
        /// </summary>
        /// <returns>0 for success</returns>
        public async Task<StringResult> StopLiveView()
        {
            CheckCameraEndpoint();

            var result = await SendRequestAsync(Endpoints["camera"], "stopLiveview");

            return StringResult.Parse(result);
        }

        /// <summary>
        /// This API provides a function to get current liveview size.
        /// </summary>
        /// <returns>Current liveview size</returns>
        public async Task<StringResult> GetLiveviewSize()
        {
            CheckCameraEndpoint();

            var result = await SendRequestAsync(Endpoints["camera"], "getLiveviewSize");

            return StringResult.Parse(result);
        }

        /// <summary>
        /// This API provides a function to get the supported liveview sizes.
        /// The client should use "getAvailableLiveviewSize" to get the available parameters at the moment.
        /// </summary>
        /// <returns>A list of supported liveview sizes</returns>
        public async Task<StringsResult> GetSupportedLiveviewSize()
        {
            CheckCameraEndpoint();

            var result = await SendRequestAsync(Endpoints["camera"], "getSupportedLiveviewSize");

            return StringsResult.Parse(result);
        }

        /// <summary>
        /// This API provides a function to get current liveview size and the available liveview sizes at the moment.
        /// The available parameters can be changed by user operations and calling APIs.
        /// </summary>
        /// <returns>0 - Current liveview size, 1:A list of available liveview sizes</returns>
        public async Task<ValuesResult> GetAvailableLiveviewSize()
        {
            CheckCameraEndpoint();

            var result = await SendRequestAsync(Endpoints["camera"], "getAvailableLiveviewSize");

            return ValuesResult.Parse(result);
        }

        /// <summary>
        /// This API provides a function to switch the liveview frame information transferring. The liveview frame
        /// information includes focus frames, face detection frames and tracking frames on the liveview.
        /// </summary>
        /// <param name="frameInfo">true - Transfer the liveview frame information, false - Not transfer</param>
        /// <returns>A list of supported liveview sizes</returns>
        public async Task<StringResult> SetLiveviewFrameInfo(bool frameInfo)
        {
            CheckCameraEndpoint();

            var result = await SendRequestAsync(Endpoints["camera"], "setLiveviewFrameInfo", new KeyValuePair<string, object>("frameInfo", frameInfo));

            return StringResult.Parse(result);
        }

        /// <summary>
        /// This API provides a function to get current setting of the liveview frame information transferring.
        /// </summary>
        /// <returns></returns>
        public async Task<DictResult> GetLiveviewFrameInfo()
        {
            CheckCameraEndpoint();

            var result = await SendRequestAsync(Endpoints["camera"], "getLiveviewFrameInfo");

            return DictResult.Parse(result);
        }

        /// <summary>
        /// This API provides a function to take picture.
        /// </summary>
        /// <returns>Array of URLs of postview.
        /// The postview is captured image data by camera. The postview
        /// image can be used for storing it as the taken picture, and
        /// showing it to the client display.</returns>
        public async Task<StringsResult> TakePicture()
        {
            CheckCameraEndpoint();

            var result = await SendRequestAsync(Endpoints["camera"], "actTakePicture");

            return StringsResult.Parse(result);
        }

        /// <summary>
        /// This API provides a function to wait while the camera is taking the picture.
        /// </summary>
        /// <returns>Array of URLs of postview.
        /// The postview is captured image data by camera. The postview
        /// image can be used for storing it as the taken picture, and
        /// showing it to the client display.</returns>
        public async Task<StringsResult> AwaitTakePicture()
        {
            CheckCameraEndpoint();

            var result = await SendRequestAsync(Endpoints["camera"], "awaitTakePicture");

            return StringsResult.Parse(result);
        }

        /// <summary>
        /// This API provides a function to zoom.
        /// </summary>
        /// <param name="direction">"in" or "out"</param>
        /// <param name="movement">"start" = Long push, "stop" = stop, "1shot" = short push</param>
        /// <returns>0 = success</returns>
        public async Task<StringResult> Zoom(string direction, string movement)
        {
            CheckCameraEndpoint();

            var result = await SendRequestAsync(Endpoints["camera"], "actZoom", new string[]{direction, movement});

            return StringResult.Parse(result);
        }

        /// <summary>
        /// This API provides a function to zoom.
        /// </summary>
        /// <param name="movement">"start" = Long push, "stop" = stop, "1shot" = short push</param>
        /// <returns>0 = success</returns>
        public async Task<StringResult> ZoomIn(string movement = "start")
        {
            return await Zoom("in", movement);
        }

        /// <summary>
        /// This API provides a function to zoom.
        /// </summary>
        /// <param name="movement">"start" = Long push, "stop" = stop, "1shot" = short push</param>
        /// <returns>0 = success</returns>
        public async Task<StringResult> ZoomInStep(string movement = "1shot")
        {
            return await Zoom("in", movement);
        }

        /// <summary>
        /// This API provides a function to zoom.
        /// </summary>
        /// <param name="movement">"start" = Long push, "stop" = stop, "1shot" = short push</param>
        /// <returns>0 = success</returns>
        public async Task<StringResult> ZoomOut(string movement = "start")
        {
            return await Zoom("out", movement);
        }

        /// <summary>
        /// This API provides a function to zoom.
        /// </summary>
        /// <param name="movement">"start" = Long push, "stop" = stop, "1shot" = short push</param>
        /// <returns>0 = success</returns>
        public async Task<StringResult> ZoomOutStep(string movement = "1shot")
        {
            return await Zoom("out", movement);
        }

        /// <summary>
        /// This API provides a function to get event from the server.
        /// </summary>
        /// <param name="longPolling">true: Callback when timeout or change point detection. false: Callback immediately.</param>
        /// <returns>Changed values</returns>
        public async Task<ValuesResult> GetEvent(bool longPolling)
        {
            CheckCameraEndpoint();

            var result = await SendRequestAsync(Endpoints["camera"], "getEvent", longPolling.ToString());

            return ValuesResult.Parse(result);
        }

        /// <summary>
        /// This API provides a function to set a value of camera function.
        /// </summary>
        /// <param name="camerafunction">Camera function</param>
        /// <returns>0= success</returns>
        public async Task<StringResult> SetCameraFunction(string camerafunction)
        {
            CheckCameraEndpoint();

            var result = await SendRequestAsync(Endpoints["camera"], "setCameraFunction", camerafunction);

            return StringResult.Parse(result);
        }

        /// <summary>
        /// Sends a request with empty parameters
        /// </summary>
        /// <param name="url">The service url</param>
        /// <param name="method">The api method</param>
        /// <returns></returns>
        public async static Task<string> SendRequestAsync(string url, string method)
        {
            return await SendRequestAsync(url, method, new string[] { });
        }

        /// <summary>
        /// Sends a request with a single parameter
        /// </summary>
        /// <param name="url">The service url</param>
        /// <param name="method">The api method</param>
        /// <param name="parameter">a single parameter</param>
        /// <returns></returns>
        public async static Task<string> SendRequestAsync(string url, string method, string parameter)
        {
            return await SendRequestAsync(url, method, new string[]{parameter});
        }

        /// <summary>
        /// Sends a request with parameters
        /// </summary>
        /// <param name="url">The service url</param>
        /// <param name="method">The api method</param>
        /// <param name="parameters">The method parameters</param>
        /// <returns></returns>
        public async static Task<string> SendRequestAsync(string url, string method, string[] parameters)
        {
            var responseMessage = await GetDefaultClient().PostAsync(url, CreateContent(method, parameters));
            responseMessage.EnsureSuccessStatusCode();

            // Not using ReadAsStringAsync() here as some devices return the content type as utf-8 not UTF-8,
            // which causes an (unneccesary) exception.
            var data = await responseMessage.Content.ReadAsByteArrayAsync();
            return System.Text.UTF8Encoding.UTF8.GetString(data, 0, data.Length);
        }

         /// <summary>
        /// Sends a request with parameters
        /// </summary>
        /// <param name="url">The service url</param>
        /// <param name="method">The api method</param>
        /// <param name="info">The method parameters</param>
        /// <returns></returns>
        public async static Task<string> SendRequestAsync(string url, string method, KeyValuePair<string, object> info)
        {
            return await SendRequestAsync(url, method, new KeyValuePair<string, object>[] { info });
        }

        /// <summary>
        /// Sends a request with parameters
        /// </summary>
        /// <param name="url">The service url</param>
        /// <param name="method">The api method</param>
        /// <param name="infos">The method parameters</param>
        /// <returns></returns>
        public async static Task<string> SendRequestAsync(string url, string method, IEnumerable<KeyValuePair<string, object>> infos)
        {
            var responseMessage = await GetDefaultClient().PostAsync(url, CreateContent(method, infos));
            responseMessage.EnsureSuccessStatusCode();

            // Not using ReadAsStringAsync() here as some devices return the content type as utf-8 not UTF-8,
            // which causes an (unneccesary) exception.
            var data = await responseMessage.Content.ReadAsByteArrayAsync();
            return System.Text.UTF8Encoding.UTF8.GetString(data, 0, data.Length);
        }

        /// <summary>
        /// Determines whether a camera endpoint exists on the device
        /// </summary>
        /// <returns></returns>
        public bool HasCameraEndpoint()
        {
            return Endpoints.ContainsKey("camera");
        }
        #endregion

        #region Private Methods

        private void LoadFromDescriptionDocument(string deviceDescriptionXml)
        {
            Endpoints = new Dictionary<string, string>();

            var xml = XDocument.Parse(deviceDescriptionXml);
            var device = xml.Root.Element(upnp_ns + "device");
            if (device != null)
            {
                FriendlyName = ReadValue(device, upnp_ns + "friendlyName");
                ModelName = ReadValue(device, upnp_ns + "modelName"); 
                Manufacturer = ReadValue(device, upnp_ns + "manufacturer");
                ManufacturerUrl = ReadUri(device, upnp_ns + "manufacturerURL");
                ModelDescription = ReadValue(device, upnp_ns + "modelDescription");
                ModelNumber = ReadValue(device, upnp_ns + "modelNumber");
                ModelUrl = ReadUri(device, upnp_ns + "modelURL");
                Udn = ReadValue(device, upnp_ns + "UDN");
                SetUuidFromUdn();

                var info = device.Element(sony_ns + "X_ScalarWebAPI_DeviceInfo");
                if (info != null)
                {
                    var list = info.Element(sony_ns + "X_ScalarWebAPI_ServiceList");

                    foreach (var service in list.Elements())
                    {
                        var name = service.Element(sony_ns + "X_ScalarWebAPI_ServiceType").Value;
                        var url = service.Element(sony_ns + "X_ScalarWebAPI_ActionList_URL").Value;
                        if (name == null || url == null)
                            continue;

                        string endpoint;
                        if (url.EndsWith("/"))
                            endpoint = url + name;
                        else
                            endpoint = url + "/" + name;

                        Endpoints.Add(name, endpoint);
                    }


                    if (Endpoints.Count == 0)
                    {
                        throw new XmlException("No endoint found in XML");
                    }
                }
            }
        }

        private string ReadValue(XElement element, string name)
        {
            var valueElement = element.Element(name);

            if (valueElement != null)
                return valueElement.Value;
            else
                return null;
        }

        private Uri ReadUri(XElement element, string name)
        {
            var valueElement = element.Element(name);

            if (valueElement != null)
                return new Uri(valueElement.Value);
            else
                return null;
        }

        private void SetUuidFromUdn()
        {
            if (Udn != null && Udn.StartsWith("uuid:", StringComparison.OrdinalIgnoreCase))
                Uuid = Udn.Substring(5).Trim();
            else
                Uuid = Udn;
        }

        private void CheckCameraEndpoint()
        {
            if (!HasCameraEndpoint())
                throw new InvalidOperationException("No 'camera' endpoint found.");
        }

        private static HttpClient GetDefaultClient()
        {
            if (s_DefaultHttpClient == null)
            {
                var handler = new System.Net.Http.HttpClientHandler();
                try
                {
                    if (handler.SupportsAutomaticDecompression)
                        handler.AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip;

                    s_DefaultHttpClient = new HttpClient(handler);
                }
                catch
                {
                    if (handler != null)
                        handler.Dispose();

                    throw;
                }
            }

            return s_DefaultHttpClient;
        }        

        private static StringContent CreateContent(string method, string[] parameters, int id = 1, string version="1.0")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine(string.Format("\"method\": \"{0}\",", method));

            string parameterValues = "";

            foreach (var value in parameters)
	        {
		        if (parameterValues.Length > 0)
                    parameterValues += ",";

                parameterValues += string.Format("\"{0}\"", value);
	        }                

            sb.AppendLine(string.Format("\"params\": [{0}],", parameterValues));
            sb.AppendLine(string.Format("\"id\":{0}, \"version\": \"{1}\"}}", id, version));

            return new System.Net.Http.StringContent(sb.ToString());
        }

        private static StringContent CreateContent(string method, IEnumerable<KeyValuePair<string, object>> parameters, int id = 1, string version = "1.0")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine(string.Format("\"method\": \"{0}\",", method));

            string parameterValues = "";

            foreach (var item in parameters)
            {
                if (parameterValues.Length > 0)
                    parameterValues += ",";

                parameterValues += string.Format("\"{0}\": {1}", item.Key, item.Value);
            }

            sb.AppendLine(string.Format("\"params\": [{{{0}}}],", parameterValues));
            sb.AppendLine(string.Format("\"id\":{0}, \"version\": \"{1}\"}}", id, version));

            return new System.Net.Http.StringContent(sb.ToString());
        }
        #endregion

        #region properties
        /// <summary>
        /// K-V pairs of service name and its endpoint URL
        /// </summary>
        public Dictionary<string, string> Endpoints { get; set; }

        /// <summary>
        /// Sets or returns the universally unique identifier for this device (without the uuid: prefix). Required.
        /// </summary>
        /// <remarks>
        /// <para>Must be the same over time for a specific device instance (i.e. must survive reboots).</para>
        /// <para>For UPnP 1.0 this can be any unique string. For UPnP 1.1 this should be a 128 bit number formatted in a specific way, preferably generated using the time and MAC based algorithm. See section 1.1.4 of http://upnp.org/specs/arch/UPnP-arch-DeviceArchitecture-v1.1.pdf for details.</para>
        /// <para>Technically this library implements UPnP 1.0, so any value is allowed, but we advise using UPnP 1.1 compatible values for good behaviour and forward compatibility with future versions.</para>
        /// </remarks>
        public string Uuid { get; set; }

        /// <summary>
        /// Returns (or sets*) a unique device name for this device. Optional, not recommended to be explicitly set.
        /// </summary>
        /// <remarks>
        /// <para>* In general you should not explicitly set this property. If it is not set (or set to null/empty string) the property will return a UDN value that is correct as per the UPnP specification, based on the other device properties.</para>
        /// <para>The setter is provided to allow for devices that do not correctly follow the specification (when we discover them), rather than to intentionally deviate from the specification.</para>
        /// <para>If a value is explicitly set, it is used verbatim, and so any prefix (such as uuid:) must be provided in the value.</para>
        /// </remarks>
        public string Udn
        {
            get
            {
                if (String.IsNullOrEmpty(_Udn) && !String.IsNullOrEmpty(this.Uuid))
                    return "uuid:" + this.Uuid;
                else
                    return _Udn;
            }
            set
            {
                _Udn = value;
            }
        }

        /// <summary>
        /// Sets or returns a friendly/display name for this device on the network. Something the user can identify the device/instance by, i.e Lounge Main Light. Required.
        /// </summary>
        /// <remarks><para>A short description for the end user. </para></remarks>
        public string FriendlyName { get; set; }

        /// <summary>
        /// Sets or returns the name of the manufacturer of this device. Required.
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// Sets or returns a URL to the manufacturers web site. Optional.
        /// </summary>
        public Uri ManufacturerUrl { get; set; }

        /// <summary>
        /// Sets or returns a description of this device model. Recommended.
        /// </summary>
        /// <remarks><para>A long description for the end user.</para></remarks>
        public string ModelDescription { get; set; }

        /// <summary>
        /// Sets or returns the name of this model. Required.
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// Sets or returns the number of this model. Recommended.
        /// </summary>
        public string ModelNumber { get; set; }

        /// <summary>
        /// Sets or returns a URL to a web page with details of this device model. Optional.
        /// </summary>
        /// <remarks>
        /// <para>Optional. May be relative to base URL.</para>
        /// </remarks>
        public Uri ModelUrl { get; set; }

        /// <summary>
        /// Sets or returns the serial number for this device. Recommended.
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the URL used to retrieve the description document for this device/tree. Required.
        /// </summary>
        public Uri Location { get; set; }
        #endregion
    }
}
