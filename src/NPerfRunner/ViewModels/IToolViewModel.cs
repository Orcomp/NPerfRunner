using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPerfRunner.ViewModels
{
    public interface IToolViewModel : IPaneViewModel
    {
        string Name { get; }

        
    }
}
