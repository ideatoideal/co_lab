using Godot;
using System;
using System.Collections.Generic;

namespace XiuxianDemo
{
    /// <summary>
    /// 角色属性系统演示类
    /// </summary>
    public partial class CharacterDemo : Node
    {
        private CharacterAttributes _characterAttributes;

        public override void _Ready()
        {
            // 初始化角色属性系统
            InitializeCharacterAttributes();

            // 演示添加经验
            AddExperienceDemo(150);

            // 演示修改属性
            ModifyAttributesDemo();

            // 演示查询属性
            QueryAttributesDemo();
        }

        /// <summary>
        /// 初始化角色属性系统
        /// </summary>
        private void InitializeCharacterAttributes()
        {
            GD.Print("=== 初始化角色属性系统 ===");

            // 创建角色属性实例
            _characterAttributes = new CharacterAttributes();

            // 注册事件监听
            _characterAttributes.OnLevelUp += OnLevelUp;
            _characterAttributes.OnAttributeChanged += OnAttributeChanged;
            _characterAttributes.OnSystemInitialized += OnSystemInitialized;

            // 定义属性列表
            List<AttributeDefinition> attributeDefinitions = new List<AttributeDefinition>
            {
                new AttributeDefinition
                {
                    Id = "strength",
                    Name = "力量",
                    Type = AttributeType.Base,
                    BaseValue = 10,
                    GrowthRate = 2.5f,
                    SortOrder = 1
                },
                new AttributeDefinition
                {
                    Id = "agility",
                    Name = "敏捷",
                    Type = AttributeType.Base,
                    BaseValue = 8,
                    GrowthRate = 2.0f,
                    SortOrder = 2
                },
                new AttributeDefinition
                {
                    Id = "intelligence",
                    Name = "智力",
                    Type = AttributeType.Base,
                    BaseValue = 6,
                    GrowthRate = 1.5f,
                    SortOrder = 3
                },
                new AttributeDefinition
                {
                    Id = "health",
                    Name = "生命值",
                    Type = AttributeType.Combat,
                    BaseValue = 100,
                    GrowthRate = 15.0f,
                    SortOrder = 4
                },
                new AttributeDefinition
                {
                    Id = "damage",
                    Name = "攻击力",
                    Type = AttributeType.Combat,
                    BaseValue = 20,
                    GrowthRate = 5.0f,
                    SortOrder = 5
                }
            };

            // 初始化属性
            _characterAttributes.InitializeAttributes(attributeDefinitions);
        }

        /// <summary>
        /// 演示添加经验
        /// </summary>
        /// <param name="amount">经验值</param>
        private void AddExperienceDemo(float amount)
        {
            GD.Print($"\n=== 添加经验: {amount} ===");
            GD.Print($"当前等级: {_characterAttributes.GetCurrentLevel()}");

            _characterAttributes.AddExperience(amount);

            GD.Print($"添加经验后等级: {_characterAttributes.GetCurrentLevel()}");
        }

        /// <summary>
        /// 演示修改属性
        /// </summary>
        private void ModifyAttributesDemo()
        {
            GD.Print("\n=== 修改属性 ===");

            // 设置属性值
            GD.Print("设置力量为 25");
            _characterAttributes.SetAttribute("strength", 25);

            // 添加属性值
            GD.Print("增加攻击力 10");
            _characterAttributes.AddAttribute("damage", 10, AttributeType.Combat);
        }

        /// <summary>
        /// 演示查询属性
        /// </summary>
        private void QueryAttributesDemo()
        {
            GD.Print("\n=== 查询属性 ===");

            // 查询单个属性
            object strength = _characterAttributes.GetAttribute("strength");
            GD.Print($"力量: {strength}");

            object damage = _characterAttributes.GetAttribute("damage", AttributeType.Combat);
            GD.Print($"攻击力: {damage}");

            // 查询所有属性
            GD.Print("\n所有属性:");
            Dictionary<string, object> allAttributes = _characterAttributes.GetAllAttributes();
            foreach (var kvp in allAttributes)
            {
                GD.Print($"{kvp.Key}: {kvp.Value}");
            }

            // 按类型查询属性
            GD.Print("\n战斗属性:");
            Dictionary<string, object> battleAttributes = _characterAttributes.GetAttributesByType(AttributeType.Combat);
            foreach (var kvp in battleAttributes)
            {
                GD.Print($"{kvp.Key}: {kvp.Value}");
            }
        }

        /// <summary>
        /// 等级提升事件处理
        /// </summary>
        /// <param name="newLevel">新等级</param>
        /// <param name="attributeChanges">属性变化</param>
        private void OnLevelUp(int newLevel, Dictionary<string, object> attributeChanges)
        {
            GD.Print($"\n=== 等级提升! 新等级: {newLevel} ===");
            GD.Print("属性变化:");
            foreach (var kvp in attributeChanges)
            {
                GD.Print($"{kvp.Key}: {kvp.Value}");
            }
        }

        /// <summary>
        /// 属性变化事件处理
        /// </summary>
        /// <param name="attributeId">属性ID</param>
        /// <param name="oldValue">旧值</param>
        /// <param name="newValue">新值</param>
        private void OnAttributeChanged(string attributeId, object oldValue, object newValue)
        {
            GD.Print($"属性变化: {attributeId} 从 {oldValue} 变为 {newValue}");
        }

        /// <summary>
        /// 系统初始化完成事件处理
        /// </summary>
        private void OnSystemInitialized()
        {
            GD.Print("角色属性系统初始化完成");
        }
    }
}