using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models.Helpers;

namespace WebApplication5.Models.ViewModels
{
    public static class ViewModelHelpers
    {
        public static UserProfileEditViewModel ToViewModel(this UserProfileInfo userProfile)
        {
            var userProfileViewModel = new UserProfileEditViewModel
            {
                UserProfileID = userProfile.Id
            };

         

            return userProfileViewModel;
        }

        public static UserProfileEditViewModel ToViewModel(this UserProfileInfo userProfile, ICollection<Position> allDbPositions, ICollection<Language> allDbLanguages)
        {
            var userProfileViewModel = new UserProfileEditViewModel
            {
               
                UserProfileID = userProfile.Id,
                avatar_url = userProfile.avatar_url,
                avatar_url2 = userProfile.avatar_url2,
                steam_url = userProfile.steam_url,
                Username = userProfile.UserName,
                Email = userProfile.Email,
                MinRating = userProfile.MinRating,
                Rating = userProfile.Rating,
                MaxRating = userProfile.MaxRating,
                TimeZone = userProfile.TimeZone,
                MinTime = userProfile.MinTime,
                MaxTime = userProfile.MaxTime,
                Microphone = userProfile.Microphone,
                Goal = userProfile.Goal,
                FavPosition = userProfile.FavPosition,
                MinAge=userProfile.MinAge,
                Age = userProfile.Age,
                MaxAge = userProfile.MaxAge,
                About = userProfile.About,
                Search=userProfile.Search,

            };

            ICollection<AssignedPositionData> allPositions = new List<AssignedPositionData>();

            foreach (var c in allDbPositions)
            {

                var assignedPosition = new AssignedPositionData
                {
                    PositionID = c.PositionID,
                    PositionNumber = c.PositionNumber,
                    Assigned = userProfile.Positions.FirstOrDefault(x => x.PositionID == c.PositionID) != null
                };

                allPositions.Add(assignedPosition);
            }



            userProfileViewModel.Positions = allPositions;
            userProfileViewModel.LangPopis = GetLanguages(userProfile,allDbLanguages);

            return userProfileViewModel;
        }

        public static UserProfileInfo ToDomainModel(this UserProfileEditViewModel userProfileViewModel)
        {
            var userProfile = new UserProfileInfo
            {
                Id = userProfileViewModel.UserProfileID
            };

            return userProfile;
        }

        public static MultiSelectList GetLanguages(UserProfileInfo userProfile,ICollection<Language> lang)
        {

            List<Language> Languagess = lang.ToList();
            List<int> SV = new List<int>();
            foreach (Language l in userProfile.Languages.ToList()) { SV.Add(l.LanguageID); }

            return new MultiSelectList(Languagess, "LanguageID", "Jezik", SV);
        }

      


    }
}