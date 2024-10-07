using Godot;
using System;

public partial class EnemyBullet : Actor
{
	//初期化処理
	public override void Initialize()
	{
		Speed = 4.0f;
	}
	
	//更新処理
	public override void Update()
	{
		if(Position.X < 0.0f){
			IsActive = false;
		}
	}
	
	//現在のプレイヤーの位置に発射
	public void SetTarget(Vector2 pos)
	{
		TargetPos = pos;
		float Angle = Position.AngleToPoint(TargetPos);

		MoveVec.X = Mathf.Cos(Angle) * Speed;
		MoveVec.Y = Mathf.Sin(Angle) * Speed;
	}
	
	//角度に合わせて散弾
	public void SetAngle(float angle, float speed = 4.0f)
	{
		Speed = speed;
		
		MoveVec.X = Mathf.Cos(angle) * Speed;
		MoveVec.Y = Mathf.Sin(angle) * Speed;
	}
	
	//衝突時の処理　layerで種別を判別
	public override void OnCollision(uint layer){
		IsActive = false;
	}
	
	//画面外で削除
	private void _on_visible_on_screen_notifier_2d_screen_exited()
	{
		IsActive = false;
	}
}



