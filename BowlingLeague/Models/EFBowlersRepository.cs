using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingLeague.Models
{
    public class EFBowlersRepository : IBowlersRepository
    {
        private BowlersDbContext context { get; set; }

        public EFBowlersRepository(BowlersDbContext temp)
        {
            context = temp;
        }
        public IQueryable<Bowler> Bowlers => context.Bowlers;
        public IQueryable<Team> Teams => context.Teams;

        public void SaveBowler(Bowler b)
        {
            context.SaveChanges();
        }

        public void CreateBowler(Bowler b)
        {
            context.Add(b);
            context.SaveChanges();
        }

        public void UpdateBowler(Bowler b)
        {
            context.Update(b);
            context.SaveChanges();
        }
        public void DeleteBowler(Bowler b)
        {
            context.Bowlers.Remove(b);
            context.SaveChanges();
        }
    }
}
