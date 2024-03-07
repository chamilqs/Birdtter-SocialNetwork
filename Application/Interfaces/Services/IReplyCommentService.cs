using SocialNetwork.Core.Application.ViewModels.ReplyComment;
using SocialNetwork.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.Interfaces.Services
{
    public interface IReplyCommentService : IGenericService<SaveReplyCommentViewModel, ReplyCommentViewModel, ReplyComment>
    {

    }
}
