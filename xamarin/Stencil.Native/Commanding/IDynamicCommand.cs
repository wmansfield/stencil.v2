﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Native.Commanding
{
    public interface IDynamicCommand<TResult>
    {
        Task<string> CanExecuteAsync(ICommandScope commandScope);

        Task<TResult> ExecuteAsync(ICommandScope commandScope, object commandParameter);

        string ExtractValue(ICommandScope scope, string group, string name);
        Task<string> ValidateUserInputValuesAsync(ICommandScope scope, string group, params string[] fieldNames);
        ICommandField ExtractCommandField(ICommandScope scope, string group, string name);
        ConcurrentDictionary<string, ICommandField> ExtractCommandGroup(ICommandScope scope, string group);
    }
}
