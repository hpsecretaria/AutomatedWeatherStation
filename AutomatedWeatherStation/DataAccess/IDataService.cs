using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace AutomatedWeatherStation.DataAccess
{
    //IDataService is to allow all entities to have the common method of add remove...
    //interface always start with I
    public interface IDataService<T> where T : class //T is the entity
    {
        void Add(T record); //void coz it returns nothing

        Task<int> AddAsync(T record); //no way of cancelling
        Task<int> AddAsync(T record, CancellationToken token); //cancelling will disregard the process


        void AddRange(List<T> records);
        Task AddRangeAsync(List<T> record);
        Task AddRangeAsync(List<T> record, CancellationToken token);


        void Remove(T record);
        Task RemoveAsync(T record);
        Task RemoveAsync(T record, CancellationToken token);

        void RemoveRange(List<T> records);
        Task RemoveRangeAsync(List<T> records);
        Task RemoveRangeAsync(List<T> records, CancellationToken token);


        void Update(T record);
        Task UpdateAsync(T record);
        Task UpdateAsync(T record, CancellationToken token);


        void UpdateRange(List<T> records);
        Task UpdateRangeAsync(List<T> records);
        Task UpdateRangeAsync(List<T> records, CancellationToken token);

        //List<T> GetRange(Func<List<T>, bool> predicate); //because it returns a list of the entity

        //T Get(Func<T, bool> predicate); //T because it returns the entity


        T Get(Expression<Func<T, bool>> condition);
        //Func is the pointer to a method that can return a value. pointer is the 'reference'
        //T inside func is the return parameterb. ool is the parameter only
        Task<T> GetAsync(Expression<Func<T, bool>> condition);
        Task<T> GetAsync(Expression<Func<T, bool>> condition, CancellationToken token);

        List<T> GetRange(Expression<Func<T, bool>> condition);
        Task<List<T>> GetRangeAsync(Expression<Func<T, bool>> condition);
        Task<List<T>> GetRangeAsync(Expression<Func<T, bool>> condition, CancellationToken token);

        List<T> GetRange();
        Task<List<T>> GetRangeAsync();
        Task<List<T>> GetRangeAsync(CancellationToken token);
    }
}