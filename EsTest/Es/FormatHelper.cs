using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EsTest.Es
{
    /// <summary>
    /// 转化帮助类
    /// </summary>
    public static class FormatHelper
    {
        /// <summary>
        /// 实体转换成DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static DataTable EntityToDataTable<T>(this T t) where T:class,new()
        {
            DataTable dt = new DataTable();
            foreach(var prop in typeof(T).GetProperties())
            {
                dt.Columns.Add(prop.Name,prop.GetValue(t).GetType());
                
            }
            //if (t != null && t != default(T))
            //{
            //    DataRow row = dt.NewRow();
            //    foreach (var prop in typeof(T).GetProperties())
            //    {
            //        row[prop.Name] = prop.GetValue(t);
            //    }
            //    dt.Rows.Add(row);
            //}
            return dt;
        }

        public static List<T> DataTableToEntity<T>(this DataTable table) where T : class, new()
        {
            List<T> ts = new List<T>();// 定义集合
            Type type = typeof(T); // 获得此模型的类型
            string tempName = "";
            foreach (DataRow dr in table.Rows)
            {
                T t = new T();
                PropertyInfo[] propertys = t.GetType().GetProperties();// 获得此模型的公共属性
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;
                    if (table.Columns.Contains(tempName))
                    {
                        if (!pi.CanWrite) continue;
                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;
        }
    }
}
