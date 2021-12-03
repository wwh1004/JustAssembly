using System;
using System.Collections.Generic;

namespace JustAssembly.Core.DiffAlgorithm.Algorithm
{
    public interface IDiffAlgorithm
    {
        IList<DiffItem> DiffText(IList<string> textA, IList<string> textB);
    }
}
