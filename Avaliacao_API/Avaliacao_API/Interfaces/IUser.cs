using Avaliacao_API.Entity;

namespace Avaliacao_API.Interfaces
{
    public interface IUser
    {
         IList<USERS> ShowUsers();

        USERS ShowUserByID(int id);

         /*void CreateUser(User user);

         void EditUser(User user);

         void DeleteUser(int id);*/
    }
}
