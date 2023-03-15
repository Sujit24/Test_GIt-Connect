using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TSS.Helper
{
    public static class PropertyCopier<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        public static void CopyPropertyValues(TSource source, TTarget destination)
        {
            //var destProperties = destination.GetType().GetProperties();

            foreach (var sourceProperty in source.GetType().GetProperties())
            {
                var destProperty = destination.GetType().GetProperty(sourceProperty.Name);
                if (destProperty != null && destProperty.CanWrite)
                {
                    destProperty.SetValue(destination, sourceProperty.GetValue(source, null), null);
                }
            }
        }
    }
}