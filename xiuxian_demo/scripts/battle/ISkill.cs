using Godot;
using System.Collections.Generic;

namespace XiuXianDemo.Battle
{
    public interface ISkill
    {
        string Id { get; }
        string Name { get; }
        float ManaCost { get; }
        float Cooldown { get; }
        float CastTime { get; }
        SkillType Type { get; }

        void Cast(BattleParticipant caster, List<BattleParticipant> targets);
        bool CanCast(BattleParticipant caster);
        float GetCooldownRemaining();
        void UpdateCooldown(float deltaTime);
    }
}