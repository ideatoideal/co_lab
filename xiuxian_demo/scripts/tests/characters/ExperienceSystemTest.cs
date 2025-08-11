using Godot;
using NUnit.Framework;
using System;
using XiuxianDemo;

namespace XiuxianDemoTests
{
    [TestFixture]
    public class ExperienceSystemTest
    {
        private ExperienceSystem _experienceSystem;
        private MockLevelSystem _mockLevelSystem;

        [SetUp]
        public void SetUp()
        {
            _mockLevelSystem = new MockLevelSystem();
            _experienceSystem = new ExperienceSystem(_mockLevelSystem);
        }

        [Test]
        public void Initialize_ShouldSetDefaultValues()
        {
            // Assert
            Assert.AreEqual(0, _experienceSystem.TotalExperience);
            Assert.AreEqual(0, _experienceSystem.LevelExperience);
            Assert.IsNotNull(_experienceSystem.GetRequiredExperience);
        }

        [Test]
        public void AddExperience_WithValidAmount_ShouldUpdateExperience()
        {
            // Act
            _experienceSystem.AddExperience(100);

            // Assert
            Assert.AreEqual(100, _experienceSystem.TotalExperience);
            Assert.AreEqual(100, _experienceSystem.LevelExperience);
        }

        [Test]
        public void AddExperience_WithInvalidAmount_ShouldNotUpdateExperience()
        {
            // Act
            _experienceSystem.AddExperience(-50);

            // Assert
            Assert.AreEqual(0, _experienceSystem.TotalExperience);
            Assert.AreEqual(0, _experienceSystem.LevelExperience);
        }

        [Test]
        public void AddExperience_EnoughForLevelUp_ShouldLevelUp()
        {
            // Arrange
            _mockLevelSystem.SetCurrentLevel(1);
            _mockLevelSystem.SetExperienceToNextLevel(150);

            // Act
            _experienceSystem.AddExperience(200);

            // Assert
            Assert.AreEqual(200, _experienceSystem.TotalExperience);
            Assert.AreEqual(0, _experienceSystem.LevelExperience);
            Assert.AreEqual(2, _mockLevelSystem.GetCurrentLevel());
        }

        [Test]
        public void AddExperience_EnoughForMultipleLevelUps_ShouldLevelUpMultipleTimes()
        {
            // Arrange
            _mockLevelSystem.SetCurrentLevel(1);
            _mockLevelSystem.SetExperienceToNextLevel(100); // 设置较低的升级经验以便测试多次升级

            // Act
            _experienceSystem.AddExperience(350);

            // Assert
            Assert.AreEqual(350, _experienceSystem.TotalExperience);
            Assert.AreEqual(50, _experienceSystem.LevelExperience);
            Assert.AreEqual(4, _mockLevelSystem.GetCurrentLevel()); // 100 + 100 + 100 = 300, 升级3次到4级
        }

        [Test]
        public void CalculateRequiredExperience_WithValidLevel_ShouldReturnCorrectValue()
        {
            // Act
            float requiredExp = _experienceSystem.CalculateRequiredExperience(5);

            // Assert
            Assert.AreEqual(550, requiredExp); // 5 * 100 + 50
        }

        [Test]
        public void CalculateRequiredExperience_WithInvalidLevel_ShouldReturnZero()
        {
            // Act
            float requiredExp = _experienceSystem.CalculateRequiredExperience(0);

            // Assert
            Assert.AreEqual(0, requiredExp);
        }

        [Test]
        public void LevelUp_WhenCanLevelUp_ShouldResetLevelExperience()
        {
            // Arrange
            _mockLevelSystem.SetCanLevelUp(true);
            _experienceSystem.AddExperience(50);

            // Act
            _experienceSystem.LevelUp();

            // Assert
            Assert.AreEqual(0, _experienceSystem.LevelExperience);
        }

        // 模拟等级系统，用于测试
        private class MockLevelSystem : ILevelSystem
        {
            private int _currentLevel = 1;
            private float _experienceToNextLevel = 150;
            private bool _canLevelUp = false;

            public int GetCurrentLevel() => _currentLevel;

            public void SetCurrentLevel(int level) => _currentLevel = level;

            public float GetCurrentExperience() => 0;

            public void AddExperience(float amount) { }

            public bool CanLevelUp() => _canLevelUp;

            public void SetCanLevelUp(bool canLevelUp) => _canLevelUp = canLevelUp;

            public bool LevelUp()
            {
                if (_canLevelUp)
                {
                    _currentLevel++;
                    _canLevelUp = false;
                    return true;
                }
                return false;
            }

            public float GetRequiredExperience(int level) => level * 100 + 50;

            public void SetExperienceToNextLevel(float experience) => _experienceToNextLevel = experience;

            public float[] GetAttributeGrowthRates() => new float[0];
        }
    }
}