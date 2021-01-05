using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate.Linq.Tests.Entities;
using NUnit.Framework;

namespace NHibernate.Linq.Tests
{
	[TestFixture]
	public class IExpandProviderTests : BaseTest
	{

		protected override ISession CreateSession()
		{
			return GlobalSetup.CreateSession();
		}

		// Create the IUpdatable Interface
		public override void Setup()
		{
			base.Setup();			
		}

	}
}
