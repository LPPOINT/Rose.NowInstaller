using System.Diagnostics;

namespace Rose.NowInstaller.Core.Instructions.InstructionsLibrary
{
    [Instruction(InstructionsMapping.RunProcess)]
    public class RunProcessInstruction : Instruction
    {


        [InstructionProperty("ProcessName")]
        public string ProcessName { get; set; }


        [InstructionProperty("ProcessArgs")]
        public string ProcessArgs { get; set; }

        protected RunProcessInstruction(int id, InstructionData data) : base(id, data)
        {
            
        }

        public RunProcessInstruction(InstructionData data)
            : base(InstructionsMapping.RunProcess, data)
        {
        }

        public override void Execute(IInstructionExecutionContext context)
        {
            if (!string.IsNullOrWhiteSpace(ProcessName))
            {
                Process.Start(ProcessName, ProcessArgs);
            }
            base.Execute(context);
        }
    }
}
