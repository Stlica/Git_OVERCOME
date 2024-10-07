using Godot;
using System;

public partial class Hud : CanvasLayer
{
	public bool isPushGuard = false;
	public int  Interval = 0;
	public bool	StartFlag = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<TextureProgressBar>("HPBar2").Hide();
		GetNode<TextureProgressBar>("HPBar3").Hide();
		GetNode<Button>("PauseButton").Hide();
		GetNode<Popup>("Popup").Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var player = GetNode<Player>;

		var bar = GetNode<TextureProgressBar>("BarrierBar01");
		//bar.Value += 20 * delta; //テスト
		bar.Value += 5 * delta;
		
		
		if (bar.Value >= 100){
			//GetNode<AudioStreamPlayer2D>("S_Sound").Play();
			if(Input.IsActionJustPressed("Special") && !isPushGuard){
				//GetNode<ProgressBar>("SpecialBar").Value = 0;
				GetNode<Timer>("Timer").Start();
				isPushGuard = true;
			}
			//bar.Value = 0;
		}

		var cbar = GetNode<TextureProgressBar>("SpecialBar01");
		//cbar.Value += 20 * delta; //テスト
		cbar.Value += 5 * delta; 
		
		
		if(Input.IsActionPressed("Pause")){
			_on_pause_button_pressed();
		}
		
//		if(!GetTree().Paused){
//			GetNode<Button>("PauseButton").Show();
//		}
		
		
	}

	public double SpecialGauge()
	{
		return GetNode<TextureProgressBar>("BarrierBar01").Value;
	}
	
	public void GetHP(int value)
	{
		var bar = GetNode<TextureProgressBar>("HPBar3");
		bar.Value = value;
		bar.Show();
	}
	
	private void _on_timer_timeout()
	{
		GetNode<TextureProgressBar>("BarrierBar01").Value = 0;
		isPushGuard = false;
	}

	private void _on_pause_button_pressed()
	{
		GetNode<Button>("PauseButton").Hide();
		GetTree().Paused = true;
		GetNode<Popup>("Popup").Show();
		GetNode<AudioStreamPlayer2D>("C_Sound").Play();
	}
	
}



