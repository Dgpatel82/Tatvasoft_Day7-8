using Data_Access_Layer.Repository;
using Data_Access_Layer.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using System.Transactions;

namespace Data_Access_Layer
{
    public class DALLogin
    {
        private readonly AppDbContext _cIDbContext;
        public DALLogin(AppDbContext cIDbContext)
        {
            _cIDbContext = cIDbContext;
        }
        public User GetUserById(int id)
        {
            try
            {
                var user = _cIDbContext.User.FirstOrDefault(u => u.Id == id && !u.IsDeleted);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving user by ID", ex);
            }
        }

        public string Register(User user)
        {
            string result = "";
            try
            {
                bool emailExists = _cIDbContext.User.Any(u => u.EmailAddress == user.EmailAddress && !u.IsDeleted);
                if (!emailExists)
                {
                    string maxEmployeeIdStr = _cIDbContext.UserDetail.Max(ud => ud.EmployeeId);
                    int maxEmployeeId = 0;
                    if (!string.IsNullOrEmpty(maxEmployeeIdStr))
                    {
                        if (int.TryParse(maxEmployeeIdStr, out int parsedEmployeeId))
                        {
                            maxEmployeeId = parsedEmployeeId;
                        }
                        else
                        {
                            throw new Exception("Error converting EmployeeId to Integer.");
                        }
                    }
                    int newEmployeeId = maxEmployeeId + 1;
                    var newUser = new User
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        EmailAddress = user.EmailAddress,
                        Password = user.Password,
                        UserType = user.UserType,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false

                    };
                    _cIDbContext.User.Add(newUser);
                    _cIDbContext.SaveChanges();
                    var newUserDetail = new UserDetail
                    {
                        UserId = newUser.Id,
                        FirstName = user.FirstName,
                        Title = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        EmailAddress = user.EmailAddress,
                        UserType = user.UserType,
                        Name = user.FirstName,
                        Surname = user.LastName,
                        EmployeeId = newEmployeeId.ToString(),
                        Department = "IT",
                        Status = true,
                        Manager = "Managaer Value"

                    };
                    
                    _cIDbContext.UserDetail.Add(newUserDetail);
                    _cIDbContext.SaveChanges();
                    

                    result = "User Register Successfully";
                }
                else
                {
                    throw new Exception("Email Address Already exist");
                }
            }
            catch (Exception) {
                throw;
            }
            return result;
        }

        public string UpdateUser(User updateUser)
        {
            string result = "";
            try
            {
                var existingUser = _cIDbContext.User.FirstOrDefault(u => u.EmailAddress == updateUser.EmailAddress && !u.IsDeleted);
                var existingUserDetail = _cIDbContext.UserDetail.FirstOrDefault(u => u.UserId == updateUser.Id && !u.IsDeleted);

                if (existingUser != null && existingUserDetail != null)
                {
                    existingUser.FirstName = updateUser.FirstName;
                    existingUser.LastName = updateUser.LastName;
                    existingUser.PhoneNumber = updateUser.PhoneNumber;
                    existingUser.UserType = updateUser.UserType;
                    existingUser.ModifiedDate = DateTime.Now;

                    existingUserDetail.FirstName = updateUser.FirstName;
                    existingUserDetail.LastName = updateUser.LastName;
                    existingUserDetail.PhoneNumber = updateUser.PhoneNumber;
                    existingUserDetail.EmailAddress = updateUser.EmailAddress;
                    existingUserDetail.UserType = updateUser.UserType;
                    existingUserDetail.Name = updateUser.FirstName;
                    existingUserDetail.Surname = updateUser.LastName;
                    existingUserDetail.ModifiedDate = DateTime.Now;

                    _cIDbContext.SaveChanges();

                    result = "User Updated Successfully.";
                }
                else
                {
                    throw new Exception("User Not found or already deleted");
                }
            }
            catch (Exception) {
                throw;
            }
            return result;
        }

        public User LoginUser(User user)
        {
            User userObj = new User();
            try
            {
                    var query = from u in _cIDbContext.User
                                where u.EmailAddress == user.EmailAddress && u.IsDeleted == false
                                select new
                                {
                                    u.Id,
                                    u.FirstName,
                                    u.LastName,
                                    u.PhoneNumber,
                                    u.EmailAddress,
                                    u.UserType,
                                    u.Password,
                                    u.UserImage
                                };

                    var userData = query.FirstOrDefault();

                    if (userData != null)
                    {
                        if (userData.Password == user.Password)
                        {
                            userObj.Id = userData.Id;
                            userObj.FirstName = userData.FirstName;
                            userObj.LastName = userData.LastName;
                            userObj.PhoneNumber = userData.PhoneNumber;
                            userObj.EmailAddress = userData.EmailAddress;
                            userObj.UserType = userData.UserType;
                            userObj.UserImage = userData.UserImage;
                            userObj.Message = "Login Successfully";
                        }
                        else
                        {
                            userObj.Message = "Incorrect Password.";
                        }
                    }
                    else
                    {
                        userObj.Message = "Email Address Not Found.";
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return userObj;
        }
    }
}
