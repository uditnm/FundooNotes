using System;

namespace FunDoNotesApplication
{
    internal class FundoException: Exception
    {
        public enum ExceptionType
        {
            INVALID_INPUT
        }

        ExceptionType type;

        public FundoException(ExceptionType type)
        {
            this.type = type;
        }

        public override string Message
        {
            get
            {
                return $"There was an Exception: {type} exception";
            }
        }
    }
}
