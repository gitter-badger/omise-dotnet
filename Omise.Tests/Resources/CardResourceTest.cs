﻿using System;
using Omise.Resources;
using NUnit.Framework;
using Omise.Models;
using Omise.Tests.Util;
using System.Reflection;

namespace Omise.Tests.Resources {
    [TestFixture]
    public class CardResourceTest : ResourceTest<CardResource> {
        const string CustomerId = "cust_test_4yq6txdpfadhbaqnwp3";
        const string CardId = "card_test_4yq6tuucl9h4erukfl0";

        [Test]
        public async void TestGetList() {
            await Resource.GetList();
            AssertRequest("GET", "https://api.omise.co/customers/{0}/cards", CustomerId);
        }
        
        [Test]
        public async void TestGet() {
            await Resource.Get(CardId);
            AssertRequest("GET", "https://api.omise.co/customers/{0}/cards/{1}", CustomerId, CardId);
        }

        [Test]
        public async void TestUpdate() {
            await Resource.Update(CardId, BuildUpdateRequest());
            AssertRequest("PATCH", "https://api.omise.co/customers/{0}/cards/{1}", CustomerId, CardId);
        }

        [Test]
        public async void TestDestroy() {
            await Resource.Destroy(CardId);
            AssertRequest("DELETE", "https://api.omise.co/customers/{0}/cards/{1}", CustomerId, CardId);
        }

        [Test]
        public void TestUpdateCardRequest() {
            AssertSerializedRequest(BuildUpdateRequest(),
                "name=MasterCard+SmartPay&" +
                "city=Bangkok&" +
                "postal_code=12345&" +
                "expiration_month=12&" +
                "expiration_year=2018"
            );
        }
            
        [Test]
        public async void TestFixturesGetList() {
            var list = await Fixtures.GetList();
            Assert.AreEqual(1, list.Count);

            var card = list[0];
            Assert.AreEqual(CardId, card.Id);
            Assert.AreEqual("4242", card.LastDigits);
        }

        [Test]
        public async void TestFixturesGet() {
            var card = await Fixtures.Get(CardId);
            Assert.AreEqual(CardId, card.Id);
            Assert.AreEqual("4242", card.LastDigits);
        }

        [Test]
        public async void TestFixturesUpdate() {
            var card = await Fixtures.Update(CardId, new UpdateCardRequest());
            Assert.AreEqual(CardId, card.Id);
            Assert.AreEqual("JOHN W. DOE", card.Name);
        }

        [Test]
        public async void TestFixturesDestroy() {
            var card = await Fixtures.Destroy(CardId);
            Assert.AreEqual(CardId, card.Id);
            Assert.IsTrue(card.Deleted);
        }
            
        protected UpdateCardRequest BuildUpdateRequest() {
            return new UpdateCardRequest
            {
                Name = "MasterCard SmartPay",
                City = "Bangkok",
                PostalCode = "12345",
                ExpirationMonth = 12,
                ExpirationYear = 2018,
            };
        }

        protected override CardResource BuildResource(IRequester requester) {
            return new CardResource(requester, CustomerId);
        }
    }
}

