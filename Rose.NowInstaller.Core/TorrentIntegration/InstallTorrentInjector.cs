using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using MonoTorrent.Common;
using Rose.NowInstaller.Core.Common;
using Rose.NowInstaller.Core.Instructions;

namespace Rose.NowInstaller.Core.TorrentIntegration
{


    public abstract class TorrentSource
    {
        protected TorrentSource(Type sourceType)
        {
            SourceType = sourceType;
        }

        public enum Type
        {
            File,
            Stream
        }

        public Type SourceType { get; private set; }

        public abstract Stream GetTorrentStream();

    }

    public class StreamTorrentSource : TorrentSource
    {

        public Stream TorrentStream { get; private set; }

        public StreamTorrentSource(Stream torrentStream) : base(Type.Stream)
        {
            TorrentStream = torrentStream;
        }

        public override Stream GetTorrentStream()
        {
            return TorrentStream;
        }
    }

    public class FileTorrentSource : TorrentSource
    {

        public string TorrentFilePath{ get; private set; }

        public FileTorrentSource(string torrentFilePath)
            : base(Type.Stream)
        {
            TorrentFilePath = torrentFilePath;
        }

        public override Stream GetTorrentStream()
        {
            return File.OpenRead(TorrentFilePath);
        }
    }

    public class InstallTorrentInjector
    {

        public InstallTorrentInjector()
        {
            Instructions = new List<Instruction>();
        }

        public InstallTorrentInjector(TorrentSource torrentSource)
        {
            TorrentSource = torrentSource;
            Instructions = new List<Instruction>();
        }

        public InstallTorrentInjector(string torrentPath)
        {
            TorrentSource = new FileTorrentSource(torrentPath);
            Instructions = new List<Instruction>();
        }

        public InstallTorrentInjector(Stream torrentStream)
        {
            TorrentSource = new StreamTorrentSource(torrentStream);
            Instructions = new List<Instruction>();
        }

        public TorrentSource TorrentSource { get; set; }

        public List<Instruction> Instructions { get; private set; }
        public string PathToArchive { get; set; }
        public bool ShouldDeleteArchiveFile { get; set; }

        public Stream CreateTorrentStream()
        {
            using (var torrentStream = TorrentSource.GetTorrentStream())
            {
                var torrent = Torrent.Load(torrentStream);


                using (var archFile = new FileStream(PathToArchive, FileMode.Open))
                {
                    var archData = archFile.ToByteArray();

                    using (var resultStream = new MemoryStream())
                    {
                        using (var writer = new BinaryWriter(resultStream))
                        {
                            writer.Write(archData.Length);
                            writer.Write(archData);
                            writer.Write(torrent.ToDictionary().Encode());

                            writer.Flush();

                            if (ShouldDeleteArchiveFile)
                            {
                                File.Delete(PathToArchive);
                            }

                            return resultStream;
                        }
                    }

                }
            }
        }

        public void CreateTorrentFile(string savePath)
        {
            using (var stream = CreateTorrentStream())
            {
                using (var fileStream = File.Create(savePath))
                {
                    stream.CopyTo(fileStream);
                }
            }
        }

        public InstallTorrent CreateTorrent()
        {
            var torrent = Torrent.Load(TorrentSource.GetTorrentStream());
            return new InstallTorrent(torrent, new InstructionsList(Instructions), ZipFile.OpenRead(PathToArchive));
        }
    }
}
