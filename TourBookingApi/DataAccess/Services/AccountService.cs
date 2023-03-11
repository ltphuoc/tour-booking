using AutoMapper;
using BusinessObject.Models;
using BusinessObject.UnitOfWork;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{

    public interface IAccountServices
    {
        IEnumerable<Account> GetAll();

        Task<AccountResponse> Create(AccountRequest request);
    }

    public class AccountService : IAccountServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;


        public AccountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }


        public IEnumerable<Account> GetAll()
        {
            var accounts = unitOfWork.Repository<Account>().GetAll().Include(x => x.Bookings);
            return accounts;
        }

        public async Task<AccountResponse> Create(AccountRequest request)
        {
            var account = mapper.Map<Account>(request);
            await unitOfWork.Repository<Account>().InsertAsync(account);
            await unitOfWork.CommitAsync();
            return mapper.Map<AccountResponse>(account);
        }

    }


}
