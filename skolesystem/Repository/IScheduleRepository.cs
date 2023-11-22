using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using skolesystem.Data;
using skolesystem.DTOs;
using skolesystem.Models;

public interface IScheduleRepository
{
    Task<IEnumerable<Schedule>> GetAll();
    Task<Schedule> GetById(int id);
    Task<int> Create(Schedule schedule);
    Task Update(int id, ScheduleCreateDto scheduleDto);
    Task Delete(int id);
}


public class ScheduleRepository : IScheduleRepository
{
    private readonly ScheduleDbContext _context;

    public ScheduleRepository(ScheduleDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Schedule>> GetAll()
    {
        return await _context.Schedule.ToListAsync();
    }

    public async Task<Schedule> GetById(int id)
    {
        return await _context.Schedule.FindAsync(id);
    }

    public async Task<int> Create(Schedule schedule)
    {
        _context.Schedule.Add(schedule);
        await _context.SaveChangesAsync();
        return schedule.schedule_id;
    }

    public async Task Update(int id, ScheduleCreateDto scheduleDto)
    {
        var scheduleToUpdate = await _context.Schedule.FindAsync(id);

        if (scheduleToUpdate == null)
        {
            throw new ArgumentException("Schedule not found");
        }

        // Map properties from DTO to the entity
        scheduleToUpdate.subject_id = scheduleDto.subject_id;
        scheduleToUpdate.day_of_week = scheduleDto.day_of_week;
        scheduleToUpdate.end_time = scheduleDto.end_time;
        scheduleToUpdate.class_id = scheduleDto.class_id;

        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var scheduleToDelete = await _context.Schedule.FindAsync(id);

        if (scheduleToDelete == null)
        {
            throw new ArgumentException("Schedule not found");
        }

        _context.Schedule.Remove(scheduleToDelete);
        await _context.SaveChangesAsync();
    }

}
