using Godot;
using System;

public partial class Enemy_3 :  Enemy
{
	
	[Export]
	public PackedScene Minienemy;
	
	//初期化処理
	public override void Initialize() 
	{
		IsHitCan = false;
		HitPoint = 3;
		Speed = 3;
		Frame = 120;
		ShootInterval = 40;
		MoveToPlayer();
	}

	//更新処理
	public override void Update() 
	{
		switch(Seq){
			case 0:
				if(--Cnt <=0){
					MoveVec.X = MoveVec.Y = 0.0f;
					Cnt = 30;
					Seq++;
				}
				break;
			case 1:
				if(--Cnt <= 0){
					Cnt = 30;
					Seq++;
				}
				break;
			case 2:
				if(Frame % 10 == 0){
					var Player = Main.GetPlayer();
					var Target = Player.Position;
					if(--ShootInterval <= 0){
						Shoot(Target);
						ShootInterval = 40;
					}
				}
				if(--Cnt <= 0){
					MoveToPlayer();
					Cnt = 60;
					Seq = 0;
				}
				break;
			default:
				break;
		}
		
		if(Position.X < 0.0f){
			IsActive = false;
		}
	}

	//分裂処理
	public void Shoot_Mini(Vector2 pos)
	{
		var Mini = (Mini_enemy_3)Minienemy.Instantiate();
		Mini.Position = pos;
		Main.AddChild(Mini);
	}

	//衝突時の処理　layerで種別を判別
	public override void OnCollision(uint layer) 
	{
		if(layer == 2 ){
			HitPoint--;
			if(HitPoint <=0 && !IsHitCan){
				IsHitCan = true;
				Main.AddScore(100);
				IsActive = false;
				for(int i = 0; i < 3; i++)
				Shoot_Mini(Position);
			}
		}
		else if(layer == 1 || layer == 128)
		{
			if(layer == 128 && !IsHitCan) {
				IsHitCan = true;
				Main.AddScore(100);
			}
			IsActive = false;
		}
	}
	
	//画面外で削除
	private void _on_visible_on_screen_notifier_2d_screen_exited()
	{
		IsActive = false;
	}
}
