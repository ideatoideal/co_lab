using GdUnit4;
using static GdUnit4.Assertions;
using Godot;
using System;
using System.Collections.Generic;

namespace XiuxianDemo.Tests.Characters
{
    [TestSuite]
    public class CharacterAttributesGdUnitTest
    {
        private CharacterAttributes _characterAttributes;
        private List<AttributeDefinition> _testAttributes;
        private bool _attributeChangedEventFired;
        private bool _systemInitializedEventFired;

        [BeforeTest]
        public void Setup()
        {
            // 创建测试对象
            _characterAttributes = new CharacterAttributes();

            // 初始化测试数据
            _testAttributes = new List<AttributeDefinition>
            {
                new AttributeDefinition
                {
                    Id = "strength",
                    Name = "力量",
                    Type = AttributeType.Base,
                    BaseValue = 10,
                    GrowthRate = 0.5f,
                    Description = "影响物理攻击",
                    IconPath = "res://icons/strength.png",
                    SortOrder = 1
                },
                new AttributeDefinition
                {
                    Id = "intelligence",
                    Name = "智力",
                    Type = AttributeType.Base,
                    BaseValue = 8,
                    GrowthRate = 0.6f,
                    Description = "影响法术攻击",
                    IconPath = "res://icons/intelligence.png",
                    SortOrder = 2
                }
            };

            // 注册事件
            _characterAttributes.OnAttributeChanged += OnAttributeChanged;
            _characterAttributes.OnSystemInitialized += OnSystemInitialized;

            // 重置事件标志
            _attributeChangedEventFired = false;
            _systemInitializedEventFired = false;
        }

        [TestCase]
        public void TestInitializeAttributes()
        {
            // 执行初始化
            _characterAttributes.InitializeAttributes(_testAttributes);

            // 验证事件是否触发
            AssertBool(_systemInitializedEventFired).IsTrue();

            // 验证属性是否正确初始化
            int strengthValue = (int)_characterAttributes.GetAttribute("strength");
            AssertInt(strengthValue).IsEqual(10);

            int intelligenceValue = (int)_characterAttributes.GetAttribute("intelligence");
            AssertInt(intelligenceValue).IsEqual(8);
        }

        [TestCase]
        public void TestSetAttribute()
        {
            // 初始化属性
            _characterAttributes.InitializeAttributes(_testAttributes);

            // 修改属性值
            _characterAttributes.SetAttribute("strength", 15);

            // 验证修改是否生效
            int strengthValue = (int)_characterAttributes.GetAttribute("strength");
            AssertInt(strengthValue).IsEqual(15);

            // 验证事件是否触发
            AssertBool(_attributeChangedEventFired).IsTrue();
        }

        [TestCase]
        public void TestAddAttribute()
        {
            // 初始化属性
            _characterAttributes.InitializeAttributes(_testAttributes);

            // 添加属性值
            _characterAttributes.AddAttribute("strength", 5);

            // 验证添加是否生效
            int strengthValue = (int)_characterAttributes.GetAttribute("strength");
            AssertInt(strengthValue).IsEqual(15);
        }

        [TestCase]
        public void TestGetAttributeByType()
        {
            // 初始化属性
            _characterAttributes.InitializeAttributes(_testAttributes);

            // 按类型获取属性
            var baseAttributes = _characterAttributes.GetAttributesByType(AttributeType.Base);

            // 验证结果
            AssertThat(baseAttributes.Count).IsEqual(2);
            AssertThat(baseAttributes.ContainsKey("strength")).IsTrue();
            AssertThat(baseAttributes.ContainsKey("intelligence")).IsTrue();
        }

        [TestCase]
        public void TestGetAllAttributes()
        {
            // 初始化属性
            _characterAttributes.InitializeAttributes(_testAttributes);

            // 获取所有属性
            var allAttributes = _characterAttributes.GetAllAttributes();

            // 验证结果
            AssertThat(allAttributes.Count).IsEqual(2);
            AssertThat(allAttributes.ContainsKey("strength")).IsTrue();
            AssertThat(allAttributes.ContainsKey("intelligence")).IsTrue();
        }

        [TestCase]
        public void TestGetNonExistentAttribute()
        {
            // 尝试获取不存在的属性
            var nonExistentAttribute = _characterAttributes.GetAttribute("nonexistent");

            // 验证结果
            AssertThat(nonExistentAttribute).IsNull();
        }

        private void OnAttributeChanged(string attributeId, object oldValue, object newValue)
        {
            _attributeChangedEventFired = true;
            GD.Print($"属性变更: {attributeId}, 旧值: {oldValue}, 新值: {newValue}");
        }

        private void OnSystemInitialized()
        {
            _systemInitializedEventFired = true;
            GD.Print("属性系统初始化完成");
        }
    }
}