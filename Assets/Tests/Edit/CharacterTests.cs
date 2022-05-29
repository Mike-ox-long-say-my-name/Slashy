using Core.Characters;
using NUnit.Framework;

namespace Tests.Edit
{
    [TestFixture]
    public class CharacterTests
    {
        [Test]
        public void Character_Dies_On_Kill()
        {
            var health = new HealthResource(10);
            var balance = new BalanceResource(10);
            var hitReceiver = new HitReceiver();

            var character = new Character(health, balance, hitReceiver);
            character.Kill();

            Assert.True(character.IsDead);
        }

        [Test]
        public void Character_Dies_On_Kill_When_CannotDie()
        {
            var health = new HealthResource(10);
            var balance = new BalanceResource(10);
            var hitReceiver = new HitReceiver();

            var character = new Character(health, balance, hitReceiver)
            {
                CanDie = false
            };
            character.Kill();

            Assert.True(character.IsDead);
        }

        [Test]
        public void Character_Dies_On_LethalDamage()
        {
            var health = new HealthResource(10);
            var balance = new BalanceResource(10);
            var hitReceiver = new HitReceiver();

            var character = new Character(health, balance, hitReceiver);
            hitReceiver.ReceiveHit(A.HitInfoWith(10, 1));

            Assert.True(character.IsDead);
        }

        [Test]
        public void Character_Receives_Damage()
        {
            var health = new HealthResource(10);
            var balance = new BalanceResource(10);
            var hitReceiver = new HitReceiver();

            var character = new Character(health, balance, hitReceiver);
            hitReceiver.ReceiveHit(A.HitInfoWith(5, 1));

            Assert.AreEqual(5, health.Value);
        }

        [Test]
        public void Character_Receives_BalanceDamage()
        {
            var health = new HealthResource(10);
            var balance = new BalanceResource(10);
            var hitReceiver = new HitReceiver();

            var character = new Character(health, balance, hitReceiver);
            hitReceiver.ReceiveHit(A.HitInfoWith(5, 6));

            Assert.AreEqual(4, balance.Value);
        }

        [Test]
        public void Character_Invoke_Staggered()
        {
            var health = new HealthResource(10);
            var balance = new BalanceResource(10);
            var hitReceiver = new HitReceiver();

            var character = new Character(health, balance, hitReceiver);

            var passed = false;
            character.Staggered += _ => passed = true;

            hitReceiver.ReceiveHit(A.HitInfoWith(1, 10, 1));

            Assert.True(passed);
        }

        [Test]
        public void Character_Invoke_StaggerEnded()
        {
            var health = new HealthResource(10);
            var balance = new BalanceResource(10);
            var hitReceiver = new HitReceiver();

            var character = new Character(health, balance, hitReceiver);
            hitReceiver.ReceiveHit(A.HitInfoWith(1, 10, 1));

            var passed = false;
            character.RecoveredFromStagger += () => passed = true;
            character.Tick(1);

            Assert.True(passed);
        }
    }
}