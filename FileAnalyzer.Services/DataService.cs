using FileAnalyzer.Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAnalyzer.Services
{
    public class DataService
    {
        private CSVReaderService csvReaderService;
        public DataService()
        {
            csvReaderService = new CSVReaderService();
        }

        /// <summary>
        /// Returns a set of key/value pairs of recurring first and last names in order of
        /// frequency (most first) followed by alphabetically.
        /// </summary>
        /// <param name="entries">A set of entries containing first and last names.</param>
        /// <returns></returns>
        public Dictionary<string, int> GetNamesByFrequency(List<Entry> entries)
        {
            if (entries == null || entries.Count == 0)
                throw new FileAnalyzerException(10006);
            var names = new Dictionary<string, int>();
            foreach(var name in entries.SelectMany(e => new[] { e.FirstName, e.LastName }))
            {
                if (!names.ContainsKey(name))
                    names.Add(name, 0);
                names[name]++;
            }
            var results = new Dictionary<string, int>();
            foreach (var name in names.OrderByDescending(r => r.Value).ThenBy(r => r.Key))
                results.Add(name.Key, name.Value);
            return results;
        }

        /// <summary>
        /// Returns a collection of addresses sorted alphabetically by street name.
        /// </summary>
        /// <param name="entries">A set of entries containing the full address.</param>
        /// <returns></returns>
        public List<string> GetAddressesByName(List<Entry> entries)
        {
            if (entries == null || entries.Count == 0)
                throw new FileAnalyzerException(10006);

            return entries.OrderBy(e => String.Join("", e.Address.Where(a => a == ' ' || Char.IsLetter(a)))).
                Select(e => e.Address).ToList();
        }

    }
}
