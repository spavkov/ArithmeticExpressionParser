using System;

namespace ArithmeticExpressionParser
{
    public class Parser
    {
        private readonly string _expression;
        private readonly TokensWalker _walker;

        public Parser(string expression)
        {
            _expression = expression;
            var tokens = new Tokenizer().Scan(_expression);
            _walker = new TokensWalker(tokens);
        }

        // EBNF Grammar:
        // Expression := [ "-" ] Term { ("+" | "-") Term }
        // Term       := Factor { ( "*" | "/" ) Factor }
        // Factor     := RealNumber | "(" Expression ")"
        // RealNumber := Digit{Digit} | [Digit] "." {Digit}
        // Digit      := "0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9" 

        // Expression := [ "-" ] Term { ("+" | "-") Term }
        public double Parse()
        {
            var isNegative = NextIsMinus();
            if (isNegative)
                GetNext();
            var valueOfExpression = TermValue();
            if (isNegative)
                valueOfExpression = -valueOfExpression;
            while (NextIsMinusOrPlus())
            {
                var op = GetTermOperand();
                var nextTermValue = TermValue();
                if (op is PlusToken)
                    valueOfExpression += nextTermValue;
                else
                    valueOfExpression -= nextTermValue;
            }
            return valueOfExpression;
        }

        // Term       := Factor { ( "*" | "/" ) Factor }
        private double TermValue()
        {
            var totalVal = FactorValue();
            while (NextIsMultiplicationOrDivision())
            {
                var op = GetFactorOperand();
                var nextFactor = FactorValue();

                if (op is DivideToken)
                    totalVal /= nextFactor;
                else
                    totalVal *= nextFactor;
            }

            return totalVal;
        }

        // Factor     := RealNumber | "(" Expression ")"
        private double FactorValue()
        {
            if (NextIsDigit())
            {
                var nr = GetNumber();
                return nr;
            }
            if (!NextIsOpeningBracket())
                throw new Exception("Expecting Real number or '(' in expression, instead got : " + (PeekNext() != null ? PeekNext().ToString()  : "End of expression"));          
            GetNext();

            var val = Parse();
            
            if (!(NextIs(typeof(ClosedParenthesisToken))))
                throw new Exception("Expecting ')' in expression, instead got: " + (PeekNext() != null ? PeekNext().ToString() : "End of expression"));           
            GetNext();
            return val;
        }

        private bool NextIsMinus()
        {
            return _walker.ThereAreMoreTokens && _walker.IsNextOfType(typeof(MinusToken));
        }

        private bool NextIsOpeningBracket()
        {
            return NextIs(typeof(OpenParenthesisToken));
        }

        private Token GetTermOperand()
        {
            var c = GetNext();
            if (c is PlusToken)
                return c;
            if (c is MinusToken)
                return c;

            throw new Exception("Expected term operand '+' or '-' but found" + c);
        }

        private Token GetFactorOperand()
        {
            var c = GetNext();
            if (c is DivideToken)
                return c;
            if (c is MultiplyToken)
                return c;

            throw new Exception("Expected factor operand '/' or '*' but found" + c);
        }

        private Token GetNext()
        {
            return _walker.GetNext();
        }

        private Token PeekNext()
        {
            return _walker.ThereAreMoreTokens ? _walker.PeekNext() : null;
        }

        private double GetNumber()
        {
            var next = _walker.GetNext();

            var nr = next as NumberConstantToken;
            if (nr == null)
                throw new Exception("Expecting Real number but got " + next);

            return nr.Value;
        }

        private bool NextIsDigit()
        {
            if (!_walker.ThereAreMoreTokens)
                return false;
            return _walker.PeekNext() is NumberConstantToken;
        }

        private bool NextIs(Type type)
        {
            return _walker.ThereAreMoreTokens && _walker.IsNextOfType(type);
        }

        private bool NextIsMinusOrPlus()
        {
            return _walker.ThereAreMoreTokens && (NextIs(typeof(MinusToken)) || NextIs(typeof(PlusToken)));
        }

        private bool NextIsMultiplicationOrDivision()
        {
            return _walker.ThereAreMoreTokens && (NextIs(typeof(MultiplyToken)) || NextIs(typeof(DivideToken)));
        }
    }
}