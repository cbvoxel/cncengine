/**
    Copyright (C) 2015  Carsten Blank

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using log4net;

namespace CncEngine.Common.Ctrl.IfThenElse
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
