using Elasticsearch.Net;
using EsTest.TestLogs;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EsTest.Es
{
    public static class ElasticSearchHelper
    {
        public static readonly string url = "http://49.235.77.237:9200/";//这个是elasticsearch远程访问ip
        public static void insert(object t, string index)
        {
            //设置连接字符串，DefaultIndex中的表名要小写
            var settings = new ConnectionSettings(new Uri(url)).DefaultIndex(index);
            var client = new ElasticClient(settings);
            var doc = t;
            //通过 IndexDocument() 方法插入数据
            var ndexResponse = client.IndexDocument(doc);
        }
        /// <summary>
        /// 单点链接到ElasticSearch
        /// </summary>
        /// <param name="url">ElasticSearch的ip地址</param>
        /// <returns></returns>
        public static ElasticClient OneConnectES(string url)
        {

            var node = new Uri(url);
            var settings = new ConnectionSettings(node);
            var client = new ElasticClient(settings);
            return client;
        }
        /// <summary>
        /// 指定多个节点使用连接池链接到Elasticsearch集群
        /// </summary>
        /// <param name="serverurl">链接ip数组</param>
        /// <returns></returns>
        public static ElasticClient ManyConnectES(string[] serverurl)
        {
            Uri[] nodes = new Uri[serverurl.Length];
            for (int i = 0; i < serverurl.Length; i++)
            {
                nodes[i] = new Uri(serverurl[i]);
            }
            var pool = new StaticConnectionPool(nodes);
            var settings = new ConnectionSettings(pool);
            var client = new ElasticClient(settings);
            return client;
        }
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="elasticClient"></param>
        public static CreateIndexResponse CreateIndex(this IElasticClient elasticClient, string indexName, int numberOfReplicas = 1, int numberOfShards = 5)
        {
            IIndexState indexState = new IndexState
            {
                Settings = new IndexSettings
                {
                    NumberOfReplicas = numberOfReplicas,
                    // [副本数量]
                    NumberOfShards = numberOfShards
                }
            };
            Func<CreateIndexDescriptor, ICreateIndexRequest> func = x => x.InitializeUsing(indexState).Map(m => m.AutoMap());
            CreateIndexResponse response = elasticClient.Indices.Create(indexName, func);
            return response;
        }

        public static List<TestLog> GetTestLogList(string index,string name, int pageIndex, int pageSize)
        {
            var settings = new ConnectionSettings(new Uri(url)).DefaultIndex(index);
            var client = new ElasticClient(settings);
            var musts = new List<Func<QueryContainerDescriptor<TestLog>, QueryContainer>>();
            musts.Add(p => p.Term(t=>t.Name(name)));
            var search = new SearchDescriptor<TestLog>();
            search = search.Query(p => p.Bool(m => m.Must(musts))).From((pageIndex - 1) * pageSize).Take(pageSize);
            var response = client.Search<TestLog>(search);
            return response.Documents.ToList();
        }

        public static List<T> SearchEntityBySql<T>(string sql) where T:class,new()
        {
            var httpResult = HttpRequestHelper.HttpPostStream("http://49.235.77.237:9200/_xpack/sql?format=csv", jsonData: JsonConvert.SerializeObject(new { query = sql }));

            var table =  DBToolV2Plus.CSVConvertToDatable(httpResult, 1);
            var result=table.DataTableToEntity<T>();
            if (result != null)
                return result;
            return default(List<T>);
        }

       
    }
}
