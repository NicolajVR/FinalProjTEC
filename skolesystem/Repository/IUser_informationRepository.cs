using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using skolesystem.Data;
using skolesystem.DTOs;
using skolesystem.Models;

// Repository for data access
public interface IUser_informationRepository
{
    Task<User_information> GetById(int id);
    Task<IEnumerable<User_information>> GetAll();
    Task<IEnumerable<User_information>> GetDeletedUser_informations();
    Task AddUser_information(User_information user_information);
    Task UpdateUser_information(int id, User_information updatedUser_information);
    Task SoftDeleteUser_information(int id);
}

public class User_informationRepository : IUser_informationRepository
{
    private readonly User_informationDbContext _context;

    public User_informationRepository(User_informationDbContext context)
    {
        _context = context;
    }

    public async Task<User_information> GetById(int id)
    {
        return await _context.User_information.FindAsync(id);
    }

    public async Task<IEnumerable<User_information>> GetAll()
    {
        return await _context.User_information.ToListAsync();
    }

    public async Task<IEnumerable<User_information>> GetDeletedUser_informations()
    {
        return await _context.User_information.Where(b => b.is_deleted).ToListAsync();
    }

    public async Task AddUser_information(User_information user_information)
    {
        _context.User_information.Add(user_information);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUser_information(int id, User_information updatedUser_information)
    {
        var existingUser_information = await _context.User_information.FindAsync(id);


        if (existingUser_information == null)
        {
            throw new ArgumentException("Absence not found");
        }
        // Update properties of existingUser_information with updatedUser_information
        existingUser_information.user_information_id = updatedUser_information.user_information_id;
        existingUser_information.name = updatedUser_information.name;
        existingUser_information.last_name = updatedUser_information.last_name;
        existingUser_information.phone = updatedUser_information.phone;
        existingUser_information.date_of_birth = updatedUser_information.date_of_birth;
        existingUser_information.address = updatedUser_information.address;
        existingUser_information.is_deleted = updatedUser_information.is_deleted;
        existingUser_information.gender_id = updatedUser_information.gender_id;

        await _context.SaveChangesAsync();
    }

    public async Task SoftDeleteUser_information(int id)
    {
        var user_informationToDelete = await _context.User_information.FindAsync(id);

        if (user_informationToDelete != null)
        {
            user_informationToDelete.is_deleted = true;
            await _context.SaveChangesAsync();
        }
    }
}


