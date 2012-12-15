using AHCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.Util;
using System.Web;
using Lucene.Net.Store;
using Lucene.Net.Search;
using AHCMS.Core.Repository;
using Lucene.Net.QueryParsers;

namespace AHCMS.Core.Search
{
    public class IndexHelper
    {
        public static string IndexPath;

        static IndexHelper()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            IndexPath = basePath + "\\Data\\Index\\";
            if (!System.IO.Directory.Exists(IndexPath))
            {
                if (!System.IO.Directory.Exists(basePath+"\\Data\\"))
                {
                    System.IO.Directory.CreateDirectory(basePath + "\\Data\\");
                }
                System.IO.Directory.CreateDirectory(IndexPath);
            }
        }

        public static IndexWriter CreateIndex(Content[] contents)
        {
            var v = Lucene.Net.Util.Version.LUCENE_30;
            var l = Lucene.Net.Index.IndexWriter.MaxFieldLength.UNLIMITED;
            var d = FSDirectory.Open(new DirectoryInfo(IndexPath));

            IndexWriter writer = new IndexWriter(d, new StandardAnalyzer(v), l);

            try
            {
                foreach (var item in contents)
                {
                    Document doc = new Document();

                    Field id = new Field("id", item.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);
                    Field title = new Field("title", item.Title, Field.Store.YES, Field.Index.ANALYZED);
                    Field username = new Field("username", item.User.UserName, Field.Store.YES, Field.Index.ANALYZED);
                    doc.Add(id);
                    doc.Add(title);
                    doc.Add(username);
                    writer.AddDocument(doc);
                }
                writer.Optimize();
                writer.Dispose();
            }
            catch (System.Exception ex)
            {

            }

            return writer;
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="field">字段</param>
        /// <param name="mun">搜索条数</param>
        /// <returns></returns>
        public static List<Content> Search(string keywords,string field,int mun)
        {
            FSDirectory d = FSDirectory.Open(IndexPath);
            IndexSearcher searcher = new IndexSearcher(d, true);

            IRepository repository = Container.AHSContainer.ResolverRepository();
            var scoreDocs = Search(searcher, keywords, field, mun, true);

            var ids = scoreDocs.Select(x =>
            {
                Guid id = Guid.Empty;
                var doc = searcher.Doc(x.Doc);
                Guid.TryParse(doc.Get("id"), out id);
                return id;
            }).ToArray();

            try
            {
                return repository.Query<Content>().Where(x => ids.Contains(x.Id)).ToList();
            }
            finally
            {
                repository.Dispose();
            }
        }

        static ScoreDoc[] Search(IndexSearcher searcher, string queryString, string field, int numHit, bool inOrder)
        {
            TopScoreDocCollector collector = TopScoreDocCollector.Create(numHit, inOrder);

            Analyzer analyser = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

            QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, field, analyser);

            Query query = parser.Parse(queryString);

            searcher.Search(query, collector);

            return collector.TopDocs().ScoreDocs;
        }
    }
}
