using DbLunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbLunch.Data
{
    public class VoteRepository : BaseRepository<Vote>, IVoteRepository
    {
        public VoteRepository() : base("votes.json") { }

        public async Task<IEnumerable<Vote>> getVotesByDate(DateTime date)
        {
            var all = await All();
            return all.Where(v => v.date.Date == date.Date);
        }
    }
}
