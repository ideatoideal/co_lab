using Godot;

namespace XiuXianDemo.Battle
{
    public enum CombatState
    {
        // 战斗未开始
        Idle,
        // 战斗准备阶段
        Preparing,
        // 战斗进行中
        Fighting,
        // 战斗胜利
        Victory,
        // 战斗失败
        Defeat
    }
}