﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Dynamic;


namespace Svam._Class
{
    public static class IQueryableApplySortExtension
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string strSort)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source", "Data source is empty.");
            }

            if (strSort == null)
            {
                return source;
            }

            var lstSort = strSort.Split(',');

            string sortExpression = string.Empty;

            foreach (var sortOption in lstSort)
            {
                if (sortOption.StartsWith("-"))
                {
                    sortExpression = sortExpression + sortOption.Remove(0, 1) + " descending,";
                }
                else
                {
                    sortExpression = sortExpression + sortOption + ",";
                }
            }

            if (!string.IsNullOrWhiteSpace(sortExpression))
            {
                // Note: system.linq.dynamic NuGet package is required here to operate OrderBy on string
                source = source.OrderBy(sortExpression.Remove(sortExpression.Count() - 1));
            }

            return source;
        }
    }
}