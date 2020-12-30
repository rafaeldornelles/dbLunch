using DbLunch.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DbLunch.Data
{
    public interface IVoteRepository : IRepository<Vote>
    {
        Task<IEnumerable<Vote>> getVotesByDate(DateTime date);
    }
}