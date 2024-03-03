using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Core.Application.ViewModels.SocialLink
{
    public class SaveSocialLinkViewModel
    {
        public int Id { get; set; }
        [DataType(DataType.Url)]
        public string? Facebook { get; set; }
        [DataType(DataType.Url)]
        public string? Twitter { get; set; }
        [DataType(DataType.Url)]
        public string? Instagram { get; set; }
        [DataType(DataType.Url)]
        public string? LinkedIn { get; set; }
        [DataType(DataType.Url)]
        public string? YouTube { get; set; }
        [DataType(DataType.Url)]
        public string? GitHub { get; set; }
        [DataType(DataType.Url)]
        public string? Website { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
