using EsTest.Es;
using EsTest.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EsTest.TestLogs
{
    public class TestLogContext : BaseEsContext<TestLog>
    {
        public override string IndexName => "testlog.log.demo";
        public TestLogContext(IEsClientProvider provider) : base(provider)
        {
        }

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="province"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<TestLog> GetTestLogs( int pageIndex, int pageSize, string name=null)
        {
            var client = _EsClientProvider.GetClient(IndexName);
            var musts = new List<Func<QueryContainerDescriptor<TestLog>, QueryContainer>>();
            //if (!string.IsNullOrWhiteSpace(name))
            //    musts.Add(p => p.Term(m => m.Field(t=>t.Name)(name)));
            var search = new SearchDescriptor<TestLog>();


            var mustFilters = new List<Func<QueryContainerDescriptor<TestLog>, QueryContainer>>();
            if (!string.IsNullOrEmpty(name))
            {
                mustFilters.Add(t => t.MatchPhrase(f => f.Field(fd => fd.Name).Query(name)));
            }
            search = search.Query(q => q.Bool(t => t.Must(mustFilters).Filter(z => z.Bool(b => b.Must(mustFilters)))));
            //if (!string.IsNullOrWhiteSpace(name))
            //{
            //    search = search.
            //    Query(p => p
            //        .Match(m => m
            //            .Field(f => f.Name)
            //            .Query(name)
            //            .Fuzziness(Fuzziness.EditDistance(2))
            //            )
            //        )
            //    ;

            //}
            //else
            //{
            //    search=search.Query(m => m.MatchAll());
            //}
            
            search = search.Sort(t => t.Ascending(m => m.Time)).From((pageIndex - 1) * pageSize).Take(pageSize);
            var response = client.Search<TestLog>(search);
            return response.Documents.ToList();
        }

        public bool DeleteAll()
        {
            var client = _EsClientProvider.GetClient(IndexName);
            var search = new DeleteByQueryDescriptor<TestLog>().Index(IndexName);
            search = search.Query(p => p.MatchAll());
            var response = client.DeleteByQuery<TestLog>(p => search);
            return response.IsValid;
        }


    }
}
