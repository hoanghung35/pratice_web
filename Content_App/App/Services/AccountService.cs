﻿using Content_App.App.DTOs.Item;
using Content_App.Infrastructure.Data;
using Content_App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Content_App.App.Services
{
    public class AccountService
    {
        private readonly LogDbContext _context;
        private readonly PasswordHasher _haser;

        public AccountService(LogDbContext context, PasswordHasher hasher)
        {
            this._context = context;
            this._haser = hasher;
        }

       public async Task<List<AccountDto>> GetAccountAsync(Guid userId)
       {
           var accounts = await _context.Accounts
               .Join(_context.Employees,
                    account => account.UserCode,
                    emp => emp.EmpCode,
                    (account, emp) => new {account, emp})
               .Where(tmp => tmp.account.DeletedAt == null)
               .Join(_context.Roles,
                    tmp => tmp.account.RoleId,
                    role => role.Id,
                    (tmp, role) => new {
                      userId = tmp.emp.Id,
                      id = tmp.account.Id,
                      code = tmp.emp.Empcode,
                      email = tm.emp.Email,
                      fullname = tmp.emp.Fullname,
                      role = role.Description!
                    })
               .ToListAsync();

           if(accounts == null) {
               throw new NullReferenceException();
           }

         List<AccountDto> list_account = new List<AccountDto> { };

         foreach (var acc in accounts) {
             if(acc.userId != userId)
             {
                 list_account.Add(new AccountDto
                  {
                      id = acc.id,
                    userCode = acc.code,
                    email = acc.email!,
                    fullName = acc.fullname,
                    roleName = acc.role
                  });
             }
         }

         return list_account;
       }

      public async Task ChangeProfileAsync(ChangeUserInforDto dto, Guid accountId)
      {
          var account = await _context.Accounts.FindAsync(accountId);

          if(account == null)
          {
              throw new KeyNotFoundException();
          }

          if(!_hasher.Verify(dto.oldPassWord, account.Password))
          {
              var error = new {message = "Password wrong"};
              throw new InvalidOperationException();
          }

          //update new profile
          account.Password = _hasher.SHA_256Hasher(dto.newPassWord);
          _context.Accounts.Update(account);

          await _context.SaveChangesAsync();
      }

      public async Task ResetAccountAsync(Guid accountId)
      {
          var account = await _context.Accounts.FindAsync(accountId);

          if(account == null)
          {
              throw new NullReferenceException();
          }

          account.Password = _hasher.SHA256Hasher("123");
          _context.Accounts.Update(account);
          await _context.SaveChangesAsync();
      }

      public async Task<string> CreateAccountAsync(AccountInitDto dto)
      {
          //check user || account existed
          var user = await _context.Employees.Where(u => u.EmpCode == dto.userCode).ToListAsync();
          var account = await _context.Accounts.Where(a => a.UserCode == dto.UserCode).ToListAsync();

          if(account.Count != 0)
          {
              throw new InvalidOperationException("Data exist!");
          }

          var newPw = _hasher.RandomPassword();
          var role = await _context.Roles.Where(r => r.Descroption!.ToLower() == dto.role.ToLower()).FirstOrDefaultAsync();

          if(user.Count() == 0)
          {
              //create employee when not exist in member list
              var dept = await _context.Departments.Where(d => d.DeptName.ToLower() == dto.dept.ToLower()).FirstOrDefaultAsync();
              var area = await _context.Areas.Where(a => a.AreaName.ToLower() == dto.area.ToLower()).FirstOrDefaultAsync();
              _context.Employees.Add(new Employee
              {
                  EmpCode = dto.userCode,
                  Fullname = dto.fullName,
                  Email = dto.email,
                  DeptId = dept!.Id,
                  AreaId = area!.Id,
                  Grade = ""
              });
          }

          //add new account
            _context.Accounts.Add(new Account
            {
                UserCode = dto.userCode,
                Password = _hasher.SHA_256Hasher(newPw),
                RoleId = role!.Id
            });

            await _context.SaveChangesAsync();

            return newPw;
      }

      public async Task DeleteAccountAsync(Guid accountId)
      {
          var account = await _context.Accounts.FindAsync(accountId);

          if(account == null)
          {
              throw new NullReferenceException();
          }

          account.DeleteAt = DateTime.SpecifyKind(DateTime.UtcNow.AddHours(7), DateTimeKind.Unspecified);

          _context.Accounts.Update(account);
          await _context.SaveChangesAsync();
      }
    }
}
