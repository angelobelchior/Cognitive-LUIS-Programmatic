﻿using Cognitive.LUIS.Programmatic.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Cognitive.LUIS.Programmatic.Tests
{
    public class PublishTests : BaseTest
    {
        public PublishTests() =>
            Initialize();

        [Fact]
        public async Task ShouldSendPublishRequest()
        {
            using (var client = new LuisProgClient(SubscriptionKey, Region))
            {
                await client.Intents.AddAsync("IntentTest", appId, appVersion);

                await client.Examples.AddAsync(appId, appVersion, new Example
                {
                    Text = "Hello World!",
                    IntentName = "IntentTest"
                });

                var trainingDetails = await client.Training.TrainAndGetFinalStatusAsync(appId, appVersion);
                if (trainingDetails.Status.Equals("Success"))
                {
                    var publish = await client.Publishing.PublishAsync(appId, appVersion);
                    Assert.NotNull(publish);
                }
            }
        }

        [Fact]
        public async Task ShouldThrowExceptionOnPublishModelWhenAppNotExists()
        {
            using (var client = new LuisProgClient(SubscriptionKey, Region))
            {
                var ex = await Assert.ThrowsAsync<Exception>(() =>
                    client.Publishing.PublishAsync(InvalidId, appVersion));

                Assert.Equal("BadArgument - Cannot find an application with the ID 51593248-363e-4a08-b946-2061964dc690.", ex.Message);
            }
        }

        public override void Dispose() =>
            Cleanup();
    }
}
