using Godot;
using System;
using System.Collections.Generic;

namespace XiuXianDemo.Battle
{
    public interface IBattleManager
    {
        // 初始化战斗
        void Initialize(List<BattleParticipant> participants);

        // 开始战斗
        void StartBattle();

        // 结束战斗
        void EndBattle(bool isPlayerVictory);

        // 更新战斗逻辑
        void Update(float deltaTime);

        // 处理输入
        void HandleInput(InputEvent input);

        // 注册事件处理器
        void RegisterEventHandler(string eventName, Action handler);

        // 获取当前战斗状态
        CombatState GetCurrentState();

        // 获取参与者列表
        List<BattleParticipant> GetParticipants();
    }
}