using Godot;
using System;
using Godot;

using Mathf = Godot.Mathf;

namespace XiuxianDemo
{
    /// <summary>
    /// 玩家角色控制器类
    /// 处理玩家输入、移动逻辑、碰撞检测和精灵方向
    /// </summary>
    public partial class Player : CharacterBody2D
    {
        /// <summary>
        /// 正常移动速度
        /// </summary>
        [Export]
        public float MoveSpeed { get; set; } = 200.0f;
        
        /// <summary>
        /// 冲刺移动速度
        /// </summary>
        [Export]
        public float SprintSpeed { get; set; } = 350.0f;
        
        /// <summary>
        /// 当前移动速度（根据是否冲刺动态变化）
        /// </summary>
        private float _currentSpeed;

        /// <summary>
        /// 输入方向向量
        /// </summary>
        private Vector2 _inputDirection = Vector2.Zero;

        /// <summary>
        /// 角色精灵节点引用
        /// </summary>
        private Sprite2D _sprite;

        /// <summary>
        /// 动画播放器引用
        /// </summary>
        private AnimationPlayer _animationPlayer;

        /// <summary>
        /// 节点就绪时调用的方法
        /// 初始化角色属性、获取子节点引用
        /// </summary>
        public override void _Ready()
        {
            _currentSpeed = MoveSpeed; // 初始化为正常移动速度
            _sprite = GetNode<Sprite2D>("Sprite2D"); // 获取精灵节点
            _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer"); // 获取动画播放器

            // 确保相机跟随
            Camera2D camera = GetNode<Camera2D>("Camera2D");
            if (camera != null)
            {
                camera.MakeCurrent(); // 设置当前相机
            }
        }
        
        /// <summary>
        /// 物理帧更新方法
        /// 每帧调用，处理输入、移动和精灵方向
        /// </summary>
        /// <param name="delta">时间增量，自上一帧以来的时间</param>
        public override void _PhysicsProcess(double delta)
        {
            HandleInput(); // 处理玩家输入
            HandleMovement(delta); // 处理移动逻辑
            UpdateSpriteDirection(); // 更新精灵方向
        }
        
        /// <summary>
        /// 处理玩家输入
        /// 检测键盘输入并更新输入方向向量
        /// </summary>
        private void HandleInput()
        {
            _inputDirection = Vector2.Zero; // 重置输入方向

            // 获取输入方向
            if (Input.IsActionPressed("ui_up"))
            {
                _inputDirection.Y -= 1;
                GD.Print("上方向键被按下");
            }
            if (Input.IsActionPressed("ui_down"))
            {
                _inputDirection.Y += 1;
                GD.Print("下方向键被按下");
            }
            if (Input.IsActionPressed("ui_left"))
            {
                _inputDirection.X -= 1;
                GD.Print("左方向键被按下");
            }
            if (Input.IsActionPressed("ui_right"))
            {
                _inputDirection.X += 1;
                GD.Print("右方向键被按下");
            }

            // 归一化输入向量，确保斜向移动速度与轴向移动速度一致
            if (_inputDirection.Length() > 0)
            {
                _inputDirection = _inputDirection.Normalized();
                GD.Print("输入方向: " + _inputDirection);
            }

            // 处理冲刺状态
            if (Input.IsActionPressed("sprint"))
            {
                _currentSpeed = SprintSpeed;
                GD.Print("冲刺状态激活，速度: " + _currentSpeed);
            }
            else
            {
                _currentSpeed = MoveSpeed;
                GD.Print("正常速度: " + _currentSpeed);
            }
        }
        
        /// <summary>
        /// 处理角色移动
        /// 根据输入方向和速度更新角色位置
        /// </summary>
        /// <param name="delta">时间增量，自上一帧以来的时间</param>
        private void HandleMovement(double delta)
        {
            if (_inputDirection.Length() > 0)
            {
                Vector2 velocity = _inputDirection * _currentSpeed; // 计算移动速度
                Velocity = velocity; // 设置角色速度
                GD.Print("移动速度: " + velocity);
                MoveAndSlide(); // 应用移动并处理碰撞
            }
            else
            {
                Velocity = Vector2.Zero; // 停止移动
                GD.Print("静止不动");
            }
        }
        
        /// <summary>
        /// 更新精灵方向
        /// 根据移动方向调整精灵的朝向
        /// </summary>
        private void UpdateSpriteDirection()
        {
            if (_sprite == null) return; // 检查精灵是否存在

            if (_inputDirection.Length() > 0)
            {
                // 根据移动方向设置精灵方向
                if (Mathf.Abs(_inputDirection.X) > Mathf.Abs(_inputDirection.Y))
                {
                    // 左右方向优先
                    _sprite.FlipH = _inputDirection.X > 0; // 当X轴输入为正时翻转精灵
                }
                // 对于上下方向，保持精灵方向不变
            }
            // else
            // {
            //     // 静止状态，显示最后一个方向的动画帧
            //     // 暂时注释，因为没有配置动画
            //     // _animationPlayer.Play("idle");
            // }
        }
    }
}