using ClrTest.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EmitOpCode = System.Reflection.Emit.OpCode;
using EmitOpCodes = System.Reflection.Emit.OpCodes;
using MonoOpCode = Mono.Cecil.Cil.OpCode;
using MonoOpCodes = Mono.Cecil.Cil.OpCodes;

namespace Haystack.Diagnostics.Amendments.Amender
{
    internal sealed class InstructionBuilder : ILInstructionVisitor
    {
        private static readonly IDictionary<EmitOpCode, MonoOpCode> opCodes = new Dictionary<EmitOpCode, MonoOpCode>()
        {
            { EmitOpCodes.Add, MonoOpCodes.Add },
            { EmitOpCodes.Add_Ovf, MonoOpCodes.Add_Ovf },
            { EmitOpCodes.Add_Ovf_Un, MonoOpCodes.Add_Ovf_Un },
            { EmitOpCodes.And, MonoOpCodes.And },
            { EmitOpCodes.Arglist, MonoOpCodes.Arglist },
            { EmitOpCodes.Beq, MonoOpCodes.Beq },
            { EmitOpCodes.Beq_S, MonoOpCodes.Beq_S },
            { EmitOpCodes.Bge, MonoOpCodes.Bge },
            { EmitOpCodes.Bge_S, MonoOpCodes.Bge_S },
            { EmitOpCodes.Bge_Un, MonoOpCodes.Bge_Un },
            { EmitOpCodes.Bge_Un_S, MonoOpCodes.Bge_Un_S },
            { EmitOpCodes.Bgt, MonoOpCodes.Bgt },
            { EmitOpCodes.Bgt_S, MonoOpCodes.Bgt_S },
            { EmitOpCodes.Bgt_Un, MonoOpCodes.Bgt_Un },
            { EmitOpCodes.Bgt_Un_S, MonoOpCodes.Bgt_Un_S },
            { EmitOpCodes.Ble, MonoOpCodes.Ble },
            { EmitOpCodes.Ble_S, MonoOpCodes.Ble_S },
            { EmitOpCodes.Ble_Un, MonoOpCodes.Ble_Un },
            { EmitOpCodes.Ble_Un_S, MonoOpCodes.Ble_Un_S },
            { EmitOpCodes.Blt, MonoOpCodes.Blt },
            { EmitOpCodes.Blt_S, MonoOpCodes.Blt_S },
            { EmitOpCodes.Blt_Un, MonoOpCodes.Blt_Un },
            { EmitOpCodes.Blt_Un_S, MonoOpCodes.Blt_Un_S },
            { EmitOpCodes.Bne_Un, MonoOpCodes.Bne_Un },
            { EmitOpCodes.Bne_Un_S, MonoOpCodes.Bne_Un_S },
            { EmitOpCodes.Box, MonoOpCodes.Box },
            { EmitOpCodes.Br, MonoOpCodes.Br },
            { EmitOpCodes.Br_S, MonoOpCodes.Br_S },
            { EmitOpCodes.Break, MonoOpCodes.Break },
            { EmitOpCodes.Brfalse, MonoOpCodes.Brfalse },
            { EmitOpCodes.Brfalse_S, MonoOpCodes.Brfalse_S },
            { EmitOpCodes.Brtrue, MonoOpCodes.Brtrue },
            { EmitOpCodes.Brtrue_S, MonoOpCodes.Brtrue_S },
            { EmitOpCodes.Call, MonoOpCodes.Call },
            { EmitOpCodes.Calli, MonoOpCodes.Calli },
            { EmitOpCodes.Callvirt, MonoOpCodes.Callvirt },
            { EmitOpCodes.Castclass, MonoOpCodes.Castclass },
            { EmitOpCodes.Ceq, MonoOpCodes.Ceq },
            { EmitOpCodes.Cgt, MonoOpCodes.Cgt },
            { EmitOpCodes.Cgt_Un, MonoOpCodes.Cgt_Un },
            { EmitOpCodes.Ckfinite, MonoOpCodes.Ckfinite },
            { EmitOpCodes.Clt, MonoOpCodes.Clt },
            { EmitOpCodes.Clt_Un, MonoOpCodes.Clt_Un },
            { EmitOpCodes.Constrained, MonoOpCodes.Constrained },
            { EmitOpCodes.Conv_I, MonoOpCodes.Conv_I },
            { EmitOpCodes.Conv_I1, MonoOpCodes.Conv_I1 },
            { EmitOpCodes.Conv_I2, MonoOpCodes.Conv_I2 },
            { EmitOpCodes.Conv_I4, MonoOpCodes.Conv_I4 },
            { EmitOpCodes.Conv_I8, MonoOpCodes.Conv_I8 },
            { EmitOpCodes.Conv_Ovf_I, MonoOpCodes.Conv_Ovf_I },
            { EmitOpCodes.Conv_Ovf_I_Un, MonoOpCodes.Conv_Ovf_I_Un },
            { EmitOpCodes.Conv_Ovf_I1, MonoOpCodes.Conv_Ovf_I1 },
            { EmitOpCodes.Conv_Ovf_I1_Un, MonoOpCodes.Conv_Ovf_I1_Un },
            { EmitOpCodes.Conv_Ovf_I2, MonoOpCodes.Conv_Ovf_I2 },
            { EmitOpCodes.Conv_Ovf_I2_Un, MonoOpCodes.Conv_Ovf_I2_Un },
            { EmitOpCodes.Conv_Ovf_I4, MonoOpCodes.Conv_Ovf_I4 },
            { EmitOpCodes.Conv_Ovf_I4_Un, MonoOpCodes.Conv_Ovf_I4_Un },
            { EmitOpCodes.Conv_Ovf_I8, MonoOpCodes.Conv_Ovf_I8 },
            { EmitOpCodes.Conv_Ovf_I8_Un, MonoOpCodes.Conv_Ovf_I8_Un },
            { EmitOpCodes.Conv_Ovf_U, MonoOpCodes.Conv_Ovf_U },
            { EmitOpCodes.Conv_Ovf_U_Un, MonoOpCodes.Conv_Ovf_U_Un },
            { EmitOpCodes.Conv_Ovf_U1, MonoOpCodes.Conv_Ovf_U1 },
            { EmitOpCodes.Conv_Ovf_U1_Un, MonoOpCodes.Conv_Ovf_U1_Un },
            { EmitOpCodes.Conv_Ovf_U2, MonoOpCodes.Conv_Ovf_U2 },
            { EmitOpCodes.Conv_Ovf_U2_Un, MonoOpCodes.Conv_Ovf_U2_Un },
            { EmitOpCodes.Conv_Ovf_U4, MonoOpCodes.Conv_Ovf_U4 },
            { EmitOpCodes.Conv_Ovf_U4_Un, MonoOpCodes.Conv_Ovf_U4_Un },
            { EmitOpCodes.Conv_Ovf_U8, MonoOpCodes.Conv_Ovf_U8 },
            { EmitOpCodes.Conv_Ovf_U8_Un, MonoOpCodes.Conv_Ovf_U8_Un },
            { EmitOpCodes.Conv_R_Un, MonoOpCodes.Conv_R_Un },
            { EmitOpCodes.Conv_R4, MonoOpCodes.Conv_R4 },
            { EmitOpCodes.Conv_R8, MonoOpCodes.Conv_R8 },
            { EmitOpCodes.Conv_U, MonoOpCodes.Conv_U },
            { EmitOpCodes.Conv_U1, MonoOpCodes.Conv_U1 },
            { EmitOpCodes.Conv_U2, MonoOpCodes.Conv_U2 },
            { EmitOpCodes.Conv_U4, MonoOpCodes.Conv_U4 },
            { EmitOpCodes.Conv_U8, MonoOpCodes.Conv_U8 },
            { EmitOpCodes.Cpblk, MonoOpCodes.Cpblk },
            { EmitOpCodes.Cpobj, MonoOpCodes.Cpobj },
            { EmitOpCodes.Div, MonoOpCodes.Div },
            { EmitOpCodes.Div_Un, MonoOpCodes.Div_Un },
            { EmitOpCodes.Dup, MonoOpCodes.Dup },
            { EmitOpCodes.Endfilter, MonoOpCodes.Endfilter },
            { EmitOpCodes.Endfinally, MonoOpCodes.Endfinally },
            { EmitOpCodes.Initblk, MonoOpCodes.Initblk },
            { EmitOpCodes.Initobj, MonoOpCodes.Initobj },
            { EmitOpCodes.Isinst, MonoOpCodes.Isinst },
            { EmitOpCodes.Jmp, MonoOpCodes.Jmp },
            { EmitOpCodes.Ldarg, MonoOpCodes.Ldarg },
            { EmitOpCodes.Ldarg_0, MonoOpCodes.Ldarg_0 },
            { EmitOpCodes.Ldarg_1, MonoOpCodes.Ldarg_1 },
            { EmitOpCodes.Ldarg_2, MonoOpCodes.Ldarg_2 },
            { EmitOpCodes.Ldarg_3, MonoOpCodes.Ldarg_3 },
            { EmitOpCodes.Ldarg_S, MonoOpCodes.Ldarg_S },
            { EmitOpCodes.Ldarga, MonoOpCodes.Ldarga },
            { EmitOpCodes.Ldarga_S, MonoOpCodes.Ldarga_S },
            { EmitOpCodes.Ldc_I4, MonoOpCodes.Ldc_I4 },
            { EmitOpCodes.Ldc_I4_0, MonoOpCodes.Ldc_I4_0 },
            { EmitOpCodes.Ldc_I4_1, MonoOpCodes.Ldc_I4_1 },
            { EmitOpCodes.Ldc_I4_2, MonoOpCodes.Ldc_I4_2 },
            { EmitOpCodes.Ldc_I4_3, MonoOpCodes.Ldc_I4_3 },
            { EmitOpCodes.Ldc_I4_4, MonoOpCodes.Ldc_I4_4 },
            { EmitOpCodes.Ldc_I4_5, MonoOpCodes.Ldc_I4_5 },
            { EmitOpCodes.Ldc_I4_6, MonoOpCodes.Ldc_I4_6 },
            { EmitOpCodes.Ldc_I4_7, MonoOpCodes.Ldc_I4_7 },
            { EmitOpCodes.Ldc_I4_8, MonoOpCodes.Ldc_I4_8 },
            { EmitOpCodes.Ldc_I4_M1, MonoOpCodes.Ldc_I4_M1 },
            { EmitOpCodes.Ldc_I4_S, MonoOpCodes.Ldc_I4_S },
            { EmitOpCodes.Ldc_I8, MonoOpCodes.Ldc_I8 },
            { EmitOpCodes.Ldc_R4, MonoOpCodes.Ldc_R4 },
            { EmitOpCodes.Ldc_R8, MonoOpCodes.Ldc_R8 },
            { EmitOpCodes.Ldelem_I, MonoOpCodes.Ldelem_I },
            { EmitOpCodes.Ldelem_I1, MonoOpCodes.Ldelem_I1 },
            { EmitOpCodes.Ldelem_I2, MonoOpCodes.Ldelem_I2 },
            { EmitOpCodes.Ldelem_I4, MonoOpCodes.Ldelem_I4 },
            { EmitOpCodes.Ldelem_I8, MonoOpCodes.Ldelem_I8 },
            { EmitOpCodes.Ldelem_R4, MonoOpCodes.Ldelem_R4 },
            { EmitOpCodes.Ldelem_R8, MonoOpCodes.Ldelem_R8 },
            { EmitOpCodes.Ldelem_Ref, MonoOpCodes.Ldelem_Ref },
            { EmitOpCodes.Ldelem_U1, MonoOpCodes.Ldelem_U1 },
            { EmitOpCodes.Ldelem_U2, MonoOpCodes.Ldelem_U2 },
            { EmitOpCodes.Ldelem_U4, MonoOpCodes.Ldelem_U4 },
            { EmitOpCodes.Ldelema, MonoOpCodes.Ldelema },
            { EmitOpCodes.Ldfld, MonoOpCodes.Ldfld },
            { EmitOpCodes.Ldflda, MonoOpCodes.Ldflda },
            { EmitOpCodes.Ldftn, MonoOpCodes.Ldftn },
            { EmitOpCodes.Ldind_I, MonoOpCodes.Ldind_I },
            { EmitOpCodes.Ldind_I1, MonoOpCodes.Ldind_I1 },
            { EmitOpCodes.Ldind_I2, MonoOpCodes.Ldind_I2 },
            { EmitOpCodes.Ldind_I4, MonoOpCodes.Ldind_I4 },
            { EmitOpCodes.Ldind_I8, MonoOpCodes.Ldind_I8 },
            { EmitOpCodes.Ldind_R4, MonoOpCodes.Ldind_R4 },
            { EmitOpCodes.Ldind_R8, MonoOpCodes.Ldind_R8 },
            { EmitOpCodes.Ldind_Ref, MonoOpCodes.Ldind_Ref },
            { EmitOpCodes.Ldind_U1, MonoOpCodes.Ldind_U1 },
            { EmitOpCodes.Ldind_U2, MonoOpCodes.Ldind_U2 },
            { EmitOpCodes.Ldind_U4, MonoOpCodes.Ldind_U4 },
            { EmitOpCodes.Ldlen, MonoOpCodes.Ldlen },
            { EmitOpCodes.Ldloc, MonoOpCodes.Ldloc },
            { EmitOpCodes.Ldloc_0, MonoOpCodes.Ldloc_0 },
            { EmitOpCodes.Ldloc_1, MonoOpCodes.Ldloc_1 },
            { EmitOpCodes.Ldloc_2, MonoOpCodes.Ldloc_2 },
            { EmitOpCodes.Ldloc_3, MonoOpCodes.Ldloc_3 },
            { EmitOpCodes.Ldloc_S, MonoOpCodes.Ldloc_S },
            { EmitOpCodes.Ldloca, MonoOpCodes.Ldloca },
            { EmitOpCodes.Ldloca_S, MonoOpCodes.Ldloca_S },
            { EmitOpCodes.Ldnull, MonoOpCodes.Ldnull },
            { EmitOpCodes.Ldobj, MonoOpCodes.Ldobj },
            { EmitOpCodes.Ldsfld, MonoOpCodes.Ldsfld },
            { EmitOpCodes.Ldsflda, MonoOpCodes.Ldsflda },
            { EmitOpCodes.Ldstr, MonoOpCodes.Ldstr },
            { EmitOpCodes.Ldtoken, MonoOpCodes.Ldtoken },
            { EmitOpCodes.Ldvirtftn, MonoOpCodes.Ldvirtftn },
            { EmitOpCodes.Leave, MonoOpCodes.Leave },
            { EmitOpCodes.Leave_S, MonoOpCodes.Leave_S },
            { EmitOpCodes.Localloc, MonoOpCodes.Localloc },
            { EmitOpCodes.Mkrefany, MonoOpCodes.Mkrefany },
            { EmitOpCodes.Mul, MonoOpCodes.Mul },
            { EmitOpCodes.Mul_Ovf, MonoOpCodes.Mul_Ovf },
            { EmitOpCodes.Mul_Ovf_Un, MonoOpCodes.Mul_Ovf_Un },
            { EmitOpCodes.Neg, MonoOpCodes.Neg },
            { EmitOpCodes.Newarr, MonoOpCodes.Newarr },
            { EmitOpCodes.Newobj, MonoOpCodes.Newobj },
            { EmitOpCodes.Nop, MonoOpCodes.Nop },
            { EmitOpCodes.Not, MonoOpCodes.Not },
            { EmitOpCodes.Or, MonoOpCodes.Or },
            { EmitOpCodes.Pop, MonoOpCodes.Pop },
            { EmitOpCodes.Readonly, MonoOpCodes.Readonly },
            { EmitOpCodes.Refanytype, MonoOpCodes.Refanytype },
            { EmitOpCodes.Refanyval, MonoOpCodes.Refanyval },
            { EmitOpCodes.Rem, MonoOpCodes.Rem },
            { EmitOpCodes.Rem_Un, MonoOpCodes.Rem_Un },
            { EmitOpCodes.Ret, MonoOpCodes.Ret },
            { EmitOpCodes.Rethrow, MonoOpCodes.Rethrow },
            { EmitOpCodes.Shl, MonoOpCodes.Shl },
            { EmitOpCodes.Shr, MonoOpCodes.Shr },
            { EmitOpCodes.Shr_Un, MonoOpCodes.Shr_Un },
            { EmitOpCodes.Sizeof, MonoOpCodes.Sizeof },
            { EmitOpCodes.Starg, MonoOpCodes.Starg },
            { EmitOpCodes.Starg_S, MonoOpCodes.Starg_S },
            { EmitOpCodes.Stelem_I, MonoOpCodes.Stelem_I },
            { EmitOpCodes.Stelem_I1, MonoOpCodes.Stelem_I1 },
            { EmitOpCodes.Stelem_I2, MonoOpCodes.Stelem_I2 },
            { EmitOpCodes.Stelem_I4, MonoOpCodes.Stelem_I4 },
            { EmitOpCodes.Stelem_I8, MonoOpCodes.Stelem_I8 },
            { EmitOpCodes.Stelem_R4, MonoOpCodes.Stelem_R4 },
            { EmitOpCodes.Stelem_R8, MonoOpCodes.Stelem_R8 },
            { EmitOpCodes.Stelem_Ref, MonoOpCodes.Stelem_Ref },
            { EmitOpCodes.Stfld, MonoOpCodes.Stfld },
            { EmitOpCodes.Stind_I, MonoOpCodes.Stind_I },
            { EmitOpCodes.Stind_I1, MonoOpCodes.Stind_I1 },
            { EmitOpCodes.Stind_I2, MonoOpCodes.Stind_I2 },
            { EmitOpCodes.Stind_I4, MonoOpCodes.Stind_I4 },
            { EmitOpCodes.Stind_I8, MonoOpCodes.Stind_I8 },
            { EmitOpCodes.Stind_R4, MonoOpCodes.Stind_R4 },
            { EmitOpCodes.Stind_R8, MonoOpCodes.Stind_R8 },
            { EmitOpCodes.Stind_Ref, MonoOpCodes.Stind_Ref },
            { EmitOpCodes.Stloc, MonoOpCodes.Stloc },
            { EmitOpCodes.Stloc_0, MonoOpCodes.Stloc_0 },
            { EmitOpCodes.Stloc_1, MonoOpCodes.Stloc_1 },
            { EmitOpCodes.Stloc_2, MonoOpCodes.Stloc_2 },
            { EmitOpCodes.Stloc_3, MonoOpCodes.Stloc_3 },
            { EmitOpCodes.Stloc_S, MonoOpCodes.Stloc_S },
            { EmitOpCodes.Stobj, MonoOpCodes.Stobj },
            { EmitOpCodes.Stsfld, MonoOpCodes.Stsfld },
            { EmitOpCodes.Sub, MonoOpCodes.Sub },
            { EmitOpCodes.Sub_Ovf, MonoOpCodes.Sub_Ovf },
            { EmitOpCodes.Sub_Ovf_Un, MonoOpCodes.Sub_Ovf_Un },
            { EmitOpCodes.Switch, MonoOpCodes.Switch },
            { EmitOpCodes.Throw, MonoOpCodes.Throw },
            { EmitOpCodes.Unaligned, MonoOpCodes.Unaligned },
            { EmitOpCodes.Unbox, MonoOpCodes.Unbox },
            { EmitOpCodes.Unbox_Any, MonoOpCodes.Unbox_Any },
            { EmitOpCodes.Volatile, MonoOpCodes.Volatile },
            { EmitOpCodes.Xor, MonoOpCodes.Xor },
        };

