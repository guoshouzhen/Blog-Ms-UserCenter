using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace Repository.Extensions
{
    /// <summary>
    /// EFCore扩展，支持原生Sql执行
    /// </summary>
    public static class EFCoreExtension
    {
        /// <summary>
        /// 查询指定sql，返回Datatable
        /// </summary>
        /// <param name="databaseFacade"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable SqlQuery(this DatabaseFacade databaseFacade, string sql, params object[] parameters) 
        {
            DbCommand cmd = CreateCommand(databaseFacade, sql, out DbConnection conn, parameters);
            DbDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            reader.Close();
            conn.Close();
            return dt;
        }

        /// <summary>
        /// 查询指定sql，并转换成指定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="databaseFacade"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IEnumerable<T> SqlQuery<T>(this DatabaseFacade databaseFacade, string sql, params object[] parameters) where T : class, new()
        {
            DataTable dt = SqlQuery(databaseFacade, sql, parameters);
            return dt?.ToEnumerable<T>();
        }

        /// <summary>
        /// 执行sql语句，并返回受影响行数
        /// </summary>
        /// <param name="databaseFacade"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int ExecuteSqlCommand(this DatabaseFacade databaseFacade, string sql, params object[] parameters) 
        {
            DbCommand cmd = CreateCommand(databaseFacade, sql, out DbConnection conn, parameters);
            try
            {
                return cmd.ExecuteNonQuery();
            }
            catch
            {
                return -1;
            }
            finally 
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 将datatable转成指定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static IEnumerable<T> ToEnumerable<T>(this DataTable dt) where T : class, new() 
        {
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            int n = dt.Rows.Count;
            T[] ts = new T[n];
            int i = 0;
            foreach (DataRow row in dt.Rows) 
            {
                T t = new T();
                foreach (PropertyInfo prop in propertyInfos) 
                {
                    if (dt.Columns.IndexOf(prop.Name) >= 0 && row[prop.Name] != DBNull.Value) 
                    {
                        prop.SetValue(t, row[prop.Name], null);
                    }
                    ts[i++] = t;
                }
            }
            return ts;
        }

        /// <summary>
        /// 根据conn创建Connmand对象
        /// </summary>
        /// <param name="databaseFacade"></param>
        /// <param name="sql"></param>
        /// <param name="conn"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        private static DbCommand CreateCommand(DatabaseFacade databaseFacade, string sql, out DbConnection conn, params object[] paramters) 
        {
            conn = databaseFacade.GetDbConnection();
            conn.Open();
            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            //设置参数
            if (paramters != null && paramters.Length > 0) 
            {
                foreach (var paramter in paramters) 
                {
                    cmd.Parameters.Add(paramter);
                }
            }
            return cmd;
        }
    }
}
