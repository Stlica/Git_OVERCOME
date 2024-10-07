using Godot;
using System;

public partial class Game_clear : CanvasLayer
{
	[Signal]
	public delegate void GoneCreditEventHandler();
	
	public bool IsCreditmove = false; //クレジットが動いているか
	public bool IsViewMes = false;	  //メッセージが見えているか
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Node2D>("End").Hide();
		GetNode<Label>("Message").Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(IsCreditmove)
			GetNode<Node2D>("End").Position += new Vector2(0.0f, -2.0f);
			
		if(GetNode<Node2D>("End").Position.Y < 0.0f && !IsViewMes) {
			EmitSignal("GoneCredit");
			IsViewMes = true;
		}
	}
	
	//スコアの読み込み
	public void readScore(int score)
	{
		GetNode<Label>("Score").Text = score.ToString();
	}

	//クレジットが終わった時の処理
	private void _on_credit_timer_timeout()
	{
		GetNode<Label>("ClearLabel").Hide();
		GetNode<Label>("ScoreLabel").Hide();
		GetNode<Label>("Score").Hide();
		GetNode<Node2D>("End").Show();
		IsCreditmove = true;
	}
	
	//クレジットを表示
	public void creditShow()
	{
		IsCreditmove = false;
		IsViewMes = false;
		GetNode<Label>("ClearLabel").Show();
		GetNode<Label>("ScoreLabel").Show();
		GetNode<Label>("Score").Show();
		GetNode<Node2D>("End").Position = GetNode<Node2D>("Start").Position;
	}
}
