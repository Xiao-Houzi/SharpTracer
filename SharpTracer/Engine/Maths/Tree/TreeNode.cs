using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Maths
{
	public class TreeNode
	{
        static int depth;
        private int l;
        private TreeNode[] _children;
        private Range[] _limits;
        private int _capacity;

        public TreeNode[] Children
        {
            get
            {
                return _children;
            }
            set
            {
                _children = value;
            }
        }
        public List<Sample<object>> Content;


        public TreeNode(int capacity, Range[] limits)
        {

            _capacity = capacity;
            _limits = limits;
            _children = null;
            Content = new List<Sample<object>>();
        }

        internal void Clear()
        {
            if(Children == null) return;
            foreach(TreeNode n in Children)
                n.Clear();
            Children = null;
        }

        internal void Search(float[] position, float range, ref List<Sample<object>> neighbours, int level = 0)
        {
            range = Math.Abs(range);
            // does the range intersect the node limits
            for(int i = 0; i < position.Length; i++)
            {
                if(position[0] - range > _limits[i].UpperBound || position[i] + range < _limits[i].LowerBound) return;
            }

            // check children
            if(Children != null)
                foreach(TreeNode n in Children)
                    n.Search(position, range, ref neighbours, level + 1);

            // check content
            if(Content.Count > 0)
                foreach(Sample<object> s in Content)
                {
                    // if s inside range
                    neighbours.Add(s);
                }
        }

        public void Insert(Sample<object> sample)
        {
            for(int i = 0; i < sample.Dimensions; i++)
            {
                if(!_limits[i].IsInRange(sample[i])) return;
            }
            if(Children != null)
            {
                foreach(TreeNode n in Children)
                    n.Insert(sample);
            }
            else
            {
                Content.Add(sample);
                if(Content.Count > _capacity)
                {
                    Subdivide(l + 1);
                }
            }
        }

        private void Subdivide(int level)
        {
            int n = (int)Math.Pow(2, Content[0].Dimensions);
            Children = new TreeNode[n];
            for(int i = 0; i < n; i++) Children[i] = new TreeNode(4, new Range[2]) { l = level };
            Split(Children, 0);
            foreach(Sample<object> s in Content)
            {
                Insert(s);
            }
            Content.Clear();
            if(l > depth) depth = l;
        }

        private void Split(TreeNode[] nodes, int dim)
        {
            TreeNode[] first = new TreeNode[nodes.Length / 2];
            TreeNode[] last = new TreeNode[nodes.Length / 2];

            for(int i = 0; i < nodes.Length; i += 2)
            {
                first[i / 2] = nodes[i];
                last[i / 2] = nodes[i + 1];
            }

            foreach(TreeNode n in first)
            {
                n._limits[dim] = new Range(_limits[dim].LowerBound, (_limits[dim].LowerBound + _limits[dim].UpperBound) / 2.0f);
            }
            foreach(TreeNode n in last)
            {
                n._limits[dim] = new Range((_limits[dim].LowerBound + _limits[dim].UpperBound) / 2.0f, _limits[dim].UpperBound);
            }
            if(first.Length > 1)
            {
                Split(first, dim + 1);
                Split(last, dim + 1);
            }
        }
    }
}