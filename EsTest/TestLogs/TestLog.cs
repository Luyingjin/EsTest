using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsTest.TestLogs
{
    [ElasticsearchType(IdProperty = "Id")]
    public class TestLog
    {

            [Keyword]
            public Guid Id { get; set; }
            [Keyword]
            public DateTime Time { get; set; }
            [Keyword]
            public int Num { get; set; }
            [Keyword]
            public string Name { get; set; }
            [Text]
            public string info { get; set; }

    }
}
