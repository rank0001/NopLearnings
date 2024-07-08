using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Widgets.CustomTest.Domain;
namespace Nop.Plugin.Widgets.CustomTest.Services;
public interface IStudentService
{
    Task<List<Student>> GetStudentListAsync();
}