        private readonly ModuleDefinition module;
        private readonly IList<Instruction> instructions;

        private InstructionBuilder(ModuleDefinition module)
        {
            this.module = module;
            instructions = new List<Instruction>();
        }

        public static IList<Instruction> GetInstructions(LambdaExpression expression, ModuleDefinition module, string memberName)
        {
            expression = ExpressionRewriter.RewriteExpression(expression, memberName);
            InstructionBuilder builder = new InstructionBuilder(module);
            new ILReader(expression.Compile().Method).Accept(builder);
            return builder.instructions;
        }
        
        public override void VisitInlineBrTargetInstruction(InlineBrTargetInstruction inlineBrTargetInstruction)
        {
            throw new NotSupportedException();
        }

        public override void VisitInlineFieldInstruction(InlineFieldInstruction inlineFieldInstruction)
        {
            instructions.Add(Instruction.Create(GetOpCode(inlineFieldInstruction), module.Import(inlineFieldInstruction.Field)));
        }

        public override void VisitInlineI8Instruction(InlineI8Instruction inlineI8Instruction)
        {
            instructions.Add(Instruction.Create(GetOpCode(inlineI8Instruction), inlineI8Instruction.Int64));
        }

        public override void VisitInlineIInstruction(InlineIInstruction inlineIInstruction)
        {
            instructions.Add(Instruction.Create(GetOpCode(inlineIInstruction), inlineIInstruction.Int32));
        }

