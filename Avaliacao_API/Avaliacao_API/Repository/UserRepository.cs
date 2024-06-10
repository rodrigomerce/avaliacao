using Avaliacao_API.Entity;
using Avaliacao_API.Interfaces;

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LinqToDB.Common;

namespace Avaliacao_API.Repository
{
    public class UserRepository : IUser
    {
        IList<USERS> users = new List<USERS>();

        /* public void CreateUser(User user)
         {
             users.Add(user);
         }

         public void DeleteUser(int id)
         {
             var deleteUser = ShowUserByID(id);
             users.Remove(deleteUser);
         }

         public void EditUser(User user)
         {
             var editUser = ShowUserByID(user.Id);
             editUser.Name = user.Name;
             editUser.Email = user.Email;
         }*/

        private readonly UserDbContext _dbContext;

        public UserRepository(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public USERS? ShowUserByID(int id)
        {

            return _dbContext.Users.FirstOrDefault(user => user.Id == id);
        }

        public IList<USERS> ShowUsers()
        {
            var users = _dbContext.Users.FromSqlRaw("SELECT * FROM public.Users").ToList();
            return users;
            //return _dbContext.Users.ToList();
        }
    }
}
