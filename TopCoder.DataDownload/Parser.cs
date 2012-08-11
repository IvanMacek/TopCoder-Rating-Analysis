using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

using TopCoder.DataDownload.Entity;

namespace TopCoder.DataDownload
{
    public class Parser
    {
        public IList<CoderRating> ParseCoders(string data)
        {
            var xml = XElement.Parse(data);

            return
                (from row in xml.Elements("row")
                 where !string.IsNullOrEmpty(row.Element("alg_rating").Value)
                 select new CoderRating
                 {
                     Id = int.Parse(row.Element("coder_id").Value),
                     Handle = row.Element("handle").Value,
                     Country = row.Element("country_name").Value,
                     Rating = int.Parse(row.Element("alg_rating").Value),
                     Volatility = int.Parse(row.Element("alg_vol").Value),
                     EventsCount = int.Parse(row.Element("alg_num_ratings").Value),
                 }
                ).ToList();
        }

        public IList<CoderRoundHistory> ParseCoderRoundHistory(Round round, string roundHistory)
        {
            var xml = XElement.Parse(roundHistory);

            return
                (from row in xml.Elements("row")

                 // NOTE: Redci koji nemaju division_placed su nekakav višak podataka i ne prikazuju se na TopCoder portalu
                 let divisionPlaced = row.Element("division_placed").Value
                 //where !string.IsNullOrWhiteSpace(divisionPlaced) || divisionPlaced == "2000" || divisionPlaced == "3000"
                 
                 select new CoderRoundHistory
                 {
                     RoundId = round.Id,
                     RoundName = round.ShortName,
                     RoundDateTime = round.DateTime,

                     CoderId = int.Parse(row.Element("coder_id").Value),
                     CoderHandle = row.Element("handle").Value,

                     Division = int.Parse(row.Element("division").Value),
                     DivisionPlace = !string.IsNullOrWhiteSpace(divisionPlaced) ? int.Parse(divisionPlaced) : 0, 
                     Points = float.Parse(row.Element("final_points").Value, CultureInfo.InvariantCulture.NumberFormat),

                     OldRating = int.Parse(row.Element("old_rating").Value),
                     NewRating = int.Parse(row.Element("new_rating").Value),
                     NewVolatility = int.Parse(row.Element("new_vol").Value),
                     NumberOfRatings = int.Parse(row.Element("num_ratings").Value),
                     IsRated = int.Parse(row.Element("rated_flag").Value) == 1

                     //Rank= int.Parse(row.Element("rank").Value),
                     //Percentile = float.Parse(row.Element("percentile").Value, CultureInfo.InvariantCulture.NumberFormat),
                     //RatingOrder= int.Parse(row.Element("rating_order").Value),
                 }
                ).ToList();
        }

        public IList<Round> ParseRounds(string data)
        {
            var xml = XElement.Parse(data);

            return
                (from row in xml.Elements("row")
                 select new Round
                 {
                     Id = int.Parse(row.Element("round_id").Value),
                     FullName = row.Element("full_name").Value,
                     ShortName = row.Element("short_name").Value,
                     Type = row.Element("round_type_desc").Value,
                     DateTime = DateTime.Parse(row.Element("date").Value),
                 }
                ).ToList();
        }
    }
}