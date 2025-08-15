using Godot;
using System;
using System.Collections.Generic;

namespace XiuXianDemo.Characters
{
    /// <summary>
    /// 角色等级系统实现
    /// </summary>
    public class CharacterLevel : ILevelSystem
    {
        private int _currentLevel = 1;
        private Dictionary<string, float> _attributeGrowthRates;

        /// <summary>
        /// 当前等级
        /// </summary>
        public int CurrentLevel => _currentLevel;

        /// <summary>
        /// 等级变化事件
        /// </summary>
        public event Action<int> OnLevelChanged;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CharacterLevel()
        {
            InitializeAttributeGrowthRates();
        }

        /// <summary>
        /// 初始化属性成长率
        /// </summary>
        private void InitializeAttributeGrowthRates()
        {
            _attributeGrowthRates = new Dictionary<string, float>
            {
                { "hp", 10.0f },
                { "mp", 5.0f },
                { "attack", 2.0f },
                { "defense", 1.5f },
                { "speed", 1.0f }
            };
        }

        /// <summary>
        /// 获取升级所需经验
        /// </summary>
        /// <param name="level">等级</param>
        /// <returns>所需经验值</returns>
        public int GetExperienceRequired(int level)
        {
            return level * level * 100;
        }

        /// <summary>
        /// 检查是否可以升级
        /// </summary>
        /// <param name="currentExperience">当前经验值</param>
        /// <returns>是否可以升级</returns>
        public bool CanLevelUp(int currentExperience)
        {
            return currentExperience >= GetExperienceRequired(_currentLevel);
        }

        /// <summary>
        /// 升级
        /// </summary>
        /// <returns>升级后的等级</returns>
        public int LevelUp()
        {
            _currentLevel++;
            OnLevelChanged?.Invoke(_currentLevel);
            return _currentLevel;
        }

        /// <summary>
        /// 获取属性成长值
        /// </summary>
        /// <param name="attributeId">属性ID</param>
        /// <returns>成长值</returns>
        public float GetAttributeGrowth(string attributeId)
        {
            if (_attributeGrowthRates.TryGetValue(attributeId.ToLower(), out float growth))
            {
                return growth;
            }
            return 1.0f;
        }

        /// <summary>
        /// 设置当前等级（调试用）
        /// </summary>
        /// <param name="level">等级</param>
        public void SetCurrentLevel(int level)
        {
            if (level > 0)
            {
                _currentLevel = level;
                OnLevelChanged?.Invoke(_currentLevel);
            }
        }
    }
}