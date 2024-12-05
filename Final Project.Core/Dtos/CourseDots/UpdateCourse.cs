using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Dtos.CourseDtos
{
    public class UpdateCourseDto : CreateCourseDto
    {
        public int CourseId { get; set; }
    }

}