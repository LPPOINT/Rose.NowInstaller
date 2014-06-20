using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using MonoTorrent;
using MonoTorrent.BEncoding;
using MonoTorrent.Common;
using Rose.NowInstaller.Core.Installation;
using Rose.NowInstaller.Core.Instructions;

namespace Rose.NowInstaller.Core.TorrentIntegration
{

    public class InstallTorrentLoadException : Exception
    {
        public InstallTorrentLoadException()
        {
        }

        public InstallTorrentLoadException(string message) : base(message)
        {
        }

        public InstallTorrentLoadException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InstallTorrentLoadException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class InstallTorrent
    {
        internal static readonly string InstructionsSector = "Instructions";

        public InstallTorrent(Torrent torrent, InstructionsList instructions, ZipArchive dataArchive)
        {
            DataArchive = dataArchive;
            Instructions = instructions;
            Torrent = torrent;
        }

        public Torrent Torrent { get; private set; }

        public InstructionsList Instructions { get; private set; }

        public ZipArchive DataArchive { get; private set; }

        public InstallProgramInfo ProgramInfo { get; private set; }

        public BEncodedDictionary ToDictionary()
        {
            return Torrent.ToDictionary();
        }
        public static InstallTorrent Load(Stream stream)
        {
            return Load(stream, string.Empty);
        }

        public static InstallTorrent Load(Stream stream, string torrentPath)
        {


            using (var reader = new BinaryReader(stream))
            {
                var dataLen = reader.ReadInt32();
                var data = reader.ReadBytes(dataLen);
                var torrentBuffer = reader.ReadBytes((int) stream.Length - data.Length);

                var archive = new ZipArchive(new MemoryStream(data), ZipArchiveMode.Read);

                var torrentData = (BEncodedDictionary)BEncodedValue.Decode(torrentBuffer);
                var torrent = Torrent.Load(torrentData);

                BEncodedValue instructionsSector;
                var getInstructionsResult = torrentData.TryGetValue(new BEncodedString(InstructionsSector), out instructionsSector);

                if (!getInstructionsResult) throw new InstallTorrentLoadException("Не удалось найти сектор инструкций");
                if (instructionsSector.GetType() != typeof(BEncodedList)) throw new InstallTorrentLoadException("Сектор инструкций имеет неверный формат");

                var instructions = new InstructionsList((instructionsSector as BEncodedList).Select(Instruction.FromBEncoded).ToList());

                return new InstallTorrent(torrent, instructions, archive);

            }

            //var bySegments = SegmentedInstallTorrent.Create(stream);
            //var torrentData = (BEncodedDictionary)BEncodedValue.Decode(bySegments.BEncodedData.CreateStream());

            //var archiveData = new ZipArchive(bySegments.BinaryData.CreateStream(), ZipArchiveMode.Read);
            //var torrent = Torrent.Load(torrentData);

            //BEncodedValue instructionsSector;
            //var getInstructionsResult = torrentData.TryGetValue(new BEncodedString(InstructionsSector), out instructionsSector);

            //if(!getInstructionsResult) throw new InstallTorrentLoadException("Не удалось найти сектор инструкций");
            //if (instructionsSector.GetType() != typeof(BEncodedList)) throw new InstallTorrentLoadException("Сектор инструкций имеет неверный формат");

            //var instructions = new InstructionsList((instructionsSector as BEncodedList).Select(Instruction.FromBEncoded).ToList());

            //return new InstallTorrent(torrent, instructions, archiveData);
        }

        public static InstallTorrent Load(string path)
        {

            return Load(File.OpenRead(path));
        }
    }
}
