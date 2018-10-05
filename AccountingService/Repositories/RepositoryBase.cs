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

        public virtual T Add(T entity) 
        {
             //only requires for in memory db
            //this.AssignId(entity);
            this.dbContext.GetDbSet<T>(typeof(T)).Add(entity);
            this.dbContext.SaveChanges();
            return entity;
        }
        
        public void Delete(string id)
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
        
        public T FindById(string id)
        {
            return this.dbContext.GetDbSet<T>(typeof(T)).FirstOrDefault(e => e.Id.ToString() == id);
        }
       
    }
}