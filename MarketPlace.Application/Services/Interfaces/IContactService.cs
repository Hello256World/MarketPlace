using MarketPlace.DataLayer.DTOs.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services.Interfaces
{
    public interface IContactService : IAsyncDisposable
    {
        #region contact us

        Task CreateContactUs(CreateContactUsDTO create, long? userId, string userIp);

        #endregion

        #region ticket

        Task<TicketResult> AddUserTicket(AddTicketDTO ticket, long userId);
        Task<FilterTicketDTO> FilterTickets(FilterTicketDTO filter);
        Task<TicketDetailDTO> GetTicketDetail(long ticketId, long userId);
        Task<AnswerTicketResult> AddAnswerTicket(AnswerTicketDTO answer, long userId);

        #endregion
    }
}
