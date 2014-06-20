using System;

namespace Rose.NowInstaller.Core.Instructions.InstructionsLibrary
{
    [Instruction(InstructionsMapping.NumericInstructionId)]
    public class NumericInstruction : Instruction
    {
        private NumericInstruction(int id, InstructionData data) : base(id, data)
        {

        }

        public NumericInstruction(InstructionData data)
            : this(InstructionsMapping.NumericInstructionId, data)
        {
            
        }

        [InstructionProperty()]
        public long Value { get; set; }

        public override void Execute(IInstructionExecutionContext context)
        {
            base.Execute(context);
        }
    }
}