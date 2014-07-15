using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results {
    public interface IResultContainer
    {

        int BranchCount { get; }

        bool Empty
        {
            get;
        }

        bool IsFinished();

        void AddResult(IResult result);

        bool IsBranchEmpty();

        void ProcessDelay(RenderTime time);

        IEnumerable<ResultBranch> GetAllBranches();
    }
}
