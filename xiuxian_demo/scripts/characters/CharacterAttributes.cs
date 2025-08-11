using Godot;
using System;
using System.Collections.Generic;

namespace XiuxianDemo
{
    /// <summary>
    /// 角色属性系统核心类，管理角色的所有属性
    /// </summary>
    public class CharacterAttributes : IAttributeManager
    {
        private Dictionary<string, object> _attributes = new Dictionary<string, object>();
        private List<AttributeDefinition> _attributeDefinitions = new List<AttributeDefinition>();
        private ILevelSystem _levelSystem;
        private IExperienceSystem _experienceSystem;

        // 事件定义
        public event Action<int, Dictionary<string, object>> OnLevelUp;
        public event Action<string, object, object> OnAttributeChanged;
        public event Action OnSystemInitialized;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CharacterAttributes()
        {
            _levelSystem = new CharacterLevel();
            _experienceSystem = new ExperienceSystem(_levelSystem);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="levelSystem">等级系统</param>
        /// <param name="experienceSystem">经验系统</param>
        public CharacterAttributes(ILevelSystem levelSystem, IExperienceSystem experienceSystem)
        {
            _levelSystem = levelSystem;
            _experienceSystem = experienceSystem;
        }

        // 获取属性值
        public object GetAttribute(string attributeId)
        {
            if (string.IsNullOrEmpty(attributeId))
            {
                GD.PrintErr("属性ID不能为空");
                return null;
            }

            // 检查缓存
            if (_attributes.TryGetValue(attributeId, out object value))
            {
                return value;
            }

            // 查找属性定义
            AttributeDefinition definition = _attributeDefinitions.Find(ad => ad.Id == attributeId);
            if (definition == null)
            {
                GD.PrintErr($"未找到属性定义: {attributeId}");
                return null;
            }

            // 计算属性值
            value = CalculateAttributeValue(definition);
            _attributes[attributeId] = value;
            return value;
        }

        // 获取属性值(按类型)
        public object GetAttribute(string attributeId, AttributeType type)
        {
            if (string.IsNullOrEmpty(attributeId))
            {
                GD.PrintErr("属性ID不能为空");
                return null;
            }

            // 查找属性定义
            AttributeDefinition definition = _attributeDefinitions.Find(ad => ad.Id == attributeId && ad.Type == type);
            if (definition == null)
            {
                GD.PrintErr($"未找到类型为{type}的属性定义: {attributeId}");
                return null;
            }

            // 检查缓存
            string cacheKey = $"{attributeId}_{type}";
            if (_attributes.TryGetValue(cacheKey, out object value))
            {
                return value;
            }

            // 计算属性值
            value = CalculateAttributeValue(definition);
            _attributes[cacheKey] = value;
            return value;
        }

        // 设置属性值
        public void SetAttribute(string attributeId, object value)
        {
            if (string.IsNullOrEmpty(attributeId))
            {
                GD.PrintErr("属性ID不能为空");
                return;
            }

            object oldValue = null;
            if (_attributes.TryGetValue(attributeId, out oldValue))
            {
                _attributes[attributeId] = value;
                OnAttributeChanged?.Invoke(attributeId, oldValue, value);
            }
            else
            {
                // 查找属性定义
                AttributeDefinition definition = _attributeDefinitions.Find(ad => ad.Id == attributeId);
                if (definition != null)
                {
                    _attributes[attributeId] = value;
                    OnAttributeChanged?.Invoke(attributeId, null, value);
                }
                else
                {
                    GD.PrintErr($"未找到属性定义: {attributeId}");
                }
            }
        }

        // 设置属性值(按类型)
        public void SetAttribute(string attributeId, object value, AttributeType type)
        {
            if (string.IsNullOrEmpty(attributeId))
            {
                GD.PrintErr("属性ID不能为空");
                return;
            }

            string cacheKey = $"{attributeId}_{type}";
            object oldValue = null;
            if (_attributes.TryGetValue(cacheKey, out oldValue))
            {
                _attributes[cacheKey] = value;
                OnAttributeChanged?.Invoke(cacheKey, oldValue, value);
            }
            else
            {
                // 查找属性定义
                AttributeDefinition definition = _attributeDefinitions.Find(ad => ad.Id == attributeId && ad.Type == type);
                if (definition != null)
                {
                    _attributes[cacheKey] = value;
                    OnAttributeChanged?.Invoke(cacheKey, null, value);
                }
                else
                {
                    GD.PrintErr($"未找到类型为{type}的属性定义: {attributeId}");
                }
            }
        }

        // 添加属性值
        public void AddAttribute(string attributeId, object value)
        {
            if (string.IsNullOrEmpty(attributeId))
            {
                GD.PrintErr("属性ID不能为空");
                return;
            }

            // 查找属性定义
            AttributeDefinition definition = _attributeDefinitions.Find(ad => ad.Id == attributeId);
            if (definition == null)
            {
                GD.PrintErr($"未找到属性定义: {attributeId}");
                return;
            }

            // 检查缓存
            if (!_attributes.TryGetValue(attributeId, out object currentValue))
            {
                // 计算当前属性值
                currentValue = CalculateAttributeValue(definition);
            }

            // 尝试进行数值相加
            try
            {
                object newValue = null;
                if (currentValue is int intValue && value is int intAdd)
                {
                    newValue = intValue + intAdd;
                }
                else if (currentValue is float floatValue && value is float floatAdd)
                {
                    newValue = floatValue + floatAdd;
                }
                else if (currentValue is double doubleValue && value is double doubleAdd)
                {
                    newValue = doubleValue + doubleAdd;
                }
                else
                {
                    // 如果不能相加，则直接设置
                    newValue = value;
                    GD.Print($"属性{attributeId}无法进行加法运算，直接设置为新值");
                }

                _attributes[attributeId] = newValue;
                OnAttributeChanged?.Invoke(attributeId, currentValue, newValue);
            }
            catch (Exception e)
            {
                GD.PrintErr($"属性{attributeId}添加值失败: {e.Message}");
            }
        }

        // 添加属性值(按类型)
        public void AddAttribute(string attributeId, object value, AttributeType type)
        {
            if (string.IsNullOrEmpty(attributeId))
            {
                GD.PrintErr("属性ID不能为空");
                return;
            }

            string cacheKey = $"{attributeId}_{type}";

            // 查找属性定义
            AttributeDefinition definition = _attributeDefinitions.Find(ad => ad.Id == attributeId && ad.Type == type);
            if (definition == null)
            {
                GD.PrintErr($"未找到类型为{type}的属性定义: {attributeId}");
                return;
            }

            // 检查缓存
            if (!_attributes.TryGetValue(cacheKey, out object currentValue))
            {
                // 计算当前属性值
                currentValue = CalculateAttributeValue(definition);
            }

            // 尝试进行数值相加
            try
            {
                object newValue = null;
                if (currentValue is int intValue && value is int intAdd)
                {
                    newValue = intValue + intAdd;
                }
                else if (currentValue is float floatValue && value is float floatAdd)
                {
                    newValue = floatValue + floatAdd;
                }
                else if (currentValue is double doubleValue && value is double doubleAdd)
                {
                    newValue = doubleValue + doubleAdd;
                }
                else
                {
                    // 如果不能相加，则直接设置
                    newValue = value;
                    GD.Print($"属性{cacheKey}无法进行加法运算，直接设置为新值");
                }

                _attributes[cacheKey] = newValue;
                OnAttributeChanged?.Invoke(cacheKey, currentValue, newValue);
            }
            catch (Exception e)
            {
                GD.PrintErr($"属性{cacheKey}添加值失败: {e.Message}");
            }
        }

        // 初始化属性
        public void InitializeAttributes(List<AttributeDefinition> definitions)
        {
            if (definitions == null || definitions.Count == 0)
            {
                GD.PrintErr("属性定义列表不能为空");
                return;
            }

            _attributeDefinitions.Clear();
            _attributeDefinitions.AddRange(definitions);
            _attributes.Clear();

            // 按SortOrder排序属性定义
            _attributeDefinitions.Sort((a, b) => a.SortOrder.CompareTo(b.SortOrder));

            // 初始化所有属性值
            foreach (AttributeDefinition definition in _attributeDefinitions)
            {
                object value = CalculateAttributeValue(definition);
                string cacheKey = definition.Type == AttributeType.Base ? definition.Id : $"{definition.Id}_{definition.Type}";
                _attributes[cacheKey] = value;
            }

            OnSystemInitialized?.Invoke();
        }

        // 获取所有属性
        public Dictionary<string, object> GetAllAttributes()
        {
            return new Dictionary<string, object>(_attributes);
        }

        // 按类型获取属性
        public Dictionary<string, object> GetAttributesByType(AttributeType type)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            foreach (KeyValuePair<string, object> kvp in _attributes)
            {
                if (type == AttributeType.Base && !kvp.Key.Contains('_'))
                {
                    result.Add(kvp.Key, kvp.Value);
                }
                else if (kvp.Key.EndsWith($"_{type}"))
                {
                    string attributeId = kvp.Key.Substring(0, kvp.Key.Length - $"_{type}".Length);
                    result.Add(attributeId, kvp.Value);
                }
            }

            return result;
        }

        // 计算属性值
        private object CalculateAttributeValue(AttributeDefinition definition)
        {
            if (definition.BaseValue is int baseInt)
            {
                return baseInt + (int)(_levelSystem.GetCurrentLevel() * definition.GrowthRate);
            }
            else if (definition.BaseValue is float baseFloat)
            {
                return baseFloat + (_levelSystem.GetCurrentLevel() * definition.GrowthRate);
            }
            else if (definition.BaseValue is double baseDouble)
            {
                return baseDouble + (_levelSystem.GetCurrentLevel() * definition.GrowthRate);
            }
            else
            {
                // 对于非数值类型，直接返回基础值
                return definition.BaseValue;
            }
        }

        // 添加经验
        public void AddExperience(float amount)
        {
            _experienceSystem.AddExperience(amount);
        }

        // 获取当前等级
        public int GetCurrentLevel()
        {
            return _levelSystem.GetCurrentLevel();
        }
    }
}