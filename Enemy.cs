using Godot;
using System;

public partial class Enemy : Actor
{
	[Signal]
	public delegate void DestroyEventHandler();
	
	[Export]
	public PackedScene Bullet;
	[Export]
	public PackedScene ECannon;
	
	public int  Level = 1;					//敵のレベル
	public int  HitPoint;					//敵のヒットポイント
	public bool IsMovingTimerStart = false; //動いているか
	public bool IsCannonTimerStart = false; //キャノンを撃てるか
	public bool IsHitCan = false;			//当たり判定を行うか

	//レベルの取得
	public void SetLevel(int lv)
	{
		Level = lv;
		Initialize();
	}
	
	//敵の弾の発射(直進)
	public void Shoot(Vector2 target)
	{
		var EnemyShoot = (EnemyBullet)Bullet.Instantiate();
		EnemyShoot.Position = Position;
		Main.AddChild(EnemyShoot);
		EnemyShoot.SetTarget(target);
	}
	
	//敵の弾の発射(プレイヤーに追尾)
	public void ShootPos(Vector2 pos,Vector2 target)
	{
		var EnemyShoot = (EnemyBullet)Bullet.Instantiate();
		EnemyShoot.Position = pos;
		Main.AddChild(EnemyShoot);
		EnemyShoot.SetTarget(target);
	}
	
	//敵の弾の発射(散弾)
	public void ShootAngle(float angle, float speed = 4.0f)
	{
		var EnemyShoot = (EnemyBullet)Bullet.Instantiate();
		EnemyShoot.Position = Position;
		Main.AddChild(EnemyShoot);
		EnemyShoot.SetAngle(angle,speed);
	}
	
	//プレイヤーの位置に移動
	public void MoveToPlayer()
	{
		var Player = Main.GetPlayer();
		var Target = Player.Position;
		float Angle = Position.AngleToPoint(Target);
		
		MoveVec.X = Mathf.Cos(Angle) * Speed;
		MoveVec.Y = Mathf.Sin(Angle) * Speed;

		Cnt = 60;
	}
}
