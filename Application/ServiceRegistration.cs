using SocialNetwork.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.Core.Application.Interfaces.Services;
using System.Reflection;

namespace SocialNetwork.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            
            #region Services
            services.AddTransient(typeof(IGenericService<,,>), typeof(GenericService<,,>));
            services.AddTransient<IUserServiceIdentity, UserServiceIdentity>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IFriendshipService, FriendshipService>();
            services.AddTransient<IReplyCommentService, ReplyCommentService>();
            services.AddTransient<ICommentService, CommentService>();
            #endregion
        }
    }
}
