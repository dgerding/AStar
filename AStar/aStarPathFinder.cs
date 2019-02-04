using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStar
{
    public class AstarPathfinder
    {
        List<List<AstarNode>> Grid;
        int GridRows
        {
            get
            {
                return Grid[0].Count;
            }
        }
        int GridCols
        {
            get
            {
                return Grid.Count;
            }
        }

        public AstarPathfinder(List<List<AstarNode>> grid)
        {
            Grid = grid;
        }

        public Stack<AstarNode> FindPath(Vector2 Start, Vector2 End)
        {
            AstarNode start = new AstarNode(new Vector2((int)(Start.X / AstarNode.NODE_SIZE), (int)(Start.Y / AstarNode.NODE_SIZE)), true);
            AstarNode end = new AstarNode(new Vector2((int)(End.X / AstarNode.NODE_SIZE), (int)(End.Y / AstarNode.NODE_SIZE)), true);

            Stack<AstarNode> Path = new Stack<AstarNode>();
            List<AstarNode> OpenList = new List<AstarNode>();
            List<AstarNode> ClosedList = new List<AstarNode>();
            List<AstarNode> adjacencies;
            AstarNode current = start;

            // add start node to Open List
            OpenList.Add(start);

            while (OpenList.Count != 0 && !ClosedList.Exists(x => x.Position == end.Position))
            {
                current = OpenList[0];
                OpenList.Remove(current);
                ClosedList.Add(current);
                adjacencies = GetAdjacentNodes(current);


                foreach (AstarNode n in adjacencies)
                {
                    if (!ClosedList.Contains(n) && n.Walkable)
                    {
                        if (!OpenList.Contains(n))
                        {
                            n.Parent = current;
                            n.DistanceToTarget = Math.Abs(n.Position.X - end.Position.X) + Math.Abs(n.Position.Y - end.Position.Y);
                            n.Cost = 1 + n.Parent.Cost;
                            OpenList.Add(n);
                            OpenList = OpenList.OrderBy(node => node.F).ToList<AstarNode>();
                        }
                    }
                }
            }

            // construct path, if end was not closed return null
            if (!ClosedList.Exists(x => x.Position == end.Position))
            {
                return null;
            }

            // if all good, return path
            AstarNode temp = ClosedList[ClosedList.IndexOf(current)];
            while (temp.Parent != start && temp != null)
            {
                Path.Push(temp);
                temp = temp.Parent;
            }
            return Path;
        }

        private List<AstarNode> GetAdjacentNodes(AstarNode n)
        {
            List<AstarNode> temp = new List<AstarNode>();

            int row = (int)n.Position.Y;
            int col = (int)n.Position.X;

            if (row + 1 < GridRows)
            {
                temp.Add(Grid[col][row + 1]);
            }
            if (row - 1 >= 0)
            {
                temp.Add(Grid[col][row - 1]);
            }
            if (col - 1 >= 0)
            {
                temp.Add(Grid[col - 1][row]);
            }
            if (col + 1 < GridCols)
            {
                temp.Add(Grid[col + 1][row]);
            }

            return temp;
        }
    }
}
