using Machina.CModels;
using Machina.TypeSystem;
using Machina.ValueSystem;
using System;
using System.Collections.Generic;

namespace Machina.Models.Function
{
    public class MachinaFunction
    {
        internal CFunction Function { get; }

        private readonly Stack<IMachinaValue> _stack = new();

        private readonly Dictionary<CIdentifier, IMachinaValue> _memory = new();

        public MachinaFunction(IMachinaType returnType, string name, List<CVariableInfo> parameters)
        {
            Function = new(returnType, new CIdentifier(name), parameters, new());
        }

        public void LoadBool(bool value)
        {
            Load(new MachinaValueBool(value));
        }

        public void NotBool()
        {
            var boolean = Pop();

            boolean.Type.ExpectType(new MachinaTypeBool());

            if (boolean.IsConst)
                boolean = new MachinaValueBool(!Convert.ToBoolean(Convert.ToInt32(((MachinaValueBool)boolean).Value)));
            else
                boolean = new MachinaValueNot(boolean);

            Load(boolean);
        }

        public void LoadConstInt8(ushort value)
        {
            Load(new MachinaValueInt(new MachinaTypeInt8(), value));
        }

        public void LoadConstInt32(uint value)
        {
            Load(new MachinaValueInt(new MachinaTypeInt32(), value));
        }

        public void LoadConstInt64(ulong value)
        {
            Load(new MachinaValueInt(new MachinaTypeInt64(), value));
        }

        public void Load(IMachinaValue value)
        {
            _stack.Push(value);
        }

        private IMachinaValue Pop()
        {
            return _stack.Pop();
        }

        public void DeclareMemory(IMachinaValue value, string name)
        {
            Function.Body.DeclareVariable(value.Type, name);
            _memory.Add(new CIdentifier(name), value);
        }

        private IMachinaValue GetMemory(string name)
        {
            return _memory[new CIdentifier(name)];
        }

        private void PopSameIntTypes(out IMachinaValue left, out IMachinaValue right)
        {
            right = Pop();
            left = Pop();

            left.Type.ExpectType(right.Type);
            left.Type.ExpectIntType();
        }

        public void AddInt()
        {
            PopSameIntTypes(out var left, out var right);

            IMachinaValue result;
            if (left.IsConst && right.IsConst)
                result = new MachinaValueInt(left.Type, (ulong)((MachinaValueInt)left).Value + (ulong)((MachinaValueInt)right).Value);
            else
                result = new MachinaValueBinary(left, right, BinaryOperator.Add);

            Load(result);
        }

        public void SubInt()
        {
            PopSameIntTypes(out var left, out var right);

            IMachinaValue result;
            if (left.IsConst && right.IsConst)
                result = new MachinaValueInt(left.Type, (ulong)((MachinaValueInt)left).Value - (ulong)((MachinaValueInt)right).Value);
            else
                result = new MachinaValueBinary(left, right, BinaryOperator.Sub);

            Load(result);
        }

        public void MulInt()
        {
            PopSameIntTypes(out var left, out var right);

            IMachinaValue result;
            if (left.IsConst && right.IsConst)
                result = new MachinaValueInt(left.Type, (ulong)((MachinaValueInt)left).Value * (ulong)((MachinaValueInt)right).Value);
            else
                result = new MachinaValueBinary(left, right, BinaryOperator.Mul);

            Load(result);
        }

        public void DivInt()
        {
            PopSameIntTypes(out var left, out var right);

            IMachinaValue result;
            if (left.IsConst && right.IsConst)
                result = new MachinaValueInt(left.Type, (ulong)((MachinaValueInt)left).Value / (ulong)((MachinaValueInt)right).Value);
            else
                result = new MachinaValueBinary(left, right, BinaryOperator.Div);

            Load(result);
        }

        public void StoreMemory(string name)
        {
            Function.Body.AssignVariable(name, Pop());
        }

        public void LoadFromMemory(string name)
        {
            Load(GetMemory(name));
        }

        public void Return()
        {
            var value = Pop();

            value.Type.ExpectType(Function.Prototype.ReturnType);

            Function.Body.Return(value);
        }
    }
}
