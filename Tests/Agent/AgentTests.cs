using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FunK.Tests
{
    public class AgentTests
    {
        [Fact (Skip = "Heavy test, use for documentation")]
        public void TestAgents()
        {
            Agent<string> logger, ping, pong = null;
            int totalCount = 0, pingCount = 0, pongCount = 0;

            logger = Agent.Start<string>(msg => ++totalCount);

            ping = Agent.Start((string msg) =>
            {
                pingCount++;
                if (msg == "STOP") return;

                logger.Tell($"Received '{msg}'; Sending 'PING'");
                Task.Delay(500).Wait();
                pong.Tell("PING");
            });

            pong = Agent.Start(0, (int count, string msg) =>
            {
                int newCount = count + 1;
                pongCount++;
                string nextMsg = (newCount < 5) ? "PONG" : "STOP";

                logger.Tell($"Received '{msg}' #{newCount}; Sending '{nextMsg}'");
                Task.Delay(500).Wait();
                ping.Tell(nextMsg);

                return newCount;
            });

            ping.Tell("START");

            Thread.Sleep(10000);
            Assert.Equal(10, totalCount);
            Assert.Equal(6, pingCount);
            Assert.Equal(5, pongCount);
        }
    }
}
