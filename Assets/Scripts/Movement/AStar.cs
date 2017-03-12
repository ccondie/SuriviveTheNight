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
        private int[,] characterMap;
        private int mapWidth;
        private int mapHeight;
        private Tile[,] tiles;
        private bool adjustedTarget = false;
        HeapPriorityQueue<Tile> q;

        private float scale;

        public AStar(Vector2 start, Vector2 target, float scale) {
            this.start  = start;
            this.target = target;
            wallMap     = GameManager.instance.getWallMap();
            mapWidth    =  wallMap.GetLength(0);
            mapHeight   = wallMap.GetLength(1);
            characterMap = new int[mapWidth, mapHeight];
            this.scale  = scale;
            q = new HeapPriorityQueue<Tile>(mapHeight * mapWidth);
            tiles = initTileArray(mapWidth, mapHeight);
            initTileVals();
            establishCharacterLocations();
        }

        private void initTileVals() {
            xStart = worldToTile(start.x);
            yStart = worldToTile(start.y);
            xTarget = worldToTile(target.x);
            yTarget = worldToTile(target.y);
        }

        private void establishCharacterLocations() {
            List<GameObject> enemies = GameManager.instance.getEnemies();
            foreach(GameObject e in enemies) {
				if (e == null)
					continue;
                characterMap[worldToTile(e.transform.position.x), worldToTile(e.transform.position.y)] = (int) character.ENEMY;
            }
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

        private bool targetEnclosed(int depth) {
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
                    return false;
                } else if (depth < 0) {
                    return false;
                }

                //check y+1
                checkNeighbor(current, current.x, current.y + 1);
                //check y-1
                checkNeighbor(current, current.x, current.y - 1);
                //check x+1
                checkNeighbor(current, current.x + 1, current.y);
                //check x-1
                checkNeighbor(current, current.x - 1, current.y);
                depth--;
            }
            return true;
        }

        private void chooseBetterTarget() {
            adjustedTarget = true;
            Queue<Tile> tileQueue = new Queue<Tile>();
            tileQueue.Enqueue(tiles[xTarget, yTarget]);
            while(tileQueue.Count != 0) {
                Tile current = tileQueue.Dequeue();
                current.checkedForBetterTarget = true;

                //check y+1
                if (!tiles[current.x, current.y + 1].checkedForBetterTarget) {
                    if (!tileObstructed(tiles[current.x, current.y + 1])) {
                        xTarget = current.x;
                        yTarget = current.y + 1;
                        return;
                    } else if (!tileObstructedByMap(tiles[current.x, current.y + 1])){
                        tileQueue.Enqueue(tiles[current.x, current.y + 1]);
                    }
                }
                //check y-1
                if (!tiles[current.x, current.y - 1].checkedForBetterTarget) {
                    if (!tileObstructed(tiles[current.x, current.y - 1])) {
                        xTarget = current.x;
                        yTarget = current.y - 1;
                        return;
                    } else if (!tileObstructedByMap(tiles[current.x, current.y - 1])) {
                        tileQueue.Enqueue(tiles[current.x, current.y - 1]);
                    }
                }
                //check x+1
                if (!tiles[current.x + 1, current.y].checkedForBetterTarget) {
                    if (!tileObstructed(tiles[current.x + 1, current.y])) {
                        xTarget = current.x + 1;
                        yTarget = current.y;
                        return;
                    } else if (!tileObstructedByMap(tiles[current.x + 1, current.y])) {
                        tileQueue.Enqueue(tiles[current.x + 1, current.y]);
                    }
                }
                //check x-1
                if (!tiles[current.x - 1, current.y].checkedForBetterTarget) {
                    if (!tileObstructed(tiles[current.x - 1, current.y])) {
                        xTarget = current.x - 1;
                        yTarget = current.y;
                        return;
                    } else if (!tileObstructedByMap(tiles[current.x - 1, current.y])) {
                        tileQueue.Enqueue(tiles[current.x - 1, current.y]);
                    }
                }
            }
        }

        public Path calculatePath() {

            AStar a = new AStar(target, this.start, scale);
            if (a.targetEnclosed(100)) {
                //Debug.Log("target enclosed");
                //Debug.Log("\t old target: " + xTarget + "," + yTarget);
                chooseBetterTarget();
                //Debug.Log("\t new target: " + xTarget + "," + yTarget);
            }

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
            while(current.cameFrom != null) {
                path.steps.Add(tileToVector(current));
                //Debug.Log("Step tile: " + current.x + ", " + current.y);
                //Debug.Log("Step coord: " + tileToVector(current));

                if (current.x == xStart && current.y == yStart) {
                    break;
                }

                current = current.cameFrom;
            }
            path.adjustedTarget = this.adjustedTarget;
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
            if (tileObstructedByMap(t)) {
                return true;
            } else if (tileObstructedByEnemy(t)) {
                return true;
            }
            return false;
        }

        private bool tileObstructedByMap(Tile t) {
            //wallMap[i,j] is indexed from top left corner
            //and i,j corresponds to row,col
            //as opposed to Unity coordinates which are indexed from bottom left corner
            //and x,y in this code corresponds to col,row
            if (wallMap[mapHeight - 1 - t.y, t.x] == (int) wall.HEDGE) {
                return true;
            } else if (wallMap[mapHeight - 1 - t.y, t.x] == (int) wall.WALL) {
                return true;
            } else if (wallMap[mapHeight - 1 - t.y, t.x] == (int) wall.WINDOW) {
                return true;
            } else
            return false;
        }

        private bool tileObstructedByEnemy(Tile t) {
            if (characterMap[t.x, t.y] == (int)character.ENEMY) {
                return true;
            }
            return false;
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
        public bool checkedForBetterTarget { get; set; }

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
            checkedForBetterTarget = false;
        }
    }
}
