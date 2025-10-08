using Godot;
using System;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

public partial class Player : CharacterBody2D
{

	private float gravity = (float)ProjectSettings.GetSetting("physics/2d/default_gravity");


	private float _currentSlowmotion;
	private bool _isJumpCharging = false;
	private bool _canMove = true;
	private bool _isClinging = false;
	private bool _movementStopped;
	private bool _isProjectionFlipped = false;
	private Timer _timer;

	// Whether or not the player is currently hiding in a bush.
	public bool IsStealthed = false;


	[ExportCategory("Jump Settings")]
	[Export] private float _jumpVelocity;
	[Export] private float _flippingSpeed;
	[Export] private Sprite2D _jumpProjection;
	[Export] private Sprite2D _jumpProjectionSprite;
	[Export] private Node2D _jumpTarget;


	[ExportCategory("Aim Settings")]
	[Export] private Sprite2D _aimProjection;
	[Export] private Sprite2D _aimProjectionSprite;
	[Export] private Node2D _fireLocation;


	[ExportCategory("Bullets")]
	[Export] private PackedScene _bulletScene;
	private bool _hasBullet = true;
	private int _bullets;
	private TextureRect _bulletTexRect1, _bulletTexRect2;


	[ExportCategory("Player Sprites")]
	private Sprite2D _chargeSprite, _idleSprite, _flippingSprite, _jumpingSprite;


	[ExportCategory("Wall Raycasts")]
	private RayCast2D _leftRay, _rightRay;


	public override void _Ready()
	{
		#region bullets
		_bulletTexRect1 = GetParent().GetNode("UI").GetNode<TextureRect>("BulletCount1");
		_bulletTexRect2 = GetParent().GetNode("UI").GetNode<TextureRect>("BulletCount2");
		#endregion

		#region sprites
		_chargeSprite = GetNode("Sprites").GetNode<Sprite2D>("ChargeSprite");
		_idleSprite = GetNode("Sprites").GetNode<Sprite2D>("IdleSprite");
		_flippingSprite = GetNode("Sprites").GetNode<Sprite2D>("FlippingSprite");
		_jumpingSprite = GetNode("Sprites").GetNode<Sprite2D>("JumpingSprite");
		#endregion

		#region wall raycasts
		_leftRay = GetNode("WallRays").GetNode<RayCast2D>("LeftRay");
		_rightRay = GetNode("WallRays").GetNode<RayCast2D>("RightRay");
		#endregion

		_timer = GetNode<Timer>("Timer");

		_aimProjectionSprite.Visible = false;

		Engine.TimeScale = 1;
	}

	public override void _Process(double delta)
	{
		if (!GameStateManager.Instance.GetState(GameState.Paused) && !GameStateManager.Instance.GetState(GameState.LevelLost) && !GameStateManager.Instance.GetState(GameState.LevelWon))
		{
			if (IsOnFloor())
			{
				if (!_isJumpCharging)
				{
					IdleSprite();
				}
				if (_bullets < 2)
				{
					RestoreBullets();
				}
				if (Engine.TimeScale < 1)
				{
					Engine.TimeScale = 1;
					_aimProjectionSprite.Visible = false;
				}
				if (!_canMove)
				{
					Velocity = new Vector2(0, Velocity.Y);
				}
				if (Input.IsActionPressed("ChargeJump"))
				{
					ChargeJump();
				}
				else if (Input.IsActionJustReleased("ChargeJump"))
				{
					Jump();
				}
			}
			else if (!IsOnFloor())
			{
				Velocity = new Vector2(Velocity.X, Velocity.Y + gravity * (float)delta);
				if (Input.IsActionPressed("Aim"))
				{
					if (_bullets > 0)
					{
						if (Engine.TimeScale == 1)
						{
							Engine.TimeScale = 0.2f;
							_aimProjectionSprite.Visible = true;
						}

						if (Velocity.X > 0)
						{
							FlippingSprite();
							_flippingSprite.RotationDegrees += _flippingSpeed * (float)delta;
						}
						else
						{
							FlippingSprite();
							_flippingSprite.RotationDegrees -= _flippingSpeed * (float)delta;
						}
						_aimProjection.LookAt(GetGlobalMousePosition());
					}
					else
					{
						_aimProjectionSprite.Visible = false;
						if (Engine.TimeScale < 1)
						{
							Engine.TimeScale = 1;
						}
					}
					if (Input.IsActionJustPressed("Shoot"))
					{
						GD.Print("Shoot!");
						if (Engine.TimeScale < 1)
						{
							Shoot();
							Engine.TimeScale = 1;
							_aimProjectionSprite.Visible = false;
						}
					}
				}
				else if (Input.IsActionJustReleased("Aim"))
				{
					GD.Print("Stopped flipping!");
					if (Engine.TimeScale < 1)
					{
						Engine.TimeScale = 1;
						_aimProjectionSprite.Visible = false;
					}
				}
				else if (Input.IsActionPressed("ChargeJump"))
				{
					if (_leftRay.IsColliding())
					{
						_isClinging = true;
						ChargeJump();
					}
					else if (_rightRay.IsColliding())
					{
						_isClinging = true;
						ChargeJump();
					}
				}
				else if (Input.IsActionJustReleased("ChargeJump") && _isClinging)
				{
					_isClinging = false;
					Jump();
				}
			}
		}
	}

