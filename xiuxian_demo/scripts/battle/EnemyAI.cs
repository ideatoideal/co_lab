using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace XiuXianDemo.Battle
{
    public class EnemyAI : IEnemyAI
    {
        private BattleParticipant _owner;
        private AIState _currentState;
        private float _decisionTimer;
        private float _difficulty;
        private Random _random;

        public EnemyAI(BattleParticipant owner)
        {
            _owner = owner;
            _currentState = AIState.Attacking;
            _decisionTimer = 0;
            _difficulty = 1.0f;
            _random = new Random();
        }

        public void Update(float deltaTime)
        {
            _decisionTimer -= deltaTime;

            if (_decisionTimer <= 0)
            {
                MakeDecision();
                _decisionTimer = (float)_random.NextDouble() * 2.0f + 1.0f; // 1-3秒随机决策间隔
            }

            // 根据当前状态执行不同行为
            switch (_currentState)
            {
                case AIState.Attacking:
                    // 攻击行为逻辑
                    break;
                case AIState.Defending:
                    // 防御行为逻辑
                    break;
                case AIState.UsingSkill:
                    // 使用技能行为逻辑
                    break;
                case AIState.Moving:
                    // 移动行为逻辑
                    break;
            }
        }

        public void MakeDecision()
        {
            // 1. 寻找目标
            BattleParticipant target = FindTarget();
            if (target == null)
                return;

            // 2. 选择技能
            ISkill skill = ChooseSkill();
            if (skill != null && skill.CanCast(_owner))
            {
                // 使用技能
                _currentState = AIState.UsingSkill;
                List<BattleParticipant> targets = new List<BattleParticipant> { target };
                skill.Cast(_owner, targets);
            }
            else
            {
                // 普通攻击
                _currentState = AIState.Attacking;
                // 这里简化处理，实际应该有攻击动作和动画
            }
        }

        public BattleParticipant FindTarget()
        {
            // 查找所有活着的玩家
            var battleManager = BattleManager.Instance;
            if (battleManager == null)
                return null;

            var participants = battleManager.GetParticipants();
            var livingPlayers = participants.Where(p => p.IsPlayer && p.Hp > 0).ToList();

            if (livingPlayers.Count == 0)
                return null;

            // 简单AI：优先攻击血量最低的玩家
            return livingPlayers.OrderBy(p => p.Hp).First();
        }

        public ISkill ChooseSkill()
        {
            // 获取所有可用技能
            var availableSkills = _owner.Skills.Where(s => s.CanCast(_owner)).ToList();

            if (availableSkills.Count == 0)
                return null;

            // 简单AI：随机选择一个可用技能
            int index = _random.Next(availableSkills.Count);
            return availableSkills[index];
        }

        public void SetDifficulty(float difficulty)
        {
            _difficulty = Mathf.Clamp(difficulty, 0.5f, 3.0f);
            // 根据难度调整AI行为
        }
    }
}