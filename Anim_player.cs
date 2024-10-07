using Godot;
using System;

public partial class Anim_player : Actor
{
	static bool PlayerAnimFlag = false;	//プレイヤーアニメーションが動いているか
	static bool EnemyAnimFlag = false;	//エネミーアニメーションが動いているか
	
	//初期化処理
	public override void Initialize() 
	{
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("default");
		if(!PlayerAnimFlag){
			MoveVec = new Vector2(2.0f,0);
			PlayerAnimFlag = true;
		}else{
			MoveVec = new Vector2(-2.0f,0);
			PlayerAnimFlag = false;
		}
		
	}

	//更新処理
	public override void Update() {}

	//衝突時の処理　layerで種別を判別
	public override void OnCollision(uint layer) {}
	
	//画面外で削除し、アニメーションタイマーをスタート
	private void _on_visible_on_screen_notifier_2d_screen_exited()
	{
		IsActive = false;
		if(!EnemyAnimFlag){
			Main.GetNode<Timer>("AnimTimer2").Start();
			EnemyAnimFlag = true;
		}else{
			EnemyAnimFlag = false;
		}
		
	}
}



