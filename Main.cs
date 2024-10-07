using Godot;
using System;
//using System.IO; 

public class StageData
{
	public int[] Enemies;
	public int	 Boss;
	
	public StageData(int[] enemies, int boss){
		Enemies = enemies;
		Boss = boss;
	}
}

public class GroundsData
{
	public int[] Grounds;

	public GroundsData(int[] grounds){
		Grounds = grounds;
	}
}

public partial class Main : Node2D
{
	[Export]
	public PackedScene Enemy_1;
	[Export]
	public PackedScene Enemy_2;
	[Export]
	public PackedScene Enemy_3;
	[Export]
	public PackedScene Enemy_4;
	[Export]
	public PackedScene Enemy_5;
	[Export]
	public PackedScene Enemy_6;
	[Export]
	public PackedScene PoewrCube;
	[Export]
	public PackedScene Meteo;
	[Export]
	public PackedScene Anim_Player;
	[Export]
	public PackedScene Anim_Enemy_1;
	[Export]
	public PackedScene Anim_Enemy_2;
	[Export]
	public PackedScene Anim_Enemy_3;
	[Export]
	public PackedScene Anim_Enemy_4;
	[Export]
	public PackedScene Anim_Enemy_5;
	[Export]
	public PackedScene Anim_Enemy_6;
	[Export]
	public PackedScene Smoke;
	
	public PackedScene[] Enemies;
	
	public StageData[]	Stages;
	
	public PackedScene[] Grounds;

	public GroundsData[]    GroundsStages;
	
	public int			 StageNum;
	public int			 ScoreNum;
	static int 			 HighScore;
	public int			 PlayerRest = 3;
	public bool			 AnimTime = false;
	public bool 		 isClear  = false;
	public bool          IsDeath  = false;
	

	public override void _Ready()
	{
		GetNode<AudioStreamPlayer2D>("BGM").Play();

		GetNode<Timer>("AnimTimer").Start();
		
		Enemies = new PackedScene[7]{
			Enemy_1,
			Enemy_2,
			Enemy_3,
			Enemy_4,
			Enemy_5,
			Enemy_6,
			PoewrCube,
		};
		
		Stages = new StageData[7]{
			new StageData(new int[1] {0},   3),
			new StageData(new int[1] {1},    3),
			new StageData(new int[2] {0,1},    3),
			new StageData(new int[1] {2},    3),
			new StageData(new int[2] {0,4},    3),
			new StageData(new int[4] {0,2,1,4},    3),
			new StageData(new int[1] {0},    5),
		};
		
		Grounds = new PackedScene[1]{
			Meteo,
		};

		GroundsStages = new GroundsData[1]{
			new GroundsData(new int [1] {0}),
		};
		
		GD.Randomize();
		
		GetPlayer().Hide();
		GetHUD().GetNode<TextureProgressBar>("BarrierBar01").Hide();
		GetHUD().GetNode<TextureProgressBar>("BarrierBar02").Hide();
		GetHUD().GetNode<Label>("Barrier").Hide();
		GetHUD().GetNode<TextureProgressBar>("SpecialBar01").Hide();
		GetHUD().GetNode<TextureProgressBar>("SpecialBar02").Hide();
		GetHUD().GetNode<Label>("Cannon").Hide();
		GetClear().Hide();
		
		filePath();
	}
	
