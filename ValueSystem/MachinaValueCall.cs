using Machina.CModels;
using Machina.TypeSystem;
using System;

namespace Machina.ValueSystem
{
    internal struct MachinaValueCall : IMachinaValue
    {
        public IMachinaValue[] Parameters { get; }
        public CIdentifier Identifier { get; }
        public IMachinaType Type { get; }
        public bool IsConst => false;
        public bool CanBePointed => false;

        public MachinaValueCall(IMachinaType returnType, CIdentifier identifier, IMachinaValue[] parameters)
        {
            Type = returnType;
            Identifier = identifier;
            Parameters = parameters;
        }

        public string GetCValue()
        {
            var parameters = "";

            for (int i = 0; i < Parameters.Length; i++)
            {
                if (i > 0)
                    parameters += ", ";

                parameters += Parameters[i].GetCValue();
            }

            return $"(({Type.GetCType()}) {Identifier}({parameters}))";
        }
    }
}
