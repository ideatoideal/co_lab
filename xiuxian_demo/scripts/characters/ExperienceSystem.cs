using Godot;
using System;
using System.Collections.Generic;

namespace XiuXianDemo.Characters
{
    /// <summary>
    /// 经验系统实现
    /// </summary>
    public class ExperienceSystem : IExperienceSystem
    {
        private float _currentExperience = 0f;
        private ILevelSystem _levelSystem;

        /// <summary>
        /// 当前经验值
        /// </summary>
        public float CurrentExperience => _currentExperience;

        /// <summary>
        /// 经验值变化事件
        /// </summary>
        public event Action<float> OnExperienceChanged;

        /// <summary>
        /// 升级事件
        /// </summary>
        public event Action<int> OnLevelUp;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="levelSystem">等级系统</param>
        public ExperienceSystem(ILevelSystem levelSystem)
        {
            _levelSystem = levelSystem ?? throw new ArgumentNullException(nameof(levelSystem));
        }

        /// <summary>
        /// 添加经验
        /// </summary>
        /// <param name="amount">经验值</param>
        public void AddExperience(float amount)
        {
            if (amount <= 0)
            {
                GD.PrintErr("经验值必须大于0");
                return;
            }

            _currentExperience += amount;
            OnExperienceChanged?.Invoke(_currentExperience);

            CheckLevelUp();
        }

        /// <summary>
        /// 获取下一级所需经验
        /// </summary>
        /// <returns>所需经验值</returns>
        public float GetExperienceToNextLevel()
        {
            return _levelSystem.GetExperienceRequired(_levelSystem.CurrentLevel);
        }

        /// <summary>
        /// 检查并处理升级
        /// </summary>
        private void CheckLevelUp()
        {
            while (_levelSystem.CanLevelUp((int)_currentExperience))
            {
                int requiredExp = _levelSystem.GetExperienceRequired(_levelSystem.CurrentLevel);
                if (_currentExperience >= requiredExp)
                {
                    _currentExperience -= requiredExp;
                    int newLevel = _levelSystem.LevelUp();
                    OnLevelUp?.Invoke(newLevel);
                    GD.Print($"恭喜升级到 {newLevel} 级！");
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 设置当前经验（调试用）
        /// </summary>
        /// <param name="experience">经验值</param>
        public void SetCurrentExperience(float experience)
        {
            if (experience < 0)
            {
                GD.PrintErr("经验值不能为负数");
                return;
            }

            _currentExperience = experience;
            OnExperienceChanged?.Invoke(_currentExperience);
            CheckLevelUp();
        }
    }
}