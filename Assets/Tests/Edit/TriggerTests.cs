using System.Linq;
using Core.Utilities;
using NUnit.Framework;

namespace Tests.Edit
{
    public class TriggerTests
    {
        [Test]
        public void Trigger_Sets()
        {
            var trigger = new Trigger();
            trigger.Set();
            Assert.True(trigger.CheckAndReset());
        }

        [Test]
        public void Trigger_ResetsAfterCheck()
        {
            var trigger = new Trigger();
            trigger.Set();
            trigger.CheckAndReset();
            Assert.True(trigger.IsFree);
        }

        [Test]
        public void TimedTrigger_SetFor()
        {
            var trigger = new TimedTrigger();
            trigger.SetFor(1.5f);
            trigger.Step(1);
            var firstCheck = trigger.IsSet;
            trigger.Step(1);
            var secondCheck = trigger.IsSet;
            Assert.True(firstCheck && !secondCheck);
        }

        [Test]
        public void TimedTrigger_SetIn()
        {
            var trigger = new TimedTrigger();
            trigger.SetIn(1.5f);
            trigger.Step(1);
            var firstCheck = trigger.IsSet;
            trigger.Step(1);
            var secondCheck = trigger.IsSet;
            Assert.True(!firstCheck && secondCheck);
        }

        [Test]
        public void TimedTrigger_ResetIn()
        {
            var trigger = new TimedTrigger();
            trigger.Set();
            trigger.ResetIn(1.5f);
            trigger.Step(1);
            var firstCheck = trigger.IsSet;
            trigger.Step(1);
            var secondCheck = trigger.IsSet;
            Assert.True(firstCheck && !secondCheck);
        }

        [Test]
        public void TimedTriggerFactory_ResetsAll()
        {
            var factory = new TimedTriggerFactory();
            var triggers = Enumerable.Repeat(0, 10).Select(_ => factory.Create()).ToList();
            foreach (var trigger in triggers)
            {
                trigger.Set();
            }

            factory.ResetAll();
            Assert.True(triggers.TrueForAll(trigger => trigger.IsFree));
        }

        [Test]
        public void TimedTriggerFactory_StepAll()
        {
            var factory = new TimedTriggerFactory();
            var triggers = Enumerable.Repeat(0, 10).Select(_ => factory.Create()).ToList();
            foreach (var trigger in triggers)
            {
                trigger.SetFor(1);
            }

            factory.StepAll(1);
            Assert.True(triggers.TrueForAll(trigger => trigger.IsFree));
        }
    }
}