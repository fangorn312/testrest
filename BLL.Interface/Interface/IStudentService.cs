using System;
using System.Collections.Generic;
using System.Text;
using BLL.Interface.Dto;

namespace BLL.Interface.Interface
{
    public interface IStudentService : ICrudService<StudentDto, int>
    {
    }
}
