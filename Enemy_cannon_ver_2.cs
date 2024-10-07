using Godot;
using System;

public partial class Enemy_cannon_ver_2 : Actor
{
	public float PosY;

	//初期化処理
	public override void Initialize()
	{
		MoveVec = new Vector2(-8.0f,0);
		
		PosY = Position.Y;
	}

	//更新処理
	public override void Update()
	{
		Position = new Vector2(Position.X,PosY);
	}

	//衝突時の処理　layerで種別を判別
	public override void OnCollision(uint layer)
	{
		if(layer == 1) {
			IsActive = false;
		}
	}
	
	//画面外で削除
	private void _on_visible_on_screen_notifier_2d_screen_exited()
	{
		IsActive = false;
	}

}

