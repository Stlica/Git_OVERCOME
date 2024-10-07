using Godot;
using System;

public partial class Back_graund_space : Node2D
{
	// シーンが生成された時に1回呼ばれる処理
	public override void _Ready() {}

	//60分の1秒に1回呼ばれる処理
	public override void _Process(double delta)
	{
		//画像の移動量
		var Vec = new Vector2(-1, 0);
		
		//移動をする
		Position += Vec;

		//画像が見切れそうになったら戻す
		if(Position.X < -1225.0f) {
			Position = new Vector2(1250.0f, 0.0f);
		}
	}
}



