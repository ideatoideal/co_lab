using System;
using System.Collections.Generic;

namespace XiuXianDemo.Characters
{
    /// <summary>
    /// 属性管理器接口，定义角色属性的基本操作
    /// </summary>
    public interface IAttributeManager
    {
        /// <summary>
        /// 获取指定属性的值
        /// </summary>
        /// <param name="attributeId">属性ID</param>
        /// <returns>属性值</returns>
        object GetAttribute(string attributeId);

        /// <summary>
        /// 获取指定类型属性的值
        /// </summary>
        /// <param name="attributeId">属性ID</param>
        /// <param name="type">属性类型</param>
        /// <returns>属性值</returns>
        object GetAttribute(string attributeId, AttributeType type);

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="attributeId">属性ID</param>
        /// <param name="value">属性值</param>
        void SetAttribute(string attributeId, object value);

        /// <summary>
        /// 设置指定类型属性的值
        /// </summary>
        /// <param name="attributeId">属性ID</param>
        /// <param name="value">属性值</param>
        /// <param name="type">属性类型</param>
        void SetAttribute(string attributeId, object value, AttributeType type);

        /// <summary>
        /// 增加属性值
        /// </summary>
        /// <param name="attributeId">属性ID</param>
        /// <param name="value">增加值</param>
        void AddAttribute(string attributeId, object value);

        /// <summary>
        /// 增加指定类型属性的值
        /// </summary>
        /// <param name="attributeId">属性ID</param>
        /// <param name="value">增加值</param>
        /// <param name="type">属性类型</param>
        void AddAttribute(string attributeId, object value, AttributeType type);

        /// <summary>
        /// 获取所有属性
        /// </summary>
        /// <returns>所有属性的字典</returns>
        Dictionary<string, object> GetAllAttributes();

        /// <summary>
        /// 按类型获取属性
        /// </summary>
        /// <param name="type">属性类型</param>
        /// <returns>指定类型的属性字典</returns>
        Dictionary<string, object> GetAttributesByType(AttributeType type);

        /// <summary>
        /// 初始化属性系统
        /// </summary>
        /// <param name="definitions">属性定义列表</param>
        void InitializeAttributes(List<AttributeDefinition> definitions);

        /// <summary>
        /// 获取当前等级
        /// </summary>
        /// <returns>当前等级</returns>
        int GetCurrentLevel();

        /// <summary>
        /// 添加经验值
        /// </summary>
        /// <param name="amount">经验值</param>
        void AddExperience(float amount);

        /// <summary>
        /// 等级提升事件
        /// </summary>
        event Action<int, Dictionary<string, object>> OnLevelUp;

        /// <summary>
        /// 属性变化事件
        /// </summary>
        event Action<string, object, object> OnAttributeChanged;

        /// <summary>
        /// 系统初始化完成事件
        /// </summary>
        event Action OnSystemInitialized;
    }
}