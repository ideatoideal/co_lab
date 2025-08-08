using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace XiuXianDemo.Common
{
    /// <summary>
    /// 配置管理器，负责加载和管理游戏中的各种配置表
    /// 采用单例模式实现全局访问
    /// </summary>
    public class ConfigManager
    {
        /// <summary>
        /// CSV文件编码格式
        /// </summary>
        private const string CSV_ENCODING = "UTF-8";
        private static ConfigManager _instance;
        public static ConfigManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ConfigManager();
                return _instance;
            }
        }

        // 存储配置表数据
        private Dictionary<string, AttributeConfig> _attributeConfigs;
        private Dictionary<int, LevelConfig> _levelConfigs;
        private Dictionary<int, EnemyExpConfig> _enemyExpConfigs;
        private Dictionary<string, TaskExpConfig> _taskExpConfigs;
        private Dictionary<string, ExploreExpConfig> _exploreExpConfigs;

        // 私有构造函数，防止外部实例化
        private ConfigManager()
        {
            _attributeConfigs = new Dictionary<string, AttributeConfig>();
            _levelConfigs = new Dictionary<int, LevelConfig>();
            _enemyExpConfigs = new Dictionary<int, EnemyExpConfig>();
            _taskExpConfigs = new Dictionary<string, TaskExpConfig>();
            _exploreExpConfigs = new Dictionary<string, ExploreExpConfig>();
        }

        /// <summary>
        /// 初始化配置管理器，加载所有配置表
        /// </summary>
        public void Initialize()
        {
            _attributeConfigs.Clear();
            _levelConfigs.Clear();
            _enemyExpConfigs.Clear();
            _taskExpConfigs.Clear();
            _exploreExpConfigs.Clear();

            LoadAttributeConfig();
            LoadLevelConfig();
            LoadExperienceConfig();

            GD.Print("配置管理器初始化完成");
        }

        /// <summary>
        /// 加载CSV文件并处理数据
        /// </summary>
        /// <param name="filePath">CSV文件路径</param>
        /// <param name="dataProcessor">数据处理委托</param>
        private void LoadCSVFile(string filePath, Action<string[], string[]> dataProcessor)
        {
            Godot.FileAccess file = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Read);
            if (file == null)
            {
                GD.PrintErr($"无法打开CSV文件: {filePath}");
                return;
            }

            // 读取标题行
            string headerLine = file.GetLine();
            string[] headers = headerLine.Split(',');

            // 读取数据行
            while (file.GetPosition() < file.GetLength())
            {
                string line = file.GetLine();
                if (string.IsNullOrEmpty(line))
                    continue;

                string[] values = line.Split(',');
                if (values.Length >= headers.Length)
                {
                    dataProcessor(headers, values);
                }
            }

            file.Close();
        }

        /// <summary>
        /// 根据字段名获取对应的值
        /// </summary>
        /// <param name="fields">字段名数组</param>
        /// <param name="values">值数组</param>
        /// <param name="fieldName">要查找的字段名</param>
        /// <returns>对应的值</returns>
        private string GetValue(string[] fields, string[] values, string fieldName)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].Trim().Equals(fieldName, StringComparison.OrdinalIgnoreCase))
                {
                    return i < values.Length ? values[i].Trim() : string.Empty;
                }
            }
            GD.PrintErr($"未找到字段: {fieldName}");
            return string.Empty;
        }

        /// <summary>
        /// 加载属性配置表
        /// </summary>
        private void LoadAttributeConfig()
        {
            string csvPath = "res://assets/configs/AttributeConfig.csv";
            LoadCSVFile(csvPath, (fields, values) =>
            {
                AttributeConfig config = new AttributeConfig();
                config.AttributeId = GetValue(fields, values, "AttributeId");
                config.AttributeName = GetValue(fields, values, "AttributeName");
                config.AttributeType = (AttributeType)Enum.Parse(typeof(AttributeType), GetValue(fields, values, "AttributeType"));
                config.BaseValue = float.Parse(GetValue(fields, values, "BaseValue"));
                config.GrowthRate = float.Parse(GetValue(fields, values, "GrowthRate"));
                config.Description = GetValue(fields, values, "Description");
                config.IconPath = GetValue(fields, values, "IconPath");
                config.SortOrder = int.Parse(GetValue(fields, values, "SortOrder"));
                _attributeConfigs[config.AttributeId] = config;
            });
            GD.Print($"已加载 {_attributeConfigs.Count} 条属性配置数据");
        }

        /// <summary>
        /// 加载等级配置表
        /// </summary>
        private void LoadLevelConfig()
        {
            string csvPath = "res://assets/configs/LevelConfig.csv";
            LoadCSVFile(csvPath, (fields, values) =>
            {
                LevelConfig config = new LevelConfig();
                config.Level = int.Parse(GetValue(fields, values, "Level"));
                config.ExpToNextLevel = float.Parse(GetValue(fields, values, "ExpToNextLevel"));
                config.HpGrowthBonus = float.Parse(GetValue(fields, values, "HpGrowthBonus"));
                config.MpGrowthBonus = float.Parse(GetValue(fields, values, "MpGrowthBonus"));
                config.AttackGrowthBonus = float.Parse(GetValue(fields, values, "AttackGrowthBonus"));
                config.DefenseGrowthBonus = float.Parse(GetValue(fields, values, "DefenseGrowthBonus"));
                config.SpeedGrowthBonus = float.Parse(GetValue(fields, values, "SpeedGrowthBonus"));
                config.UnlockFeature = GetValue(fields, values, "UnlockFeature");
                _levelConfigs[config.Level] = config;
            });
            GD.Print($"已加载 {_levelConfigs.Count} 条等级配置数据");
        }

        /// <summary>
        /// 加载经验配置表
        /// </summary>
        private void LoadExperienceConfig()
        {
            string csvPath = "res://assets/configs/ExperienceConfig.csv";
            LoadCSVFile(csvPath, (fields, values) =>
            {
                string type = GetValue(fields, values, "Type");
                switch (type)
                {
                    case "enemy":
                        EnemyExpConfig enemyConfig = new EnemyExpConfig();
                        enemyConfig.EnemyLevel = int.Parse(GetValue(fields, values, "Level"));
                        enemyConfig.EnemyBaseExp = float.Parse(GetValue(fields, values, "BaseExp"));
                        enemyConfig.DifficultyCoefficient = float.Parse(GetValue(fields, values, "ExpMultiplier"));
                        _enemyExpConfigs[enemyConfig.EnemyLevel] = enemyConfig;
                        break;
                    case "task":
                        TaskExpConfig taskConfig = new TaskExpConfig();
                        taskConfig.TaskId = GetValue(fields, values, "Id");
                        taskConfig.TaskBaseExp = float.Parse(GetValue(fields, values, "BaseExp"));
                        taskConfig.DifficultyCoefficient = float.Parse(GetValue(fields, values, "ExpMultiplier"));
                        _taskExpConfigs[taskConfig.TaskId] = taskConfig;
                        break;
                    case "explore":
                        ExploreExpConfig exploreConfig = new ExploreExpConfig();
                        exploreConfig.ExploreId = GetValue(fields, values, "Id");
                        exploreConfig.ExploreExp = float.Parse(GetValue(fields, values, "BaseExp"));
                        _exploreExpConfigs[exploreConfig.ExploreId] = exploreConfig;
                        break;
                }
            });
            GD.Print($"已加载 {_enemyExpConfigs.Count} 条敌人经验配置，{_taskExpConfigs.Count} 条任务经验配置，{_exploreExpConfigs.Count} 条探索经验配置");
        }

        /// <summary>
        /// 获取属性配置
        /// </summary>
        /// <param name="attributeId">属性ID</param>
        /// <returns>属性配置对象</returns>
        public AttributeConfig GetAttributeConfig(string attributeId)
        {
            if (_attributeConfigs.TryGetValue(attributeId, out var config))
                return config;
            GD.PrintErr($"未找到属性配置: {attributeId}");
            return null;
        }

        /// <summary>
        /// 获取等级配置
        /// </summary>
        /// <param name="level">等级</param>
        /// <returns>等级配置对象</returns>
        public LevelConfig GetLevelConfig(int level)
        {
            if (_levelConfigs.TryGetValue(level, out var config))
                return config;
            GD.PrintErr($"未找到等级配置: {level}");
            return null;
        }

        /// <summary>
        /// 获取敌人经验配置
        /// </summary>
        /// <param name="enemyLevel">敌人等级</param>
        /// <returns>敌人经验配置对象</returns>
        public EnemyExpConfig GetEnemyExpConfig(int enemyLevel)
        {
            if (_enemyExpConfigs.TryGetValue(enemyLevel, out var config))
                return config;
            GD.PrintErr($"未找到敌人经验配置: {enemyLevel}");
            return null;
        }

        /// <summary>
        /// 获取任务经验配置
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <returns>任务经验配置对象</returns>
        public TaskExpConfig GetTaskExpConfig(string taskId)
        {
            if (_taskExpConfigs.TryGetValue(taskId, out var config))
                return config;
            GD.PrintErr($"未找到任务经验配置: {taskId}");
            return null;
        }

        /// <summary>
        /// 获取探索经验配置
        /// </summary>
        /// <param name="exploreId">探索点ID</param>
        /// <returns>探索经验配置对象</returns>
        public ExploreExpConfig GetExploreExpConfig(string exploreId)
        {
            if (_exploreExpConfigs.TryGetValue(exploreId, out var config))
                return config;
            GD.PrintErr($"未找到探索经验配置: {exploreId}");
            return null;
        }
    }

    /// <summary>
    /// 属性类型枚举
    /// </summary>
    public enum AttributeType
    {
        Base,       // 基础属性
        Combat,     // 战斗属性
        Special     // 特殊属性
    }

    /// <summary>
    /// 属性配置类
    /// </summary>
    public class AttributeConfig
    {
        public string AttributeId { get; set; }
        public string AttributeName { get; set; }
        public AttributeType AttributeType { get; set; }
        public float BaseValue { get; set; }
        public float GrowthRate { get; set; }
        public string Description { get; set; }
        public string IconPath { get; set; }
        public int SortOrder { get; set; }
    }

    /// <summary>
    /// 等级配置类
    /// </summary>
    public class LevelConfig
    {
        public int Level { get; set; }
        public float ExpToNextLevel { get; set; }
        public float HpGrowthBonus { get; set; }
        public float MpGrowthBonus { get; set; }
        public float AttackGrowthBonus { get; set; }
        public float DefenseGrowthBonus { get; set; }
        public float SpeedGrowthBonus { get; set; }
        public string UnlockFeature { get; set; }
    }

    /// <summary>
    /// 敌人经验配置类
    /// </summary>
    public class EnemyExpConfig
    {
        public int EnemyLevel { get; set; }
        public float EnemyBaseExp { get; set; }
        public float DifficultyCoefficient { get; set; }
    }

    /// <summary>
    /// 任务经验配置类
    /// </summary>
    public class TaskExpConfig
    {
        public string TaskId { get; set; }
        public float TaskBaseExp { get; set; }
        public float DifficultyCoefficient { get; set; }
    }

    /// <summary>
    /// 探索经验配置类
    /// </summary>
    public class ExploreExpConfig
    {
        public string ExploreId { get; set; }
        public float ExploreExp { get; set; }
    }
}