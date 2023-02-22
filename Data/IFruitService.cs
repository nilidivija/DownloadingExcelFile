using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DownloadingExcelFile.Data
{
    public interface IFruitService
    {
        Task<List<Fruit>> GetAllFruits();

        Task<List<Fruit>> SearchFruits(string query);

    }
}
