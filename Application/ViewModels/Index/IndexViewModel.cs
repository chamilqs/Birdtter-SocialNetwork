using SocialNetwork.Core.Application.DTOs.Account;
using SocialNetwork.Core.Application.ViewModels.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.NewFolder
{
    public class IndexViewModel
    {
        public SavePostViewModel SavePostViewModel { get; set; }
        public List<PostViewModel> ListPostViewModel { get; set; }
    }
}
