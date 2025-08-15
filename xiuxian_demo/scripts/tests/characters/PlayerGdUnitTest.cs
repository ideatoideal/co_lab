using GdUnit4;
using static GdUnit4.Assertions;
using Godot;
using System;
using XiuXianDemo.Characters;

namespace XiuXianDemo.Tests.Characters
{
    [TestSuite]
    public class PlayerGdUnitTest
    {
        private Player _player;
        private CharacterAttributes _mockAttributes;

        [BeforeTest]
        public void Setup()
        {
            // 注意：由于Player继承自CharacterBody2D，在单元测试中需要特殊处理
            // 这里我们创建一个简化版本的测试对象
            _player = new Player();
            _mockAttributes = new CharacterAttributes();
            
            // 使用反射或直接设置内部属性
            typeof(Player).GetField("_attributes", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(_player, _mockAttributes);
        }

        [TestCase]
        [RequireGodotRuntime]
        public void TestInitialCharacterName()
        {
            // 测试默认角色名称
            AssertString(_player.CharacterName).IsEqual("Player");
        }

        [TestCase]
        [RequireGodotRuntime]
        public void TestInitialSpeed()
        {
            // 测试默认速度
            AssertFloat(_player.Speed).IsEqual(300.0f);
        }

        [TestCase]
        [RequireGodotRuntime]
        public void TestCharacterNameSetter()
        {
            // 测试设置角色名称
            _player.CharacterName = "TestPlayer";
            AssertString(_player.CharacterName).IsEqual("TestPlayer");
        }

        [TestCase]
        [RequireGodotRuntime]
        public void TestSpeedSetter()
        {
            // 测试设置速度
            _player.Speed = 500.0f;
            AssertFloat(_player.Speed).IsEqual(500.0f);
        }

        [TestCase]
        [RequireGodotRuntime]
        public void TestAttributesNotNull()
        {
            // 测试属性系统不为null
            AssertObject(_player.Attributes).IsNotNull();
        }

        [TestCase]
        [RequireGodotRuntime]
        public void TestAddExperience()
        {
            // 测试添加经验值
            _player.AddExperience(100.0f);
            
            // 由于Player类委托给CharacterAttributes，这里验证不抛出异常
            // 更详细的测试应在CharacterAttributes或ExperienceSystem中
            AssertBool(true).IsTrue(); // 占位断言
        }

        [TestCase]
        [RequireGodotRuntime]
        public void TestTakeDamage()
        {
            // 测试受到伤害
            var attributes = _player.Attributes;
            var attributeDefinitions = new System.Collections.Generic.List<AttributeDefinition>
            {
                new AttributeDefinition("hp", "生命值", AttributeType.Base, 100, 0, "", "", 1)
            };
            attributes.InitializeAttributes(attributeDefinitions);

            _player.TakeDamage(20.0f);
            
            // 验证生命值减少 - 100 - 20 = 80
            float currentHp = (float)attributes.GetAttribute("hp");
            AssertFloat(currentHp).IsEqual(80.0f);
        }

        [TestCase]
        [RequireGodotRuntime]
        public void TestTakeDamageExceedsHealth()
        {
            // 测试伤害超过生命值
            var attributes = _player.Attributes;
            var attributeDefinitions = new System.Collections.Generic.List<AttributeDefinition>
            {
                new AttributeDefinition("hp", "生命值", AttributeType.Base, 100, 0, "", "", 1)
            };
            attributes.InitializeAttributes(attributeDefinitions);

            _player.TakeDamage(150.0f);
            
            // 验证生命值不会低于0
            float currentHp = (float)attributes.GetAttribute("hp");
            AssertFloat(currentHp).IsEqual(0.0f);
        }

        [TestCase]
        [RequireGodotRuntime]
        public void TestHeal()
        {
            // 测试治疗
            var attributes = _player.Attributes;
            var attributeDefinitions = new System.Collections.Generic.List<AttributeDefinition>
            {
                new AttributeDefinition("hp", "生命值", AttributeType.Base, 100, 0, "", "", 1)
            };
            attributes.InitializeAttributes(attributeDefinitions);

            // 先受伤再治疗 - 初始值为100，受伤到30
            attributes.SetAttribute("hp", 30.0f);
            _player.Heal(20.0f); // 治疗20点
            
            // 验证生命值增加 - 30 + 20 = 50
            float currentHp = (float)attributes.GetAttribute("hp");
            AssertFloat(currentHp).IsEqual(50.0f);
        }

        [TestCase]
        [RequireGodotRuntime]
        public void TestHealExceedsMaxHealth()
        {
            // 测试治疗超过最大生命值
            var attributes = _player.Attributes;
            var attributeDefinitions = new System.Collections.Generic.List<AttributeDefinition>
            {
                new AttributeDefinition("hp", "生命值", AttributeType.Base, 100, 0, "", "", 1)
            };
            attributes.InitializeAttributes(attributeDefinitions);

            // 轻微受伤 - 当前90，最大100
            attributes.SetAttribute("hp", 90.0f);
            _player.Heal(15.0f); // 治疗15点，应该到100
            
            // 验证生命值不会超过最大值
            float currentHp = (float)attributes.GetAttribute("hp");
            AssertFloat(currentHp).IsEqual(100.0f);
        }

        [TestCase]
        [RequireGodotRuntime]
        public void TestRestoreMana()
        {
            // 测试恢复魔法值
            var attributes = _player.Attributes;
            var attributeDefinitions = new System.Collections.Generic.List<AttributeDefinition>
            {
                new AttributeDefinition("mp", "魔法值", AttributeType.Base, 50, 0, "", "", 1)
            };
            attributes.InitializeAttributes(attributeDefinitions);

            // 先消耗再恢复 - 初始值为50，消耗到5
            attributes.SetAttribute("mp", 5.0f);
            _player.RestoreMana(15.0f); // 恢复15点
            
            // 验证魔法值增加 - 5 + 15 = 20
            float currentMp = (float)attributes.GetAttribute("mp");
            AssertFloat(currentMp).IsEqual(20.0f);
        }

        [TestCase]
        [RequireGodotRuntime]
        public void TestRestoreManaExceedsMaxMana()
        {
            // 测试恢复魔法值超过最大值
            var attributes = _player.Attributes;
            var attributeDefinitions = new System.Collections.Generic.List<AttributeDefinition>
            {
                new AttributeDefinition("mp", "魔法值", AttributeType.Base, 50, 0, "", "", 1)
            };
            attributes.InitializeAttributes(attributeDefinitions);

            // 轻微消耗 - 当前45，最大50
            attributes.SetAttribute("mp", 45.0f);
            _player.RestoreMana(10.0f); // 恢复10点，但只能恢复5点到最大值
            
            // 验证魔法值不会超过最大值
            float currentMp = (float)attributes.GetAttribute("mp");
            AssertFloat(currentMp).IsEqual(50.0f);
        }

        [TestCase]
        [RequireGodotRuntime]
        public void TestTakeDamageWithNullAttributes()
        {
            // 测试属性系统为null时的处理
            var player = new Player();
            
            // 应该能安全处理null情况
            player.TakeDamage(10.0f);
            
            AssertBool(true).IsTrue(); // 占位断言，验证不抛出异常
        }

        [TestCase]
        [RequireGodotRuntime]
        public void TestHealWithNullAttributes()
        {
            // 测试属性系统为null时的治疗
            var player = new Player();
            
            // 应该能安全处理null情况
            player.Heal(10.0f);
            
            AssertBool(true).IsTrue(); // 占位断言
        }

        [TestCase]
        [RequireGodotRuntime]
        public void TestRestoreManaWithNullAttributes()
        {
            // 测试属性系统为null时的恢复魔法值
            var player = new Player();
            
            // 应该能安全处理null情况
            player.RestoreMana(10.0f);
            
            AssertBool(true).IsTrue(); // 占位断言
        }

        [TestCase]
        [RequireGodotRuntime]
        public void TestAddExperienceWithNullAttributes()
        {
            // 测试属性系统为null时的添加经验
            var player = new Player();
            
            // 应该能安全处理null情况
            player.AddExperience(100.0f);
            
            AssertBool(true).IsTrue(); // 占位断言
        }
    }
}