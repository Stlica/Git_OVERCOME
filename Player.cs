using Godot;
using System;

public partial class Player : Actor
{
	[Signal]
	public delegate void DestroyEventHandler();

	[Export]
	public PackedScene Bullet;
	[Export]
	public PackedScene CannonMan;
	[Export]
	public PackedScene Explosion;
	
	static int		Hitcount;			//ダメージを受けた回数
	public int		PlayerRest = 3;		//残機数
	public int		PlayerLevel = 0;	//レベル
	public bool		Special = false;	//特殊能力解放
	public bool		WideShoot = false;	//ワイドショット解放(二発)
	public bool		WideShoot1 = false;	//ワイドショット解放(三発)
	public bool		Guard = false;		//バリアが出ているか
	public float	Accel = 1.0f;		//加速度
	public int 		PlayerLook = 0;		//見た目

	//初期化処理
	public override void Initialize()
	{
		IsVisible = true;
		GetNode<AnimatedSprite2D>("Guard").Hide();
		PlayerHP = 90;
		ShootInterval = 0;
		Hitcount = 0;

		if(Main.IsDeath && WideShoot){
			WideShoot = true;
			WideShoot1 = false;
		}else if(Main.IsDeath&&!WideShoot){
			WideShoot = false;
			WideShoot1 = true;
		}else{
			WideShoot = false;
			WideShoot1 = false;
		}

		if(PlayerLevel <= 1){
			WideShoot = false;
			WideShoot1 = false;
		}
	}

	//更新処理
	public override void Update()
	{
		if(!Main.isClear){
			//非表示
			if(!IsVisible){
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Hide();
				return;
			}

			//移動
			MoveVec = Vector2.Zero;
			if(Input.IsActionPressed("MoveLeft")){
				MoveVec.X = -4.0f * Accel;
			}else if(Input.IsActionPressed("MoveRight")){
				MoveVec.X = 4.0f * Accel;
			}
			if(Input.IsActionPressed("MoveUp")){
				MoveVec.Y = -4.0f * Accel;
			}else if(Input.IsActionPressed("MoveDown")){
				MoveVec.Y = 4.0f * Accel;
			}

			//移動制限
			Position = new Vector2(
				Math.Clamp(Position.X,60,ScreenSize.X - 60),
				Math.Clamp(Position.Y,30,ScreenSize.Y - 30)
			);

			//ショット
			if(Input.IsActionPressed("Shoot")){
				if(--ShootInterval <= 0){
					ShootInterval = 10;
					if(WideShoot){
						Shoot(MuzzlePosition1());
						Shoot(MuzzlePosition());
					}else if(WideShoot1){
						Shoot(MuzzlePosition());
						Shoot(MuzzlePosition1());
						Shoot(MuzzlePosition2());
					}else{
						Shoot(MuzzlePosition());
					}
				}
			}

			//キャノン
			if(Input.IsActionPressed("Cannon") && Special){
				if(Main.GetHUD().GetNode<TextureProgressBar>("SpecialBar01").Value >= 100){
					Main.GetHUD().GetNode<TextureProgressBar>("SpecialBar01").Value = 0;
					ShootCannon(MuzzlePosition());
				}
			}

			//バリア
			if(ReadySpecial()){
				SpecialBarrier();
			}

			//見た目の変更
			switch(PlayerLevel){
				case 0:
				case 1:
				default:
					PlayerLook = 0;
					break;
				case 2:
				case 3:
					PlayerLook = 1;
					break;
				case 4:
					PlayerLook = 2;
					break;
			}
			var Name = "Level" + PlayerLook.ToString();
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play(Name);
		}
	}

	//ショット処理
	public void Shoot(Vector2 pos)
	{
		ShootSound();

		var PlayerShot = (Bullet)Bullet.Instantiate();
		PlayerShot.Position = pos;
		GetOwner<Node>().AddChild(PlayerShot);
	}

	
	private Vector2 MuzzlePosition()
	{
		var Muzzle = GetNode<Node2D>("Muzzle");
		var MuzzlePos = Position + Muzzle.Position;
		return MuzzlePos;
	} 

	//ワイドショット(二発)処理
	private Vector2 MuzzlePosition1()
	{
		var Widemuzzle = GetNode<Node2D>("WideMuzzle");
		var MuzzlePos = Position + Widemuzzle.Position;
		return MuzzlePos;
	} 

	//ワイドショット(三発)処理
	private Vector2 MuzzlePosition2()
	{
		var Widemuzzle2 = GetNode<Node2D>("WideMuzzle2");
		var MuzzlePos = Position + Widemuzzle2.Position;
		return MuzzlePos;
	} 
	
