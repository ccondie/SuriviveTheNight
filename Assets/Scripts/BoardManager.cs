using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace SurviveTheNight {

	public enum floor { GRASS1, GRASS2, GRASS3, GRASS4, TILES, N, S, E, W, NW, NE, SW, SE } ;
	public enum wall { WALL, WINDOW, DOORV, DOORH, NONE } ;

	public class BoardManager : MonoBehaviour {

		[Serializable]
		public class Count {
			public int minimum;
			public int maximum;
			public Count (int min, int max) {
				minimum = min;
				maximum = max;
			}
		}

		public static int columns = 16;
		public static int rows = 12;
		public static float scale = 0.6f;
		public GameObject[] floorTiles;
		public GameObject[] wallTiles;

		private Transform boardHolder;
		private List <Vector3> gridPositions = new List <Vector3> ();
		private int[,] worldMapFloorLayer = new int[rows, columns];
		private int[,] worldMapWallLayer = new int[rows, columns];

		void LoadWorldMap() {
			for (int r = 0; r < rows; r++) {
				for (int c = 0; c < columns; c++) {
					worldMapFloorLayer [r, c] = Random.Range ((int)floor.GRASS1, 4);
					worldMapWallLayer [r, c] = (int)wall.NONE;
				}
			}
			LoadHouseFloorTiles (new Count (2, 6), new Count (1, 4));
			worldMapWallLayer [4, 5] = (int)wall.DOORH;
			worldMapWallLayer [4, 3] = (int)wall.WINDOW;
			worldMapWallLayer [2, 2] = (int)wall.WINDOW;
			worldMapWallLayer [3, 6] = (int)wall.WINDOW;
			worldMapWallLayer [1, 4] = (int)wall.WINDOW;
			LoadHouseFloorTiles (new Count (3, 7), new Count (7, 10));
			worldMapWallLayer [7, 4] = (int)wall.DOORH;
			worldMapWallLayer [7, 6] = (int)wall.WINDOW;
			worldMapWallLayer [8, 3] = (int)wall.WINDOW;
			worldMapWallLayer [9, 7] = (int)wall.WINDOW;
			worldMapWallLayer [10, 5] = (int)wall.WINDOW;
			LoadHouseFloorTiles (new Count (8, 12), new Count (1, 4));
			worldMapWallLayer [4, 11] = (int)wall.DOORH;
			worldMapWallLayer [4, 9] = (int)wall.WINDOW;
			worldMapWallLayer [2, 8] = (int)wall.WINDOW;
			worldMapWallLayer [3, 12] = (int)wall.WINDOW;
			worldMapWallLayer [1, 10] = (int)wall.WINDOW;
			LoadHouseFloorTiles (new Count (9, 13), new Count (7, 10));
			worldMapWallLayer [7, 10] = (int)wall.DOORH;
			worldMapWallLayer [7, 12] = (int)wall.WINDOW;
			worldMapWallLayer [8, 9] = (int)wall.WINDOW;
			worldMapWallLayer [9, 13] = (int)wall.WINDOW;
			worldMapWallLayer [10, 11] = (int)wall.WINDOW;
		}

		void LoadHouseFloorTiles(Count width, Count depth) {
			for (int r = depth.minimum; r <= depth.maximum; r++) {
				for (int c = width.minimum; c <= width.maximum; c++) {
					worldMapWallLayer [r, c] = (int)wall.WALL;
					if (r == depth.minimum) {
						if (c == width.minimum) {
							worldMapFloorLayer [r, c] = (int)floor.SW;
						} else if (c == width.maximum) {
							worldMapFloorLayer [r, c] = (int)floor.SE;
						} else
							worldMapFloorLayer [r, c] = (int)floor.S;
					} else if (r == depth.maximum) {
						if (c == width.minimum) {
							worldMapFloorLayer [r, c] = (int)floor.NW;
						} else if (c == width.maximum) {
							worldMapFloorLayer [r, c] = (int)floor.NE;
						} else
							worldMapFloorLayer [r, c] = (int)floor.N;
					} else if (c == width.minimum) {
						worldMapFloorLayer [r, c] = (int)floor.W;
					} else if (c == width.maximum) {
						worldMapFloorLayer [r, c] = (int)floor.E;
					} else {
						worldMapFloorLayer [r, c] = (int)floor.TILES;
						worldMapWallLayer [r, c] = (int)wall.NONE;
					}
				}
			}
		}

		void InitializeList() {
			gridPositions.Clear ();
			for(int x = 0; x < columns; x++)
				for(int y = 0; y < rows; y++)
					gridPositions.Add(new Vector3(x*scale,y*scale,0f));
		}

		void BoardSetup() {
			LoadWorldMap ();
			boardHolder = new GameObject ("Board").transform;
			for (int x = 0; x < columns; x++) {
				for (int y = 0; y < rows; y++) {
					GameObject toInstantiate = floorTiles [worldMapFloorLayer [y, x]];
					GameObject instance = Instantiate (toInstantiate, new Vector3 (x * scale, y * scale, 0f), Quaternion.identity) as GameObject;
					instance.transform.SetParent (boardHolder);
					if(worldMapWallLayer [y, x] != (int)wall.NONE) {
						toInstantiate = wallTiles [worldMapWallLayer [y, x]];
						instance = Instantiate (toInstantiate, new Vector3 (x * scale, y * scale, 0f), Quaternion.identity) as GameObject;
						instance.transform.SetParent (boardHolder);
					}
				}
			}
		}
					
		public void SetupScene() {
			BoardSetup ();
			// TO DO: generate walls, enemies
		}
	}

}
