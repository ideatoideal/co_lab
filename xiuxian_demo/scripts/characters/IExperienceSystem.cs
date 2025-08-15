using System;

namespace XiuXianDemo.Characters
{
    /// <summary>
    /// 经验系统接口
    /// </summary>
    public interface IExperienceSystem
    {
        /// <summary>
        /// 当前经验值
        /// </summary>
        float CurrentExperience { get; }

        /// <summary>
        /// 添加经验
        /// </summary>
        /// <param name="amount">经验值</param>
        void AddExperience(float amount);

        /// <summary>
        /// 获取下一级所需经验
        /// </summary>
        /// <returns>所需经验值</returns>
        float GetExperienceToNextLevel();

        /// <summary>
        /// 经验值变化事件
        /// </summary>
        event Action<float> OnExperienceChanged;

        /// <summary>
        /// 升级事件
        /// </summary>
        event Action<int> OnLevelUp;
    }
}