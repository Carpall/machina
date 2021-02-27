using Machina.CModels;
using Machina.TypeSystem;
using Machina.ValueSystem;
using System;
using System.Collections.Generic;

namespace Machina.Models.Function
{
    public class MachinaFunction : IMachinaValue
    {
        internal CFunction Function { get; }
        public IMachinaType Type { get; }
        public bool IsConst => true;
        public bool CanBePointed => throw new NotImplementedException();

        private readonly Stack<IMachinaValue> _stack = new();

        private readonly Dictionary<CIdentifier, MachinaValueMemory> _memory = new();


        public MachinaFunction(IMachinaType returnType, string name, List<CVariableInfo> parameters)
        {
            Function = new(returnType, CIdentifier.FunctionPrototype(name, parameters), parameters, new());

            parameters.ForEach(parameter =>  DeclareMemory(parameter.Type, parameter.Name.Name, false));

            Type = new MachinaTypeFunctionPointer(Function.Prototype);
        }

        private MachinaValueCall Call(IMachinaType returntype, CIdentifier name, int paramcount)
        {
            var parameters = new List<IMachinaValue>();
            for (int i = 0; i < paramcount; i++)
                parameters.Add(Pop());

            var @params = parameters.ToArray();

            return new MachinaValueCall(returntype, name, @params);
        }

        public void CallFunction()
        {
            var function = Pop();

            if (function is MachinaFunction func)
            {
                if (_stack.Count < func.Function.Prototype.Parameters.Count)
                    throw new Exception("wrong parameter number");

                Load(
                    Call(
                        func.Function.Prototype.ReturnType,
                        func.Function.Prototype.Name,
                        func.Function.Prototype.Parameters.Count)
                    );
            }
            else
                throw new Exception("called uncallable item");
        }

        public void LoadFunction(MachinaFunction function)
        {
            Load(function);
        }

        public void LoadConstInt32(int value)
        {
            Load(new MachinaValueInt(new MachinaTypeInt32(), (uint)value));
        }

        public void LoadBool(bool value)
        {
            Load(new MachinaValueBool(value));
        }

        public void LoadAddressFromMemory(string name)
        {
            Load(new MachinaValuePointer(_memory[new CIdentifier(name)]));
        }

        public void NotBool()
        {
            var boolean = Pop();

            boolean.Type.ExpectBoolType();

            if (boolean.IsConst)
                boolean = new MachinaValueBool(!Convert.ToBoolean(Convert.ToInt32(((MachinaValueBool)boolean).Value)));
            else
                boolean = new MachinaValueNot(boolean);

            Load(boolean);
        }

        public void LoadConstInt8(short value)
        {
            Load(new MachinaValueInt(new MachinaTypeInt8(), (ushort)value));
        }

        public void LoadConstInt(long value)
        {
            Load(new MachinaValueInt(new MachinaTypeIntPlatform(), (ulong)value));
        }

        public void LoadConstInt64(long value)
        {
            Load(new MachinaValueInt(new MachinaTypeInt64(), (ulong)value));
        }

        public void Load(IMachinaValue value)
        {
            _stack.Push(value);
        }

        private IMachinaValue Pop()
        {
            return _stack.Pop();
        }

        private IMachinaType PeekType()
        {
            return _stack.Peek().Type;
        }

        public void PopTop()
        {
            var value = Pop();
            if (value is MachinaValueCall call)
                Function.Body.CallFunction(call.Identifier, call.Parameters);
        }

        public void DeclareMemory(IMachinaType type, string name, bool declare = true)
        {
            var allocation = new MachinaValueMemory(type, name);

            if (declare)
                Function.Body.DeclareVariable(allocation.Type, name);
            _memory.Add(new CIdentifier(name), allocation);
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
            var type = PeekType();
            type.ExpectType(_memory[new CIdentifier(name)].Type);

            Function.Body.AssignVariable(name, Pop());
        }

        public void LoadFromMemory(string name)
        {
            Load(GetMemory(name));
        }

        public void ReturnVoid()
        {
            Function.Prototype.ReturnType.ExpectVoidType();

            Function.Body.Return(new MachinaValueVoid());
        }

        public void Return()
        {
            var value = Pop();

            value.Type.ExpectType(Function.Prototype.ReturnType);

            Function.Body.Return(value);
        }

        public string GetCValue()
        {
            throw new NotImplementedException();
        }
    }
}
