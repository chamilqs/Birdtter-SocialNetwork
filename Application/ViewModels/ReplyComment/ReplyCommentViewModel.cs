using SocialNetwork.Core.Application.ViewModels.AAA_Common;

namespace SocialNetwork.Core.Application.ViewModels.ReplyComment
{
    public class ReplyCommentViewModel : UserProperties
    {
        public string Content { get; set; }        
        public int CommentId { get; set; }
        public string CommentUserId { get; set; }
    }
}
