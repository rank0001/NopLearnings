using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Blogs;
using Nop.Plugin.Widgets.CustomTest.Domain;
namespace Nop.Plugin.Widgets.CustomTest.Services;
public interface IStudentService
{
    Task<List<Student>> GetStudentListAsync();
    Task InsertStudentAsync(Student student);
    Task InsertEventTestAsync(EventTest eventTest);
    Task<Student> GetStudentPostByIdAsync(int blogPostId);
    Task DeleteStudentPostAsync(Student student);
    Task UpdateStudentPostAsync(Student student);
    Task<IPagedList<Student>> GetAllStudentPostsAsync(
    int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false, string name = null, int age = 0);
}
