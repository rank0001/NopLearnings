using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Data;
using Nop.Plugin.Widgets.CustomTest.Domain;

namespace Nop.Plugin.Widgets.CustomTest.Services;
public class StudentService: IStudentService
{
    #region Fields

    protected readonly IRepository<Student> _studentRepository;

    #endregion

    #region Ctor

    public StudentService(IRepository<Student> studentRepository)
    {
        _studentRepository = studentRepository;
    }

    #endregion

    #region Methods
    public async Task<List<Student>> GetStudentListAsync()
    {
        var query = _studentRepository.Table;

        return await query.ToListAsync();
    }

    #endregion
}
