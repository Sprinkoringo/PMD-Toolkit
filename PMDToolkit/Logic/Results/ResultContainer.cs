/*The MIT License (MIT)

Copyright (c) 2014 PMU Staff

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/


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
