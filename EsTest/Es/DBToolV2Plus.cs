using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsTest.Es
{
    public static class DBToolV2Plus
    {
        /// <summary>
        /// 将DataTable导出到CSV.
        /// </summary>
        /// <param name="table">DataTable</param>
        /// <param name="fullSavePath">保存路径</param>
        /// <param name="tableheader">标题信息</param>
        /// <param name="columname">列名称『eg:姓名,年龄』</param>
        /// <returns>导出成功true;导出失败false</returns>
        public static bool ToCSV(this DataTable table,string fullSavePath, string tableheader, string columname)
        {
            //ArgumentChecked(table, fullSavePath);
            //------------------------------------------------------------------------------------
            try
            {
                string _bufferLine = "";
                using (StreamWriter _writerObj = new StreamWriter(fullSavePath, false, Encoding.UTF8))
                {
                    if (!string.IsNullOrEmpty(tableheader))
                        _writerObj.WriteLine(tableheader);
                    if (!string.IsNullOrEmpty(columname))
                        _writerObj.WriteLine(columname);
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        _bufferLine = "";
                        for (int j = 0; j < table.Columns.Count; j++)
                        {
                            if (j > 0)
                                _bufferLine += ",";
                            _bufferLine += table.Rows[i][j].ToString();
                        }
                        _writerObj.WriteLine(_bufferLine);
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        ///// <summary>
        ///// 参数检查
        ///// </summary>
        ///// <param name="table"></param>
        ///// <param name="fullSavePath"></param>
        //private static void ArgumentChecked(DataTable table, string fullSavePath)
        //{
        //    if (table == null)
        //        throw new ArgumentNullException("table");
        //    if (string.IsNullOrEmpty(fullSavePath))
        //        throw new ArgumentNullException("fullSavePath");
        //    string _fileName = CSharpToolV2.GetFileNameOnly(fullSavePath);
        //    if (string.IsNullOrEmpty(_fileName))
        //        throw new ArgumentException(string.Format("参数fullSavePath的值{0},不是正确的文件路径!", fullSavePath));
        //    if (!_fileName.InvalidFileNameChars())
        //        throw new ArgumentException(string.Format("参数fullSavePath的值{0},包含非法字符!", fullSavePath));
        //}
        /// <summary>
        /// 将CSV文件数据导入到Datable中
        /// </summary>
        /// <param name="table"></param>
        /// <param name="filePath">DataTable</param>
        /// <param name="rowIndex">保存路径</param>
        /// <returns>Datable</returns>
        public static DataTable AppendCSVRecord(this DataTable table, StreamReader reader, int rowIndex)
        {
            //ArgumentChecked(table, filePath);
            if (rowIndex < 0)
                throw new ArgumentException("rowIndex");

            int i = 0, j = 0;
            reader.Peek();
            while (reader.Peek() > 0)
            {
                j = j + 1;
                string _line = reader.ReadLine();
                if (j >= rowIndex + 1)
                {
                    string[] _split = _line.Split(',');
                    DataRow _row = table.NewRow();
                    for (i = 0; i < _split.Length; i++)
                    {
                        _row[i] = _split[i];
                    }
                    table.Rows.Add(_row);
                }
            }
            reader.Dispose();
            return table;

        }


        /// <summary>
        /// 将CSV文件数据导入到Datable中
        /// </summary>
        /// <param name="table"></param>
        /// <param name="filePath">DataTable</param>
        /// <param name="rowIndex">保存路径</param>
        /// <returns>Datable</returns>
        public static DataTable CSVConvertToDatable(StreamReader reader,int firstRowIndex)
        {
            DataTable table = new DataTable();
            int i = 0, j = 0;
            reader.Peek();
            while (reader.Peek() > 0)
            {
                string _line = reader.ReadLine();
                if(j==0)
                {
                    string[] _split = _line.Split(',');
                    for (int z = 0; z < _split.Length; z++)
                    {
                        table.Columns.Add(_split[z], typeof(string));
                    }
                }
                else
                {
                    
                        string[] _split = _line.Split(',');
                        DataRow _row = table.NewRow();
                        for (i = 0; i < _split.Length; i++)
                        {
                            _row[i] = _split[i];
                        }
                        table.Rows.Add(_row);
                   
                }
                j = j + 1;

               
            }
            reader.Dispose();
            return table;

        }
    }
}
