using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NHibernate.Linq.Tests
{
	[TestFixture]
	public class BinaryExpressionOrdererTests : BaseTest
	{
		protected override ISession CreateSession()
		{
			return GlobalSetup.CreateSession();
		}

		[Test]
		[Ignore("")]
		public void PropertyCriteriaDoesntSwap()
		{
		}
		[Test]
		[Ignore("")]
		public void CriteriaPropertySwapsToPropertyCriteria()
		{
		}
		[Test]
		[Ignore("")]
		public void ValueCriteriaDoesntSwap()
		{
		}
		[Test]
		[Ignore("")]
		public void CriteriaValueSwapsValueCriteria()
		{
		}
		[Test]
		[Ignore("")]
		public void CriteriaCriteriaDoesntSwap()
		{
		}
	}
}