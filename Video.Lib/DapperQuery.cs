using Dapper;
//using HG.Infrastructure.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;

namespace Video.Lib
{
    public class DapperQuery : IDisposable
    {
        private IDbConnection conn;

        public DapperQuery()
        {
            this.conn = DbConnectionFactory.CreateConnection(Enums.Core);
            //if (this.conn.State == ConnectionState.Closed) this.conn.Open();
        }

        public DapperQuery(Enums dbName)
        {
            this.conn = DbConnectionFactory.CreateConnection(dbName);
        }

        public T QuerySingle<T>(string sql, object paramPairs = null) 
        {
            return this.conn.Query<T>(sql, paramPairs).SingleOrDefault();
        }

        public IEnumerable<T> QueryList<T>(string sql, object paramPairs = null)
        {
            return this.conn.Query<T>(sql, paramPairs);
        }

        public long Count(string sql, object paramPairs = null)
        {
            return this.conn.Query<long>(sql, paramPairs).SingleOrDefault();
        }

        public long Count(string sql, IDbConnection conn, object paramPairs = null)
        {
            return this.conn.Query<long>(sql, paramPairs).SingleOrDefault();
        }

        #region

        public void Dispose()
        {
            //调用带参数的Dispose方法，释放托管和非托管资源
            Dispose(true);
            //手动调用了Dispose释放资源，那么析构函数就是不必要的了，这里阻止GC调用析构函数
            System.GC.SuppressFinalize(this);
        }

        //protected的Dispose方法，保证不会被外部调用。
        //传入bool值disposing以确定是否释放托管资源
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                ///TODO:在这里加入清理"托管资源"的代码，应该是xxx.Dispose();
                if (this.conn != null)
                {
                    if (this.conn.State == ConnectionState.Open)
                    {
                        this.conn.Close();
                    }

                    this.conn.Dispose();
                }
            }
            ///TODO:在这里加入清理"非托管资源"的代码
        }

        //供GC调用的析构函数
        ~DapperQuery()
        {
            Dispose(false);//释放非托管资源
        }

        #endregion
    }
}
