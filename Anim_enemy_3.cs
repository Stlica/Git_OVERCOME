using Godot;
using System;

public partial class Anim_enemy_3 : Actor
{
	//初期化処理
	public override void Initialize() 
	{
		MoveVec = new Vector2(-2.0f,0);
	}

	//更新処理
	public override void Update() {}

	//衝突時の処理　layerで種別を判別
	public override void OnCollision(uint layer) {}
	
	//画面外で削除
	private void _on_visible_on_screen_notifier_2d_screen_exited()
	{
		IsActive = false;
	}
}



