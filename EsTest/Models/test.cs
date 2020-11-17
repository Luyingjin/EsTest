using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsTest.Models
{
    public class test
    {
        private string _id;
        public string id {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                if (!string.IsNullOrWhiteSpace(value))
                    this.FormatId = Convert.ToInt32(value);
            }
        }
        private string _date;
        public string date { 
            get 
            {
                return _date; 
            } 
            set 
            {
                _date = value ;
                if (!string.IsNullOrWhiteSpace(value))
                    this.FormatDate = Convert.ToDateTime(value).AddHours(-8);
            } 
        }
        public string name { get; set; }
        public string twitter { get; set; }
        public DateTime FormatDate { get; set; }
        public int FormatId { get; set; }
    }
}
