using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.Enums;
using static System.Net.WebRequestMethods;

namespace SharpForms.Api.DAL.Seeds;

public class UserSeeds
{
    public static UserEntity User1 => new UserEntity
    {
        Id = new Guid("26744e13-77c9-49bf-90cd-0310e379e46d"), Name = "John Doe", Role = UserRole.General, PhotoUrl = "https://oodp.ca/media/tutor-8.jpg",
    };

    public static UserEntity User2 => new UserEntity
    {
        Id = new Guid("eebf7395-5e10-4cc5-8c10-a05a0c0f8783"), Name = "Jane Doe", Role = UserRole.General, PhotoUrl = "https://yt3.googleusercontent.com/ytc/AIdro_mW7V85vFJSZICozJ-x8A9bWlztOnCopWPgERbIvec_6Ls=s900-c-k-c0x00ffffff-no-rj",
    };

    public static UserEntity User3 => new UserEntity
    {
        Id = new Guid("602344a1-3fa0-4c33-befe-00c984cd27db"), Name = "Brock Samson", Role = UserRole.General, PhotoUrl = "https://www.looktothestars.org/photo/4318-patrick-warburton/story_half_width.jpg"
    };

    public static UserEntity User4 => new UserEntity
    {
        Id = new Guid("c6640bc2-87f0-46ac-ac76-1268be75c970"), Name = "Jeremy Osborne", Role = UserRole.General, PhotoUrl = "https://mmbcreative.com/app/uploads/2016/08/Robert-Webb.jpg"
    };

    public static UserEntity User5 => new UserEntity
    {
        Id = new Guid("3b9387a5-491c-413c-a093-67be1cd3d4fd"), Name = "Admin", Role = UserRole.Admin, PhotoUrl = "https://i.pinimg.com/originals/6a/44/f0/6a44f0e35b10e6ed063eeebf7ed844f9.jpg"
    };

    public static IList<UserEntity> SeededUsers =>
        new List<UserEntity>()
        {
            User1,
            User2,
            User3,
            User4,
            User5,
        };
}
