using Godot;
using System;

public partial class Enemy_4 : Enemy
{
	//敵の状態
	enum SEQ 
	{
		APPEAR_START,
		APPEAR,
		APPEAR_STOP,
		MOVE_SIDE,
		CANNON,
	};
	
	public float Theta;
	
	//初期化処理
	public override void Initialize()
	{
		IsBossDeath = false;

		Theta = 0.0f;
		switch(Level){
			case 1:
				HitPoint = 50;
				break;
			case 2:
				HitPoint = 100;
				break;
			case 3:
				default:
					HitPoint = 200;
					break;
		}
		
		var Name = "level" + Level.ToString();
		switch(Level){
			case 1:
			case 2:
				default:
				Name = "level1";
				break;
			case 3:
			case 4:
				Name = "level2";
				break;
			case 5:
			case 6:
				Name = "level3";
				break;
		}
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play(Name);
	}
	
	//更新処理
	public override void Update() 
	{
		switch((SEQ)Seq){
			case SEQ.APPEAR_START:
				MoveVec.X = -3.0f;
				Seq++;
				break;
			case SEQ.APPEAR:
				if(Position.X <= 850.0f){
					MoveVec.X = 0.0f;
					Seq++;
				}
				break;
			case SEQ.APPEAR_STOP:
				GetNode<Timer>("ShootTimer").Start();
				
				
				if(Level >= 3){
					GetNode<Timer>("ShootTimer2").Start();
				}
				
				if(GD.RandRange(0,100) % 2 == 0) {
					MoveVec.Y = -2.0f;
				}
				else {
					MoveVec.Y = 2.0f;
				}
				Seq++;
				break;
			case SEQ.MOVE_SIDE:
				if(!IsMovingTimerStart && Main.StageNum >= 5) {
					GetNode<Timer>("MovingTimer").Start();
					IsMovingTimerStart = true;
				}

				if(Position.Y <= 95){
				   MoveVec.Y = 2.0f;
				}else if(Position.Y >= ScreenSize.Y - 95){
					MoveVec.Y =-2.0f;
				}
				MoveVec.X = Mathf.Cos(Theta) * 5;
				Theta += 0.04f;
				break;
			case SEQ.CANNON:
				MoveVec = Vector2.Zero;
				if(!IsCannonTimerStart) {
					GetNode<Timer>("ChargeTimer").Start();
					IsCannonTimerStart = true;
				}
				break;
		}
	}
	
	//衝突時の処理　layerで種別を判別
	public override void OnCollision(uint layer)
	 {
		if(--HitPoint <= 0){
			IsActive = false;
			if(!IsBossDeath){
				IsBossDeath = true;
				EmitSignal("Destroy");
				Main.Explode(Position);
			}
		}
	}
	
	//弾の発射
	private void _on_shoot_timer_timeout()
	{
		var Left = GetNode<Node2D>("LeftMuzzle");
		var Right = GetNode<Node2D>("RightMuzzle");
		
		var LeftPos = Position + Left.Position;
		var RightPos = Position + Right.Position;
		
		var LeftTarget = LeftPos + new Vector2(-100,0);
		var RightTarget = RightPos + new Vector2(-100,0);
			
		ShootPos(LeftPos,LeftTarget);
		ShootPos(RightPos,RightTarget);
	}
	
	//散弾の発射
	private void _on_shoot_timer_2_timeout()
	{
		float Angle = 0.0f;
		
		for(int i=0; i < 8; i++){
			ShootAngle(Mathf.DegToRad(Angle),5.0f);
			Angle += 360.0f / 8.0f;
		}
	}

	//敵のキャノンの発射
	public void ShootCannon(Vector2 pos)
	{
		var Enemycannon = (Enemy_cannon_ver_2)ECannon.Instantiate();
		Enemycannon.Position = pos;
		Main.AddChild(Enemycannon);
	}

	//敵のキャノンの位置
	private Vector2 CannonPosition()
	{
		var Muzzle = GetNode<Node2D>("CannonPos");
		var MuzzlePos = Position + Muzzle.Position;
		return MuzzlePos ;
	}

	//動きを止めてキャノンの発射を依頼
	private void _on_moving_timer_timeout()
	{
		IsCannonTimerStart = false;
		Seq++;
		GetNode<Timer>("ShootTimer").Stop();
		GetNode<Timer>("ShootTimer2").Stop();

		GetNode<AudioStreamPlayer2D>("ChargeSound").Play();
	}

	//チャージ音の終了とキャノンの発射
	private void _on_charge_timer_timeout()
	{
		GetNode<Timer>("CannonTimer").Start();
		ShootCannon(CannonPosition());

		GetNode<AudioStreamPlayer2D>("CannonSound").Play();
	}

	//キャノン発射後、通常の行動を行う
	private void _on_cannon_timer_timeout()
	{
		IsMovingTimerStart = false;
		if(GD.RandRange(0,100) % 2 == 0) {
			MoveVec.Y = -2.0f;
		}
		else {
			MoveVec.Y = 2.0f;
		}
		GetNode<Timer>("ShootTimer").Start();
		if(Level >= 3)
			GetNode<Timer>("ShootTimer2").Start();
		Seq = 0;
	}

}
