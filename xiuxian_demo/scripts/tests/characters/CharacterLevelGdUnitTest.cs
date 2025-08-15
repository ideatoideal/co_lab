using GdUnit4;
using static GdUnit4.Assertions;
using System;
using System.Collections.Generic;
using XiuXianDemo.Characters;

namespace XiuXianDemo.Tests.Characters
{
    [TestSuite]
    public class CharacterLevelGdUnitTest
    {
        private CharacterLevel _levelSystem;

        [BeforeTest]
        public void Setup()
        {
            _levelSystem = new CharacterLevel();
        }

        [TestCase]
        public void TestInitialLevel()
        {
            // 测试初始等级为1
            AssertInt(_levelSystem.CurrentLevel).IsEqual(1);
        }

        [TestCase]
        public void TestGetExperienceRequired()
        {
            // 测试获取升级所需经验
            int requiredExp = _levelSystem.GetExperienceRequired(1);
            AssertInt(requiredExp).IsEqual(100);

            requiredExp = _levelSystem.GetExperienceRequired(2);
            AssertInt(requiredExp).IsEqual(400);
        }

        [TestCase]
        public void TestCanLevelUp()
        {
            // 测试是否可以升级
            AssertBool(_levelSystem.CanLevelUp(50)).IsFalse();
            AssertBool(_levelSystem.CanLevelUp(100)).IsTrue();
            AssertBool(_levelSystem.CanLevelUp(150)).IsTrue();
        }

        [TestCase]
        public void TestLevelUp()
        {
            // 测试升级功能
            int oldLevel = _levelSystem.CurrentLevel;
            _levelSystem.LevelUp();
            
            AssertInt(_levelSystem.CurrentLevel).IsEqual(oldLevel + 1);
        }

        [TestCase]
        public void TestGetAttributeGrowth()
        {
            // 测试获取属性成长值
            float growth = _levelSystem.GetAttributeGrowth("hp");
            AssertFloat(growth).IsGreater(0);

            growth = _levelSystem.GetAttributeGrowth("attack");
            AssertFloat(growth).IsGreater(0);
        }

        [TestCase]
        public void TestSetCurrentLevel()
        {
            // 测试设置当前等级（调试用）
            _levelSystem.SetCurrentLevel(5);
            AssertInt(_levelSystem.CurrentLevel).IsEqual(5);
        }
    }
}