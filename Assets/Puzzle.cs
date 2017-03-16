using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using iS.iSCentralDispatch;

public class Puzzle {

	public PuzzleBlock[,] puzzle;
	int currentSize = -1;

	public Puzzle(){}

	private Puzzle(PuzzleBlock[,] p, int cs){
		PuzzleBlock[,] n = new PuzzleBlock[cs, cs];
		//lock (puzzle) {
			for (int x = 0; x < cs; x++) {
				for (int y = 0; y < cs; y++) {
					//Debug.Log (puzzle [x, y]);
				if (p [x, y] != null) {
						n [x, y] = new PuzzleBlock {
						curPosition = p [x, y].curPosition,
						wantedPosition = p [x, y].wantedPosition,
						id = p [x, y].id
						};
					} else
						n [x, y] = null;
				}
			}
		//}

		puzzle = n;
		currentSize = cs;
	}

	public void RandomizePuzzle(int depth){
		System.Random r = new System.Random (System.DateTime.Now.Millisecond);
		for(int i = 0; i < depth; i++){
			List<int> mb = new List<int> ();
			foreach (PuzzleBlock b in puzzle)
				if (b != null && MoveableDirection (b.id) >= 0)
					mb.Add (b.id);


			MoveBlock (mb [r.Next(0, mb.Count)]);
		}
	}

	public bool MoveBlock(int id){
		PuzzleBlock tb = BlockForID (id);
		int moveableDir = MoveableDirection (id);
		if (moveableDir < 0)return false;

		Vector2 wantedPos = Vector2.zero;

		switch (moveableDir) {
		case 0: wantedPos = tb.curPosition + Vector2.up; break;
		case 1: wantedPos = tb.curPosition + Vector2.down; break;
		case 2: wantedPos = tb.curPosition + Vector2.left; break;
		case 3: wantedPos = tb.curPosition + Vector2.right; break;
		}

		PuzzleBlock cache = BlockAtPosition (wantedPos);
		if(cache != null)cache.curPosition = tb.curPosition;
		tb.curPosition = wantedPos;

		return true;
	}

	public bool IsCompleted(){
		bool c = true;
		foreach (PuzzleBlock b in puzzle) {
			if (b != null && b.curPosition != b.wantedPosition)
				c = false;
		}
		return c;
	}

	public Puzzle Premove(int id){
		Puzzle p = new Puzzle (puzzle, currentSize);
		p.MoveBlock (id);
		return p;
	}

	public int MoveableDirection(int id){
		PuzzleBlock tb = BlockForID (id);
//		if (tb == null)
//			return -1;
		if (InsidePuzzle(tb.curPosition + Vector2.up) && BlockAtPosition (tb.curPosition + Vector2.up) == null) return 0;
		else if (InsidePuzzle(tb.curPosition + Vector2.down) && BlockAtPosition (tb.curPosition + Vector2.down) == null) return 1;
		else if (InsidePuzzle(tb.curPosition + Vector2.left) && BlockAtPosition (tb.curPosition + Vector2.left) == null) return 2;
		else if (InsidePuzzle(tb.curPosition + Vector2.right) && BlockAtPosition (tb.curPosition + Vector2.right) == null) return 3;
		else return -1;
	}

	bool InsidePuzzle(Vector2 pos){
		return new Rect (0, 0, currentSize, currentSize).Contains (pos);
	}

	public PuzzleBlock BlockAtPosition(Vector2 pos){
		foreach (PuzzleBlock b in puzzle)
			if (b != null && b.curPosition == pos)
				return b;
		return null;
	}

	public PuzzleBlock BlockForID(int id){
		foreach (PuzzleBlock p in puzzle)
			if (p!= null && p.id == id)
				return p;
		return null;
	}

	public void GeneratePuzzle(int size){
		currentSize = size;
		puzzle = new PuzzleBlock[size, size];
		int count = 0;
		for (int x = 0; x < size; x++) {
			for (int y = 0; y < size; y++) {
				
				PuzzleBlock pb = new PuzzleBlock {
					curPosition = new Vector2(x,y),
					wantedPosition = new Vector2(x,y),
					id = count
				};
				count++;
				puzzle [x, y] = pb;
			}
		}
		puzzle [size - 1, size - 1] = null;
	}
}

[System.Serializable]
public class PuzzleBlock{
	public Vector2 curPosition;
	public Vector2 wantedPosition;
	public int id;
}
