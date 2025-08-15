using GdUnit4;
using static GdUnit4.Assertions;
using System;
using System.Collections.Generic;
using XiuXianDemo.Characters;

namespace XiuXianDemo.Tests.Characters
{
    [TestSuite]
    public class CharacterSystemTestRunner
    {
        [TestCase]
        public void TestCharacterSystemIntegration()
        {
            // 测试角色系统的集成
            var attributes = new CharacterAttributes();
            var levelSystem = new CharacterLevel();
            var experienceSystem = new ExperienceSystem(levelSystem);
            
            // 初始化属性
            attributes.InitializeAttributes(new List<AttributeDefinition>
            {
                new AttributeDefinition("hp", "生命值", AttributeType.Base, 100, 0, "", "", 1),
                new AttributeDefinition("mp", "魔法值", AttributeType.Base, 50, 0, "", "", 1),
                new AttributeDefinition("attack", "攻击力", AttributeType.Base, 10, 0, "", "", 1)
            });
            
            // 测试升级流程
            AssertThat(experienceSystem.CurrentExperience).IsEqual(0);
            AssertThat(levelSystem.CurrentLevel).IsEqual(1);
            
            // 添加经验值升级
            experienceSystem.AddExperience(100);
            AssertThat(levelSystem.CurrentLevel).IsGreater(1);
            AssertThat(experienceSystem.CurrentExperience).IsGreater(0);
            
            // 验证属性成长
            float hpAfterLevelUp = (float)attributes.GetAttribute("hp");
            AssertThat(hpAfterLevelUp).IsGreater(100);
        }
    }
}