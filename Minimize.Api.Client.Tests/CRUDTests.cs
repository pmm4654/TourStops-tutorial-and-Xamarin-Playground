using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
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

            string testEmail = $"csharp.integrationtests.{DateTime.Now.ToString("yyyyMMddhhmmss")}@test.com";

            var signupResponse = client.Signup(new Models.SignupRequest
            {
                email = testEmail,
                first_name = "integration",
                last_name = "tests",
                password = "Password123",
                password_confirmation = "Password123"
            }).Result;

            Assert.IsNotNull(signupResponse.auth_token, signupResponse.message);

            var loginResponse = client.Login(new Models.LoginRequest
            {
                email = testEmail,
                password = "Password123"
            }).Result;

            CleanThings();
            CleanCategories();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            CleanThings();
            CleanCategories();
        }

        [TestMethod]
        public void Things_CRUDMethods()
        {
            var mything = new Models.Thing
            {
                title = "my first thing",
                category_id = 10
            };

            var createdThing = client.PostThing(mything).Result;

            AssertThings(mything, createdThing);

            var gotThing = client.GetThing(createdThing.id).Result;

            AssertThings(mything, gotThing);

            var newThing = new Models.Thing
            {
                title = "my new thing",
                category_id = 20
            };

            Assert.IsTrue(client.PutThing(gotThing.id, newThing).Result);

            var gotUpdatedThing = client.GetThing(createdThing.id).Result;

            AssertThings(newThing, gotUpdatedThing);

            var anotherThing = client.PostThing(mything).Result;

            var myThings = client.GetThings().Result;

            Assert.IsTrue(myThings.Any(t => t.title == mything.title));
            Assert.IsTrue(myThings.Any(t => t.title == gotUpdatedThing.title));

            foreach (var thing in myThings)
            {
                Assert.IsTrue(client.DeleteThing(thing.id).Result);
            }

            var noThings = client.GetThings().Result;
            Assert.IsTrue(noThings.Count == 0);
        }

        [TestMethod]
        public void Categories_CRUDMethods()
        {
            var myCategory = new Models.Category
            {
                name = "my first Category",
                threshold = 10
            };

            var createdCategory = client.PostCategory(myCategory).Result;

            AssertCategories(myCategory, createdCategory);

            var gotCategory = client.GetCategory(createdCategory.id).Result;

            AssertCategories(myCategory, gotCategory);

            var newCategory = new Models.Category
            {
                name = "my new Category",
                threshold = 20
            };

            var wasUpdated = client.PutCategory(gotCategory.id, newCategory).Result;

            Assert.IsTrue(wasUpdated);

            var gotUpdatedCategory = client.GetCategory(createdCategory.id).Result;

            AssertCategories(newCategory, gotUpdatedCategory);

            var anotherCategory = client.PostCategory(myCategory).Result;

            var myCategories = client.GetCategories().Result;

            Assert.IsTrue(myCategories.Any(t => t.name == myCategory.name));
            Assert.IsTrue(myCategories.Any(t => t.name == gotUpdatedCategory.name));

            foreach (var Category in myCategories)
            {
                Assert.IsTrue(client.DeleteCategory(Category.id).Result);
            }

            var noCategories = client.GetCategories().Result;
            Assert.IsTrue(noCategories.Count == 0);

            CleanCategories();
        }

        private void AssertThings(Models.Thing one, Models.Thing two)
        {
            Assert.AreEqual(one.title, two.title);
            Assert.AreEqual(one.category_id, two.category_id);
        }

        private void AssertCategories(Models.Category one, Models.Category two)
        {
            Assert.AreEqual(one.name, two.name);
            Assert.AreEqual(one.threshold, two.threshold);
        }

        private static void CleanThings()
        {
            var things = client.GetThings().Result;
            foreach (var thing in things)
            {
                Assert.IsTrue(client.DeleteThing(thing.id).Result);
            }
        }

        private static void CleanCategories()
        {
            var categories = client.GetCategories().Result;
            foreach (var category in categories)
            {
                Assert.IsTrue(client.DeleteCategory(category.id).Result);
            }
        }
    }
}
