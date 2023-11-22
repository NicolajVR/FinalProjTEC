using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using skolesystem.DTOs;
using skolesystem.Models;

public interface IScheduleService
{
    Task<IEnumerable<Schedule>> GetAllSchemata();
    Task<Schedule> GetScheduleById(int id);
    Task<int> CreateSchedule(Schedule schedule);
    Task UpdateSchedule(int id, ScheduleCreateDto scheduleDto);
    Task DeleteSchedule(int id);
}

public class ScheduleService : IScheduleService
{
    private readonly IScheduleRepository _scheduleRepository;

    public ScheduleService(IScheduleRepository scheduleRepository)
    {
        _scheduleRepository = scheduleRepository;
    }

    public async Task<IEnumerable<Schedule>> GetAllSchemata()
    {
        return await _scheduleRepository.GetAll();
    }

    public async Task<Schedule> GetScheduleById(int id)
    {
        return await _scheduleRepository.GetById(id);
    }

    public async Task<int> CreateSchedule(Schedule schedule)
    {
        return await _scheduleRepository.Create(schedule);
    }

    public async Task UpdateSchedule(int id, ScheduleCreateDto scheduleDto)
    {
        var existingSchedule = await _scheduleRepository.GetById(id);

        if (existingSchedule == null)
        {
            throw new ArgumentException($"Schedule with ID {id} not found.");
        }

        await _scheduleRepository.Update(id, scheduleDto);
    }


    public async Task DeleteSchedule(int id)
    {
        await _scheduleRepository.Delete(id);
    }
}