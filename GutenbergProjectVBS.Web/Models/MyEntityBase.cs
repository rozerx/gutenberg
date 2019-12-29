using System;

namespace GutenbergProjectVBS.Web.Models
{
    public class MyEntityBase
    {
        public DateTime CreatedAt { get; set; }

        public Nullable<DateTime> UpdatedAt { get; set; }

        public Nullable<DateTime> DeletedAt { get; set; }
    }
}