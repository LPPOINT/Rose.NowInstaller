using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Rose.NowInstaller.Core.Instructions
{
    public class InstructionExecutionException : Exception
    {
        public InstructionExecutionException()
        {
        }

        public InstructionExecutionException(string message) : base(message)
        {
        }

        public InstructionExecutionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InstructionExecutionException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
