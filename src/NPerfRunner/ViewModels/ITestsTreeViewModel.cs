using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPerfRunner.ViewModels
{
    using ReactiveUI;

    public interface ITestsTreeViewModel : IToolViewModel
    {
        IReactiveCollection Testers { get; }
    }
}
