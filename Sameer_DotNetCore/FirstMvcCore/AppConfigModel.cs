using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstMvcCore
{
    public class AppConfigModel
    {
        public string Company { get; set; }
        public string ProjectName { get; set; }
        public string Duration { get; set; }
        public string ASPNETCORE_ENVIRONMENT { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }
        public string Message { get; set; }
        public Address Address { get; set; }
        public Course Course { get; set; }

    }

    public class Address
    {

        public string City { get; set; }
        public string BuildingNo { get; set; }
    }

    public class Course
    {

        public string Title { get; set; }
        public int TotalCount { get; set; }
    }
}
