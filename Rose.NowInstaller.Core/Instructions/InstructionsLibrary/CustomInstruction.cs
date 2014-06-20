using System;
using System.Linq;

namespace Rose.NowInstaller.Core.Instructions.InstructionsLibrary
{

    /// <summary>
    /// Выполняет произвольный код заданного статического метода, объявленного в библиотеке
    /// </summary>
    [Instruction(InstructionsMapping.CustomInstruction)]
    public class CustomInstruction : Instruction
    {
        public CustomInstruction(InstructionData data) : base(InstructionsMapping.CustomInstruction, data)
        {
        }

        [InstructionProperty("Dll")]
        public string Dll { get; set; }

        [InstructionProperty("Class")]
        public string Class { get; set; }


        [InstructionProperty("Function")]
        public string Function { get; set; }

        public override void Execute(IInstructionExecutionContext context)
        {
            try
            {
                var assembly = context.LoadDllAssembly(Dll);

                var staticClass = assembly.GetType(Class);

                if (staticClass == null)
                    throw new InstructionExecutionException();

                var method = staticClass.GetMethod(Function);
                if (method == null || !method.IsStatic || !method.IsPublic)
                    throw new InstructionExecutionException();

                if (method.GetParameters().Length != 1 ||
                    method.GetParameters().First().ParameterType != typeof (IInstructionExecutionContext))
                    throw new InstructionExecutionException();

                method.Invoke(null, new object[] {context});
            }
            catch (InstructionExecutionException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new InstructionExecutionException("Непредвиденная ошибка во время выполнения инструкции", e);
            }
            finally
            {
                base.Execute(context);
            }

        }
    }
}