	//衝突時の処理　layerで種別を判別
	public override void OnCollision(uint layer)
	{
		if(!IsVisible) {
			return;
		}

		MoveVec = Vector2.Zero;

		if(!Guard){	//バリアなし
			if(layer == 16){		//パワーキューブ
				PlayerHP += 30;
				Hitcount -= 3;
				Main.GetHUD().GetHP(PlayerHP);
				if(PlayerHP >= 90)
				{
					PlayerHP = 90;
					Hitcount = 0;
				}
			}else{					//敵
				if(layer == 512) {	//敵キャノン
					Hitcount = 10;
				}
				Damaged();
			}
		}
		else{		//バリア発動時
			GetNode<AudioStreamPlayer2D>("GuardSound").Play();
		}
	}

	//ダメージ処理
	public void Damaged()
	{
		CollideSound();

		if(Hitcount <= 7){
			PlayerHP -= 10;
			Main.GetHUD().GetHP(PlayerHP);
			Hitcount++;
		}else {
			PlayerHP = 100;
			Main.GetHUD().GetHP(PlayerHP);
			Input.StartJoyVibration(0,1,0,1);
			PlayerDestroy();
			Hitcount = 0;
		}
	}

	//バリア処理
	public void SpecialBarrier()
	{
		if(Special){
			if(Input.IsActionJustPressed("Special") && !Guard){
			GetNode<AnimatedSprite2D>("Guard").Show();
			GetNode<Timer>("GuardTimer").Start();
			Guard = true;
			}
		}
	}

	//バリアゲージを確認
	public bool ReadySpecial()
	{
		if(Main.GetNode<Hud>("HUD").SpecialGauge() >= 100){
			return true;	
		}
		else{
			return false;
		}
	}

	//GuardTimerが終わったとき
	private void _on_timer_timeout()
	{
		GetNode<AnimatedSprite2D>("Guard").Hide();
		Guard = false;
	}

	//キャノン処理
	public void ShootCannon(Vector2 pos)
	{
		var Cannon = (CannonMan)CannonMan.Instantiate();
		Cannon.Position = pos;
		GetOwner<Node>().AddChild(Cannon);
	}

	//死亡処理
	public void PlayerDestroy()
	{
		var explo = (Explosion)Explosion.Instantiate();
		explo.Position = Position;
		GetOwner<Node>().AddChild(explo);

		//残機を減らす
		PlayerRest--;

		DestroySound();

		//残機数が0以下ならゲームオーバー
		if (PlayerRest <= 0){
			PlayerRest = 0;
			Main.GameOver();
		}
		else //ゲームオーバーでなければプレイヤーを初期位置に戻す
		{
			//リスポーン
			Main.RestartPlayer();
			IsVisible = false;

			GetNode<Timer>("RespawnTimer").Start();
		}
		
	}

	//RespawnTimerが終わったとき
	private void _on_respawn_timer_timeout()
	{
		IsVisible = true;
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Show();
		//残機数表示の更新
		Main.GetNode<Title>("Title").UpdatePlayerRest(PlayerRest);
	}

	//レベルアップ処理
	public void LevelUp()
	{
		if(PlayerLevel >= 4){
			PlayerLevel = 4;
			if(PlayerHP >= 90){
				return;
			}
			GetHealSound();
			return;
		}

		PlayerLevel++;
		LevelUpSound();

		//能力解放
		switch (PlayerLevel) {
		case 1:
			Special = true;
			Main.GetHUD().GetNode<TextureProgressBar>("BarrierBar01").Show();
			Main.GetHUD().GetNode<TextureProgressBar>("BarrierBar02").Show();
			Main.GetHUD().GetNode<Label>("Barrier").Show();
			Main.GetHUD().GetNode<Label>("Cannon").Show();
			Main.GetHUD().GetNode<TextureProgressBar>("SpecialBar01").Show();
			Main.GetHUD().GetNode<TextureProgressBar>("SpecialBar02").Show();
			break;
		case 2:
			WideShoot = true;
			break;
		case 3:
			WideShoot = false;
			WideShoot1 = true;
				break;
		case 4:
			Accel = 1.5f;
			break;
		default:
			WideShoot = false;
			WideShoot1 = false;
			break;
		}
	}

	//PlayerRestを返す
	public int GetPlayerRest()
	{
		return PlayerRest;
	}

	//PlayerLevelを返す
	public int GetPlayerLevel()
	{
		return PlayerLevel;
	}

	//ゲームクリア処理
	private void _on_clear_move_timer_timeout()
	{
		MoveVec = new Vector2(7.0f, 0.0f);
	}
}


