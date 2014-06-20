using System;
using System.IO;
using System.Reflection;
using Rose.NowInstaller.Core.Common;
using Rose.NowInstaller.Core.TorrentIntegration;

namespace Rose.NowInstaller.Core.Instructions
{
    public class InstructionExecutionContext : IInstructionExecutionContext
    {
        private AppDomain dataDomain;


        public InstructionExecutionContext(InstallTorrent torrent)
        {
            Torrent = torrent;
        }

        private AppDomain DataDomain
        {
            get { return dataDomain ?? (dataDomain = AppDomain.CreateDomain("DataDomain")); }
        }

        public InstallTorrent Torrent { get; private set; }

        public Stream GetFile(string fileName)
        {
            return Torrent.DataArchive.GetEntry(fileName).Open();
        }  

        public Assembly LoadDllAssembly(string dllPath)
        {
            var assemblyDataStream = GetFile(dllPath);
            var domain = DataDomain;

            return domain.Load(assemblyDataStream.ToByteArray(), GetFile("data2.pdb").ToByteArray());
        }

        public void ExecuteInstruction(Instruction instruction)
        {
            throw new NotImplementedException();
        }
    }
}