namespace SocialNetwork.Core.Application.ViewModels.ReplyComment
{
    public class SaveReplyCommentViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
    }
}
