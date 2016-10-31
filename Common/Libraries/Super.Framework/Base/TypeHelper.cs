using System;
using System.Collections.Generic;
using System.Reflection;

namespace Super.Framework
{
    #region Type辅助器
    /// <summary>
    /// Type辅助器
    /// </summary>
    public static class TypeHelper
    {
        #region 获取可空类型的实际类型
        /// <summary>
        /// 获取可空类型的实际类型
        /// </summary>
        public static Type GetUnNullableType(this Type InputType)
        {
            try
            {
                string Key = InputType.Name + "-GetUnNullableType";
                if (CacheManage.Get(Key) != null)
                {
                    InputType = (Type)(CacheManage.Get(Key));
                }
                else
                {
                    if (InputType.IsGenericType && InputType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        var nullableConverter = new System.ComponentModel.NullableConverter(InputType);
                        InputType = nullableConverter.UnderlyingType;
                    }
                    CacheManage.Insert(Key, InputType);
                }
                return InputType;
            }
            catch (Exception ex)
            {
                ex.Data.Add("Running Process", "Super.Framework.TypeHelper.GetUnNullableType");
                ex.Data.Add("Working Parameter Type", InputType.Name);
                throw ex;
            }
        }
        #endregion 获取可空类型的实际类型

        #region 获取命名空间下的所有类型名称列表
        /// <summary>
        /// 获取命名空间下的所有类型名称列表
        /// </summary>
        /// <param name="InputNameSpace">指定的命名空间</param>
        public static List<string> GetClassList(string InputNameSpace, string FileName = "")
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            if (FileName.IsNotNullOrEmpty())
            {
                asm = Assembly.LoadFrom(SuperManager.FileFullPath("~/bin/" + FileName));
            }
            List<string> NameSpaceList = new List<string>();
            List<string> reVal = new List<string>();
            foreach (Type type in asm.GetTypes())
            {
                if (type.Namespace == InputNameSpace)
                    NameSpaceList.Add(type.Name);
            }
            foreach (string ClassName in NameSpaceList)
                reVal.Add(ClassName);

            return reVal;
        }
        #endregion 获取命名空间下的所有类型名称列表

        #region 获取命名空间下是否有指定名称的类
        /// <summary>
        /// 获取命名空间下是否有指定名称的类
        /// </summary>
        /// <param name="InputClassName">指定名称的类名</param>
        public static bool IsHasClass(this string InputNameSpace, string InputClassName, string FileName = "")
        {
            bool reVal = false;
            string Key = InputNameSpace + "-" + InputClassName + "-IsHasClass";
            if (CacheManage.Get(Key) != null)
            {
                reVal = (bool)CacheManage.Get(Key);
            }
            else
            {
                List<string> ClassList = GetClassList(InputNameSpace, FileName);
                reVal = InputClassName.In(ClassList);
                CacheManage.Insert(Key, reVal);
            }
            return reVal;
        }
        #endregion 获取命名空间下是否有指定名称的类

