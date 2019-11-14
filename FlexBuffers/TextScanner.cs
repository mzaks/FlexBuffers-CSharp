using System;
using System.IO;

namespace FlexBuffers
{
    internal struct TextPosition
    {
        public long column;
        public long line;
    }
	
    internal sealed class TextScanner
    {
        private readonly TextReader _reader;
        private TextPosition _position;

        public TextPosition Position => _position;

        public bool CanRead => (_reader.Peek() != -1);

        internal TextScanner(TextReader reader)
        {
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        internal char Peek()
        {
            var next = _reader.Peek();

            if (next == -1)
            {
                throw new Exception($"Incomplete message {_position}");
            }

            return (char)next;
        }

        internal char Read()
        {
            var next = _reader.Read();

            if (next == -1)
            {
                throw new Exception($"Incomplete message {_position}");
            }

            switch (next)
            {
                case '\r':
                    // Normalize '\r\n' line encoding to '\n'.
                    if (_reader.Peek() == '\n')
                    {
                        _reader.Read();
                    }
                    goto case '\n';

                case '\n':
                    _position.line += 1;
                    _position.column = 0;
                    return '\n';

                default:
                    _position.column += 1;
                    return (char)next;
            }
        }

        internal void SkipWhitespace()
        {
            while (char.IsWhiteSpace(Peek()))
            {
                Read();
            }
        }

        internal void Assert(char next)
        {
            if (Peek() == next)
            {
                Read();
            }
            else
            {
                throw new Exception($"Parser expected {next} at position {_position}");
            }
        }

        public void Assert(string next)
        {
            for (var i = 0; i < next.Length; i += 1)
            {
                Assert(next[i]);
            }
        }
    }
}