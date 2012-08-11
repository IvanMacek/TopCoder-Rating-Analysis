using System.IO;
using System.Net;

namespace TopCoder.DataDownload
{
    public class Downloader
    {
        private const string _CodersDataFeed = "http://www.topcoder.com/tc?module=BasicData&c=dd_coder_list";
        private const string _CoderHistoryDataFeed = "http://www.topcoder.com/tc?module=BasicData&c=dd_rating_history&cr={0}";
        private const string _RoundsDataFeed = "http://www.topcoder.com/tc?module=BasicData&c=dd_round_list";
        private const string _RoundHistoryDataFeed = "http://www.topcoder.com/tc?module=BasicData&c=dd_round_results&rd={0}";

        public string DownloadCoders()
        {
            using (var webClient = new WebClient())
            {
                return webClient.DownloadString(_CodersDataFeed);
            }
        }

        public string DownloadCoderHistory(int coderId)
        {
            using (var webClient = new WebClient())
            {
                return webClient.DownloadString(string.Format(_CoderHistoryDataFeed, coderId));
            }
        }

        public string DownloadRounds()
        {
            using (var webClient = new WebClient())
            {
                return webClient.DownloadString(_RoundsDataFeed);
            }
        }

        public string DownloadRoundHistory(int roundId)
        {
            var request = WebRequest.Create(string.Format(_RoundHistoryDataFeed, roundId));
            request.Timeout = 10*60*1000; // 10 min

            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var streamReader = new StreamReader(responseStream))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}
