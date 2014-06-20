using System.Threading;
using System.Threading.Tasks;
using NLog;
using Rose.NowInstaller.Core.Instructions;
using Rose.NowInstaller.Core.Instructions.InstructionsLibrary;

namespace Rose.NowInstaller.Core.Installation
{
    public class InstallScript
    {

        public void ExecuteInstruction(Instruction instruction, bool async = true)
        {
            
        }

        public virtual InstructionsList CreateInstructions(IInstructionTriggers triggers)
        {
            var list = new InstructionsList();

            list.AddAfterLast(new DelegateInstruction( async context =>
                                                      {
                                                          LogManager.GetCurrentClassLogger().Info("Instruction1");
                                                          await Task.Delay(50000);
                                                      }));
            list.AddAfterLast(new DelegateInstruction(context => LogManager.GetCurrentClassLogger().Info("Instruction2")));
            list.AddAfterLast(new DelegateInstruction(context => LogManager.GetCurrentClassLogger().Info("Instruction3")));

            return list;
        }
    }
}
