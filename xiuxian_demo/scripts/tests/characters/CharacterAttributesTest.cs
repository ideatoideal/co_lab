using Godot;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using XiuxianDemo;

namespace XiuxianDemoTests
{
    [TestFixture]
    public class CharacterAttributesTest
    {
        private CharacterAttributes _attributes;
        private MockLevelSystem _mockLevelSystem;
        private MockExperienceSystem _mockExperienceSystem;

        [SetUp]
        public void SetUp()
        {
            _mockLevelSystem = new MockLevelSystem();
            _mockExperienceSystem = new MockExperienceSystem(_mockLevelSystem);
            _attributes = new CharacterAttributes(_mockLevelSystem, _mockExperienceSystem);
        }

        [Test]
        public void InitializeAttributes_WithValidDefinitions_ShouldInitializeAttributes()
        {
            // Arrange
            var definitions = new List<AttributeDefinition>
            {
                new AttributeDefinition
                {
                    Id = "hp",
                    Name = "气血",
                    Type = AttributeType.Base,
                    BaseValue = 100,
                    GrowthRate = 20,
                    SortOrder = 1
                },
                new AttributeDefinition
                {
                    Id = "mp",
                    Name = "法力",
                    Type = AttributeType.Base,
                    BaseValue = 50,
                    GrowthRate = 10,
                    SortOrder = 2
                }
            };

            // Act
            _attributes.InitializeAttributes(definitions);

            // Assert
            var allAttributes = _attributes.GetAllAttributes();
            Assert.AreEqual(2, allAttributes.Count);
            Assert.AreEqual(100, allAttributes["hp"]);
            Assert.AreEqual(50, allAttributes["mp"]);
        }

        [Test]
        public void GetAttribute_WithExistingAttributeId_ShouldReturnCorrectValue()
        {
            // Arrange
            var definitions = new List<AttributeDefinition>
            {
                new AttributeDefinition
                {
                    Id = "attack",
                    Name = "攻击",
                    Type = AttributeType.Combat,
                    BaseValue = 10,
                    GrowthRate = 2,
                    SortOrder = 1
                }
            };
            _attributes.InitializeAttributes(definitions);
            _mockLevelSystem.SetCurrentLevel(5);

            // Act
            var value = _attributes.GetAttribute("attack", AttributeType.Combat);

            // Assert
            Assert.AreEqual(20, value); // 10 + 5 * 2
        }

        [Test]
        public void SetAttribute_WithExistingAttributeId_ShouldUpdateValue()
        {
            // Arrange
            var definitions = new List<AttributeDefinition>
            {
                new AttributeDefinition
                {
                    Id = "defense",
                    Name = "防御",
                    Type = AttributeType.Combat,
                    BaseValue = 5,
                    GrowthRate = 1,
                    SortOrder = 1
                }
            };
            _attributes.InitializeAttributes(definitions);
            bool attributeChanged = false;
            _attributes.OnAttributeChanged += (id, oldVal, newVal) =>
            {
                attributeChanged = true;
                Assert.AreEqual("defense", id);
                Assert.AreEqual(5, oldVal);
                Assert.AreEqual(15, newVal);
            };

            // Act
            _attributes.SetAttribute("defense", 15);

            // Assert
            Assert.AreEqual(15, _attributes.GetAttribute("defense"));
            Assert.IsTrue(attributeChanged);
        }

        [Test]
        public void AddAttribute_WithExistingAttributeId_ShouldAddValue()
        {
            // Arrange
            var definitions = new List<AttributeDefinition>
            {
                new AttributeDefinition
                {
                    Id = "speed",
                    Name = "速度",
                    Type = AttributeType.Base,
                    BaseValue = 100,
                    GrowthRate = 5,
                    SortOrder = 1
                }
            };
            _attributes.InitializeAttributes(definitions);
            bool attributeChanged = false;
            _attributes.OnAttributeChanged += (id, oldVal, newVal) =>
            {
                attributeChanged = true;
                Assert.AreEqual("speed", id);
                Assert.AreEqual(100, oldVal);
                Assert.AreEqual(120, newVal);
            };

            // Act
            _attributes.AddAttribute("speed", 20);

            // Assert
            Assert.AreEqual(120, _attributes.GetAttribute("speed"));
            Assert.IsTrue(attributeChanged);
        }

        [Test]
        public void GetAttributesByType_WithValidType_ShouldReturnCorrectAttributes()
        {
            // Arrange
            var definitions = new List<AttributeDefinition>
            {
                new AttributeDefinition
                {
                    Id = "hp",
                    Name = "气血",
                    Type = AttributeType.Base,
                    BaseValue = 100,
                    GrowthRate = 20,
                    SortOrder = 1
                },
                new AttributeDefinition
                {
                    Id = "attack",
                    Name = "攻击",
                    Type = AttributeType.Combat,
                    BaseValue = 10,
                    GrowthRate = 2,
                    SortOrder = 2
                },
                new AttributeDefinition
                {
                    Id = "defense",
                    Name = "防御",
                    Type = AttributeType.Combat,
                    BaseValue = 5,
                    GrowthRate = 1,
                    SortOrder = 3
                }
            };
            _attributes.InitializeAttributes(definitions);

            // Act
            var combatAttributes = _attributes.GetAttributesByType(AttributeType.Combat);

            // Assert
            Assert.AreEqual(2, combatAttributes.Count);
            Assert.IsTrue(combatAttributes.ContainsKey("attack"));
            Assert.IsTrue(combatAttributes.ContainsKey("defense"));
        }

        // 模拟等级系统，用于测试
        private class MockLevelSystem : ILevelSystem
        {
            private int _currentLevel = 1;
            private float[] _growthRates = new float[] { 20, 10, 2, 1, 1, 0.5f };

            public int GetCurrentLevel() => _currentLevel;

            public void SetCurrentLevel(int level) => _currentLevel = level;

            public float GetCurrentExperience() => 0;

            public void AddExperience(float amount) { }

            public bool CanLevelUp() => false;

            public bool LevelUp() => false;

            public float GetRequiredExperience(int level) => 100;

            public float[] GetAttributeGrowthRates() => _growthRates;
        }

        // 模拟经验系统，用于测试
        private class MockExperienceSystem : IExperienceSystem
        {
            private ILevelSystem _levelSystem;

            public MockExperienceSystem(ILevelSystem levelSystem)
            {
                _levelSystem = levelSystem;
            }

            public float GetTotalExperience() => 0;

            public float GetLevelExperience() => 0;

            public void AddExperience(float amount) { }

            public float CalculateRequiredExperience(int level) => 100;

            public void LevelUp() { }
        }
    }
}