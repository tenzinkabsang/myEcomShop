using Ecom.Core.Domain;
using Ecom.Data;
using Ecom.Web.Infrastructure;

namespace Ecom.Web.Models;

public record TestUser
{
    private const string KEY = "you-the-test-customer";
    public int CustomerId { get; set; }

    public static TestUser GetTestUser(IServiceProvider services)
    {
        var session = services.GetRequiredService<IHttpContextAccessor>().HttpContext?.Session;
        var user = session?.GetJson<TestUser>(KEY);
        if (user != null) return user;

        var repository = services.GetRequiredService<IRepository<Customer>>();
        var entity = new Customer
        {
            CustomerGuid = Guid.NewGuid(),
            Username = "TestUser",
            Email = "TestUser@email.com",
            FirstName = "TEST",
            LastName = "TEST"
        };
        var createTestCustomerTask = repository.InsertAsync(entity);
        createTestCustomerTask.Wait();

        var testUser = new TestUser { CustomerId = entity.Id };
        session?.SetJson(KEY, testUser);

        return testUser;
    }
}