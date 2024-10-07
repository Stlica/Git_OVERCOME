using Godot;
using System;

public partial class Actor : CharacterBody2D
{
	public Vector2 MoveVec; 			//移動量
	public Vector2 AcceleVec; 			//加速度
	public bool    IsActive; 			//活性状態
	public Vector2 ScreenSize;  		//画面サイズ
	public Main    Main; 				//メインシーン
	public int     Seq; 				//汎用シーケンス
	public Vector2 TargetPos; 			//敵の位置
	public float   Speed;				//速さ
	public int	   Cnt;					//カウント
	public int	   Frame;				//フレーム数
	public int 	   ShootInterval;		//発射間隔
	public int 	   PlayerHP;			//プレイヤーのHP
	public bool    IsBossDeath = false; //ボスが死んだか
	public bool    IsVisible;			//見えているか

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MoveVec = AcceleVec = Vector2.Zero;
		ScreenSize = GetViewportRect().Size;
		IsActive = true;
		Main = (Main)GetParent();
		Initialize();
		Seq = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(!IsActive){
			QueueFree();
			return;
		}
		
		if(!Visible)return;
		Update();
		MoveVec += AcceleVec;
		
		var Obj = MoveAndCollide(MoveVec);
		//衝突時の処理
		if(Obj != null && Visible)
		{
			var Actor = (Actor)Obj.GetCollider();
			OnCollision(Actor.CollisionLayer);
			if(Actor.Visible){
				Actor.OnCollision(CollisionLayer);
			}
		}
	}

	//初期化処理
	public virtual void Initialize() {}

	//更新処理
	public virtual void Update() {}

	//衝突時の処理　layerで種別を判別
	public virtual void OnCollision(uint layer) {}
	
	//プレイヤーのHPを返す
	public int GetHP(){return PlayerHP;}
	
	//回復音
	public void GetHealSound()
	{
		GetNode<AudioStreamPlayer2D>("HealSound").Play();
	}
	
	//レベルアップ音
	public void LevelUpSound()
	{
		GetNode<AudioStreamPlayer2D>("LevelSound").Play();
	}
	
	//発射音
	public void ShootSound()
	{
		GetNode<AudioStreamPlayer2D>("ShootSound").Play();
	}
	
	//キャノンマンチャージ音
	public void CannonManChargeSound()
	{
		GetNode<AudioStreamPlayer2D>("CargeSound").Play();
	}
	
	//キャノンマンチャージ音の停止
	public void CannonManChargeSoundStop()
	{
		GetNode<AudioStreamPlayer2D>("CargeSound").Stop();
	}
	
	//キャノンマンキャノン音の発射音
	public void CannonManCannonSound()
	{
		GetNode<AudioStreamPlayer2D>("CannonManCannon").Play();
	}
	
	//キャノンマンキャノン音の停止
	public void CannonManCannonSoundStop()
	{
		GetNode<AudioStreamPlayer2D>("CannonManCannon").Stop();
	}
	
	//破壊音
	public void DestroySound()
	{
		GetNode<AudioStreamPlayer2D>("DestroySound").Play();
	}
	
	//衝突音
	public void CollideSound()
	{
		GetNode<AudioStreamPlayer2D>("CollideSound").Play();
	}
} 
