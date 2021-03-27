using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using HeroesGame.Constant;
using HeroesGame.Contract;
using HeroesGame.Implementation.Hero;
using HeroesGame.Implementation.Weapon;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace HeroesGame.Tests
{
    [TestFixture]
    public class HeroShould
    {
        private Mock<Mage> _hero;

        [SetUp]
        public void TestSetup()
        {
            //this._hero = new Mage();
            this._hero = new Mock<Mage>();
            this._hero.Protected().Setup("LevelUp").CallBase();
        }

        [Test]
        public void HaveCorrectInitialValues()
        {
            //Arrange
            //Act

            //Assert
            Assert.That(_hero.Object.Level, Is.EqualTo(HeroConstants.InitialLevel));
            Assert.That(_hero.Object.Armor, Is.EqualTo(HeroConstants.InitialArmor));
            Assert.That(_hero.Object.Health, Is.EqualTo(HeroConstants.InitialMaxHealth));
            Assert.That(_hero.Object.MaxHealth, Is.EqualTo(HeroConstants.InitialMaxHealth));
            Assert.That(_hero.Object.Experience, Is.EqualTo(HeroConstants.InitialExperience));
            Assert.That(_hero.Object.Weapon, Is.Not.Null);
            //Assert.That(hero.Weapon,Is.TypeOf(typeof(Staff)));
        }

        [Test]

        public void TakeHitCorrectly([Range(-50,100,10)] double damage)
        {
            //Arrange

            //Act
            if (damage > 0) { 

                _hero.Object.TakeHit(damage);
            }
            //Assert
            if (damage < 0)
            {
                Assert.Throws<ArgumentException>(() => _hero.Object.TakeHit(damage), "Damage value cannot be negative");
            }
            else
            {
                Assert.That(_hero.Object.Health, Is.EqualTo(HeroConstants.InitialMaxHealth - damage + _hero.Object.Armor));
            }

        }
        [Test]
        public void GainExperienceCorrectly([Range(25,500,25)]double xp)
        {
            //Arrange

            //Act
            _hero.Object.GainExperience(xp);
            //Assert
            if (xp >= HeroConstants.MaximumExperience)
            {
                var expectedXp = (HeroConstants.InitialExperience + xp) % HeroConstants.MaximumExperience;
                Assert.That(_hero.Object.Experience, Is.EqualTo(expectedXp));
                Assert.That(_hero.Object.Level,Is.EqualTo(HeroConstants.InitialLevel + 1));
            }
            else
            {
                Assert.That(_hero.Object.Experience,Is.EqualTo(HeroConstants.InitialExperience + xp));
            }
        }

        [Test]
        public void HealCorrectly([Range(5,25,1)]int level, [Range(25,500,25)]int damage)
        {
            //Arrange
            //Act
            this.LevelUp(level);
            double totalDamage = HeroConstants.InitialMaxHealth + damage;
            totalDamage = _hero.Object.TakeHit(totalDamage);
            _hero.Object.Heal();
            //Assert
            var expectedHealValue = _hero.Object.Level * HeroConstants.HealPerLevel;
            var expectedHealth = (_hero.Object.MaxHealth - totalDamage) + expectedHealValue;
            if (expectedHealth > _hero.Object.MaxHealth)
            {
                expectedHealth = _hero.Object.MaxHealth;
            }
            Assert.That(_hero.Object.Health, Is.EqualTo(expectedHealth));
        }

        private void LevelUp(int levels)
        {
            for (int i = 0; i < levels; i++)
            {
                _hero.Object.GainExperience(HeroConstants.MaximumExperience);
            }
        }
    }

}
