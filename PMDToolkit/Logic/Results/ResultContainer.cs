using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results {
    public class ResultContainer : IResultContainer {

        private List<ResultBranch> branches;
        
        public int BranchCount { get { return branches.Count; } }

        public bool Empty
        {
            get
            {
                for (int i = 0; i < branches.Count; i++)
                {
                    if (branches[i].Results.Count > 0)
                        return false;
                }
                return true;
            }
        }

        public ResultContainer()
        {
            branches = new List<ResultBranch>();
            OpenNewBranch();
        }

        public bool IsFinished()
        {
            for (int i = 0; i < branches.Count; i++)
            {
                if (branches[i].Results.Count > 0 || branches[i].Delay > RenderTime.Zero)
                    return false;
            }
            return true;
        }

        public void OpenNewBranch()
        {
            branches.Add(new ResultBranch());
        }
        
        public void AddResult(IResult result) {
            branches[BranchCount - 1].Results.Enqueue(result);
        }

        public bool IsBranchEmpty()
        {
            return (branches[BranchCount - 1].Results.Count == 0);
        }

        public void ProcessDelay(RenderTime time)
        {
            for (int i = 0; i < BranchCount; i++)
            {
                branches[i].Delay -= time;
                if (branches[i].Delay < RenderTime.Zero)
                    branches[i].Delay = RenderTime.Zero;
            }
        }

        public IEnumerable<ResultBranch> GetAllBranches()
        {
            foreach(ResultBranch branch in branches)
                yield return branch;
        }
    }
}
