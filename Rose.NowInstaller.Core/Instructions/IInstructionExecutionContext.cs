using System.IO;
using System.Reflection;

namespace Rose.NowInstaller.Core.Instructions
{
    public interface IInstructionExecutionContext
    {

        Stream GetFile(string fileName);

        Assembly LoadDllAssembly(string dllPath);
        void ExecuteInstruction(Instruction instruction);
    }
}
