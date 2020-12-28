using System;
using System.Collections.Generic;
using System.Text;

namespace Machina
{
    public struct ValueType
    {
        public ValueTypes Kind;
        public string CustomType;
        public ValueType(ValueTypes type, string custom)
        {
            Kind = type;
            CustomType = custom;
        }
    }
    public enum ValueTypes
    {
        String,
        Char,
        Int8,
        Int16,
        Int32,
        Int64,
        UInt8,
        UInt16,
        UInt32,
        UInt64,
        Unknow,
        PointerToType,
        CustomType
    }
}