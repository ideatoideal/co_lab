using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using XiuXianDemo.Common;

namespace XiuXianDemo.Battle
{
    public class BattleManager : IBattleManager
    {
        private static BattleManager _instance;
        private List<BattleParticipant> _participants;
        private CombatState _currentState;
        private float _battleTimer;
        private Dictionary<string, List<Action>> _eventHandlers;
        private List<IEnemyAI> _enemyAIs;

        public static BattleManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BattleManager();
                }
                return _instance;
            }
        }

        public BattleManager()
        {
            _eventHandlers = new Dictionary<string, List<Action>>();
            _enemyAIs = new List<IEnemyAI>();
            _currentState = CombatState.Idle;
        }

        public void Initialize(List<BattleParticipant> participants)
        {
            _participants = participants;
            _currentState = CombatState.Preparing;
            _battleTimer = 0;
            _enemyAIs.Clear();

            // 为每个敌人创建AI
            foreach (var participant in _participants)
            {
                if (!participant.IsPlayer)
                {
                    var ai = new EnemyAI(participant);
                    _enemyAIs.Add(ai);
                }
            }

            // 注册事件
            EventBus.Instance.Subscribe("OnBattleEnded", OnBattleEndedHandler);
        }

        public void StartBattle()
        {
            _currentState = CombatState.Fighting;
            EventBus.Instance.Publish("OnBattleStarted");
        }

        public void EndBattle(bool isPlayerVictory)
        {
            _currentState = isPlayerVictory ? CombatState.Victory : CombatState.Defeat;
            _battleTimer = 0;
            EventBus.Instance.Publish("OnBattleEnded", isPlayerVictory);
        }

        public void Update(float deltaTime)
        {
            _battleTimer += deltaTime;

            switch (_currentState)
            {
                case CombatState.Preparing:
                    // 准备阶段逻辑
                    if (_battleTimer >= 3.0f) // 3秒准备时间
                    {
                        StartBattle();
                    }
                    break;

                case CombatState.Fighting:
                    // 战斗阶段逻辑
                    UpdateParticipants(deltaTime);
                    UpdateAIs(deltaTime);
                    CheckBattleEndCondition();
                    break;

                case CombatState.Victory:
                case CombatState.Defeat:
                    // 结束阶段逻辑
                    if (_battleTimer >= 5.0f) // 显示结果5秒
                    {
                        // 返回地图
                        EventBus.Instance.Publish("OnBattleResultConfirmed", _currentState == CombatState.Victory);
                    }
                    break;
            }
        }

        private void UpdateParticipants(float deltaTime)
        {
            // 更新所有参与者状态
            foreach (var participant in _participants)
            {
                // 更新技能冷却
                foreach (var skill in participant.Skills)
                {
                    skill.UpdateCooldown(deltaTime);
                }

                // 更新 buff/debuff 效果
                UpdateBuffs(participant, deltaTime);
            }
        }

        private void UpdateBuffs(BattleParticipant participant, float deltaTime)
        {
            // 简化实现，实际应该有更复杂的buff效果处理
            var buffsToRemove = new List<string>();
            foreach (var buff in participant.Buffs)
            {
                // 减少buff持续时间
                participant.Buffs[buff.Key] -= deltaTime;
                if (participant.Buffs[buff.Key] <= 0)
                {
                    buffsToRemove.Add(buff.Key);
                }
            }

            // 移除过期的buff
            foreach (var buffId in buffsToRemove)
            {
                participant.Buffs.Remove(buffId);
            }
        }

        private void UpdateAIs(float deltaTime)
        {
            // 更新所有敌人AI
            foreach (var ai in _enemyAIs)
            {
                ai.Update(deltaTime);
            }
        }

        private void CheckBattleEndCondition()
        {
            // 检查玩家是否全灭
            bool allPlayersDead = _participants.Where(p => p.IsPlayer).All(p => p.Hp <= 0);

            // 检查敌人是否全灭
            bool allEnemiesDead = _participants.Where(p => !p.IsPlayer).All(p => p.Hp <= 0);

            if (allPlayersDead)
            {
                EndBattle(false);
            }
            else if (allEnemiesDead)
            {
                EndBattle(true);
            }
        }

        public void HandleInput(InputEvent input)
        {
            // 处理玩家输入，例如技能释放指令
            if (input is InputEventKey keyEvent && keyEvent.Pressed)
            {
                // 简化实现，实际应该有更复杂的输入处理逻辑
                if (keyEvent.Scancode == (int)KeyList.Space)
                {
                    // 假设空格键释放第一个技能
                    var player = _participants.FirstOrDefault(p => p.IsPlayer);
                    if (player != null && player.Skills.Count > 0)
                    {
                        var skill = player.Skills[0];
                        if (skill.CanCast(player))
                        {
                            var enemies = _participants.Where(p => !p.IsPlayer && p.Hp > 0).ToList();
                            if (enemies.Count > 0)
                            {
                                skill.Cast(player, new List<BattleParticipant> { enemies[0] });
                            }
                        }
                    }
                }
            }
        }

        public void RegisterEventHandler(string eventName, Action handler)
        {
            if (!_eventHandlers.ContainsKey(eventName))
            {
                _eventHandlers[eventName] = new List<Action>();
            }
            _eventHandlers[eventName].Add(handler);
        }

        public CombatState GetCurrentState()
        {
            return _currentState;
        }

        public List<BattleParticipant> GetParticipants()
        {
            return _participants;
        }

        private void OnBattleEndedHandler(object[] args)
        {
            bool isVictory = (bool)args[0];
            // 处理战斗结束逻辑，如奖励发放
            if (isVictory)
            {
                // 给玩家发放经验和物品
                var player = _participants.FirstOrDefault(p => p.IsPlayer);
                if (player != null)
                {
                    // 简化实现，实际应该根据敌人配置计算奖励
                    EventBus.Instance.Publish("OnBattleReward", 100, new List<string> { "potion_health" });
                }
            }
        }
    }
}