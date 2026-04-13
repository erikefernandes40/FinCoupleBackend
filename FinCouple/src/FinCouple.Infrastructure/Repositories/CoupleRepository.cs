using FinCouple.Domain.Entities;
using FinCouple.Domain.Repositories;
using FinCouple.Infrastructure.Data;

namespace FinCouple.Infrastructure.Repositories;

public class CoupleRepository : BaseRepository<Couple>, ICoupleRepository
{
    public CoupleRepository(AppDbContext context) : base(context) { }
}
