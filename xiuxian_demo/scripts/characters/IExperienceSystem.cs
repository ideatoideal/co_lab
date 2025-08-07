namespace XiuxianDemo
{
    /// <summary>
    /// 经验系统接口，定义经验相关的操作方法
    /// </summary>
    public interface IExperienceSystem
    {
        // 获取总经验
        float GetTotalExperience();

        // 获取当前等级经验
        float GetLevelExperience();

        // 添加经验
        void AddExperience(float amount);

        // 计算升级所需经验
        float CalculateRequiredExperience(int level);

        // 升级
        void LevelUp();
    }
}