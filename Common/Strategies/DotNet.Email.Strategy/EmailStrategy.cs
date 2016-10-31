using Super.Framework;
using System;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace DotNet.Email.Strategy
{
    #region 邮件策略
    /// <summary>
    /// 邮件策略
    /// </summary>
    [StrategyCode("Email")]
    [StrategyTitle("使用.NET内置功能的邮件策略")]
    [StrategyVersion("1.0")]
    public class EmailStrategy : IEmailStrategy
    {
        #region 发送邮件
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="InputConfig">邮件配置</param>
        /// <param name="InputReceiveInfo">邮件接收信息</param>
        /// <returns>返回值：发送是否成功</returns>
        public bool Send(EmailConfig InputConfig, EmailReceiveInfo InputReceiveInfo)
        {
            bool reVal = false;
            try
            {
                MailMessage MM = new MailMessage
                {
                    IsBodyHtml = InputReceiveInfo.IsHTML,
                    Subject = InputReceiveInfo.Subject,
                    Body = InputReceiveInfo.Content,
                    From = new MailAddress(InputConfig.From, InputConfig.FromName)
                };
                if (!InputConfig.ReplayTo.IsNullOrEmpty())
                {
                    MM.ReplyToList.Add(
                        new MailAddress(InputConfig.ReplayTo, InputConfig.FromName));
                }
                Regex regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                for (int i = 0; i < InputReceiveInfo.ReceiverList.Count; i++)
                {
                    if (regex.IsMatch(InputReceiveInfo.ReceiverList[i].Address))
                    {
                        MM.To.Add(
                            new MailAddress(
                                InputReceiveInfo.ReceiverList[i].Address,
                                InputReceiveInfo.ReceiverList[i].NickName)
                                );
                    }
                }
                if (MM.To.Count == 0)
                {
                    return false;
                }
                SmtpClient client = new SmtpClient
                {
                    EnableSsl = InputConfig.ISSSL,
                    UseDefaultCredentials = true
                };
                NetworkCredential NC = new NetworkCredential(InputConfig.UserName, InputConfig.Password);
                client.Credentials = NC;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Host = InputConfig.Host;
                client.Port = InputConfig.Port;
                client.Send(MM);
                reVal = true;
            }
            catch (Exception ex)
            {
                ex.Data.Add("Email Subject", InputReceiveInfo.Subject);
                throw ex;
            }
            return reVal;
        }
        #endregion 发送邮件
    }
    #endregion 邮件策略
}