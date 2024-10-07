using Godot;
using System;

public partial class Mini_meteo : Enemy
{
	static int num = 0;	//分裂数

	//初期化処理
	public override void Initialize()
	{
		HitPoint = 5;

		switch(num++ % 3){
			case 0:
				MoveVec = new Vector2(-2.0f,0.0f);
				break;
			case 1:
				MoveVec = new Vector2(-2.0f,1.0f);
				break;
			case 2:
				MoveVec = new Vector2(-2.0f,-1.0f);
				break;
		}
	}

	//更新処理
	public override void Update()
	{
		if(Position.X < 0.0f){
			IsActive = false;
		}
	}

	//衝突時の処理　layerで種別を判別
	public override void OnCollision(uint layer)
	{
		if(layer == 1 || layer == 128){	//プレイヤーorキャノン
			IsActive = false;
		}else if(layer == 2){			//弾
			HitPoint--;
			if(HitPoint <= 0){
				IsActive = false;
				Main.AddScore(5);
			}
		}
	}

	//画面外に出たら消去
	private void _on_visible_on_screen_notifier_2d_screen_exited()
	{
		IsActive = false;
	}
}


