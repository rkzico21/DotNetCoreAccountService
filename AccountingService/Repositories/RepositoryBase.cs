namespace AccountingService.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AccountingService.DbContexts;
    using AccountingService.Entities;
    
    public class RepositoryBase<T> where  T: EntityBase
    {
        private readonly AccountingDbContext dbContext;

        public RepositoryBase(AccountingDbContext dbContext)
        {
            this.dbContext = dbContext;       
        }

        protected AccountingDbContext DbContext
        {
            get { return this.dbContext; }
        }

        public T Add(T entity) 
        {
             //only requires for in memory db
            this.AssignId(entity);
            this.dbContext.GetDbSet<T>(typeof(T)).Add(entity);
            this.dbContext.SaveChanges();
            return entity;
        }
        
        public void Delete(int id)
        {
             var entity = this.FindById(id);
             if(entity != null)
             {
                this.dbContext.GetDbSet<T>(typeof(T)).Remove(entity);
                this.dbContext.SaveChanges();
             }
        }

        public IEnumerable<T> FindAll()
        {
            return this.dbContext.GetDbSet<T>(typeof(T)).AsEnumerable();
        }
        
        public T FindById(int id)
        {
            return this.dbContext.GetDbSet<T>(typeof(T)).FirstOrDefault(e => e.Id == id);
        }
       
        private void AssignId(T entity)
        {
            try
            {
                var value = this.dbContext.GetDbSet<T>(typeof(T)).Max(e=>e.Id);
                entity.Id = value + 1 ;
            }
            catch(Exception ex)
            {
                entity.Id = 1;
            }
            
        }
    }
}