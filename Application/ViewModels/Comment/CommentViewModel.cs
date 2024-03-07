using SocialNetwork.Core.Application.ViewModels.AAA_Common;
using SocialNetwork.Core.Application.ViewModels.ReplyComment;

namespace SocialNetwork.Core.Application.ViewModels.Comment
{
    public class CommentViewModel : UserProperties
    {       
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int PostId { get; set; }
        public ICollection<ReplyCommentViewModel>? Replies { get; set; }
    }
}
