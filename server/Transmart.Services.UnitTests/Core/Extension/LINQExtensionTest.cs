using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using TranSmart.Core.Extension;
using TranSmart.Domain.Entities.Payroll;

namespace Transmart.Services.UnitTests.Core
{
	public class LINQExtensionTest
	{
		[Fact]
		public void Exclude_DifferenceOfTwoLists_Success()
		{
			var listA = new List<HraDeclaration>
			{
				new HraDeclaration
				{
					ID= Guid.Parse("f9a4217a-38a7-4f2b-8cfd-eb7b18b55793"),
					Amount = 45000,
					AddedAt = DateTime.Now,
					Address = "Madhapur",
					City = "Hyderabad",
					DeclarationId = Guid.Parse("4ab3fa66-7381-41e4-978a-b790f645c4c6"),
					Pan = "APS3145R",
					RentalFrom = DateTime.Now,
					RentalTo = DateTime.Now.AddDays(30),
					LandLord = "Vishnu",
					Total = 78000
				},
				new HraDeclaration
				{
					ID= Guid.Parse("257ac38a-937c-4420-aecf-84418013a970"),
					Amount = 45000,
					AddedAt = DateTime.Now,
					Address = "Madhapur",
					City = "Hyderabad",
					DeclarationId = Guid.Parse("4ab3fa66-7381-41e4-978a-b790f645c4c6"),
					Pan = "APS3145R",
					RentalFrom = DateTime.Now,
					RentalTo = DateTime.Now.AddDays(30),
					LandLord = "Vishnu",
					Total = 78000
				},
			};
			var listB = new List<HraDeclaration>
			{
				new HraDeclaration
				{
					ID= Guid.Parse("f9a4217a-38a7-4f2b-8cfd-eb7b18b55793"),
					Amount = 35000,
					AddedAt = DateTime.Now,
					Address = "HiTechCity",
					City = "Hyderabad",
					DeclarationId = Guid.Parse("d35e4d5a-8843-488c-a897-14286de3b7ee"),
					Pan = "TGD4529T",
					RentalFrom = DateTime.Now,
					RentalTo = DateTime.Now.AddDays(15),
					LandLord = "Mahesh",
					Total = 97450
				},
				new HraDeclaration
				{
					ID= Guid.Parse("b08532f6-21f6-4e84-8368-0e06940db942"),
					Amount = 35000,
					AddedAt = DateTime.Now,
					Address = "HiTechCity",
					City = "Hyderabad",
					DeclarationId = Guid.Parse("d35e4d5a-8843-488c-a897-14286de3b7ee"),
					Pan = "TGD4529T",
					RentalFrom = DateTime.Now,
					RentalTo = DateTime.Now.AddDays(15),
					LandLord = "Mahesh",
					Total = 97450
				},
			};
			var response = listA.Exclude(listB, (x, y) => x.ID == y.ID);
			Assert.True(listA.ToList()[0].ID != response.ToList()[0].ID);
		}

		[Fact]
		public void Intersection_IntersectsTheTwoList_Success()
		{
			var listA = new List<HraDeclaration>
			{
				new HraDeclaration
				{
					ID= Guid.Parse("f9a4217a-38a7-4f2b-8cfd-eb7b18b55793"),
					Amount = 45000,
					AddedAt = DateTime.Now,
					Address = "Madhapur",
					City = "Hyderabad",
					DeclarationId = Guid.Parse("4ab3fa66-7381-41e4-978a-b790f645c4c6"),
					Pan = "APS3145R",
					RentalFrom = DateTime.Now,
					RentalTo = DateTime.Now.AddDays(30),
					LandLord = "Vishnu",
					Total = 78000
				},
				new HraDeclaration
				{
					ID= Guid.Parse("257ac38a-937c-4420-aecf-84418013a970"),
					Amount = 45000,
					AddedAt = DateTime.Now,
					Address = "Madhapur",
					City = "Hyderabad",
					DeclarationId = Guid.Parse("4ab3fa66-7381-41e4-978a-b790f645c4c6"),
					Pan = "APS3145R",
					RentalFrom = DateTime.Now,
					RentalTo = DateTime.Now.AddDays(30),
					LandLord = "Vishnu",
					Total = 78000
				},
			};
			var listB = new List<HraDeclaration>
			{
				new HraDeclaration
				{
					ID= Guid.Parse("f9a4217a-38a7-4f2b-8cfd-eb7b18b55793"),
					Amount = 35000,
					AddedAt = DateTime.Now,
					Address = "HiTechCity",
					City = "Hyderabad",
					DeclarationId = Guid.Parse("d35e4d5a-8843-488c-a897-14286de3b7ee"),
					Pan = "TGD4529T",
					RentalFrom = DateTime.Now,
					RentalTo = DateTime.Now.AddDays(15),
					LandLord = "Mahesh",
					Total = 97450
				},
				new HraDeclaration
				{
					ID= Guid.Parse("b08532f6-21f6-4e84-8368-0e06940db942"),
					Amount = 35000,
					AddedAt = DateTime.Now,
					Address = "HiTechCity",
					City = "Hyderabad",
					DeclarationId = Guid.Parse("d35e4d5a-8843-488c-a897-14286de3b7ee"),
					Pan = "TGD4529T",
					RentalFrom = DateTime.Now,
					RentalTo = DateTime.Now.AddDays(15),
					LandLord = "Mahesh",
					Total = 97450
				},
			};
			var response = listA.Intersection(listB, (x, y) => x.ID == y.ID);
			Assert.True(listA.ToList()[0].ID == response.ToList()[0].ID);
		}

