using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minimize.Api.Client.Tests
{
    [TestClass]
    public class CRUDTests
    {
        private static ApiClient client;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            client = new ApiClient();

            //string testEmail = $"csharp.integrationtests.{DateTime.Now.ToString("yyyyMMddhhmmss")}@test.com";

            //var signupResponse = client.Signup(new Models.SignupRequest
            //{
            //    email = testEmail,
            //    first_name = "integration",
            //    last_name = "tests",
            //    password = "Password123",
            //    password_confirmation = "Password123"
            //}).Result;

            //Assert.IsNotNull(signupResponse.AuthorizationToken, signupResponse.message);

            //var loginResponse = client.Login(new Models.LoginRequest
            //{
            //    email = "marco.fatica@gmail.com",
            //    password = "deeznuts"
            //}).Result;

            client.AuthorizationToken = "eyJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoxMSwiZXhwIjoxNTI5ODg4ODE2fQ.SA4VlUah2xLNpG1YJyjBzJrDXUwvqutzuUzCZtgqV1Q";
            
        }

        [TestMethod]
        public async Task Things_CRUDMethods()
        {
            CleanThings();
            
            var mything = new Models.Thing
            {
                title = "my first thing",
                category_id = 10
            };

            var createdThing = await client.PostThing(mything);

            AssertThings(mything, createdThing);

            var gotThing = await client.GetThing(createdThing.id);

            AssertThings(mything, gotThing);

            var newThing = new Models.Thing
            {
                title = "my new thing",
                category_id = 20
            };

            var wasUpdated = await client.PutThing(gotThing.id, newThing);

            Assert.IsTrue(wasUpdated);

            var gotNewThing = await client.GetThing(createdThing.id);

            AssertThings(newThing, gotNewThing);

            await client.PostThing(mything);

            var myThings = await client.GetThings();

            CollectionAssert.AreEqual(
                    new List<Models.Thing> { gotNewThing, mything },
                    myThings);

            foreach(var thing in myThings)
            {
                await client.DeleteThing(thing.id);
            }

            var noThings = await client.GetThings();
            Assert.IsTrue(noThings.Count == 0);

            CleanThings();
        }

        private void AssertThings(Models.Thing one, Models.Thing two)
        {
            Assert.AreEqual(one.title, two.title);
            Assert.AreEqual(one.category_id, two.category_id);
        }

        private async void CleanThings()
        {
            var things = await client.GetThings();
            foreach (var thing in things)
            {
                await client.DeleteThing(thing.id);
            }
        }
    }
}
