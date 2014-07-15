using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results {
    public class MotionResultContainer : IResultContainer
    {

        private Dictionary<int, ResultBranch> branches;
        private int currentId;

        public int BranchCount { get { return branches.Count; } }

        public bool Empty
        {
            get
            {
                foreach (KeyValuePair<int, ResultBranch> entry in branches)
                {
                    if (entry.Value.Results.Count > 0)
                        return false;
                }
                return true;
            }
        }

        public MotionResultContainer(int id)
        {
            branches = new Dictionary<int, ResultBranch>();
            OpenBranch(id);
        }

        public bool IsFinished()
        {
            foreach (KeyValuePair<int, ResultBranch> entry in branches)
            {
                if (entry.Value.Results.Count > 0 || entry.Value.Delay > RenderTime.Zero)
                    return false;
            }
            return true;
        }


        public void OpenBranch(int id)
        {
            if (!branches.ContainsKey(id))
                branches.Add(id, new ResultBranch());

            currentId = id;
        }
        
        public void AddResult(IResult result) {
            branches[currentId].Results.Enqueue(result);
        }
        
        public bool IsBranchEmpty()
        {
            return (branches[currentId].Results.Count == 0);
        }

        public void ProcessDelay(RenderTime time)
        {
            foreach (KeyValuePair<int, ResultBranch> entry in branches)
            {
                entry.Value.Delay -= time;
                if (entry.Value.Delay < RenderTime.Zero)
                    entry.Value.Delay = RenderTime.Zero;
            }
        }

        public IEnumerable<ResultBranch> GetAllBranches()
        {
            foreach (KeyValuePair<int, ResultBranch> entry in branches)
            {
                yield return entry.Value;
            }
        }
    }
}
