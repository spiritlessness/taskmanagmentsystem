using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TaskManagementSystem.Models;

namespace TaskManagementSystem
{
    public static class Repository
    {
        public static IQueryable<TEntity> Select<TEntity>(ApplicationDbContext db)
      where TEntity : class
        {
            return db.Set<TEntity>();
        }

        public static void Insert<TEntity>(TEntity entity, ApplicationDbContext db) where TEntity : class
        {
            db.Entry(entity).State = EntityState.Added;
            db.SaveChanges();
        }

        public static void Update<TEntity>(TEntity entity,ApplicationDbContext db)
    where TEntity : class
        {
            db.Entry<TEntity>(entity).State = EntityState.Modified;
            db.SaveChanges();
        }

        public static void Delete<TEntity>(TEntity entity, ApplicationDbContext db)
            where TEntity : class
        {
            db.Entry<TEntity>(entity).State = EntityState.Deleted;
            db.SaveChanges();
        } 
    }
}
