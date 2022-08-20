using MarketPlace.Application.Extensions;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.DataLayer.DTOs.Contacts;
using MarketPlace.DataLayer.DTOs.Paging;
using MarketPlace.DataLayer.Entities.Account;
using MarketPlace.DataLayer.Entities.Contacts;
using MarketPlace.DataLayer.Repository;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Application.Services.Implementations
{
    public class ContactService : IContactService
    {
        #region constructor

        private readonly IGenericRepository<ContactUs> _contactUsRepository;
        private readonly IGenericRepository<Ticket> _ticketRepository;
        private readonly IGenericRepository<TicketMessage> _ticketMessageRepository;

        public ContactService(IGenericRepository<ContactUs> contactUsRepository, IGenericRepository<Ticket> ticketRepository, IGenericRepository<TicketMessage> ticketMessageRepository, IGenericRepository<User> userRepository)
        {
            _contactUsRepository = contactUsRepository;
            _ticketMessageRepository = ticketMessageRepository;
            _ticketRepository = ticketRepository;
        }

        #endregion

        #region contact us

        public async Task CreateContactUs(CreateContactUsDTO create, long? userId, string userIp)
        {
            var newContactUs = new ContactUs
            {
                UserId = userId != null && userId.Value != 0 ? userId : (long?)null,
                UserIp = userIp,
                Email = create.Email,
                FullName = create.FullName,
                Subject = create.Subject,
                Text = create.Text
            };

            await _contactUsRepository.AddEntity(newContactUs);
            await _contactUsRepository.SaveChanges();
        }

        #endregion

        #region ticket

        public async Task<TicketResult> AddUserTicket(AddTicketDTO ticket, long userId)
        {
            if (string.IsNullOrEmpty(ticket.Text)) return TicketResult.Error;

            // add ticket
            var newTicket = new Ticket
            {
                OwnerId = userId,
                Title = ticket.Title,
                TicketState = TicketState.UnderProgress,
                TicketPriority = ticket.TicketPriority,
                TicketSection = ticket.TicketSection,
                IsReadByOwner = true,
            };

            await _ticketRepository.AddEntity(newTicket);
            await _ticketRepository.SaveChanges();

            // add ticket message
            var newTicketMessage = new TicketMessage
            {
                SenderId = userId,
                Text = ticket.Text,
                TicketId = newTicket.Id
            };

            await _ticketMessageRepository.AddEntity(newTicketMessage);
            await _ticketMessageRepository.SaveChanges();

            return TicketResult.Success;
        }

        public async Task<FilterTicketDTO> FilterTickets(FilterTicketDTO filter)
        {
            var query = _ticketRepository.GetQuery();

            #region state

            switch (filter.FilterTicketState)
            {
                case FilterTicketState.All:
                    break;
                case FilterTicketState.NotDeleted:
                    query = query.Where(x => !x.IsDelete);
                    break;
                case FilterTicketState.Deleted:
                    query = query.Where(x => x.IsDelete);
                    break;
            }

            switch (filter.OrderBy)
            {
                case FilterTicketOrder.CreateDate_DES:
                    query = query.OrderByDescending(x => x.CreateDate);
                    break;
                case FilterTicketOrder.CreateDate_ASC:
                    query = query.OrderBy(x => x.CreateDate);
                    break;
            }

            #endregion

            #region filter

            if (filter.TicketPriority != null)
                query = query.Where(x => x.TicketPriority == filter.TicketPriority.Value);

            if (filter.TicketSection != null)
                query = query.Where(x => x.TicketSection == filter.TicketSection.Value);

            if (filter.UserId != null && filter.UserId != 0)
                query = query.Where(x => x.OwnerId == filter.UserId.Value);

            if (!string.IsNullOrEmpty(filter.Title))
                query = query.Where(x => EF.Functions.Like(x.Title, $"%{filter.Title}%"));

            #endregion

            #region paging

            var pager = Pager.Builder(filter.PageId, filter.AllEntitiesCount, await query.CountAsync(), filter.HowManyShowPageAfterAndBefore);
            var allTickets = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetTickets(allTickets);
        }

        public async Task<TicketDetailDTO> GetTicketDetail(long ticketId, long userId)
        {
            var ticket = await _ticketRepository.GetQuery().Include(x => x.Owner).SingleOrDefaultAsync(x => x.Id == ticketId);

            if (ticket == null || ticket.OwnerId != userId) return null;

            return new TicketDetailDTO
            {
                Ticket = ticket,
                TicketMessages = await _ticketMessageRepository.GetQuery().OrderByDescending(x => x.CreateDate)
                .Where(x => x.TicketId == ticketId && !x.IsDelete).ToListAsync(),
            };
        }

        public async Task<AnswerTicketResult> AddAnswerTicket(AnswerTicketDTO answer, long userId)
        {
            var ticket = await _ticketRepository.GetEntityById(answer.Id);

            if (ticket == null) return AnswerTicketResult.NotFound;

            if (ticket.OwnerId != userId) return AnswerTicketResult.NotForUser;

            var newTicketMessage = new TicketMessage
            {
                SenderId = userId,
                TicketId = answer.Id,
                Text = answer.Text.Sanitize()
            };

            await _ticketMessageRepository.AddEntity(newTicketMessage);
            await _ticketMessageRepository.SaveChanges();

            ticket.IsReadByOwner= true;
            ticket.IsReadByAdmin = false;

            _ticketRepository.EditeEntity(ticket);
            await _ticketRepository.SaveChanges();

            return AnswerTicketResult.Success;
        }

        #endregion

        #region dispose

        public async ValueTask DisposeAsync()
        {
            if (_contactUsRepository != null) await _contactUsRepository.DisposeAsync();
            if (_ticketMessageRepository != null) await _ticketMessageRepository.DisposeAsync();
            if (_ticketRepository != null) await _ticketRepository.DisposeAsync();
        }

        #endregion
    }
}
