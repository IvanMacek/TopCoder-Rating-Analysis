using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TopCoder.DataDownload.Entity;

namespace TopCoder.DataDownload
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var fileManager = new FileManager(@"J:\Data\TopCoder");
            var downloader = new Downloader();
            var reader = new Parser();

            //_DownloadData(reader, fileManager, downloader);

            //var coders = reader.ParseCoders(fileManager.ReadCoders());
            //Console.WriteLine("Read {0} coders.", coders.Count);
            //var sb = new StringBuilder();
            //foreach (var coder in coders)
            //{
            //    sb.AppendFormat("{0},", coder.Id);
            //    sb.AppendFormat("{0},", coder.Handle);
            //    sb.AppendFormat("{0},", coder.Country);
            //    sb.AppendFormat("{0},", coder.Rating);
            //    sb.AppendFormat("{0},", coder.Volatility);
            //    sb.AppendFormat("{0}", coder.EventsCount);
            //    sb.AppendLine();
            //}
            //fileManager.SaveCodersToCsv(sb.ToString());
            //Console.WriteLine("All coders CSV is saved.");

            //var rounds = reader.ParseRounds(fileManager.ReadRounds());
            //Console.WriteLine("Read {0} rounds.", rounds.Count);
            //var sb = new StringBuilder();
            //foreach (var round in rounds)
            //{
            //    sb.AppendFormat("{0},", round.Id);
            //    sb.AppendFormat("{0},", round.FullName);
            //    sb.AppendFormat("{0},", round.ShortName);
            //    sb.AppendFormat("{0},", round.Type);
            //    sb.AppendFormat("{0}", round.DateTime.ToString("yyyy-MM-dd hh:mm"));
            //    sb.AppendLine();
            //}
            //fileManager.SaveRoundsToCsv(sb.ToString());
            //Console.WriteLine("All rounds CSV is saved.");

            var rounds = reader.ParseRounds(fileManager.ReadRounds());
            Console.WriteLine("Read {0} rounds.", rounds.Count);
            fileManager.AppendCoderHistoryToCsv("RoundId,RoundName,RoundDate,CoderId,CoderHandle,Division,DivisionPlace,Points,OldRating,NewRating,NewVolatility,NumberOfRatings,IsRated" + Environment.NewLine);
            foreach (var round in rounds)
            {
                var coderRoundHistories =
                    (fileManager.ExistsRoundHistory(round.Id))
                        ? reader.ParseCoderRoundHistory(round, fileManager.ReadRoundHistory(round.Id))
                        : Enumerable.Empty<CoderRoundHistory>();

                var sb = new StringBuilder();
                foreach (var x in coderRoundHistories)
                {
                    sb.AppendFormat("{0},", x.RoundId);
                    sb.AppendFormat(@"""{0}"",", x.RoundName);
                    sb.AppendFormat(@"{0},", x.RoundDateTime.ToString("yyyy-MM-dd"));
                    sb.AppendFormat("{0},", x.CoderId);
                    sb.AppendFormat(@"""{0}"",", x.CoderHandle);
                    sb.AppendFormat("{0},", x.Division);
                    sb.AppendFormat("{0},", x.DivisionPlace);
                    sb.AppendFormat("{0},", x.Points.ToString(CultureInfo.InvariantCulture.NumberFormat));
                    sb.AppendFormat("{0},", x.OldRating);
                    sb.AppendFormat("{0},", x.NewRating);
                    sb.AppendFormat("{0},", x.NewVolatility);
                    sb.AppendFormat("{0},", x.NumberOfRatings);
                    sb.AppendFormat("{0}", x.IsRated);
                    sb.AppendLine();
                }

                fileManager.AppendCoderHistoryToCsv(sb.ToString());
            }
            Console.WriteLine("All coders history CSV is saved.");

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void _DownloadData(Parser reader, FileManager fileManager, Downloader downloader)
        {
            //fileManager.CreateDirectories();
            //Console.WriteLine("Directories created.");

            //fileManager.SaveCoders(downloader.DownloadCoders());
            //Console.WriteLine("Coders list downloaded and saved.");

            //fileManager.SaveRounds(downloader.DownloadRounds());
            //Console.WriteLine("Matchs list downloaded and saved.");

            //var coders = reader.ParseCoders(fileManager.ReadCoders());
            //Console.WriteLine("Read {0} coders.", coders.Count);
            //Parallel.ForEach(coders, new ParallelOptions { MaxDegreeOfParallelism = 50 },
            //                 coder => fileManager.SaveCoderHistory(coder.Id, downloader.DownloadCoderHistory(coder.Id)));
            //Console.WriteLine("Coder history is downloaded and saved.");

            var i = 1;
            var rounds = reader.ParseRounds(fileManager.ReadRounds());
            Console.WriteLine("Read {0} rounds.", rounds.Count);
            Parallel.ForEach(rounds, new ParallelOptions { MaxDegreeOfParallelism = 3 }, round =>
            {
                if (!fileManager.ExistsRoundHistory(round.Id))
                {
                    try
                    {
                        fileManager.SaveRoundHistory(round.Id, downloader.DownloadRoundHistory(round.Id));
                    }
                    catch
                    {
                        Console.WriteLine("{0}: {1} - {2}", i++, round.Id, round.ShortName);
                    }
                }
            });
            Console.WriteLine("Round history is downloaded and saved.");
        }
    }
}
