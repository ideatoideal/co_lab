using System;

namespace XiuXianDemo.Characters
{
    /// <summary>
    /// 属性定义类，定义属性的基本信息
    /// </summary>
    [Serializable]
    public class AttributeDefinition
    {
        /// <summary>
        /// 属性ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 属性类型
        /// </summary>
        public AttributeType Type { get; set; }

        /// <summary>
        /// 基础值
        /// </summary>
        public float BaseValue { get; set; }

        /// <summary>
        /// 成长率
        /// </summary>
        public float GrowthRate { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 图标路径
        /// </summary>
        public string IconPath { get; set; }

        /// <summary>
        /// 排序顺序
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public AttributeDefinition()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">属性ID</param>
        /// <param name="name">属性名称</param>
        /// <param name="type">属性类型</param>
        /// <param name="baseValue">基础值</param>
        /// <param name="growthRate">成长率</param>
        /// <param name="description">描述</param>
        /// <param name="iconPath">图标路径</param>
        /// <param name="sortOrder">排序顺序</param>
        public AttributeDefinition(string id, string name, AttributeType type, float baseValue, float growthRate, string description, string iconPath, int sortOrder)
        {
            Id = id;
            Name = name;
            Type = type;
            BaseValue = baseValue;
            GrowthRate = growthRate;
            Description = description;
            IconPath = iconPath;
            SortOrder = sortOrder;
        }

        /// <summary>
        /// 返回属性的字符串表示
        /// </summary>
        public override string ToString()
        {
            return $"AttributeDefinition[Id={Id}, Name={Name}, Type={Type}, BaseValue={BaseValue}, GrowthRate={GrowthRate}]";
        }
    }
}