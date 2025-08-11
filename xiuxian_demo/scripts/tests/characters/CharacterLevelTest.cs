using System;
using XiuxianDemo;
using NUnit.Framework;

namespace XiuxianDemoTests
{
    [TestFixture]
    public class CharacterLevelTest
    {
        private CharacterLevel _levelSystem;

        [NUnit.Framework.SetUp]
        public void SetUp()
        {
            _levelSystem = new CharacterLevel();
        }

        [Test]
        public void Initialize_ShouldSetDefaultValues()
        {
            // Assert
            Assert.AreEqual(1, _levelSystem.CurrentLevel);
            Assert.AreEqual(0, _levelSystem.CurrentExperience);
            Assert.AreEqual(150, _levelSystem.ExperienceToNextLevel);
            Assert.IsNotNull(_levelSystem.AttributeGrowthRates);
            Assert.AreEqual(6, _levelSystem.AttributeGrowthRates.Length);
        }

        [Test]
        public void SetCurrentLevel_WithValidLevel_ShouldUpdateLevel()
        {
            // Act
            _levelSystem.SetCurrentLevel(5);

            // Assert
            Assert.AreEqual(5, _levelSystem.CurrentLevel);
            Assert.AreEqual(550, _levelSystem.ExperienceToNextLevel); // 5 * 100 + 50
        }

        [Test]
        public void SetCurrentLevel_WithInvalidLevel_ShouldNotUpdateLevel()
        {
            // Act
            _levelSystem.SetCurrentLevel(0);

            // Assert
            Assert.AreEqual(1, _levelSystem.CurrentLevel);
        }

        [Test]
        public void AddExperience_WithValidAmount_ShouldUpdateExperience()
        {
            // Act
            _levelSystem.AddExperience(100);

            // Assert
            Assert.AreEqual(100, _levelSystem.CurrentExperience);
        }

        [Test]
        public void AddExperience_WithInvalidAmount_ShouldNotUpdateExperience()
        {
            // Act
            _levelSystem.AddExperience(-50);

            // Assert
            Assert.AreEqual(0, _levelSystem.CurrentExperience);
        }

        [Test]
        public void CanLevelUp_WhenExperienceIsEnough_ShouldReturnTrue()
        {
            // Arrange
            _levelSystem.AddExperience(150);

            // Act
            bool canLevelUp = _levelSystem.CanLevelUp();

            // Assert
            Assert.IsTrue(canLevelUp);
        }

        [Test]
        public void CanLevelUp_WhenExperienceIsNotEnough_ShouldReturnFalse()
        {
            // Arrange
            _levelSystem.AddExperience(100);

            // Act
            bool canLevelUp = _levelSystem.CanLevelUp();

            // Assert
            Assert.IsFalse(canLevelUp);
        }

        [Test]
        public void LevelUp_WhenCanLevelUp_ShouldIncreaseLevel()
        {
            // Arrange
            _levelSystem.AddExperience(150);

            // Act
            bool leveledUp = _levelSystem.LevelUp();

            // Assert
            Assert.IsTrue(leveledUp);
            Assert.AreEqual(2, _levelSystem.CurrentLevel);
            Assert.AreEqual(0, _levelSystem.CurrentExperience);
            Assert.AreEqual(250, _levelSystem.ExperienceToNextLevel); // 2 * 100 + 50
        }

        [Test]
        public void LevelUp_WhenCannotLevelUp_ShouldNotIncreaseLevel()
        {
            // Arrange
            _levelSystem.AddExperience(100);

            // Act
            bool leveledUp = _levelSystem.LevelUp();

            // Assert
            Assert.IsFalse(leveledUp);
            Assert.AreEqual(1, _levelSystem.CurrentLevel);
            Assert.AreEqual(100, _levelSystem.CurrentExperience);
        }

        [Test]
        public void GetRequiredExperience_WithValidLevel_ShouldReturnCorrectValue()
        {
            // Act
            float requiredExp = _levelSystem.GetRequiredExperience(5);

            // Assert
            Assert.AreEqual(550, requiredExp); // 5 * 100 + 50
        }

        [Test]
        public void GetAttributeGrowthRates_ShouldReturnUpdatedRatesAfterLevelUp()
        {
            // Arrange
            _levelSystem.SetCurrentLevel(1);
            float[] initialRates = _levelSystem.GetAttributeGrowthRates();

            // Act
            _levelSystem.SetCurrentLevel(5);
            float[] newRates = _levelSystem.GetAttributeGrowthRates();

            // Assert
            Assert.AreEqual(20 + (5-1)*2, newRates[0]);    // 气血成长率
            Assert.AreEqual(10 + (5-1)*1, newRates[1]);    // 法力成长率
            Assert.AreEqual(2 + (5-1)*0.5f, newRates[2]);  // 攻击成长率
            Assert.AreEqual(1 + (5-1)*0.3f, newRates[3]);  // 防御成长率
            Assert.AreEqual(1 + (5-1)*0.2f, newRates[4]);  // 速度成长率
            Assert.AreEqual(0.5f + (5-1)*0.1f, newRates[5]); // 暴击成长率
        }
    }
}