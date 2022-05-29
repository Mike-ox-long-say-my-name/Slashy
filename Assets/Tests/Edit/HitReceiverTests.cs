using Core.Attacking;
using Core.Characters;
using NUnit.Framework;

namespace Tests.Edit
{
    [TestFixture]
    public class HitReceiverTests
    {
        [Test]
        public void ReceivesHit()
        {
            var hitReceiver = new HitReceiver();
            var passed = false;
            hitReceiver.HitReceived += (_, info) => passed = true;
            hitReceiver.ReceiveHit(A.HitInfoWith(1, 0, 0));
            Assert.True(passed);
        }
        
        [Test]
        public void ReceivesHit_And_Invokes_WithSameHitInfo()
        {
            var hitReceiver = new HitReceiver();

            var expectedInfo = A.HitInfoWith(1, 2, 3);
            HitInfo receivedInfo = null;
            
            hitReceiver.HitReceived += (_, info) => receivedInfo = info;
            hitReceiver.ReceiveHit(expectedInfo);

            Assert.AreSame(expectedInfo, receivedInfo);
        }
    }
}