	public void NewGame()
	{
		isClear  = false;
		AnimTime = true;
		EwaseAllEnemy();
		//プレイヤーを開始位置へ
		//プレイヤーを取得
		var player = GetNode<Player>("Player");
		//開始位置を取得
		var startPosition = GetNode<Node2D>("StartPosition");

		//プレイヤーの座標を開始位置の座標にする
		player.Position = startPosition.Position;
		player.Initialize();
		player.Show();
		var hp = player.GetHP();
		player.Special = false;
		player.Guard = false;
		player.PlayerLevel = 0;
		player.PlayerRest = 3;
		player.WideShoot = false;
		player.WideShoot1 = false;
		player.Accel = 1.0f;

		//メッセージの表示
		var hud = GetTitle();
		hud.ShowMessage("Get Ready!");
		hud.ShowScore("Score:");
		hud.ShowStageLabel("Stage:");
		
		//敵の発生タイマーを開始
		GetNode<Timer>("EnemyTimer").Start();
		GetNode<Timer>("BossTimer").Start();
		GetNode<Timer>("GroundsTimer").Start();

		//残機数の初期化
		var PlayerRest = GetPlayer().GetPlayerRest();
		GetTitle().UpdatePlayerRest(PlayerRest);

		//ステージの初期化
		StageNum = 1;
		GetTitle().UpdateStageNum(StageNum);
		
		//スコアの初期化
		ScoreNum = 0;
		GetTitle().UpdateScoreNum(ScoreNum);
		
		PlayerRest = 3;
		GetTitle().UpdatePlayerRest(PlayerRest);
		
		var PlayerLevel = GetPlayer().GetPlayerLevel();

		
		GetHUD().GetNode<TextureProgressBar>("HPBar2").Show();
		GetHUD().GetNode<TextureProgressBar>("HPBar3").Show();
		GetHUD().GetNode<TextureProgressBar>("BarrierBar01").Hide();
		GetHUD().GetNode<TextureProgressBar>("BarrierBar02").Hide();
		GetHUD().GetNode<Label>("Barrier").Hide();
		GetHUD().GetNode<Label>("Cannon").Hide();
		GetHUD().GetNode<TextureProgressBar>("SpecialBar01").Hide();
		GetHUD().GetNode<TextureProgressBar>("SpecialBar02").Hide();
		GetHUD().Show();
		GetHUD().GetHP(hp);

	}
	
	public override void _Process(double delta)
	{

	}
	
	public Player GetPlayer()
	{
		return GetNode<Player>("Player");
	}
	
	public Hud GetHUD()
	{
		return GetNode<Hud>("HUD");
	}
	
	public Title GetTitle()
	{
		return GetNode<Title>("Title");
	}
	
	public Game_clear GetClear()
	{
		return GetNode<Game_clear>("GameClear");
	}

	public Enemy CreateEnemy(int kind, Vector2 pos)
	{
		if(kind < 0 || kind >= Enemies.Length){
			return null;
		}
		
		var enemy = (Enemy)Enemies[kind].Instantiate();
		
		if(kind  == 4) {
			pos.X = -100.0f;
		}
		enemy.Position = pos;
		AddChild(enemy);
		
		return enemy;
	}

	public Meteo CreateGrounds(int kind, Vector2 pos)
	{
		if(kind < 0 || kind >= Grounds.Length){
			return null;
		}

		var grounds = (Meteo)Grounds[kind].Instantiate();
		grounds.Position = pos;
		AddChild(grounds);

		return grounds;
	}

	public void RestartPlayer()
	{
		var player = GetPlayer();
		var startPosition = GetNode<Node2D>("StartPosition");

		player.Position = startPosition.Position;
		IsDeath = true;
		player.Initialize();
	}
	
	private void _on_enemy_timer_timeout()
	{
		
		var stageData = Stages[StageNum - 1];
		int len = stageData.Enemies.Length;
		
		//敵の種類を乱数で選択する
		int kind = stageData.Enemies[GD.RandRange(0,len - 1)];
		
		//指定したはんいの乱数を発生させて位置を計算する
		var enemy = CreateEnemy(kind ,new Vector2(1250,GD.RandRange(50,650)));
		enemy.SetLevel(StageNum);
	}
		
	public void AddScore(int val)
	{
		ScoreNum += val;
		GetTitle().UpdateScoreNum(ScoreNum);
		
		if(ScoreNum>HighScore){
			HighScore = ScoreNum;
			GetTitle().UpdateHighScoreNum(ScoreNum);
		}
	}
	
	public void GameOver()
	{
		GetNode<AudioStreamPlayer2D>("BossBGM").Stop();
		GetNode<AudioStreamPlayer2D>("BGM").Play();
		HideBar();
		//敵の発生を止める
		GetNode<Timer>("EnemyTimer").Stop();
		GetNode<Timer>("GroundsTimer").Stop();
		//ゲームオーバー表示
		var hud = GetTitle();
		hud.ShowGameover();
		//プレイヤーを非表示
		GetPlayer().Hide();
		EwaseAllEnemy();
		fileWrite();
	}
	
