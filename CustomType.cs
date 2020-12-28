using System;
using System.Collections.Generic;
using System.Text;

namespace Machina
{
    public struct CustomType
    {
        string Name;
        Method[] Methods;
        Method Constructor;
        Data[] Variables;
        public CustomType(string name, Method[] methods, Method constructor, Data[] variables)
        {
            Name = name;
            Methods = methods;
            Constructor = constructor;
            Variables = variables;
        }
    }
}
