using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Rose.NowInstaller.Core.Instructions.InstructionsLibrary;

namespace Rose.NowInstaller.Core.Instructions
{
    public static class InstructionsFactory
    {

        public static CustomInstruction Custom([NotNull] string dll, [NotNull] string className,
            [NotNull] string staticMethod)
        {
            if (dll == null) throw new ArgumentNullException("dll");
            if (className == null) throw new ArgumentNullException("className");
            if (staticMethod == null) throw new ArgumentNullException("staticMethod");

            var dictionary = new Dictionary<string, InstructionData>();

            dictionary.Add("Dll", InstructionData.String(dll));
            dictionary.Add("Class", InstructionData.String(className));
            dictionary.Add("Function", InstructionData.String(staticMethod));

            var data = InstructionData.Dictionary(dictionary);

            return new CustomInstruction(data);

        }

        public static NumericInstruction Numeric(long value)
        {
            return new NumericInstruction(InstructionData.Number(value));
        }

        public static StartDownloadInstruction StartDownload()
        {
            return new StartDownloadInstruction(InstructionData.Empty);
        }
        public static StartDownloadInstruction PauseDownload()
        {
            return new StartDownloadInstruction(InstructionData.Empty);
        }

        public static ChangeInstallStatusInstruction ChangeInstallStatus(string newStatus)
        {
            var newStatusData = InstructionData.String(newStatus);
            return new ChangeInstallStatusInstruction(newStatusData);
        }

        public static RunProcessInstruction RunProcess(string processName, string processArgs)
        {
            var d = new Dictionary<string, InstructionData>();
            d.Add("ProcessName", InstructionData.String(processName));
            d.Add("ProcessArgs", InstructionData.String(processArgs));

            return new RunProcessInstruction(InstructionData.Dictionary(d));

        }

        public static RunProcessInstruction RunProcess(string processName)
        {
            return RunProcess(processName, string.Empty);
        }

        public static DelayInstruction Delay()
        {
            return new DelayInstruction(InstructionData.Empty);
        }
    }
}
