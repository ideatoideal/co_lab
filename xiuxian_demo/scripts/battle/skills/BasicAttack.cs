using Godot;
using System.Collections.Generic;
using XiuXianDemo.Common;

namespace XiuXianDemo.Battle.Skills
{
    public class BasicAttack : ISkill
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public float ManaCost { get; private set; }
        public float Cooldown { get; private set; }
        public float CastTime { get; private set; }
        public SkillType Type { get; private set; }

        private float _cooldownRemaining;

        public BasicAttack()
        {
            Id = "basic_attack";
            Name = "普通攻击";
            ManaCost = 0;
            Cooldown = 1.0f;
            CastTime = 0;
            Type = SkillType.Attack;
            _cooldownRemaining = 0;
        }

        public void Cast(BattleParticipant caster, List<BattleParticipant> targets)
        {
            if (!CanCast(caster) || targets == null || targets.Count == 0)
                return;

            // 扣除法力值
            caster.Mp -= ManaCost;

            // 重置冷却
            _cooldownRemaining = Cooldown;

            // 对第一个目标造成伤害
            var target = targets[0];
            float damage = caster.Attack * 1.2f; // 1.2倍攻击力
            target.TakeDamage(damage);

            // 触发攻击事件
            EventBus.Instance.Publish("OnSkillCast", caster, this, targets);
        }

        public bool CanCast(BattleParticipant caster)
        {
            return _cooldownRemaining <= 0 && caster.Mp >= ManaCost;
        }

        public float GetCooldownRemaining()
        {
            return _cooldownRemaining;
        }

        public void UpdateCooldown(float deltaTime)
        {
            if (_cooldownRemaining > 0)
            {
                _cooldownRemaining -= deltaTime;
            }
        }
    }
}