namespace Super.Framework
{
    #region 查询信息
    /// <summary>
    /// 查询信息
    /// </summary>
    public class QueryInfo
    {
        #region 数据库连接字符串
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnString { get; set; }
        #endregion 数据库连接字符串

        #region 数据表名
        /// <summary>
        /// 数据表名
        /// </summary>
        public string TableName { get; set; }
        #endregion 数据表名

        #region 过滤条件
        /// <summary>
        /// 过滤条件
        /// </summary>
        public string Filter { get; set; }
        #endregion 过滤条件

        #region 排序字段
        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderColumn { get; set; }
        #endregion 排序字段

        #region 排序方向
        /// <summary>
        /// 排序方向
        /// </summary>
        public string OrderDirection { get; set; }
        #endregion 排序方向

        #region 页索引
        /// <summary>
        /// 页索引
        /// </summary>
        public int PageIndex { get; set; }
        #endregion 页索引

        #region 页大小
        /// <summary>
        /// 页大小
        /// </summary>

        public int PageSize { get; set; }
        #endregion 页大小

        #region 查询字段
        /// <summary>
        /// 查询字段
        /// </summary>
        public string SelectColumn { get; set; }
        #endregion 查询字段

        #region 查询信息
        /// <summary>
        /// 查询信息
        /// </summary>
        public QueryInfo()
        {
            OrderColumn = "ID";
            OrderDirection = "ASC";
            SelectColumn = " * ";
            PageIndex = 0;
            PageSize = 100000;
        }
        #endregion 查询信息
    }
    #endregion 查询信息
}