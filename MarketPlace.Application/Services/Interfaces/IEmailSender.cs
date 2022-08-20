using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services.Interfaces
{
    public interface IEmailSender
    {
        Task SendMail(string to, string code, string subject, string text);
    }
}
