﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Linq2Azure;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace IntegrationTests
{
    [TestClass]
    public class SubscriptionTests
    {
        [TestMethod]
        public void CanLoadFromPublisherSettingsFile()
        {
            Assert.IsTrue(!string.IsNullOrEmpty(TestConstants.Subscription.Name));
            Assert.IsTrue(TestConstants.Subscription.ID != Guid.Empty);
            Assert.IsNotNull(TestConstants.Subscription.ManagementCertificate);
        }

        [TestMethod]
        public async Task CleanupOldResidue()
        {
            foreach (var cs in await TestConstants.Subscription.GetCloudServicesAsync())
                if (cs.Name.StartsWith("Test-") && cs.DateLastModified < DateTime.UtcNow.AddMinutes(-5))
                {
                    foreach (var d in await cs.GetDeploymentsAsync()) await d.DeleteAsync();
                    cs.DeleteAsync().Wait();
                }
        }
    }
}