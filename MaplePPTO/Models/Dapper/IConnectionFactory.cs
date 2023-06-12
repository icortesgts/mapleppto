using System;
using System.Data;

namespace MaplePPTO.Models.Dapper
{
    public interface IConnectionFactory:IDisposable
    {
        IDbConnection GetConnection { get; }
    }
}
