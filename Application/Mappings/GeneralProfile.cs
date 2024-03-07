using AutoMapper;
using SocialNetwork.Core.Application.ViewModels.SocialLink;
using SocialNetwork.Core.Application.ViewModels.User;
using SocialNetwork.Core.Domain.Entities;
using SocialNetwork.Core.Application.DTOs.Account;
using SocialNetwork.Core.Application.ViewModels.Post;
using SocialNetwork.Core.Application.ViewModels.Comment;
using SocialNetwork.Core.Application.ViewModels.ReplyComment;
using SocialNetwork.Core.Application.ViewModels.Friendship;

namespace SocialNetwork.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region PostProfile
            CreateMap<Post, PostViewModel>()
            .ForMember(x => x.UserName, opt => opt.Ignore())
            .ForMember(x => x.UserUsername, opt => opt.Ignore())
            .ForMember(x => x.UserLastName, opt => opt.Ignore())
            .ForMember(x => x.UserProfilePicture, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(x => x.Created, opt => opt.Ignore())
            .ForMember(x => x.CreatedBy, opt => opt.Ignore())
            .ForMember(x => x.LastModified, opt => opt.Ignore())
            .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());

            CreateMap<Post, SavePostViewModel>()
            .ReverseMap()
            .ForMember(x => x.Comments, opt => opt.Ignore())
            .ForMember(x => x.Created, opt => opt.Ignore())
            .ForMember(x => x.CreatedBy, opt => opt.Ignore())
            .ForMember(x => x.LastModified, opt => opt.Ignore())
            .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());
            #endregion

            #region CommentProfile
            CreateMap<Comment, CommentViewModel>()
            .ForMember(x => x.UserName, opt => opt.Ignore())
            .ForMember(x => x.UserUsername, opt => opt.Ignore())
            .ForMember(x => x.UserLastName, opt => opt.Ignore())
            .ForMember(x => x.UserProfilePicture, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(x => x.Created, opt => opt.Ignore())
            .ForMember(x => x.CreatedBy, opt => opt.Ignore())
            .ForMember(x => x.LastModified, opt => opt.Ignore())
            .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());

            CreateMap<Comment, SaveCommentViewModel>()
            .ReverseMap()
            .ForMember(x => x.Replies, opt => opt.Ignore())
            .ForMember(x => x.Created, opt => opt.Ignore())
            .ForMember(x => x.CreatedBy, opt => opt.Ignore())
            .ForMember(x => x.LastModified, opt => opt.Ignore())
            .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());
            #endregion

            #region ReplyCommentProfile
            CreateMap<ReplyComment, ReplyCommentViewModel>()
            .ForMember(x => x.UserName, opt => opt.Ignore())
            .ForMember(x => x.UserUsername, opt => opt.Ignore())
            .ForMember(x => x.UserLastName, opt => opt.Ignore())
            .ForMember(x => x.UserProfilePicture, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(x => x.Created, opt => opt.Ignore())
            .ForMember(x => x.CreatedBy, opt => opt.Ignore())
            .ForMember(x => x.LastModified, opt => opt.Ignore())
            .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());

            CreateMap<ReplyComment, SaveReplyCommentViewModel>()
            .ReverseMap()
            .ForMember(x => x.Created, opt => opt.Ignore())
            .ForMember(x => x.CreatedBy, opt => opt.Ignore())
            .ForMember(x => x.LastModified, opt => opt.Ignore())
            .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());
            #endregion

            #region UserProfileIdentity
            CreateMap<AuthenticationRequest, LoginIdentityViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<RegisterRequest, SaveIdentityUserViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ForgotPasswordRequest, ForgotPasswordViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ResetPasswordRequest, ResetPasswordViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();
            #endregion

            #region FriendshipProfile
            CreateMap<Friendship, FriendshipViewModel>()
                .ForMember(x => x.UserUsername, opt => opt.Ignore())
                .ForMember(x => x.UserName, opt => opt.Ignore())
                .ForMember(x => x.UserLastName, opt => opt.Ignore())
                .ForMember(x => x.UserProfilePicture, opt => opt.Ignore())
                .ForMember(x => x.FriendUsername, opt => opt.Ignore())
                .ForMember(x => x.FriendName, opt => opt.Ignore())
                .ForMember(x => x.FriendLastName, opt => opt.Ignore())
                .ForMember(x => x.FriendProfilePicture, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModified, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());

            CreateMap<Friendship, SaveFriendshipViewModel>()
                .ReverseMap()
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModified, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());
            #endregion


        }
    }
}
