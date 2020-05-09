﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using udemyApp.API.Models;

namespace udemyApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;
        public DatingRepository(DataContext context) 
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
            //throw new NotImplementedException();
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
            //throw new NotImplementedException();
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(photo => photo.Photos).FirstOrDefaultAsync(user => user.Id == id);

            return user;
            //throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.Include(photo => photo.Photos).ToListAsync();

            return users;
            //throw new NotImplementedException();
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
            //throw new NotImplementedException();
        }
    }
}
