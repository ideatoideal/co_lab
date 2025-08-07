using Godot;
using System.Collections.Generic;

namespace XiuXianDemo.Battle
{
    public interface IEnemyAI
    {
        // 更新AI状态
        void Update(float deltaTime);

        // 做出决策
        void MakeDecision();

        // 寻找目标
        BattleParticipant FindTarget();

        // 选择技能
        ISkill ChooseSkill();

        // 设置AI难度
        void SetDifficulty(float difficulty);
    }
}