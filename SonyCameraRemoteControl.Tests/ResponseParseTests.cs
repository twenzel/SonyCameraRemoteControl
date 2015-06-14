using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SonyCameraRemoteControl.Tests
{
    [TestClass]
    public class ResponseParseTests
    {
        [TestMethod]
        public void SimpleString()
        {
            string response = "{\"result\": \"Hello Camera Remote API\", \"error\": null, \"id\": 1}";
            StringResult result = StringResult.Parse(response);

            Assert.AreEqual("Hello Camera Remote API", result.Value);
            Assert.AreEqual(null, result.Error);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.AreEqual("1", result.Id);
        }

        [TestMethod]
        public void SimpleString_2()
        {
            string response = "{\"result\": \"Hello Camera Remote API\", \"id\": 1}";
            StringResult result = StringResult.Parse(response);

            Assert.AreEqual("Hello Camera Remote API", result.Value);
            Assert.AreEqual(null, result.Error);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.AreEqual("1", result.Id);
        }

        [TestMethod]
        public void ErrorResponse()
        {
            string response = "{\"error\": [5, \"Illegal Request\"], \"id\": 1}";
            StringResult result = StringResult.Parse(response);

            Assert.AreEqual(null, result.Value);
            Assert.AreEqual("Illegal Request", result.Error);
            Assert.AreEqual(5, result.ErrorCode);
            Assert.AreEqual("1", result.Id);
        }

        [TestMethod]
        public void Empty()
        {
            string response = "{\"result\": [], \"id\": 1}";
            StringsResult result = StringsResult.Parse(response);

            Assert.AreNotEqual(null, result.Value);
            Assert.AreEqual(0, result.Value.Length);
            Assert.AreEqual(null, result.Error);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.AreEqual("1", result.Id);
        }

        [TestMethod]
        public void StringArray()
        {
            string response = "{\"result\": [\"Hello Camera Remote API\"], \"error\": null, \"id\": 1}";
            StringsResult result = StringsResult.Parse(response);

            Assert.AreEqual(1, result.Value.Length);
            Assert.AreEqual("Hello Camera Remote API", result.Value[0]);
            Assert.AreEqual(null, result.Error);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.AreEqual("1", result.Id);
        }

        [TestMethod]
        public void StringArray_2()
        {
            string response = "{\"result\": [\"first\", \"second\"], \"id\": 1}";
            StringsResult result = StringsResult.Parse(response);

            Assert.AreEqual(2, result.Value.Length);
            Assert.AreEqual("first", result.Value[0]);
            Assert.AreEqual("second", result.Value[1]);
            Assert.AreEqual(null, result.Error);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.AreEqual("1", result.Id);
        }

        [TestMethod]
        public void Dictionary()
        {
            string response = "{\"id\": 1, \"result\": [{\"frameInfo\": true}]}";
            DictResult result = DictResult.Parse(response);

            Assert.AreEqual(1, result.Value.Count);

            Assert.AreEqual("frameInfo", result.Value.ElementAt(0).Key);
            Assert.AreEqual(true, result.Value.ElementAt(0).Value);            
        }

        [TestMethod]
        public void MultipleValues()
        {
            string response = "{\"id\": 1, \"result\": [{\"contShootingMode\": \"Spd Priority Cont.\",\"candidate\": [\"Single\",\"Continuous\",\"Spd Priority Cont.\"]}]}";
            ValuesResult result = ValuesResult.Parse(response);

            Assert.AreEqual(1, result.Value.Count);
            IDictionary<string, object> values = result.Value[0] as IDictionary<string, object>;

            Assert.AreEqual(2, values.Count);

            Assert.AreEqual("contShootingMode", values.ElementAt(0).Key);
            Assert.AreEqual("Spd Priority Cont.", values.ElementAt(0).Value);
            Assert.AreEqual("candidate", values.ElementAt(1).Key);
            Assert.AreEqual(typeof(ArrayList), values.ElementAt(1).Value.GetType());
            Assert.AreEqual(null, result.Error);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.AreEqual("1", result.Id);
        }
    }
}
