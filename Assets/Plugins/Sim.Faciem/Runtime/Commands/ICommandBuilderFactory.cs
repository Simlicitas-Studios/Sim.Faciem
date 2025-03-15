using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Sim.Faciem.Commands
{
    public interface ICommandBuilderFactory
    {
        CommandBuilder Execute(Action action);
        AsyncCommandBuilder ExecuteAsync(Func<CancellationToken, UniTask> action);
    }
}