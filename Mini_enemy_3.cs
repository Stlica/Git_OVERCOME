using Godot;
using System;

public partial class Mini_enemy_3 : Enemy
{
	static int num = 0;

	public override void Initialize()
	{
		HitPoint = 1;
		Speed = 3;
		ShootInterval = 70;

		switch(num++ % 3){
			case 0:
				Position += new Vector2(0.0f,150.0f);
				break;
			case 1:
				Position += new Vector2(0.0f,300.0f);
				break;
			case 2:
				Position += new Vector2(50.0f,-150.0f);
				break;
			}
	}
		
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
					var player = Main.GetPlayer();
					var target = player.Position;
					if(--ShootInterval <= 0){
						Shoot(target);
						ShootInterval = 70;
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
	
	
	
	public override void OnCollision(uint layer)
	{
		if(layer == 1){
			IsActive = false;
		}else if(layer == 2 || layer == 128){
			HitPoint--;
			if(HitPoint <= 0 && !IsHitCan){
				Main.AddScore(10);
				IsActive = false;
			}
		}
	}
	
	private void _on_visible_on_screen_notifier_2d_screen_exited()
	{
		IsActive = false;
	}
}



