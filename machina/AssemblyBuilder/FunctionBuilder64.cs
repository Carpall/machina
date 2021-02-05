using Machina.Emitter;

namespace Machina.AssemblyBuilder
{
    public class FunctionBuilder64 : AssemblyBuilder
    {
        readonly Emitter.Emitter _emitter = new();
        readonly MachinaType[] _parameters;
        readonly MachinaType _returnType;

        int _allocationSize = 0;
        
        public FunctionBuilder64(string name, MachinaType[] parameters, MachinaType returnType)
        {
            _parameters = parameters;
            _returnType = returnType;
            _emitter.EmitFunctionLabel32(name, parameters.Length);
        }
        public InstructionBuilder8086 GetInstructions() => _emitter.Assemble();

        public string DumpAssembly() => _emitter.DumpAssembly(false);
        void LoadConstant(object value)
        {
            _emitter.Load(Value.Constant(value));
        }
        public void LoadInt(int value)
        {
            LoadConstant(value);
        }
        public void AddInt()
        {
            _emitter.EmitAddInt32();
        }
        public void SubInt()
        {
            _emitter.EmitSubInt32();
        }
        public void MulInt()
        {
            _emitter.EmitMulInt32();
        }
        public void DivInt()
        {
            _emitter.EmitDivInt32();
        }

        bool SavedFrame()
        {
            return _allocationSize != 0;
        }
        public void Ret()
        {
            switch (_returnType)
            {
                case MachinaType.Int:
                    _emitter.EmitRetInt32(SavedFrame());
                    break;
                case MachinaType.Void:
                    _emitter.EmitRetVoid(SavedFrame());
                    break;
            }
        }
    }
}