        public override void VisitInlineMethodInstruction(InlineMethodInstruction inlineMethodInstruction)
        {
            instructions.Add(Instruction.Create(GetOpCode(inlineMethodInstruction), module.Import(inlineMethodInstruction.Method)));
        }

        public override void VisitInlineNoneInstruction(InlineNoneInstruction inlineNoneInstruction)
        {
            instructions.Add(Instruction.Create(GetOpCode(inlineNoneInstruction)));
        }

        public override void VisitInlineRInstruction(InlineRInstruction inlineRInstruction)
        {
            instructions.Add(Instruction.Create(GetOpCode(inlineRInstruction), inlineRInstruction.Double));
        }

        public override void VisitInlineSigInstruction(InlineSigInstruction inlineSigInstruction)
        {
            throw new NotSupportedException();
        }

        public override void VisitInlineStringInstruction(InlineStringInstruction inlineStringInstruction)
        {
            instructions.Add(Instruction.Create(GetOpCode(inlineStringInstruction), inlineStringInstruction.String));
        }

        public override void VisitInlineSwitchInstruction(InlineSwitchInstruction inlineSwitchInstruction)
        {
            throw new NotSupportedException();
        }

        public override void VisitInlineTokInstruction(InlineTokInstruction inlineTokInstruction)
        {
            throw new NotSupportedException();
        }

