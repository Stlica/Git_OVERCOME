using Godot;
using System;

public partial class PowerCube : Enemy
{
	//初期化処理
	public override void Initialize() 
	{
		MoveVec = new Vector2(-2,0);
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("default");
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
		Main.GetPlayer().LevelUp();
		IsActive = false;
	}

	//画面外に出たら消去
	private void _on_visible_on_screen_notifier_2d_screen_exited()
	{
		IsActive = false;
	}

}

