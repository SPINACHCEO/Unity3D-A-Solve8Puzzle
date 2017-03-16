using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using iS.iSCentralDispatch;
using System;
public class PuzzleVisualizer : MonoBehaviour {

	public int size;
	public bool controlable;
	public Image template;
	public Transform holder;
	public float blockSpacing;
	public float blockSpeed;
	public bool REVERSE;

	Action finishCb;
	bool visualizeable = false;
	Transform[] images;
	public Puzzle puzzle;

	public void SetVisualizeable(bool v){
		visualizeable = v;
	}

	public void Init (Action cb) {

		puzzle = new Puzzle ();
		puzzle.GeneratePuzzle (size);
		images = new Transform[size * size - 1];

		for(int i = 0;i < images.Length; i++){
			GameObject n = Instantiate (template.gameObject) as GameObject;
			n.transform.SetParent (holder);

			Image img = n.GetComponent<Image> ();
			Color c = UnityEngine.Random.ColorHSV ();
			c.a = 1f;
			img.color = c;

			if (c.grayscale > 0.6f)
				n.GetComponentInChildren<Text> ().color = Color.black;

			n.GetComponentInChildren<Text> ().text = (i+1).ToString ();
			if (controlable)
				n.GetComponent<Block> ().Setup (this, i);

			images [i] = n.transform;
		}
		SetVisualizeable (true);
		finishCb = cb;
	}

	public void ButtonRandom(Action cb){
		iSCentralDispatch.DispatchNewThread (() => {
			puzzle.RandomizePuzzle (22480);
			if(cb != null){
				iSCentralDispatch.DispatchMainThread(() => {
					cb();
				});
			}
		});
	}

	public void Move(int id){
		puzzle.MoveBlock (id);
		if (finishCb != null && puzzle.IsCompleted ())
			finishCb ();
	}

	void Update () {
		if (!visualizeable)
			return;
		
		for(int i = 0; i < images.Length; i++){
			PuzzleBlock p = puzzle.BlockForID(i);
			if (p == null) continue;

			float haftfullSize = 0;//blockSpacing * size / 2f;

			float esY = -p.curPosition.x * blockSpacing - haftfullSize;
			float esX = p.curPosition.y * blockSpacing;
			if(REVERSE) esX -= blockSpacing * size;

			RectTransform t = images[i].GetComponent<RectTransform>();
			t.localPosition = Vector3.Lerp (t.localPosition, new Vector3 (esX, esY, 0), blockSpeed * Time.deltaTime);
		}
	}
}
