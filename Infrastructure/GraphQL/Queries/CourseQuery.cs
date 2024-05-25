using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Infrastructure.GraphQL.Queries;

public class CourseQuery(ICourseService courseService)
{
    public readonly ICourseService _courseService = courseService;

    [GraphQLName("getCourses")]
    public async Task<IEnumerable<Course>> GetCourses()
    {
        return await _courseService.GetCoursesAsync();
    }

    [GraphQLName("getCourseById")]
    public async Task<Course> GetCourseByIdAsync(string id)
    {
        return await _courseService.GetCourseByIdAsync(id);
    }
}