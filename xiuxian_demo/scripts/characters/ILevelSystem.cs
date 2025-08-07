namespace XiuxianDemo
{
    /// <summary>
    /// 等级系统接口，定义等级相关的操作方法
    /// </summary>
    public interface ILevelSystem
    {
        // 获取当前等级
        int GetCurrentLevel();

        // 设置当前等级
        void SetCurrentLevel(int currentLevel);

        // 获取当前经验
        float GetCurrentExperience();

        // 添加经验
        void AddExperience(float amount);

        // 检查是否可以升级
        bool CanLevelUp();

        // 升级
        bool LevelUp();

        // 获取升级所需经验
        float GetRequiredExperience(int level);

        // 获取属性成长率
        float[] GetAttributeGrowthRates();
    }
}