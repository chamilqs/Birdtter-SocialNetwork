using SocialNetwork.Core.Application.ViewModels.AAA_Common;

namespace SocialNetwork.Core.Application.ViewModels.ReplyComment
{
    public class ReplyCommentViewModel : UserProperties
    {
        public string Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int CommentId { get; set; }
    }
}
