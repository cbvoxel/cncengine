using System;
using log4net;

namespace CncEngine.Common.Ctrl
{
    public class ThenExecutor
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ThenExecutor));

        private readonly IfExecutor _ifExecutor;

        internal ThenExecutor(IfExecutor ifExecutor)
        {
            _ifExecutor = ifExecutor;
        }

        public ElseIfExecutor ElseIf(Func<Message, bool> func)
        {
            Logger.Debug("Step");
            _ifExecutor.ConditionMessageProcessorList.Add(new ConditionMessageProcessor(func));
            return new ElseIfExecutor(_ifExecutor);
        }

        public ElseExecutor Else(Func<Message, Message> elsePart)
        {
            Logger.Debug("Step");
            _ifExecutor.ElseBranchFunc = elsePart;
            return new ElseExecutor(_ifExecutor);
        }

        public Message EndIf()
        {
            Logger.Debug("Step");
            return _ifExecutor.Execute();
        }
    }
}