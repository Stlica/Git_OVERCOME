using Godot;
using System;

public partial class Meteo : Enemy
{
	[Export]
	public PackedScene MiniMeteo;

	//初期化処理
	public override void Initialize()
	{
		IsHitCan = false;

		HitPoint = 10;
		MoveVec = new Vector2(-2.0f,0.0f);
		GetNode<AnimatedSprite2D>("Anim").Play("default");
	}

	//更新処理
	public override void Update()
	{
		if(Position.X < 0.0f){
			IsActive = false;
		}
	}

	//衝突時の処理　layerで種別を判別
	public override void OnCollision(uint layer) {
		if(layer == 2){
			HitPoint--;
			if(HitPoint <=0){
				if(Main.StageNum>=3) {
					for(int i = 0; i < 3; i++)
						Shoot_Mini(Position);
				}
				Main.AddScore(10);
				IsActive = false;
			}
		}
		else if(layer == 1 || layer == 128)
		{
			if(layer == 128 && !IsHitCan) {
				IsHitCan = true;
				Main.AddScore(10);
			}
			IsActive = false;
		}
	}

	public void Shoot_Mini(Vector2 pos)
	{
		var mini = (Mini_meteo)MiniMeteo.Instantiate();
		mini.Position = pos;
		Main.AddChild(mini);
	}

	private void _on_visible_on_screen_notifier_2d_screen_exited()
	{
		IsActive = false;
	}
}
