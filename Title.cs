using Godot;
using System;

public partial class Title : CanvasLayer
{
	[Signal]
	public delegate void ShowTitleEventHandler();
	[Signal]
	public delegate void StartGameEventHandler();

	public bool IsNotControlScene;	//操作説明画面が出ているか
	public bool IsNotStart; 		//ゲームがスタートしているか
	public bool NoTouchZ;			//Zキーの反応を無くす

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Label>("Message").Hide();
		GetNode<Label>("Score").Hide();
		GetNode<Label>("StageLabel").Hide();
		GetNode<Label>("StageNumber").Hide();
		GetNode<Label>("ScoreNumber").Hide();
		GetNode<Label>("RestNumber").Hide();
		GetNode<Sprite2D>("OperationTexture").Hide();
		IsNotControlScene = true;
		IsNotStart = true;
		NoTouchZ = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionPressed("Start") && IsNotStart && IsNotControlScene){
			_on_start_button_pressed();
		}
		
		if(Input.IsActionPressed("Control") && IsNotControlScene){
			_on_control_button_pressed();
		}

		if(Input.IsActionJustPressed("Close")&& IsNotStart && !IsNotControlScene){
			_on_close_button_pressed();
		}
	}

	//メッセージ表示
	public void ShowMessage(string text)
	{
		var Message = GetNode<Label>("Message");
		Message.Text = text;
		Message.Show();

		GetNode<Timer>("MessageTimer").Start();
	}

	//スコア表示
	public void ShowScore(string text)
	{
		IsNotStart = false;
		var Score = GetNode<Label>("Score");
		Score.Text = text;
		Score.Show();
	}

	//ステージ数表示
	public void ShowStageLabel(string text)
	{
		var Stagelabel = GetNode<Label>("StageLabel");
		Stagelabel.Text = text;
		Stagelabel.Show();
	}

	//ゲームオーバー処理
	async public void ShowGameover()
	{
		ShowMessage("Game Over!");
		//タイマーの終了を同期して待つ
		var MessageTimer = GetNode<Timer>("MessageTimer");
		await ToSignal(MessageTimer, "timeout");

		ShowTitleLabel();
	}

	//タイトル表示
	async public void ShowTitleLabel()
	{
		//タイトルを表示
		GetNode<Label>("TitleLabel").Show();
		EmitSignal("ShowTitle");
		//動的に1秒のタイマーを生成して、同期する
		await ToSignal(GetTree().CreateTimer(1), "timeout");
		//スタートボタンを表示
		GetNode<Button>("StartButton").Show();
		GetNode<Button>("ControlButton").Show();
		GetNode<Label>("HighScore").Show();
		GetNode<Label>("HighScoreNumber").Show();

		IsNotControlScene = true;
		IsNotStart = true;
	}

	//StartButtonが押されたとき
	private void _on_start_button_pressed()
	{
		GetNode<AudioStreamPlayer2D>("StartSound").Play();

		IsNotControlScene = false;

		GetNode<Button>("ControlButton").Hide();
		GetNode<Label>("TitleLabel").Hide();
		GetNode<Button>("StartButton").Hide();
		GetNode<Label>("HighScore").Hide();
		GetNode<Label>("HighScoreNumber").Hide();
		EmitSignal("StartGame");
	}

	//ControlButtonが押されたとき
	private void _on_control_button_pressed()
	{
		if(NoTouchZ) {
			GetNode<AudioStreamPlayer2D>("StartSound").Play();

			IsNotControlScene = false;

			GetNode<Button>("StartButton").Hide();
			GetNode<Sprite2D>("OperationTexture").Show();
		}
	}

	//CloseButtonが押されたとき
	private void _on_close_button_pressed()
	{
		GetNode<AudioStreamPlayer2D>("CloseSound").Play();
		GetNode<Button>("StartButton").Show();
		GetNode<Sprite2D>("OperationTexture").Hide();

		IsNotControlScene = true;
		NoTouchZ = false;

		GetNode<Timer>("NoTouchTimer").Start();
	}

	//NoTouchTimerが終わったとき
	private void _on_notouch_timer_timeout()
	{
		NoTouchZ = true;
	}

	//MessageTimerが終わったとき
	private void _on_message_timer_timeout()
	{
		GetNode<Label>("Message").Hide();
	}

	//残機数更新
	public void UpdatePlayerRest(int num)
	{
		var Rest = GetNode<Label>("RestNumber");
		Rest.Text = num.ToString();
		Rest.Show();
	}

	//ステージ数更新
	public void UpdateStageNum(int num)
	{
		var Stagenum = GetNode<Label>("StageNumber");
		Stagenum.Text = num.ToString();
		Stagenum.Show();
	}

	//スコア処理
	public void UpdateScoreNum(int num)
	{
		var Scorenum = GetNode<Label>("ScoreNumber");
		Scorenum.Text = num.ToString();
		Scorenum.Show();
	}

	//ハイスコア更新
	public void UpdateHighScoreNum(int num)
	{
		var Highscorenum = GetNode<Label>("HighScoreNumber");
		Highscorenum.Text = num.ToString();
	}
}

