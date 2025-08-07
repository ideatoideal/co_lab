using Godot;
using System;

namespace XiuxianDemo
{
    /// <summary>
    /// 经验系统实现类，处理经验相关的逻辑
    /// </summary>
    public class ExperienceSystem : IExperienceSystem
    {
        private float _totalExperience = 0;
        private float _levelExperience = 0;
        private ILevelSystem _levelSystem;

        // 可配置的经验计算公式
        public Func<int, float> GetRequiredExperience { get; set; }

        // 总经验
        public float TotalExperience => _totalExperience;

        // 当前等级经验
        public float LevelExperience => _levelExperience;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="levelSystem">等级系统</param>
        public ExperienceSystem(ILevelSystem levelSystem)
        {
            _levelSystem = levelSystem;
            // 默认经验公式
            GetRequiredExperience = level => level * 100 + 50;
        }

        // 获取总经验
        public float GetTotalExperience()
        {
            return _totalExperience;
        }

        // 获取当前等级经验
        public float GetLevelExperience()
        {
            return _levelExperience;
        }

        // 添加经验
        public void AddExperience(float amount)
        {
            if (amount < 0)
            {
                GD.PrintErr("经验值不能为负数");
                return;
            }

            _totalExperience += amount;
            _levelExperience += amount;

            // 检查是否可以升级
            while (_levelSystem.CanLevelUp())
            {
                _levelSystem.LevelUp();
                _levelExperience = 0;
            }
        }

        // 计算升级所需经验
        public float CalculateRequiredExperience(int level)
        {
            if (level < 1)
            {
                GD.PrintErr("等级不能小于1");
                return 0;
            }

            return GetRequiredExperience(level);
        }

        // 升级
        public void LevelUp()
        {
            if (_levelSystem.LevelUp())
            {
                _levelExperience = 0;
            }
        }
    }
}