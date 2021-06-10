using Entitys.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entitys.Helper.ExcelAround
{
    public class ExcelAroundBorder
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="s"></param>
        /// <param name="d"></param>
        public static void AroundBorder(ExcelWorksheet ws, string s, string d)
        {
            ws.Cells[$"{s}:{d}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"{s}:{d}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"{s}:{d}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"{s}:{d}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }
        public static void AroundBorder(ExcelWorksheet ws, int row, int col)
        {
            ws.Cells[row, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[row, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[row, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[row, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }
        public static void AroundBorder(ExcelWorksheet ws, int fromRow, int fromCol, int toRow, int toCol)
        {
            ws.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[fromRow, fromCol, toRow, toCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="s"></param>
        public static void AroundBorder(ExcelWorksheet ws, string s)
        {
            ws.Cells[$"{s}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"{s}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"{s}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[$"{s}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="column"></param>
        /// <param name="model"></param>
        /// <param name="s1"></param>
        /// <param name="count"></param>
        public static void Book171Helper(ExcelWorksheet ws, int column, Book171Excel model, string[] s1, int count)
        {
            for (int i = 0; i < s1.Length; i++)
            {
                AroundBorder(ws, $"{s1[i]}{column}");
                ws.Cells[$"{s1[i]}{column}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
            string a = model.Account.Substring(0, 5);
            ws.Cells[$"B{column}:C{column}"].Value = $"{a}({count})";
            ws.Cells[$"B{column}:C{column}"].Merge = true;
            ws.Cells[$"B{column}:C{column}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            AroundBorder(ws, $"B{column}", $"C{column}");
            ws.Cells[$"B{column}:C{column}"].Style.WrapText = true;
            ws.Cells[$"B{column}:C{column}"].Merge = true;
            ws.Cells[$"D{column}"].Value = $"{model.SaldoBeginSumma:N}";
            ws.Cells[$"E{column}"].Value = $"{model.KirimSoni}";
            ws.Cells[$"F{column}"].Value = $"{model.KirimSumma:N}";
            ws.Cells[$"G{column}"].Value = $"{model.ChiqimSoni}";
            ws.Cells[$"H{column}"].Value = $"{model.ChiqimSumma:N}";
            ws.Cells[$"I{column}"].Value = $"{model.SaldoEndSumma:N}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="columnName"></param>
        /// <param name="columnCount"></param>
        public static void ExcelWidth(ExcelWorksheet ws, string columnName, int columnCount)
        {
            var list = new List<Excel>();
            if (list.Count == 0)
            {
                ws.Column(columnCount).Width = columnName.Length + 3;
                list.Add(new Excel { ColumnCount = columnCount, ColumnName = columnName });
            }

            foreach (var item in list)
            {
                if (item.ColumnCount == columnCount && columnName.Length > item.ColumnName.Length)
                {
                    ws.Column(columnCount).Width = columnName.Length + 3;
                    list.Add(new Excel { ColumnCount = columnCount, ColumnName = columnName });
                }
                else if (item.ColumnCount != item.ColumnCount)
                {
                    list.Add(new Excel { ColumnCount = columnCount, ColumnName = columnName });
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class Excel
        {
            /// <summary>
            /// 
            /// </summary>
            public int ColumnCount { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string ColumnName { get; set; }
        }
    }
}
