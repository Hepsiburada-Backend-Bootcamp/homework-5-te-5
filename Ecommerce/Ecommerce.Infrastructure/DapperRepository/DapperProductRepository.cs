using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Parameters;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Context;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Ecommerce.Infrastructure.DapperRepository
{
    public class DapperProductRepository : IProductRepository
    {
        private readonly IDbConnection _dbConnection;

        public DapperProductRepository(IDbConnection dbContext)
        {
            _dbConnection = dbContext;
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        }
        public async Task<Guid> AddAsync(Product product)
        {

            product.Id = Guid.NewGuid();
            string addSql = "INSERT INTO Products (Id,Name,Brand,Category, Price) VALUES(@Id,@Name,@Brand,@Category,@Price)";
            await _dbConnection.ExecuteAsync(addSql, product);

            string getSql = "SELECT Id FROM Products WHERE Name = @Name AND Brand = @Brand AND Category = @Category AND Price = @Price";
            return await _dbConnection.QuerySingleOrDefaultAsync<Guid>(getSql, product);
        }

        public async Task<List<Product>> GetAllAsync(ProductParameters parameters)
        {
            string sql = "SELECT * FROM Products WHERE Price > @MinPrice ";
            string pagingSql = "ORDER BY Name " +
                               "LIMIT @PageSize " +
                               "OFFSET @Offset ";

            if (parameters.MaxPrice.HasValue)
                sql += "AND Price <= @MaxPrice ";
            if (!String.IsNullOrEmpty(parameters.Brand))
                sql += "AND Brand = @Brand ";
            if (!String.IsNullOrEmpty(parameters.Category))
                sql += "AND Category = @Category ";
            if (!String.IsNullOrEmpty(parameters.Name))
                sql += "AND Name = @Name ";
            if (!String.IsNullOrEmpty(parameters.SearchByBrand))
                sql += "AND Brand LIKE CONCAT('%', @SearchByBrand, '%') ";
            if (!String.IsNullOrEmpty(parameters.SearchByCategory))
                sql += "AND Category LIKE CONCAT('%', @SearchByCategory, '%') ";
            if (!String.IsNullOrEmpty(parameters.SearchByName))
                sql += "AND Name LIKE CONCAT('%', @SearchByName, '%') ";

            sql += pagingSql;
            var result = await _dbConnection.QueryAsync<Product>(sql, parameters);
            return result.ToList();
        }

        public async Task<Product> FindByIdAsync(Guid id)
        {
            string sql = "SELECT * FROM Products WHERE Id = @Id";
            var result = await _dbConnection.QuerySingleOrDefaultAsync<Product>(sql, new { Id = id });
            return result;
        }

        public Task DeleteAsync(Guid id)
        {
            string sql = "DELETE FROM Products WHERE Id = @Id";
            return _dbConnection.ExecuteAsync(sql, new { Id = id });

        }

        public Task UpdateAsync(Product product)
        {
            string sql = "UPDATE Products SET Name = @Name , Brand = @Brand , Category = @Category , Price = @Price WHERE Id = @Id ";
            return _dbConnection.ExecuteAsync(sql, product);
        }
    }
}