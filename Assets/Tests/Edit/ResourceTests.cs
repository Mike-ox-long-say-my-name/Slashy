using Core.Characters;
using NUnit.Framework;

namespace Tests.Edit
{
    [TestFixture]
    public class ResourceTests
    {
        [Test]
        public void Changes_Value()
        {
            var resource = new HealthResource(10);
            resource.Value = 1;
            Assert.AreEqual(resource.Value, 1);
        }
        
        [Test]
        public void Recovers_Value()
        {
            var resource = new HealthResource(10)
            {
                Value = 0
            };
            resource.Recover(3);
            Assert.AreEqual(resource.Value, 3);
        }
        
        [Test]
        public void Spends_Value()
        {
            var resource = new HealthResource(10)
            {
                Value = 10
            };
            resource.Spend(3);
            Assert.AreEqual(resource.Value, 7);
        }
        
        [Test]
        public void Clamps_Value_To_Max()
        {
            var resource = new HealthResource(10);
            resource.Value = 100;
            Assert.AreEqual(resource.Value, 10);
        }
        
        [Test]
        public void Clamps_Value_To_Zero()
        {
            var resource = new HealthResource(10);
            resource.Value = -10;
            Assert.AreEqual(resource.Value, 0);
        }
        
        [Test]
        public void DoesNotChange_Value_When_Frozen()
        {
            var resource = new HealthResource(10)
            {
                Value = 3
            };
            resource.Frozen = true;
            resource.Value = 5;
            Assert.AreEqual(resource.Value, 3);
        }
    }
}