		[Fact]
		public void HasDuplicate_FindDuplicate_ShouldBeDuplicate()
		{
			var listA = new List<Section6A80C>
			{
				new Section6A80C
				{
					Section80CId = Guid.Parse("71eed0f1-aff9-42ba-b75c-0182c3a4901f"),
					Amount = 75000
				},
				new Section6A80C
				{
					Section80CId = Guid.Parse("71eed0f1-aff9-42ba-b75c-0182c3a4901f"),
					Amount = 98000
				},
				new Section6A80C
				{
					Section80CId = Guid.Parse("7cce1491-6163-469d-af4f-df7d8a0ab17e"),
					Amount = 98000
				}
			};

			var response = listA.HasDuplicate(x => x.Section80CId); //Returns true if Duplicate Section80CId exist in the list
			Assert.True(response);
		}

		[Fact]
		public void GetOverlappedTimes_FindDuplicate_ShouldNotBeDuplicate() 
		{
			var list = new List<HraDeclaration>
			{
				new HraDeclaration
				{
					RentalFrom = DateTime.Now,
					RentalTo = DateTime.Now,
					City = "Hyderabad"
				},
				new HraDeclaration
				{
					RentalFrom = DateTime.Now.AddMonths(1),
					RentalTo = DateTime.Now.AddMonths(1),
					City = "Vijayawada"
				}
			};

			var response = list.GetOverlappedTimes(list, x => x.RentalFrom, x => x.RentalTo);

			Assert.Empty(response);
		}

		[Fact]
		public void GetOverlappedTimes_FindDuplicate_ShouldBeDuplicate()
		{
			var list = new List<HraDeclaration>
			{
				new HraDeclaration
				{
					RentalFrom = DateTime.Now,
					RentalTo = DateTime.Now,
					City = "Hyderabad"
				},
				new HraDeclaration
				{
					RentalFrom = DateTime.Now,
					RentalTo = DateTime.Now,
					City = "Vijayawada"
				}
			};

			var response = list.GetOverlappedTimes(list, x => x.RentalFrom, x => x.RentalTo);

			Assert.Equal(2, response.Count());
		}

		[Fact]
		public void Compare_CompareList_Success()
		{
			var listA = new List<Arrear>
			{
				new Arrear
				{
					EmployeeID = Guid.Parse("e9eeb322-73fc-4304-a89c-d5da3d090f0a"),
					Pay = 45000
				},
				new Arrear
				{
					EmployeeID = Guid.Parse("b790c4c5-4324-444e-8e6d-651c75236520"),
					Pay = 65000
				}
			};

			var listB = new List<Arrear>
			{
				new Arrear
				{
					EmployeeID = Guid.Parse("e9eeb322-73fc-4304-a89c-d5da3d090f0a"),
					Pay = 45000
				},
				new Arrear
				{
					EmployeeID = Guid.Parse("3a66a238-80ef-4ef3-8e2e-4af45a77a8a5"),
					Pay = 75000
				}
			};

			var response = listA.Compare(listB, (x, y) => x.EmployeeID == y.EmployeeID);
			Assert.Equal(listA.ToList()[0].EmployeeID, response.Same.ToList()[0].EmployeeID); //Same
			Assert.Equal(listB.ToList()[1].EmployeeID, response.Added.ToList()[0].EmployeeID); // Added
			Assert.Equal(listA.ToList()[1].EmployeeID, response.Deleted.ToList()[0].EmployeeID);// Deleted
		}

		[Fact]
		public void Compare_ListAAndBAreNull_ReturnsEmptyList()
		{
			List<Arrear> listA = null;
			List<Arrear> listB = null;

			var response = listA.Compare(listB, (x, y) => x.EmployeeID == y.EmployeeID);
			Assert.True(!response.Same.Any()); //Same
			Assert.True(!response.Added.Any()); //Added
			Assert.True(!response.Deleted.Any()); //Deleted
		}

		[Fact]
		public void Compare_ListANull_ShouldBeEmptyListA()
		{
			List<Arrear> listA = null;
			var listB = new List<Arrear>
			{
				new Arrear
				{
					EmployeeID = Guid.Parse("e9eeb322-73fc-4304-a89c-d5da3d090f0a"),
					Pay = 45000
				},
				new Arrear
				{
					EmployeeID = Guid.Parse("3a66a238-80ef-4ef3-8e2e-4af45a77a8a5"),
					Pay = 75000
				}
			};

			var response = listA.Compare(listB, (x, y) => x.EmployeeID == y.EmployeeID);
			Assert.True(!response.Same.Any());
			Assert.Equal(listB, response.Added);
			Assert.True(!response.Deleted.Any());
		}

		[Fact]
		public void Compare_ListBNull_ShouldBeEmptyListB()
		{
			var listA = new List<Arrear>
			{
				new Arrear
				{
					EmployeeID = Guid.Parse("e9eeb322-73fc-4304-a89c-d5da3d090f0a"),
					Pay = 45000
				},
				new Arrear
				{
					EmployeeID = Guid.Parse("b790c4c5-4324-444e-8e6d-651c75236520"),
					Pay = 65000
				}
			};
			List<Arrear> listB = null;

			var response = listA.Compare(listB, (x, y) => x.EmployeeID == y.EmployeeID);
			Assert.True(!response.Same.Any());
			Assert.True(!response.Added.Any());
			Assert.Equal(listA, response.Deleted);
		}
	}
}
