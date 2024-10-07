using Godot;
using System;

public partial class Explosion : Actor
{
	//初期化処理
	public override void Initialize()
	{
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("fire");
	}

	//更新処理
	public override void Update()
	{
		//アニメーションが終了したら消去
		if(!GetNode<AnimatedSprite2D>("AnimatedSprite2D").IsPlaying())
		{
			QueueFree();
		}
	}

	//衝突時の処理　layerで種別を判別
	public override void OnCollision(uint layer){}
}
