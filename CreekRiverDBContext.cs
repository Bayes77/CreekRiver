using Microsoft.EntityFrameworkCore;
using CreekRiver.Models;

public class CreekRiverDBContext : DbContext
{
    public DbSet<Reservation> Reservations {get; set;}
    public DbSet<UserProfile> UserProfiles {get; set;}
    public DbSet<Campsite> Campsites {get; set;}
    public DbSet<CampsiteType> CampsiteTypes {get; set;}

    public CreekRiverDBContext(DbContextOptions<CreekRiverDBContext> context) : base(context)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CampsiteType>().HasData(new CampsiteType[]
        {
        new CampsiteType {Id = 1, CampsiteTypeName = "Tent", FeePerNight = 15.99m, MaxReservationDays = 7},
        new CampsiteType {Id = 2, CampsiteTypeName = "RV", FeePerNight = 26.50m, MaxReservationDays = 14},
        new CampsiteType {Id = 3, CampsiteTypeName = "Primitive", FeePerNight = 10.00m, MaxReservationDays = 3},
        new CampsiteType {Id = 4, CampsiteTypeName = "Hammock", FeePerNight = 12m, MaxReservationDays = 7},

      
    });
    

      modelBuilder.Entity<Campsite>().HasData(new Campsite[]
        {
            new Campsite {Id= 1, CampsiteTypeId = 1, NickName = "Barrerl Owl", ImageUrl ="https://tnstateparks.com/assets/images/content-images/campgrounds/249/colsp-area2-site73.jpg"},
            new Campsite {Id= 2, CampsiteTypeId = 2, NickName = "Beaver Garden", ImageUrl ="https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.gettyimages.com%2Fphotos%2Fbeaver&psig=AOvVaw3lTs0ZxAR_oSHSZMVI1lMP&ust=1739933392927000&source=images&cd=vfe&opi=89978449&ved=0CBEQjRxqFwoTCNCIy5KbzIsDFQAAAAAdAAAAABAJ"},
            new Campsite {Id= 3, CampsiteTypeId = 3, NickName = "Hamster Wheel", ImageUrl ="https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.thesprucepets.com%2Fpopular-hamster-breeds-5101161&psig=AOvVaw2-BoSYaME4vvZGo7ECxi9g&ust=1739933545298000&source=images&cd=vfe&opi=89978449&ved=0CBEQjRxqFwoTCJirs9mbzIsDFQAAAAAdAAAAABAE"},
            new Campsite {Id= 2, CampsiteTypeId = 4, NickName = "Horse Hills", ImageUrl ="https://www.google.com/url?sa=i&url=https%3A%2F%2Fracingclub.com%2Fguides%2Fdifferent-types-of-racehorses%2F&psig=AOvVaw2ddk3lLrB9aa12TIDXSFms&ust=1739933567459000&source=images&cd=vfe&opi=89978449&ved=0CBEQjRxqFwoTCJD_6eObzIsDFQAAAAAdAAAAABAE"},
            new Campsite {Id= 5, CampsiteTypeId = 5, NickName = "Gopher Holes", ImageUrl ="https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.istockphoto.com%2Fphotos%2Fpocket-gopher&psig=AOvVaw0i2tQHTkMM9E0olWjMBVle&ust=1739933587658000&source=images&cd=vfe&opi=89978449&ved=0CBEQjRxqFwoTCLib2u2bzIsDFQAAAAAdAAAAABAE"},
            new Campsite {Id= 6, CampsiteTypeId = 6, NickName = "Squirrel Nuts", ImageUrl ="https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.istockphoto.com%2Fphotos%2Fsquirrel&psig=AOvVaw0pLOWid2fY6og6auB7V7kT&ust=1739933609877000&source=images&cd=vfe&opi=89978449&ved=0CBEQjRxqFwoTCJCou_ybzIsDFQAAAAAdAAAAABAE"},

        });

     modelBuilder.Entity<UserProfile>().HasData(new UserProfile[]
     {
        new UserProfile {Id = 1, FirstName = "Raph", LastName = "Turtle", Email = "ninjaturlered@gmail.com" }
     });

    modelBuilder.Entity<Reservation>().HasData(new Reservation[]
    {
        new Reservation {Id = 1, CampsiteId = 2, UserProfileId = 1, CheckinDate = DateTime.Today, CheckoutDate = DateTime.MaxValue}
    });


    }
}
