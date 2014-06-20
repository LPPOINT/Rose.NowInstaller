using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Rose.NowInstaller.Core.Instructions
{
    public class InstructionDecodingException : Exception
    {
        public InstructionDecodingException()
        {
        }

        public InstructionDecodingException(string message) : base(message)
        {
        }

        public InstructionDecodingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InstructionDecodingException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
