using System;
using System.Collections.Generic;
using System.Linq;
using log4net;

namespace CncEngine.Common.Ctrl
{
    public class IfExecutor
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(IfExecutor));

        private readonly Message _message;
        internal Func<Message, bool> ConditionExpressionFunc;
        internal Func<Message, Message> ElseBranchFunc;

        internal List<ConditionMessageProcessor> ConditionMessageProcessorList = new List<ConditionMessageProcessor>();

        internal IfExecutor(Message message, Func<Message, bool> conditionExpressionFunc)
        {
            _message = message;
            ConditionExpressionFunc = conditionExpressionFunc;
            ConditionMessageProcessorList.Add(new ConditionMessageProcessor(conditionExpressionFunc));
            ElseBranchFunc = m => m;
        }

        public ThenExecutor Then(Func<Message, Message> thenPart)
        {
            Logger.Debug("Step");
            ConditionMessageProcessorList.Last().ProcessorFunc = thenPart;
            return new ThenExecutor(this);
        }

        internal Message Execute()
        {
            Logger.Debug("Step");
            var firstCondition = ConditionMessageProcessorList
                .Where( m => m.ConditionExpressionFunc(_message))
                .Select( (m,i) => new { Index = i, Value = m})
                .FirstOrDefault();

            Logger.DebugFormat("One condition found to be true={0}", firstCondition != null);
            if (firstCondition != null)
            {
                Logger.DebugFormat("Condition Index {0} executing process.", firstCondition.Index);
                var result = firstCondition.Value.ProcessorFunc(_message);
                Logger.DebugFormat("Condition Index {0} process finished.", firstCondition.Index);
                return result;
            }
            else
            {
                Logger.DebugFormat("Else Condition executing process.");
                var result = ElseBranchFunc(_message);
                Logger.DebugFormat("Else Condition process finished");
                return result;
            }
        }
    }
}
