using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Maths
{
	public class KdTree
	{
        private int _dimensions;
        private int _cellCapacity;

        private TreeNode _tree;

        public KdTree(int dimensions, Range[] extents, int cellCapacity)
        {
            _dimensions = dimensions;
            _cellCapacity = cellCapacity;
            _tree = new TreeNode(4, extents);
        }

        public void Insert(Sample<object> sample)
        {
            _tree.Insert(sample);
        }

        public void Clear()
        {
            _tree.Clear();
        }

        public List<Sample<object>> GatherNeighbours(float[] sample, float range)
        {
            List<Sample<object>> neighbours = new List<Sample<object>>();

            _tree.Search(sample, range, ref neighbours);

            return neighbours;
        }

    }
}

