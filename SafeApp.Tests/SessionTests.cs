using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SafeApp.Tests
{
    [TestFixture]
    internal class SessionTests
    {
        [Test]
        public async Task NetworkReconnectTest()
        {
            var session = await Utils.CreateTestApp();

            Session.Disconnected += async (o, i) =>
            {
                Assert.True(session.IsDisconnected);
                await session.ReconnectAsync();
                Assert.False(session.IsDisconnected);
            };

            Assert.False(session.IsDisconnected);
            await session.SimulateMockNetworkDisconnectAsync();
        }
    }
}
