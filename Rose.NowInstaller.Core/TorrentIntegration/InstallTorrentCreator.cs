using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using MonoTorrent;
using MonoTorrent.BEncoding;
using MonoTorrent.Common;
using Rose.NowInstaller.Core.Common;
using Rose.NowInstaller.Core.Instructions;

namespace Rose.NowInstaller.Core.TorrentIntegration
{
    public class InstallTorrentCreator : TorrentCreator
    {

        public InstallTorrentCreator()
        {
            Instructions = new ObservableCollection<Instruction>();
            Instructions.CollectionChanged += InstructionsChanges;
            SetCustom(InstallTorrent.InstructionsSector, new BEncodedList());
        }

        private BEncodedList GetInstructionsList()
        {
            return GetCustom(InstallTorrent.InstructionsSector) as BEncodedList;
        }

        private void InstructionsChanges(object sender, NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    var list = GetInstructionsList();
                    foreach (var item in args.NewItems.Cast<Instruction>())
                    {
                        list.Add(item.ToBEncoded());
                    }
                    break;
                }
                case NotifyCollectionChangedAction.Remove:
                {
                    var list = GetInstructionsList();
                    foreach (var item in args.OldItems.Cast<Instruction>())
                    {
                        list.Remove(item.ToBEncoded());
                    }
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public ObservableCollection<Instruction> Instructions { get; private set; }

        public string PathToArchive { get; set; }

        public bool ShouldDeleteArchiveFile { get; set; }

        public void CreateArchivePath(ZipArchive archive, bool shouldDeleteArchiveFile=true)
        {
            ShouldDeleteArchiveFile = shouldDeleteArchiveFile;
            if (!string.IsNullOrWhiteSpace(PathToArchive))
            {
                File.Delete(PathToArchive);
            }
            var temp = Path.ChangeExtension(Path.GetTempFileName(), ".zip");

            using (var file = File.Create(temp))
            {
                
            }

        }

        public override void Create(ITorrentFileSource fileSource, string savePath)
        {

            if(string.IsNullOrWhiteSpace(PathToArchive))
                throw new Exception();

            Check.SavePath(savePath);
            var info = CreateTorrentData(fileSource);
            var encoded = info.Encode();

            using (var archFile = new FileStream(PathToArchive, FileMode.Open))
            {
                var archData = archFile.ToByteArray();

                using (var saveFile = File.Create(savePath))
                {
                    using (var writer = new BinaryWriter(saveFile))
                    {
                        writer.Write(archData.Length);
                        writer.Write(archData);
                        writer.Write(encoded);

                        writer.Flush();

                        if (ShouldDeleteArchiveFile)
                        {
                            File.Delete(PathToArchive);
                        }

                    }
                }

            }

        }

        public override void Create(ITorrentFileSource fileSource, Stream stream)
        {
            Check.Stream(stream);

            var data = CreateTorrentData(fileSource).Encode();
            stream.Write(data, 0, data.Length);
        }
    }
}
