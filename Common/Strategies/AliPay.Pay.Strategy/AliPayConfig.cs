using Super.Framework;

namespace AliPay.Pay.Strategy
{
    #region 阿里支付配置档
    /// <summary>
    /// 阿里支付配置档
    /// </summary>
    public class AliPayConfig
    {
        #region 类型名称
        private const string TypeName = "AliPay";
        #endregion 类型名称

        #region 合作者ID
        /// <summary>
        /// 合作者ID
        /// </summary>
        public string PartnerID
        {
            get
            {
                string reVal = ConfigHelper.ReadData(TypeName, "PartnerID");
                return reVal;
            }

            set
            {
                if (value.IsNotNullOrEmpty())
                {
                    ConfigHelper.WriteData(TypeName, "PartnerID", value);
                }
            }
        }
        #endregion 合作者ID

        #region 收款账号
        /// <summary>
        /// 收款账号
        /// </summary>
        public string CollectionAccount
        {
            get
            {
                string reVal = ConfigHelper.ReadData(TypeName, "CollectionAccount");
                return reVal;
            }
            set
            {
                if (value.IsNotNullOrEmpty())
                {
                    ConfigHelper.WriteData(TypeName, "CollectionAccount", value);
                }
            }
        }
        #endregion 收款账号

        #region 安全校验码
        /// <summary>
        /// 安全校验码
        /// </summary>
        public string SecurityCheckCode
        {
            get
            {
                string reVal = ConfigHelper.ReadData(TypeName, "SecurityCheckCode");
                return reVal;
            }
            set
            {
                if (value.IsNotNullOrEmpty())
                {
                    ConfigHelper.WriteData(TypeName, "SecurityCheckCode", value);
                }
            }
        }
        #endregion 安全校验码

        #region 字符编码格式
        /// <summary>
        /// 字符编码格式
        /// </summary>
        public string InputCharset
        {
            get
            {
                string reVal = ConfigHelper.ReadData(TypeName, "InputCharset");
                return reVal;
            }
            set
            {
                if (value.IsNotNullOrEmpty())
                {
                    ConfigHelper.WriteData(TypeName, "InputCharset", value);
                }
            }
        }
        #endregion 字符编码格式

        #region 签名方式
        /// <summary>
        /// 签名方式
        /// </summary>
        public string SignType
        {
            get
            {
                string reVal = ConfigHelper.ReadData(TypeName, "SignType");
                return reVal;
            }
            set
            {
                if (value.IsNotNullOrEmpty())
                {
                    ConfigHelper.WriteData(TypeName, "SignType", value);
                }
            }
        }
        #endregion 签名方式

        #region 服务器异步通知页面路径
        /// <summary>
        /// 服务器异步通知页面路径
        /// </summary>
        public string NotifyUrl
        {
            get
            {
                string reVal = ConfigHelper.ReadData(TypeName, "NotifyUrl");
                reVal = SuperManager.Web.RootUrl + "/" + reVal;
                reVal = reVal.More2One("/");
                return reVal;
            }

            set
            {
                if(value.IsNotNullOrEmpty())
                {
                    ConfigHelper.WriteData(TypeName, "NotifyUrl", value);
                }
            }
        }
        #endregion 服务器异步通知页面路径

        #region 页面跳转同步通知页面路径
        /// <summary>
        /// 页面跳转同步通知页面路径
        /// </summary>
        public string ReturnUrl
        {
            get
            {
                string reVal = ConfigHelper.ReadData(TypeName, "ReturnUrl");
                reVal = SuperManager.Web.RootUrl + "/" + reVal;
                reVal = reVal.More2One("/");
                return reVal;
            }
            set
            {
                if (value.IsNotNullOrEmpty())
                {
                    ConfigHelper.WriteData(TypeName, "ReturnUrl", value);
                }
            }
        }
        #endregion 页面跳转同步通知页面路径
    }
    #endregion 阿里支付配置档
}