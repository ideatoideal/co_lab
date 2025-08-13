using Godot;
using System;
using Godot;

using Mathf = Godot.Mathf;

namespace XiuxianDemo
{
    /// <summary>
    /// 世界地图管理器
    /// 处理地图背景设置、边界检查和玩家位置限制
    /// </summary>
    public partial class WorldMapManager : Node2D
    {
        /// <summary>
        /// 地图大小
        /// </summary>
        [Export]
        public Vector2 MapSize { get; set; } = new Vector2(1920, 1080);
        
        /// <summary>
        /// 地图边界颜色
        /// </summary>
        [Export]
        public Color MapBoundsColor { get; set; } = new Color(1, 0, 0, 0.5f);
        
        /// <summary>
        /// 地图背景精灵
        /// </summary>
        private Sprite2D _mapBackground;

        /// <summary>
        /// 玩家引用
        /// </summary>
        private Player _player;
        
        /// <summary>
        /// 节点就绪时调用的方法
        /// 初始化地图管理器，获取地图背景和玩家引用
        /// </summary>
        public override void _Ready()
        {
            _mapBackground = GetNode<Sprite2D>("MapBackground"); // 获取地图背景精灵
            _player = GetNode<Player>("Player"); // 获取玩家节点
            
            if (_mapBackground == null)
            {
                GD.PrintErr("WorldMapManager: MapBackground not found!");
            }
            
            if (_player == null)
            {
                GD.PrintErr("WorldMapManager: Player not found!");
            }
            
            SetupMapBackground(); // 设置地图背景
        }
        
        /// <summary>
        /// 每帧更新方法
        /// 检查并限制玩家在地图边界内
        /// </summary>
        /// <param name="delta">时间增量，自上一帧以来的时间</param>
        public override void _Process(double delta)
        {
            if (_player != null)
            {
                CheckMapBounds(); // 检查地图边界
            }
        }
        
        /// <summary>
        /// 设置地图背景
        /// 初始化地图背景位置和相机边界
        /// </summary>
        private void SetupMapBackground()
        {
            if (_mapBackground == null) return; // 检查地图背景是否存在

            // 确保地图背景居中并适应地图大小
            _mapBackground.Centered = true;
            _mapBackground.Position = MapSize / 2;

            // 如果需要调整地图背景大小以适应MapSize
            // 可以在这里添加缩放逻辑

            // 设置相机边界
            Camera2D camera = GetViewport().GetCamera2D();
            if (camera is CameraController cameraController)
            {
                cameraController.SetBounds(new Rect2(0, 0, MapSize.X, MapSize.Y));
            }

            GD.Print("WorldMapManager: Map background setup completed.");
        }
        
        /// <summary>
        /// 检查地图边界
        /// 确保玩家不会移出地图可见区域
        /// </summary>
        private void CheckMapBounds()
        {
            if (_player == null) return; // 检查玩家是否存在
            
            Vector2 playerPos = _player.GlobalPosition; // 获取玩家全局位置
            Vector2 screenSize = GetViewportRect().Size; // 获取视口大小
            
            // 计算地图边界（考虑相机居中）
            float minX = screenSize.X / 2;
            float maxX = MapSize.X - screenSize.X / 2;
            float minY = screenSize.Y / 2;
            float maxY = MapSize.Y - screenSize.Y / 2;
            
            // 限制玩家在地图边界内
            float clampedX = Mathf.Clamp(playerPos.X, Mathf.Min(minX, maxX), Mathf.Max(minX, maxX));
            float clampedY = Mathf.Clamp(playerPos.Y, Mathf.Min(minY, maxY), Mathf.Max(minY, maxY));
            _player.GlobalPosition = new Vector2(clampedX, clampedY);
        }
        
        /// <summary>
        /// 检查位置是否在地图边界内
        /// </summary>
        /// <param name="position">要检查的位置</param>
        /// <returns>如果位置在边界内返回true，否则返回false</returns>
        public bool IsPositionInBounds(Vector2 position)
        {
            return position.X >= 0 && position.X <= MapSize.X &&
                   position.Y >= 0 && position.Y <= MapSize.Y;
        }
        
        /// <summary>
        /// 获取地图内的随机有效位置
        /// </summary>
        /// <returns>地图内的随机位置向量</returns>
        public Vector2 GetRandomValidPosition()
        {
            Random random = new Random();
            float x = (float)random.NextDouble() * MapSize.X;
            float y = (float)random.NextDouble() * MapSize.Y;
            return new Vector2(x, y);
        }
    }
}