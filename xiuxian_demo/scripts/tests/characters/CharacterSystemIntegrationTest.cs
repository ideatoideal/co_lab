using Godot;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using XiuxianDemo;

namespace XiuxianDemoTests
{
    [TestFixture]
    public class CharacterSystemIntegrationTest
    {
        private CharacterAttributes _attributes;
        private CharacterLevel _levelSystem;
        private ExperienceSystem _experienceSystem;

        [SetUp]
        public void SetUp()
        {
            _levelSystem = new CharacterLevel();
            _experienceSystem = new ExperienceSystem(_levelSystem);
            _attributes = new CharacterAttributes(_levelSystem, _experienceSystem);

            // 初始化属性
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
                },
                new AttributeDefinition
                {
                    Id = "attack",
                    Name = "攻击",
                    Type = AttributeType.Combat,
                    BaseValue = 10,
                    GrowthRate = 2,
                    SortOrder = 3
                }
            };
            _attributes.InitializeAttributes(definitions);
        }

        [Test]
        public void AddExperience_EnoughForLevelUp_ShouldUpdateAttributes()
        {
            // Arrange
            int initialLevel = _levelSystem.CurrentLevel;
            int initialHp = (int)_attributes.GetAttribute("hp");
            int initialMp = (int)_attributes.GetAttribute("mp");
            int initialAttack = (int)_attributes.GetAttribute("attack", AttributeType.Combat);

            // 确保有足够经验升级
            float requiredExp = _levelSystem.GetRequiredExperience(initialLevel);
            bool levelUpEventFired = false;
            Dictionary<string, object> newAttributes = null;

            _attributes.OnLevelUp += (level, attributes) =>
            {
                levelUpEventFired = true;
                newAttributes = attributes;
            };

            // Act
            _experienceSystem.AddExperience(requiredExp + 50); // 多添加50经验

            // Assert
            Assert.AreEqual(initialLevel + 1, _levelSystem.CurrentLevel);
            Assert.AreEqual(initialHp + 20, _attributes.GetAttribute("hp")); // 基础值100 + 20/级 * 1级
            Assert.AreEqual(initialMp + 10, _attributes.GetAttribute("mp")); // 基础值50 + 10/级 * 1级
            Assert.AreEqual(initialAttack + 2, _attributes.GetAttribute("attack", AttributeType.Combat)); // 基础值10 + 2/级 * 1级
            Assert.IsTrue(levelUpEventFired);
            Assert.IsNotNull(newAttributes);
            Assert.IsTrue(newAttributes.ContainsKey("hp"));
            Assert.IsTrue(newAttributes.ContainsKey("mp"));
        }

        [Test]
        public void LevelUp_MultipleTimes_ShouldUpdateAttributesCorrectly()
        {
            // Arrange
            _levelSystem.SetCurrentLevel(1);
            int initialLevel = _levelSystem.CurrentLevel;
            int targetLevel = 5;

            // 计算升级到目标等级所需的总经验
            float totalExpNeeded = 0;
            for (int i = initialLevel; i < targetLevel; i++)
            {
                totalExpNeeded += _levelSystem.GetRequiredExperience(i);
            }

            // Act
            _experienceSystem.AddExperience(totalExpNeeded);

            // Assert
            Assert.AreEqual(targetLevel, _levelSystem.CurrentLevel);
            Assert.AreEqual(100 + 20 * (targetLevel - 1), _attributes.GetAttribute("hp"));
            Assert.AreEqual(50 + 10 * (targetLevel - 1), _attributes.GetAttribute("mp"));
            Assert.AreEqual(10 + 2 * (targetLevel - 1), _attributes.GetAttribute("attack", AttributeType.Combat));
        }

        [Test]
        public void AttributeChangeEvent_ShouldFireWhenAttributeIsModified()
        {
            // Arrange
            bool eventFired = false;
            string changedAttributeId = null;
            object oldValue = null;
            object newValue = null;

            _attributes.OnAttributeChanged += (id, oldVal, newVal) =>
            {
                eventFired = true;
                changedAttributeId = id;
                oldValue = oldVal;
                newValue = newVal;
            };

            // Act
            _attributes.SetAttribute("hp", 200);

            // Assert
            Assert.IsTrue(eventFired);
            Assert.AreEqual("hp", changedAttributeId);
            Assert.AreEqual(100, oldValue);
            Assert.AreEqual(200, newValue);
        }

        [Test]
        public void SystemInitializedEvent_ShouldFireAfterInitialization()
        {
            // Arrange
            bool eventFired = false;
            var newAttributes = new CharacterAttributes();

            newAttributes.OnSystemInitialized += () =>
            {
                eventFired = true;
            };

            // Act
            var definitions = new List<AttributeDefinition>
            {
                new AttributeDefinition { Id = "test", BaseValue = 100, Type = AttributeType.Base }
            };
            newAttributes.InitializeAttributes(definitions);

            // Assert
            Assert.IsTrue(eventFired);
        }
    }
}