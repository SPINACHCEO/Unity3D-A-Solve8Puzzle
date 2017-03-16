using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using iS.iSCentralDispatch;

public class TrulyBrainPuzzle {

	Action<List<int>> cb;

	public void start(Puzzle p, Action<List<int>> callback){
		cb = callback;
		iSCentralDispatch.DispatchNewThread (() => {
			
			Node root = new Node (p);
			Debug.Log("brain start...");
			List<Node> frontier = new List<Node>();
			List<Puzzle> es = new List<Puzzle>();
			frontier.Add (root);

			while (true) {
				
				frontier = frontier.OrderBy (n => n.f ()).ToList ();

				Node cn = frontier [0];
				frontier.RemoveAt (0);
				es.Add(cn.cp);

				if (cn.goal ()){
					iSCentralDispatch.DispatchMainThread(() => {
						cb(cn.solution ());
					});

					break;
				}

				Node[] proceeds = cn.proceed ();
//				frontier.AddRange(proceeds)
				foreach(Node n in proceeds){
					bool hasSame = false;
					foreach(Puzzle esp in es){
						bool hasDifferent = false;
						for(int i = 0; i < esp.puzzle.Length; i++){
							PuzzleBlock b1 = esp.BlockForID(i);
							PuzzleBlock b2 = n.cp.BlockForID(i);
							if(b1 != null && b2 != null && b1.curPosition != b2.curPosition){
								hasDifferent = true;
							}
						}
						if(!hasDifferent){
							hasSame = true;
							break;
						}
					}

					if(hasSame) continue;
					frontier.Add(n);
				}
			}
		});
	}

	class Node{
		public Puzzle cp;
		List<int> aa = new List<int>();
		List<int> al = new List<int>();

		public Node(Puzzle puzzle){
			cp = puzzle;
			fetch();
		}

		void fetch(){
			foreach (PuzzleBlock b in cp.puzzle)
				if (b != null && cp.MoveableDirection (b.id) >= 0)
					aa.Add (b.id);
		}

		public int f(){
			int s = 0;
			foreach (PuzzleBlock b in cp.puzzle) if(b != null) s += h (b);
			return s;
		}

		int h(PuzzleBlock b){
			return (int)(Math.Abs (b.curPosition.x - b.wantedPosition.x) 
				+ Math.Abs (b.curPosition.y - b.wantedPosition.y));
		}

		public bool goal(){
			return f () == 0;
		}

		public List<int> solution(){
			return al;
		}

		public Node[] proceed(){
			Node[] ns = new Node[aa.Count];
			for (int i = 0; i < ns.Length; i++) {
				
				ns [i] = new Node (cp.Premove (aa [i]));
				ns [i].al = new List<int>();
				ns [i].al.AddRange (al);
				ns [i].al.Add (aa[i]);
			}
			return ns;
		}
	}
}
