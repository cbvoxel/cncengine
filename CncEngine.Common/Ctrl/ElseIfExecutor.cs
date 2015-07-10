using System;
using System.Linq;

namespace CncEngine.Common.Ctrl
{
    public class ElseIfExecutor
    {
        private readonly IfExecutor _ifExecutor;

        public ElseIfExecutor(IfExecutor ifExecutor)
        {
            _ifExecutor = ifExecutor;
        }

        public ThenExecutor Then(Func<Message, Message> thenPart)
        {
            _ifExecutor.ConditionMessageProcessorList.Last().ProcessorFunc = thenPart;
            return new ThenExecutor(_ifExecutor);
        }

        public Message EndIf()
        {
            return _ifExecutor.Execute();
        }
    }
}