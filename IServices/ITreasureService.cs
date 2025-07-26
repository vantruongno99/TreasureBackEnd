using Microsoft.AspNetCore.Mvc;
using TreasureBackEnd.Model;
using TreasureBackEnd.Model;

namespace TreasureBackEnd.IServices
{
    public interface ITreasureService
    {
        Task<double> CalculateMinimumFuel(TreasureInput input);

        Task<List<TreasureRecord>> GetHistory();

    }
}
