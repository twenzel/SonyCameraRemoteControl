using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace SonyCameraRemoteControl.Tests
{
	[TestFixture ()]
    public class ResponseParseTests
    {
		[Test ()]
        public void SimpleString()
        {
            string response = "{\"result\": \"Hello Camera Remote API\", \"error\": null, \"id\": 1}";
            StringResult result = StringResult.Parse(response);

            Assert.AreEqual("Hello Camera Remote API", result.Value);
            Assert.AreEqual(null, result.ErrorText);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.AreEqual("1", result.Id);
        }

		[Test ()]
        public void SimpleString_2()
        {
            string response = "{\"result\": \"Hello Camera Remote API\", \"id\": 1}";
            StringResult result = StringResult.Parse(response);

            Assert.AreEqual("Hello Camera Remote API", result.Value);
            Assert.AreEqual(null, result.ErrorText);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.AreEqual("1", result.Id);
        }

		[Test ()]
        public void ErrorResponse()
        {
            string response = "{\"error\": [5, \"Illegal Request\"], \"id\": 1}";
            StringResult result = StringResult.Parse(response);

            Assert.AreEqual(null, result.Value);
            Assert.AreEqual("Illegal Request", result.ErrorText);
            Assert.AreEqual(5, result.ErrorCode);
            Assert.AreEqual("1", result.Id);
        }

		[Test ()]
        public void Empty()
        {
            string response = "{\"result\": [], \"id\": 1}";
            StringsResult result = StringsResult.Parse(response);

            Assert.AreNotEqual(null, result.Value);
            Assert.AreEqual(0, result.Value.Length);
            Assert.AreEqual(null, result.ErrorText);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.AreEqual("1", result.Id);
        }

		[Test ()]
        public void StringArray()
        {
            string response = "{\"result\": [\"Hello Camera Remote API\"], \"error\": null, \"id\": 1}";
            StringsResult result = StringsResult.Parse(response);

            Assert.AreEqual(1, result.Value.Length);
            Assert.AreEqual("Hello Camera Remote API", result.Value[0]);
            Assert.AreEqual(null, result.ErrorText);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.AreEqual("1", result.Id);
        }

		[Test ()]
        public void StringArray_2()
        {
            string response = "{\"result\": [\"first\", \"second\"], \"id\": 1}";
            StringsResult result = StringsResult.Parse(response);

            Assert.AreEqual(2, result.Value.Length);
            Assert.AreEqual("first", result.Value[0]);
            Assert.AreEqual("second", result.Value[1]);
            Assert.AreEqual(null, result.ErrorText);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.AreEqual("1", result.Id);
        }

		[Test ()]
        public void Dictionary()
        {
            string response = "{\"id\": 1, \"result\": [{\"frameInfo\": true}]}";
            DictResult result = DictResult.Parse(response);

            Assert.AreEqual(1, result.Value.Count);

            Assert.AreEqual("frameInfo", result.Value.ElementAt(0).Key);
            Assert.AreEqual(true, result.Value.ElementAt(0).Value);            
        }

        [Test()]
        public void Dictionary_2()
        {
            string response = "{\"id\": 1, \"result\": [{\"frameInfo\": true, \"test\": 33}]}";
            DictResult result = DictResult.Parse(response);

            Assert.AreEqual(2, result.Value.Count);

            Assert.AreEqual("frameInfo", result.Value.ElementAt(0).Key);
            Assert.AreEqual(true, result.Value.ElementAt(0).Value);

            Assert.AreEqual("test", result.Value.ElementAt(1).Key);
            Assert.AreEqual(33, result.Value.ElementAt(1).Value);
        }

		[Test ()]
        public void MultipleValues()
        {
            string response = "{\"id\": 1, \"result\": [{\"contShootingMode\": \"Spd Priority Cont.\",\"candidate\": [\"Single\",\"Continuous\",\"Spd Priority Cont.\"]}]}";
            ValuesResult result = ValuesResult.Parse(response);

            Assert.AreEqual(2, result.Value.Count);
            assetMultiple(result);
           
        }

        [Test()]
        public void MultipleValues_2()
        {
            string response = "{\"id\": 1, \"result\": [{\"contShootingMode\": \"Spd Priority Cont.\",\"candidate\": [\"Single\",\"Continuous\",\"Spd Priority Cont.\"], \"testMode\": \"still\"}]}";
            ValuesResult result = ValuesResult.Parse(response);

            Assert.AreEqual(3, result.Value.Count);

            assetMultiple(result);
            Assert.AreEqual("testMode", result.Value.ElementAt(2).Key);
            Assert.AreEqual("still", result.Value.ElementAt(2).Value);

            List<object> innerList = (List<object>)result.Value.ElementAt(1).Value;
            Assert.AreEqual(3, innerList.Count);
            Assert.AreEqual("Single", innerList[0]);
            Assert.AreEqual("Continuous", innerList[1]);
            Assert.AreEqual("Spd Priority Cont.", innerList[2]);
        }


        [Test()]
        public void MultipleValues_3()
        {
            string response = "{\"id\": 1, \"result\": [{\"contShootingMode\": \"Spd Priority Cont.\",\"candidate\": [\"Single\",{\"key1\": \"val1\", \"key2\": false},\"Spd Priority Cont.\"], \"testMode\": \"still\"}]}";
            ValuesResult result = ValuesResult.Parse(response);

            Assert.AreEqual(3, result.Value.Count);

            assetMultiple(result);
            Assert.AreEqual("testMode", result.Value.ElementAt(2).Key);
            Assert.AreEqual("still", result.Value.ElementAt(2).Value);

            List<object> innerList = (List<object>)result.Value.ElementAt(1).Value;
            Assert.AreEqual(3, innerList.Count);
            Assert.AreEqual("Single", innerList[0]);
            Assert.AreEqual(typeof(Dictionary<string, object>), innerList[1].GetType());
            Assert.AreEqual("Spd Priority Cont.", innerList[2]);

            Dictionary<string, object> innerdict = (Dictionary<string, object>)innerList[1];
            Assert.AreEqual("key1", innerdict.ElementAt(0).Key);
            Assert.AreEqual("val1", innerdict.ElementAt(0).Value);

            Assert.AreEqual("key2", innerdict.ElementAt(1).Key);
            Assert.AreEqual(false, innerdict.ElementAt(1).Value);
        }

        private void assetMultiple(ValuesResult result)
        {
            Assert.AreEqual("contShootingMode", result.Value.ElementAt(0).Key);
            Assert.AreEqual("Spd Priority Cont.", result.Value.ElementAt(0).Value);
            Assert.AreEqual("candidate", result.Value.ElementAt(1).Key);
            Assert.AreEqual(typeof(List<object>), result.Value.ElementAt(1).Value.GetType());
            Assert.AreEqual(null, result.ErrorText);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.AreEqual("1", result.Id);
        }
    }
}
