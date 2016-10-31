using System;
using System.Collections.Generic;
using System.Reflection;

namespace Super.Framework
{
    #region 特性辅助类
    /// <summary>
    /// 特性辅助类
    /// </summary>
    public static class AttributeHelper
    {
        #region 程序集是否含有指定的特性的类
        /// <summary>
        /// 程序集是否含有指定的特性的类
        /// </summary>
        /// <param name="InputAssembly">程序集</param>
        /// <param name="InputAttribute">指定的特性</param>
        /// <param name="AttributeClassList">指定特性的类的列表</param>
        public static bool IsHasAttributeClass<T>(this Assembly InputAssembly)
        {
            bool reVal = false;
            Type[] AssemblyTypes = InputAssembly.GetExportedTypes();
            Func<System.Attribute[], bool> IsAtte = o =>
            {
                foreach (System.Attribute a in o)
                {
                    if (a is T)
                    {
                        return true;
                    }
                }
                return false;
            };
            foreach (Type InType in AssemblyTypes)
            {
                if (IsAtte(Attribute.GetCustomAttributes(InType)))
                {
                    reVal = true;
                    break;
                }
            }
            return reVal;
        }
        #endregion 程序集是否含有指定的特性的类

        #region 程序集是否含有指定的特性的方法
        /// <summary>
        /// 程序集是否含有指定的特性的方法
        /// </summary>
        public static bool IsHasAttributeClassMethond<T>(this Assembly InputAssembly)
        {
            bool reVal = false;
            Func<System.Object[], bool> IsAtte = o =>
            {
                foreach (System.Attribute a in o)
                {
                    if (a is T)
                    {
                        return true;
                    }
                }
                return false;
            };
            try
            {
                Type[] AssemblyTypes = InputAssembly.GetExportedTypes();
                foreach (Type InType in AssemblyTypes)
                {
                    MethodInfo[] MethodInfos = InType.GetMethods();
                    foreach (MethodInfo InMethod in MethodInfos)
                    {
                        if (IsAtte(Attribute.GetCustomAttributes(InMethod)))
                        {
                            reVal = true;
                            goto IsHasAtte;
                        }
                    }
                    if (!reVal)
                    {
                        PropertyInfo[] PropertyInfos = InType.GetProperties();
                        foreach (PropertyInfo InProperty in PropertyInfos)
                        {
                            if (IsAtte(InProperty.GetCustomAttributes(true)))
                            {
                                reVal = true;
                                goto IsHasAtte;
                            }
                        }
                    }
                }

            IsHasAtte:
                return reVal;
            }
            catch (Exception ex)
            {
                ex.ToLog();
                return false;
            }
        }
        #endregion 程序集是否含有指定的特性的方法

        #region 获取程序集类型方法中指定特性的信息
        public static List<T> GetAttributeList<T>(this Assembly InputAssembly) where T : Attribute
        {
            List<T> reVal = new List<T>();
            Type[] AssemblyTypes = InputAssembly.GetExportedTypes();

            foreach (Type InType in AssemblyTypes)
            {
                foreach (MethodInfo InMethod in InType.GetMethods())
                {
                    foreach (Attribute InAttribute in Attribute.GetCustomAttributes(InMethod))
                    {
                        if (InAttribute is T)
                        {
                            reVal.Add((T)InAttribute);
                        }
                    }
                }
            }

            return reVal;
        }
        #endregion 获取程序集类型方法中指定特性的信息
    }
    #endregion 特性辅助类
}