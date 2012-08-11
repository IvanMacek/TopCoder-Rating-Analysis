using System.IO;

namespace TopCoder.DataDownload
{
    public partial class FileManager
    {
        public void CreateDirectories()
        {
            if (!Directory.Exists(_rootDirectory)) { Directory.CreateDirectory(_rootDirectory); }

            if (!Directory.Exists(_codersHistoryDirectory)) { Directory.CreateDirectory(_codersHistoryDirectory); }

            if (!Directory.Exists(_roundsHistoryDirectory)) { Directory.CreateDirectory(_roundsHistoryDirectory); }
        }

        public void SaveCoders(string data)
        {
            File.WriteAllText(_codersFile, data);
        }
        public void SaveCodersToCsv(string data)
        {
            File.WriteAllText(_codersCsvFile, data);
        }
        public void SaveRounds(string data)
        {
            File.WriteAllText(_roundsFile, data);
        }
        public void SaveRoundsToCsv(string data)
        {
            File.WriteAllText(_roundsCsvFile, data);
        }
        public void SaveCoderHistory(int coderId, string data)
        {
            File.WriteAllText(string.Format(_coderHistoryFile, coderId), data);
        }
        public void SaveRoundHistory(int roundId, string data)
        {
            File.WriteAllText(string.Format(_roundHistoryFile, roundId), data);
        }

        public void AppendCoderHistoryToCsv(string data)
        {
            File.AppendAllText(_codersHistoryCsv, data);
        }
        
        public string ReadCoders()
        {
            return File.ReadAllText(_codersFile);
        }
        public string ReadRounds()
        {
            return File.ReadAllText(_roundsFile);
        }
        public string ReadCoderHistory(int coderId)
        {
            return File.ReadAllText(string.Format(_coderHistoryFile, coderId));
        }
        public string ReadRoundHistory(int roundId)
        {
            return File.ReadAllText(string.Format(_roundHistoryFile, roundId));
        }

        public bool ExistsRoundHistory(int roundId)
        {
            return File.Exists(string.Format(_roundHistoryFile, roundId));
        }
    }

    public partial class FileManager
    {
        private readonly string _rootDirectory;
        private readonly string _codersHistoryDirectory;
        private readonly string _roundsHistoryDirectory;

        private readonly string _codersFile;
        private readonly string _codersCsvFile;
        private readonly string _coderHistoryFile;
        private readonly string _codersHistoryCsv;
        private readonly string _roundsFile;
        private readonly string _roundsCsvFile;
        private readonly string _roundHistoryFile;

        public FileManager(string rootDirectory)
        {
            _rootDirectory = rootDirectory;

            _codersHistoryDirectory = Path.Combine(_rootDirectory, "coders-history");
            _roundsHistoryDirectory = Path.Combine(_rootDirectory, "rounds-history");

            _codersFile = Path.Combine(_rootDirectory, "coders.xml");
            _codersCsvFile = Path.Combine(_rootDirectory, "coders.csv");
            _coderHistoryFile = Path.Combine(_codersHistoryDirectory, "coder-{0}.xml");
            _codersHistoryCsv = Path.Combine(_rootDirectory, "coders-history.csv");
            _roundsFile = Path.Combine(_rootDirectory, "rounds.xml");
            _roundsCsvFile = Path.Combine(_rootDirectory, "rounds.csv");
            _roundHistoryFile = Path.Combine(_roundsHistoryDirectory, "round-{0}.xml");
        }
    }
}