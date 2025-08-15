using Godot;
using System;
using System.Collections.Generic;

namespace XiuXianDemo.Characters
{
    /// <summary>
    /// 玩家角色类
    /// </summary>
    public partial class Player : CharacterBody2D
    {
        [Export] public float Speed { get; set; } = 300.0f;
        
        private CharacterAttributes _attributes;
        private Sprite2D _sprite;
        private AnimationPlayer _animationPlayer;
        private Label _nameLabel;
        private ProgressBar _hpBar;
        private ProgressBar _mpBar;

        /// <summary>
        /// 角色名称
        /// </summary>
        [Export] public string CharacterName { get; set; } = "Player";

        /// <summary>
        /// 角色属性系统
        /// </summary>
        public CharacterAttributes Attributes => _attributes;

        public override void _Ready()
        {
            InitializeComponents();
            InitializeAttributes();
            SetupUI();
        }

        private void InitializeComponents()
        {
            _sprite = GetNode<Sprite2D>("Sprite2D");
            _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            _nameLabel = GetNode<Label>("UI/NameLabel");
            _hpBar = GetNode<ProgressBar>("UI/HPBar");
            _mpBar = GetNode<ProgressBar>("UI/MPBar");
        }

        private void InitializeAttributes()
        {
            _attributes = new CharacterAttributes();
            
            // 连接属性变化事件
            _attributes.OnAttributeChanged += (attributeId, oldValue, newValue) => {
                if (attributeId.ToLower() == "hp" || attributeId.ToLower() == "health")
                {
                    UpdateHealthUI();
                }
                else if (attributeId.ToLower() == "mp" || attributeId.ToLower() == "mana")
                {
                    UpdateManaUI();
                }
            };
            
            _attributes.OnLevelUp += (newLevel, attributeChanges) => {
                GD.Print($"玩家升级到 {newLevel} 级！");
                // 可以在这里添加升级特效或其他逻辑
            };
        }

        private void SetupUI()
        {
            if (_nameLabel != null)
                _nameLabel.Text = CharacterName;
            
            UpdateHealthUI();
            UpdateManaUI();
        }

        public override void _PhysicsProcess(double delta)
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            Vector2 velocity = Vector2.Zero;

            if (Input.IsActionPressed("ui_right"))
                velocity.X += 1.0f;
            if (Input.IsActionPressed("ui_left"))
                velocity.X -= 1.0f;
            if (Input.IsActionPressed("ui_down"))
                velocity.Y += 1.0f;
            if (Input.IsActionPressed("ui_up"))
                velocity.Y -= 1.0f;

            if (velocity.Length() > 0)
            {
                velocity = velocity.Normalized() * Speed;
                PlayAnimation("walk");
            }
            else
            {
                PlayAnimation("idle");
            }

            Velocity = velocity;
            MoveAndSlide();
        }

        private void PlayAnimation(string animationName)
        {
            if (_animationPlayer != null && _animationPlayer.HasAnimation(animationName))
            {
                if (_animationPlayer.CurrentAnimation != animationName)
                {
                    _animationPlayer.Play(animationName);
                }
            }
        }

        // 移除旧的事件处理方法
        // private void OnAttributeChanged(string attributeId, float oldValue, float newValue)
        // {
        //     switch (attributeId.ToLower())
        //     {
        //         case "hp":
        //         case "health":
        //             UpdateHealthUI();
        //             break;
        //         case "mp":
        //         case "mana":
        //             UpdateManaUI();
        //             break;
        //     }
        // }

        // private void OnLevelUp(int newLevel)
        // {
        //     GD.Print($"玩家升级到 {newLevel} 级！");
        //     // 可以在这里添加升级特效或其他逻辑
        // }

        private void UpdateHealthUI()
        {
            if (_hpBar != null && _attributes != null)
            {
                float maxHp = (float)_attributes.GetAttribute("hp");
                float currentHp = (float)_attributes.GetAttribute("hp");
                _hpBar.MaxValue = maxHp;
                _hpBar.Value = currentHp;
            }
        }

        private void UpdateManaUI()
        {
            if (_mpBar != null && _attributes != null)
            {
                float maxMp = (float)_attributes.GetAttribute("mp");
                float currentMp = (float)_attributes.GetAttribute("mp");
                _mpBar.MaxValue = maxMp;
                _mpBar.Value = currentMp;
            }
        }

        /// <summary>
        /// 添加经验值
        /// </summary>
        /// <param name="amount">经验值</param>
        public void AddExperience(float amount)
        {
            _attributes?.AddExperience(amount);
        }

        /// <summary>
        /// 受到伤害
        /// </summary>
        /// <param name="damage">伤害值</param>
        public void TakeDamage(float damage)
        {
            if (_attributes != null)
            {
                float currentHp = (float)_attributes.GetAttribute("hp");
                float newHp = Math.Max(0, currentHp - damage);
                _attributes.SetAttribute("hp", newHp);
                
                if (newHp <= 0)
                {
                    Die();
                }
            }
        }

        /// <summary>
        /// 角色死亡
        /// </summary>
        private void Die()
        {
            GD.Print("玩家死亡！");
            // 可以在这里添加死亡动画或游戏结束逻辑
            QueueFree();
        }

        /// <summary>
        /// 恢复生命值
        /// </summary>
        /// <param name="amount">恢复量</param>
        public void Heal(float amount)
        {
            if (_attributes != null)
            {
                float maxHp = 100.0f; // 默认最大生命值
                float currentHp = (float)_attributes.GetAttribute("hp");
                float newHp = Math.Min(maxHp, currentHp + amount);
                _attributes.SetAttribute("hp", newHp);
            }
        }

        /// <summary>
        /// 恢复魔法值
        /// </summary>
        /// <param name="amount">恢复量</param>
        public void RestoreMana(float amount)
        {
            if (_attributes != null)
            {
                float maxMp = 50.0f; // 默认最大魔法值
                float currentMp = (float)_attributes.GetAttribute("mp");
                float newMp = Math.Min(maxMp, currentMp + amount);
                _attributes.SetAttribute("mp", newMp);
            }
        }
    }
}