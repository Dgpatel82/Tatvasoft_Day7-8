using Data_Access_Layer.Repository;
using Data_Access_Layer.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer
{
    public class DALAdminUser
    {
        private readonly AppDbContext _cIDbContext;
        public DALAdminUser(AppDbContext cIDbContext)
        {
            _cIDbContext = cIDbContext;
        }
        public async Task<List<UserDetail>> UserDetailListAsync()
        {
            try
            {
                var userDetails = await (from u in _cIDbContext.User
                                         join ud in _cIDbContext.UserDetail on u.Id equals ud.UserId into userDetailGroup
                                         from userDetail in userDetailGroup.DefaultIfEmpty()
                                         where !u.IsDeleted && u.UserType == "user" && (userDetail == null || !userDetail.IsDeleted)
                                         select new UserDetail
                                         {
                                             Id = u.Id,
                                             FirstName = u.FirstName,
                                             LastName = u.LastName,
                                             PhoneNumber = u.PhoneNumber,
                                             EmailAddress = u.EmailAddress,
                                             UserType = u.UserType,
                                             UserId = userDetail.Id,
                                             Name = userDetail.Name,
                                             Surname = userDetail.Surname,
                                             EmployeeId = userDetail.EmployeeId,
                                             Department = userDetail.Department,
                                             Title = userDetail.Title,
                                             Manager = userDetail.Manager,
                                             WhyIVolunteer = userDetail.WhyIVolunteer,
                                             CountryId = userDetail.CountryId,
                                             CityId = userDetail.CityId,
                                             Avilability = userDetail.Avilability,
                                             LinkdInUrl = userDetail.LinkdInUrl,
                                             MySkills = userDetail.MySkills,
                                             UserImage = userDetail.UserImage,
                                             Status = userDetail.Status,
                                         }).ToListAsync();
                return userDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> DeleteUserAndUserDetailAsync(int userId)
        {
            try
            {
                string result = "";
                using (var transaction = await _cIDbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var userDetail = await _cIDbContext.UserDetail.FirstOrDefaultAsync(ud => ud.UserId == userId);
                        if (userDetail != null)
                        {
                            userDetail.IsDeleted = true;
                        }
                        var user = await _cIDbContext.User.FirstOrDefaultAsync(u => u.Id == userId);
                        if (user != null)
                        {
                            user.IsDeleted = true;
                        }
                        await _cIDbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                        result = "Delete User Successfully.";
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw ex;
                    }
                }
                return result; // Ensure the method always returns a value
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<User>> GetUserListAsync()
        {
            return await _cIDbContext.User.Where(u => !u.IsDeleted).ToListAsync();
        }
        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _cIDbContext.User.FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
        }
        public async Task<string> UpdateUserAsync(User updateUser)
        {
            string result = "";
            try
            {
                var existingUser = await _cIDbContext.User.FirstOrDefaultAsync(u => u.Id == updateUser.Id);
                if (existingUser != null)
                {
                    existingUser.FirstName = updateUser.FirstName;
                    existingUser.LastName = updateUser.LastName;
                    existingUser.PhoneNumber = updateUser.PhoneNumber;
                    existingUser.EmailAddress = updateUser.EmailAddress;
                    existingUser.Password = updateUser.Password;
                    existingUser.UserType = updateUser.UserType;

                    await _cIDbContext.SaveChangesAsync();
                    result = "User Updated Successfully";
                }
                else
                {
                    result = "User Not Found";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

    }
}
