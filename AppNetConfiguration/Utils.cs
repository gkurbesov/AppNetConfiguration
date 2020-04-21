using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AppNetConfiguration
{
    /// <summary>
    /// Utilities for working with data
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// Copies values from one instance of a class to another
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void CopyObjectData(object source, object target)
        {
            MemberInfo[] mi_arr = target.GetType().GetMembers();
            foreach (MemberInfo field in mi_arr)
            {
                if (field.MemberType == MemberTypes.Field)
                {
                    FieldInfo source_field = source.GetType().GetField(field.Name);
                    if (source_field == null)
                        continue;

                    object sourve_target = source_field.GetValue(source);
                    ((FieldInfo)field).SetValue(target, sourve_target);
                }
                else if (field.MemberType == MemberTypes.Property)
                {
                    PropertyInfo property_target = field as PropertyInfo;
                    PropertyInfo property_source = source.GetType().GetProperty(field.Name);
                    if (property_source == null)
                        continue;

                    if (property_target.CanWrite && property_source.CanRead)
                    {
                        object value = property_source.GetValue(source, null);
                        property_target.SetValue(target, value, null);
                    }
                }
            }
        }
    }
}
