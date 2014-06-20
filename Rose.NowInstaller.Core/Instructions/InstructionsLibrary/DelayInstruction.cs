using System.Threading.Tasks;

namespace Rose.NowInstaller.Core.Instructions.InstructionsLibrary
{
    [Instruction(InstructionsMapping.Delay)]
    public class DelayInstruction : AsyncInstruction
    {
        protected DelayInstruction(int id, InstructionData data) : base(id, data)
        {
        }

        public DelayInstruction(InstructionData data)
            : base(InstructionsMapping.Delay, data)
        {
        }


        public override async Task ExecuteAsync(IInstructionExecutionContext context)
        {
            await Task.Delay(5000);
        }
    }
}
