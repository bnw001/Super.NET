using Super.Framework;

namespace YiChi.SMS.Strategy
{
    [StrategyCode("SMS")]
    [StrategyTitle("厦门易驰软件有限公司的短信策略")]
    [StrategyVersion("1.0")]
    public class SMSStrategy : ISMSStrategy
    {
        #region 发送短信
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="InputConfig">配置档</param>
        /// <param name="InputReceiverInfo">短信接收信息</param>
        public bool Send(SMSConfig InputConfig, SMSReceiveInfo InputReceiverInfo)
        {
            bool reVal = false;
            string Url=InputConfig.Url+"?suser=@UserName&spass=@Password&telnum=@Tel&nr=@Content";
            string ReceverNum=string.Empty;
            foreach(SMSReceiver Receiver in InputReceiverInfo.ReceiverList)
            {
                ReceverNum += Receiver.PhoneNum + ",";
            }
            if(!ReceverNum.IsNullOrEmpty())
            {
                ReceverNum=ReceverNum.RemoveEndChar(",");
                Url = Url
                    .Replace("@UserName", InputConfig.UserName)
                    .Replace("@Password", InputConfig.Password)
                    .Replace("@Tel", ReceverNum)
                    .Replace("@Content", InputReceiverInfo.Content);
            }
            return reVal;
        }
        #endregion 发送短信
    }
}