using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ArithmeticExpressionParser
{
    // Expression := [ "-" ] Term { ("+" | "-") Term }
    // Term       := Factor> { ( "*" | "/" ) Factor }
    // Factor     := RealNumber | "(" Expression ")"
    // RealNumber := Digit{Digit} | [Digit] "." {Digit}
    // Digit      := "0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9" 

    public class Tokenizer
    {
        private StringReader _reader;

        public IEnumerable<Token> Scan(string expression)
        {
            _reader = new StringReader(expression);

            var tokens = new List<Token>();
            while (_reader.Peek() != -1)
            {
                var c = (char)_reader.Peek();
                if (Char.IsWhiteSpace(c))
                {
                    _reader.Read();
                    continue;
                }

                if (Char.IsDigit(c) || c == '.')
                {
                    var nr = ParseNumber();
                    tokens.Add(new NumberConstantToken(nr));
                }
                else if (c == '-')
                {
                    tokens.Add(new MinusToken());
                    _reader.Read();
                }
                else if (c == '+')
                {
                    tokens.Add(new PlusToken());
                    _reader.Read();
                }
                else if (c == '*')
                {
                    tokens.Add(new MultiplyToken());
                    _reader.Read();
                }
                else if (c == '/')
                {
                    tokens.Add(new DivideToken());
                    _reader.Read();
                }
                else if (c == '(')
                {
                    tokens.Add(new OpenParenthesisToken());
                    _reader.Read();
                }
                else if (c == ')')
                {
                    tokens.Add(new ClosedParenthesisToken());
                    _reader.Read();
                }
                else
                    throw new Exception("Unknown character in expression: " + c);
            }

            return tokens;
        }

        private double ParseNumber()
        {
            var sb = new StringBuilder();
            var decimalExists = false;
            while (Char.IsDigit((char)_reader.Peek()) || ((char) _reader.Peek() == '.'))
            {
                var digit = (char)_reader.Read();
                if (digit == '.')
                {
                    if (decimalExists) throw new Exception("Multiple dots in decimal number");
                    decimalExists = true;
                }
                sb.Append(digit);
            }

            double res;
            if (!double.TryParse(sb.ToString(), out res))
                throw new Exception("Could not parse number: " + sb);

           return res;
        }
    }
}
