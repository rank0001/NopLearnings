using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Blogs;
using Nop.Data;
using Nop.Plugin.Widgets.CustomTest.Domain;

namespace Nop.Plugin.Widgets.CustomTest.Services;
public class StudentService : IStudentService
{
    #region Fields

    protected readonly IRepository<Student> _studentRepository;
    protected readonly IRepository<EventTest> _eventTestRepository;

    #endregion

    #region Ctor

    public StudentService(IRepository<Student> studentRepository, IRepository<EventTest> eventTestRepository)
    {
        _studentRepository = studentRepository;
        _eventTestRepository = eventTestRepository;
    }

    #endregion

    #region Methods
    public async Task<List<Student>> GetStudentListAsync()
    {
        var query = _studentRepository.Table;

        return await query.ToListAsync();
    }

    public virtual async Task InsertStudentAsync(Student student)
    {
        await _studentRepository.InsertAsync(student);
    }

    public virtual async Task<Student> GetStudentPostByIdAsync(int studentId)
    {
        return await _studentRepository.GetByIdAsync(studentId, cache => default, useShortTermCache: true);
    }

    public virtual async Task DeleteStudentPostAsync(Student student)
    {
        await _studentRepository.DeleteAsync(student);
    }

    public virtual async Task UpdateStudentPostAsync(Student student)
    {
        await _studentRepository.UpdateAsync(student);
    }

    public virtual async Task InsertEventTestAsync(EventTest eventTest)
    {
        await _eventTestRepository.InsertAsync(eventTest);
    }

    public virtual async Task<IPagedList<Student>> GetAllStudentPostsAsync(
    int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false, string name = null, int age = 0)
    {
        return await _studentRepository.GetAllPagedAsync(query =>
        {
            if (!string.IsNullOrEmpty(name))
                query = query.Where(b => b.Name.Contains(name));

            if (age > 0)
                query = query.Where(x => x.Age == age);

            return query;
        }, pageIndex, pageSize);
    }
    #endregion
}
