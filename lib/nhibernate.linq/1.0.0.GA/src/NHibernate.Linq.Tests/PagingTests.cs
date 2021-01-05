using System.Linq;
using NUnit.Framework;

namespace NHibernate.Linq.Tests
{
	[TestFixture]
	public class PagingTests : BaseTest
	{
		[Test]
		public void Customers1to5()
		{
			var q = (from c in nwnd.Customers select c.CustomerID).Take(5);
			var query = q.ToList();

			Assert.AreEqual(5, query.Count);
		}

	}
}
