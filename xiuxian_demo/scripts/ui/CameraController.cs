using Godot;
using System;

namespace XiuXianDemo.UI
{
    /// <summary>
    /// 相机控制器类
    /// 处理相机跟随目标、平滑移动和边界限制
    /// </summary>
    public partial class CameraController : Camera2D
    {
        /// <summary>
        /// 目标节点路径
        /// </summary>
        [Export]
        public NodePath TargetPath { get; set; }
        
        /// <summary>
        /// 平滑移动速度
        /// </summary>
        [Export]
        public float SmoothSpeed { get; set; } = 5.0f;
        
        /// <summary>
        /// 相机偏移量
        /// </summary>
        [Export]
        public Vector2 CameraOffset { get; set; } = Vector2.Zero;
        
        /// <summary>
        /// 是否限制相机边界
        /// </summary>
        [Export]
        public bool LimitBounds { get; set; } = true;
        
        /// <summary>
        /// 相机边界矩形
        /// </summary>
        [Export]
        public Rect2 Bounds { get; set; } = new Rect2(0, 0, 1920, 1080);
        
        /// <summary>
        /// 跟随目标节点
        /// </summary>
        private Node2D _target;

        /// <summary>
        /// 目标位置
        /// </summary>
        private Vector2 _targetPosition;
        
        /// <summary>
        /// 节点就绪时调用的方法
        /// 初始化相机设置和目标引用
        /// </summary>
        public override void _Ready()
        {
            if (!string.IsNullOrEmpty(TargetPath))
            {
                _target = GetNode<Node2D>(TargetPath); // 获取目标节点
            }
            
            MakeCurrent(); // 设置为当前相机
        }
        
        /// <summary>
        /// 每帧更新方法
        /// 处理相机跟随目标、边界限制和平滑移动
        /// </summary>
        /// <param name="delta">时间增量，自上一帧以来的时间</param>
        public override void _Process(double delta)
        {
            if (_target == null) return; // 检查目标是否存在
            
            _targetPosition = _target.GlobalPosition + CameraOffset; // 计算目标位置加上偏移量
            
            if (LimitBounds)
            {
                // 限制相机在边界内
                _targetPosition.X = Mathf.Clamp(_targetPosition.X, Bounds.Position.X, Bounds.End.X);
                _targetPosition.Y = Mathf.Clamp(_targetPosition.Y, Bounds.Position.Y, Bounds.End.Y);
            }
            
            // 平滑移动相机到目标位置
            Vector2 smoothedPosition = GlobalPosition.Lerp(_targetPosition, (float)delta * SmoothSpeed);
            GlobalPosition = smoothedPosition; // 更新相机位置
        }
        
        /// <summary>
        /// 设置相机跟随目标
        /// </summary>
        /// <param name="newTarget">新的跟随目标节点</param>
        public void SetTarget(Node2D newTarget)
        {
            _target = newTarget;
        }
        
        /// <summary>
        /// 设置相机边界
        /// </summary>
        /// <param name="newBounds">新的相机边界矩形</param>
        public void SetBounds(Rect2 newBounds)
        {
            Bounds = newBounds;
        }
    }
}