	public void EwaseAllEnemy()
	{
		foreach(Node child in GetChildren()){
			if(child.IsInGroup("enemy")){
				child.QueueFree();
			}
		}
	}
	private void _on_boss_timer_timeout()
	{
		var stageData = Stages[StageNum - 1];
		var kind = stageData.Boss;
		
		var boss = CreateEnemy(kind,new Vector2(1500,350));
		GetNode<Timer>("EnemyTimer").Stop();
		GetNode<Timer>("GroundsTimer").Stop();
		

		
		boss.Destroy += () => Input.StartJoyVibration(0,0,1,1);
		boss.Destroy += () => AddScore(1000);
		boss.Destroy += () => StageClear();
		boss.Destroy += () => BombSound();
		if(StageNum != 7){
			boss.Destroy += () => CreateEnemy(6,new Vector2(1250,GD.RandRange(50,650)));
		}
		boss.SetLevel(StageNum);
	}

	public void BombSound()
	{
		GetNode<AudioStreamPlayer2D>("D_Sound").Play();

	}

	 public void StageClear()
	{
		StageNum++;

		if(StageNum == 7) {
			GetNode<AudioStreamPlayer2D>("BGM").Stop();
			GetNode<AudioStreamPlayer2D>("BossBGM").Play();
		}

		if(StageNum > 7) {
			EwaseAllEnemy();
			GameClear();
			return;
		}

		var hud = GetTitle();
		hud.ShowMessage("Stage" + (StageNum - 1).ToString() 
			+ "\r\nClear!");

		if(StageNum > Stages.Length){
			StageNum = Stages.Length;
		}

		GetTitle().UpdateStageNum(StageNum);
		GetNode<Timer>("EnemyTimer").Start();
		GetNode<Timer>("BossTimer").Start();
		GetNode<Timer>("GroundsTimer").Start();
	}

	public void GameClear()
	{
		int bounus = 500 * PlayerRest;
		AddScore(bounus);
		
		GetNode<AudioStreamPlayer2D>("BossBGM").Stop();
		GetNode<AudioStreamPlayer2D>("ClearBGM").Play();
		
		HideBar();
		GetClear().Show();
		GetClear().GetNode<Timer>("CreditTimer").Start();
		GetClear().readScore(ScoreNum);
		GetPlayer().MoveVec = new Vector2(0.0f, 0.0f);
		isClear = true;
		GetPlayer().GetNode<Timer>("ClearMoveTimer").Start();
		fileWrite();
		GetNode<Timer>("EnemyTimer").Stop();
		GetNode<Timer>("BossTimer").Stop();
		GetNode<Timer>("GroundsTimer").Stop();

		EwaseAllEnemy();

	}

	
	public void filePath()
	{
		string path = "res://highscore.txt";
		using var file = FileAccess.Open(path,FileAccess.ModeFlags.Read);
		string high = file.GetAsText();
		file.Close();
		HighScore = int.Parse(high);
		GetTitle().UpdateHighScoreNum(HighScore);
	
	}
	
	public void fileWrite()
	{
		string path = "res://highscore.txt";
		using var file = FileAccess.Open(path,FileAccess.ModeFlags.Write);
		string scr = HighScore.ToString();
		file.StoreString(scr);
		file.Close();
	}

	
	public void P_Shoot(Vector2 pos){
		var objx = (Anim_player)Anim_Player.Instantiate();
		objx.Position = pos;;
		AddChild(objx);
	}
	
	private Vector2 P_MuzzlePosition()
	{
		var muzzle = GetNode<Node2D>("Anim_Player");
		var muzzlePos = muzzle.Position;
		return muzzlePos ;
	} 
	
	public void enemy1_Shoot(Vector2 pos){
		var objx = (Anim_enemy_1)Anim_Enemy_1.Instantiate();
		objx.Position = pos;;
		AddChild(objx);
	}
	
	private Vector2 enemy1_AnimPos()
	{
		var muzzle = GetNode<Node2D>("Anim_enemy_1");
		var muzzlePos = muzzle.Position;
		return muzzlePos ;
	} 
	
	private Vector2 P_MuzzlePosition2()
	{
		var muzzle = GetNode<Node2D>("Anim_Player2");
		var muzzlePos = muzzle.Position;
		return muzzlePos ;
	} 
	
	private Vector2 enemy2_AnimPos()
	{
		var muzzle = GetNode<Node2D>("Anim_enemy_2");
		var muzzlePos = muzzle.Position;
		return muzzlePos ;
	} 
	
	public void enemy2_Shoot(Vector2 pos){
		var objx = (Anim_enemy_2)Anim_Enemy_2.Instantiate();
		objx.Position = pos;;
		AddChild(objx);
	}
	
