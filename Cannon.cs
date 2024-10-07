using Godot;
using System;

public partial class Cannon : Actor
{
	public Vector2 Pos;		//出現位置

	//初期化処理
	public override void Initialize()
	{
		Pos = Position;
		GetNode<Timer>("VisibleTimer").Start();
	}

	//更新処理
	public override void Update()
	{
		Position = Pos;
	}

	//衝突時の処理　layerで種別を判別
	public override void OnCollision(uint layer) {}
	
	//時間外になると削除
	private void _on_timer_timeout()
	{
		IsActive = false;
	}

}


