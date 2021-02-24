using Machina.CModels;
using Machina.TypeSystem;
using Machina.ValueSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machina.CGenerator
{
    class TextGenerator
    {
        const string IncludeDirective = "#include";
        const string StructKeyword = "struct";

        const string ReturnKeyword = "return";

        const int BodyIndent = 2;

        const int CodeCapacity = 5000;

        private readonly StringBuilder _code = new("", CodeCapacity);

        public void WriteReturn()
        {
            Write(ReturnKeyword);
            Space();
        }

        public void WriteValue(IMachinaValue value)
        {
            Write(value.GetCValue());
        }

        public void WriteIdentifier(CIdentifier identifier)
        {
            Write(identifier.Name);
        }

        public void WriteType(IMachinaType type)
        {
            Write(type.GetCType());
            Space();
        }

        public void WriteEqual()
        {
            Write(" = ");
        }

        private void WriteLine(string line)
        {
            _code.AppendLine(line);
        }

        public void WriteFunctionPrototype(IMachinaType returnType, CIdentifier identifier, List<CVariableInfo> parameters, bool hasBody)
        {
            Write(returnType.GetCType());
            Space();
            Write(identifier.Name);
            WriteMatchedParenthesis('(', string.Join(", ", parameters));
            if (!hasBody)
            {
                WriteSemicolon();
                NewLine();
            }
        }

        public void WriteSemicolon()
        {
            Write(";");
        }

        private void WriteString(string text)
        {
            Write($"\"{text}\"");
        }

        private void WriteMatchedParenthesis(char par, string text)
        {
            Write($"{par}{text}{MatchOppositeParenthesis(par)}");
        }

        private void Write(string text)
        {
            _code.Append(text);
        }

        private void Space()
        {
            Write(" ");
        }

        public void NewLine()
        {
            WriteLine("");
        }

        public void Indent()
        {
            Write($"{new string(' ', BodyIndent)}");
        }

        public void WriteStructPrototype(string name, bool hasBody)
        {
            Write(StructKeyword);
            Space();
            Write(name);
            if (!hasBody)
                WriteSemicolon();
        }

        private char MatchOppositeParenthesis(char par)
        {
            return par switch
            {
                '(' => ')',
                '[' => ']',
                '{' => '}',
                '<' => '>',
                ')' => '(',
                ']' => '[',
                '}' => '{',
                '>' => '<',
                _ => throw new ArgumentException("'par' was not a parenthesis")
            };
        }

        public void LinkTextgenerator(TextGenerator generator)
        {
            NewLine();
            Write(generator.GetCode());
        }

        public void WriteBlock(string block)
        {
            WriteMatchedParenthesis('{', block);
        }

        public void WriteInclude(CInclude include)
        {
            Write(IncludeDirective);
            Space();
            if (include.IsLocal)
                WriteString(include.Path);
            else
                WriteMatchedParenthesis('<', include.Path);
        }

        public string GetCode()
        {
            return _code.ToString();
        }
    }
}
