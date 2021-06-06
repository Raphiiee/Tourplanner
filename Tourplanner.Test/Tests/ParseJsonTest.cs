using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tourplanner.BusinessLayer;
using Tourplanner.Models;

namespace Tourplanner.Test.Tests
{
    [TestFixture]
    public class ParseJsonTest
    {
        private TourItemFactoryImpl _tourItemFactory;
        private JObject _jsonObject;
        private TourItem _item;

        [SetUp]
        public void Setup()
        {
            _tourItemFactory = new TourItemFactoryImpl();
        }

        [Test]
        public void ParseWork()
        {
            _jsonObject = JObject.Parse("{\"route\":{\"fuelUsed\":0.23,\"realTime\":653,\"distance\":3.693,\"time\":501,\"sessionId\":\"55e60cd9-00b6-001a-02b7-20ac-00163e7dd551\",\"legs\":[{\"maneuvers\":[{\"index\":0,\"narrative\":\"Start out going east on Clarendon Blvd toward N Queen St.\",\"distance\":0.031,\"formattedTime\":\"00:00:06\"},{\"index\":2,\"narrative\":\"Turn right onto 14th St N.\",\"distance\":0.003,\"formattedTime\":\"00:00:05\"},{\"index\":3,\"narrative\":\"Merge onto Arlington Blvd/US-50 W via the ramp on the left.\",\"distance\":1.606,\"formattedTime\":\"00:02:31\"},{\"index\":4,\"narrative\":\"Turn left onto N Fillmore St.\",\"distance\":0.408,\"formattedTime\":\"00:01:14\"},{\"index\":5,\"narrative\":\"Stay straight to go onto S Walter Reed Dr.\",\"distance\":0.884,\"formattedTime\":\"00:02:29\"},{\"index\":6,\"narrative\":\"Turn slight left onto S Glebe Rd/VA-120.\",\"distance\":0.593,\"formattedTime\":\"00:01:24\"},{\"index\":7,\"narrative\":\"2400 S GLEBE RD is on the right.\",\"distance\":0,\"formattedTime\":\"00:00:00\"}]}],\"formattedTime\":\"00:08:21\",\"routeError\":{\"message\":\"\",\"errorCode\":-400}},\"info\":{\"statuscode\":0,\"messages\":[]}}");
            _item = new TourItem();

            _tourItemFactory.LoadJsonData(_item, _jsonObject);

            Assert.AreEqual(0.23f, _item.FuelUsed);
        }

        [Test]
        public void ParseError()
        {
            _jsonObject = JObject.Parse("{\"route\":{\"routeError\":{\"errorCode\":2,\"message\":\"\"}},\"info\":{\"statuscode\":402,\"messages\":[\"We are unable to route with the given locations.\"]}}");
            _item = new TourItem();

            _tourItemFactory.LoadJsonData(_item, _jsonObject);

            Assert.AreEqual("We are unable to route with the given locations.", _item.TourDescription);
            
        }
    }
}