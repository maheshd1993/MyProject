using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Svam._Class
{
    public class ImportExcelToDataTable
    {
        public DataTable XLStoDTusingInterOp(string FilePath)
        {
            #region Excel important Note.
            /* Excel creates XLS and XLSX files. These files are hard to read in C# programs. 
             * They are handled with the Microsoft.Office.Interop.Excel assembly. 
             * This assembly sometimes creates performance issues. Step-by-step instructions are helpful.
             * 
             * Add the Microsoft.Office.Interop.Excel assembly by going to Project -> Add Reference.
             */
            #endregion

            Microsoft.Office.Interop.Excel.Application excelApp = null;
            Microsoft.Office.Interop.Excel.Workbook workbook = null;
            System.Data.DataTable dt = new System.Data.DataTable(); //Creating datatable to read the content of the Sheet in File.
            try
            {
                excelApp = new Microsoft.Office.Interop.Excel.Application(); // Initialize a new Excel reader. Must be integrated with an Excel interface object.
                //Opening Excel file(myData.xlsx)
                workbook = excelApp.Workbooks.Open(FilePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets.get_Item(1);
                Microsoft.Office.Interop.Excel.Range excelRange = ws.UsedRange; //gives the used cells in sheet
                
                ws = null; // now No need of this so should expire.
                //Reading Excel file.               
                object[,] valueArray = (object[,])excelRange.get_Value(Microsoft.Office.Interop.Excel.XlRangeValueDataType.xlRangeValueDefault);
                excelRange = null; // you don't need to do any more Interop. Now No need of this so should expire.
                dt = ProcessObjects(valueArray);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                #region Clean Up
                if (workbook != null)
                {
                    #region Clean Up Close the workbook and release all the memory.
                    workbook.Close(false, FilePath, Missing.Value);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                   
                    #endregion
                }
                workbook = null;
                if (excelApp != null)
                {
                    excelApp.Quit();
                }
                excelApp = null;                
                #endregion
            }
            return (dt);
        }

        private DataTable ProcessObjects(object[,] valueArray)
        {
            DataTable dt = new DataTable();

            #region Get the COLUMN names
            for (int k = 1; k <= valueArray.GetLength(1); k++)
            {
                dt.Columns.Add((string)valueArray[1, k]);  //add columns to the data table.
            }
            #endregion

            #region Load Excel SHEET DATA into data table
            object[] singleDValue = new object[valueArray.GetLength(1)];
            //value array first row contains column names. so loop starts from 2 instead of 1
            for (int i = 2; i <= valueArray.GetLength(0); i++)
            {
                for (int j = 0; j < valueArray.GetLength(1); j++)
                {
                    if (valueArray[i, j + 1] != null)
                    {
                        singleDValue[j] = valueArray[i, j + 1].ToString();
                    }
                    else
                    {
                        singleDValue[j] = valueArray[i, j + 1];
                    }
                }
                dt.LoadDataRow(singleDValue, System.Data.LoadOption.PreserveChanges);
            }
            #endregion

            return (dt);
        }
    }
}