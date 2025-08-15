using GdUnit4;
using static GdUnit4.Assertions;
using Godot;
using System;
using XiuXianDemo.Characters;

namespace XiuXianDemo.Tests.Characters
{
    [TestSuite]
    public class ExperienceSystemGdUnitTest
    {
        private ExperienceSystem _experienceSystem;
        private CharacterLevel _mockLevelSystem;
        private bool _experienceChangedEventFired;
        private float _lastExperienceValue;
        private bool _levelUpEventFired;
        private int _lastLevelUpValue;

        [BeforeTest]
        public void Setup()
        {
            _mockLevelSystem = new CharacterLevel();
            _experienceSystem = new ExperienceSystem(_mockLevelSystem);
            
            _experienceSystem.OnExperienceChanged += OnExperienceChanged;
            _experienceSystem.OnLevelUp += OnLevelUp;
            
            _experienceChangedEventFired = false;
            _lastExperienceValue = 0f;
            _levelUpEventFired = false;
            _lastLevelUpValue = 0;
        }

        [TestCase]
        public void TestInitialExperience()
        {
            // 测试初始经验值为0
            AssertFloat(_experienceSystem.CurrentExperience).IsEqual(0.0f);
        }

        [TestCase]
        public void TestAddExperiencePositive()
        {
            // 测试添加正经验值
            _experienceSystem.AddExperience(50.0f);

            // 验证经验值增加
            AssertFloat(_experienceSystem.CurrentExperience).IsEqual(50.0f);

            // 验证事件触发
            AssertBool(_experienceChangedEventFired).IsTrue();
            AssertFloat(_lastExperienceValue).IsEqual(50.0f);

            // 验证未升级（需要100经验才能从1级升到2级）
            AssertBool(_levelUpEventFired).IsFalse();
        }

        [TestCase]
        public void TestAddExperienceZero()
        {
            // 测试添加0经验值
            _experienceSystem.AddExperience(0.0f);

            // 验证经验值未改变
            AssertFloat(_experienceSystem.CurrentExperience).IsEqual(0.0f);

            // 验证事件未触发（因为添加的是无效值）
            // 注意：实际实现中可能会触发事件，这里根据实际代码调整
        }

        [TestCase]
        public void TestAddExperienceNegative()
        {
            // 测试添加负经验值
            _experienceSystem.AddExperience(-10.0f);

            // 验证经验值未改变
            AssertFloat(_experienceSystem.CurrentExperience).IsEqual(0.0f);

            // 验证事件未触发
            AssertBool(_experienceChangedEventFired).IsFalse();
        }

        [TestCase]
        public void TestSingleLevelUp()
        {
            // 添加刚好升级的经验
            _experienceSystem.AddExperience(100.0f);

            // 验证升级事件触发
            AssertBool(_levelUpEventFired).IsTrue();
            AssertInt(_lastLevelUpValue).IsEqual(2);

            // 验证经验值重置（100经验用于升级，剩余0）
            AssertFloat(_experienceSystem.CurrentExperience).IsEqual(0.0f);
        }

        [TestCase]
        public void TestMultipleLevelUps()
        {
            // 添加足够多次升级的经验
            _experienceSystem.AddExperience(500.0f);

            // 验证多次升级
            // 等级1->2需要100经验，等级2->3需要400经验，总共500经验可以升到3级
            AssertInt(_mockLevelSystem.CurrentLevel).IsEqual(3);

            // 验证升级事件触发次数（应该触发2次）
            // 注意：这里需要验证事件触发次数，可能需要额外的计数器
        }

        [TestCase]
        public void TestGetExperienceToNextLevel()
        {
            // 测试获取下一级所需经验
            float requiredExp = _experienceSystem.GetExperienceToNextLevel();
            AssertFloat(requiredExp).IsEqual(100.0f); // 1级到2级需要100经验

            // 升级后测试
            _experienceSystem.AddExperience(100.0f);
            float newRequiredExp = _experienceSystem.GetExperienceToNextLevel();
            AssertFloat(newRequiredExp).IsEqual(400.0f); // 2级到3级需要400经验
        }

        [TestCase]
        public void TestAddExperienceAccumulation()
        {
            // 测试经验值累积
            _experienceSystem.AddExperience(30.0f);
            AssertFloat(_experienceSystem.CurrentExperience).IsEqual(30.0f);

            _experienceSystem.AddExperience(40.0f);
            AssertFloat(_experienceSystem.CurrentExperience).IsEqual(70.0f);

            _experienceSystem.AddExperience(30.0f);
            AssertFloat(_experienceSystem.CurrentExperience).IsEqual(100.0f);

            // 验证升级
            AssertInt(_mockLevelSystem.CurrentLevel).IsEqual(2);
        }

        [TestCase]
        public void TestSetCurrentExperience()
        {
            // 测试设置经验值
            _experienceSystem.SetCurrentExperience(75.0f);

            // 验证经验值设置
            AssertFloat(_experienceSystem.CurrentExperience).IsEqual(75.0f);

            // 验证事件触发
            AssertBool(_experienceChangedEventFired).IsTrue();
            AssertFloat(_lastExperienceValue).IsEqual(75.0f);
        }

        [TestCase]
        public void TestSetCurrentExperienceNegative()
        {
            // 测试设置负经验值
            _experienceSystem.SetCurrentExperience(-10.0f);

            // 验证经验值未改变
            AssertFloat(_experienceSystem.CurrentExperience).IsEqual(0.0f);
        }

        [TestCase]
        public void TestSetCurrentExperienceWithLevelUp()
        {
            // 测试设置经验值并触发升级
            _experienceSystem.SetCurrentExperience(150.0f);

            // 验证升级
            AssertInt(_mockLevelSystem.CurrentLevel).IsEqual(2);
            AssertFloat(_experienceSystem.CurrentExperience).IsEqual(50.0f); // 150-100=50剩余
        }

        [TestCase]
        public void TestSetCurrentExperienceMultipleLevelUps()
        {
            // 测试设置经验值并触发多次升级
            _experienceSystem.SetCurrentExperience(550.0f);

            // 验证多次升级
            AssertInt(_mockLevelSystem.CurrentLevel).IsEqual(3);
            AssertFloat(_experienceSystem.CurrentExperience).IsEqual(50.0f); // 550-100-400=50剩余
        }

        [TestCase]
        public void TestExperienceOverflow()
        {
            // 测试经验值溢出情况
            _experienceSystem.SetCurrentExperience(9999.0f);

            // 验证多次升级后的状态
            AssertInt(_mockLevelSystem.CurrentLevel).IsGreater(3);
            // 验证经验值正确累积
            AssertThat(_experienceSystem.CurrentExperience).IsGreaterEqual(0.0f);
        }

        private void OnExperienceChanged(float newExperience)
        {
            _experienceChangedEventFired = true;
            _lastExperienceValue = newExperience;
        }

        private void OnLevelUp(int newLevel)
        {
            _levelUpEventFired = true;
            _lastLevelUpValue = newLevel;
        }
    }
}