    public override void _PhysicsProcess(double delta)
    {
		if (!GameStateManager.Instance.GetState(GameState.Paused) && !GameStateManager.Instance.GetState(GameState.LevelLost) && !GameStateManager.Instance.GetState(GameState.LevelWon) && !_isClinging)
		{
			MoveAndSlide();
		}
    }

	private void ChargeJump()
	{
		_isJumpCharging = true;
		ChargeSprite();
		_jumpProjectionSprite.Visible = true;
		_jumpProjectionSprite.Scale = new Vector2(Position.DistanceTo(GetGlobalMousePosition()) * 0.005f, Position.DistanceTo(GetGlobalMousePosition()) * 0.005f);

		_jumpProjection.LookAt(GetGlobalMousePosition());
	}

	private void Jump()
	{
		JumpingSprite();
		_canMove = true;
		_timer.Start();
		_jumpProjectionSprite.Visible = false;
		var Direction = Position.DirectionTo(_jumpTarget.GlobalPosition);
		Velocity = Direction.Normalized() * Position.DistanceTo(GetGlobalMousePosition());

		if (Velocity.X > 0)
		{
			_jumpingSprite.Scale = new Vector2(2.3f, _jumpingSprite.Scale.Y);
		}
		else
		{
			_jumpingSprite.Scale = new Vector2(-2.3f, _jumpingSprite.Scale.Y);
		}
	}


	private void Shoot()
	{
		if (_hasBullet)
		{
			LoseBullet();
			Node2D Bullet = (Node2D)ResourceLoader.Load<PackedScene>(_bulletScene.ResourcePath).Instantiate();
			AddChild(Bullet);
			Bullet.Transform = _fireLocation.GlobalTransform;
			var Direction = Position.DirectionTo(_fireLocation.GlobalPosition);
			Velocity = Direction.Normalized() * -_jumpVelocity;
		}
	}

	public void GetBullet()
	{
		_bullets++;
		CheckBullets();
	}

	private void RestoreBullets()
	{
		_bullets = 2;
		CheckBullets();
	}

	private void LoseBullet()
	{
		_bullets--;
		CheckBullets();
	}

	private void CheckBullets()
	{
		if (_bullets <= 0)
		{
			_bullets = 0;
			_hasBullet = false;
		}
		else if (_bullets > 0)
		{
			_hasBullet = true;
		}
		if (_bullets >= 2)
		{
			_bullets = 2;
		}

		UpdateUI();

	}

	public void Die()
	{
		GameManager.Instance.LoseLevel();
	}

	private void UpdateUI()
	{
		switch (_bullets)
		{
			case 0:
				_bulletTexRect1.Visible = false;
				_bulletTexRect2.Visible = false;
				break;
			case 1:
				_bulletTexRect1.Visible = true;
				_bulletTexRect2.Visible = false;
				break;
			case 2:
				_bulletTexRect1.Visible = true;
				_bulletTexRect2.Visible = true;
				break;
		}
	}

	private void OnTimerTimeout()
	{
		_canMove = false;
		_isJumpCharging = false;
	}

	private void ChargeSprite()
	{
		_chargeSprite.Visible = true;
		_idleSprite.Visible = false;
		_flippingSprite.Visible = false;
		_jumpingSprite.Visible = false;
	}

	private void IdleSprite()
	{
		_chargeSprite.Visible = false;
		_idleSprite.Visible = true;
		_flippingSprite.Visible = false;
		_jumpingSprite.Visible = false;
	}

	private void FlippingSprite()
	{
		_chargeSprite.Visible = false;
		_idleSprite.Visible = false;
		_flippingSprite.Visible = true;
		_jumpingSprite.Visible = false;
	}

	private void JumpingSprite()
	{
		_chargeSprite.Visible = false;
		_idleSprite.Visible = false;
		_flippingSprite.Visible = false;
		_jumpingSprite.Visible = true;
	}
}
