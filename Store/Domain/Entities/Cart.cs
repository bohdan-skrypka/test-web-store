using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>(0);
        public IEnumerable<CartLine> Lines
        {
            get
            {
                return lineCollection;
            }
        }
        public void AddItem(Goods good, int count)
        {
            CartLine line = lineCollection.Where(b => b.Good.GoodsId == good.GoodsId)
                .FirstOrDefault();
            if (line==null)
            {
                lineCollection.Add(new CartLine { Good = good, Count = count });
            }
            else
            {
                line.Count += count;
            }
        }

        public void RemoveLine(Goods good)
        {
            lineCollection.RemoveAll(l=>l.Good.GoodsId==good.GoodsId);
        } 

        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Good.Price * e.Count);
        }

        public void Clear()
        {
            lineCollection.Clear();
        }
    }

    public class CartLine
    {
        public int Count { get;  set; }
        public Goods Good { get;  set; }
    
    }
}
