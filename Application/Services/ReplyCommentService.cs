using AutoMapper;
using Microsoft.AspNetCore.Http;
using SocialNetwork.Core.Application.DTOs.Account;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Application.Interfaces.Repositories;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.ViewModels.Comment;
using SocialNetwork.Core.Application.ViewModels.ReplyComment;
using SocialNetwork.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.Services
{
    public class ReplyCommentService : GenericService<SaveReplyCommentViewModel, ReplyCommentViewModel, ReplyComment>, IReplyCommentService
    {
        private readonly IReplyCommentRepository _replyrepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse userViewModel;
        private readonly IMapper _mapper;

        public ReplyCommentService(IReplyCommentRepository replyrepository, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(replyrepository, mapper)
        {
            _replyrepository = replyrepository;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _mapper = mapper;
        }

        public override async Task<SaveReplyCommentViewModel> Add(SaveReplyCommentViewModel vm)
        {
            vm.UserId = userViewModel.Id;
            return await base.Add(vm);
        }
    }
}