        #region 获取指定类是否含有指定名称的方法
        /// <summary>
        /// 获取指定类是否含有指定名称的方法
        /// </summary>
        /// <param name="ClassPath">类的全路径</param>
        /// <param name="InputMethodName">方法名称</param>
        public static bool IsHasMethod(this string ClassPath, string InputMethodName, string FileName = "")
        {
            bool reVal = false;
            string Key = ClassPath + "-" + InputMethodName + "-IsHasMethod";
            if (CacheManage.Get(Key) != null)
            {
                reVal = (bool)CacheManage.Get(Key);
            }
            else
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                if (FileName.IsNotNullOrEmpty())
                {
                    asm = Assembly.LoadFrom(SuperManager.FileFullPath("~/bin/" + FileName));
                }
                var type = asm.GetType(ClassPath);
                List<string> MethodNameList = GetMethodNameList(type);
                if (MethodNameList.Contains(InputMethodName))
                {
                    reVal = true;
                }
                CacheManage.Insert(Key, reVal);
            }
            return reVal;
        }
        #endregion 获取指定类是否含有指定名称的方法

        #region 创建运行实例下的程序集中的类运行实例
        /// <summary>
        /// 创建运行实例下的程序集中的类运行实例
        /// </summary>
        public static object RunningInstance(this string ClassPath)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            var reVal = asm.CreateInstance(ClassPath);

            return reVal;
        }
        #endregion 创建运行实例下的程序集中的类运行实例

        #region 调用实例的方法
        /// <summary>
        /// 调用实例的方法
        /// </summary>
        /// <param name="ClassPath">类路径</param>
        /// <param name="MethodName">方法名称</param>
        public static object CallMethod(this string ClassPath, string MethodName, object[] Prams, string FileName = "")
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            if (FileName.IsNotNullOrEmpty())
            {
                asm = Assembly.LoadFrom(SuperManager.FileFullPath("~/bin/" + FileName));
            }
            var RunInstance = asm.CreateInstance(ClassPath);
            var type = asm.GetType(ClassPath);
            var Method = type.GetMethod(MethodName);
            var reVal = Method.Invoke(RunInstance, Prams);

            return reVal;
        }
        #endregion 调用实例的方法

        #region 获取类型的字段列表
        /// <summary>
        /// 获取类型的字段列表
        /// </summary>
        /// <param name="InputType">输入的类型</param>
        public static List<FieldInfo> GetFieldList(this Type InputType)
        {
            try
            {
                string Key = InputType.Name + "-GetFieldList";
                List<FieldInfo> reVal = null;
                if (CacheManage.Get(Key) != null)
                {
                    reVal = (List<FieldInfo>)CacheManage.Get(Key);
                }
                else
                {
                    reVal = new List<FieldInfo>(InputType.GetFields());
                    CacheManage.Insert(Key, reVal);
                }
                return reVal;
            }
            catch (Exception ex)
            {
                ex.Data.Add("Running Process", "Super.Framework.TypeHelper.GetFieldNameList");
                ex.Data.Add("Working Parameter Type", InputType.Name);
                throw ex;
            }
        }
        #endregion 获取类型的字段列表

        #region 获取类型的字段名称列表
        /// <summary>
        /// 获取类型的字段名称列表
        /// </summary>
        /// <param name="InputType"></param>
        /// <returns></returns>
        public static List<string> GetFieldNameList(this Type InputType)
        {
            try
            {
                List<string> reVal = new List<string>();
                string Key = InputType.Name + "-GetFieldNameList";
                if (!Key.IsNullOrEmpty())
                {
                    reVal = (List<string>)CacheManage.Get(Key);
                }
                else
                {
                    FieldInfo[] FI = InputType.GetFields();
                    foreach (FieldInfo F in FI)
                    {
                        if (!reVal.Contains(F.Name))
                        {
                            reVal.Add(F.Name);
                        }
                    }
                    CacheManage.Insert(Key, reVal);
                }
                return reVal;
            }
            catch (Exception ex)
            {
                ex.Data.Add("Running Process", "Super.Framework.TypeHelper.GetFieldNameList");
                ex.Data.Add("Working Parameter Type", InputType.Name);
                throw ex;
            }
        }
        #endregion 获取类型的字段名称列表

        #region 获取类型的属性列表
        /// <summary>
        /// 获取类型的属性列表
        /// </summary>
        /// <param name="InputType">输入的类型</param>
        public static List<PropertyInfo> GetPropertyList(this Type InputType)
        {
            try
            {
                List<PropertyInfo> reVal = null;
                string Key = InputType.Name + "-GetPropertyList";
                if (CacheManage.Get(Key) != null)
                {
                    reVal = (List<PropertyInfo>)CacheManage.Get(Key);
                }
                else
                {
                    reVal = new List<PropertyInfo>(InputType.GetProperties());
                    CacheManage.Insert(Key, reVal);
                }
                return reVal;
            }
            catch (Exception ex)
            {
                ex.Data.Add("Running Process", "Super.Framework.TypeHelper.GetPropertyList");
                ex.Data.Add("Working Parameter Type", InputType.Name);
                throw ex;
            }
        }
        #endregion 获取类型的属性列表

        #region 获取类型的属性名称列表
        /// <summary>
        /// 获取类型的属性列表
        /// </summary>
        /// <param name="InputType">输入的类型</param>
        public static List<string> GetPropertyNameList(this Type InputType)
        {
            try
            {
                List<string> reVal = new List<string>();
                string Key = InputType.Name + "-PropertyNameList";
                if (CacheManage.Get(Key) != null)
                {
                    reVal = (List<string>)CacheManage.Get(Key);
                }
                else
                {
                    PropertyInfo[] MI = InputType.GetProperties();
                    foreach (MemberInfo M in MI)
                    {
                        reVal.Add(M.Name);
                    }
                    CacheManage.Insert(Key, reVal);
                }
                return reVal;
            }
            catch (Exception ex)
            {
                ex.Data.Add("Running Process", "Super.Framework.TypeHelper.GetPropertyNameList");
                ex.Data.Add("Working Parameter Type", InputType.Name);
                throw ex;
            }
        }
        #endregion 获取类型的属性名称列表

        #region 获取类型的方法列表
        /// <summary>
        /// 获取类型的方法列表
        /// </summary>
        /// <param name="InputType">输入的类型</param>
        public static List<MethodInfo> GetMethodList(this Type InputType)
        {
            try
            {
                List<MethodInfo> reVal = null;
                string Key = InputType.Name + "-GetMethodList";
                if (CacheManage.Get(Key) != null)
                {
                    reVal = (List<MethodInfo>)CacheManage.Get(Key);
                }
                else
                {
                    reVal = new List<MethodInfo>(InputType.GetMethods());
                    CacheManage.Insert(Key, reVal);
                }
                return reVal;
            }
            catch (Exception ex)
            {
                ex.Data.Add("Running Process", "Super.Framework.TypeHelper.GetMethodList");
                ex.Data.Add("Working Parameter Type", InputType.Name);
                throw ex;
            }
        }
        #endregion 获取类型的方法列表

        #region 获取类型的方法名称列表
        /// <summary>
        /// 获取类型的方法名称列表
        /// </summary>
        /// <param name="InputType">输入类型</param>
        public static List<string> GetMethodNameList(Type InputType)
        {
            try
            {
                List<string> reVal = new List<string>();
                string Key = InputType + "-GetMethodNameList";
                if (CacheManage.Get(Key) != null)
                {
                    reVal = (List<string>)CacheManage.Get(Key);
                }
                else
                {
                    MethodInfo[] MIS = InputType.GetMethods();
                    foreach (MethodInfo MI in MIS)
                    {
                        if (!reVal.Contains(MI.Name))
                        {
                            reVal.Add(MI.Name);
                        }
                    }
                    CacheManage.Insert(Key, reVal);
                }
                return reVal;
            }
            catch (Exception ex)
            {
                ex.Data.Add("Running Process", "Super.Framework.TypeHelper.GetMethodNameList");
                ex.Data.Add("Working Parameter Type", InputType.Name);
                throw ex;
            }
        }
        #endregion 获取类型的方法名称列表

        #region 获取静态类的属性值
        /// <summary>
        /// 获取静态类的属性值
        /// </summary>
        /// <param name="InputType">输入的类型</param>
        /// <param name="PropertyName">属性名称</param>
        public static string GetStaticTypePropertyValue(this Type InputType, string PropertyName)
        {
            try
            {
                string reVal = string.Empty;
                string Key = InputType.Name + "-Property." + PropertyName + "-GetStaticTypePropertyValue";
                PropertyInfo p = InputType.GetProperty(PropertyName);
                var Val = p.GetValue(null, null);
                if (Val != null)
                {
                    reVal = Val.ToString();
                }
                return reVal;
            }
            catch (Exception ex)
            {
                ex.Data.Add("Running Process", "Super.Framework.TypeHelper.GetPropertyValue");
                ex.Data.Add("Working Parameter Type", InputType.Name);
                ex.Data.Add("Working Parameter PropertyName", PropertyName);
                throw ex;
            }
        }
        #endregion 获取静态类的属性值

        #region 设置静态类的属性值
        /// <summary>
        /// 设置静态类的属性值
        /// </summary>
        /// <param name="InputType">输入类型</param>
        /// <param name="PropertyName">属性名称</param>
        /// <param name="PropertyValue">属性值</param>
        public static bool SetStaticTypePropertyValue(this Type InputType, string PropertyName, string PropertyValue)
        {
            try
            {
                bool reVal = false;
                PropertyInfo p = InputType.GetProperty(PropertyName);
                object val = Convert.ChangeType(PropertyValue, p.PropertyType);
                p.SetValue(null, val, null);
                reVal = true;
                return reVal;
            }
            catch (Exception ex)
            {
                ex.Data.Add("Running Process", "Super.Framework.TypeHelper.SetPropertyValue");
                ex.Data.Add("Working Parameter Type", InputType.Name);
                ex.Data.Add("Working Parameter PropertyName", PropertyName);
                ex.Data.Add("Working Parameter PropertyValue", PropertyValue);
                throw ex;
            }
        }
        #endregion 设置静态类的属性值

        #region 获取对象的属性值
        /// <summary>
        /// 获取对象的属性值
        /// </summary>
        /// <param name="InputObject">对象</param>
        /// <param name="PropertyName">属性名称</param>
        public static string GetObjectPropertyValue(this object InputObject, string PropertyName)
        {
            try
            {
                string reVal = string.Empty;
                if (PropertyName.IsNotNullOrEmpty())
                {
                    Type InputType = InputObject.GetType();
                    PropertyInfo p = InputType.GetProperty(PropertyName);
                    var Val = p.GetValue(InputObject, null);
                    if (Val != null)
                    {
                        reVal = Val.ToString();
                    }
                }
                return reVal;
            }
            catch (Exception ex)
            {
                ex.Data.Add("Running Process", "Super.Framework.TypeHelper.GetObjectPropertyValue");
                ex.Data.Add("Working Parameter object", InputObject.ToString());
                ex.Data.Add("Working Parameter PropertyName", PropertyName);
                throw ex;
            }
        }
        #endregion 获取对象的属性值

        #region 设置对象的属性值
        /// <summary>
        /// 设置对象的属性值
        /// </summary>
        /// <param name="InputObject">对象</param>
        /// <param name="PropertyName">属性名称</param>
        /// <param name="PropertyValue">属性值</param>
        public static bool SetObjectPropertyValue(this object InputObject, string PropertyName, string PropertyValue)
        {
            try
            {
                bool reVal = false;
                Type InputType = InputObject.GetType();
                PropertyInfo p = InputType.GetProperty(PropertyName);
                if (p.PropertyType == typeof(System.Int32) && PropertyValue == "")
                {
                    PropertyValue = "0";
                }
                object val = Convert.ChangeType(PropertyValue, p.PropertyType);
                p.SetValue(InputObject, val, null);
                reVal = true;
                return reVal;
            }
            catch (Exception ex)
            {
                ex.Data.Add("Running Process", "Super.Framework.TypeHelper.SetObjectPropertyValue");
                ex.Data.Add("Working Parameter Object", InputObject.ToString());
                ex.Data.Add("Working Parameter PropertyName", PropertyName);
                ex.Data.Add("Working Parameter PropertyValue", PropertyValue);
                throw ex;
            }
        }
        #endregion 设置对象的属性值
    }
    #endregion Type辅助器
}