using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EsTest.Addresses;
using EsTest.Es;
using EsTest.Models;
using EsTest.TestLogs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EsTest.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private AddressContext _AddressContext;
        private TestLogContext _testLogContext;
        public AddressController(AddressContext context, TestLogContext testLogContext)
        {
            _AddressContext = context;
            _testLogContext = testLogContext;
        }
        /// <summary>
        /// 新增或者修改
        /// </summary>
        /// <param name="address"></param>
        [HttpPost("addaddress")]
        public void AddAddress(List<Address> addressList)
        {
            if (addressList == null || addressList.Count < 1)
            {
                return;
            }
            _AddressContext.InsertMany(addressList);
        }

        /// <summary>
        /// 删除地址
        /// </summary>
        /// <param name="id"></param>
        [HttpPost("deleteAddress")]
        public void DeleteAdress(string id)
        {
            _AddressContext.DeleteById(id);
        }
        /// <summary>
        /// 获取所有与地址
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAllAddress")]
        public List<Address> GetAllAddress()
        {
            return _AddressContext.GetAllAddresses();
        }
        /// <summary>
        /// 获取地址总数
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAddressTotalCount")]
        public long GetAddressTotalCount()
        {
            return _AddressContext.GetTotalCount();
        }

        /// <summary>
        /// 分页获取（可以进一步封装查询条件）
        /// </summary>
        /// <param name="province"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpPost("getAddressByProvince")]
        public List<Address> GetAddressByProvince(string province, int pageIndex, int pageSize)
        {
            return _AddressContext.GetAddresses(province, pageIndex, pageSize);
        }


        /// <summary>
        /// 插入log
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("InsertLog")]
        public void InsertLog()
        {
            List<TestLog> list = new List<TestLog>();
            //插入200条数据
            for (int i = 0; i < 200; i++)
            {
                var d = new TestLog
                {
                    Id=Guid.NewGuid(),
                    Time = DateTime.Now,
                    Num = i+200,
                    Name = $"name{i+200}",
                    info = $"hello world {i+200}!"
                };
                //ElasticSearchHelper.insert(d, "address.log.demo");
                list.Add(d);

            }

            _testLogContext.InsertMany(list);

        }
        /// <summary>
        /// 查询log
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("GetLog")]
        public List<TestLog> GetLog(int pageIndex=1,int pageSize=200,string name=null)
        {
            //var result=JsonConvert.SerializeObject( ElasticSearchHelper.GetTestLogList("address.log.demo", "12313", 1, 100));
            //var result= $"获取总数为{_testLogContext.GetTotalCount()}" ;
            var result1 = _testLogContext.GetTestLogs(pageIndex, pageSize,name);
            return result1;
        }

        /// <summary>
        /// 查询log
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("GetLogTotal")]
        public long GetLogTotal()
        {
            //var result= $"获取总数为{_testLogContext.GetTotalCount()}" ;
            var result1 = _testLogContext.GetTotalCount();
            return result1;
        }
        /// <summary>
        /// 查询log
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("DeleteLogs")]
        public bool DeleteLogs()
        {
            var result1 = _testLogContext.DeleteAll();
            return result1;
        }

        /// <summary>
        /// 查询jackietestmodel
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("getjackietestmodel")]
        public List<test> GetJackietestModel()
        {

            var result = ElasticSearchHelper.SearchEntityBySql<test>("select id,date,name,twitter from test limit 10");
            return result;
        }

    }

}