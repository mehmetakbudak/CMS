using CMS.Business.Helper;
using CMS.Storage.Dtos.Mail;
using MassTransit;
using System.Threading.Tasks;

namespace CMS.Business.Consumers
{
    public class SendEmailConsumer(IMailHelper mailHelper) : IConsumer<MailDto>
    {
        public async Task Consume(ConsumeContext<MailDto> context)
        {
            await mailHelper.Send(context.Message);
        }
    }
}
