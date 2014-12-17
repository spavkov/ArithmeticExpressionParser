namespace ArithmeticExpressionParser
{

    public abstract class Token
    {
    }

    public class OperatorToken : Token
    {

    }
    public class PlusToken : OperatorToken
    {
    }

    public class MinusToken : OperatorToken
    {
    }

    public class MultiplyToken : OperatorToken
    {
    }

    public class DivideToken : OperatorToken
    {
    }

    public class ParenthesisToken : Token
    {
        
    }

    public class OpenParenthesisToken : ParenthesisToken
    {
    }

    public class ClosedParenthesisToken : ParenthesisToken
    {
    }


    public class NumberConstantToken : Token
    {
        private readonly double _value;

        public NumberConstantToken(double value)
        {
            _value = value;
        }

        public double Value
        {
            get { return _value; }
        }
    }
}