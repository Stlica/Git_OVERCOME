using Godot;
using System;

public partial class Smoke : Actor
{
	//初期化処理
	public override void Initialize()
	{
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("smoke");
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
}
