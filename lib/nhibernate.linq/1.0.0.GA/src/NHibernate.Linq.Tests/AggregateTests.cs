using System;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NHibernate.Linq.Tests
{
	[TestFixture]
	public class AggregateTests : BaseTest
	{
		[Test]
		public void AggregateWithEquals()
		{
			var query = (from c in db.Customers
						 where c.CustomerID.Equals("ALFKI") || c.CustomerID.Equals("ANATR") || c.CustomerID.Equals("ANTON")
						 select c.CustomerID)
				.Aggregate((prev, next) => (prev + "," + next));

			Console.WriteLine(query);
			Assert.AreEqual("ALFKI,ANATR,ANTON", query);
		}

		
		[Test]
		[Ignore("TODO")]
		public void AggregateWithMonthFunction()
		{
			var date = new DateTime(2007, 1, 1);

			var query = (from e in db.Employees
						 where db.Methods.Month(e.BirthDate) == date.Month
						 select e.FirstName)
				.Aggregate(new StringBuilder(), (sb, name) => sb.Length > 0 ? sb.Append(", ").Append(name) : sb.Append(name));

			Console.WriteLine("{0} Birthdays:", date.ToString("MMMM"));
			Console.WriteLine(query);
		}

		[Test]
		[Ignore("TODO")]
		public void AggregateWithBeforeYearFunction()
		{
			var date = new DateTime(1960, 1, 1);

			var query = (from e in db.Employees
						 where db.Methods.Year(e.BirthDate) < date.Year
						 select db.Methods.Upper(e.FirstName))
				.Aggregate(new StringBuilder(), (sb, name) => sb.Length > 0 ? sb.Append(", ").Append(name) : sb.Append(name));

			Console.WriteLine("Birthdays before {0}:", date.ToString("yyyy"));
			Console.WriteLine(query);
		}

		[Test]
		[Ignore("TODO")]
		public void AggregateWithOnOrAfterYearFunction()
		{
			var date = new DateTime(1960, 1, 1);

			var query = (from e in db.Employees
						 where db.Methods.Year(e.BirthDate) >= date.Year && db.Methods.Len(e.FirstName) > 4
						 select e.FirstName)
				.Aggregate(new StringBuilder(), (sb, name) => sb.Length > 0 ? sb.Append(", ").Append(name) : sb.Append(name));

			Console.WriteLine("Birthdays after {0}:", date.ToString("yyyy"));
			Console.WriteLine(query);
		}

		[Test]
		[Ignore("TODO")]
		public void AggregateWithUpperAndLowerFunctions()
		{
			var date = new DateTime(2007, 1, 1);

			var query = (from e in db.Employees
						 where db.Methods.Month(e.BirthDate) == date.Month
						 select new { First = e.FirstName.ToUpper(), Last = db.Methods.Lower(e.LastName) })
				.Aggregate(new StringBuilder(), (sb, name) => sb.Length > 0 ? sb.Append(", ").Append(name) : sb.Append(name));

			Console.WriteLine("{0} Birthdays:", date.ToString("MMMM"));
			Console.WriteLine(query);
		}

		[Test]
		[Ignore("TODO")]
		public void AggregateWithCustomFunction()
		{
			var date = new DateTime(1960, 1, 1);

			var query = (from e in db.Employees
						 where db.Methods.Year(e.BirthDate) < date.Year
						 select db.Methods.fnEncrypt(e.FirstName))
				.Aggregate(new StringBuilder(), (sb, name) => sb.AppendLine(BitConverter.ToString(name)));

			Console.WriteLine(query);
		}
	}
}