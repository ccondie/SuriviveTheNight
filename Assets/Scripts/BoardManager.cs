using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace SurviveTheNight {

	public enum grass { GRASS1, GRASS2, GRASS3, GRASS4 }
	public enum floor { 
		GRASS, TILES, 
		DIV_N, DIV_W, DIV_E, DIV_S, 
		DIV_NW, DIV_NE, DIV_SW, DIV_SE, 
		DIV_INW, DIV_INE, DIV_ISW, DIV_ISE, 
		DIV_NWSE, DIV_NESW, 
		SIDEWALK, ROAD,
		THRESH_N, THRESH_W, THRESH_E, THRESH_S 
	} ;
	public enum wall { EMPTY, WALL, WINDOW, DOORH, DOORV, HEDGE } ;

	public class BoardManager : MonoBehaviour {

		public int columns = 0;
		public int rows = 0;
		public static float scale = 0.6f;
		public GameObject[] grassTiles;
		public GameObject[] floorTiles;
		public GameObject[] wallTiles;
		public GameObject edgeTile;

		private Transform boardHolder;
		private int[,] floorMap = null;
		private int[,] wallMap = null;
		private List <Vector3> spawnPositions = new List <Vector3>();
		private List <Vector3> indoorPositions = new List <Vector3>();

		void LoadWorldMap() {

			String mapPath = "Assets/Maps/";
			/*
            String[] worldMaps = { 
				"4player.map",
				"suburbs.map"
			};
            */
            String[] worldMaps = {
                "suburbs.map"
            };

            System.IO.StreamReader file = new System.IO.StreamReader(mapPath + worldMaps[Random.Range (0, worldMaps.Length)]);
			String floorStr = file.ReadLine ();
			String wallStr = file.ReadLine ();
			file.Close();

			floorMap = parse2DarrayStr (floorStr);
			wallMap = parse2DarrayStr (wallStr);

			rows = floorMap.GetLength (0);
			columns = floorMap.GetLength (1);
		}

		int[,] parse2DarrayStr(String str) {
			str = str.Substring (2, str.Length - 4);
			String[] delim = new String[] {"],["};
			String[] r = str.Split(delim, StringSplitOptions.RemoveEmptyEntries);
			delim = new String[] {","};
			String[] c = r[0].Split(delim, StringSplitOptions.RemoveEmptyEntries);
			int[,] arr = new int[r.Length, c.Length];
			for(int i = 0; i < r.Length; i++) {
				c = r[i].Split(delim, StringSplitOptions.RemoveEmptyEntries);
				for(int j = 0; j < c.Length; j++)
					arr[i,j] = Int32.Parse(c[j]);
			}
			return arr;
		}

		void BoardSetup() {
			LoadWorldMap ();
			boardHolder = new GameObject ("Board").transform;
			GameObject toInstantiate = null;
			GameObject instance = null;
			int grassCount = Enum.GetNames (typeof(grass)).Length;
			// iterate through each cell of the board
			for (int x = -1; x <= columns; x++) {
				for (int y = -1; y <= rows; y++) {
					if(x == -1 || y == -1 || x == columns || y == rows) {
						toInstantiate = edgeTile;
						instance = Instantiate (toInstantiate, new Vector3 (x * scale, y * scale, 0f), Quaternion.identity) as GameObject;
						instance.transform.SetParent (boardHolder);
						continue;
					}
					if (floorMap [rows - y - 1, x] == (int)floor.GRASS)
						toInstantiate = grassTiles [Random.Range ((int)grass.GRASS1, grassCount)];
					else
						toInstantiate = floorTiles [floorMap [rows - y - 1, x]];
					instance = Instantiate (toInstantiate, new Vector3 (x * scale, y * scale, 0f), Quaternion.identity) as GameObject;
					instance.transform.SetParent (boardHolder);

					if (wallMap [rows - y - 1, x] != (int)wall.EMPTY) {
						toInstantiate = wallTiles [wallMap [rows - y - 1, x]];
						instance = Instantiate (toInstantiate, new Vector3 (x * scale, y * scale, 0f), Quaternion.identity) as GameObject;
						instance.transform.SetParent (boardHolder);
					} 
					else if (floorMap [rows - y - 1, x] == (int)floor.GRASS
						|| floorMap [rows - y - 1, x] == (int)floor.SIDEWALK 
						|| floorMap [rows - y - 1, x] == (int)floor.ROAD)
						spawnPositions.Add (new Vector3 (x * scale, y * scale, 0f));
					else if(floorMap [rows - y - 1, x] == (int)floor.TILES)
						indoorPositions.Add(new Vector3(x * scale, y * scale, 0f));
				}
			}
		}
					
		public void SetupScene() {
			BoardSetup ();
			// TO DO: generate walls, enemies
		}

        public int[,] getWallMap() {
            return wallMap;
		}

		public Vector3 getRandomSpawnPosition() {
			return spawnPositions[Random.Range (0, spawnPositions.Count)];
		}

		public Vector3 getRandomIndoorPosition() {
			return indoorPositions[Random.Range (0, indoorPositions.Count)];
		}
	}

}
