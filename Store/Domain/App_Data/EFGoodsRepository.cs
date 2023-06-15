using Domain.Abstract;
using System.Collections.Generic;
using Domain.Entities;
using System;

namespace Domain.App_Data
{
    public class EFGoodsRepository: IGoodsRepository
    {
        EFDataDb data = new EFDataDb();

        public IEnumerable<Goods> Goods
        {
            get
            {
                return data.Goods;
            }
        }

        public void SaveGood(Goods good)
        {
           
                Goods db = data.Goods.Find(good.GoodsId);
                if (db != null)
                {
                    db.Name = good.Name;
                    db.Author = good.Author;
                    db.Description = good.Description;
                    db.Genre = good.Description;
                    db.Price = good.Price;
                    db.ImageData = good.ImageData;
                    db.ImageMimeType = good.ImageMimeType;
                }
            data.SaveChanges();
        }
        public void DeleteGood(Goods good)
        {
            if (good.GoodsId == 0)
            {
                data.Goods.Add(good);
            }
            else
            {
                Goods db = data.Goods.Find(good.GoodsId);
                if (db != null)
                {
                    db.Name = good.Name;
                    db.Author = good.Author;
                    db.Description = good.Description;
                    db.Genre = good.Description;
                    db.Price = good.Price;
                    db.ImageData = good.ImageData;
                    db.ImageMimeType = good.ImageMimeType;
                }
            }
            data.SaveChanges();
        }

        public void AddGood(Goods good)
        {
            if (good.GoodsId == 0)
            {
                data.Goods.Add(good);
            }
            else
            {
                Goods db = data.Goods.Find(good.GoodsId);
                if (db != null)
                {
                    db.Name = good.Name;
                    db.Author = good.Author;
                    db.Description = good.Description;
                    db.Genre = good.Description;
                    db.Price = good.Price;
                    db.ImageData = good.ImageData;
                    db.ImageMimeType = good.ImageMimeType;
                }
            }
            data.SaveChanges();
        }
    }
}