using Microsoft.EntityFrameworkCore;
using Service;
using Service.Interface;
using Services.Entity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<IContactService, ContactService>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseInMemoryDatabase("InMemoryDb");
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Seed data here
    dbContext.Contacts.AddRange(
        new Contact
        {
            Id = 1,
            Name = "John Doe",
            BirthDate = new DateTime(1990, 1, 1),
        },
        new Contact
        {
            Id = 2,
            Name = "Jane Smith",
            BirthDate = new DateTime(1991, 1, 1),
        },
        new Contact
        {
            Id = 3,
            Name = "Dave Wilson",
            BirthDate = new DateTime(1992, 1, 1),
        },
        new Contact
        {
            Id = 4,
            Name = "Avrum Cohen",
            BirthDate = new DateTime(1993, 1, 1),
        },
        new Contact
        {
            Id = 5,
            Name = "Dave Luke",
            BirthDate = new DateTime(1994, 1, 1),
        }
    );
    dbContext.Emails.AddRange(
        new Email
        {
            Id = 1,
            ContactId = 1,
            IsPrimary = true,
            Address = "john@example.com",
        },
        new Email
        {
            Id = 2,
            ContactId = 1,
            IsPrimary = false,
            Address = "john.work@example.com",
        },
        new Email
        {
            Id = 3,
            ContactId = 2,
            IsPrimary = true,
            Address = "jane@example.com",
        },
        new Email
        {
            Id = 4,
            ContactId = 2,
            IsPrimary = false,
            Address = "jane.work@example.com",
        },
        new Email
        {
            Id = 5,
            ContactId = 3,
            IsPrimary = true,
            Address = "dave@example.com",
        },
        new Email
        {
            Id = 6,
            ContactId = 3,
            IsPrimary = false,
            Address = "dave.work@example.com",
        },
        new Email
        {
            Id = 7,
            ContactId = 4,
            IsPrimary = true,
            Address = "avrum@example.com",
        },
        new Email
        {
            Id = 8,
            ContactId = 4,
            IsPrimary = false,
            Address = "avrum.work@example.com",
        },
        new Email
        {
            Id = 9,
            ContactId = 5,
            IsPrimary = true,
            Address = "steve@example.com",
        },
        new Email
        {
            Id = 10,
            ContactId = 5,
            IsPrimary = false,
            Address = "steve.work@example.com",
        }
    );

    dbContext.SaveChanges();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


