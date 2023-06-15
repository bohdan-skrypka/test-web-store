using Domain.Entities;
using System.Collections.Generic;

namespace Domain.Abstract
{
    public interface IGoodsRepository
    {
        IEnumerable<Goods> Goods { get; }
        void SaveGood(Goods good);
        void DeleteGood(Goods good);
        void AddGood(Goods good);
    }
}
