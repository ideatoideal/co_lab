using System;
using System.Collections.Generic;

namespace XiuXianDemo.Characters
{
    /// <summary>
    /// 等级系统接口
    /// </summary>
    public interface ILevelSystem
    {
        /// <summary>
        /// 当前等级
        /// </summary>
        int CurrentLevel { get; }

        /// <summary>
        /// 获取升级所需经验
        /// </summary>
        /// <param name="level">等级</param>
        /// <returns>所需经验值</returns>
        int GetExperienceRequired(int level);

        /// <summary>
        /// 检查是否可以升级
        /// </summary>
        /// <param name="currentExperience">当前经验值</param>
        /// <returns>是否可以升级</returns>
        bool CanLevelUp(int currentExperience);

        /// <summary>
        /// 升级
        /// </summary>
        /// <returns>升级后的等级</returns>
        int LevelUp();

        /// <summary>
        /// 获取属性成长值
        /// </summary>
        /// <param name="attributeId">属性ID</param>
        /// <returns>成长值</returns>
        float GetAttributeGrowth(string attributeId);

        /// <summary>
        /// 等级变化事件
        /// </summary>
        event Action<int> OnLevelChanged;
    }
}