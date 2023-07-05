using CinemaTicketOffice.Domain.Email;
using CinemaTicketOffice.Repository.Interface;
using CinemaTicketOffice.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaTicketOffice.Service.Implementation
{
    public class BackgroundEmailSender : IBackgroundEmailSender
    {

        private readonly IEmailService _emailService;
        private readonly IRepository<EmailMessage> _mailRepository;

        public BackgroundEmailSender(IEmailService emailService, IRepository<EmailMessage> mailRepository)
        {
            _emailService = emailService;
            _mailRepository = mailRepository;
        }
        public async Task DoWork()
        {
            List<EmailMessage> messages = _mailRepository.GetAll().Where(z => !z.Status).ToList();
            await _emailService.SendEmailAsync(messages);
            foreach (EmailMessage message in messages)
            {
                message.Status = true;
                _mailRepository.Update(message);
            }
        }
    }
}
