using Business_logic_Layer;
using Microsoft.AspNetCore.Mvc;
using Data_Access_Layer.Repository.Entities;
using Data_Access_Layer;
using Microsoft.AspNetCore.Authorization;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminUserController : ControllerBase
    {
        private readonly BALAdminUser _balAdminUser;
        private readonly BALLogin _balLogin;
        ResponseResult result = new ResponseResult();
        public AdminUserController(BALAdminUser balAdminUser, BALLogin balLogin)
        {
            _balAdminUser = balAdminUser;
            _balLogin = balLogin;
        }


        [HttpPost]
        [Route("Register")]
        public ResponseResult RegisterUser(User user)
        {
            try
            {

                result.Data = _balLogin.Register(user);
                result.Result = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                result.Result = ResponseStatus.Error;
                result.Message = ex.Message;

            }
            return result;
        }

        [HttpGet("UserDetailList")]
        public async Task<IActionResult> GetUserDetailList()
        {
            try
            {
                var userDetailList = await _balAdminUser.UserDetailListAsync();
                return Ok(new ResponseResult { Data = userDetailList, Result = ResponseStatus.Success });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult { Result = ResponseStatus.Error, Message = ex.Message });
            }
        }

        [HttpDelete("DeleteUserAndUserDetail/{userId}")]
        public async Task<IActionResult> DeleteUserAndUserDetail(int userId)
        {
            try
            {
                var result = await _balAdminUser.DeleteUserAndUserDetailAsync(userId);
                return Ok(new ResponseResult { Data = result, Result = ResponseStatus.Success });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult { Result = ResponseStatus.Error, Message = ex.Message });
            }
        }
        [HttpPost]
        [Route("UpdateUser")]
        [Authorize]
        public ResponseResult UpdateUser(User user)
        {
            try
            {
                result.Data = _balLogin.UpdateUser(user);
                result.Result = ResponseStatus.Success;
            }
            catch (Exception ex)
            {
                result.Result = ResponseStatus.Error;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
