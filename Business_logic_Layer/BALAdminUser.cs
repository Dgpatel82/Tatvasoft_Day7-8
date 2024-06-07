using Data_Access_Layer;
using Data_Access_Layer.Repository.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic_Layer
{
    public class BALAdminUser
    {
        private readonly DALAdminUser _dalAdminUser;


        public BALAdminUser(DALAdminUser dalAdminUser)
        {
            _dalAdminUser = dalAdminUser;
        }

        public async Task<List<User>> GetUserListAsync()
        {
            return await _dalAdminUser.GetUserListAsync();
        }
        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _dalAdminUser.GetUserByIdAsync(userId);
        }

        public async Task<ResponseResult> UpdateUserAsync(User user)
        {
            var result = new ResponseResult();
            try
            {
                var message = await _dalAdminUser.UpdateUserAsync(user);
                result.Message = message;
                result.Result = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Result = ResponseStatus.Error;
            }
            return result;
        }


        public async Task<List<UserDetail>> UserDetailListAsync()
        {
            return await _dalAdminUser.UserDetailListAsync();
        }

        public async Task<string> DeleteUserAndUserDetailAsync(int userId)
        {
            return await _dalAdminUser.DeleteUserAndUserDetailAsync(userId);
        }
       
    }
}
