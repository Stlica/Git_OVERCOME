using Godot;
using System;

public partial class Enemy_6 : Enemy
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
	
	//ボスの行動
	enum SEQ 
	{
		APPEAR_START,
		APPEAR,
		CREATEENEMY,
	};
	
	//初期化処理
	public override void Initialize() 
	{
		ShootInterval = 40;
		HitPoint = 1000;
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
				if(Position.X <= 1250.0f){
					MoveVec.X = 0.0f;
					Seq++;
				}
				break;
			case SEQ.CREATEENEMY:
			if(--ShootInterval <= 0){
				ShootInterval = 40;
				Spawn();
			}
				break;
	}
}

	//衝突時の処理　layerで種別を判別
	public override void OnCollision(uint layer)
	{
		if(--HitPoint <= 0){
			IsActive = false;
			EmitSignal("Destroy");
		}
	}
	
	//敵を出現させる
	public void Spawn()
	{
		var stageData = Main.Stages[Main.StageNum - 2];
		int len = stageData.Enemies.Length;

		//敵の種類を乱数で選択する
		int kind = stageData.Enemies[GD.RandRange(0,len - 1)];

		//指定したはんいの乱数を発生させて位置を計算する
		var enemy = Main.CreateEnemy(kind ,new Vector2(1150,GD.RandRange(380,550)));
		enemy.SetLevel(Main.StageNum);
	}
	
	//出現させた後、通常の行動にする
	private void _on_spawn_timer_timeout()
	{
		IsMovingTimerStart = false;
		Seq--;
	}	
}



