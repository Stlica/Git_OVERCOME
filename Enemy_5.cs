using Godot;
using System;

public partial class Enemy_5 : Enemy
{
	//初期化処理
	public override void Initialize() 
	{
		MoveVec = new Vector2(2,0);
	}

	//更新処理
	public override void Update() 
	{
		if(Position.X > 1250.0f){
			IsActive = false;
		}
	}

	//衝突時の処理　layerで種別を判別
	public override void OnCollision(uint layer) 
	{
		if(layer != 1 && !IsHitCan) {
			IsHitCan = true;
			Main.AddScore(100);
		}
		IsActive = false;
	}
	
	//弾の発射依頼
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




