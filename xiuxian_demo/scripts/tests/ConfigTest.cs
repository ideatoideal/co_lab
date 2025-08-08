using Godot;
using XiuXianDemo.Common;
using System;

namespace XiuXianDemo.Tests
{
    /// <summary>
    /// 配置管理器测试类
    /// </summary>
    public partial class ConfigTest : Node
    {
        public override void _Ready()
        {
            // 初始化配置管理器
            ConfigManager.Instance.Initialize();
            GD.Print("配置测试开始");

            // 测试属性配置
            TestAttributeConfig();

            // 测试等级配置
            TestLevelConfig();

            // 测试经验配置
            TestExperienceConfig();

            GD.Print("配置测试结束");
        }

        /// <summary>
        /// 测试属性配置
        /// </summary>
        private void TestAttributeConfig()
        {
            GD.Print("\n测试属性配置:");
            var hpConfig = ConfigManager.Instance.GetAttributeConfig("hp");
            if (hpConfig != null)
            {
                GD.Print($"气血属性: ID={hpConfig.AttributeId}, 名称={hpConfig.AttributeName}, 基础值={hpConfig.BaseValue}, 成长率={hpConfig.GrowthRate}");
            }

            var attackConfig = ConfigManager.Instance.GetAttributeConfig("attack");
            if (attackConfig != null)
            {
                GD.Print($"攻击属性: ID={attackConfig.AttributeId}, 名称={attackConfig.AttributeName}, 基础值={attackConfig.BaseValue}, 成长率={attackConfig.GrowthRate}");
            }

            var defenseConfig = ConfigManager.Instance.GetAttributeConfig("defense");
            if (defenseConfig != null)
            {
                GD.Print($"防御属性: ID={defenseConfig.AttributeId}, 名称={defenseConfig.AttributeName}, 基础值={defenseConfig.BaseValue}, 成长率={defenseConfig.GrowthRate}");
            }
        }

        /// <summary>
        /// 测试等级配置
        /// </summary>
        private void TestLevelConfig()
        {
            GD.Print("\n测试等级配置:");
            for (int i = 1; i <= 3; i++)
            {
                var levelConfig = ConfigManager.Instance.GetLevelConfig(i);
                if (levelConfig != null)
                {
                    GD.Print($"等级 {i}: 升级经验={levelConfig.ExpToNextLevel}, HP成长={levelConfig.HpGrowthBonus}, MP成长={levelConfig.MpGrowthBonus}, 攻击成长={levelConfig.AttackGrowthBonus}, 防御成长={levelConfig.DefenseGrowthBonus}, 解锁功能={levelConfig.UnlockFeature}");
                }
            }
        }

        /// <summary>
        /// 测试经验配置
        /// </summary>
        private void TestExperienceConfig()
        {
            GD.Print("\n测试经验配置:");

            // 测试敌人经验
            for (int i = 1; i <= 3; i++)
            {
                var enemyExpConfig = ConfigManager.Instance.GetEnemyExpConfig(i);
                if (enemyExpConfig != null)
                {
                    GD.Print($"敌人等级 {i}: 基础经验={enemyExpConfig.EnemyBaseExp}, 难度系数={enemyExpConfig.DifficultyCoefficient}");
                }
            }

            // 测试任务经验
            var task1Config = ConfigManager.Instance.GetTaskExpConfig("task_1001");
            if (task1Config != null)
            {
                GD.Print($"任务 task_1001: 基础经验={task1Config.TaskBaseExp}, 难度系数={task1Config.DifficultyCoefficient}");
            }

            var task2Config = ConfigManager.Instance.GetTaskExpConfig("task_1002");
            if (task2Config != null)
            {
                GD.Print($"任务 task_1002: 基础经验={task2Config.TaskBaseExp}, 难度系数={task2Config.DifficultyCoefficient}");
            }

            // 测试探索经验
            var explore1Config = ConfigManager.Instance.GetExploreExpConfig("explore_001");
            if (explore1Config != null)
            {
                GD.Print($"探索点 explore_001: 经验值={explore1Config.ExploreExp}");
            }

            var explore2Config = ConfigManager.Instance.GetExploreExpConfig("explore_002");
            if (explore2Config != null)
            {
                GD.Print($"探索点 explore_002: 经验值={explore2Config.ExploreExp}");
            }
        }
    }
}