using Core.Utilities;
using NUnit.Framework;

namespace Tests.Edit
{
    [TestFixture]
    public class TimerTests
    {
        [Test]
        public void Timer_Timeout()
        {
            var timer = new Timer();
            var passed = false;
            timer.Timeout += () => passed = true;
            timer.Start(1);
            timer.Tick(1);
            Assert.True(passed);
        }

        [Test]
        public void Timer_NoTimeout()
        {
            var timer = new Timer();
            var passed = true;
            timer.Timeout += () => passed = false;
            timer.Start(1);
            timer.Tick(0.5f);
            Assert.True(passed);
        }

        [Test]
        public void Timer_Timeout_Repeat()
        {
            var timer = new Timer();

            var timeouts = 0;
            timer.Timeout += () => timeouts++;
            timer.Start(1, true);
            
            for (var i = 0; i < 10; i++)
            {
                timer.Tick(1);
            }

            Assert.AreEqual(10, timeouts);
        }
    }
}