﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NPerfRunner.Dialogs
{
    public interface ILoadAssemblyDialog
    {
        Assembly LoadAssembly();
    }
}
