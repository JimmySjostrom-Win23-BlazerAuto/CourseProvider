﻿using Infrastructure.Data.Contexts;
using Infrastructure.Factories;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;
public class CourseService(IDbContextFactory<DataContext> contextFactory) : ICourseService
{
	private readonly IDbContextFactory<DataContext> _contextFactory = contextFactory;

	public async Task<Course> CreateCourseAsync(CourseCreateRequest request)
	{
		await using var context = _contextFactory.CreateDbContext();
		var courseEntity = CourseFactory.Create(request);
		context.Courses.Add(courseEntity);
		await context.SaveChangesAsync();	
		return CourseFactory.Create(courseEntity);
	}

	public async Task<Course> GetCourseByIdAsync(string id)
	{
		await using var context = _contextFactory.CreateDbContext();
		var courseEntity = await context.Courses.FirstOrDefaultAsync(c => c.Id == id);
		return courseEntity == null ? null! : CourseFactory.Create(courseEntity);
	}

	public async Task<IEnumerable<Course>> GetCoursesAsync()
	{
		await using var context = _contextFactory.CreateDbContext();
		var courseEntities = await context.Courses.ToListAsync();
		return courseEntities.Select(CourseFactory.Create);
	}

	public async Task<Course> UpdateCourseAsync(CourseUpdateRequest request)
	{
		await using var context = _contextFactory.CreateDbContext();
		var existingCourse = await context.Courses.FirstOrDefaultAsync(c => c.Id == request.Id);
		if (existingCourse == null)
			return null!;

		var updatedCourseEntity = CourseFactory.Create(request);
		updatedCourseEntity.Id = existingCourse.Id;
		context.Entry(existingCourse).CurrentValues.SetValues(updatedCourseEntity);

		await context.SaveChangesAsync();	
		return CourseFactory.Create(existingCourse);
	}

	public async Task<bool> DeleteCourseAysnc(string id)
	{
		await using var context = _contextFactory.CreateDbContext();
	
		var courseEntity = await context.Courses.FirstOrDefaultAsync(c => c.Id == id);
		if (courseEntity == null)
			return false;

		context.Courses.Remove(courseEntity);
		await context.SaveChangesAsync();
		return true;
	}
}