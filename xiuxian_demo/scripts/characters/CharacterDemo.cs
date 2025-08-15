using Godot;
using System;
using System.Collections.Generic;
using XiuXianDemo.Common;

namespace XiuXianDemo.Characters
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

            // 从CSV配置文件读取属性定义
            List<AttributeDefinition> attributeDefinitions = LoadAttributeDefinitionsFromCsv();

            // 初始化属性
            _characterAttributes.InitializeAttributes(attributeDefinitions);
        }

        /// <summary>
        /// 从CSV配置文件加载属性定义
        /// </summary>
        /// <returns>属性定义列表</returns>
        private List<AttributeDefinition> LoadAttributeDefinitionsFromCsv()
        {
            string csvPath = "res://assets/configs/AttributeConfig.csv";

            // 使用ConfigManager的通用CSV加载方法
            return ConfigManager.Instance.LoadConfig<AttributeDefinition>(csvPath, (headers, values) =>
            {
                if (values.Length < 7)
                {
                    GD.PrintErr("CSV行格式不正确: " + string.Join(',', values));
                    return null;
                }

                // 创建属性定义
                return new AttributeDefinition
                {
                    Id = values[0].Trim(),
                    Name = values[1].Trim(),
                    Type = values[2].Trim() == "Base" ? AttributeType.Base : AttributeType.Combat,
                    BaseValue = float.Parse(values[3].Trim()),
                    GrowthRate = float.Parse(values[4].Trim()),
                    Description = values[5].Trim(),
                    IconPath = values[6].Trim(),
                    SortOrder = int.Parse(values[7].Trim())
                };
            });
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
            GD.Print("设置攻击为 25");
            _characterAttributes.SetAttribute("attack", 25);

            // 添加属性值
            GD.Print("增加防御 10");
            _characterAttributes.AddAttribute("defense", 10, AttributeType.Combat);
        }

        /// <summary>
        /// 演示查询属性
        /// </summary>
        private void QueryAttributesDemo()
        {
            GD.Print("\n=== 查询属性 ===");

            // 查询单个属性
            object attack = _characterAttributes.GetAttribute("attack");
            GD.Print($"攻击: {attack}");

            object defense = _characterAttributes.GetAttribute("defense", AttributeType.Combat);
            GD.Print($"防御: {defense}");

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