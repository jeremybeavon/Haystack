using Haystack.Core;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Haystack.Diagnostics.Amendments.Amender
{
    internal sealed class TypeAmender
    {
        private ModuleDefinition module;


        public void AmendType(TypeDefinition typeDefinition, TypeAmendment typeAmendment)
        {
            AmendProperties(typeDefinition, typeAmendment);
        }

        private void AmendProperties(TypeDefinition typeDefinition, TypeAmendment typeAmendment)
        {
            foreach (PropertyDefinition propertyDefinition in typeDefinition.Properties)
            {
                PropertyAmendment propertyAmendment = typeAmendment.GetPropertyAmendment(propertyDefinition);
                if (propertyAmendment == null)
                {
                    continue;
                }

                string memberName = propertyDefinition.Name;
                PrependExpressions(propertyAmendment.BeforePropertyGetExpressions, propertyDefinition.GetMethod, memberName);
                AddExpressions(propertyAmendment.AfterPropertyGetExpressions, propertyDefinition.GetMethod, memberName);
                PrependExpressions(propertyAmendment.BeforePropertySetExpressions, propertyDefinition.SetMethod, memberName);
                AddExpressions(propertyAmendment.AfterPropertySetExpressions, propertyDefinition.SetMethod, memberName);
            }
        }

        private void PrependExpressions(IEnumerable<LambdaExpression> expressions, MethodDefinition method, string memberName)
        {
            foreach (LambdaExpression expression in expressions)
            {
                method.Body.Instructions.Insert(0, InstructionBuilder.GetInstructions(expression, module, memberName));
            }
        }

        private void AddExpressions(IEnumerable<LambdaExpression> expressions, MethodDefinition method, string memberName)
        {
            IEnumerable<int> returnIndexes = method.Body.Instructions
                .Select((instruction, index) => new { Instruction = instruction, Index = index })
                .Where(instruction => instruction.Instruction.OpCode == OpCodes.Ret)
                .OrderByDescending(instruction => instruction.Index)
                .Select(instruction => instruction.Index);
            foreach (int returnIndex in returnIndexes)
            {
                foreach (LambdaExpression expression in expressions)
                {
                    method.Body.Instructions.Insert(returnIndex, InstructionBuilder.GetInstructions(expression, module, memberName));
                }
            }
        }
    }
}
