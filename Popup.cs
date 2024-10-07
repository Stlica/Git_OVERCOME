using Godot;
using System;

public partial class Popup : Godot.Popup
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionPressed("Close")){
			_on_close_button_pressed();
		}
	}

	//CloseButtonが押されたとき
	private void _on_close_button_pressed()
	{
		Hide();
		GetTree().Paused = false;
	}
}


