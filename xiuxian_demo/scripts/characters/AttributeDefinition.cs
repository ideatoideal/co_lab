using System;

namespace XiuxianDemo
{
    /// <summary>
    /// 属性定义类，用于存储属性的基本信息
    /// </summary>
    [Serializable]
    public class AttributeDefinition
    {
        public string Id;              // 属性唯一标识
        public string Name;            // 属性名称
        public AttributeType Type;     // 属性类型
        public object BaseValue;       // 基础值（支持多种数据类型）
        public float GrowthRate;       // 成长率
        public string Description;     // 属性描述
        public string IconPath;        // 图标路径
        public int SortOrder;          // 排序顺序
    }
}