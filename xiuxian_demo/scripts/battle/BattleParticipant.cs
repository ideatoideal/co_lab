using Godot;
using System.Collections.Generic;

namespace XiuXianDemo.Battle
{
    public class BattleParticipant
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public float Hp { get; set; }
        public float MaxHp { get; set; }
        public float Mp { get; set; }
        public float MaxMp { get; set; }
        public float Attack { get; set; }
        public float Defense { get; set; }
        public float Speed { get; set; }
        public Vector2 Position { get; set; }
        public bool IsPlayer { get; set; }
        public List<ISkill> Skills { get; set; }
        public Dictionary<string, float> Buffs { get; set; }

        // 构造函数
        public BattleParticipant()
        {
            Skills = new List<ISkill>();
            Buffs = new Dictionary<string, float>();
        }

        // 受到伤害
        public void TakeDamage(float damage)
        {
            // 计算最终伤害（考虑防御）
            float finalDamage = Mathf.Max(1, damage - Defense * 0.1f);
            Hp = Mathf.Max(0, Hp - finalDamage);
        }

        // 治疗
        public void Heal(float amount)
        {
            Hp = Mathf.Min(MaxHp, Hp + amount);
        }
    }
}