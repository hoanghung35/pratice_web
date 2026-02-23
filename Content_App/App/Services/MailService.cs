using Content_App.App.DTOs.User;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace Content_App.App.Services
{
    public class MailService
    {
        public async Task SendMail(MailMessage msg)
        {
            try
            {
                MailDto cs = new MailDto
                {
                    MailFromAddr = msg.From.Address,
                    MailFromName = msg.From.DisplayName,
                    MailBody = msg.Body,
                    Subject = msg.Subject,
                    IsBodyHtml = msg.IsBodyHtml,
                    MailBcc = msg.Bcc.Select(x => x.Address).ToArray(),
                    MailTo = msg.To.Select(x => x.Address).ToArray(),
                    MailCc = msg.CC.Select(x => x.Address).ToArray()
                };

                if(msg.Attachments.Count() > 0)
                {
                    var listAtt = new List<byte[]>();
                    var listAttN = new List<string>();

                    foreach(var att in msg.Attachments)
                    {
                        using (var mem = new MemoryStream())
                        {
                            att.ContentStream.CopyTo(mem);
                            listAtt.Add(mem.ToArray());
                            listAttN.Add(att.ContentDisposition.FileName);
                        }
                    }
                    cs.Attachment = listAtt;
                    cs.FileName = listAttN;

                    using (HttpClient client = new HttpClient())
                    {
                        string url = "http://10.16.1.32:5109/api";
                        var option = new JsonSerializerOptions()
                        {
                            WriteIndented = true
                        };

                        string json = JsonSerializer.Serialize(cs, option);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PostAsync(url, content);
                    }
                }
            }
            catch(Exception e)
            {

            }
        }

        public MailMessage CreateMailFull(List<string> recipients, string subject, string content, List<byte[]> attachmentData, List<string>attachmentContentType, List<string> attachmentFile)
        {
            MailMessage mailmsg = new MailMessage
            {
                From = new MailAddress("lmsc@local.canon-vn.com.vn", "LMSC SYSTEM")
            };

            //send to
            recipients.ForEach(recepient =>
            {
                mailmsg.To.Add(recepient);
            });

            if(attachmentData != null)
            {
                for(int i = 0; i < attachmentData.Count(); i++)
                {
                    var att = attachmentData[i];
                    MemoryStream memoryStream = new MemoryStream(att);
                    Attachment attachment = new Attachment(memoryStream, new ContentType(attachmentContentType[i]));
                    attachment.ContentDisposition.FileName = attachmentFile[i];
                    mailmsg.Attachments.Add(attachment);
                }
            }

            mailmsg.Body = content;
            mailmsg.Subject = subject;
            mailmsg.IsBodyHtml = true;
            mailmsg.Priority = MailPriority.Normal;

            return mailmsg;
        }
    }
}