        public override void VisitInlineTypeInstruction(InlineTypeInstruction inlineTypeInstruction)
        {
            instructions.Add(Instruction.Create(GetOpCode(inlineTypeInstruction), module.Import(inlineTypeInstruction.Type)));
        }

        public override void VisitInlineVarInstruction(InlineVarInstruction inlineVarInstruction)
        {
            instructions.Add(Instruction.Create(GetOpCode(inlineVarInstruction), inlineVarInstruction.Ordinal));
        }

        public override void VisitShortInlineBrTargetInstruction(ShortInlineBrTargetInstruction shortInlineBrTargetInstruction)
        {
            throw new NotSupportedException();
        }

        public override void VisitShortInlineIInstruction(ShortInlineIInstruction shortInlineIInstruction)
        {
            instructions.Add(Instruction.Create(GetOpCode(shortInlineIInstruction), shortInlineIInstruction.Byte));
        }

        public override void VisitShortInlineRInstruction(ShortInlineRInstruction shortInlineRInstruction)
        {
            instructions.Add(Instruction.Create(GetOpCode(shortInlineRInstruction), shortInlineRInstruction.Single));
        }

        public override void VisitShortInlineVarInstruction(ShortInlineVarInstruction shortInlineVarInstruction)
        {
            instructions.Add(Instruction.Create(GetOpCode(shortInlineVarInstruction), shortInlineVarInstruction.Ordinal));
        }

        private static MonoOpCode GetOpCode(ILInstruction instruction)
        {
            MonoOpCode monoOpCode;
            if (!opCodes.TryGetValue(instruction.OpCode, out monoOpCode))
            {
                throw new NotSupportedException("opCode is not supported: " + instruction.OpCode);
            }

            return monoOpCode;
        }
    }
}
