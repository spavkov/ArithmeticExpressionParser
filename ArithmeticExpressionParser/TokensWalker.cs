using System;
using System.Collections.Generic;
using System.Linq;

namespace ArithmeticExpressionParser
{
    public class TokensWalker
    {
        private readonly List<Token> _tokens = new List<Token>();
        private int _currentIndex = -1;

        public bool ThereAreMoreTokens
        {
            get { return _currentIndex < _tokens.Count - 1; }
        }

        public TokensWalker(IEnumerable<Token> tokens)
        {
            _tokens = tokens.ToList();
        }

        public Token GetNext()
        {
            MakeSureWeDontGoPastTheEnd();
            return _tokens[++_currentIndex];
        }

        private void MakeSureWeDontGoPastTheEnd()
        {
            if (!ThereAreMoreTokens)
                throw new Exception("Cannot read pass the end of tokens list");
        }

        public Token PeekNext()
        {
            MakeSureWeDontPeekPastTheEnd();
            return _tokens[_currentIndex + 1];
        }

        private void MakeSureWeDontPeekPastTheEnd()
        {
            var weCanPeek = (_currentIndex + 1 < _tokens.Count);
            if (!weCanPeek)
                throw new Exception("Cannot peek pass the end of tokens list");
        }

        public bool IsNextOfType(Type type)
        {
            return PeekNext().GetType() == type;
        }
    }
}