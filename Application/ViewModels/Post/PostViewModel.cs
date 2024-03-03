using SocialNetwork.Core.Application.ViewModels.AAA_Common;
using SocialNetwork.Core.Application.ViewModels.Comment;

namespace SocialNetwork.Core.Application.ViewModels.Post
{
    public class PostViewModel : UserProperties
    {
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<CommentViewModel>? Comments { get; set; }
    }
}