	private Vector2 enemy3_AnimPos()
	{
		var muzzle = GetNode<Node2D>("Anim_enemy_3");
		var muzzlePos = muzzle.Position;
		return muzzlePos ;
	} 
	
	public void enemy3_Shoot(Vector2 pos){
		var objx = (Anim_enemy_3)Anim_Enemy_3.Instantiate();
		objx.Position = pos;;
		AddChild(objx);
	}
	
	private Vector2 enemy4_AnimPos()
	{
		var muzzle = GetNode<Node2D>("Anim_enemy_4");
		var muzzlePos = muzzle.Position;
		return muzzlePos ;
	} 
	
	public void enemy4_Shoot(Vector2 pos){
		var objx = (Anim_enemy_4)Anim_Enemy_4.Instantiate();
		objx.Position = pos;;
		AddChild(objx);
	}
	
	private Vector2 enemy5_AnimPos()
	{
		var muzzle = GetNode<Node2D>("Anim_enemy_5");
		var muzzlePos = muzzle.Position;
		return muzzlePos ;
	} 
	
	public void enemy5_Shoot(Vector2 pos){
		var objx = (Anim_enemy_5)Anim_Enemy_5.Instantiate();
		objx.Position = pos;;
		AddChild(objx);
	}
	
	private Vector2 enemy6_AnimPos()
	{
		var muzzle = GetNode<Node2D>("Anim_enemy_6");
		var muzzlePos = muzzle.Position;
		return muzzlePos ;
	} 
	
	public void enemy6_Shoot(Vector2 pos){
		var objx = (Anim_enemy_6)Anim_Enemy_6.Instantiate();
		objx.Position = pos;;
		AddChild(objx);
	}
	
	private void _on_anim_timer_timeout()
	{
		GetNode<Timer>("AnimTimer").Stop();
		if(!AnimTime){
			P_Shoot(P_MuzzlePosition());
		}
	}
	
	private void _on_anim_timer_2_timeout()
	{
		enemy6_Shoot(enemy6_AnimPos());
		enemy5_Shoot(enemy5_AnimPos());
		enemy4_Shoot(enemy4_AnimPos());
		enemy3_Shoot(enemy3_AnimPos());
		enemy2_Shoot(enemy2_AnimPos());
		enemy1_Shoot(enemy1_AnimPos());
		P_Shoot(P_MuzzlePosition2());
		GetNode<Timer>("AnimTimer2").Stop();
	}
	
	private void _on_clear_timer_timeout()
	{
		GetClear().GetNode<Label>("Mes").Hide();
	}

	async private void _on_game_clear_gone_credit()
	{
		GetClear().GetNode<Label>("Mes").Show();

		var timer = GetNode<Timer>("ClearTimer");
		timer.Start();
		await ToSignal(timer, "timeout");

		await ToSignal(GetTree().CreateTimer(2), "timeout");

		GetClear().creditShow();
		GetClear().Hide();
		GetTitle().ShowTitleLabel();
		
		GetNode<AudioStreamPlayer2D>("ClearBGM").Stop();
		GetNode<AudioStreamPlayer2D>("BGM").Play();
	}

	
	public void HideBar()
	{
		GetHUD().Hide();
		GetTitle().GetNode<Label>("RestNumber").Hide();
		GetTitle().GetNode<Label>("Score").Hide();
		GetTitle().GetNode<Label>("ScoreNumber").Hide();
		GetTitle().GetNode<Label>("StageLabel").Hide();
		GetTitle().GetNode<Label>("StageNumber").Hide();
		}

	public void Explode(Vector2 pos)
	{
		var explosion = (Smoke)Smoke.Instantiate();
		explosion.Position = pos;
		AddChild(explosion);
	}

	private void _on_grounds_timer_timeout()
	{
		//var groundsData = GroundsStages[StageNum - 1];
		var groundsData = GroundsStages[0]; //テスト

		int len = groundsData.Grounds.Length;

		//敵の種類を乱数で選択する
		int kind = groundsData.Grounds[GD.RandRange(0,len - 1)];

		//指定したはんいの乱数を発生させて位置を計算する
		var grounds = CreateGrounds(kind ,new Vector2(1250,GD.RandRange(50,650)));
		grounds.SetLevel(StageNum);
	}

}


