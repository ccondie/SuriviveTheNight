using System;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

namespace SurviveTheNight{

    public class AStar {

        private Vector2 start;
        private int xStart;
        private int yStart;
        private Vector2 target;
        private int xTarget;
        private int yTarget;
        private int[,] wallMap;
        private int mapWidth;
        private int mapHeight;
        private Tile[,] tiles;
        HeapPriorityQueue<Tile> q;

        private float scale;

        public AStar(Vector2 start, Vector2 target, float scale) {
            this.start  = start;
            this.target = target;
            wallMap     = GameManager.instance.getWallMap();
            /*wallMap = new int[5, 5];
            wallMap[1, 1] = 1;
            //wallMap[2, 1] = 1;
            wallMap[3, 1] = 1;
            wallMap[3, 2] = 1;
            wallMap[3, 3] = 1;
            //wallMap[2, 3] = 1;
            wallMap[1, 3] = 1;
            wallMap[1, 2] = 1;

            wallMap[0, 3] = 1;*/
            mapWidth    =  wallMap.GetLength(0);
            mapHeight   = wallMap.GetLength(1);
            this.scale  = scale;
            q = new HeapPriorityQueue<Tile>(mapHeight * mapWidth);
            tiles = initTileArray(mapWidth, mapHeight);
            initTileVals();
        }

        private void initTileVals() {
            xStart = worldToTile(start.x);
            yStart = worldToTile(start.y);
            xTarget = worldToTile(target.x);
            yTarget = worldToTile(target.y);
            /*Debug.Log("Start tile: " + xStart + ", " + yStart);
            Debug.Log("Target tile: " + xTarget + ", " + yTarget);
            Debug.Log("Start coord: " + start);
            Debug.Log("Target coord: " + target);*/
            /*xStart = 0;
            yStart = 2;
            xTarget = 0;
            yTarget = 4;*/
        }

        private Tile[,] initTileArray(int height, int width) {
            Tile[,] arr = new Tile[width, height];
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    arr[i, j] = new SurviveTheNight.Tile(i, j);
                }
            }
            return arr;
        }

        private Tile[,] getPositionArray(int height, int width) {
            Tile[,] arr = new Tile[width, height];
            return arr;
        }

        public Path calculatePath() {

            Tile start = new Tile(xStart, yStart);
            tiles[xStart, yStart] = start;
            start.openSet = true;
            start.gScore = 0;
            start.fScore = heuristic(start);
            q.Enqueue(start, start.fScore);

            while (q.Count > 0) {
                Tile current = q.Dequeue();
                current.openSet = false;
                current.closedSet = true;

                if (current.x == xTarget && current.y == yTarget) {
                    break;
                }

                //check y+1
                checkNeighbor(current, current.x, current.y + 1);
                //check y-1
                checkNeighbor(current, current.x, current.y - 1);
                //check x+1
                checkNeighbor(current, current.x + 1, current.y);
                //check x-1
                checkNeighbor(current, current.x - 1, current.y);
            }

            return reconstructPath();
        }

        private Path reconstructPath() {
            Path path = new SurviveTheNight.Path();
            Tile current = tiles[xTarget, yTarget];
            while(true) {
                path.steps.Add(tileToVector(current));
                //Debug.Log("Step tile: " + current.x + ", " + current.y);
                //Debug.Log("Step coord: " + tileToVector(current));

                if (current.x == xStart && current.y == yStart) {
                    break;
                }

                current = current.cameFrom;
            }
            return path;
        }

        private void checkNeighbor(Tile current, int x, int y) {
            if (x < 0 || y < 0 || x == mapWidth || y == mapHeight) {
                return;
            }

            Tile neighbor = tiles[x, y];

            if (!neighbor.closedSet && !tileObstructed(neighbor)) {
                int tempGScore = current.gScore + 1;
                if (!neighbor.openSet) {
                    neighbor.fScore = tempGScore + heuristic(neighbor);
                    neighbor.openSet = true;
                    q.Enqueue(neighbor, neighbor.fScore);
                } else if (tempGScore >= neighbor.gScore) {
                    return;
                }
                neighbor.cameFrom = current;
                neighbor.gScore = tempGScore;
                neighbor.fScore = tempGScore + heuristic(neighbor);
                q.UpdatePriority(neighbor, neighbor.fScore);
            }
        }

        private bool tileObstructed(Tile t) {
            //wallMap[i,j] is indexed from top left corner
            //and i,j corresponds to row,col
            //as opposed to Unity coordinates which are indexed from bottom left corner
            //and x,y in this code corresponds to col,row
            if (wallMap[mapHeight - 1 - t.y, t.x] == 0) {
                return false;
            } else if (wallMap[mapHeight - 1 - t.y, t.x] == 3) {
                return false;
            } else if (wallMap[mapHeight - 1 - t.y, t.x] == 4) {
                return false;
            }
            return true;
        }

        private int worldToTile(float position) {
            return (int)((position + (scale / 2)) / scale);
        }

        private float tileToWorld(int position) {
            return (float)((position * scale));
        }

        private Vector2 tileToVector(Tile t) {
            return new Vector2(tileToWorld(t.x), tileToWorld(t.y));
        }

        private double heuristic(Tile t) {
            int dX = xTarget - t.x;
            int dY = yTarget - t.y;
            double xPow = Math.Pow(dX, 2);
            double yPow = Math.Pow(dY, 2);
            double sqrt = Math.Sqrt(xPow + yPow);
            return sqrt;
            //return Math.Sqrt(Math.Pow(xTarget - t.x, 2) + Math.Pow(yTarget - t.y, 2));
        }

    }

    public class Tile : PriorityQueueNode {

        public int x { get; set; }
        public int y { get; set; }
        public Tile cameFrom { get; set; }
        public bool closedSet { get; set; }
        public bool openSet { get; set; }
        public int gScore { get; set; }
        public double fScore { get; set; }

        public Tile() {
            x = int.MaxValue;
            y = int.MaxValue;
            initVals();
        }

        public Tile(int x, int y) {
            this.x = x;
            this.y = y;
            initVals();
        }

        private void initVals() {
            closedSet = false;
            openSet = false;
            gScore = int.MaxValue;
            fScore = double.MaxValue;
        }
    }
}
