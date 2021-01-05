using System.Configuration;
using NUnit.Framework;
using System;
namespace NHibernate.Linq.Tests
{
    public class BaseTest
	{
		protected ISession session;
		private static Exception ex;
		protected virtual string ConnectionStringName
		{
			get { return "Northwind"; }
		}

		static BaseTest()
		{
			try{new GlobalSetup().SetupNHibernate();}
			catch(Exception ex){BaseTest.ex=ex;}
		}

		[SetUp]
		public virtual void Setup()
		{
			if(ex!=null)
				throw ex;
			session = CreateSession();
		}

		protected virtual ISession CreateSession()
		{
			var con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString);
			con.Open();
			return GlobalSetup.CreateSession(con);
		}

		[TearDown]
		public virtual void TearDown()
		{
			session.Connection.Dispose();
			session.Dispose();
			session = null;
		}
	}
}