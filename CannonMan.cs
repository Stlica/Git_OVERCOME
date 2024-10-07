using Godot;
using System;

public partial class CannonMan : Actor
{
	[Export]
	public PackedScene Cannon;
	
	//初期化処理
	public override void Initialize()
	{
		GetNode<Timer>("CannonTimer").Start();
		GetNode<AnimatedSprite2D>("Cannon").Hide();
		CannonManChargeSound();
	}
	
	//キャノンのチャージ音
	private void _on_cannon_timer_timeout()
	{
		CannonManChargeSoundStop();
		GetNode<AnimatedSprite2D>("Cannon").Show();
		Shoot(CannonPos());
	}
	
	//キャノン発射
	public void Shoot(Vector2 pos)
	{
		GetNode<Timer>("ShootingTimer").Start();
		CannonManCannonSound();
		var ShotCannon = (Cannon)Cannon.Instantiate();
		ShotCannon.Position = pos;
		Main.AddChild(ShotCannon);
	}

	//キャノンの位置
	private Vector2 CannonPos()
	{
		var Cannon = GetNode<Node2D>("CannonPos");
		var CannonPosition = Position + Cannon.Position;
		return CannonPosition;
	} 
	
	//キャノンの削除
	private void _on_shooting_timer_timeout()
	{
		IsActive = false;
		CannonManCannonSoundStop();
	}
}



