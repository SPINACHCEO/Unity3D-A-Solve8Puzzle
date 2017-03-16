using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public GameObject menu;
	public GameObject game;

	public PuzzleVisualizer player1;
	public PuzzleVisualizer player2;
	public TrulySpinach spinach;

	public Text time1;
	public Text time2;

	float startTime = 0;

	public void StartQuick(){
		StartGame (3);
	}

	public void StartNormal(){
		StartGame (4);
	}

	public void StartSlow(){
		StartGame (5);
	}

	void StartGame(int size){
		startTime = Time.realtimeSinceStartup;
		menu.SetActive (false);
		game.SetActive(true);

		player1.size = size;
		player2.size = size;

		player1.Init (() => {
			time1.text = "完成用时 ： " + (Time.realtimeSinceStartup-startTime).ToString();
		});
		player2.Init (() => {
			time2.text = "完成用时 ： " + (Time.realtimeSinceStartup-startTime).ToString();
		});

		player1.ButtonRandom (null);
		player2.ButtonRandom (() => {
			spinach.StartSolve();
		});
	}
}
