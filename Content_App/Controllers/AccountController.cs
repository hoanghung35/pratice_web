
﻿using System.Security.Claims;
using Content_App.App.DTOs.Auth;
using Content_App.App.Services;
using Content_App.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Content_App.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : Controller
    {
       private readonly AccountService _accountService;
       private readonly MailService _mailService;

       public AccountController(AccountService accountService, MailService mailService) {
           this._accountService = accountService;
           this._mailService = mailService;
       }

       [Authorize(Policy="dev")]
       [HttpGet]
       public async Task<IActionResult> GetAccount() {
           var userId = new Guid(User.FindFirst(JwtClaimConstant.UserId)!.Value!);

           return Ok(await _accountService.GetAccountServiceAsync(userId));
       }

       [Authorize(Policy = "dev")]
       [HttpPost("create-account")]
       public async Task<IActionResult> CreateAccount([FromBody] AccountDto dto) {
           var userName = User.FindFirst(JwtClaimConstant.UserName)?.Value!; //fullName
           var newPw = await _accountService.CreateAccountAsync(dto);
           var content = _mailService.CreateMailFull(dto.fullName, dto.userCode, newPw, true);
           var msg = _mailService.CreateMailFull([dto.email], "[Notification] ACCOUNT INFORMATION", content, [], [], []);
           await _mailService.SendMail(msg);

           return Ok();
       }

       [Authorize(Policy = "dev")]
       [HttpDelete("delete-account/{accountId}")]
       public async Task<IActionResult> DeleteAccount(Guid accountId) {
           await _accountService.DeleteAccountAsync(accountId);

           return Ok();
       }

       [Authorize]
       [HttpPut("change-profile")]
       public async Task<IActionResult> ChangeProfile(ChangeUserInforDto dto) {
           var accountId = User.FindFirst(JwtClaimConstant.PicId)?.Value!;
           await _accountService.ChangeProfileAsync(dto, new Guid(accountId));

           return Ok();
       }
    }
}
