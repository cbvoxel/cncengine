using System;

namespace CncEngine.Common.Ctrl
{
    internal class ConditionMessageProcessor
    {
        internal Func<Message, bool> ConditionExpressionFunc;
        internal Func<Message, Message> ProcessorFunc;

        internal ConditionMessageProcessor(Func<Message, bool> conditionFunc)
        {
            ConditionExpressionFunc = conditionFunc;
            ProcessorFunc = m => m;
        }
    }
}