using System.Collections.Generic;

namespace XiuxianDemo
{
    /// <summary>
    /// 属性管理接口，定义属性操作的基本方法
    /// </summary>
    public interface IAttributeManager
    {
        // 获取属性值
        object GetAttribute(string attributeId);
        object GetAttribute(string attributeId, AttributeType type);

        // 设置属性值
        void SetAttribute(string attributeId, object value);
        void SetAttribute(string attributeId, object value, AttributeType type);

        // 添加属性值
        void AddAttribute(string attributeId, object value);
        void AddAttribute(string attributeId, object value, AttributeType type);

        // 初始化属性
        void InitializeAttributes(List<AttributeDefinition> definitions);

        // 获取所有属性
        Dictionary<string, object> GetAllAttributes();
        Dictionary<string, object> GetAttributesByType(AttributeType type);
    }
}