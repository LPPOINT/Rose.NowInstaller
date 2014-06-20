using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rose.NowInstaller.Core.Common;

namespace Rose.NowInstaller.Core.Instructions
{
    public class InstructionsExecutor
    {


        public InstructionsExecutor()
        {
            Instructions = new InstructionsList();
        }

        public InstructionsExecutor(InstructionsList list)
        {
            Instructions = list;
        }

        public event EventHandler<ObjectEventArgs<Instruction>> InstructionExecuted;
        protected virtual void OnInstructionExecuted(Instruction i)
        {
            var handler = InstructionExecuted;
            if (handler != null) handler(this, new ObjectEventArgs<Instruction>(i));
        }

        public InstructionsList Instructions { get; private set; }


        public async Task ExecuteInstruction(IInstructionExecutionContext context, Instruction instruction)
        {
            if(!instruction.IsAsync)
                await Task.Run(() => instruction.Execute(context));
            else
            {
                var asAsync = instruction as AsyncInstruction;
                await asAsync.ExecuteAsync(context);
            }

            await InvokeTrigger(context, instruction.AfterExecution);

        }

        public async Task InvokeTrigger(IInstructionExecutionContext context, InstructionTrigger trigger)
        {
            var targets = Instructions.Where(triggerable => Equals(triggerable.Trigger, trigger));


            foreach (var triggerable in targets)
            {
                var t = triggerable;
                await Task.Run(() => ExecuteInstruction(context, t));
            }

        }

    }
}
