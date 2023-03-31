using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace CRUD_First.Models
{
    public class MinAgeAttribute : ValidationAttribute
    {
        int _minimumAge;
        public MinAgeAttribute(int minimumAge)
        {
            _minimumAge = minimumAge;
        }
        public override bool IsValid(object value)
        {
            DateTime insertedDate;
            DateTime dateAfterAddedYears;
            if(value == null)
            {
                return true;
            }
            if(value != null)
            {
                insertedDate = Convert.ToDateTime(value.ToString());
                dateAfterAddedYears = insertedDate.AddYears(_minimumAge);
                var actualAge = DateTime.Now.Year - insertedDate.Year;

                if (Convert.ToInt32(actualAge) >= 18)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
