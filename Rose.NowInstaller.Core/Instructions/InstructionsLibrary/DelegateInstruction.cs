using System.Threading.Tasks;
using NLog;

namespace Rose.NowInstaller.Core.Instructions.InstructionsLibrary
{
    [Instruction(InstructionsMapping.Delegate)]
    public class DelegateInstruction : AsyncInstruction
    {

        public delegate void DelegateInstructionDelegate(IInstructionExecutionContext context);

        protected DelegateInstruction(int id, InstructionData data) : base(id, data)
        {
        }

        public DelegateInstruction(InstructionData data)
            : base(InstructionsMapping.Delegate, data)
        {
        }


        public DelegateInstructionDelegate Delegate { get; private set; }

        public DelegateInstruction(DelegateInstructionDelegate del) : base(InstructionData.Empty)
        {
            Delegate = del;
        }


        public override async Task ExecuteAsync(IInstructionExecutionContext context)
        {
            LogManager.GetCurrentClassLogger().Info("sada");
            //if (Delegate != null)
            //    await Task.Run(() => Delegate(context));
            await Task.Delay(50000);

        }
    }
}
