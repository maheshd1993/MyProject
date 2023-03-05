using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using ExcelDataReader;

namespace Svam._Class
{
    public class ExcelReader
    {
        string _path;
        Stream _stream;

        public ExcelReader(string path)
        {
            _path = path;
        }

        public ExcelReader(Stream excel)
        {
            _stream = excel;
        }

        public IExcelDataReader getExcelReader()
        {
            // ExcelDataReader works with the binary Excel file, so it needs a FileStream
            // to get started. This is how we avoid dependencies on ACE or Interop:
            FileStream stream = File.Open(_path, FileMode.Open, FileAccess.Read);

            // We return the interface, so that
           IExcelDataReader reader = null;
            try
            {
                if (_path.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                if (_path.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                return reader;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IExcelDataReader getExcelReaderStream()
        {
            // ExcelDataReader works with the binary Excel file, so it needs a FileStream
            // to get started. This is how we avoid dependencies on ACE or Interop:
            Stream stream = _stream;

            // We return the interface, so that
            IExcelDataReader reader = null;
            try
            {
                //if (_path.EndsWith(".xls"))
                //{
                // reader = ExcelReaderFactory.CreateBinaryReader(stream);
                //}
                //if (_path.EndsWith(".xlsx"))
                //{
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                //}
                return reader;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<string> getWorksheetNames()
        {
            var reader = this.getExcelReader();
            var workbook = reader.AsDataSet();
            var sheets = from DataTable sheet in workbook.Tables select sheet.TableName;
            return sheets;
        }

        //public IEnumerable<DataRow> getData(string sheet, bool firstRowIsColumnNames = true)
        //{
        //    var reader = this.getExcelReaderStream();
        //    reader.IsFirstRowAsColumnNames = firstRowIsColumnNames;
        //    var workSheet = reader.AsDataSet().Tables[sheet];
        //    //var rows = from DataRow a in workSheet.Rows select a;
        //    var filteredRows = workSheet.Rows.Cast<DataRow>().Where(row => row.ItemArray.Any(field => !(field is System.DBNull)));//remove blank data row from excel table
        //    return filteredRows;
        //}

      
    }
}