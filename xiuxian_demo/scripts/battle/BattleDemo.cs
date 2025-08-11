using Godot;
using System;
using System.Collections.Generic;
using XiuXianDemo.Battle.Skills;
using XiuXianDemo.Common;

namespace XiuXianDemo.Battle
{
    public partial class BattleDemo : Node2D
    {
        private BattleManager _battleManager;

        public override void _Ready()
        {
            // 初始化战斗管理器
            _battleManager = BattleManager.Instance;

            // 注册战斗事件
            EventBus.Instance.Subscribe("OnBattleStarted", OnBattleStarted);
            EventBus.Instance.Subscribe("OnBattleEnded", OnBattleEnded);
            EventBus.Instance.Subscribe("OnSkillCast", OnSkillCast);
            EventBus.Instance.Subscribe("OnBattleReward", OnBattleReward);

            // 创建战斗参与者
            List<BattleParticipant> participants = CreateParticipants();

            // 初始化战斗
            _battleManager.Initialize(participants);
        }

        public override void _Process(double delta)
        {
            // 更新战斗逻辑
            _battleManager.Update((float)delta);
        }

        public override void _Input(InputEvent input)
        {
            // 处理战斗输入
            _battleManager.HandleInput(input);
        }

        private List<BattleParticipant> CreateParticipants()
        {
            List<BattleParticipant> participants = new List<BattleParticipant>();

            // 创建玩家
            BattleParticipant player = new BattleParticipant
            {
                Id = "player1",
                Name = "玩家",
                Hp = 100,
                MaxHp = 100,
                Mp = 50,
                MaxMp = 50,
                Attack = 20,
                Defense = 10,
                Speed = 8,
                Position = new Vector2(100, 200),
                IsPlayer = true
            };

            // 给玩家添加技能
            player.Skills.Add(new BasicAttack());

            // 创建敌人1
            BattleParticipant enemy1 = new BattleParticipant
            {
                Id = "enemy1",
                Name = "野狼",
                Hp = 50,
                MaxHp = 50,
                Mp = 20,
                MaxMp = 20,
                Attack = 10,
                Defense = 5,
                Speed = 5,
                Position = new Vector2(300, 150),
                IsPlayer = false
            };

            // 给敌人添加技能
            enemy1.Skills.Add(new BasicAttack());

            // 创建敌人2
            BattleParticipant enemy2 = new BattleParticipant
            {
                Id = "enemy2",
                Name = "猛虎",
                Hp = 80,
                MaxHp = 80,
                Mp = 30,
                MaxMp = 30,
                Attack = 15,
                Defense = 8,
                Speed = 7,
                Position = new Vector2(350, 250),
                IsPlayer = false
            };

            // 给敌人添加技能
            enemy2.Skills.Add(new BasicAttack());

            participants.Add(player);
            participants.Add(enemy1);
            participants.Add(enemy2);

            return participants;
        }

        private void OnBattleStarted(object[] args)
        {
            GD.Print("战斗开始！");
        }

        private void OnBattleEnded(object[] args)
        {
            bool isVictory = (bool)args[0];
            if (isVictory)
            {
                GD.Print("战斗胜利！");
            }
            else
            {
                GD.Print("战斗失败！");
            }
        }

        private void OnSkillCast(object[] args)
        {
            BattleParticipant caster = (BattleParticipant)args[0];
            ISkill skill = (ISkill)args[1];
            List<BattleParticipant> targets = (List<BattleParticipant>)args[2];

            GD.Print($"{caster.Name} 使用了 {skill.Name} 技能，目标：{targets[0].Name}");
        }

        private void OnBattleReward(object[] args)
        {
            int experience = (int)args[0];
            List<string> items = (List<string>)args[1];

            GD.Print($"获得经验：{experience}");
            GD.Print("获得物品：");
            foreach (var item in items)
            {
                GD.Print($"- {item}");
            }
        }
    }
}