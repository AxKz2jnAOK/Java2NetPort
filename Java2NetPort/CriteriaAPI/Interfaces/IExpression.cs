﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Java2NetPort.CriteriaAPI
{
    public interface IExpression : ISelection
    {
        string ExpressionValue { get; }
    }
}
