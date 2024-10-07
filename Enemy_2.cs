using Godot;
using System;

public partial class Enemy_2 : Enemy
{
	//初期化処理
	public override void Initialize() 
	{
		IsHitCan = false;
		MoveVec.X = -6.0f;
		
		if(Position.Y < ScreenSize.Y * 0.5f)
		{
			MoveVec.Y = 2.0f;
		} else {
			MoveVec.Y = -2.0f;
		}
		
		if( Level ==2 ){
			GetNode<Timer>("ShootTimer").WaitTime = 0.3;
		} else if( Level >= 3){
			GetNode<Timer>("ShootTimer").WaitTime = 0.2;
		}
	}

	//更新処理
	public override void Update() 
	{
		if(Position.X <= 0.0f){
			IsActive = false;
		}
	}

	//衝突時の処理　layerで種別を判別
	public override void OnCollision(uint layer) 
	{
		if(layer != 1 && !IsHitCan){
			IsHitCan = true;
			Main.AddScore(50);
		}
		IsActive = false;
	}
	
	//切り返す挙動
	private void _on_escape_timer_timeout()
	{
		AcceleVec.X = 0.3f;
		GetNode<Timer>("ShootTimer").Start();
	}
	
	//プレイヤーに弾を発射
	private void _on_shoot_timer_timeout()
	{
		var player = Main.GetPlayer();
		var target = player.Position;
		Shoot(target);
	}

	//画面外で削除
	private void _on_visible_on_screen_notifier_2d_screen_exited()
	{
		IsActive = false;
	}

}







