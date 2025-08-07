using Godot;
using System;

namespace XiuxianDemo
{
    /// <summary>
    /// 等级系统实现类，处理等级相关的逻辑
    /// </summary>
    public class CharacterLevel : ILevelSystem
    {
        private int _currentLevel = 1;
        private float _currentExperience = 0;
        private float _experienceToNextLevel = 150;
        private float[] _attributeGrowthRates = new float[6] { 20, 10, 2, 1, 1, 0.5f };

        // 当前等级
        public int CurrentLevel => _currentLevel;

        // 当前经验
        public float CurrentExperience => _currentExperience;

        // 升级所需经验
        public float ExperienceToNextLevel => _experienceToNextLevel;

        // 属性成长率
        public float[] AttributeGrowthRates => _attributeGrowthRates;

        // 获取当前等级
        public int GetCurrentLevel()
        {
            return _currentLevel;
        }

        // 设置当前等级
        public void SetCurrentLevel(int currentLevel)
        {
            if (currentLevel < 1)
            {
                GD.PrintErr("等级不能小于1");
                return;
            }

            _currentLevel = currentLevel;
            UpdateExperienceToNextLevel();
            UpdateAttributeGrowthRates();
        }

        // 获取当前经验
        public float GetCurrentExperience()
        {
            return _currentExperience;
        }

        // 添加经验
        public void AddExperience(float amount)
        {
            if (amount < 0)
            {
                GD.PrintErr("经验值不能为负数");
                return;
            }

            _currentExperience += amount;
        }

        // 检查是否可以升级
        public bool CanLevelUp()
        {
            return _currentExperience >= _experienceToNextLevel;
        }

        // 升级
        public bool LevelUp()
        {
            if (!CanLevelUp())
            {
                return false;
            }

            _currentExperience -= _experienceToNextLevel;
            _currentLevel++;
            UpdateExperienceToNextLevel();
            UpdateAttributeGrowthRates();

            return true;
        }

        // 获取升级所需经验
        public float GetRequiredExperience(int level)
        {
            if (level < 1)
            {
                GD.PrintErr("等级不能小于1");
                return 0;
            }

            // 经验公式：level * 100 + 50
            return level * 100 + 50;
        }

        // 获取属性成长率
        public float[] GetAttributeGrowthRates()
        {
            return _attributeGrowthRates;
        }

        // 更新升级所需经验
        private void UpdateExperienceToNextLevel()
        {
            _experienceToNextLevel = GetRequiredExperience(_currentLevel);
        }

        // 更新属性成长率
        private void UpdateAttributeGrowthRates()
        {
            // 根据等级调整属性成长率
            // 这里只是示例，实际项目中可能需要从配置表加载
            _attributeGrowthRates[0] = 20 + (_currentLevel - 1) * 2;    // 气血成长率
            _attributeGrowthRates[1] = 10 + (_currentLevel - 1) * 1;    // 法力成长率
            _attributeGrowthRates[2] = 2 + (_currentLevel - 1) * 0.5f;  // 攻击成长率
            _attributeGrowthRates[3] = 1 + (_currentLevel - 1) * 0.3f;  // 防御成长率
            _attributeGrowthRates[4] = 1 + (_currentLevel - 1) * 0.2f;  // 速度成长率
            _attributeGrowthRates[5] = 0.5f + (_currentLevel - 1) * 0.1f; // 暴击成长率
        }
    }
}